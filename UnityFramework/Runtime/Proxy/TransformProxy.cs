/*
* FileName:          TransformProxy
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
    public class TransformProxy : ComponentProxy<Transform>
    {
        private Transform originParent;         //初始父节点
        private Vector3 originPosition;         //初始位置
        private Vector3 originEuler;            //初始旋转
        private Vector3 originScale;            //初始缩放

        /// <summary>
        /// 构造函数 初始化Transform信息
        /// </summary>
        /// <param name="gameObject">游戏对象</param>
        public TransformProxy(GameObject gameObject) : base(gameObject)
        {
            originParent = Agent.parent;
            originPosition = Agent.position;
            originEuler = Agent.eulerAngles;
            originScale = Agent.localScale;
        }

        /// <summary>
        /// 重置Transform信息
        /// </summary>
        public void ResetTransform()
        {
            Agent.parent = originParent;
            Agent.position = originPosition;
            Agent.eulerAngles = originEuler;
            Agent.localScale = originScale;
        }
    }
}
