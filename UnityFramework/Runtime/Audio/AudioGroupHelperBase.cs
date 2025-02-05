/*
* FileName:          AudioGroupHelperBase
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using Framework.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityFramework.Runtime
{

    /// <summary>
    /// 声音组辅助器基类。
    /// </summary>
    public abstract class AudioGroupHelperBase : MonoBehaviour, IAudioGroupHelper
    {

        [SerializeField]
        private AudioMixerGroup m_AudioMixerGroup = null;

        /// <summary>
        /// 获取或设置声音组辅助器所在的混音组。
        /// </summary>
        public AudioMixerGroup AudioMixerGroup
        {
            get
            {
                return m_AudioMixerGroup;
            }
            set
            {
                m_AudioMixerGroup = value;
            }
        }
    }

}