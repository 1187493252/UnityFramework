/*
* FileName:          PlayAudioInfo
* CompanyName:       
* Author:            relly
* Description:       
*/

namespace Framework.Audio
{
    internal sealed partial class AudioManager : FrameworkModule, IAudioManager
    {
        private sealed class PlayAudioInfo : IReference
        {
            private int m_SerialId;
            private AudioGroup m_AudioGroup;
            private PlayAudioParams m_PlayAudioParams;
            private object m_UserData;

            public PlayAudioInfo()
            {
                m_SerialId = 0;
                m_AudioGroup = null;
                m_PlayAudioParams = null;
                m_UserData = null;
            }

            public int SerialId
            {
                get
                {
                    return m_SerialId;
                }
            }

            public AudioGroup AudioGroup
            {
                get
                {
                    return m_AudioGroup;
                }
            }

            public PlayAudioParams PlayAudioParams
            {
                get
                {
                    return m_PlayAudioParams;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }

            public static PlayAudioInfo Create(int serialId, AudioGroup audioGroup, PlayAudioParams playAudioParams, object userData)
            {
                PlayAudioInfo playAudioInfo = ReferencePool.Acquire<PlayAudioInfo>();
                playAudioInfo.m_SerialId = serialId;
                playAudioInfo.m_AudioGroup = audioGroup;
                playAudioInfo.m_PlayAudioParams = playAudioParams;
                playAudioInfo.m_UserData = userData;
                return playAudioInfo;
            }

            public void Clear()
            {
                m_SerialId = 0;
                m_AudioGroup = null;
                m_PlayAudioParams = null;
                m_UserData = null;
            }
        }
    }
}
