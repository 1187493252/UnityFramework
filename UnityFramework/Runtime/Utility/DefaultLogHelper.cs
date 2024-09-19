/*
* FileName:          DefaultLogHelper
* CompanyName:  
* Author:            
* Description:       
* 
*/

using Framework;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public class DefaultLogHelper : ILogHelper
    {
        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="message">日志内容。</param>
        public void Log(FrameworkLogLevel level, object message)
        {
            switch (level)
            {
                case FrameworkLogLevel.Debug:
                    Debug.Log(Utility.Text.Format("<color=#FFFFFF>{0}</color>", message.ToString()));
                    break;

                case FrameworkLogLevel.Warning:
                    Debug.LogWarning(Utility.Text.Format("<color=#FFC107>{0}</color>", message.ToString()));
                    break;

                case FrameworkLogLevel.Error:
                    Debug.LogError(Utility.Text.Format("<color=#FF0000>{0}</color>", message.ToString()));
                    break;

                default:
                    throw new System.Exception(message.ToString());
            }
        }
    }
}