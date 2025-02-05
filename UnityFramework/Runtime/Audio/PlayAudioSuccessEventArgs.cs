/*
* FileName:          PlayAudioSuccessEventArgs
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
    /// 播放声音成功事件。
    /// </summary>
    public sealed class PlayAudioSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 播放声音成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PlayAudioSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化播放声音成功事件的新实例。
        /// </summary>
        public PlayAudioSuccessEventArgs()
        {
            SerialId = 0;
            AudioAssetName = null;
            AudioAgent = null;
            Duration = 0f;
            BindingEntity = null;
            UserData = null;
        }

        /// <summary>
        /// 获取播放声音成功事件编号。
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
        /// 获取用于播放的声音代理。
        /// </summary>
        public IAudioAgent AudioAgent
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
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
        /// 创建播放声音成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的播放声音成功事件。</returns>
        public static PlayAudioSuccessEventArgs Create(Framework.Audio.PlayAudioSuccessEventArgs e)
        {
            PlayAudioInfo playAudioInfo = (PlayAudioInfo)e.UserData;
            PlayAudioSuccessEventArgs playAudioSuccessEventArgs = ReferencePool.Acquire<PlayAudioSuccessEventArgs>();
            playAudioSuccessEventArgs.SerialId = e.SerialId;
            playAudioSuccessEventArgs.AudioAssetName = e.AudioAssetName;
            playAudioSuccessEventArgs.AudioAgent = e.AudioAgent;
            playAudioSuccessEventArgs.Duration = e.Duration;
            playAudioSuccessEventArgs.BindingEntity = playAudioInfo.BindingEntity;
            playAudioSuccessEventArgs.UserData = playAudioInfo.UserData;
            ReferencePool.Release(playAudioInfo);
            return playAudioSuccessEventArgs;
        }

        /// <summary>
        /// 清理播放声音成功事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            AudioAssetName = null;
            AudioAgent = null;
            Duration = 0f;
            BindingEntity = null;
            UserData = null;
        }
    }
}
