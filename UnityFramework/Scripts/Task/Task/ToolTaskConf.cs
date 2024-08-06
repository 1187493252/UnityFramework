#if VIU_STEAMVR_2_0_0_OR_NEWER

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityFramework;

namespace UnityFramework.Runtime.Task
{
    /// <summary>
    /// 描述工具对应某个任务的具体信息
    /// </summary>
    public class ToolTaskConf : ScriptableObject
    {
        public ToolConf toolConf;

        public bool examinationModeHight = false;


        //任务开始时设置(OnTaskStart)
        [Header("任务开始时设置(OnTaskStart)")]
        public bool isSetStartCanHover;
        public bool isStartCanHover;
        public bool isHighlightOnHover;
        public bool isSetStartCanCatch;
        public bool isStartCanCatch;
        public bool isSetStartKinematic;
        public bool isStartKinematic;
        public bool isSetStartHide;
        public bool isStartHide;
        public bool isSetStartHighlight;
        public bool isStartHighlight;
        public bool isSetStartScaleSize;
        public Vector3 startScaleSize;
        #region//道具被拆卸又安装,或者有复杂的父类关系
        [Header("任务开始是否设置父物体")]
        public bool m_isSetParent;
        [Tooltip("设置在车下")]
        public bool m_isCar;
        [Tooltip("设置在别的工具下")]
        public bool m_isTool;
        [Tooltip("被设置工具")]
        public ToolConf m_parentTool;
        #endregion
        public bool isSetStartPose;
        public Vector3 StartPosition;
        public Vector3 StartAngle;

        public bool isSetStartCollider = false;
        public bool isStartCollider = false;
        [Header("任务开始设置从工具栏移除是否隐藏物体")]
        public bool isSetStartHideByRemoveFastTool = true;
        public bool StartHideByRemoveFastTool = true;

        //任务完成时设置(OnTaskEnd)   
        [Header("任务完成时设置(OnTaskEnd)")]
        public bool isSetEndCanHover;
        public bool isEndCanHover;
        public bool isSetEndCanCatch;
        public bool isEndCanCatch;
        public bool isSetEndKinematic;
        public bool isEndKinematic;
        public bool isSetEndHide;
        public bool isEndHide;
        public bool isSetEndHighlight;
        public bool isEndHighlight;
        public bool isSetEndScaleSize;
        public Vector3 endScaleSize;

        #region//道具被拆卸又安装,或者有复杂的父类关系
        [Header("任务结束是否设置父物体")]
        public bool isSetParent;
        [Tooltip("设置在车下")]
        public bool isCar;
        [Tooltip("设置在别的工具下")]
        public bool isTool;
        [Tooltip("被设置工具")]
        public ToolConf parentTool;
        #endregion

        public bool isSetEndPose;
        public Vector3 EndPosition;
        public Vector3 EndAngle;

        [Header("结束是否设置碰撞")]
        public bool isSetEndCollider = false;
        public bool isEndCollider = false;
        [Header("任务结束设置从工具栏移除是否隐藏物体")]
        public bool isSetEndHideByRemoveFastTool = true;
        public bool EndHideByRemoveFastTool = true;
        //public bool isHighLightOnExam = false;
        //public bool isHideOnExam = false;
        /// <summary>
        /// 设置凹槽的位置
        /// </summary>
        [Header("设置凹槽的位置")]
        public int aocaoStartIndex = -1;
        public int aocaoEndIndex = -1;
        public bool isSetAoCaoStart = false;
        public bool isAoCaoStart = false;
        public bool isSetAoCaoEnd = false;
        public bool isAoCaoEnd = false;
        /// <summary>
        /// 在某些模式下的状态
        /// </summary>
        public TaskMode activeMode = TaskMode.Study;


        public bool asAChild = false;
        public ToolTaskConf targetPos = null;
        public bool isSetTrigger = false;

        //public bool isColliderEnable = true;

        //此参数针对功能较复杂的复合型Tool设计 通过自定义参数来处理任务跳转时工具的状态刷新
        //当上述配置不能满足这些复杂工具的状态初始化时 可以通过下列参数进行处理
        [Header("复杂工具的开始任务参数配置 只在跳转任务时生效")]
        /// <summary>
        /// 自定义配置 任务开始时执行
        /// 由实现了TaskJumpToolHandler的工具类通过TaskStart接口实现
        /// </summary>
        [Tooltip("复杂工具的自定义配置")]
        public ScriptableObject taskStartConf;
        /// <summary>
        /// 此参数针对功能较复杂的复合型Tool设计 通过自定义参数来处理任务跳转是工具的强制刷新
        /// </summary>
        [Tooltip("复杂工具的自定义参数")]
        public string[] taskStartArgs;

        public void OnTaskInit(TaskConf taskConf)
        {
            //Debug.Log(taskConf + "====OnTaskInit====" + toolConf);
            if (activeMode == TaskMode.Default || activeMode == TaskManager.Instance.TaskMode)
            {
                toolConf.toolBasic?.SetToolTaskInit(this);
                toolConf.toolBasic?.OnTaskInit(taskConf);
            }
        }

        public void OnTaskStart(TaskConf taskConf)
        {
            if (activeMode == TaskMode.Default || activeMode == TaskManager.Instance.TaskMode)
            {
                //Debug.Log(taskConf + " ====OnTaskStart====  " + toolConf);
                toolConf.toolBasic?.SetToolTaskStart(this);

                toolConf.toolBasic?.OnTaskStart(taskConf);
            }
        }


        public void OnTaskDoing(TaskConf taskConf)
        {
            if (activeMode == TaskMode.Default || activeMode == TaskManager.Instance.TaskMode)
            {
                toolConf.toolBasic?.OnTaskDoing(taskConf);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskConf"></param>
        public void OnTaskEnd(TaskConf taskConf)
        {
            if (activeMode == TaskMode.Default || activeMode == TaskManager.Instance.TaskMode)
            {
                //Debug.Log("activeMode------------" + activeMode);
                toolConf.toolBasic?.SetToolTaskEnd(this);
                toolConf.toolBasic?.OnTaskEnd(taskConf);
            }
        }



#if UNITY_EDITOR
        [MenuItem("Assets/UnityFramework/Task/ToolTaskConf", false, 0)]
        static void CreateDynamicConf()
        {
            UnityEngine.Object obj = Selection.activeObject;
            if (obj)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ScriptableObject asset = CreateInstance<ToolTaskConf>();
                if (asset)
                {
                    int index = 0;
                    string confName = "";
                    UnityEngine.Object obj1 = null;
                    do
                    {
                        confName = path + "/" + typeof(ToolTaskConf).Name + "_" + index + ".asset";
                        obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(ToolTaskConf));
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

#endif