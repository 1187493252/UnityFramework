/*
* FileName:          RigidbodyProxy
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
    public class RigidbodyProxy : ComponentProxy<Rigidbody>
    {
        private Rigidbody rigidbody;
        private bool originUseGravity;
        private bool originIsKinematic;


        /// <summary>
        /// 构造函数 初始化Transform信息
        /// </summary>
        /// <param name="gameObject">游戏对象</param>
        public RigidbodyProxy(GameObject gameObject) : base(gameObject)
        {
            originUseGravity = Agent.useGravity;
            originIsKinematic = Agent.isKinematic;
        }

        public void SetGravity(bool boolean)
        {
            Agent.useGravity = boolean;
        }

        public void SetKinematic(bool boolean)
        {
            Agent.isKinematic = boolean;
        }

        /// <summary>
        /// 重置Rigidbody信息
        /// </summary>
        public void ResetRigidbody()
        {
            Agent.useGravity = originUseGravity;
            Agent.isKinematic = originIsKinematic;
        }
    }
}
