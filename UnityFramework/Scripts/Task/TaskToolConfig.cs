/*
* FileName:          TaskToolConfig
* CompanyName:       
* Author:            
* Description:       任务工具
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityFramework.Runtime
{
    public class TaskToolConfig : ScriptableObject
    {


        public string Name;
        public int ToolID;


        public TaskMode TaskMode = TaskMode.Default;



        /// <summary>
        /// 任务初始化
        /// </summary>
        public void OnTaskInit(TaskConfig taskConfig)
        {
            if (TaskMode == TaskMode.Default || TaskMode == ComponentEntry.Task.TaskMode)
            {

            }
        }

        /// <summary>
        /// 任务开始
        /// </summary>
        public void OnTaskStart(TaskConfig taskConfig)
        {
            if (TaskMode == TaskMode.Default || TaskMode == ComponentEntry.Task.TaskMode)
            {

            }
        }

        /// <summary>
        /// 任务持续中
        /// </summary>
        public void OnTaskDoing(TaskConfig taskConfig)
        {
            if (TaskMode == TaskMode.Default || TaskMode == ComponentEntry.Task.TaskMode)
            {

            }
        }


        /// <summary>
        /// 任务结束 不论任务的结果正确/出错/跳过
        /// </summary>
        public void OnTaskEnd(TaskConfig taskConfig)
        {
            if (TaskMode == TaskMode.Default || TaskMode == ComponentEntry.Task.TaskMode)
            {

            }
        }



#if UNITY_EDITOR
        [MenuItem("Assets/UnityFramework/Task/TaskToolConfig", false, 0)]
        static void CreateDynamicConf()
        {
            UnityEngine.Object obj = Selection.activeObject;
            if (obj)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ScriptableObject asset = CreateInstance<TaskToolConfig>();
                if (asset)
                {
                    int index = 0;
                    string confName = "";
                    UnityEngine.Object obj1 = null;
                    do
                    {
                        confName = path + "/" + typeof(TaskToolConfig).Name + "_" + index + ".asset";
                        obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(TaskToolConfig));
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