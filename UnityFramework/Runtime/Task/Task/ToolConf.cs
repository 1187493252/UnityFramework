#if VIU_STEAMVR_2_0_0_OR_NEWER

using HighlightingSystem;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace UnityFramework.Runtime.Task
{
    public struct ToolTaskInfo
    {
        public bool isSetCanHover;
        public bool isCanHover;
        public bool isSetCanCatch;
        public bool isCanCatch;
        public bool isSetKinematic;
        public bool isKinematic;
        public bool isSetHide;
        public bool isHide;
        public bool isSetHighlight;
        public bool isHighlight;
        public bool isSetScaleSize;

        public bool isSetParent;
        /// <summary>
        /// 
        /// </summary>
        public ToolConf setParentTool;
        public Vector3 scaleSize;
        public bool isSetPose;
        public Vector3 position;
        public Vector3 angle;
        public int indexAoCao;
        public bool isSetColliderStart;
        public bool isColliderStart;

    }

    public class ToolConf : ScriptableObject
    {
        public string toolName;
        public float mass = 0.5f;
        [Range(0, 1)]
        [Tooltip("阻力")]
        public float drag = 0f;
        [Range(0, 1)]
        [Tooltip("角阻力")]
        public float angularDrag = 0.05f;
        public GameObject toolModel;

        //高亮信息设置
        [Header("高亮信息设置")]
        public bool isLightFlash;
        public bool isShowBody;
        public bool isLightAll;
        public float flashFrequency;
        public LoopMode loopMode = LoopMode.PingPong;
        public Easing easing = Easing.CubicInOut;

        public List<string> lightObjNames;
        public Material lightMat;
        [HideInInspector]
        public ToolBasic toolBasic;

        //拿起时的物理信息设置
        [Header("拿起时的物理信息设置")]
        [EnumFlags]
        public Hand.AttachmentFlags catchFlags = Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.VelocityMovement | Hand.AttachmentFlags.TurnOffGravity;
        public Vector3 handPosition;
        public Vector3 handAngle;

        /// <summary>
        /// 抓取高亮设置
        /// </summary>
        [Header("抓取高亮设置")]
        public bool isSetCatchHighlight;
        public bool isCatchHighlight;
        //碰到其他工具时的姿势设置(OnTrigger)
        [Header("碰到其他工具时的姿势设置(OnTrigger)初始化的时候也会读取")]
        public bool isSetTriggerPose;
        public Vector3 triggerPositon;
        public Vector3 triggerAngle;

        public bool isSetInitCanHover;
        public bool isInitCanHover;
        public bool isSetInitCanCatch;
        public bool isInitCanCatch;
        public bool isSetInitKinematic;
        public bool isInitKinematic;
        public bool isSetInitHide;
        public bool isInitHide;
        public bool isSetInitHighlight;
        public bool isInitHighlight;
        public bool isSetInitScaleSize;
        public Vector3 InitScaleSize;
        public bool isSetInitPose;
        public Vector3 InitPosition;
        public Vector3 InitAngle;

        public bool isSetColliderStart;
        /// <summary>
        /// true的时候是trigger勾选
        /// </summary>
        [Header("true的时候是trigger勾选")]
        public bool isCollider;

        /// <summary>
        /// 可以放到的某些位置的配置
        /// </summary>

        public int indexAoCao = 0;
        public bool canPutAoCao = true;
        [Header("任务开始时是否打开碰撞体")]
        public bool isColliderEnable = true;
        [Header("工具缩略图")]
        public Sprite thumbImage;

        [Header("是否显示工具名字")]
        public bool isShowToolName = false;
        [Header("工具显示名字")]
        public string showNameKey;
        [Header("是否使用重力")]
        public bool isUseGravity = true;
        [Header("从工具栏抓取松手后是否隐藏")]
        public bool isHideByFastToolDetach = true;
        [Header("从工具栏移除是否隐藏物体")]
        public bool isHideByRemoveFastTool = true;

#if UNITY_EDITOR
        [MenuItem("Assets/UnityFramework/Task/ToolConf", false, 0)]
        static void CreateDynamicConf()
        {
            UnityEngine.Object obj = Selection.activeObject;
            if (obj)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ScriptableObject asset = CreateInstance<ToolConf>();
                if (asset)
                {
                    int index = 0;
                    string confName = "";
                    UnityEngine.Object obj1 = null;
                    do
                    {
                        confName = path + "/" + typeof(ToolConf).Name + "_" + index + ".asset";
                        obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(ToolConf));
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