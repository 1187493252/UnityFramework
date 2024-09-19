/*
* FileName:          ShutdownType
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 关闭框架类型。
    /// </summary>
    public enum ShutdownType : byte
    {
        /// <summary>
        /// 仅关闭框架。
        /// </summary>
        None = 0,

        /// <summary>
        /// 关闭框架并重启。
        /// </summary>
        Restart,

        /// <summary>
        /// 关闭框架并退出。
        /// </summary>
        Quit,
    }
}