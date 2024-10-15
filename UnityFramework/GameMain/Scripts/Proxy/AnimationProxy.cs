/*
* FileName:          AnimationProxy
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
    [RequireComponent(typeof(Animation))]
    public class AnimationProxy : AnimProxy
    {
        private Animation agent;
        public Animation Agent
        {
            get
            {
                if (agent == null)
                {
                    agent = GetComponentInChildren<Animation>();
                    firstClip = agent.clip;
                }
                return agent;
            }
        }

        private AnimationClip firstClip;        //第一个动画

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
            Agent.Stop();

        }

        /// <summary>
        /// 暂停
        /// </summary>
        public override void Pause()
        {
            AnimationState animationState = Agent[Agent.clip.name];
            animationState.speed = 0;

        }

        /// <summary>
        /// 恢复
        /// </summary>
        public override void Resume()
        {
            AnimationState animationState = Agent[Agent.clip.name];
            animationState.speed = 1;
        }

        /// <summary>
        /// 重置动画状态
        /// </summary>
        public override void ResetAnimState()
        {
            AnimationState state = Agent[firstClip.name];
            Agent.Play(firstClip.name);
            state.time = 0;
            Agent.Sample();
            state.enabled = false;
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
            return Agent.GetClip(name);
        }

        public override float GetCurrentAnimLength()
        {
            return Agent.clip.length;
        }

        public override AnimationClip GetCurrentClip()
        {
            return Agent.clip;
        }

    }
}
