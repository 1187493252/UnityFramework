/*
* FileName:          DictionaryExtension
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace UnityFramework
{
    public static class DictionaryExtension
    {

        public static Dictionary<TKey, TValue> DeepCopyJson<TKey, TValue>(this Dictionary<TKey, TValue> obj)
        {
            // 序列化
            string json = JsonHelper.ToJsonByNewtonsoftJson(obj);
            // 反序列化
            return JsonHelper.ToObjectByNewtonsoftJson<Dictionary<TKey, TValue>>(json);
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> DeepCopyBinary<TKey, TValue>(this Dictionary<TKey, TValue> dic)
        {
            object rel;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, dic);
                ms.Seek(0, SeekOrigin.Begin);
                rel = bf.Deserialize(ms);
                ms.Close();
            }
            return (Dictionary<TKey, TValue>)rel;
        }

        /// <summary>
        /// 向字典中批量添加键值对
        /// </summary>
        /// <param name="replaceExisted">如果已存在，是否替换,默认替换</param>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted = true)
        {
            foreach (var item in values)
            {
                if (!dict.ContainsKey(item.Key) || replaceExisted)
                {
                    dict[item.Key] = item.Value;
                }
            }
            return dict;
        }

        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        /// 根据值返回第一个对应的key
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool TryGetKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value, ref TKey key)
        {
            if (dict.ContainsValue(value))
            {
                key = dict.First(q => q.Value.Equals(value)).Key;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加
        /// </summary>
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }

        public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dic, Action<TKey, TValue> action)
        {
            foreach (var item in dic) { action(item.Key, item.Value); }
        }
    }
}