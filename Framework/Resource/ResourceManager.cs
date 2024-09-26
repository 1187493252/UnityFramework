/*
* FileName:          ResourceManager
* CompanyName:       
* Author:            relly
* Description:       
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Resource
{
    internal sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        private ResourceMode m_ResourceMode;

        private IResourceHelper m_ResourceHelper;


        public ResourceMode ResourceMode => m_ResourceMode;



        public void SetResourceHelper(IResourceHelper resourceHelper)
        {
            m_ResourceHelper = resourceHelper;
        }

        public void SetResourceMode(ResourceMode resourceMode)
        {
            m_ResourceMode = resourceMode;
        }

        public void InitResources(Action initResourcesCompleteCallback)
        {

        }

        public void StopUpdateResources()
        {
        }

        public void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            m_ResourceHelper.LoadAsset(assetName, assetType, priority, loadAssetCallbacks, userData);
        }


        public void UnloadAsset(object asset)
        {
            m_ResourceHelper.UnloadAsset(asset);
        }


        public void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData)
        {
            m_ResourceHelper.LoadScene(sceneAssetName, priority, loadSceneCallbacks, userData);
        }






        public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            m_ResourceHelper.UnloadScene(sceneAssetName, unloadSceneCallbacks, userData);

        }

        internal override void Shutdown()
        {
            m_ResourceHelper.Shutdown();
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }





    }


}
