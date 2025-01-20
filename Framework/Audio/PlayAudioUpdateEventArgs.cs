/*
* FileName:          PlayAudioUpdateEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/


namespace Framework.Audio
{
    /// <summary>
    /// 播放声音更新事件。
    /// </summary>
    public sealed class PlayAudioUpdateEventArgs : FrameworkEventArgs
    {
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
        /// 获取加载声音进度。
        /// </summary>
        public float Progress
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
        /// <param name="serialId">声音的序列编号。</param>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <param name="progress">加载声音进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的播放声音更新事件。</returns>
        public static PlayAudioUpdateEventArgs Create(int serialId, string audioAssetName, string audioGroupName, PlayAudioParams playAudioParams, float progress, object userData)
        {
            PlayAudioUpdateEventArgs playSoundUpdateEventArgs = ReferencePool.Acquire<PlayAudioUpdateEventArgs>();
            playSoundUpdateEventArgs.SerialId = serialId;
            playSoundUpdateEventArgs.AudioAssetName = audioAssetName;
            playSoundUpdateEventArgs.AudioGroupName = audioGroupName;
            playSoundUpdateEventArgs.PlayAudioParams = playAudioParams;
            playSoundUpdateEventArgs.Progress = progress;
            playSoundUpdateEventArgs.UserData = userData;
            return playSoundUpdateEventArgs;
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
            UserData = null;
        }
    }
}
