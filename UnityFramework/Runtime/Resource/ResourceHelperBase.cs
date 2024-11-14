/*
* FileName:          ResourceHelperBase
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Resource;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public abstract class ResourceHelperBase : MonoBehaviour, IResourceHelper
    {
        /// <summary>
        /// 直接从指定文件路径加载数据流。
        /// </summary>
        /// <param name="fileUri">文件路径。</param>
        /// <param name="loadBytesCallbacks">加载数据流回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public abstract void LoadBytes(string fileUri, LoadBytesCallbacks loadBytesCallbacks, object userData);

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public abstract void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData);


        public abstract void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData);
        public abstract void UnloadAsset(object asset);
        public abstract void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData);
        public abstract void Shutdown();
    }
}
