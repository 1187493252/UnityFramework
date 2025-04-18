/*
* FileName:          ResourceUpdateAllCompleteEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 资源更新全部完成事件。
    /// </summary>
    public sealed class ResourceUpdateAllCompleteEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源更新全部完成事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceUpdateAllCompleteEventArgs).GetHashCode();

        /// <summary>
        /// 初始化资源更新全部完成事件的新实例。
        /// </summary>
        public ResourceUpdateAllCompleteEventArgs()
        {
        }

        /// <summary>
        /// 获取资源更新全部完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 创建资源更新全部完成事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的资源更新全部完成事件。</returns>
        public static ResourceUpdateAllCompleteEventArgs Create(Framework.Resource.ResourceUpdateAllCompleteEventArgs e)
        {
            return ReferencePool.Acquire<ResourceUpdateAllCompleteEventArgs>();
        }

        /// <summary>
        /// 清理资源更新全部完成事件。
        /// </summary>
        public override void Clear()
        {
        }
    }
}
