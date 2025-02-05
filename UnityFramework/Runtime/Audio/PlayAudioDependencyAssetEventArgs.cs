/*
* FileName:          PlayAudioDependencyAssetEventArgs
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
    /// 播放声音时加载依赖资源事件。
    /// </summary>
    public sealed class PlayAudioDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 播放声音时加载依赖资源事件编号。
        /// </summary>
        public static readonly int EventId = typeof(PlayAudioDependencyAssetEventArgs).GetHashCode();

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
            BindingEntity = null;
            UserData = null;
        }

        /// <summary>
        /// 获取播放声音时加载依赖资源事件编号。
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
        /// 创建播放声音时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的播放声音时加载依赖资源事件。</returns>
        public static PlayAudioDependencyAssetEventArgs Create(Framework.Audio.PlayAudioDependencyAssetEventArgs e)
        {
            PlayAudioInfo playAudioInfo = (PlayAudioInfo)e.UserData;
            PlayAudioDependencyAssetEventArgs playAudioDependencyAssetEventArgs = ReferencePool.Acquire<PlayAudioDependencyAssetEventArgs>();
            playAudioDependencyAssetEventArgs.SerialId = e.SerialId;
            playAudioDependencyAssetEventArgs.AudioAssetName = e.AudioAssetName;
            playAudioDependencyAssetEventArgs.AudioGroupName = e.AudioGroupName;
            playAudioDependencyAssetEventArgs.PlayAudioParams = e.PlayAudioParams;
            playAudioDependencyAssetEventArgs.DependencyAssetName = e.DependencyAssetName;
            playAudioDependencyAssetEventArgs.LoadedCount = e.LoadedCount;
            playAudioDependencyAssetEventArgs.TotalCount = e.TotalCount;
            playAudioDependencyAssetEventArgs.BindingEntity = playAudioInfo.BindingEntity;
            playAudioDependencyAssetEventArgs.UserData = playAudioInfo.UserData;
            return playAudioDependencyAssetEventArgs;
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
            BindingEntity = null;
            UserData = null;
        }
    }
}
