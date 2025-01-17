/*
* FileName:          ResourceManager
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections.Generic;

namespace Framework.Resource
{
    internal sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {



        private string m_ReadOnlyPath = null;
        private string m_ReadWritePath = null;
        private ResourceMode m_ResourceMode;


        private Dictionary<string, UnityEngine.Object> m_CachedAssets = null;

        private IResourceHelper m_ResourceHelper;

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        public string ReadOnlyPath
        {
            get
            {
                return m_ReadOnlyPath;
            }
        }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        public string ReadWritePath
        {
            get
            {
                return m_ReadWritePath;
            }
        }

        /// <summary>
        /// 获取资源模式。
        /// </summary>
        public ResourceMode ResourceMode
        {
            get
            {
                return m_ResourceMode;
            }
        }
        /// <summary>
        /// 获取等待编辑器加载的资源数量。
        /// </summary>


        public int AssetCount => 0;

        public int ResourceCount => 0;

        private EventHandler<ResourceApplySuccessEventArgs> m_ResourceApplySuccessEventHandler;
        private EventHandler<ResourceApplyFailureEventArgs> m_ResourceApplyFailureEventHandler;
        private EventHandler<ResourceUpdateStartEventArgs> m_ResourceUpdateStartEventHandler;
        private EventHandler<ResourceUpdateChangedEventArgs> m_ResourceUpdateChangedEventHandler;
        private EventHandler<ResourceUpdateSuccessEventArgs> m_ResourceUpdateSuccessEventHandler;
        private EventHandler<ResourceUpdateFailureEventArgs> m_ResourceUpdateFailureEventHandler;
        private EventHandler<ResourceUpdateAllCompleteEventArgs> m_ResourceUpdateAllCompleteEventHandler;


        /// <summary>
        /// 资源应用成功事件。
        /// </summary>
        public event EventHandler<ResourceApplySuccessEventArgs> ResourceApplySuccess
        {
            add
            {
                m_ResourceApplySuccessEventHandler += value;
            }
            remove
            {
                m_ResourceApplySuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 资源应用失败事件。
        /// </summary>
        public event EventHandler<ResourceApplyFailureEventArgs> ResourceApplyFailure
        {
            add
            {
                m_ResourceApplyFailureEventHandler += value;
            }
            remove
            {
                m_ResourceApplyFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 资源更新开始事件。
        /// </summary>
        public event EventHandler<ResourceUpdateStartEventArgs> ResourceUpdateStart
        {
            add
            {
                m_ResourceUpdateStartEventHandler += value;
            }
            remove
            {
                m_ResourceUpdateStartEventHandler -= value;
            }
        }

        /// <summary>
        /// 资源更新改变事件。
        /// </summary>
        public event EventHandler<ResourceUpdateChangedEventArgs> ResourceUpdateChanged
        {
            add
            {
                m_ResourceUpdateChangedEventHandler += value;
            }
            remove
            {
                m_ResourceUpdateChangedEventHandler -= value;
            }
        }

        /// <summary>
        /// 资源更新成功事件。
        /// </summary>
        public event EventHandler<ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess
        {
            add
            {
                m_ResourceUpdateSuccessEventHandler += value;
            }
            remove
            {
                m_ResourceUpdateSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 资源更新失败事件。
        /// </summary>
        public event EventHandler<ResourceUpdateFailureEventArgs> ResourceUpdateFailure
        {
            add
            {
                m_ResourceUpdateFailureEventHandler += value;
            }
            remove
            {
                m_ResourceUpdateFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 资源更新全部完成事件。
        /// </summary>
        public event EventHandler<ResourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete
        {
            add
            {
                m_ResourceUpdateAllCompleteEventHandler += value;
            }
            remove
            {
                m_ResourceUpdateAllCompleteEventHandler -= value;
            }
        }


        public ResourceManager()
        {
            m_ReadOnlyPath = null;
            m_ReadWritePath = null;

        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {




        }



        /// <summary>
        /// 设置资源只读区路径。
        /// </summary>
        /// <param name="readOnlyPath">资源只读区路径。</param>
        public void SetReadOnlyPath(string readOnlyPath)
        {
            if (string.IsNullOrEmpty(readOnlyPath))
            {
                throw new FrameworkException("Read-only path is invalid.");
            }

            m_ReadOnlyPath = readOnlyPath;
        }

        /// <summary>
        /// 设置资源读写区路径。
        /// </summary>
        /// <param name="readWritePath">资源读写区路径。</param>
        public void SetReadWritePath(string readWritePath)
        {
            if (string.IsNullOrEmpty(readWritePath))
            {
                throw new FrameworkException("Read-write path is invalid.");

            }

            m_ReadWritePath = readWritePath;
        }

        /// <summary>
        /// 设置资源模式。
        /// </summary>
        /// <param name="resourceMode">资源模式。</param>
        public void SetResourceMode(ResourceMode resourceMode)
        {
            m_ResourceMode = resourceMode;
        }


        /// <summary>
        /// 设置资源辅助器。
        /// </summary>
        /// <param name="resourceHelper">资源辅助器。</param>
        public void SetResourceHelper(IResourceHelper resourceHelper)
        {
            m_ResourceHelper = resourceHelper;
        }



        /// <summary>
        /// 使用单机模式并初始化资源。
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成时的回调函数。</param>
        public void InitResources(Action initResourcesCompleteCallback)
        {
            initResourcesCompleteCallback?.Invoke();
        }

        /// <summary>
        /// 停止更新资源。
        /// </summary>
        public void StopUpdateResources()
        {
        }







        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks)
        {
            LoadAsset(assetName, null, 0, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks)
        {
            LoadAsset(assetName, assetType, 0, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks)
        {
            LoadAsset(assetName, null, priority, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            LoadAsset(assetName, null, 0, loadAssetCallbacks, userData);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks)
        {
            LoadAsset(assetName, assetType, priority, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            LoadAsset(assetName, assetType, 0, loadAssetCallbacks, userData);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            LoadAsset(assetName, null, priority, loadAssetCallbacks, userData);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            if (loadAssetCallbacks == null)
            {
                throw new FrameworkException("Load asset callbacks is invalid.");

            }

            if (string.IsNullOrEmpty(assetName))
            {
                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotExist, "Asset name is invalid.", userData);
                }

                return;
            }

            m_ResourceHelper.LoadAsset(assetName, assetType, priority, loadAssetCallbacks, userData);
        }




        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadAsset(object asset)
        {
            m_ResourceHelper.UnloadAsset(asset);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        public void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks)
        {
            LoadScene(sceneAssetName, 0, loadSceneCallbacks, null);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        public void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks)
        {
            LoadScene(sceneAssetName, priority, loadSceneCallbacks, null);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks, object userData)
        {
            LoadScene(sceneAssetName, 0, loadSceneCallbacks, userData);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData)
        {
            if (loadSceneCallbacks == null)
            {
                throw new FrameworkException("Load scene callbacks is invalid.");

            }

            if (string.IsNullOrEmpty(sceneAssetName))
            {
                if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                {
                    loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.NotExist, "Scene asset name is invalid.", userData);
                }

                return;
            }





            m_ResourceHelper.LoadScene(sceneAssetName, priority, loadSceneCallbacks, userData);

        }

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks)
        {
            UnloadScene(sceneAssetName, unloadSceneCallbacks, null);
        }

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new FrameworkException("Scene asset name is invalid.");
            }


            if (unloadSceneCallbacks == null)
            {
                throw new FrameworkException("Unload scene callbacks is invalid.");

            }



            m_ResourceHelper.UnloadScene(sceneAssetName, unloadSceneCallbacks, userData);
        }

        /// <summary>
        /// 直接从指定文件路径加载数据流。
        /// </summary>
        /// <param name="fileUri">文件路径。</param>
        /// <param name="loadBinaryCallbacks">加载数据流回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadBinary(string fileUri, LoadBinaryCallbacks loadBinaryCallbacks, object userData)
        {
            if (loadBinaryCallbacks == null)
            {
                throw new FrameworkException("Load bytes callbacks is invalid.");

            }

            if (string.IsNullOrEmpty(fileUri))
            {
                if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                {
                    loadBinaryCallbacks.LoadBinaryFailureCallback(fileUri, LoadResourceStatus.NotExist, "FileUri is invalid.", userData);
                }

                return;
            }

            m_ResourceHelper.LoadBytes(fileUri, loadBinaryCallbacks, userData);
        }

        internal override void Shutdown()
        {
            m_ResourceHelper.Shutdown();
        }
    }
}
