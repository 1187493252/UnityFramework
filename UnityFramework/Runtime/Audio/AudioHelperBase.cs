/*
* FileName:          AudioHelperBase
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using Framework.Audio;
using UnityEngine;

namespace UnityFramework.Runtime
{

    /// <summary>
    /// 声音辅助器基类。
    /// </summary>
    public abstract class AudioHelperBase : MonoBehaviour, IAudioHelper
    {

        /// <summary>
        /// 释放声音资源。
        /// </summary>
        /// <param name="soundAsset">要释放的声音资源。</param>
        public abstract void ReleaseAudioAsset(object soundAsset);
    }

}