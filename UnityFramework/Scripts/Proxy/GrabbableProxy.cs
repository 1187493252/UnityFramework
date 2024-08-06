/*
* FileName:          GrabbableProxy
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public class GrabbableProxy
    {
        /// <summary>
        /// 抓取
        /// </summary>
        public Action<object, object> OnGrab;

        /// <summary>
        /// 抓取中
        /// </summary>
        public Action<object, object> OnGrabbing;

        /// <summary>
        /// 释放
        /// </summary>
        public Action<object, object> OnRelease;

        /// <summary>
        /// 触摸
        /// </summary>
        public Action<object, object> OnTouch;

        /// <summary>
        /// 结束触摸
        /// </summary>
        public Action<object, object> OnTouchEnd;

        /// <summary>
        /// 扳机键
        /// </summary>
        public Action<object, object> OnTrigger;

        /// <summary>
        /// 扳机键按下
        /// </summary>
        public Action<object, object> OnTriggerDown;

        /// <summary>
        /// 扳机键抬起
        /// </summary>
        public Action<object, object> OnTriggerUp;

        /// <summary>
        /// 按钮1,右手A,左手X
        /// </summary>
        public Action<object, object> OnButton1;

        /// <summary>
        /// 按钮1,右手A,左手X
        /// </summary>
        public Action<object, object> OnButton1Down;

        /// <summary>
        /// 按钮1,右手A,左手X
        /// </summary>
        public Action<object, object> OnButton1Up;

        /// <summary>
        /// 按钮2,右手B,左手Y
        /// </summary>
        public Action<object, object> OnButton2;

        /// <summary>
        /// 按钮2,右手B,左手Y
        /// </summary>
        public Action<object, object> OnButton2Down;

        /// <summary>
        /// 按钮2,右手B,左手Y
        /// </summary>
        public Action<object, object> OnButton2Up;


        public void Clear()
        {
            OnGrab = null;
            OnGrabbing = null;
            OnRelease = null;
            OnTouch = null;
            OnTouchEnd = null;
            OnTrigger = null;
            OnTriggerDown = null;
            OnTriggerUp = null;
            OnButton1 = null;
            OnButton1Down = null;
            OnButton1Up = null;
            OnButton2 = null;
            OnButton2Down = null;
            OnButton2Up = null;
        }
    }
}
