/*
* FileName:          PlayAudioFailureEventArgs
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
    /// 播放声音失败事件。
    /// </summary>
    public sealed class PlayAudioFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 播放声音失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PlayAudioFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化播放声音失败事件的新实例。
        /// </summary>
        public PlayAudioFailureEventArgs()
        {
            SerialId = 0;
            AudioAssetName = null;
            AudioGroupName = null;
            PlayAudioParams = null;
            BindingEntity = null;
            ErrorCode = 0;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取播放声音失败事件编号。
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
        /// 获取声音绑定的实体。
        /// </summary>
        public Entity BindingEntity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误码。
        /// </summary>
        public PlayAudioErrorCode ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
        /// 创建播放声音失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的播放声音失败事件。</returns>
        public static PlayAudioFailureEventArgs Create(Framework.Audio.PlayAudioFailureEventArgs e)
        {
            PlayAudioInfo playAudioInfo = (PlayAudioInfo)e.UserData;
            PlayAudioFailureEventArgs playAudioFailureEventArgs = ReferencePool.Acquire<PlayAudioFailureEventArgs>();
            playAudioFailureEventArgs.SerialId = e.SerialId;
            playAudioFailureEventArgs.AudioAssetName = e.AudioAssetName;
            playAudioFailureEventArgs.AudioGroupName = e.AudioGroupName;
            playAudioFailureEventArgs.PlayAudioParams = e.PlayAudioParams;
            playAudioFailureEventArgs.BindingEntity = playAudioInfo.BindingEntity;
            playAudioFailureEventArgs.ErrorCode = e.ErrorCode;
            playAudioFailureEventArgs.ErrorMessage = e.ErrorMessage;
            playAudioFailureEventArgs.UserData = playAudioInfo.UserData;
            ReferencePool.Release(playAudioInfo);
            return playAudioFailureEventArgs;
        }

        /// <summary>
        /// 清理播放声音失败事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            AudioAssetName = null;
            AudioGroupName = null;
            PlayAudioParams = null;
            BindingEntity = null;
            ErrorCode = 0;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
