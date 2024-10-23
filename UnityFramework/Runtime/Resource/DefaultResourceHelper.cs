
using Framework.Resource;
using System;
using System.Collections;
using UnityEngine;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif
using UnityEngine.SceneManagement;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 默认资源辅助器。
    /// </summary>
    public class DefaultResourceHelper : ResourceHelperBase
    {
        /// <summary>
        /// 直接从指定文件路径加载数据流。
        /// </summary>
        /// <param name="fileUri">文件路径。</param>
        /// <param name="loadBytesCallbacks">加载数据流回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void LoadBytes(string fileUri, LoadBytesCallbacks loadBytesCallbacks, object userData)
        {
            StartCoroutine(LoadBytesCo(fileUri, loadBytesCallbacks, userData));
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
#if UNITY_5_5_OR_NEWER
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(UnloadSceneCo(sceneAssetName, unloadSceneCallbacks, userData));
            }
            else
            {
                SceneManager.UnloadSceneAsync((sceneAssetName));
            }
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
        /// 释放资源。
        /// </summary>
        /// <param name="objectToRelease">要释放的资源。</param>
        public override void Release(object objectToRelease)
        {
            AssetBundle assetBundle = objectToRelease as AssetBundle;
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
                return;
            }

        }

        private void Start()
        {
        }

        private IEnumerator LoadBytesCo(string fileUri, LoadBytesCallbacks loadBytesCallbacks, object userData)
        {
            bool isError = false;
            byte[] bytes = null;
            string errorMessage = null;
            DateTime startTime = DateTime.UtcNow;

#if UNITY_5_4_OR_NEWER
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(fileUri);
#if UNITY_2017_2_OR_NEWER
            yield return unityWebRequest.SendWebRequest();
#else
            yield return unityWebRequest.Send();
#endif

#if UNITY_2020_2_OR_NEWER
            isError = unityWebRequest.result != UnityWebRequest.Result.Success;
#elif UNITY_2017_1_OR_NEWER
            isError = unityWebRequest.isNetworkError || unityWebRequest.isHttpError;
#else
            isError = unityWebRequest.isError;
#endif
            bytes = unityWebRequest.downloadHandler.data;
            errorMessage = isError ? unityWebRequest.error : null;
            unityWebRequest.Dispose();
#else
            WWW www = new WWW(fileUri);
            yield return www;

            isError = !string.IsNullOrEmpty(www.error);
            bytes = www.bytes;
            errorMessage = www.error;
            www.Dispose();
#endif

            if (!isError)
            {
                float elapseSeconds = (float)(DateTime.UtcNow - startTime).TotalSeconds;
                loadBytesCallbacks.LoadBytesSuccessCallback(fileUri, bytes, elapseSeconds, userData);
            }
            else if (loadBytesCallbacks.LoadBytesFailureCallback != null)
            {
                loadBytesCallbacks.LoadBytesFailureCallback(fileUri, errorMessage, userData);
            }
        }

        private IEnumerator UnloadSceneCo(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync((sceneAssetName));
            if (asyncOperation == null)
            {
                yield break;
            }

            yield return asyncOperation;

            if (asyncOperation.allowSceneActivation)
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
        }

        public override void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
        }

        public override void UnloadAsset(object asset)
        {
        }

        public override void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData)
        {
        }

        public override void Shutdown()
        {

        }
    }
}
