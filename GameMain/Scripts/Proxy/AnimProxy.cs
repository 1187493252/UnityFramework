/*
* FileName:          AnimProxy
* CompanyName:       
* Author:            relly
* Description:       动画代理
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public abstract class AnimProxy : MonoBehaviour
    {

        /// <summary>
        /// 播放
        /// </summary>
        public virtual void Play(string name)
        {

        }

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop()
        {

        }

        /// <summary>
        /// 暂停
        /// </summary>
        public virtual void Pause()
        {

        }

        /// <summary>
        /// 恢复
        /// </summary>
        public virtual void Resume()
        {

        }

        /// <summary>
        /// 重置动画状态
        /// </summary>
        public virtual void ResetAnimState()
        {

        }

        /// <summary>
        /// 获取动画长度
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract float GetClipLength(string name);

        /// <summary>
        /// 获取动画
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract AnimationClip GetClip(string name);

        public abstract float GetCurrentAnimLength();

        public abstract AnimationClip GetCurrentClip();

    }
}
