/*
* FileName:          TaskTarget_LoadScene
* CompanyName:       
* Author:            
* Description:       
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityFramework.Runtime
{

    public class TaskTarget_LoadScene : TaskTargetConfig
    {
        public enum LoadMode
        {
            Async,//异步
            Sync
        }
        public LoadMode loadMode = LoadMode.Sync;


        public string sceneName;
        /// <summary>
        /// 任务初始化
        /// </summary>
        public override void OnTaskInit(TaskConfig taskConfig)
        {
            base.OnTaskInit(taskConfig);
        }

        /// <summary>
        /// 任务开始
        /// </summary>
        public override void OnTaskStart(TaskConfig taskConfig)
        {
            base.OnTaskStart(taskConfig);



            switch (loadMode)
            {
                case LoadMode.Async:
                    PlayerPrefs.SetString("NextSceneName", sceneName);
                    SceneManager.activeSceneChanged += ActiveSceneChanged;

                    ComponentEntry.Scene.AsyncLoadScene(sceneName);
                    break;
                case LoadMode.Sync:
                    ComponentEntry.Scene.LoadScene(sceneName);
                    NotifyResult(true);

                    break;
                default:
                    break;
            }
        }

        public void ActiveSceneChanged(Scene scene, Scene scene1)
        {

            if (scene1.name == sceneName)
            {
                SceneManager.activeSceneChanged -= ActiveSceneChanged;

                NotifyResult(true);

            }

        }
        /// <summary>
        /// 任务持续中
        /// </summary>
        public override void OnTaskDoing(TaskConfig taskConfig)
        {
            base.OnTaskDoing(taskConfig);

        }


        /// <summary>
        /// 任务结束 不论任务的结果正确/出错/跳过
        /// </summary>
        public override void OnTaskEnd(TaskConfig taskConfig)
        {
            base.OnTaskEnd(taskConfig);

        }

        /// <summary>
        /// 强制完成任务
        /// 检测并记录目标是否完成
        /// </summary>
        public override void ForceCompleteTask(TaskConfig taskConf)
        {
            base.ForceCompleteTask(taskConf);
        }

        public override void OnTargetStart()
        {
            base.OnTargetStart();
        }



        /// <summary>
        /// 强制提示,任务未完成等
        /// </summary>
        public override void ForceTip()
        {
            base.ForceTip();
        }






        /// <summary>
        /// 任务开始语音播放完毕
        /// </summary>
        /// <param name="taskConf"></param>
        public override void OnTaskPlayStartAudioComplete(TaskConfig taskConf)
        {
            base.OnTaskPlayStartAudioComplete(taskConf);
        }

        /// <summary>
        /// 任务超时
        /// </summary>
        protected override void OnTaskTimeOut()
        {
            base.OnTaskTimeOut();
        }





        /// <summary>
        /// 发送结果
        /// </summary>
        /// <param name="success">是否成功完成任务目标</param>
        public override void NotifyResult(bool success)
        {
            base.NotifyResult(success);


        }



#if UNITY_EDITOR
        [MenuItem("Assets/UnityFramework/Task/TaskTarget/TaskTarget_LoadScene", false, 0)]
        static void CreateDynamicConf()
        {
            UnityEngine.Object obj = Selection.activeObject;
            if (obj)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ScriptableObject asset = CreateInstance<TaskTarget_LoadScene>();
                if (asset)
                {
                    int index = 0;
                    string confName = "";
                    UnityEngine.Object obj1 = null;
                    do
                    {
                        confName = path + "/" + typeof(TaskTarget_LoadScene).Name + "_" + index + ".asset";
                        obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(TaskTarget_LoadScene));
                        index++;
                    } while (obj1);
                    AssetDatabase.CreateAsset(asset, confName);
                    AssetDatabase.SaveAssets();
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = asset;
                }
            }
        }

#endif


    }
}