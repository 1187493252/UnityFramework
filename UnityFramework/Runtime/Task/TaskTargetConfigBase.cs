/*
* FileName:          
* CompanyName:       
* Author:            
* Description:       
*/


using System.Collections.Generic;
using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityFramework.Runtime
{

    public class TaskTargetConfigBase : TaskTargetConfig
    {

        public List<int> StartShowEntityId = new List<int>();
        public List<int> StartShowUIFormId = new List<int>();
        public List<int> StartHideUIFormId = new List<int>();

        public List<int> EndHideEntityId = new List<int>();
        public List<int> EndHideUIFormId = new List<int>();
        public List<int> EndShowEntityId = new List<int>();


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
            foreach (var item in StartShowEntityId)
            {
                EntityBase entityBase = ComponentEntry.Entity.GetSceneEntity(item);
                entityBase.Show();
            }
            foreach (var item in StartShowUIFormId)
            {
                EntityBase entityBase = ComponentEntry.Entity.GetSceneEntity(item);
                entityBase.Show();
            }
            foreach (var item in StartHideUIFormId)
            {
                EntityBase entityBase = ComponentEntry.Entity.GetSceneEntity(item);
                entityBase.Hide();
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

            foreach (var item in EndHideUIFormId)
            {
                EntityBase entityBase = ComponentEntry.Entity.GetSceneEntity(item);
                entityBase.Hide();
            }
            foreach (var item in EndHideEntityId)
            {
                EntityBase entityBase = ComponentEntry.Entity.GetSceneEntity(item);
                entityBase.Hide();
            }
            foreach (var item in EndShowEntityId)
            {
                EntityBase entityBase = ComponentEntry.Entity.GetSceneEntity(item);
                entityBase.Show();
            }

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
        [MenuItem("Assets/UnityFramework/Task/TaskTarget/TaskTargetConfigBase", false, 0)]
        static void CreateDynamicConf()
        {
            UnityEngine.Object obj = Selection.activeObject;
            if (obj)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ScriptableObject asset = CreateInstance<TaskTargetConfigBase>();
                if (asset)
                {
                    int index = 0;
                    string confName = "";
                    UnityEngine.Object obj1 = null;
                    do
                    {
                        confName = path + "/" + typeof(TaskTargetConfigBase).Name + "_" + index + ".asset";
                        obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(TaskTargetConfigBase));
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