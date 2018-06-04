﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 字典 数据节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class Dictionary<keyType, valueType> : Node
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 字典
        /// </summary>
        private readonly System.Collections.Generic.Dictionary<HashCodeKey<keyType>, valueType> dictionary = AutoCSer.DictionaryCreator<HashCodeKey<keyType>>.Create<valueType>();
        /// <summary>
        /// 字典 数据节点
        /// </summary>
        /// <param name="parser"></param>
        private Dictionary(ref OperationParameter.NodeParser parser) { }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.SetValue:
                    if (parser.ValueData.Type == ValueData.Data<valueType>.DataType)
                    {
                        valueType value = ValueData.Data<valueType>.GetData(ref parser.ValueData);
                        if (parser.LoadValueData() && parser.IsEnd)
                        {
                            HashCodeKey<keyType> key;
                            if (HashCodeKey<keyType>.Get(ref parser, out key))
                            {
                                dictionary[key] = value;
                                parser.IsOperation = true;
                                parser.ReturnParameter.Set(true);
                                return null;
                            }
                        }
                    }
                    parser.ReturnParameter.Type = ReturnType.ValueDataLoadError;
                    break;
                default: parser.ReturnParameter.Type = ReturnType.OperationTypeError; break;
            }
            return null;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.Remove: remove(ref parser); return;
                case OperationParameter.OperationType.Clear:
                    if (dictionary.Count != 0)
                    {
                        dictionary.Clear();
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.Set(true);
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="parser"></param>
        private void remove(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<keyType> key;
            if (HashCodeKey<keyType>.Get(ref parser, out key))
            {
                if (dictionary.Remove(key))
                {
                    parser.IsOperation = true;
                    parser.ReturnParameter.Set(true);
                }
                else parser.ReturnParameter.Set(false);
            }
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void QueryEnd(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<keyType> key;
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.Set(dictionary.Count); return;
                case OperationParameter.OperationType.GetValue:
                    if (HashCodeKey<keyType>.Get(ref parser, out key))
                    {
                        valueType value;
                        if (dictionary.TryGetValue(key, out value))
                        {
                            ValueData.Data<valueType>.SetData(ref parser.ReturnParameter.Parameter, value);
                            parser.ReturnParameter.Type = ReturnType.Success;
                        }
                        else parser.ReturnParameter.Type = ReturnType.NotFoundDictionaryKey;
                    }
                    return;
                case OperationParameter.OperationType.ContainsKey:
                    if (HashCodeKey<keyType>.Get(ref parser, out key)) parser.ReturnParameter.Set(dictionary.ContainsKey(key));
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
        }

        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal override Snapshot.Node CreateSnapshot()
        {
            KeyValue<keyType, valueType>[] array = new KeyValue<keyType, valueType>[dictionary.Count];
            int index = 0;
            foreach (System.Collections.Generic.KeyValuePair<HashCodeKey<keyType>, valueType> node in dictionary) array[index++].Set(node.Key.Value, node.Value);
            return new Snapshot.Value.Dictionary<keyType, valueType>(array);
        }

#if NOJIT
        /// <summary>
        /// 创建字典 数据节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Dictionary<keyType, valueType> create(ref OperationParameter.NodeParser parser)
        {
            return new Dictionary<keyType, valueType>(ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Dictionary<keyType, valueType>> nodeInfo;
        static Dictionary()
        {
            nodeInfo = new NodeInfo<Dictionary<keyType, valueType>>
            {
#if NOJIT
                Constructor = (Constructor<Dictionary<keyType, valueType>>)Delegate.CreateDelegate(typeof(Constructor<Dictionary<keyType, valueType>>), typeof(Dictionary<keyType, valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Dictionary<keyType, valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(Dictionary<keyType, valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
