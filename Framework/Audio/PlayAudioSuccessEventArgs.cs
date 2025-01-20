/*
* FileName:          PlayAudioSuccessEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

namespace Framework.Audio
{
    /// <summary>
    /// 播放声音成功事件。
    /// </summary>
    public sealed class PlayAudioSuccessEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 初始化播放声音成功事件的新实例。
        /// </summary>
        public PlayAudioSuccessEventArgs()
        {
            SerialId = 0;
            AudioAssetName = null;
            AudioAgent = null;
            Duration = 0f;
            UserData = null;
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
        /// <param name="serialId">声音的序列编号。</param>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioAgent">用于播放的声音代理。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的播放声音成功事件。</returns>
        public static PlayAudioSuccessEventArgs Create(int serialId, string audioAssetName, IAudioAgent audioAgent, float duration, object userData)
        {
            PlayAudioSuccessEventArgs playAudioSuccessEventArgs = ReferencePool.Acquire<PlayAudioSuccessEventArgs>();
            playAudioSuccessEventArgs.SerialId = serialId;
            playAudioSuccessEventArgs.AudioAssetName = audioAssetName;
            playAudioSuccessEventArgs.AudioAgent = audioAgent;
            playAudioSuccessEventArgs.Duration = duration;
            playAudioSuccessEventArgs.UserData = userData;
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
            UserData = null;
        }
    }
}
