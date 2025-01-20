/*
* FileName:          AudioGroup
* CompanyName:       
* Author:            relly
* Description:       
*/

using System.Collections.Generic;

namespace Framework.Audio
{
    internal sealed partial class AudioManager : FrameworkModule, IAudioManager
    {

        private sealed class AudioGroup : IAudioGroup
        {

            private readonly string m_Name;
            private readonly IAudioGroupHelper m_AudioGroupHelper;
            private readonly List<AudioAgent> m_AudioAgents;
            private bool m_AvoidBeingReplacedBySamePriority;
            private bool m_Mute;
            private float m_Volume;

            /// <summary>
            /// 初始化声音组的新实例。
            /// </summary>
            /// <param name="name">声音组名称。</param>
            /// <param name="audioGroupHelper">声音组辅助器。</param>
            public AudioGroup(string name, IAudioGroupHelper audioGroupHelper)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new FrameworkException("Audio group name is invalid.");
                }

                if (audioGroupHelper == null)
                {
                    throw new FrameworkException("Audio group helper is invalid.");
                }

                m_Name = name;
                m_AudioGroupHelper = audioGroupHelper;
                m_AudioAgents = new List<AudioAgent>();
            }

            /// <summary>
            /// 获取声音组名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取声音代理数。
            /// </summary>
            public int AudioAgentCount
            {
                get
                {
                    return m_AudioAgents.Count;
                }
            }

            /// <summary>
            /// 获取或设置声音组中的声音是否避免被同优先级声音替换。
            /// </summary>
            public bool AvoidBeingReplacedBySamePriority
            {
                get
                {
                    return m_AvoidBeingReplacedBySamePriority;
                }
                set
                {
                    m_AvoidBeingReplacedBySamePriority = value;
                }
            }

            /// <summary>
            /// 获取或设置声音组静音。
            /// </summary>
            public bool Mute
            {
                get
                {
                    return m_Mute;
                }
                set
                {
                    m_Mute = value;
                    foreach (AudioAgent audioAgent in m_AudioAgents)
                    {
                        audioAgent.RefreshMute();
                    }
                }
            }

            /// <summary>
            /// 获取或设置声音组音量。
            /// </summary>
            public float Volume
            {
                get
                {
                    return m_Volume;
                }
                set
                {
                    m_Volume = value;
                    foreach (AudioAgent audioAgent in m_AudioAgents)
                    {
                        audioAgent.RefreshVolume();
                    }
                }
            }

            /// <summary>
            /// 获取声音组辅助器。
            /// </summary>
            public IAudioGroupHelper Helper
            {
                get
                {
                    return m_AudioGroupHelper;
                }
            }

            /// <summary>
            /// 增加声音代理辅助器。
            /// </summary>
            /// <param name="audioHelper">声音辅助器接口。</param>
            /// <param name="audioAgentHelper">要增加的声音代理辅助器。</param>
            public void AddAudioAgentHelper(IAudioHelper audioHelper, IAudioAgentHelper audioAgentHelper)
            {
                m_AudioAgents.Add(new AudioAgent(this, audioHelper, audioAgentHelper));
            }

            /// <summary>
            /// 播放声音。
            /// </summary>
            /// <param name="serialId">声音的序列编号。</param>
            /// <param name="audioAsset">声音资源。</param>
            /// <param name="playAudioParams">播放声音参数。</param>
            /// <param name="errorCode">错误码。</param>
            /// <returns>用于播放的声音代理。</returns>
            public IAudioAgent PlayAudio(int serialId, object audioAsset, PlayAudioParams playAudioParams, out PlayAudioErrorCode? errorCode)
            {
                errorCode = null;
                AudioAgent candidateAgent = null;
                foreach (AudioAgent audioAgent in m_AudioAgents)
                {
                    if (!audioAgent.IsPlaying)
                    {
                        candidateAgent = audioAgent;
                        break;
                    }

                    if (audioAgent.Priority < playAudioParams.Priority)
                    {
                        if (candidateAgent == null || audioAgent.Priority < candidateAgent.Priority)
                        {
                            candidateAgent = audioAgent;
                        }
                    }
                    else if (!m_AvoidBeingReplacedBySamePriority && audioAgent.Priority == playAudioParams.Priority)
                    {
                        if (candidateAgent == null || audioAgent.SetAudioAssetTime < candidateAgent.SetAudioAssetTime)
                        {
                            candidateAgent = audioAgent;
                        }
                    }
                }

                if (candidateAgent == null)
                {
                    errorCode = PlayAudioErrorCode.IgnoredDueToLowPriority;
                    return null;
                }

                if (!candidateAgent.SetAudioAsset(audioAsset))
                {
                    errorCode = PlayAudioErrorCode.SetAudioAssetFailure;
                    return null;
                }

                candidateAgent.SerialId = serialId;
                candidateAgent.Time = playAudioParams.Time;
                candidateAgent.MuteInAudioGroup = playAudioParams.MuteInSoundGroup;
                candidateAgent.Loop = playAudioParams.Loop;
                candidateAgent.Priority = playAudioParams.Priority;
                candidateAgent.VolumeInAudioGroup = playAudioParams.VolumeInSoundGroup;
                candidateAgent.Pitch = playAudioParams.Pitch;
                candidateAgent.PanStereo = playAudioParams.PanStereo;
                candidateAgent.SpatialBlend = playAudioParams.SpatialBlend;
                candidateAgent.MaxDistance = playAudioParams.MaxDistance;
                candidateAgent.DopplerLevel = playAudioParams.DopplerLevel;
                candidateAgent.Play(playAudioParams.FadeInSeconds);
                return candidateAgent;
            }

            /// <summary>
            /// 停止播放声音。
            /// </summary>
            /// <param name="serialId">要停止播放声音的序列编号。</param>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
            /// <returns>是否停止播放声音成功。</returns>
            public bool StopAudio(int serialId, float fadeOutSeconds)
            {
                foreach (AudioAgent audioAgent in m_AudioAgents)
                {
                    if (audioAgent.SerialId != serialId)
                    {
                        continue;
                    }

                    audioAgent.Stop(fadeOutSeconds);
                    return true;
                }

                return false;
            }

            /// <summary>
            /// 暂停播放声音。
            /// </summary>
            /// <param name="serialId">要暂停播放声音的序列编号。</param>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
            /// <returns>是否暂停播放声音成功。</returns>
            public bool PauseAudio(int serialId, float fadeOutSeconds)
            {
                foreach (AudioAgent audioAgent in m_AudioAgents)
                {
                    if (audioAgent.SerialId != serialId)
                    {
                        continue;
                    }

                    audioAgent.Pause(fadeOutSeconds);
                    return true;
                }

                return false;
            }

            /// <summary>
            /// 恢复播放声音。
            /// </summary>
            /// <param name="serialId">要恢复播放声音的序列编号。</param>
            /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
            /// <returns>是否恢复播放声音成功。</returns>
            public bool ResumeAudio(int serialId, float fadeInSeconds)
            {
                foreach (AudioAgent audioAgent in m_AudioAgents)
                {
                    if (audioAgent.SerialId != serialId)
                    {
                        continue;
                    }

                    audioAgent.Resume(fadeInSeconds);
                    return true;
                }

                return false;
            }

            /// <summary>
            /// 停止所有已加载的声音。
            /// </summary>
            public void StopAllLoadedAudios()
            {
                foreach (AudioAgent audioAgent in m_AudioAgents)
                {
                    if (audioAgent.IsPlaying)
                    {
                        audioAgent.Stop();
                    }
                }
            }

            /// <summary>
            /// 停止所有已加载的声音。
            /// </summary>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
            public void StopAllLoadedAudios(float fadeOutSeconds)
            {
                foreach (AudioAgent audioAgent in m_AudioAgents)
                {
                    if (audioAgent.IsPlaying)
                    {
                        audioAgent.Stop(fadeOutSeconds);
                    }
                }
            }
        }
    }
}
