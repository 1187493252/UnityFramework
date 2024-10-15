/*
* FileName:          AnimatorProxy
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
    [DisallowMultipleComponent()]
    [RequireComponent(typeof(Animator))]
    public class AnimatorProxy : AnimProxy
    {
        private Animator agent;
        public Animator Agent
        {
            get
            {
                if (agent == null)
                {
                    agent = GetComponentInChildren<Animator>();
                }
                return agent;
            }
        }


        /// <summary>
        /// 播放
        /// </summary>
        public override void Play(string name)
        {
            Agent.Play(name);
        }

        /// <summary>
        /// 停止
        /// </summary>
        public override void Stop()
        {
            Agent.StopPlayback();

        }

        /// <summary>
        /// 暂停
        /// </summary>
        public override void Pause()
        {
            Agent.speed = 0;

        }

        /// <summary>
        /// 恢复
        /// </summary>
        public override void Resume()
        {
            Agent.speed = 1;
        }

        /// <summary>
        /// 重置动画状态
        /// </summary>
        public override void ResetAnimState()
        {
            Agent.Play("New State", -1, 0f);
            Agent.Update(0f);
        }

        /// <summary>
        /// 获取动画长度
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override float GetClipLength(string name)
        {
            return GetClip(name).length;
        }

        /// <summary>
        /// 获取动画
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override AnimationClip GetClip(string name)
        {
            AnimationClip[] clips = Agent.runtimeAnimatorController.animationClips;
            return clips.First(x => x.name == name);
        }

        public override float GetCurrentAnimLength()
        {
            return GetCurrentClip().length;
        }

        public override AnimationClip GetCurrentClip()
        {
            return Agent.GetCurrentAnimatorClipInfo(0)[0].clip;
        }

    }
}
