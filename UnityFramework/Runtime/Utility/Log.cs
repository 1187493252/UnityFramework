/*
* FileName:          Log
* CompanyName:  
* Author:            
* Description:       
* 
*/

using Framework;
using System.Diagnostics;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 日志工具集。
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="message">日志内容。</param>
        //// <remarks>仅在带有 ENABLE_LOG、ENABLE_DEBUG_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        public static void Debug(object message)
        {
            FrameworkLog.Debug(message);
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="message">日志内容。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_DEBUG_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        public static void Debug(string message)
        {
            FrameworkLog.Debug(message);
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_DEBUG_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        public static void Debug(string format, object arg0)
        {
            FrameworkLog.Debug(format, arg0);
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_DEBUG_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        public static void Debug(string format, object arg0, object arg1)
        {
            FrameworkLog.Debug(format, arg0, arg1);
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_DEBUG_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        public static void Debug(string format, object arg0, object arg1, object arg2)
        {
            FrameworkLog.Debug(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_DEBUG_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        public static void Debug(string format, params object[] args)
        {
            FrameworkLog.Debug(format, args);
        }


        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_WARNING_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        public static void Warning(object message)
        {
            FrameworkLog.Warning(message);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_WARNING_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        public static void Warning(string message)
        {
            FrameworkLog.Warning(message);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_WARNING_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        public static void Warning(string format, object arg0)
        {
            FrameworkLog.Warning(format, arg0);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_WARNING_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        public static void Warning(string format, object arg0, object arg1)
        {
            FrameworkLog.Warning(format, arg0, arg1);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_WARNING_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        public static void Warning(string format, object arg0, object arg1, object arg2)
        {
            FrameworkLog.Warning(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_WARNING_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        public static void Warning(string format, params object[] args)
        {
            FrameworkLog.Warning(format, args);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_ERROR_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        public static void Error(object message)
        {
            FrameworkLog.Error(message);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_ERROR_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        public static void Error(string message)
        {
            FrameworkLog.Error(message);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_ERROR_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        public static void Error(string format, object arg0)
        {
            FrameworkLog.Error(format, arg0);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_ERROR_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        public static void Error(string format, object arg0, object arg1)
        {
            FrameworkLog.Error(format, arg0, arg1);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_ERROR_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        public static void Error(string format, object arg0, object arg1, object arg2)
        {
            FrameworkLog.Error(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        /// <remarks>仅在带有 ENABLE_LOG、ENABLE_ERROR_LOG 预编译选项时生效。</remarks>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        public static void Error(string format, params object[] args)
        {
            FrameworkLog.Error(format, args);
        }

     


    }
}