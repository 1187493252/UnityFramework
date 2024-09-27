/*
* FileName:          AudioSourceComponent
* CompanyName:  
* Author:            relly
* Description:       AudioComponent:注册AudioSource
*                    
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework.Runtime
{
    [DisallowMultipleComponent]
    public class AudioSourceAgent : MonoBehaviour
    {
        [Header("播放器标识,为空不会加入缓存")]
        public string mName;
        AudioSource mAudioSource;
        protected virtual void Awake()
        {
            mAudioSource = GetComponent<AudioSource>();
            if (mAudioSource == null)
            {
                mAudioSource = gameObject.AddComponent<AudioSource>();
            }
            ComponentEntry.Audio.AddAudioSourse(mName, mAudioSource);
        }
    }
}
