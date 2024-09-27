using System;
using UnityEngine;
using UnityFramework.Runtime;

namespace UnityFramework.Runtime
{
    [DisallowMultipleComponent]
    public class AnimComponent : UnityFrameworkComponent
    {
        public Action AnimPlayEndEvent;
        public Action AnimPlayStartEvent;
        /// <summary>
        /// 获取Animator指定clip
        /// </summary>
        /// <param name="_animator"></param>
        /// <param name="_clipName"></param>
        /// <returns></returns>
        public AnimationClip GetAnimationClip(Animator _animator, string _clipName)
        {
            AnimationClip[] _clips = _animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].name == _clipName)
                {
                    return _clips[i];
                }
            }
            return null;
        }
        /// <summary>
        /// 给Animatorָ指定Clip指定时间点绑定事件
        /// </summary>
        /// <param name="_animator"></param>
        /// <param name="_clipName"></param>
        /// <param name="_eventFunctionName">事件名称,必须写在挂载到_animator物体上的脚本里</param>
        /// <param name="_time">时间0到1区间,0代表动画开头1代表动画末尾</param>
        public void AddAnimationEvent(Animator _animator, string _clipName, string _eventFunctionName, float _time)
        {
            AnimationClip[] _clips = _animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].name == _clipName)
                {
                    AnimationEvent _event = new AnimationEvent();
                    _event.functionName = _eventFunctionName;
                    _event.time = _clips[i].length * _time;
                    _clips[i].AddEvent(_event);
                    break;
                }
            }
        }
        /// <summary>
        /// 给Animatorָ指定Clip绑定动画结束事件
        /// </summary>
        /// <param name="_animator"></param>
        /// <param name="_clipName"></param>
        /// <param name="_eventFunctionName">事件名称,必须写在挂载到_animator物体上的脚本里</param>
        public void AddAnimationEndEvent(Animator _animator, string _clipName, string _eventFunctionName)
        {
            AnimationClip[] _clips = _animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].name == _clipName)
                {
                    AnimationEvent _event = new AnimationEvent();
                    _event.functionName = _eventFunctionName;
                    _event.time = _clips[i].length;
                    _clips[i].AddEvent(_event);
                    break;
                }
            }
        }

        /// <summary>
        /// 给Animatorָ指定Clip绑定动画开始事件
        /// </summary>
        /// <param name="_animator"></param>
        /// <param name="_clipName"></param>
        /// <param name="_eventFunctionName">事件名称,必须写在挂载到_animator物体上的脚本里</param>
        public void AddAnimationStartEvent(Animator _animator, string _clipName, string _eventFunctionName)
        {
            AnimationClip[] _clips = _animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].name == _clipName)
                {
                    AnimationEvent _event = new AnimationEvent();
                    _event.functionName = _eventFunctionName;
                    _event.time = 0;
                    _clips[i].AddEvent(_event);
                    break;
                }
            }
        }
        /// <summary>
        /// 给Animatorָ所有Clip绑定动画开始事件
        /// </summary>
        /// <param name="_animator"></param>
        /// <param name="_eventFunctionName">事件名称,必须写在挂载到_animator物体上的脚本里</param>
        public void AddAnimationStartEvent(Animator _animator, string _eventFunctionName)
        {
            AnimationClip[] _clips = _animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < _clips.Length; i++)
            {

                AnimationEvent _event = new AnimationEvent();
                _event.functionName = _eventFunctionName;
                _event.time = 0;
                _clips[i].AddEvent(_event);

            }
        }
        /// <summary>
        /// 给Animatorָ所有Clip绑定动画结束事件
        /// </summary>
        /// <param name="_animator"></param>
        /// <param name="_eventFunctionName">事件名称,必须写在挂载到_animator物体上的脚本里</param>
        public void AddAnimationEndEvent(Animator _animator, string _eventFunctionName)
        {
            AnimationClip[] _clips = _animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < _clips.Length; i++)
            {

                AnimationEvent _event = new AnimationEvent();
                _event.functionName = _eventFunctionName;
                _event.time = _clips[i].length;
                _clips[i].AddEvent(_event);

            }
        }
    }

}
