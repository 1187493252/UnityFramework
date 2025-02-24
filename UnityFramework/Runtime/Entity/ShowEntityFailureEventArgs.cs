/*
* FileName:          ShowEntityFailureEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;
using System;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 显示实体失败事件。
    /// </summary>
    public sealed class ShowEntityFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 显示实体失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ShowEntityFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化显示实体失败事件的新实例。
        /// </summary>
        public ShowEntityFailureEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取显示实体失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int EntityId
        {
            get;
            private set;
        }


        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体组名称。
        /// </summary>
        public string EntityGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建显示实体失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的显示实体失败事件。</returns>
        public static ShowEntityFailureEventArgs Create(Framework.Entity.ShowEntityFailureEventArgs e)
        {

            ShowEntityFailureEventArgs showEntityFailureEventArgs = ReferencePool.Acquire<ShowEntityFailureEventArgs>();
            showEntityFailureEventArgs.EntityId = e.EntityId;

            showEntityFailureEventArgs.EntityAssetName = e.EntityAssetName;
            showEntityFailureEventArgs.EntityGroupName = e.EntityGroupName;
            showEntityFailureEventArgs.ErrorMessage = e.ErrorMessage;
            showEntityFailureEventArgs.UserData = e.UserData;
            return showEntityFailureEventArgs;
        }

        /// <summary>
        /// 清理显示实体失败事件。
        /// </summary>
        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
