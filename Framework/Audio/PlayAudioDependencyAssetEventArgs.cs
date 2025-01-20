/*
* FileName:          PlayAudioDependencyAssetEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

namespace Framework.Audio
{
    /// <summary>
    /// 播放声音时加载依赖资源事件。
    /// </summary>
    public sealed class PlayAudioDependencyAssetEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 初始化播放声音时加载依赖资源事件的新实例。
        /// </summary>
        public PlayAudioDependencyAssetEventArgs()
        {
            SerialId = 0;
            AudioAssetName = null;
            AudioGroupName = null;
            PlayAudioParams = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
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
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount
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
        /// 创建播放声音时加载依赖资源事件。
        /// </summary>
        /// <param name="serialId">声音的序列编号。</param>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
        /// <param name="loadedCount">当前已加载依赖资源数量。</param>
        /// <param name="totalCount">总共加载依赖资源数量。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的播放声音时加载依赖资源事件。</returns>
        public static PlayAudioDependencyAssetEventArgs Create(int serialId, string audioAssetName, string audioGroupName, PlayAudioParams playAudioParams, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            PlayAudioDependencyAssetEventArgs playSoundDependencyAssetEventArgs = ReferencePool.Acquire<PlayAudioDependencyAssetEventArgs>();
            playSoundDependencyAssetEventArgs.SerialId = serialId;
            playSoundDependencyAssetEventArgs.AudioAssetName = audioAssetName;
            playSoundDependencyAssetEventArgs.AudioGroupName = audioGroupName;
            playSoundDependencyAssetEventArgs.PlayAudioParams = playAudioParams;
            playSoundDependencyAssetEventArgs.DependencyAssetName = dependencyAssetName;
            playSoundDependencyAssetEventArgs.LoadedCount = loadedCount;
            playSoundDependencyAssetEventArgs.TotalCount = totalCount;
            playSoundDependencyAssetEventArgs.UserData = userData;
            return playSoundDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理播放声音时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            AudioAssetName = null;
            AudioGroupName = null;
            PlayAudioParams = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
