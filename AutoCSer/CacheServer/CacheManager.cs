﻿using System;
using System.Collections.Generic;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    internal sealed unsafe partial class CacheManager
    {
        /// <summary>
        /// 缓存从服务
        /// </summary>
        internal readonly SlaveServer SlaveServer;
        /// <summary>
        /// 缓存服务配置
        /// </summary>
        internal readonly ServerConfig Config;
        /// <summary>
        /// 缓存主服务配置
        /// </summary>
        internal MasterServerConfig MasterConfig
        {
            get { return new UnionType { Value = Config }.MasterServerConfig; }
        }
        /// <summary>
        /// 缓存服务
        /// </summary>
        internal readonly AutoCSer.Net.TcpInternalServer.Server TcpServer;
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        private readonly AutoCSer.Net.TcpServer.ServerCallCanDisposableQueue tcpQueue;
        /// <summary>
        /// 查询数据缓冲区
        /// </summary>
        private readonly Buffer queryBuffer = new Buffer();
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        private readonly Buffer loadBuffer;
        /// <summary>
        /// 释放资源
        /// </summary>
        private readonly Action disposeHandle;
        /// <summary>
        /// 文件流写入器
        /// </summary>
        internal FileStreamWriter File;
        /// <summary>
        /// 当前创建中的文件流写入器
        /// </summary>
        internal FileStreamWriter NewFile;
        /// <summary>
        /// 获取缓存数据链表
        /// </summary>
        private CacheGetter getterLink;
        /// <summary>
        /// 获取缓存数据链表
        /// </summary>
        private CacheGetter waitGetterLink;
        /// <summary>
        /// 当前获取缓存数据
        /// </summary>
        private CacheGetter currentGetter;
        /// <summary>
        /// 数据结构定义集合
        /// </summary>
        internal readonly Dictionary<HashString, ServerDataStructure> DataStructures = DictionaryCreator<HashString>.Create<ServerDataStructure>();
        /// <summary>
        /// 数据结构数组集合
        /// </summary>
        internal DataStructureItem[] Array = NullValue<DataStructureItem>.Array;
        /// <summary>
        /// 数据结构空闲索引
        /// </summary>
        internal LeftArray<int> FreeIndexs;
        /// <summary>
        /// 加载中的消息队列集合
        /// </summary>
        internal LeftArray<Cache.MessageQueue.Node> LoadMessageQueues;
        /// <summary>
        /// 创建缓存节点后的回调操作
        /// </summary>
        internal Action<IndexIdentity> OnDataStructureCreated;
        /// <summary>
        /// 是否允许写操作
        /// </summary>
        internal bool CanWrite;
        /// <summary>
        /// 是否重建中
        /// </summary>
        private bool isRebuild;
        /// <summary>
        /// 是否已经加载完成
        /// </summary>
        internal bool IsLoaded;
        /// <summary>
        /// 是否写文件持久化
        /// </summary>
        internal bool IsFile;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 缓存管理
        /// </summary>
        /// <param name="config">缓存服务配置</param>
        /// <param name="tcpServer">缓存服务</param>
        internal CacheManager(MasterServerConfig config, AutoCSer.Net.TcpInternalServer.Server tcpServer)
        {
            Config = config;
            TcpServer = tcpServer;
            tcpQueue = tcpServer.CallQueue;
            if (config.IsFile)
            {
                disposeHandle = Dispose;
                IsFile = true;
                File = new FileStreamWriter(this, config);
                foreach (Cache.MessageQueue.Node messageQueue in LoadMessageQueues) messageQueue.Start();
                LoadMessageQueues.SetNull();
                IsLoaded = CanWrite = true;
                AutoCSer.DomainUnload.Unloader.Add(disposeHandle, DomainUnload.Type.Action);
            }
            else IsLoaded = CanWrite = true;
        }
        /// <summary>
        /// 缓存管理
        /// </summary>
        /// <param name="slaveServer">缓存从服务</param>
        /// <param name="tcpServer">缓存服务</param>
        internal CacheManager(SlaveServer slaveServer, AutoCSer.Net.TcpInternalServer.Server tcpServer)
        {
            SlaveServer = slaveServer;
            TcpServer = tcpServer;
            Config = slaveServer.Config;
            CanWrite = false;
            loadBuffer = new Buffer();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                //timeout.Free();
                if (File != null) AutoCSer.DomainUnload.Unloader.Remove(disposeHandle, DomainUnload.Type.Action, false);
                tcpQueue.Add(new ServerCall.CacheManager(this, ServerCall.CacheManagerServerCallType.Dispose));
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal void DisposeTcpQueue()
        {
            CanWrite = false;
            if (File != null)
            {
                File.Close();
                File = null;
            }
            CacheGetter getter = waitGetterLink;
            waitGetterLink = null;
            while (getter != null) getter = getter.Cancel();
            getter = getterLink;
            getterLink = currentGetter = null;
            while (getter != null) getter = getter.Cancel();
        }
        /// <summary>
        /// 重建文件流
        /// </summary>
        /// <returns></returns>
        internal ReturnType NewFileStream()
        {
            if (isDisposed == 0)
            {
                if (File != null)
                {
                    if (NewFile == null)
                    {
                        NewFile = new FileStreamWriter(File);
                        return currentGetter == null ? NewFile.Start(File) : ReturnType.Success;
                    }
                    return ReturnType.FileStreamExists;
                }
                return ReturnType.NotFoundFileStream;
            }
            return ReturnType.Disposed;
        }
        /// <summary>
        /// 重建文件流失败
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CreateNewFileStreamError()
        {
            Buffer.DisposeLink(NewFile.BufferLink.GetClear());
            NewFile = null;
        }
        /// <summary>
        /// 重建文件流
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnCreatedNewFileStream()
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(File.Close);
            File = NewFile;
            NewFile = null;
        }
        /// <summary>
        /// 下一个获取缓存数据
        /// </summary>
        internal void NextGetter()
        {
            if (NewFile == null)
            {
                if ((currentGetter = waitGetterLink) != null)
                {
                    waitGetterLink = currentGetter.LinkNext;
                    currentGetter.Start();
                    Append(currentGetter);
                }
            }
            else
            {
                currentGetter = null;
                NewFile.Start(File);
            }
        }
        /// <summary>
        /// 添加获取缓存数据
        /// </summary>
        /// <param name="getter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Append(CacheGetter getter)
        {
            getter.LinkNext = getterLink;
            getterLink = getter;
        }
        /// <summary>
        /// 添加获取缓存数据
        /// </summary>
        /// <param name="getter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AppendWait(CacheGetter getter)
        {
            if (IsLoaded && currentGetter == null)
            {
                currentGetter = getter;
                getter.Start();
                Append(getter);
            }
            else
            {
                getter.LinkNext = waitGetterLink;
                waitGetterLink = getter;
            }
        }

        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteFile()
        {
            if (File != null) File.Write();
        }

        /// <summary>
        /// 加载操作数据
        /// </summary>
        /// <param name="loadData">加载数据</param>
        /// <returns>是否加载成功</returns>
        internal bool Load(ref LoadData loadData)
        {
            OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, ref loadData);
            do
            {
                if (parser.Load(ref loadData))
                {
                    switch (parser.OperationType)
                    {
                        case OperationParameter.OperationType.LoadArraySize:
                            int size = parser.ReadInt();
                            if (parser.IsEnd)
                            {
                                Array = new DataStructureItem[size];
                                isRebuild = true;
                                break;
                            }
                            return false;
                        case OperationParameter.OperationType.GetOrCreateDataStructure:
                            if(loadDataStructure(loadData.Buffer)) break;
                            return false;
                        case OperationParameter.OperationType.RemoveDataStructure:
                            if(loadRemoveDataStructure(loadData.Buffer)) break;
                            return false;
                        case OperationParameter.OperationType.LoadIndexIdentity:
                            if (loadIndexIdentity(ref parser)) break;
                            return false;
                        default:
                            ServerDataStructure dataStructure = parser.Get(Array);
                            if (dataStructure != null) dataStructure.Node.Operation(ref parser);
                            break;
                    }
                }
                else return false;
            }
            while (loadData.IsNext);
            return true;
        }
        /// <summary>
        /// 添加数据结构定义
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns></returns>
        private bool loadDataStructure(Buffer buffer)
        {
            DataStructureBuffer dataStructureBuffer = new DataStructureBuffer(buffer);
            ServerDataStructure dataStructure = new ServerDataStructure(this, buffer, ref dataStructureBuffer);
            int index = dataStructureBuffer.Identity.Index;
            if (loadBuffer == null)
            {
                if (!isRebuild && index != getFreeIndex()) return false;
            }
            else if (index >= Array.Length) Array = Array.copyNew(Math.Max(Array.Length << 1, Math.Max(sizeof(int), index + 1)));
            DataStructures.Add(dataStructureBuffer.CacheName, dataStructure);
            Array[index].Load(dataStructure);
            return true;
        }
        /// <summary>
        /// 删除数据结构定义
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns></returns>
        private bool loadRemoveDataStructure(Buffer buffer)
        {
            DataStructureBuffer dataStructureBuffer = new DataStructureBuffer(buffer);
            HashString name = dataStructureBuffer.CacheName;
            ServerDataStructure dataStructure;
            if (DataStructures.TryGetValue(name, out dataStructure))
            {
                int index = dataStructure.Identity.Index;
                if (dataStructureBuffer.Identity.Index == index)
                {
                    if (!isRebuild && loadBuffer == null) FreeIndexs.Add(index);
                    Array[index].LoadFree(dataStructureBuffer.Identity.Identity);
                    DataStructures.Remove(name);
                    dataStructure.Node.OnRemoved();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 加载数据结构索引标识
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        private bool loadIndexIdentity(ref OperationParameter.NodeParser parser)
        {
            if (parser.ReadInt() == Array.Length)
            {
                FreeIndexs.PrepLength(Array.Length);
                ServerDataStructure dataStructure;
                for (int index = Array.Length; index != 0; )
                {
                    if (!Array[--index].Load((ulong)parser.ReadLong(), out dataStructure)) return false;
                    if (dataStructure == null) FreeIndexs.UnsafeAdd(index);
                }
                isRebuild = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取空闲索引
        /// </summary>
        /// <returns></returns>
        private int getFreeIndex()
        {
            if (FreeIndexs.Length != 0) return FreeIndexs.UnsafePopOnly();
            int index = Array.Length;
            Array = Array.copyNew(Math.Max(index << 1, sizeof(int)));
            FreeIndexs.PrepLength(Array.Length - index);
            for (int freeIndex = Array.Length - 1; freeIndex != index; FreeIndexs.UnsafeAdd(freeIndex--)) ;
            return index;
        }
        /// <summary>
        /// 数据操作完以后的持久化处理
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        private void onOperation(Buffer buffer)
        {
            if (File != null)
            {
                buffer.Reference();
                File.BufferLink.Push(buffer);
                if (NewFile != null)
                {
                    buffer.Reference();
                    NewFile.BufferLink.Push(buffer);
                }
            }
            CacheGetter getter = getterLink;
            if (getter != null)
            {
                CacheGetter father = null;
                do
                {
                    if (getter.Append(buffer))
                    {
                        father = getter;
                        getter = getter.LinkNext;
                    }
                    else
                    {
                        if (getter == currentGetter) NextGetter();
                        getter = getter.LinkNext;
                        if (father == null) getterLink = getter;
                        else father.LinkNext = getter;
                    }
                }
                while (getter != null);
            }
            buffer.FreeReference();
        }
        /// <summary>
        /// 添加数据结构定义
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns>数据结构索引标识</returns>
        internal IndexIdentity GetOrCreateDataStructure(Buffer buffer)
        {
            if (CanWrite)
            {
                DataStructureBuffer dataStructureBuffer = new DataStructureBuffer(buffer);
                HashString name = dataStructureBuffer.CacheName;
                ServerDataStructure dataStructure;
                if (DataStructures.TryGetValue(name, out dataStructure))
                {
                    if (dataStructure.IsNodeData(ref dataStructureBuffer.Data))
                    {
                        buffer.FreeReference();
                        return dataStructure.Identity;
                    }
                    buffer.FreeReference();
                    return new IndexIdentity { ReturnType = ReturnType.DataStructureNameExists };
                }
                OnDataStructureCreated = onDataStructureCreated;
                if ((dataStructure = new ServerDataStructure(this, buffer, ref dataStructureBuffer)).Node != null)
                {
                    int index = getFreeIndex();
                    DataStructures.Add(name, dataStructure);
                    Array[index].Set(index, dataStructure);
                    buffer.SetIdentity(ref dataStructure.Identity);
                    onOperation(buffer);
                    OnDataStructureCreated(dataStructure.Identity);
                    OnDataStructureCreated = onDataStructureCreated;
                    return dataStructure.Identity;
                }
                buffer.FreeReference();
                return new IndexIdentity { ReturnType = dataStructure.ReturnType };
            }
            buffer.FreeReference();
            return new IndexIdentity { ReturnType = ReturnType.CanNotWrite };
        }
        /// <summary>
        /// 删除数据结构定义
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns>被删除数据结构索引标识</returns>
        internal IndexIdentity RemoveDataStructure(Buffer buffer)
        {
            if (CanWrite)
            {
                DataStructureBuffer dataStructureBuffer = new DataStructureBuffer(buffer);
                HashString name = dataStructureBuffer.CacheName;
                ServerDataStructure dataStructure;
                if (DataStructures.TryGetValue(name, out dataStructure))
                {
                    int index = dataStructure.Identity.Index;
                    FreeIndexs.Add(index);
                    buffer.SetIdentity(new IndexIdentity { Index = index, Identity = Array[index].Free() });
                    DataStructures.Remove(name);
                    onOperation(buffer);
                    dataStructure.Node.OnRemoved();
                    return dataStructure.Identity;
                }
                buffer.FreeReference();
                return new IndexIdentity { ReturnType = ReturnType.DataStructureNameNotFound };
            }
            buffer.FreeReference();
            return new IndexIdentity { ReturnType = ReturnType.CanNotWrite };
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns>返回参数</returns>
        internal ReturnParameter Operation(Buffer buffer)
        {
            if (CanWrite)
            {
                fixed (byte* dataFixed = buffer.Array.Array)
                {
                    OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, buffer, dataFixed);
                    ServerDataStructure dataStructure = parser.Get(Array);
                    if (dataStructure != null)
                    {
                        dataStructure.Node.Operation(ref parser);
                        if (parser.IsOperation) onOperation(buffer);
                        else buffer.FreeReference();
                        return parser.ReturnParameter;
                    }
                }
                buffer.FreeReference();
                return new ReturnParameter { Type = ReturnType.DataStructureIdentityError };
            }
            buffer.FreeReference();
            return new ReturnParameter { Type = ReturnType.CanNotWrite };
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="onOperation"></param>
        internal void Operation(Buffer buffer, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onOperation)
        {
            int isReturn = 0;
            try
            {
                if (CanWrite)
                {
                    fixed (byte* dataFixed = buffer.Array.Array)
                    {
                        OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, buffer, dataFixed);
                        ServerDataStructure dataStructure = parser.Get(Array);
                        if (dataStructure != null)
                        {
                            parser.OnReturn = onOperation;
                            try
                            {
                                dataStructure.Node.Operation(ref parser);
                            }
                            finally
                            {
                                if (parser.OnReturn == null) isReturn = 1;
                            }
                            if (parser.IsOperation) this.onOperation(buffer);
                            else buffer.FreeReference();
                            return;
                        }
                    }
                    buffer.FreeReference();
                    isReturn = 1;
                    onOperation(new ReturnParameter { Type = ReturnType.DataStructureIdentityError });
                    return;
                }
                buffer.FreeReference();
            }
            finally
            {
                if (isReturn == 0) onOperation(new ReturnParameter { Type = ReturnType.CanNotWrite });
            }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="queryData">查询数据</param>
        /// <returns>返回参数</returns>
        internal ReturnParameter Query(ref SubArray<byte> queryData)
        {
            queryBuffer.Set(ref queryData);
            fixed (byte* dataFixed = queryData.Array)
            {
                OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, queryBuffer, dataFixed);
                ServerDataStructure dataStructure = parser.Get(Array);
                if (dataStructure != null)
                {
                    dataStructure.Node.Query(ref parser);
                    return parser.ReturnParameter;
                }
            }
            return new ReturnParameter { Type = ReturnType.DataStructureIdentityError };
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="queryData">查询数据</param>
        /// <param name="onQuery"></param>
        /// <param name="isDeSerializeStream">是否反序列化网络流，否则需要 Copy 数据</param>
        internal void Query(ref SubArray<byte> queryData, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onQuery, bool isDeSerializeStream)
        {
            try
            {
                queryBuffer.Set(ref queryData);
                fixed (byte* dataFixed = queryData.Array)
                {
                    OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, queryBuffer, dataFixed);
                    ServerDataStructure dataStructure = parser.Get(Array);
                    if (dataStructure != null)
                    {
                        parser.SetOnReturn(onQuery, isDeSerializeStream);
                        onQuery = null;
                        try
                        {
                            dataStructure.Node.Query(ref parser);
                        }
                        finally
                        {
                            parser.CallOnReturn();
                        }
                    }
                }
            }
            finally
            {
                if (onQuery != null) onQuery(new ReturnParameter { Type = ReturnType.DataStructureIdentityError });
            }
        }

        /// <summary>
        /// 获取数据结构索引标识
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns>数据结构索引标识</returns>
        internal IndexIdentity GetDataStructure(Buffer buffer)
        {
            DataStructureBuffer dataStructureBuffer = new DataStructureBuffer(buffer);
            HashString name = dataStructureBuffer.CacheName;
            ServerDataStructure dataStructure;
            if (DataStructures.TryGetValue(name, out dataStructure))
            {
                if (dataStructure.IsNodeData(ref dataStructureBuffer.Data))
                {
                    buffer.FreeReference();
                    return dataStructure.Identity;
                }
                buffer.FreeReference();
                return new IndexIdentity { ReturnType = ReturnType.DataStructureNameExists };
            }
            buffer.FreeReference();
            return new IndexIdentity { ReturnType = ReturnType.DataStructureNameNotFound };
        }
        /// <summary>
        /// 加载缓存数据
        /// </summary>
        /// <param name="parameter"></param>
        internal void Load(AutoCSer.Net.TcpServer.ReturnValue<CacheReturnParameter> parameter)
        {
            if (parameter.Type == AutoCSer.Net.TcpServer.ReturnType.Success) load(ref parameter.Value.LoadData);
        }
        /// <summary>
        /// 加载缓存数据
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe void load(ref SubArray<byte> data)
        {
            if (data.Array != null)
            {
                if (data.Length != 0)
                {
                    LoadData loadData = new LoadData { Buffer = loadBuffer };
                    fixed (byte* dataFixed = data.Array)
                    {
                        loadData.Set(ref data, dataFixed);
                        if (Load(ref loadData)) return;
                    }
                    TcpServer.Log.Add(Log.LogType.Fatal, "缓存数据解析失败");
                }
                else
                {
                    SlaveServer.Cache = this;
                    IsLoaded = true;
                    NextGetter();
                }
            }
        }

        /// <summary>
        /// 创建缓存节点后的回调操作
        /// </summary>
        private static readonly Action<IndexIdentity> onDataStructureCreated = onDataStructureCreatedNull;
        /// <summary>
        /// 创建缓存节点后的回调操作
        /// </summary>
        /// <param name="identity"></param>
        private static void onDataStructureCreatedNull(IndexIdentity identity) { }
    }
}