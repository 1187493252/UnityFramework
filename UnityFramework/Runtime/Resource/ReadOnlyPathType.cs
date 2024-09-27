/*
* FileName:          ReadOnlyPathType
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 只读区路径类型。
    /// </summary>
    public enum ReadOnlyPathType : byte
    {
        /// <summary>
        /// 未指定。
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 临时缓存。
        /// </summary>
        TemporaryCache,

        /// <summary>
        /// 持久化数据。
        /// </summary>
        PersistentData,
    }
}
