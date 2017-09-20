//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpOpenSimpleServer
{
        internal partial class Asynchronous
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[1];
                names[0].Set(@"(int,int,System.Func<AutoCSer.Net.TcpServer.ReturnValue<int>,bool>)Add", 0);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpOpenSimpleServer.Asynchronous TCP服务
            /// </summary>
            public sealed class TcpOpenSimpleServer : AutoCSer.Net.TcpOpenSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpOpenSimpleServer.Asynchronous Value;
                /// <summary>
                /// AutoCSer.Example.TcpOpenSimpleServer.Asynchronous TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleServer(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpOpenSimpleServer.Asynchronous", typeof(AutoCSer.Example.TcpOpenSimpleServer.Asynchronous))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpOpenSimpleServer.Asynchronous();
                    setCommandData(1);
                    setCommand(0);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="socket">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                /// <returns>是否成功</returns>
                public override bool DoCommand(int index, AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p2 outputParameter = new _p2();
                                    
                                    Value.Add(inputParameter.left, inputParameter.right,  socket.GetCallback<_p2, int>(_c0, ref outputParameter));
                                    return true;
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int left;
                    public int right;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public int Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public int Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (int)value; }
                    }
#endif
                }
            }
        }
}namespace AutoCSer.Example.TcpOpenSimpleServer
{
        internal partial class Field
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[3];
                names[0].Set(@"()get_GetField", 0);
                names[1].Set(@"()get_SetField", 1);
                names[2].Set(@"(int)set_SetField", 2);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpOpenSimpleServer.Field TCP服务
            /// </summary>
            public sealed class TcpOpenSimpleServer : AutoCSer.Net.TcpOpenSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpOpenSimpleServer.Field Value;
                /// <summary>
                /// AutoCSer.Example.TcpOpenSimpleServer.Field TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleServer(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpOpenSimpleServer.Field", typeof(AutoCSer.Example.TcpOpenSimpleServer.Field))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpOpenSimpleServer.Field();
                    setCommandData(3);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="socket">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                /// <returns>是否成功</returns>
                public override bool DoCommand(int index, AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.GetField;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.SetField;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c1, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.SetField = inputParameter.value;
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public int Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public int Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (int)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public int value;
                }
            }
        }
}namespace AutoCSer.Example.TcpOpenSimpleServer
{
        internal partial class NoAttribute
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[1];
                names[0].Set(@"(int,int)Add", 0);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpOpenSimpleServer.NoAttribute TCP服务
            /// </summary>
            public sealed class TcpOpenSimpleServer : AutoCSer.Net.TcpOpenSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpOpenSimpleServer.NoAttribute Value;
                /// <summary>
                /// AutoCSer.Example.TcpOpenSimpleServer.NoAttribute TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleServer(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpOpenSimpleServer.NoAttribute", typeof(AutoCSer.Example.TcpOpenSimpleServer.NoAttribute))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpOpenSimpleServer.NoAttribute();
                    setCommandData(1);
                    setCommand(0);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="socket">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                /// <returns>是否成功</returns>
                public override bool DoCommand(int index, AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    int Return;
                                    
                                    Return = Value.Add(inputParameter.left, inputParameter.right);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int left;
                    public int right;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public int Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public int Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (int)value; }
                    }
#endif
                }
            }
        }
}namespace AutoCSer.Example.TcpOpenSimpleServer
{
        internal partial class Property
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[5];
                names[0].Set(@"()get_GetProperty", 0);
                names[1].Set(@"(int)get_Item", 1);
                names[2].Set(@"(int,int)set_Item", 2);
                names[3].Set(@"()get_SetProperty", 3);
                names[4].Set(@"(int)set_SetProperty", 4);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpOpenSimpleServer.Property TCP服务
            /// </summary>
            public sealed class TcpOpenSimpleServer : AutoCSer.Net.TcpOpenSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpOpenSimpleServer.Property Value;
                /// <summary>
                /// AutoCSer.Example.TcpOpenSimpleServer.Property TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleServer(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpOpenSimpleServer.Property", typeof(AutoCSer.Example.TcpOpenSimpleServer.Property))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpOpenSimpleServer.Property();
                    setCommandData(5);
                    setCommand(0);
                    setCommand(1);
                    setCommand(2);
                    setCommand(3);
                    setCommand(4);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="socket">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                /// <returns>是否成功</returns>
                public override bool DoCommand(int index, AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.GetProperty;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 1:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p2 inputParameter = new _p2();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value[inputParameter.index];
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c1, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 2:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p3 inputParameter = new _p3();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value[inputParameter.index] = inputParameter.value;
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        case 3:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                {
                                    _p1 _outputParameter_ = new _p1();
                                    
                                    int Return;
                                    Return = Value.SetProperty;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c3, ref _outputParameter_);
                                }
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        case 4:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p4 inputParameter = new _p4();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    
                                    Value.SetProperty = inputParameter.value;
                                    return socket.Send();
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.Send(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c1 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c2 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c3 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 1 };
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c4 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 0 };

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public int Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public int Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (int)value; }
                    }
#endif
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
                {
                    public int index;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p3
                {
                    public int index;
                    public int value;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p4
                {
                    public int value;
                }
            }
        }
}namespace AutoCSer.Example.TcpOpenSimpleServer
{
        internal partial class RefOut
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[1];
                names[0].Set(@"(int,ref int,out int)Add", 0);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpOpenSimpleServer.RefOut TCP服务
            /// </summary>
            public sealed class TcpOpenSimpleServer : AutoCSer.Net.TcpOpenSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpOpenSimpleServer.RefOut Value;
                /// <summary>
                /// AutoCSer.Example.TcpOpenSimpleServer.RefOut TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleServer(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpOpenSimpleServer.RefOut", typeof(AutoCSer.Example.TcpOpenSimpleServer.RefOut))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpOpenSimpleServer.RefOut();
                    setCommandData(1);
                    setCommand(0);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="socket">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                /// <returns>是否成功</returns>
                public override bool DoCommand(int index, AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    AutoCSer.Net.TcpServer.ReturnValue<int> Return;
                                    
                                    Return = Value.Add(inputParameter.left, ref inputParameter.right, out _outputParameter_.product);
                                    
                                    _outputParameter_.right = inputParameter.right;
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int left;
                    public int product;
                    public int right;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.Net.TcpServer.ReturnValue<int>>
#endif
                {
                    public int product;
                    public int right;
                    [AutoCSer.Json.IgnoreMember]
                    public AutoCSer.Net.TcpServer.ReturnValue<int> Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public AutoCSer.Net.TcpServer.ReturnValue<int> Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (AutoCSer.Net.TcpServer.ReturnValue<int>)value; }
                    }
#endif
                }
            }
        }
}namespace AutoCSer.Example.TcpOpenSimpleServer
{
        internal partial class Static
        {
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            private static KeyValue<string, int>[] _identityCommandNames_()
            {
                KeyValue<string, int>[] names = new KeyValue<string, int>[1];
                names[0].Set(@"(int,int)Add", 0);
                return names;
            }
            /// <summary>
            /// AutoCSer.Example.TcpOpenSimpleServer.Static TCP服务
            /// </summary>
            public sealed class TcpOpenSimpleServer : AutoCSer.Net.TcpOpenSimpleServer.Server
            {
                public readonly AutoCSer.Example.TcpOpenSimpleServer.Static Value;
                /// <summary>
                /// AutoCSer.Example.TcpOpenSimpleServer.Static TCP调用服务端
                /// </summary>
                /// <param name="attribute">TCP调用服务器端配置信息</param>
                /// <param name="log">日志接口</param>
                public TcpOpenSimpleServer(AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Log.ILog log = null)
                    : base(attribute ?? (attribute = AutoCSer.Net.TcpOpenSimpleServer.ServerAttribute.GetConfig("AutoCSer.Example.TcpOpenSimpleServer.Static", typeof(AutoCSer.Example.TcpOpenSimpleServer.Static))), verify, log, false)
                {
                    Value =new AutoCSer.Example.TcpOpenSimpleServer.Static();
                    setCommandData(1);
                    setCommand(0);
                    if (attribute.IsAutoServer) Start();
                }
                /// <summary>
                /// 命令处理
                /// </summary>
                /// <param name="index">命令序号</param>
                /// <param name="socket">TCP 内部服务套接字数据发送</param>
                /// <param name="data">命令数据</param>
                /// <returns>是否成功</returns>
                public override bool DoCommand(int index, AutoCSer.Net.TcpOpenSimpleServer.ServerSocket socket, ref SubArray<byte> data)
                {
                    AutoCSer.Net.TcpServer.ReturnType returnType;
                    switch (index - 128)
                    {
                        case 0:
                            returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
                            try
                            {
                                _p1 inputParameter = new _p1();
                                if (socket.DeSerialize(ref data, ref inputParameter))
                                {
                                    _p2 _outputParameter_ = new _p2();
                                    
                                    int Return;
                                    
                                    Return = Value.Add(inputParameter.left, inputParameter.right);
                                    _outputParameter_.Return = Return;
                                    return socket.Send(_c0, ref _outputParameter_);
                                }
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
                            }
                            catch (Exception error)
                            {
                                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                                socket.Log(error);
                            }
                            return socket.SendOutput(returnType);
                        default: return false;
                    }
                }
                private static readonly AutoCSer.Net.TcpSimpleServer.OutputInfo _c0 = new AutoCSer.Net.TcpSimpleServer.OutputInfo { OutputParameterIndex = 2 };

                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p1
                {
                    public int left;
                    public int right;
                }
                [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
                [AutoCSer.Metadata.BoxSerialize]
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
                {
                    [AutoCSer.Json.IgnoreMember]
                    public int Ret;
                    [AutoCSer.IOS.Preserve(Conditional = true)]
                    public int Return
                    {
                        get { return Ret; }
                        set { Ret = value; }
                    }
#if NOJIT
                    [AutoCSer.Metadata.Ignore]
                    public object ReturnObject
                    {
                        get { return Ret; }
                        set { Ret = (int)value; }
                    }
#endif
                }
            }
        }
}
#endif