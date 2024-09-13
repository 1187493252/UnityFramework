/*
* FileName:          ResourceManager
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityFramework.Runtime;

namespace Framework.Resource
{
    internal sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private const int DefaultPriority = 0;
        private static readonly int AssetsStringLength = "Assets".Length;
        [SerializeField]
        private bool m_EnableCachedAssets = true;

        [SerializeField]
        private int m_LoadAssetCountPerFrame = 100;

        [SerializeField]
        private float m_MinLoadAssetRandomDelaySeconds = 0f;

        [SerializeField]
        private float m_MaxLoadAssetRandomDelaySeconds = 0f;

        private string m_ReadOnlyPath = null;
        private string m_ReadWritePath = null;
        private Dictionary<string, UnityEngine.Object> m_CachedAssets = null;
        private Dictionary<string, object> m_CachedDatas = null;

        private FrameworkLinkedList<LoadAssetInfo> m_LoadAssetInfos = null;
        private FrameworkLinkedList<LoadSceneInfo> m_LoadSceneInfos = null;
        private FrameworkLinkedList<UnloadSceneInfo> m_UnloadSceneInfos = null;
        private FrameworkLinkedList<LoadDataInfo> m_LoadDataInfos = null;
        WebRequestComponent m_UnityWebRequestComponent;

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
                return ResourceMode.Unspecified;
            }
        }
        /// <summary>
        /// 获取等待编辑器加载的资源数量。
        /// </summary>
        public int LoadWaitingAssetCount
        {
            get
            {
                return m_LoadAssetInfos.Count;
            }
        }

        public int AssetCount => throw new NotImplementedException();

        public int ResourceCount => throw new NotImplementedException();

        /// <summary>
        /// 资源应用成功事件。
        /// </summary>
        public event EventHandler<Framework.Resource.ResourceApplySuccessEventArgs> ResourceApplySuccess = null;

        /// <summary>
        /// 资源应用失败事件。
        /// </summary>
        public event EventHandler<Framework.Resource.ResourceApplyFailureEventArgs> ResourceApplyFailure = null;

        /// <summary>
        /// 资源更新开始事件。
        /// </summary>
        public event EventHandler<Framework.Resource.ResourceUpdateStartEventArgs> ResourceUpdateStart = null;

        /// <summary>
        /// 资源更新改变事件。
        /// </summary>
        public event EventHandler<Framework.Resource.ResourceUpdateChangedEventArgs> ResourceUpdateChanged = null;

        /// <summary>
        /// 资源更新成功事件。
        /// </summary>
        public event EventHandler<Framework.Resource.ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess = null;

        /// <summary>
        /// 资源更新失败事件。
        /// </summary>
        public event EventHandler<Framework.Resource.ResourceUpdateFailureEventArgs> ResourceUpdateFailure = null;

        /// <summary>
        /// 资源更新全部完成事件。
        /// </summary>
        public event EventHandler<Framework.Resource.ResourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete = null;

        public ResourceManager()
        {
            m_ReadOnlyPath = null;
            m_ReadWritePath = null;
            m_CachedAssets = new Dictionary<string, UnityEngine.Object>(StringComparer.Ordinal);
            m_LoadAssetInfos = new FrameworkLinkedList<LoadAssetInfo>();
            m_LoadSceneInfos = new FrameworkLinkedList<LoadSceneInfo>();
            m_UnloadSceneInfos = new FrameworkLinkedList<UnloadSceneInfo>();
            m_LoadDataInfos = new FrameworkLinkedList<LoadDataInfo>();
            m_CachedDatas = new Dictionary<string, object>(StringComparer.Ordinal);
            m_UnityWebRequestComponent = UnityFrameworkEntry.GetComponent<WebRequestComponent>();


        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            //加载资源
            if (m_LoadAssetInfos.Count > 0)
            {
                int count = 0;
                LinkedListNode<LoadAssetInfo> current = m_LoadAssetInfos.First;
                while (current != null && count < m_LoadAssetCountPerFrame)
                {
                    LoadAssetInfo loadAssetInfo = current.Value;
                    float elapseSecond = (float)(DateTime.UtcNow - loadAssetInfo.StartTime).TotalSeconds;
                    if (elapseSecond >= loadAssetInfo.DelaySeconds)
                    {
                        UnityEngine.Object asset = GetCachedAsset(loadAssetInfo.AssetName);
                        if (asset == null)
                        {
#if UNITY_EDITOR
                            if (loadAssetInfo.AssetType != null)
                            {
                                asset = UnityEditor.AssetDatabase.LoadAssetAtPath(loadAssetInfo.AssetName, loadAssetInfo.AssetType);
                            }
                            else
                            {
                                asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(loadAssetInfo.AssetName);
                            }

                            if (m_EnableCachedAssets && asset != null)
                            {
                                m_CachedAssets.Add(loadAssetInfo.AssetName, asset);
                            }
#endif


                        }

                        if (asset == null)
                        {
                            asset = Resources.Load(loadAssetInfo.AssetName);
                            if (m_EnableCachedAssets && asset != null)
                            {
                                m_CachedAssets.Add(loadAssetInfo.AssetName, asset);
                            }
                        }

                        if (asset != null)
                        {
                            if (loadAssetInfo.LoadAssetCallbacks.LoadAssetSuccessCallback != null)
                            {
                                loadAssetInfo.LoadAssetCallbacks.LoadAssetSuccessCallback(loadAssetInfo.AssetName, asset, elapseSeconds, loadAssetInfo.UserData);
                            }
                        }
                        else
                        {
                            if (loadAssetInfo.LoadAssetCallbacks.LoadAssetFailureCallback != null)
                            {
                                loadAssetInfo.LoadAssetCallbacks.LoadAssetFailureCallback(loadAssetInfo.AssetName, LoadResourceStatus.AssetError, "Can not load this asset from asset database.", loadAssetInfo.UserData);
                            }
                        }

                        LinkedListNode<LoadAssetInfo> next = current.Next;
                        m_LoadAssetInfos.Remove(loadAssetInfo);
                        current = next;
                        count++;
                    }
                    else
                    {
                        if (loadAssetInfo.LoadAssetCallbacks.LoadAssetUpdateCallback != null)
                        {
                            loadAssetInfo.LoadAssetCallbacks.LoadAssetUpdateCallback(loadAssetInfo.AssetName, elapseSecond / loadAssetInfo.DelaySeconds, loadAssetInfo.UserData);
                        }

                        current = current.Next;
                    }
                }
            }
            //加载场景
            if (m_LoadSceneInfos.Count > 0)
            {
                LinkedListNode<LoadSceneInfo> current = m_LoadSceneInfos.First;
                while (current != null)
                {
                    LoadSceneInfo loadSceneInfo = current.Value;
                    if (loadSceneInfo.AsyncOperation.isDone)
                    {
                        if (loadSceneInfo.AsyncOperation.allowSceneActivation)
                        {
                            if (loadSceneInfo.LoadSceneCallbacks.LoadSceneSuccessCallback != null)
                            {
                                loadSceneInfo.LoadSceneCallbacks.LoadSceneSuccessCallback(loadSceneInfo.SceneAssetName, (float)(DateTime.UtcNow - loadSceneInfo.StartTime).TotalSeconds, loadSceneInfo.UserData);
                            }
                        }
                        else
                        {
                            if (loadSceneInfo.LoadSceneCallbacks.LoadSceneFailureCallback != null)
                            {
                                loadSceneInfo.LoadSceneCallbacks.LoadSceneFailureCallback(loadSceneInfo.SceneAssetName, LoadResourceStatus.AssetError, "Can not load this scene from asset database.", loadSceneInfo.UserData);
                            }
                        }

                        LinkedListNode<LoadSceneInfo> next = current.Next;
                        m_LoadSceneInfos.Remove(loadSceneInfo);
                        current = next;
                    }
                    else
                    {
                        if (loadSceneInfo.LoadSceneCallbacks.LoadSceneUpdateCallback != null)
                        {
                            loadSceneInfo.LoadSceneCallbacks.LoadSceneUpdateCallback(loadSceneInfo.SceneAssetName, loadSceneInfo.AsyncOperation.progress, loadSceneInfo.UserData);
                        }

                        current = current.Next;
                    }
                }
            }

            //卸载场景
            if (m_UnloadSceneInfos.Count > 0)
            {
                LinkedListNode<UnloadSceneInfo> current = m_UnloadSceneInfos.First;
                while (current != null)
                {
                    UnloadSceneInfo unloadSceneInfo = current.Value;
                    if (unloadSceneInfo.AsyncOperation.isDone)
                    {
                        if (unloadSceneInfo.AsyncOperation.allowSceneActivation)
                        {
                            if (unloadSceneInfo.UnloadSceneCallbacks.UnloadSceneSuccessCallback != null)
                            {
                                unloadSceneInfo.UnloadSceneCallbacks.UnloadSceneSuccessCallback(unloadSceneInfo.SceneAssetName, unloadSceneInfo.UserData);
                            }
                        }
                        else
                        {
                            if (unloadSceneInfo.UnloadSceneCallbacks.UnloadSceneFailureCallback != null)
                            {
                                unloadSceneInfo.UnloadSceneCallbacks.UnloadSceneFailureCallback(unloadSceneInfo.SceneAssetName, unloadSceneInfo.UserData);
                            }
                        }

                        LinkedListNode<UnloadSceneInfo> next = current.Next;
                        m_UnloadSceneInfos.Remove(unloadSceneInfo);
                        current = next;
                    }
                    else
                    {
                        current = current.Next;
                    }
                }
            }


        }

        void LoadData()
        {
            foreach (var item in m_LoadDataInfos)
            {
                LoadDataInfo loadDataInfo = item;
                float elapseSecond = (float)(DateTime.UtcNow - loadDataInfo.StartTime).TotalSeconds;
                if (elapseSecond >= loadDataInfo.DelaySeconds)
                {
                    object asset = GetCachedData(loadDataInfo.DataName);

                    if (asset == null)
                    {
                        string dataContent = "";


                        //加载数据
                        string url = ReadOnlyPath + "/Data/" + loadDataInfo.DataName;
                        switch (loadDataInfo.DataType)
                        {
                            case DataType.Json:
                                url += ".json";
                                break;
                            case DataType.Txt:
                                url += ".txt";
                                break;
                            case DataType.Xml:
                                url += ".xml";
                                break;
                            case DataType.Base64:
                                url += ".base";
                                break;
                            case DataType.Binary:
                                url += ".sc";
                                break;
                        }
                        m_UnityWebRequestComponent.RequestGet(url, callBack: delegate (string tmp, byte[] data)
                        {

                            byte[] datas;

                            switch (loadDataInfo.DataType)
                            {
                                case DataType.Base64:
                                    datas = Convert.FromBase64String(tmp);
                                    dataContent = Encoding.Unicode.GetString(datas);
                                    break;
                                case DataType.Binary:
                                    Stream stream = new MemoryStream(data);
                                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                                    datas = (byte[])binaryFormatter.Deserialize(stream);
                                    dataContent = Encoding.Unicode.GetString(datas);
                                    stream.Dispose();
                                    break;
                                default:
                                    dataContent = tmp;
                                    break;
                            }

                            asset = dataContent;
                            if (asset != null)
                            {
                                if (loadDataInfo.LoadDataCallbacks.LoadDataSuccessCallback != null)
                                {
                                    loadDataInfo.LoadDataCallbacks.LoadDataSuccessCallback(loadDataInfo.DataName, asset, 0, loadDataInfo.UserData);
                                }
                            }

                        }, failCallBack: delegate
                        {
                            if (loadDataInfo.LoadDataCallbacks.LoadDataFailureCallback != null)
                            {
                                loadDataInfo.LoadDataCallbacks.LoadDataFailureCallback(loadDataInfo.DataName, LoadResourceStatus.AssetError, "Can not load this asset from asset database.", loadDataInfo.UserData);
                            }

                        });
                    }
                    else
                    {
                        if (loadDataInfo.LoadDataCallbacks.LoadDataSuccessCallback != null)
                        {
                            loadDataInfo.LoadDataCallbacks.LoadDataSuccessCallback(loadDataInfo.DataName, asset, 0, loadDataInfo.UserData);
                        }

                    }

                }
                else
                {
                    if (loadDataInfo.LoadDataCallbacks.LoadDataUpdateCallback != null)
                    {
                        loadDataInfo.LoadDataCallbacks.LoadDataUpdateCallback(loadDataInfo.DataName, elapseSecond / loadDataInfo.DelaySeconds, loadDataInfo.UserData);
                    }

                }
            }
            m_LoadDataInfos.Clear();

        }

        /// <summary>
        /// 设置资源只读区路径。
        /// </summary>
        /// <param name="readOnlyPath">资源只读区路径。</param>
        public void SetReadOnlyPath(string readOnlyPath)
        {
            if (string.IsNullOrEmpty(readOnlyPath))
            {
                Log.Error("Read-only path is invalid.");
                return;
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
                Log.Error("Read-write path is invalid.");
                return;
            }

            m_ReadWritePath = readWritePath;
        }

        /// <summary>
        /// 设置资源模式。
        /// </summary>
        /// <param name="resourceMode">资源模式。</param>
        public void SetResourceMode(ResourceMode resourceMode)
        {
            throw new NotSupportedException("SetResourceMode");
        }


        /// <summary>
        /// 设置资源辅助器。
        /// </summary>
        /// <param name="resourceHelper">资源辅助器。</param>
        public void SetResourceHelper(IResourceHelper resourceHelper)
        {
            throw new NotSupportedException("SetResourceHelper");
        }

        /// <summary>
        /// 增加加载资源代理辅助器。
        /// </summary>
        /// <param name="loadResourceAgentHelper">要增加的加载资源代理辅助器。</param>
        public void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper)
        {
            throw new NotSupportedException("AddLoadResourceAgentHelper");
        }

        /// <summary>
        /// 使用单机模式并初始化资源。
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成时的回调函数。</param>
        public void InitResources(Action initResourcesCompleteCallback)
        {
            throw new NotSupportedException("InitResources");
        }

        /// <summary>
        /// 停止更新资源。
        /// </summary>
        public void StopUpdateResources()
        {
            throw new NotSupportedException("StopUpdateResources");
        }


        /// <summary>
        /// 检查资源是否存在。
        /// </summary>
        /// <param name="assetName">要检查资源的名称。</param>
        /// <returns>检查资源是否存在的结果。</returns>
        public HasAssetResult HasAsset(string assetName)
        {
#if UNITY_EDITOR
            UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetName);
            if (obj == null)
            {
                return HasAssetResult.NotExist;
            }

            HasAssetResult result = obj.GetType() == typeof(UnityEditor.DefaultAsset) ? HasAssetResult.BinaryOnDisk : HasAssetResult.AssetOnDisk;
            obj = null;
            UnityEditor.EditorUtility.UnloadUnusedAssetsImmediate();
            return result;
#else
            return HasAssetResult.NotExist;
#endif
        }




        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks)
        {
            LoadAsset(assetName, null, DefaultPriority, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks)
        {
            LoadAsset(assetName, assetType, DefaultPriority, loadAssetCallbacks, null);
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
            LoadAsset(assetName, null, DefaultPriority, loadAssetCallbacks, userData);
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
            LoadAsset(assetName, assetType, DefaultPriority, loadAssetCallbacks, userData);
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
                Log.Error("Load asset callbacks is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotExist, "Asset name is invalid.", userData);
                }

                return;
            }

            //if (!assetName.StartsWith("Assets/", StringComparison.Ordinal))
            //{
            //    if (loadAssetCallbacks.LoadAssetFailureCallback != null)
            //    {
            //        loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotExist, Utility.Text.Format("Asset name '{0}' is invalid.", assetName), userData);
            //    }

            //    return;
            //}

            //if (!HasFile(assetName))
            //{
            //    if (loadAssetCallbacks.LoadAssetFailureCallback != null)
            //    {
            //        loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotExist, Utility.Text.Format("Asset '{0}' is not exist.", assetName), userData);
            //    }

            //    return;
            //}

            m_LoadAssetInfos.AddLast(new LoadAssetInfo(assetName, assetType, priority, DateTime.UtcNow, m_MinLoadAssetRandomDelaySeconds + (float)Utility.Random.GetRandomDouble() * (m_MaxLoadAssetRandomDelaySeconds - m_MinLoadAssetRandomDelaySeconds), loadAssetCallbacks, userData));
        }


        /// <summary>
        /// 加载数据。
        /// </summary>
        /// <param name="dataName">要加载资源的名称。</param>
        /// <param name="dataType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadDataCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadData(string dataName, DataType dataType, int priority, LoadDataCallbacks loadDataCallbacks, object userData)
        {
            if (loadDataCallbacks == null)
            {
                Log.Error("Load data callbacks is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(dataName))
            {
                if (loadDataCallbacks.LoadDataFailureCallback != null)
                {
                    loadDataCallbacks.LoadDataFailureCallback(dataName, LoadResourceStatus.NotExist, "Data name is invalid.", userData);
                }

                return;
            }


            m_LoadDataInfos.AddLast(new LoadDataInfo(dataName, dataType, priority, DateTime.UtcNow, m_MinLoadAssetRandomDelaySeconds + (float)Utility.Random.GetRandomDouble() * (m_MaxLoadAssetRandomDelaySeconds - m_MinLoadAssetRandomDelaySeconds), loadDataCallbacks, userData));
            LoadData();
        }

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadAsset(object asset)
        {
            // Do nothing in editor resource mode.
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        public void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks)
        {
            LoadScene(sceneAssetName, DefaultPriority, loadSceneCallbacks, null);
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
            LoadScene(sceneAssetName, DefaultPriority, loadSceneCallbacks, userData);
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
                Log.Error("Load scene callbacks is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(sceneAssetName))
            {
                if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                {
                    loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.NotExist, "Scene asset name is invalid.", userData);
                }

                return;
            }

            if (!sceneAssetName.StartsWith("Assets/", StringComparison.Ordinal) || !sceneAssetName.EndsWith(".unity", StringComparison.Ordinal))
            {
                if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                {
                    loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.NotExist, Utility.Text.Format("Scene asset name '{0}' is invalid.", sceneAssetName), userData);
                }

                return;
            }

            if (!HasFile(sceneAssetName))
            {
                if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                {
                    loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.NotExist, Utility.Text.Format("Scene '{0}' is not exist.", sceneAssetName), userData);
                }

                return;
            }

#if UNITY_5_5_OR_NEWER
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneAssetName, LoadSceneMode.Additive);
#else
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneComponent.GetSceneName(sceneAssetName), LoadSceneMode.Additive);
#endif
            if (asyncOperation == null)
            {
                return;
            }

            m_LoadSceneInfos.AddLast(new LoadSceneInfo(asyncOperation, sceneAssetName, priority, DateTime.UtcNow, loadSceneCallbacks, userData));
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
                Log.Error("Scene asset name is invalid.");
                return;
            }

            if (!sceneAssetName.StartsWith("Assets/", StringComparison.Ordinal) || !sceneAssetName.EndsWith(".unity", StringComparison.Ordinal))
            {
                Log.Error("Scene asset name '{0}' is invalid.", sceneAssetName);
                return;
            }

            if (unloadSceneCallbacks == null)
            {
                Log.Error("Unload scene callbacks is invalid.");
                return;
            }

            if (!HasFile(sceneAssetName))
            {
                Log.Error("Scene '{0}' is not exist.", sceneAssetName);
                return;
            }

#if UNITY_5_5_OR_NEWER
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneAssetName);
            if (asyncOperation == null)
            {
                return;
            }

            m_UnloadSceneInfos.AddLast(new UnloadSceneInfo(asyncOperation, sceneAssetName, unloadSceneCallbacks, userData));
#else
            if (SceneManager.UnloadScene(SceneComponent.GetSceneName(sceneAssetName)))
            {
                if (unloadSceneCallbacks.UnloadSceneSuccessCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneSuccessCallback(sceneAssetName, userData);
                }
            }
            else
            {
                if (unloadSceneCallbacks.UnloadSceneFailureCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
                }
            }
#endif
        }

        /// <summary>
        /// 获取二进制资源的实际路径。
        /// </summary>
        /// <param name="binaryAssetName">要获取实际路径的二进制资源的名称。</param>
        /// <returns>二进制资源的实际路径。</returns>
        /// <remarks>此方法仅适用于二进制资源存储在磁盘（而非文件系统）中的情况。若二进制资源存储在文件系统中时，返回值将始终为空。</remarks>
        public string GetBinaryPath(string binaryAssetName)
        {
            if (!HasFile(binaryAssetName))
            {
                return null;
            }

            return Application.dataPath.Substring(0, Application.dataPath.Length - AssetsStringLength) + binaryAssetName;
        }

        /// <summary>
        /// 获取二进制资源的实际路径。
        /// </summary>
        /// <param name="binaryAssetName">要获取实际路径的二进制资源的名称。</param>
        /// <param name="storageInReadOnly">二进制资源是否存储在只读区中。</param>
        /// <param name="storageInFileSystem">二进制资源是否存储在文件系统中。</param>
        /// <param name="relativePath">二进制资源或存储二进制资源的文件系统，相对于只读区或者读写区的相对路径。</param>
        /// <param name="fileName">若二进制资源存储在文件系统中，则指示二进制资源在文件系统中的名称，否则此参数返回空。</param>
        /// <returns>是否获取二进制资源的实际路径成功。</returns>
        public bool GetBinaryPath(string binaryAssetName, out bool storageInReadOnly, out bool storageInFileSystem, out string relativePath, out string fileName)
        {
            throw new NotSupportedException("GetBinaryPath");
        }

        /// <summary>
        /// 获取二进制资源的长度。
        /// </summary>
        /// <param name="binaryAssetName">要获取长度的二进制资源的名称。</param>
        /// <returns>二进制资源的长度。</returns>
        public int GetBinaryLength(string binaryAssetName)
        {
            string binaryPath = GetBinaryPath(binaryAssetName);
            if (string.IsNullOrEmpty(binaryPath))
            {
                return -1;
            }

            return (int)new System.IO.FileInfo(binaryPath).Length;
        }

        /// <summary>
        /// 异步加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="loadBinaryCallbacks">加载二进制资源回调函数集。</param>
        public void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks)
        {
            LoadBinary(binaryAssetName, loadBinaryCallbacks, null);
        }

        /// <summary>
        /// 异步加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="loadBinaryCallbacks">加载二进制资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData)
        {
            if (loadBinaryCallbacks == null)
            {
                Log.Error("Load binary callbacks is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(binaryAssetName))
            {
                if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                {
                    loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.NotExist, "Binary asset name is invalid.", userData);
                }

                return;
            }

            if (!binaryAssetName.StartsWith("Assets/", StringComparison.Ordinal))
            {
                if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                {
                    loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.NotExist, Utility.Text.Format("Binary asset name '{0}' is invalid.", binaryAssetName), userData);
                }

                return;
            }

            string binaryPath = GetBinaryPath(binaryAssetName);
            if (binaryPath == null)
            {
                if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                {
                    loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.NotExist, Utility.Text.Format("Binary asset '{0}' is not exist.", binaryAssetName), userData);
                }

                return;
            }

            try
            {
                byte[] binaryBytes = File.ReadAllBytes(binaryPath);
                loadBinaryCallbacks.LoadBinarySuccessCallback(binaryAssetName, binaryBytes, 0f, userData);
            }
            catch (Exception exception)
            {
                if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                {
                    loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.AssetError, exception.ToString(), userData);
                }
            }
        }

        /// <summary>
        /// 从文件系统中加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <returns>存储加载二进制资源的二进制流。</returns>
        public byte[] LoadBinaryFromFileSystem(string binaryAssetName)
        {
            throw new NotSupportedException("LoadBinaryFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源的二进制流。</param>
        /// <returns>实际加载了多少字节。</returns>
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer)
        {
            throw new NotSupportedException("LoadBinaryFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源的二进制流。</param>
        /// <param name="startIndex">存储加载二进制资源的二进制流的起始位置。</param>
        /// <returns>实际加载了多少字节。</returns>
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex)
        {
            throw new NotSupportedException("LoadBinaryFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源的二进制流。</param>
        /// <param name="startIndex">存储加载二进制资源的二进制流的起始位置。</param>
        /// <param name="length">存储加载二进制资源的二进制流的长度。</param>
        /// <returns>实际加载了多少字节。</returns>
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length)
        {
            throw new NotSupportedException("LoadBinaryFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>存储加载二进制资源片段内容的二进制流。</returns>
        public byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="offset">要加载片段的偏移。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>存储加载二进制资源片段内容的二进制流。</returns>
        public byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <returns>实际加载了多少字节。</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>实际加载了多少字节。</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <param name="startIndex">存储加载二进制资源片段内容的二进制流的起始位置。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>实际加载了多少字节。</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="offset">要加载片段的偏移。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <returns>实际加载了多少字节。</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="offset">要加载片段的偏移。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>实际加载了多少字节。</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="offset">要加载片段的偏移。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <param name="startIndex">存储加载二进制资源片段内容的二进制流的起始位置。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>实际加载了多少字节。</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int startIndex, int length)
        {
            throw new NotSupportedException("LoadBinarySegmentFromFileSystem");
        }


        private bool HasFile(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                return false;
            }

            if (HasCachedAsset(assetName))
            {
                return true;
            }

            string assetFullName = Application.dataPath.Substring(0, Application.dataPath.Length - AssetsStringLength) + assetName;
            if (string.IsNullOrEmpty(assetFullName))
            {
                return false;
            }

            string[] splitedAssetFullName = assetFullName.Split('/');
            string currentPath = Path.GetPathRoot(assetFullName);
            for (int i = 1; i < splitedAssetFullName.Length - 1; i++)
            {
                string[] directoryNames = Directory.GetDirectories(currentPath, splitedAssetFullName[i]);
                if (directoryNames.Length != 1)
                {
                    return false;
                }

                currentPath = directoryNames[0];
            }

            string[] fileNames = Directory.GetFiles(currentPath, splitedAssetFullName[splitedAssetFullName.Length - 1]);
            if (fileNames.Length != 1)
            {
                return false;
            }

            string fileFullName = Utility.Path.GetRegularPath(fileNames[0]);
            if (fileFullName == null)
            {
                return false;
            }

            if (assetFullName != fileFullName)
            {
                if (assetFullName.ToLowerInvariant() == fileFullName.ToLowerInvariant())
                {
                    Log.Warning("The real path of the specific asset '{0}' is '{1}'. Check the case of letters in the path.", assetName, "Assets" + fileFullName.Substring(Application.dataPath.Length));
                }

                return false;
            }

            return true;
        }

        private bool HasCachedAsset(string assetName)
        {
            if (!m_EnableCachedAssets)
            {
                return false;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                return false;
            }

            return m_CachedAssets.ContainsKey(assetName);
        }

        private UnityEngine.Object GetCachedAsset(string assetName)
        {
            if (!m_EnableCachedAssets)
            {
                return null;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                return null;
            }

            UnityEngine.Object asset = null;
            if (m_CachedAssets.TryGetValue(assetName, out asset))
            {
                return asset;
            }

            return null;
        }
        private object GetCachedData(string dataName)
        {
            if (!m_EnableCachedAssets)
            {
                return null;
            }

            if (string.IsNullOrEmpty(dataName))
            {
                return null;
            }

            object data = null;
            if (m_CachedDatas.TryGetValue(dataName, out data))
            {
                return data;
            }

            return null;
        }


        internal override void Shutdown()
        {
            m_CachedAssets.Clear();
            m_LoadAssetInfos.Clear();
            m_LoadSceneInfos.Clear();
            m_UnloadSceneInfos.Clear();
        }

        [StructLayout(LayoutKind.Auto)]
        private struct LoadAssetInfo
        {
            private readonly string m_AssetName;
            private readonly Type m_AssetType;
            private readonly int m_Priority;
            private readonly DateTime m_StartTime;
            private readonly float m_DelaySeconds;
            private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
            private readonly object m_UserData;

            public LoadAssetInfo(string assetName, Type assetType, int priority, DateTime startTime, float delaySeconds, LoadAssetCallbacks loadAssetCallbacks, object userData)
            {
                m_AssetName = assetName;
                m_AssetType = assetType;
                m_Priority = priority;
                m_StartTime = startTime;
                m_DelaySeconds = delaySeconds;
                m_LoadAssetCallbacks = loadAssetCallbacks;
                m_UserData = userData;
            }

            public string AssetName
            {
                get
                {
                    return m_AssetName;
                }
            }

            public Type AssetType
            {
                get
                {
                    return m_AssetType;
                }
            }

            public int Priority
            {
                get
                {
                    return m_Priority;
                }
            }

            public DateTime StartTime
            {
                get
                {
                    return m_StartTime;
                }
            }

            public float DelaySeconds
            {
                get
                {
                    return m_DelaySeconds;
                }
            }

            public LoadAssetCallbacks LoadAssetCallbacks
            {
                get
                {
                    return m_LoadAssetCallbacks;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }
        }

        [StructLayout(LayoutKind.Auto)]
        private struct LoadSceneInfo
        {
            private readonly AsyncOperation m_AsyncOperation;
            private readonly string m_SceneAssetName;
            private readonly int m_Priority;
            private readonly DateTime m_StartTime;
            private readonly LoadSceneCallbacks m_LoadSceneCallbacks;
            private readonly object m_UserData;

            public LoadSceneInfo(AsyncOperation asyncOperation, string sceneAssetName, int priority, DateTime startTime, LoadSceneCallbacks loadSceneCallbacks, object userData)
            {
                m_AsyncOperation = asyncOperation;
                m_SceneAssetName = sceneAssetName;
                m_Priority = priority;
                m_StartTime = startTime;
                m_LoadSceneCallbacks = loadSceneCallbacks;
                m_UserData = userData;
            }

            public AsyncOperation AsyncOperation
            {
                get
                {
                    return m_AsyncOperation;
                }
            }

            public string SceneAssetName
            {
                get
                {
                    return m_SceneAssetName;
                }
            }

            public int Priority
            {
                get
                {
                    return m_Priority;
                }
            }

            public DateTime StartTime
            {
                get
                {
                    return m_StartTime;
                }
            }

            public LoadSceneCallbacks LoadSceneCallbacks
            {
                get
                {
                    return m_LoadSceneCallbacks;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }
        }

        [StructLayout(LayoutKind.Auto)]
        private struct UnloadSceneInfo
        {
            private readonly AsyncOperation m_AsyncOperation;
            private readonly string m_SceneAssetName;
            private readonly UnloadSceneCallbacks m_UnloadSceneCallbacks;
            private readonly object m_UserData;

            public UnloadSceneInfo(AsyncOperation asyncOperation, string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
            {
                m_AsyncOperation = asyncOperation;
                m_SceneAssetName = sceneAssetName;
                m_UnloadSceneCallbacks = unloadSceneCallbacks;
                m_UserData = userData;
            }

            public AsyncOperation AsyncOperation
            {
                get
                {
                    return m_AsyncOperation;
                }
            }

            public string SceneAssetName
            {
                get
                {
                    return m_SceneAssetName;
                }
            }

            public UnloadSceneCallbacks UnloadSceneCallbacks
            {
                get
                {
                    return m_UnloadSceneCallbacks;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }
        }

        [StructLayout(LayoutKind.Auto)]
        private struct LoadDataInfo
        {
            private readonly string m_DataName;

            private readonly DataType m_DataType;
            private readonly int m_Priority;
            private readonly DateTime m_StartTime;
            private readonly float m_DelaySeconds;
            private readonly LoadDataCallbacks m_LoadDataCallbacks;
            private readonly object m_UserData;

            public LoadDataInfo(string dataName, DataType dataType, int priority, DateTime startTime, float delaySeconds, LoadDataCallbacks loadDataCallbacks, object userData)
            {
                m_DataName = dataName;
                m_DataType = dataType;
                m_Priority = priority;
                m_StartTime = startTime;
                m_DelaySeconds = delaySeconds;
                m_LoadDataCallbacks = loadDataCallbacks;
                m_UserData = userData;
            }

            public string DataName
            {
                get
                {
                    return m_DataName;
                }
            }

            public UnityFramework.Runtime.DataType DataType
            {
                get
                {
                    return m_DataType;
                }
            }

            public int Priority
            {
                get
                {
                    return m_Priority;
                }
            }

            public DateTime StartTime
            {
                get
                {
                    return m_StartTime;
                }
            }

            public float DelaySeconds
            {
                get
                {
                    return m_DelaySeconds;
                }
            }

            public LoadDataCallbacks LoadDataCallbacks
            {
                get
                {
                    return m_LoadDataCallbacks;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }
        }
    }
}