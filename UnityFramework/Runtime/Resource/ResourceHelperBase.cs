/*
* FileName:          ResourceHelperBase
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public abstract class ResourceHelperBase : MonoBehaviour, IResourceHelper
    {
        [SerializeField]
        private ResourceMode m_ResourceMode;
        public ResourceMode ResourceMode => m_ResourceMode;


        public abstract void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData);

        public abstract void UnloadAsset(object asset);

        public abstract void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData);

        public abstract void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData);

        public abstract void Shutdown();
    }
}
