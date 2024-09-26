/*
* FileName:          DefaultResourceHelper
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityFramework.Runtime
{

    public class DefaultResourceHelper : ResourceHelperBase
    {

        [SerializeField]
        private bool m_EnableCachedAssets = true;

        [SerializeField]
        private int m_LoadAssetCountPerFrame = 100;

        [SerializeField]
        private float m_MinLoadAssetRandomDelaySeconds = 0f;

        [SerializeField]
        private float m_MaxLoadAssetRandomDelaySeconds = 0f;

        private Dictionary<string, UnityEngine.Object> m_CachedAssets = null;
        private FrameworkLinkedList<LoadAssetInfo> m_LoadAssetInfos = null;
        private FrameworkLinkedList<LoadSceneInfo> m_LoadSceneInfos = null;
        private FrameworkLinkedList<UnloadSceneInfo> m_UnloadSceneInfos = null;

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

        private void Awake()
        {
            m_CachedAssets = new Dictionary<string, UnityEngine.Object>(StringComparer.Ordinal);
            m_LoadAssetInfos = new FrameworkLinkedList<LoadAssetInfo>();
            m_LoadSceneInfos = new FrameworkLinkedList<LoadSceneInfo>();
            m_UnloadSceneInfos = new FrameworkLinkedList<UnloadSceneInfo>();


        }


        private void Update()
        {
            //加载资源
            if (m_LoadAssetInfos.Count > 0)
            {
                int count = 0;
                LinkedListNode<LoadAssetInfo> current = m_LoadAssetInfos.First;
                while (current != null && count < m_LoadAssetCountPerFrame)
                {
                    LoadAssetInfo loadAssetInfo = current.Value;
                    float elapseSeconds = (float)(DateTime.UtcNow - loadAssetInfo.StartTime).TotalSeconds;
                    if (elapseSeconds >= loadAssetInfo.DelaySeconds)
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
                            loadAssetInfo.LoadAssetCallbacks.LoadAssetUpdateCallback(loadAssetInfo.AssetName, elapseSeconds / loadAssetInfo.DelaySeconds, loadAssetInfo.UserData);
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





        public override void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData)
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


            m_LoadAssetInfos.AddLast(new LoadAssetInfo(assetName, assetType, priority, DateTime.UtcNow, m_MinLoadAssetRandomDelaySeconds + (float)Utility.Random.GetRandomDouble() * (m_MaxLoadAssetRandomDelaySeconds - m_MinLoadAssetRandomDelaySeconds), loadAssetCallbacks, userData));
        }




        public override void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData)
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







        public override void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                Log.Error("Scene asset name is invalid.");
                return;
            }

            if (unloadSceneCallbacks == null)
            {
                Log.Error("Unload scene callbacks is invalid.");
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

        public override void UnloadAsset(object asset)
        {

        }

        public override void Shutdown()
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
    }
}
