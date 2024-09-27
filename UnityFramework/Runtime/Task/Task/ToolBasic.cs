#if VIU_STEAMVR_2_0_0_OR_NEWER
using System;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace UnityFramework.Runtime.Task
{
    /// <summary>
    /// 工具基本功能，若要扩展功能，可继承此类
    /// </summary>
    public class ToolBasic : ToolInteractable
    {
        public ToolConf toolConf;
        [HideInInspector]
        public Hand catchHand;
        [HideInInspector]
        public Hand pressHand;
        protected Rigidbody mRigidbody;
        protected VelocityEstimator velocityEstimator;
        protected ToolHighLight toolHighLight;//可以穿透
        protected Interactable interactable;
        protected bool isComplete = false;

        public event Action<Hand, ToolConf> OnCatch;
        public event Action<Hand, ToolConf> OnCatching;
        public event Action<Hand, ToolConf> OnDetach;
        public event Action<Hand, ToolConf> OnHoverBegin;
        public event Action<Hand, ToolConf> OnHover;
        public event Action<Hand, ToolConf> OnHoverEnd;
        public event Action<Hand, ToolConf> OnPress;
        public event Action<Hand, ToolConf> OnPressUp;
        public event Action<Hand, ToolConf> OnPressDown;
        public event Action<ToolConf> OnEnterTool;
        public event Action<ToolConf> OnExitTool;
        public event Action<ToolConf, ToolConf> OnCollider;
        public event Action<ToolConf, ToolConf> OnTrigger;

        /// <summary>
        /// 新增与指定工具的交互
        /// </summary>
        public event Action<ToolConf> OnBeginHoverTool;
        public event Action<ToolConf> OnEndHoverTool;








        /// <summary>
        /// 从其他指定位置放回到车上
        /// </summary>
        public bool isCanBack;
        public bool taskStartBoxColliderState = true;
        /// <summary>
        /// 是否是安装的过程
        /// </summary>
        public bool isInstal = false;
        public BoxCollider[] boxColliders;
        //---------------------
        protected TextMesh toolNameMesh;
        internal bool attached { get => catchHand != null; }


        internal Vector3 localpos;
        internal Quaternion localq;
        internal Transform localparent;

        /// <summary>
        /// 考试模式是是否显示Mesh，默认显示（true）
        /// </summary>
        [SerializeField]
        private bool isShowMeshOnExam = true;

        protected virtual void OnDisable()
        {
            if (ComponentEntry.Timer)
            {
                ComponentEntry.Timer.CreateTimer(() =>
                {
                    if (catchHand)
                        catchHand.DetachObject(gameObject);
                    if (pressHand)
                        pressHand.DetachObject(gameObject);

                    catchHand = null;
                    pressHand = null;

                }, "", Time.deltaTime);
            }
        }

        protected override void Awake()
        {



            base.Awake();
            InitComponent();
            InitToolModel();
            InitTool();
            boxColliders = transform.GetComponentsInChildren<BoxCollider>();

            localpos = transform.localPosition;
            localq = transform.localRotation;
            localparent = transform.parent;





        }


        protected override void OnDestroy()
        {



            base.OnDestroy();
        }

        public void ResetPos()
        {
            transform.parent = localparent;
            transform.localPosition = localpos;
            transform.localRotation = localq;
        }

        public void ToolAwake()
        {
            Awake();
            SetToolTaskInitInToolBasic();
            if (toolConf.isSetInitPose)
            {
                transform.localPosition = toolConf.InitPosition;
                transform.localEulerAngles = toolConf.InitAngle;

            }
            if (toolConf.isSetColliderStart)
            {
                SetTrigger(toolConf.isCollider);
            }

        }

        protected virtual void Start()
        {

        }

        private void OnEnterAoCaoTool(ToolConf obj)
        {

        }


        #region 碰撞体触发器
        /// <summary>
        /// 与其他工具发生交互
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("OnTriggerEnter---------------");
            ToolBasic otherTool = GetTypeParent<ToolBasic>(other.gameObject, true);

            //TestDebug.inst.SetMsg(toolConf.toolName + "   " + otherTool+" "+other.name);
            if (otherTool)
            {
                colliderTool = otherTool;
                OnEnterTool_(otherTool.toolConf);
            }
        }

        public ToolBasic colliderTool;
        private void OnCollisionEnter(Collision collision)
        {
            ToolBasic otherTool = GetTypeParent<ToolBasic>(collision.gameObject, true);
            if (otherTool)
            {
                colliderTool = otherTool;
                OnColliderEnter(otherTool.toolConf);
            }

        }

        private void OnCollisionExit(Collision collision)
        {
            ToolBasic otherTool = GetTypeParent<ToolBasic>(collision.gameObject, true);
            if (otherTool)
            {
                if (colliderTool && colliderTool == otherTool)
                {
                    colliderTool = null;
                }
                OnColliderExit(otherTool.toolConf);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            //Debug.Log("OnTriggerExit");
            ToolBasic otherTool = GetTypeParent<ToolBasic>(other.gameObject, true);
            if (otherTool)
            {
                if (colliderTool && colliderTool == otherTool)
                {
                    colliderTool = null;
                }
                OnExitTool_(otherTool.toolConf);
            }


        }
        #endregion

        private void InitComponent()
        {
            mRigidbody = GetComponent<Rigidbody>();
            toolHighLight = GetComponent<ToolHighLight>();
            interactable = GetComponent<Interactable>();
            throwable = GetComponent<Throwable>();
            velocityEstimator = GetComponent<VelocityEstimator>();

        }


        private void InitToolModel()
        {
            if (toolConf)
            {
                if (toolConf.toolModel != null)
                {
                    var goTool = Instantiate<GameObject>(toolConf.toolModel, transform);
                    goTool.name = toolConf.toolModel.name;
                    goTool.transform.localPosition = Vector3.zero;
                    goTool.transform.eulerAngles = Vector3.zero;
                }
            }
        }


        #region 交互功能
        protected override void OnCatch_(Hand hand)
        {
            //Debug.LogError("OnCatch--" + toolConf);
            catchHand = hand;

            if (toolConf)
            {
                //Debug.Log("OnCatch------"+ toolConf);
                if (OnCatch != null)
                    OnCatch(hand, toolConf);
                if (toolConf.isSetCatchHighlight)
                    SetToolHighlight(toolConf.isCatchHighlight);
            }
            SetCatchPose(hand);
            SetTrigger(true);


            SetToolKinematic(true);
        }

        protected override void OnCatching_(Hand hand)
        {
            //Debug.LogError("OnCatching--" + toolConf);
            if (toolConf)
            {
                if (OnCatching != null)
                    OnCatching(hand, toolConf);
                //拿着物体的手柄震动
                //VRHandHelper.Instance.ShockHand(hand, (ushort)(500));
            }

        }

        protected override void OnDetach_(Hand hand)
        {
            //Debug.LogError("OnDetach--" + toolConf);
            catchHand = null;
            if (toolConf)
            {
                if (OnDetach != null)
                {
                    OnDetach(hand, toolConf);
                }
            }

            SetTrigger(false);
        }

        protected override void OnHoverBegin_(Hand hand)
        {
            //Debug.LogError("OnHoverBegin---"+ toolConf);

            if (toolConf)
            {
                if (OnHoverBegin != null)
                    OnHoverBegin(hand, toolConf);
            }
        }

        protected override void OnHover_(Hand hand)
        {
            //Debug.LogError("OnHover" + toolConf);

            if (toolConf)
            {
                if (OnHover != null)
                    OnHover(hand, toolConf);
            }
        }

        protected override void OnHoverEnd_(Hand hand)
        {
            //Debug.LogError("OnHoverEnd" + toolConf);

            if (toolConf)
            {
                if (OnHoverEnd != null)
                    OnHoverEnd(hand, toolConf);
            }

        }

        protected override void OnPress_(Hand hand)
        {
            //Debug.LogError("OnPress");

            pressHand = hand;
            if (toolConf)
            {
                if (OnPress != null)
                    OnPress(hand, toolConf);
            }
        }

        protected override void OnPressDown_(Hand hand)
        {
            //Debug.LogError("OnPressDown:" + toolConf);
            catchHand = hand;
            if (toolConf)
            {
                OnPressDown?.Invoke(hand, toolConf);
                //if (OnPressDown != null)
                //    OnPressDown(hand, toolConf);
            }
        }

        protected override void OnPressUp_(Hand hand)
        {
            //Debug.LogError("OnPressUp");

            pressHand = null;
            if (toolConf)
            {
                if (OnPressUp != null)
                    OnPressUp(hand, toolConf);
            }

        }


        //当进入可以匹配的工具
        protected virtual void OnEnterTool_(ToolConf otherTool)
        {
            //if (toolConf && otherTool)
            //    Debug.LogFormat("{0} OnEnter {1}", toolConf.toolName, otherTool.toolName);

            if (toolConf)
            {
                if (OnEnterTool != null)
                    OnEnterTool(otherTool);
            }
            if (toolConf)
            {
                if (OnTrigger != null)
                {
                    OnTrigger(toolConf, otherTool);
                }
            }

            //进行位置设置,进行高亮设置
            if (toolConf && toolConf.isSetTriggerPose)
            {
                mRigidbody.isKinematic = true;
                transform.localPosition = toolConf.triggerPositon;
                transform.localEulerAngles = toolConf.triggerAngle;
            }
        }


        protected virtual void OnColliderEnter(ToolConf otherTool)
        {
            if (toolConf)
            {
                if (OnEnterTool != null)
                    OnEnterTool(otherTool);
            }
            if (toolConf)
            {
                if (OnCollider != null)
                {
                    OnCollider(toolConf, otherTool);
                }
            }

            //进行位置设置,进行高亮设置
            if (toolConf && toolConf.isSetTriggerPose)
            {
                mRigidbody.isKinematic = true;
                transform.localPosition = toolConf.triggerPositon;
                transform.localEulerAngles = toolConf.triggerAngle;
            }
        }
        protected virtual void OnColliderExit(ToolConf otherTool)
        {

        }

        //离开可以匹配的工具
        protected virtual void OnExitTool_(ToolConf otherTool)
        {
            //Debug.Log("OnExitTool");
            if (toolConf)
            {
                if (OnExitTool != null)
                    OnExitTool(otherTool);
            }
            if (toolConf && otherTool)
                /*Debug.LogFormat("{0} OnExit {1}", toolConf.toolName, otherTool.toolName)*/
                ;
        }

        //某个交互工具进入
        protected virtual void OnBeginHoverTool_(ToolConf otherTool)
        {
            OnBeginHoverTool?.Invoke(otherTool);
            //Debug.LogFormat("{0} OnBeginHoverTool_ {1}", toolConf.toolName, otherTool.toolName);
        }
        //某个交互工具离开
        protected virtual void OnEndHoverTool_(ToolConf otherTool)
        {
            OnEndHoverTool?.Invoke(otherTool);
            //Debug.LogFormat("{0} OnEndHoverTool_ {1}", toolConf.toolName, otherTool.toolName);
        }

        #endregion

        #region 任务功能,监听任务

        //任务未开始时设置
        public virtual void InitTool()
        {
            //Debug.Log(string.Format("{0} InitTool", toolConf.toolName));
            if (toolConf)
            {
                toolConf.toolBasic = this;

                if (mRigidbody)
                {
                    mRigidbody.mass = toolConf.mass;
                    mRigidbody.drag = toolConf.drag;
                    mRigidbody.angularDrag = toolConf.angularDrag;
                }


                if (throwable && toolConf)
                    throwable.attachmentFlags = toolConf.catchFlags;
            }
        }

        /// <summary>
        /// 设置工具最初状态
        public virtual void OnTaskInit(TaskConf taskConf)
        {

        }

        public virtual void OnTaskStart(TaskConf taskConf)
        {

        }

        public virtual void OnTaskDoing(TaskConf taskConf)
        {

        }

        public virtual void OnTaskEnd(TaskConf taskConf)
        {

        }
        #endregion


        #region 功能性方法

        protected T GetTypeParent<T>(GameObject goSlef, bool recursive = true)
        {
            var result = goSlef.GetComponent<T>();
            if (result != null)
                return result;

            if (recursive)
            {
                var trParent = goSlef.transform.parent;
                if (trParent)
                    return GetTypeParent<T>(trParent.gameObject, true);
            }

            return default(T);
        }
        public void SetHoverTool(ToolConf otherTool)
        {
            this.OnBeginHoverTool_(otherTool);
        }
        public void SetHoverExitTool(ToolConf otherTool)
        {
            this.OnEndHoverTool_(otherTool);
        }

        protected void SetCatchPose(Hand hand)
        {
            Transform objectAttachmentPoint = UnityUtil.GetTypeChildByName<Transform>(hand.gameObject, "ObjectAttachmentPoint");
            //需要进行抓取姿态设置
            if (toolConf)
            {
                if (toolConf.catchFlags.HasFlag(Hand.AttachmentFlags.SnapOnAttach))
                {
                    objectAttachmentPoint.localPosition = toolConf.handPosition;
                    objectAttachmentPoint.localEulerAngles = toolConf.handAngle;
                }
            }
        }

        private void SetToolTaskInitInToolBasic()
        {
            SetToolHighlight(false);
            ToolTaskInfo toolTaskInfo = TaskManager.Instance.GetToolInitInfo(toolConf);
            //Debug.LogError("运行SetToolTaskInitInToolBasic---" + gameObject.name + "的Init    " + toolTaskInfo.isSetHide);

            if (toolTaskInfo.isSetPose && catchHand == null)
            {





                if (toolTaskInfo.isSetParent && toolTaskInfo.setParentTool && toolTaskInfo.setParentTool.toolBasic)
                    transform.SetParent(toolTaskInfo.setParentTool.toolBasic.transform);

                transform.localPosition = toolTaskInfo.position;
                transform.localEulerAngles = toolTaskInfo.angle;

                //Debug.LogError("------------------");
                //Debug.Log("______物品："+transform.name+"重置位置："+ toolTaskInfo.position+" _________________________ "+ transform.localPosition);
            }
            //print(name);
            if (toolTaskInfo.isSetCanCatch)
                SetToolCatch(toolTaskInfo.isCanCatch);
            if (toolTaskInfo.isSetKinematic)
                SetToolKinematic(toolTaskInfo.isKinematic);
            if (toolTaskInfo.isSetHighlight)
                SetToolHighlight(toolTaskInfo.isHighlight);
            if (toolTaskInfo.isSetHide)
            {
                gameObject.SetActive(!toolTaskInfo.isHide);
            }
            if (toolTaskInfo.isSetScaleSize)
                transform.localScale = toolTaskInfo.scaleSize;
            if (toolTaskInfo.isSetColliderStart)
            {
                SetCollider(toolTaskInfo.isColliderStart);
            }

            if (toolTaskInfo.isSetCanHover && interactable)
            {
                isCanHover = toolTaskInfo.isCanHover;
                //interactable.highlightOnHover = isCanHover;
            }

        }


        public void SetToolTaskInit(ToolTaskConf toolTaskConf)
        {
            SetToolHighlight(false);
            ToolTaskInfo toolTaskInfo = TaskManager.Instance.GetToolInitInfo(toolConf);
            //Debug.LogError("运行SetToolTaskInit---" + gameObject.name + "的Init    " + toolTaskInfo.isSetHide);

            if (catchHand != null)
                OnDetach_(catchHand);

            if (toolTaskInfo.isSetPose && catchHand == null)
            {

                if (toolTaskInfo.isSetParent && toolTaskInfo.setParentTool && toolTaskInfo.setParentTool.toolBasic)
                    transform.SetParent(toolTaskInfo.setParentTool.toolBasic.transform);

                //Debug.Log(transform.name);
                transform.localPosition = toolTaskInfo.position;
                transform.localEulerAngles = toolTaskInfo.angle;

                //Debug.LogError("------------------");
                //Debug.LogError("______物品："+transform.name+"重置位置："+ toolTaskInfo.position+" _________________________ "+ transform.localPosition);
            }


            SetCollidersEnabled(toolConf.isColliderEnable);
            //Debug.LogError(gameObject.name+"的Init    "+ toolConf.isColliderEnable);
            if (toolTaskInfo.isSetCanCatch)
                SetToolCatch(toolTaskInfo.isCanCatch);
            if (toolTaskInfo.isSetKinematic)
                SetToolKinematic(toolTaskInfo.isKinematic);
            if (toolTaskInfo.isSetHighlight)
                SetToolHighlight(toolTaskInfo.isHighlight);
            if (toolTaskInfo.isSetHide)
            {
                gameObject.SetActive(!toolTaskInfo.isHide);
            }
            if (toolTaskInfo.isSetScaleSize)
                transform.localScale = toolTaskInfo.scaleSize;

            if (toolTaskInfo.isSetColliderStart)
            {
                SetCollider(toolTaskInfo.isColliderStart);
            }

            if (toolTaskInfo.isSetCanHover && interactable)
            {
                isCanHover = toolTaskInfo.isCanHover;
                //interactable.highlightOnHover = isCanHover;
            }


        }

        public void SetToolTaskStart(ToolTaskConf toolTaskConf)
        {
            //Debug.Log("SetToolTaskStart---" + TaskManager.Instance.CurrentTask + "-----" + toolTaskConf);
            //SetToolHighlight(false);
            if (toolTaskConf)
            {
                //需要设置父物体
                if (toolTaskConf.m_isSetParent)
                {
                    if (toolTaskConf.m_isTool)
                    {
                        transform.SetParent(toolTaskConf.m_parentTool.toolBasic.transform);
                    }

                }
                if (toolTaskConf.isSetStartPose && catchHand == null)
                {
                    //需要设置父物体
                    if (toolTaskConf.m_isSetParent)
                    {
                    }
                    else
                    {



                    }

                    transform.localPosition = toolTaskConf.StartPosition;
                    transform.localEulerAngles = toolTaskConf.StartAngle;

                }

                SetCollidersEnabled(taskStartBoxColliderState);
                if (toolTaskConf.isSetStartCollider)
                {
                    SetCollider(toolTaskConf.isStartCollider);
                }

                if (toolTaskConf.isSetStartCanCatch)
                    SetToolCatch(toolTaskConf.isStartCanCatch);
                if (toolTaskConf.isSetStartKinematic)
                    SetToolKinematic(toolTaskConf.isStartKinematic);
                if (toolTaskConf.isSetStartHighlight)
                {
                    if (TaskManager.Instance.IsExam)
                        SetToolHighlight(toolTaskConf.isStartHighlight && toolTaskConf.examinationModeHight, true);
                    else
                        SetToolHighlight(toolTaskConf.isStartHighlight, true);

                }
                if (toolTaskConf.isSetStartHide)
                    gameObject.SetActive(!toolTaskConf.isStartHide);
                if (toolTaskConf.isSetStartScaleSize)
                    transform.localScale = toolTaskConf.startScaleSize;

                if (toolTaskConf.isSetStartCanHover && interactable)
                {
                    isCanHover = toolTaskConf.isStartCanHover;
                    //	interactable.highlightOnHover = isCanHover;
                }
                if (toolTaskConf && interactable)
                {
                    interactable.highlightOnHover = toolTaskConf.isHighlightOnHover && !TaskManager.Instance.IsExam;
                }

                if (toolTaskConf.isSetTrigger)
                {
                    foreach (var item in boxColliders)
                    {
                        //Debug.Log(item.name);
                        item.isTrigger = true;
                    }
                }
                if (toolTaskConf.isSetStartHideByRemoveFastTool)
                {
                    toolConf.isHideByRemoveFastTool = toolTaskConf.StartHideByRemoveFastTool;
                }
            }
        }

        public void SetToolTaskEnd(ToolTaskConf toolTaskConf)
        {
            //Debug.Log("SetToolTaskEnd---" + TaskManager.Instance.CurrentTask + "-----" + toolTaskConf);
            if (toolTaskConf)
            {
                if (toolTaskConf.isSetParent)
                {
                    if (toolTaskConf.isTool)
                    {
                        transform.SetParent(toolTaskConf.parentTool.toolBasic.transform);
                    }

                }
                if (toolTaskConf.isSetEndPose && catchHand == null)
                {
                    //需要设置父物体
                    if (toolTaskConf.isSetParent)
                    {

                    }
                    else
                    {

                        if (toolTaskConf.asAChild && toolTaskConf.targetPos != null)
                        {
                            transform.SetParent(toolTaskConf.targetPos.toolConf.toolBasic.transform.parent);
                        }
                    }

                    transform.localPosition = toolTaskConf.EndPosition;
                    transform.localEulerAngles = toolTaskConf.EndAngle;
                }
                if (toolTaskConf.isSetEndCollider)
                {
                    SetCollider(toolTaskConf.isEndCollider);
                }
                //SetCollidersEnabled(toolConf.isColliderEnable);
                if (toolTaskConf.isSetEndCanCatch)
                    SetToolCatch(toolTaskConf.isEndCanCatch);
                //Debug.Log("toolTaskConf----" + toolTaskConf + "-----------isEndKinematic---" + toolTaskConf.isEndKinematic);
                if (toolTaskConf.isSetEndKinematic)
                    SetToolKinematic(toolTaskConf.isEndKinematic);
                if (toolTaskConf.isSetEndHighlight)
                    if (TaskManager.Instance.IsExam)
                        SetToolHighlight(toolTaskConf.isEndHighlight && toolTaskConf.isEndHighlight, true);
                    else
                        SetToolHighlight(toolTaskConf.isEndHighlight, true);

                if (toolTaskConf.isSetEndHide)
                {
                    if (!toolTaskConf.isEndHide == false && catchHand != null)
                    {
                        catchHand.DetachObject(gameObject);
                        catchHand = null;
                        ComponentEntry.Timer.CreateTimer(() => gameObject.SetActive(false), "", 0.3f, 1);
                    }
                    else
                        gameObject.SetActive(!toolTaskConf.isEndHide);
                }
                if (toolTaskConf.isSetEndScaleSize)
                    transform.localScale = toolTaskConf.endScaleSize;

                if (toolTaskConf.isSetEndCanHover && interactable)
                {
                    isCanHover = toolTaskConf.isEndCanHover;
                    //interactable.highlightOnHover = isCanHover;
                    //此处单独设置是因为upbtn全程都可以使用，并且触碰的时候高亮
                    if (toolTaskConf.name.Equals("10_UpBtn"))
                    {
                        //interactable.highlightOnHover = true;
                    }
                }

                if (toolTaskConf.isSetEndHideByRemoveFastTool)
                {
                    toolConf.isHideByRemoveFastTool = toolTaskConf.EndHideByRemoveFastTool;
                }
            }
            ///任务结束后 关闭高亮
            SetToolHighlight(false);
        }

        public void SetCollider(bool isTirgger)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            //设置碰撞体
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = isTirgger;
            }
        }

        public void SetTrigger(bool isTirgger)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            //设置碰撞体
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].isTrigger = isTirgger;
            }
        }

        public void SetCollidersEnabled(bool enabled)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            //设置碰撞体
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = enabled;
            }
        }

        public void ToggleCollider(bool isOn)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            //设置碰撞体
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = isOn;
            }
        }

        public void SetToolHover(bool isCanHover)
        {
            if (interactable)
            {
                //interactable.highlightOnHover = isCanHover;
                this.isCanHover = isCanHover;
            }
        }

        //设置工具物理属性
        public void SetToolCatch(bool isCanCatch)
        {
            //设置碰撞体
            //Collider[] colliders = GetComponentsInChildren<Collider>();
            //for (int i = 0; i < colliders.Length; i++)
            //{
            //   colliders[i].enabled = isPhysics;
            //}

            //设置刚体

            //if (mRigidbody)
            //    mRigidbody.isKinematic = !isPhysics;
            this.isCanCatch = isCanCatch;

            if (throwable)
            {
                if (isCanCatch)
                    throwable.attachmentFlags = toolConf.catchFlags;
                else
                    throwable.attachmentFlags = Hand.AttachmentFlags.TurnOnKinematic;
            }

            if (!isCanCatch && velocityEstimator)
                velocityEstimator.FinishEstimatingVelocity();
        }

        public void SetToolKinematic(bool isKinematic)
        {
            if (mRigidbody)
                mRigidbody.isKinematic = isKinematic;
            //SetTrigger(isKinematic);
        }
        public void SetToolUseGravity(bool isUseGravity)
        {
            if (mRigidbody)
                mRigidbody.useGravity = isUseGravity;
            //SetTrigger(isKinematic);
        }
        public void SetToolHighlight(bool isHighlight, bool handlerChilds = false)
        {
            //Debug.Log(gameObject.name+"---SetToolHighlight---" + isHighlight);



            if (toolHighLight)
            {
                if (isHighlight /*&& !catchHand*/)
                {
                    toolHighLight.ShowToolLight();
                }
                else
                    toolHighLight.HideToolLight();
            }


            if (handlerChilds)
            {
                ToolBasic[] ts = transform.GetComponentsInChildren<ToolBasic>();
                for (int i = 0; i < ts.Length; i++)
                    ts[i].SetToolHighlight(isHighlight);
            }
        }
        public void SetHighlight(bool isHighlight, bool isTween = false)
        {
            if (toolHighLight)
            {
                if (isHighlight)
                {
                    toolHighLight.ShowToolLight(isTween);
                }
                else
                    toolHighLight.HideToolLight();
            }
        }



        public void ShowBody()
        {
            toolHighLight.ShowBody();
        }
        public void HideBody()
        {
            toolHighLight.HideBody();
        }
        public void SetLightEnd(Hand arg1)
        {
            toolHighLight.SetLightEnd();

        }
        #endregion
        //------------------
        public void HideTool()
        {
            if (gameObject.activeInHierarchy)
            {
                ToggleCollider(false);
                gameObject.SetActive(false);
                SetToolHighlight(false);
                HideToolName();
            }
        }
        public void ShowTool()
        {
            if (!gameObject.activeInHierarchy)
            {
                ToggleCollider(true);
                gameObject.SetActive(true);
            }
        }
        protected void HideToolName()
        {
            if (toolNameMesh != null)
            {
                if (toolNameMesh.gameObject.activeSelf)
                {
                    toolNameMesh.gameObject.SetActive(false);
                }
            }
        }




        //Vector3 toolPos;
        Vector3 toolScale;
        Vector3 toolAngle;
        Transform toolParent;
        /// <summary>
        /// <summary>
        /// 重置工具位置
        /// </summary>


        public void UseGravity(bool _state)
        {
            //Debug.LogError("UseGravity");

            if (mRigidbody == null)
            {
                mRigidbody = gameObject.AddComponent<Rigidbody>();
            }
            if (mRigidbody == null) return;
            SetMass(true);
            mRigidbody.useGravity = _state;
            mRigidbody.velocity = Vector3.zero;
            mRigidbody.angularVelocity = Vector3.zero;
        }

        public void SetMass(bool _state)
        {
            if (mRigidbody != null)
            {
                mRigidbody.mass = _state ? toolConf.mass : 1;
            }
        }



    }



}
#endif