﻿/*
* FileName:          PlayAudioErrorCode
* CompanyName:       
* Author:            relly
* Description:       
*/

namespace Framework.Audio
{
    /// <summary>
    /// 播放声音错误码。
    /// </summary>
    public enum PlayAudioErrorCode : byte
    {
        /// <summary>
        /// 未知错误。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 声音组不存在。
        /// </summary>
        AudioGroupNotExist,

        /// <summary>
        /// 声音组没有声音代理。
        /// </summary>
        AudioGroupHasNoAgent,

        /// <summary>
        /// 加载资源失败。
        /// </summary>
        LoadAssetFailure,

        /// <summary>
        /// 播放声音因优先级低被忽略。
        /// </summary>
        IgnoredDueToLowPriority,

        /// <summary>
        /// 设置声音资源失败。
        /// </summary>
        SetAudioAssetFailure
    }
}
