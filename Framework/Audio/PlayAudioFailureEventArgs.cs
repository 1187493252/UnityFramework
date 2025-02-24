﻿/*
* FileName:          PlayAudioFailureEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/


namespace Framework.Audio
{
    /// <summary>
    /// 播放声音失败事件。
    /// </summary>
    public sealed class PlayAudioFailureEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 初始化播放声音失败事件的新实例。
        /// </summary>
        public PlayAudioFailureEventArgs()
        {
            SerialId = 0;
            AudioAssetName = null;
            AudioGroupName = null;
            PlayAudioParams = null;
            ErrorCode = PlayAudioErrorCode.Unknown;
            ErrorMessage = null;
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
        /// <param name="serialId">声音的序列编号。</param>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <param name="errorCode">错误码。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的播放声音失败事件。</returns>
        public static PlayAudioFailureEventArgs Create(int serialId, string audioAssetName, string audioGroupName, PlayAudioParams playAudioParams, PlayAudioErrorCode errorCode, string errorMessage, object userData)
        {
            PlayAudioFailureEventArgs playSoundFailureEventArgs = ReferencePool.Acquire<PlayAudioFailureEventArgs>();
            playSoundFailureEventArgs.SerialId = serialId;
            playSoundFailureEventArgs.AudioAssetName = audioAssetName;
            playSoundFailureEventArgs.AudioGroupName = audioGroupName;
            playSoundFailureEventArgs.PlayAudioParams = playAudioParams;
            playSoundFailureEventArgs.ErrorCode = errorCode;
            playSoundFailureEventArgs.ErrorMessage = errorMessage;
            playSoundFailureEventArgs.UserData = userData;
            return playSoundFailureEventArgs;
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
            ErrorCode = PlayAudioErrorCode.Unknown;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
