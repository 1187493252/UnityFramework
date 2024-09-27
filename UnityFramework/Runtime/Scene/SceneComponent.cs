/*
* FileName:          SceneComponent
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 场景组件
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneComponent : UnityFrameworkComponent
    {

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneId">场景id</param>
        public void LoadScene(int sceneId)
        {
            SceneManager.LoadScene(sceneId);
        }

        /// <summary>
        /// 添加场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public Scene AddtiveScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            return SceneManager.GetSceneByName(sceneName);
        }



        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void AsyncLoadScene(string sceneName)
        {
            PlayerPrefs.SetString("NextSceneName", sceneName);
            PlayerPrefs.SetString("SceneSyncLoadType", "Single");


            SceneManager.LoadScene("AsyncLoadScene");
        }

        /// <summary>
        /// 异步添加场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void AsyncAddtiveScene(string sceneName)
        {
            PlayerPrefs.SetString("NextSceneName", sceneName);
            PlayerPrefs.SetString("SceneSyncLoadType", "Additive");
            SceneManager.LoadScene("AsyncLoadScene", LoadSceneMode.Additive);
        }

        /// <summary>
        /// 异步卸载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public AsyncOperation UnloadSceneAsync(string sceneName)
        {
            return SceneManager.UnloadSceneAsync(sceneName);
        }

    }
}
