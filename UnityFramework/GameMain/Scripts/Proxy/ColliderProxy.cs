/*
* FileName:          ColliderProxy
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
    public class ColliderProxy
    {
        public Collider[] Agents { get; private set; }


        public ColliderProxy(GameObject gameObject)
        {
            Agents = gameObject.GetComponents<Collider>();
        }

        public Action<object, Collision> OnCollisionEnter;
        public Action<object, Collision> OnCollisionStay;
        public Action<object, Collision> OnCollisionExit;

        public Action<object, Collider> OnTriggerEnter;
        public Action<object, Collider> OnTriggerStay;
        public Action<object, Collider> OnTriggerExit;

        /// <summary>
        /// 设置触发状态
        /// </summary>
        /// <param name="isTrigger"></param>
        /// <returns></returns>
        public ColliderProxy SetTrigger(bool isTrigger)
        {
            foreach (var item in Agents)
            {
                item.isTrigger = isTrigger;
            }
            return this;
        }

        /// <summary>
        /// 设置组件显隐状态
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public ColliderProxy SetEnable(bool enable)
        {
            foreach (var item in Agents)
            {
                item.enabled = enable;
            }
            return this;
        }

        public void Clear()
        {
            OnCollisionEnter = null;
            OnCollisionStay = null;
            OnCollisionExit = null;
            OnTriggerEnter = null;
            OnTriggerStay = null;
            OnTriggerExit = null;
        }
    }
}
