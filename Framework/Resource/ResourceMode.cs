﻿/*
* FileName:          ResourceMode
* CompanyName:       
* Author:            relly
* Description:       
*/



namespace Framework.Resource
{
    /// <summary>
    /// 资源模式。
    /// </summary>
    public enum ResourceMode : byte
    {
        /// <summary>
        /// 未指定。
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 单机模式。
        /// </summary>
        Package,

        /// <summary>
        /// 预下载的可更新模式。
        /// </summary>
        Updatable,

        /// <summary>
        /// 使用时下载的可更新模式。
        /// </summary>
        UpdatableWhilePlaying
    }
}
