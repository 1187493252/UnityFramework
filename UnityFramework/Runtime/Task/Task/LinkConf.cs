/*
* FileName:          LinkConf
* CompanyName:       
* Author:            
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime.Task
{
    public class LinkConf : ScriptableObject
    {
        /// <summary>
        /// 父节点，可以为空，代表属于根节点的任务
        /// </summary>
        [HideInInspector]
        public LinkConf Parent;

        /// <summary>
        /// 子任务节点，可以为空，代表此任务无子任务
        /// </summary>
        public LinkConf Child;

        /// <summary>
        /// 同级下一个任务，如果为空，代表同级不存在任务，属于同级任务的最后一个任务
        /// </summary>
        public LinkConf Next;

        /// <summary>
        /// 同级上一个任务，如果为空，代表此任务属于节点下第一个任务
        /// </summary>
        [HideInInspector]
        public LinkConf Last;
        /// <summary>
        /// 是否已经遍历过此节点
        /// </summary>
        public bool isRead = false;


        /// <summary>
        /// 获取下一个任务,下一个任务可能为空，当无下一个任务时，代表所有任务已经完成。
        /// </summary>
        public LinkConf NextNode
        {
            get
            {
                LinkConf next = null;
                if (Child && !isRead)
                {
                    isRead = true;
                    next = Child;
                    next.Parent = this;
                }
                else if (Next)
                {
                    next = Next;
                    next.Last = this;
                    next.Parent = this.Parent;
                }
                else if (Parent)
                {
                    return Parent.NextNode;
                }
                return next;
            }
        }

        public LinkConf NextBigNode
        {
            get
            {
                LinkConf next = null;
                if (Parent)
                {
                    return Parent.NextBigNode;
                }
                else if (Next)
                {
                    return Next;
                }
                return next;
            }
        }
        /// <summary>
        /// 初始化此任务，将此任务设置为未读取状态
        /// 因脚本配置文件会缓存属性，因此在读取任务之前需要先将任务设置为未读取状态
        /// </summary>
        public void InitTask()
        {
            isRead = false;
        }

    }
}