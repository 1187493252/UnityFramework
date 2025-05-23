﻿/*
* FileName:          PlayAudioUpdateEventArgs
* CompanyName:  
* Author:            relly
* Description:       
*                    
*/

using Framework;
using Framework.Audio;
using Framework.Event;


namespace UnityFramework.Runtime
{
    /// <summary>
    /// 播放声音更新事件。
    /// </summary>
    public sealed class PlayAudioUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 播放声音更新事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PlayAudioUpdateEventArgs).GetHashCode();

        /// <summary>
        /// 初始化播放声音更新事件的新实例。
        /// </summary>
        public PlayAudioUpdateEventArgs()
        {
            SerialId = 0;
            AudioAssetName = null;
            AudioGroupName = null;
            PlayAudioParams = null;
            Progress = 0f;
            BindingEntity = null;
            UserData = null;
        }

        /// <summary>
        /// 获取播放声音更新事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取声音的序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音资源名称。
        /// </summary>
        public string AudioAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音组名称。
        /// </summary>
        public string AudioGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取播放声音参数。
        /// </summary>
        public PlayAudioParams PlayAudioParams
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载声音进度。
        /// </summary>
        public float Progress
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音绑定的实体。
        /// </summary>
        public Entity BindingEntity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建播放声音更新事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的播放声音更新事件。</returns>
        public static PlayAudioUpdateEventArgs Create(Framework.Audio.PlayAudioUpdateEventArgs e)
        {
            PlayAudioInfo playAudioInfo = (PlayAudioInfo)e.UserData;
            PlayAudioUpdateEventArgs playAudioUpdateEventArgs = ReferencePool.Acquire<PlayAudioUpdateEventArgs>();
            playAudioUpdateEventArgs.SerialId = e.SerialId;
            playAudioUpdateEventArgs.AudioAssetName = e.AudioAssetName;
            playAudioUpdateEventArgs.AudioGroupName = e.AudioGroupName;
            playAudioUpdateEventArgs.PlayAudioParams = e.PlayAudioParams;
            playAudioUpdateEventArgs.Progress = e.Progress;
            playAudioUpdateEventArgs.BindingEntity = playAudioInfo.BindingEntity;
            playAudioUpdateEventArgs.UserData = playAudioInfo.UserData;
            return playAudioUpdateEventArgs;
        }

        /// <summary>
        /// 清理播放声音更新事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            AudioAssetName = null;
            AudioGroupName = null;
            PlayAudioParams = null;
            Progress = 0f;
            BindingEntity = null;
            UserData = null;
        }
    }
}
