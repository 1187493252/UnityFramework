/*
* FileName:          ShowEntityDependencyAssetEventArgs
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
    /// 显示实体时加载依赖资源事件。
    /// </summary>
    public sealed class ShowEntityDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 显示实体时加载依赖资源事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ShowEntityDependencyAssetEventArgs).GetHashCode();

        /// <summary>
        /// 初始化显示实体时加载依赖资源事件的新实例。
        /// </summary>
        public ShowEntityDependencyAssetEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取显示实体时加载依赖资源事件编号。
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
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount
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
        /// 创建显示实体时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的显示实体时加载依赖资源事件。</returns>
        public static ShowEntityDependencyAssetEventArgs Create(Framework.Entity.ShowEntityDependencyAssetEventArgs e)
        {

            ShowEntityDependencyAssetEventArgs showEntityDependencyAssetEventArgs = ReferencePool.Acquire<ShowEntityDependencyAssetEventArgs>();
            showEntityDependencyAssetEventArgs.EntityId = e.EntityId;

            showEntityDependencyAssetEventArgs.EntityAssetName = e.EntityAssetName;
            showEntityDependencyAssetEventArgs.EntityGroupName = e.EntityGroupName;
            showEntityDependencyAssetEventArgs.DependencyAssetName = e.DependencyAssetName;
            showEntityDependencyAssetEventArgs.LoadedCount = e.LoadedCount;
            showEntityDependencyAssetEventArgs.TotalCount = e.TotalCount;
            showEntityDependencyAssetEventArgs.UserData = e.UserData;
            return showEntityDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理显示实体时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
