/*
* FileName:          ListExtension
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
    public static class ListExtension
    {

        public static List<T> DeepCopyJson<T>(this List<T> obj)
        {
            // 序列化
            string json = JsonHelper.ToJson(obj);
            // 反序列化
            return JsonHelper.ToObject<List<T>>(json);
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> DeepCopyBinary<T>(this List<T> list)
        {
            object rel;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, list);
                ms.Seek(0, SeekOrigin.Begin);
                rel = bf.Deserialize(ms);
                ms.Close();
            }
            return (List<T>)rel;
        }

        /// <summary>
        /// 添加指定的值,如果不存在就添加,存在不处理
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<TValue> TryAdd<TValue>(this List<TValue> list, TValue value)
        {
            if (list.Contains(value) == false) list.Add(value);
            return list;
        }

        /// <summary>
        ///  对比两个list 大小 元素是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool CompareEqual<T>(this List<T> list, List<T> other) where T : List<T>
        {
            if (list == null || other == null || list.Count != other.Count)
            {
                return false;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != other[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static void ForEach<TValue>(this List<TValue> list, Action<TValue> action)
        {
            foreach (var item in list) { action(item); }
        }
    }
}