/*
* FileName:          Extension
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework
{
    public static class Extension
    {



        /// <summary>
        /// while 拓展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="predicate">条件</param>
        /// <param name="actions">执行操作</param>
        public static void While<T>(this T t, Predicate<T> predicate, params Action<T>[] actions) where T : class
        {
            while (predicate(t))
            {
                foreach (var action in actions)
                    action?.Invoke(t);
            }
        }

        /// <summary>
        /// Switch 拓展
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="input">要匹配的参数</param>
        /// <param name="inputSource">case 值</param>
        /// <param name="outputSource">case 对应处理</param>
        /// <param name="defaultOutput">default</param>
        /// <returns></returns>
        public static TOutput Switch<TOutput, TInput>(this TInput input, IEnumerable<TInput> inputSource, IEnumerable<TOutput> outputSource, TOutput defaultOutput)
        {
            IEnumerator<TInput> inputIterator = inputSource.GetEnumerator();
            IEnumerator<TOutput> outputIterator = outputSource.GetEnumerator();

            TOutput result = defaultOutput;
            while (inputIterator.MoveNext())
            {
                if (outputIterator.MoveNext())
                {
                    if (input.Equals(inputIterator.Current))
                    {
                        result = outputIterator.Current;
                        break;
                    }
                }
                else break;
            }
            return result;
        }



        /// <summary>
        /// if拓展 string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="predicate"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string If(this string s, Predicate<string> predicate, Func<string, string> func)
        {
            return predicate(s) ? func(s) : s;
        }

        /// <summary>
        /// if拓展 值类型 (int a = -121;  int b = a.If(i => i < 0, i => -i);)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="predicate"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T If<T>(this T t, Predicate<T> predicate, Func<T, T> func) where T : struct
        {
            return predicate(t) ? func(t) : t;
        }

        /// <summary>
        /// if拓展 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="predicate">判断条件</param>
        /// <param name="trueaction">true 处理</param>
        /// <param name="falseaction">false 处理</param>
        /// <returns></returns>
        public static T If<T>(this T t, Predicate<T> predicate, Action<T> trueaction, Action<T> falseaction = null)
        {
            if (t == null) throw new ArgumentNullException();
            if (predicate(t))
            {
                trueaction?.Invoke(t);
            }
            else
            {
                falseaction?.Invoke(t);
            }
            return t;
        }

        public static T If<T>(this T t, Predicate<T> predicate, Action trueaction, Action falseaction = null)
        {
            if (t == null) throw new ArgumentNullException();
            if (predicate(t))
            {
                trueaction?.Invoke();
            }
            else
            {
                falseaction?.Invoke();
            }
            return t;

        }
        /// <summary>
        /// 比较是否在区间内
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <param name="includeLowerBound"></param>
        /// <param name="includeUpperBound"></param>
        /// <returns></returns>
        public static bool IsBetween<T>(this T t, T lowerBound, T upperBound, bool includeLowerBound = false, bool includeUpperBound = false) where T : IComparable<T>
        {
            if (t == null) throw new ArgumentNullException("t");

            var lowerCompareResult = t.CompareTo(lowerBound);
            var upperCompareResult = t.CompareTo(upperBound);

            return (includeLowerBound && lowerCompareResult == 0) ||
                (includeUpperBound && upperCompareResult == 0) ||
                (lowerCompareResult > 0 && upperCompareResult < 0);
        }
    }
}