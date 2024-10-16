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
                    ComponentEntry.OnInitFinish += delegate
                    {
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
        public override void Show()
        {
            base.Show();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="isDestroy">是否销毁</param>
        /// <param name="userData"></param>
        public override void Hide(bool isDestroy = false)
        {
            base.Hide(isDestroy);
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


        /// <summary>
        /// 实体回收。
        /// </summary>
        protected internal override void OnRecycle()
        {
            base.OnRecycle();
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="childEntity">附加的子实体。</param>
        /// <param name="parentTransform">被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="childEntity">解除的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransform">被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
        }
    }
}
