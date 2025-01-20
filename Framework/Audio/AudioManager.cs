/*
* FileName:          AudioManager
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections.Generic;
using Framework.Resource;

namespace Framework.Audio
{
    internal sealed partial class AudioManager : FrameworkModule, IAudioManager
    {
        private readonly Dictionary<string, AudioGroup> m_AudioGroups;
        private readonly List<int> m_AudiosBeingLoaded;
        private readonly HashSet<int> m_AudiosToReleaseOnLoad;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IResourceManager m_ResourceManager;
        private IAudioHelper m_AudioHelper;
        private int m_Serial;
        private EventHandler<PlayAudioSuccessEventArgs> m_PlayAudioSuccessEventHandler;
        private EventHandler<PlayAudioFailureEventArgs> m_PlayAudioFailureEventHandler;
        private EventHandler<PlayAudioUpdateEventArgs> m_PlayAudioUpdateEventHandler;
        private EventHandler<PlayAudioDependencyAssetEventArgs> m_PlayAudioDependencyAssetEventHandler;

        /// <summary>
        /// 初始化声音管理器的新实例。
        /// </summary>
        public AudioManager()
        {
            m_AudioGroups = new Dictionary<string, AudioGroup>(StringComparer.Ordinal);
            m_AudiosBeingLoaded = new List<int>();
            m_AudiosToReleaseOnLoad = new HashSet<int>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback, LoadAssetUpdateCallback, LoadAssetDependencyAssetCallback);
            m_ResourceManager = null;
            m_AudioHelper = null;
            m_Serial = 0;
            m_PlayAudioSuccessEventHandler = null;
            m_PlayAudioFailureEventHandler = null;
            m_PlayAudioUpdateEventHandler = null;
            m_PlayAudioDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取声音组数量。
        /// </summary>
        public int AudioGroupCount
        {
            get
            {
                return m_AudioGroups.Count;
            }
        }

        /// <summary>
        /// 播放声音成功事件。
        /// </summary>
        public event EventHandler<PlayAudioSuccessEventArgs> PlayAudioSuccess
        {
            add
            {
                m_PlayAudioSuccessEventHandler += value;
            }
            remove
            {
                m_PlayAudioSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 播放声音失败事件。
        /// </summary>
        public event EventHandler<PlayAudioFailureEventArgs> PlayAudioFailure
        {
            add
            {
                m_PlayAudioFailureEventHandler += value;
            }
            remove
            {
                m_PlayAudioFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 播放声音更新事件。
        /// </summary>
        public event EventHandler<PlayAudioUpdateEventArgs> PlayAudioUpdate
        {
            add
            {
                m_PlayAudioUpdateEventHandler += value;
            }
            remove
            {
                m_PlayAudioUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 播放声音时加载依赖资源事件。
        /// </summary>
        public event EventHandler<PlayAudioDependencyAssetEventArgs> PlayAudioDependencyAsset
        {
            add
            {
                m_PlayAudioDependencyAssetEventHandler += value;
            }
            remove
            {
                m_PlayAudioDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 声音管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理声音管理器。
        /// </summary>
        internal override void Shutdown()
        {
            StopAllLoadedAudios();
            m_AudioGroups.Clear();
            m_AudiosBeingLoaded.Clear();
            m_AudiosToReleaseOnLoad.Clear();
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new FrameworkException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置声音辅助器。
        /// </summary>
        /// <param name="AudioHelper">声音辅助器。</param>
        public void SetAudioHelper(IAudioHelper AudioHelper)
        {
            if (AudioHelper == null)
            {
                throw new FrameworkException("Audio helper is invalid.");
            }

            m_AudioHelper = AudioHelper;
        }

        /// <summary>
        /// 是否存在指定声音组。
        /// </summary>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <returns>指定声音组是否存在。</returns>
        public bool HasAudioGroup(string AudioGroupName)
        {
            if (string.IsNullOrEmpty(AudioGroupName))
            {
                throw new FrameworkException("Audio group name is invalid.");
            }

            return m_AudioGroups.ContainsKey(AudioGroupName);
        }

        /// <summary>
        /// 获取指定声音组。
        /// </summary>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <returns>要获取的声音组。</returns>
        public IAudioGroup GetAudioGroup(string AudioGroupName)
        {
            if (string.IsNullOrEmpty(AudioGroupName))
            {
                throw new FrameworkException("Audio group name is invalid.");
            }

            AudioGroup AudioGroup = null;
            if (m_AudioGroups.TryGetValue(AudioGroupName, out AudioGroup))
            {
                return AudioGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有声音组。
        /// </summary>
        /// <returns>所有声音组。</returns>
        public IAudioGroup[] GetAllAudioGroups()
        {
            int index = 0;
            IAudioGroup[] results = new IAudioGroup[m_AudioGroups.Count];
            foreach (KeyValuePair<string, AudioGroup> AudioGroup in m_AudioGroups)
            {
                results[index++] = AudioGroup.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有声音组。
        /// </summary>
        /// <param name="results">所有声音组。</param>
        public void GetAllAudioGroups(List<IAudioGroup> results)
        {
            if (results == null)
            {
                throw new FrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, AudioGroup> AudioGroup in m_AudioGroups)
            {
                results.Add(AudioGroup.Value);
            }
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <param name="AudioGroupHelper">声音组辅助器。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddAudioGroup(string AudioGroupName, IAudioGroupHelper AudioGroupHelper)
        {
            return AddAudioGroup(AudioGroupName, false, Constant.DefaultMute, Constant.DefaultVolume, AudioGroupHelper);
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <param name="AudioGroupAvoidBeingReplacedBySamePriority">声音组中的声音是否避免被同优先级声音替换。</param>
        /// <param name="AudioGroupMute">声音组是否静音。</param>
        /// <param name="AudioGroupVolume">声音组音量。</param>
        /// <param name="AudioGroupHelper">声音组辅助器。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddAudioGroup(string AudioGroupName, bool AudioGroupAvoidBeingReplacedBySamePriority, bool AudioGroupMute, float AudioGroupVolume, IAudioGroupHelper AudioGroupHelper)
        {
            if (string.IsNullOrEmpty(AudioGroupName))
            {
                throw new FrameworkException("Audio group name is invalid.");
            }

            if (AudioGroupHelper == null)
            {
                throw new FrameworkException("Audio group helper is invalid.");
            }

            if (HasAudioGroup(AudioGroupName))
            {
                return false;
            }

            AudioGroup AudioGroup = new AudioGroup(AudioGroupName, AudioGroupHelper)
            {
                AvoidBeingReplacedBySamePriority = AudioGroupAvoidBeingReplacedBySamePriority,
                Mute = AudioGroupMute,
                Volume = AudioGroupVolume
            };

            m_AudioGroups.Add(AudioGroupName, AudioGroup);

            return true;
        }

        /// <summary>
        /// 增加声音代理辅助器。
        /// </summary>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <param name="AudioAgentHelper">要增加的声音代理辅助器。</param>
        public void AddAudioAgentHelper(string AudioGroupName, IAudioAgentHelper AudioAgentHelper)
        {
            if (m_AudioHelper == null)
            {
                throw new FrameworkException("You must set Audio helper first.");
            }

            AudioGroup AudioGroup = (AudioGroup)GetAudioGroup(AudioGroupName);
            if (AudioGroup == null)
            {
                throw new FrameworkException(Utility.Text.Format("Audio group '{0}' is not exist.", AudioGroupName));
            }

            AudioGroup.AddAudioAgentHelper(m_AudioHelper, AudioAgentHelper);
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号。
        /// </summary>
        /// <returns>所有正在加载声音的序列编号。</returns>
        public int[] GetAllLoadingAudioSerialIds()
        {
            return m_AudiosBeingLoaded.ToArray();
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号。
        /// </summary>
        /// <param name="results">所有正在加载声音的序列编号。</param>
        public void GetAllLoadingAudioSerialIds(List<int> results)
        {
            if (results == null)
            {
                throw new FrameworkException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_AudiosBeingLoaded);
        }

        /// <summary>
        /// 是否正在加载声音。
        /// </summary>
        /// <param name="serialId">声音序列编号。</param>
        /// <returns>是否正在加载声音。</returns>
        public bool IsLoadingAudio(int serialId)
        {
            return m_AudiosBeingLoaded.Contains(serialId);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="AudioAssetName">声音资源名称。</param>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string AudioAssetName, string AudioGroupName)
        {
            return PlayAudio(AudioAssetName, AudioGroupName, Resource.Constant.DefaultPriority, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="AudioAssetName">声音资源名称。</param>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string AudioAssetName, string AudioGroupName, int priority)
        {
            return PlayAudio(AudioAssetName, AudioGroupName, priority, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="AudioAssetName">声音资源名称。</param>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string AudioAssetName, string AudioGroupName, PlayAudioParams playAudioParams)
        {
            return PlayAudio(AudioAssetName, AudioGroupName, Resource.Constant.DefaultPriority, playAudioParams, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="AudioAssetName">声音资源名称。</param>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string AudioAssetName, string AudioGroupName, object userData)
        {
            return PlayAudio(AudioAssetName, AudioGroupName, Resource.Constant.DefaultPriority, null, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="AudioAssetName">声音资源名称。</param>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string AudioAssetName, string AudioGroupName, int priority, PlayAudioParams playAudioParams)
        {
            return PlayAudio(AudioAssetName, AudioGroupName, priority, playAudioParams, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="AudioAssetName">声音资源名称。</param>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string AudioAssetName, string AudioGroupName, int priority, object userData)
        {
            return PlayAudio(AudioAssetName, AudioGroupName, priority, null, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="AudioAssetName">声音资源名称。</param>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string AudioAssetName, string AudioGroupName, PlayAudioParams playAudioParams, object userData)
        {
            return PlayAudio(AudioAssetName, AudioGroupName, Resource.Constant.DefaultPriority, playAudioParams, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="AudioAssetName">声音资源名称。</param>
        /// <param name="AudioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string AudioAssetName, string AudioGroupName, int priority, PlayAudioParams playAudioParams, object userData)
        {
            if (m_ResourceManager == null)
            {
                throw new FrameworkException("You must set resource manager first.");
            }

            if (m_AudioHelper == null)
            {
                throw new FrameworkException("You must set Audio helper first.");
            }

            if (playAudioParams == null)
            {
                playAudioParams = PlayAudioParams.Create();
            }

            int serialId = ++m_Serial;
            PlayAudioErrorCode? errorCode = null;
            string errorMessage = null;
            AudioGroup AudioGroup = (AudioGroup)GetAudioGroup(AudioGroupName);
            if (AudioGroup == null)
            {
                errorCode = PlayAudioErrorCode.AudioGroupNotExist;
                errorMessage = Utility.Text.Format("Audio group '{0}' is not exist.", AudioGroupName);
            }
            else if (AudioGroup.AudioAgentCount <= 0)
            {
                errorCode = PlayAudioErrorCode.AudioGroupHasNoAgent;
                errorMessage = Utility.Text.Format("Audio group '{0}' is have no Audio agent.", AudioGroupName);
            }

            if (errorCode.HasValue)
            {
                if (m_PlayAudioFailureEventHandler != null)
                {
                    PlayAudioFailureEventArgs playAudioFailureEventArgs = PlayAudioFailureEventArgs.Create(serialId, AudioAssetName, AudioGroupName, playAudioParams, errorCode.Value, errorMessage, userData);
                    m_PlayAudioFailureEventHandler(this, playAudioFailureEventArgs);
                    ReferencePool.Release(playAudioFailureEventArgs);

                    if (playAudioParams.Referenced)
                    {
                        ReferencePool.Release(playAudioParams);
                    }

                    return serialId;
                }

                throw new FrameworkException(errorMessage);
            }

            m_AudiosBeingLoaded.Add(serialId);
            m_ResourceManager.LoadAsset(AudioAssetName, priority, m_LoadAssetCallbacks, PlayAudioInfo.Create(serialId, AudioGroup, playAudioParams, userData));
            return serialId;
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopAudio(int serialId)
        {
            return StopAudio(serialId, Constant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopAudio(int serialId, float fadeOutSeconds)
        {
            if (IsLoadingAudio(serialId))
            {
                m_AudiosToReleaseOnLoad.Add(serialId);
                m_AudiosBeingLoaded.Remove(serialId);
                return true;
            }

            foreach (KeyValuePair<string, AudioGroup> AudioGroup in m_AudioGroups)
            {
                if (AudioGroup.Value.StopAudio(serialId, fadeOutSeconds))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 停止所有已加载的声音。
        /// </summary>
        public void StopAllLoadedAudios()
        {
            StopAllLoadedAudios(Constant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 停止所有已加载的声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void StopAllLoadedAudios(float fadeOutSeconds)
        {
            foreach (KeyValuePair<string, AudioGroup> AudioGroup in m_AudioGroups)
            {
                AudioGroup.Value.StopAllLoadedAudios(fadeOutSeconds);
            }
        }

        /// <summary>
        /// 停止所有正在加载的声音。
        /// </summary>
        public void StopAllLoadingAudios()
        {
            foreach (int serialId in m_AudiosBeingLoaded)
            {
                m_AudiosToReleaseOnLoad.Add(serialId);
            }
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="serialId">要暂停播放声音的序列编号。</param>
        public void PauseAudio(int serialId)
        {
            PauseAudio(serialId, Constant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="serialId">要暂停播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void PauseAudio(int serialId, float fadeOutSeconds)
        {
            foreach (KeyValuePair<string, AudioGroup> AudioGroup in m_AudioGroups)
            {
                if (AudioGroup.Value.PauseAudio(serialId, fadeOutSeconds))
                {
                    return;
                }
            }

            throw new FrameworkException(Utility.Text.Format("Can not find Audio '{0}'.", serialId.ToString()));
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="serialId">要恢复播放声音的序列编号。</param>
        public void ResumeAudio(int serialId)
        {
            ResumeAudio(serialId, Constant.DefaultFadeInSeconds);
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="serialId">要恢复播放声音的序列编号。</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        public void ResumeAudio(int serialId, float fadeInSeconds)
        {
            foreach (KeyValuePair<string, AudioGroup> AudioGroup in m_AudioGroups)
            {
                if (AudioGroup.Value.ResumeAudio(serialId, fadeInSeconds))
                {
                    return;
                }
            }

            throw new FrameworkException(Utility.Text.Format("Can not find Audio '{0}'.", serialId.ToString()));
        }

        private void LoadAssetSuccessCallback(string AudioAssetName, object AudioAsset, float duration, object userData)
        {
            PlayAudioInfo playAudioInfo = (PlayAudioInfo)userData;
            if (playAudioInfo == null)
            {
                throw new FrameworkException("Play Audio info is invalid.");
            }

            if (m_AudiosToReleaseOnLoad.Contains(playAudioInfo.SerialId))
            {
                m_AudiosToReleaseOnLoad.Remove(playAudioInfo.SerialId);
                if (playAudioInfo.PlayAudioParams.Referenced)
                {
                    ReferencePool.Release(playAudioInfo.PlayAudioParams);
                }

                ReferencePool.Release(playAudioInfo);
                m_AudioHelper.ReleaseAudioAsset(AudioAsset);
                return;
            }

            m_AudiosBeingLoaded.Remove(playAudioInfo.SerialId);

            PlayAudioErrorCode? errorCode = null;
            IAudioAgent AudioAgent = playAudioInfo.AudioGroup.PlayAudio(playAudioInfo.SerialId, AudioAsset, playAudioInfo.PlayAudioParams, out errorCode);
            if (AudioAgent != null)
            {
                if (m_PlayAudioSuccessEventHandler != null)
                {
                    PlayAudioSuccessEventArgs playAudioSuccessEventArgs = PlayAudioSuccessEventArgs.Create(playAudioInfo.SerialId, AudioAssetName, AudioAgent, duration, playAudioInfo.UserData);
                    m_PlayAudioSuccessEventHandler(this, playAudioSuccessEventArgs);
                    ReferencePool.Release(playAudioSuccessEventArgs);
                }

                if (playAudioInfo.PlayAudioParams.Referenced)
                {
                    ReferencePool.Release(playAudioInfo.PlayAudioParams);
                }

                ReferencePool.Release(playAudioInfo);
                return;
            }

            m_AudiosToReleaseOnLoad.Remove(playAudioInfo.SerialId);
            m_AudioHelper.ReleaseAudioAsset(AudioAsset);
            string errorMessage = Utility.Text.Format("Audio group '{0}' play Audio '{1}' failure.", playAudioInfo.AudioGroup.Name, AudioAssetName);
            if (m_PlayAudioFailureEventHandler != null)
            {
                PlayAudioFailureEventArgs playAudioFailureEventArgs = PlayAudioFailureEventArgs.Create(playAudioInfo.SerialId, AudioAssetName, playAudioInfo.AudioGroup.Name, playAudioInfo.PlayAudioParams, errorCode.Value, errorMessage, playAudioInfo.UserData);
                m_PlayAudioFailureEventHandler(this, playAudioFailureEventArgs);
                ReferencePool.Release(playAudioFailureEventArgs);

                if (playAudioInfo.PlayAudioParams.Referenced)
                {
                    ReferencePool.Release(playAudioInfo.PlayAudioParams);
                }

                ReferencePool.Release(playAudioInfo);
                return;
            }

            if (playAudioInfo.PlayAudioParams.Referenced)
            {
                ReferencePool.Release(playAudioInfo.PlayAudioParams);
            }

            ReferencePool.Release(playAudioInfo);
            throw new FrameworkException(errorMessage);
        }

        private void LoadAssetFailureCallback(string AudioAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            PlayAudioInfo playAudioInfo = (PlayAudioInfo)userData;
            if (playAudioInfo == null)
            {
                throw new FrameworkException("Play Audio info is invalid.");
            }

            if (m_AudiosToReleaseOnLoad.Contains(playAudioInfo.SerialId))
            {
                m_AudiosToReleaseOnLoad.Remove(playAudioInfo.SerialId);
                if (playAudioInfo.PlayAudioParams.Referenced)
                {
                    ReferencePool.Release(playAudioInfo.PlayAudioParams);
                }

                return;
            }

            m_AudiosBeingLoaded.Remove(playAudioInfo.SerialId);
            string appendErrorMessage = Utility.Text.Format("Load Audio failure, asset name '{0}', status '{1}', error message '{2}'.", AudioAssetName, status.ToString(), errorMessage);
            if (m_PlayAudioFailureEventHandler != null)
            {
                PlayAudioFailureEventArgs playAudioFailureEventArgs = PlayAudioFailureEventArgs.Create(playAudioInfo.SerialId, AudioAssetName, playAudioInfo.AudioGroup.Name, playAudioInfo.PlayAudioParams, PlayAudioErrorCode.LoadAssetFailure, appendErrorMessage, playAudioInfo.UserData);
                m_PlayAudioFailureEventHandler(this, playAudioFailureEventArgs);
                ReferencePool.Release(playAudioFailureEventArgs);

                if (playAudioInfo.PlayAudioParams.Referenced)
                {
                    ReferencePool.Release(playAudioInfo.PlayAudioParams);
                }

                return;
            }

            throw new FrameworkException(appendErrorMessage);
        }

        private void LoadAssetUpdateCallback(string AudioAssetName, float progress, object userData)
        {
            PlayAudioInfo playAudioInfo = (PlayAudioInfo)userData;
            if (playAudioInfo == null)
            {
                throw new FrameworkException("Play Audio info is invalid.");
            }

            if (m_PlayAudioUpdateEventHandler != null)
            {
                PlayAudioUpdateEventArgs playAudioUpdateEventArgs = PlayAudioUpdateEventArgs.Create(playAudioInfo.SerialId, AudioAssetName, playAudioInfo.AudioGroup.Name, playAudioInfo.PlayAudioParams, progress, playAudioInfo.UserData);
                m_PlayAudioUpdateEventHandler(this, playAudioUpdateEventArgs);
                ReferencePool.Release(playAudioUpdateEventArgs);
            }
        }

        private void LoadAssetDependencyAssetCallback(string AudioAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            PlayAudioInfo playAudioInfo = (PlayAudioInfo)userData;
            if (playAudioInfo == null)
            {
                throw new FrameworkException("Play Audio info is invalid.");
            }

            if (m_PlayAudioDependencyAssetEventHandler != null)
            {
                PlayAudioDependencyAssetEventArgs playAudioDependencyAssetEventArgs = PlayAudioDependencyAssetEventArgs.Create(playAudioInfo.SerialId, AudioAssetName, playAudioInfo.AudioGroup.Name, playAudioInfo.PlayAudioParams, dependencyAssetName, loadedCount, totalCount, playAudioInfo.UserData);
                m_PlayAudioDependencyAssetEventHandler(this, playAudioDependencyAssetEventArgs);
                ReferencePool.Release(playAudioDependencyAssetEventArgs);
            }
        }

    }
}
