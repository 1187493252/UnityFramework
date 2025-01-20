/*
* FileName:          IAudioHelper
* CompanyName:       
* Author:            relly
* Description:       
*/

namespace Framework.Audio
{
    /// <summary>
    /// 声音辅助器接口。
    /// </summary>
    public interface IAudioHelper
    {
        /// <summary>
        /// 释放声音资源。
        /// </summary>
        /// <param name="audioAsset">要释放的声音资源。</param>
        void ReleaseAudioAsset(object audioAsset);
    }
}
