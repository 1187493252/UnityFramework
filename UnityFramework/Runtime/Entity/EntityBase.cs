/*
* FileName:          EntityLogic
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public class EntityBase : EntityLogic
    {
        [SerializeField]
        private int id;
        public bool Dynamic = true;
        public bool InitShow = true;
        public int Id
        {
            get
            {
                if (Dynamic)
                {
                    id = Entity.Id;
                }
                return id;
            }
        }
        private void Awake()
        {
            if (!Dynamic)
            {
                OnInit(this);
            }
        }
        private void Update()
        {
            if (!Dynamic)
            {
                OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
            }
        }


        protected void RegisterEntity()
        {
            if (Id != 0)
            {
                if (ComponentEntry.Entity)
                {
                    ComponentEntry.Entity.AddSceneEntity(this);
                }
                else
                {
                    ComponentEntry.OnInitFinish += delegate {
                        ComponentEntry.Entity.AddSceneEntity(this);
                    };
                }

            }
        }

        /// <summary>
        /// 移除UIForm 移除UIForm缓存
        /// </summary>
        protected void RemoveEntity()
        {
            ComponentEntry.Entity.RemoveSceneEntity(Id);
        }

        /// <summary>
        /// 打开 
        /// </summary>
        public override void Show(object userData = null)
        {
            base.Show(userData);
            OnShow(userData);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void Hide(bool isShutdown = false, object userData = null)
        {
            base.Hide(isShutdown, userData);
            OnHide(isShutdown, userData);

        }

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            RegisterEntity();

            if (!Dynamic)
            {
                SetVisible(InitShow);
            }
        }



        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            if (isShutdown)
            {
                RemoveEntity();
                Destroy(this.gameObject);
            }
        }



        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }


        public virtual void SetToolTaskInit(TaskToolConfig taskToolConfig)
        {

        }


        public virtual void SetToolTaskStart(TaskToolConfig taskToolConfig)
        {
            if (taskToolConfig.isSetStartShowHide)
            {
                if (taskToolConfig.isStartShow)
                {
                    Show();
                }
                else
                {
                    Hide(false);
                }
            }
            if (taskToolConfig.isSetStartPose)
            {
                transform.localPosition = taskToolConfig.startPos;
                transform.localEulerAngles = taskToolConfig.startAngle;
            }
            if (taskToolConfig.isSetStartScale)
            {
                transform.localScale = taskToolConfig.startScale;
            }
        }

        public virtual void SetToolTaskEnd(TaskToolConfig taskToolConfig)
        {
            if (taskToolConfig.isSetEndShowHide)
            {
                if (taskToolConfig.isEndShow)
                {
                    Show();
                }
                else
                {
                    Hide(false);
                }
            }
            if (taskToolConfig.isSetEndPose)
            {
                transform.localPosition = taskToolConfig.endPos;
                transform.localEulerAngles = taskToolConfig.endAngle;
            }
            if (taskToolConfig.isSetEndScale)
            {
                transform.localScale = taskToolConfig.endScale;
            }
        }
        public virtual void OnTaskInit(TaskConfig taskConfig)
        {

        }
        public virtual void OnTaskStart(TaskConfig taskConfig)
        {

        }

        public virtual void OnTaskDoing(TaskConfig taskConfig)
        {

        }
        public virtual void OnTaskEnd(TaskConfig taskConfig)
        {

        }
    }
}
