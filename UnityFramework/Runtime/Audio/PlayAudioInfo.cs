/*
* FileName:          PlayAudioInfo
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using Framework;
using UnityEngine;

namespace UnityFramework.Runtime
{

    internal sealed class PlayAudioInfo : IReference
    {
        private Entity m_BindingEntity;
        private Vector3 m_WorldPosition;
        private object m_UserData;

        public PlayAudioInfo()
        {
            m_BindingEntity = null;
            m_WorldPosition = Vector3.zero;
            m_UserData = null;
        }

        public Entity BindingEntity
        {
            get
            {
                return m_BindingEntity;
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                return m_WorldPosition;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public static PlayAudioInfo Create(Entity bindingEntity, Vector3 worldPosition, object userData)
        {
            PlayAudioInfo playSoundInfo = ReferencePool.Acquire<PlayAudioInfo>();
            playSoundInfo.m_BindingEntity = bindingEntity;
            playSoundInfo.m_WorldPosition = worldPosition;
            playSoundInfo.m_UserData = userData;
            return playSoundInfo;
        }

        public void Clear()
        {
            m_BindingEntity = null;
            m_WorldPosition = Vector3.zero;
            m_UserData = null;
        }
    }

}