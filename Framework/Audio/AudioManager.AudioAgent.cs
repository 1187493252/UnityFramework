/*
* FileName:          AudioAgent
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;

namespace Framework.Audio
{
    internal sealed partial class AudioManager : FrameworkModule, IAudioManager
    {

        private sealed class AudioAgent : IAudioAgent
        {
            private readonly AudioGroup m_AudioGroup;
            private readonly IAudioHelper m_AudioHelper;
            private readonly IAudioAgentHelper m_AudioAgentHelper;
            private int m_SerialId;
            private object m_AudioAsset;
            private DateTime m_SetAudioAssetTime;
            private bool m_MuteInAudioGroup;
            private float m_VolumeInAudioGroup;

            /// <summary>
            /// 初始化声音代理的新实例。
            /// </summary>
            /// <param name="audioGroup">所在的声音组。</param>
            /// <param name="audioHelper">声音辅助器接口。</param>
            /// <param name="audioAgentHelper">声音代理辅助器接口。</param>
            public AudioAgent(AudioGroup audioGroup, IAudioHelper audioHelper, IAudioAgentHelper audioAgentHelper)
            {
                if (audioGroup == null)
                {
                    throw new FrameworkException("Audio group is invalid.");
                }

                if (audioHelper == null)
                {
                    throw new FrameworkException("Audio helper is invalid.");
                }

                if (audioAgentHelper == null)
                {
                    throw new FrameworkException("Audio agent helper is invalid.");
                }

                m_AudioGroup = audioGroup;
                m_AudioHelper = audioHelper;
                m_AudioAgentHelper = audioAgentHelper;
                m_AudioAgentHelper.ResetAudioAgent += OnResetAudioAgent;
                m_SerialId = 0;
                m_AudioAsset = null;
                Reset();
            }

            /// <summary>
            /// 获取所在的声音组。
            /// </summary>
            public IAudioGroup AudioGroup
            {
                get
                {
                    return m_AudioGroup;
                }
            }

            /// <summary>
            /// 获取或设置声音的序列编号。
            /// </summary>
            public int SerialId
            {
                get
                {
                    return m_SerialId;
                }
                set
                {
                    m_SerialId = value;
                }
            }

            /// <summary>
            /// 获取当前是否正在播放。
            /// </summary>
            public bool IsPlaying
            {
                get
                {
                    return m_AudioAgentHelper.IsPlaying;
                }
            }

            /// <summary>
            /// 获取声音长度。
            /// </summary>
            public float Length
            {
                get
                {
                    return m_AudioAgentHelper.Length;
                }
            }

            /// <summary>
            /// 获取或设置播放位置。
            /// </summary>
            public float Time
            {
                get
                {
                    return m_AudioAgentHelper.Time;
                }
                set
                {
                    m_AudioAgentHelper.Time = value;
                }
            }

            /// <summary>
            /// 获取是否静音。
            /// </summary>
            public bool Mute
            {
                get
                {
                    return m_AudioAgentHelper.Mute;
                }
            }

            /// <summary>
            /// 获取或设置在声音组内是否静音。
            /// </summary>
            public bool MuteInAudioGroup
            {
                get
                {
                    return m_MuteInAudioGroup;
                }
                set
                {
                    m_MuteInAudioGroup = value;
                    RefreshMute();
                }
            }

            /// <summary>
            /// 获取或设置是否循环播放。
            /// </summary>
            public bool Loop
            {
                get
                {
                    return m_AudioAgentHelper.Loop;
                }
                set
                {
                    m_AudioAgentHelper.Loop = value;
                }
            }

            /// <summary>
            /// 获取或设置声音优先级。
            /// </summary>
            public int Priority
            {
                get
                {
                    return m_AudioAgentHelper.Priority;
                }
                set
                {
                    m_AudioAgentHelper.Priority = value;
                }
            }

            /// <summary>
            /// 获取音量大小。
            /// </summary>
            public float Volume
            {
                get
                {
                    return m_AudioAgentHelper.Volume;
                }
            }

            /// <summary>
            /// 获取或设置在声音组内音量大小。
            /// </summary>
            public float VolumeInAudioGroup
            {
                get
                {
                    return m_VolumeInAudioGroup;
                }
                set
                {
                    m_VolumeInAudioGroup = value;
                    RefreshVolume();
                }
            }

            /// <summary>
            /// 获取或设置声音音调。
            /// </summary>
            public float Pitch
            {
                get
                {
                    return m_AudioAgentHelper.Pitch;
                }
                set
                {
                    m_AudioAgentHelper.Pitch = value;
                }
            }

            /// <summary>
            /// 获取或设置声音立体声声相。
            /// </summary>
            public float PanStereo
            {
                get
                {
                    return m_AudioAgentHelper.PanStereo;
                }
                set
                {
                    m_AudioAgentHelper.PanStereo = value;
                }
            }

            /// <summary>
            /// 获取或设置声音空间混合量。
            /// </summary>
            public float SpatialBlend
            {
                get
                {
                    return m_AudioAgentHelper.SpatialBlend;
                }
                set
                {
                    m_AudioAgentHelper.SpatialBlend = value;
                }
            }

            /// <summary>
            /// 获取或设置声音最大距离。
            /// </summary>
            public float MaxDistance
            {
                get
                {
                    return m_AudioAgentHelper.MaxDistance;
                }
                set
                {
                    m_AudioAgentHelper.MaxDistance = value;
                }
            }

            /// <summary>
            /// 获取或设置声音多普勒等级。
            /// </summary>
            public float DopplerLevel
            {
                get
                {
                    return m_AudioAgentHelper.DopplerLevel;
                }
                set
                {
                    m_AudioAgentHelper.DopplerLevel = value;
                }
            }

            /// <summary>
            /// 获取声音代理辅助器。
            /// </summary>
            public IAudioAgentHelper Helper
            {
                get
                {
                    return m_AudioAgentHelper;
                }
            }

            /// <summary>
            /// 获取声音创建时间。
            /// </summary>
            internal DateTime SetAudioAssetTime
            {
                get
                {
                    return m_SetAudioAssetTime;
                }
            }

            /// <summary>
            /// 播放声音。
            /// </summary>
            public void Play()
            {
                m_AudioAgentHelper.Play(Constant.DefaultFadeInSeconds);
            }

            /// <summary>
            /// 播放声音。
            /// </summary>
            /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
            public void Play(float fadeInSeconds)
            {
                m_AudioAgentHelper.Play(fadeInSeconds);
            }

            /// <summary>
            /// 停止播放声音。
            /// </summary>
            public void Stop()
            {
                m_AudioAgentHelper.Stop(Constant.DefaultFadeOutSeconds);
            }

            /// <summary>
            /// 停止播放声音。
            /// </summary>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
            public void Stop(float fadeOutSeconds)
            {
                m_AudioAgentHelper.Stop(fadeOutSeconds);
            }

            /// <summary>
            /// 暂停播放声音。
            /// </summary>
            public void Pause()
            {
                m_AudioAgentHelper.Pause(Constant.DefaultFadeOutSeconds);
            }

            /// <summary>
            /// 暂停播放声音。
            /// </summary>
            /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
            public void Pause(float fadeOutSeconds)
            {
                m_AudioAgentHelper.Pause(fadeOutSeconds);
            }

            /// <summary>
            /// 恢复播放声音。
            /// </summary>
            public void Resume()
            {
                m_AudioAgentHelper.Resume(Constant.DefaultFadeInSeconds);
            }

            /// <summary>
            /// 恢复播放声音。
            /// </summary>
            /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
            public void Resume(float fadeInSeconds)
            {
                m_AudioAgentHelper.Resume(fadeInSeconds);
            }

            /// <summary>
            /// 重置声音代理。
            /// </summary>
            public void Reset()
            {
                if (m_AudioAsset != null)
                {
                    m_AudioHelper.ReleaseAudioAsset(m_AudioAsset);
                    m_AudioAsset = null;
                }

                m_SetAudioAssetTime = DateTime.MinValue;
                Time = Constant.DefaultTime;
                MuteInAudioGroup = Constant.DefaultMute;
                Loop = Constant.DefaultLoop;
                Priority = Constant.DefaultPriority;
                VolumeInAudioGroup = Constant.DefaultVolume;
                Pitch = Constant.DefaultPitch;
                PanStereo = Constant.DefaultPanStereo;
                SpatialBlend = Constant.DefaultSpatialBlend;
                MaxDistance = Constant.DefaultMaxDistance;
                DopplerLevel = Constant.DefaultDopplerLevel;
                m_AudioAgentHelper.Reset();
            }

            internal bool SetAudioAsset(object audioAsset)
            {
                Reset();
                m_AudioAsset = audioAsset;
                m_SetAudioAssetTime = DateTime.UtcNow;
                return m_AudioAgentHelper.SetAudioAsset(audioAsset);
            }

            internal void RefreshMute()
            {
                m_AudioAgentHelper.Mute = m_AudioGroup.Mute || m_MuteInAudioGroup;
            }

            internal void RefreshVolume()
            {
                m_AudioAgentHelper.Volume = m_AudioGroup.Volume * m_VolumeInAudioGroup;
            }

            private void OnResetAudioAgent(object sender, ResetAudioAgentEventArgs e)
            {
                Reset();
            }

        }
    }
}
