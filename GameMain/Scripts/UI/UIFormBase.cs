/*
* FileName:          UIForm
* CompanyName:  
* Author:            relly
* Description:       
* 
*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UnityFramework.Runtime
{
    public class UIFormBase : UIFormLogic
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
                    id = UIForm.Id;
                }
                return id;
            }
        }

        /// <summary>
        /// 深度
        /// </summary>
        [Header("深度/SortOrder")]
        public int Depth = 0;
        Canvas canvas;

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




        /// <summary>
        /// UIForm注册 添加UIForm缓存
        /// </summary>
        protected void RegisterUIForm()
        {
            if (Id != 0)
            {
                if (ComponentEntry.UI)
                {
                    ComponentEntry.UI.AddSceneUIForm(this);
                }
                else
                {
                    ComponentEntry.OnInitFinish += delegate
                    {
                        ComponentEntry.UI.AddSceneUIForm(this);
                    };
                }
            }
        }

        /// <summary>
        /// 移除UIForm 移除UIForm缓存
        /// </summary>
        protected void RemoveUIForm()
        {
            ComponentEntry.UI.RemoveSceneUIForm(Id);
        }



        /// <summary>
        /// 打开 
        /// </summary>
        public override void Show()
        {
            OnOpen(this);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="isDestroy">是否销毁</param>
        /// <param name="userData"></param>
        public override void Hide(bool isDestroy = false)
        {
            OnClose(isDestroy, this);
        }






        /// <summary>
        /// 深度改变
        /// </summary>
        /// <param name="depth">深度</param>
        public void SetDepth(int depth)
        {
            if (canvas)
            {
                canvas.sortingOrder = depth;
            }
        }

        /// <summary>
        /// 界面初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            canvas = GetComponent<Canvas>();
            SetDepth(Depth);
            RegisterUIForm();

            if (!Dynamic)
            {
                SetVisible(InitShow);
            }
        }


        /// <summary>
        /// 界面打开。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        /// <summary>
        /// 界面关闭。
        /// </summary>
        /// <param name="isShutdown">是否是关闭界面管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            if (isShutdown)
            {
                RemoveUIForm();
                Destroy(this.gameObject);
            }
        }



        /// <summary>
        /// 界面轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 界面深度改变。
        /// </summary>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="depthInUIGroup">界面在界面组中的深度。</param>
        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            SetDepth(depthInUIGroup);
        }

        /// <summary>
        /// 界面回收。
        /// </summary>
        protected internal override void OnRecycle()
        {
            base.OnRecycle();
        }

        /// <summary>
        /// 界面暂停。
        /// </summary>
        protected internal override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// 界面暂停恢复。
        /// </summary>
        protected internal override void OnResume()
        {
            base.OnResume();
        }

        /// <summary>
        /// 界面遮挡。
        /// </summary>
        protected internal override void OnCover()
        {
            base.OnCover();
        }

        /// <summary>
        /// 界面遮挡恢复。
        /// </summary>
        protected internal override void OnReveal()
        {
            base.OnReveal();
        }

        /// <summary>
        /// 界面激活。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);
        }
    }
}

