/*
* FileName:          RandomExtension
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

    public static class RandomExtension
    {
        /// <summary>
        /// 随机返回区间内一个时间
        /// </summary>
        /// <param name="random"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static DateTime NextDateTime(this System.Random random, DateTime minValue, DateTime maxValue)
        {
            var ticks = minValue.Ticks + (long)((maxValue.Ticks - minValue.Ticks) * random.NextDouble());
            return new DateTime(ticks);
        }
        public static DateTime NextDateTime(this System.Random random)
        {
            return NextDateTime(random, DateTime.MinValue, DateTime.MaxValue);
        }

        /// <summary>
        /// 随机返回 true 或 false
        /// </summary>
        public static bool NextBool(this System.Random random)
        {
            return random.NextDouble() > 0.5;
        }

        /// <summary>
        /// 随机返回一个枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random"></param>
        /// <returns></returns>
        public static T NextEnum<T>(this System.Random random) where T : struct
        {
            Type type = typeof(T);
            if (type.IsEnum == false) throw new InvalidOperationException();

            var array = Enum.GetValues(type);
            var index = random.Next(array.GetLowerBound(0), array.GetUpperBound(0) + 1);
            return (T)array.GetValue(index);
        }

        /// <summary>
        /// 随机返回一个byte 数组
        /// </summary>
        /// <param name="random"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] NextBytes(this System.Random random, int length)
        {
            var data = new byte[length];
            random.NextBytes(data);
            return data;
        }

        public static ushort NextUInt16(this System.Random random)
        {
            return BitConverter.ToUInt16(random.NextBytes(2), 0);
        }

        public static float NextFloat(this System.Random random)
        {
            return BitConverter.ToSingle(random.NextBytes(4), 0);
        }
    }
}