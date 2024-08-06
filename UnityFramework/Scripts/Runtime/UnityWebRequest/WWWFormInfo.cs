/*
* FileName:          WWWFormInfo
* CompanyName:       
* Author:            relly
* Description:       
*/


using Framework;
using Framework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework.Runtime
{
    internal sealed class WWWFormInfo : IReference
    {
        private WWWForm m_WWWForm;
        private object m_UserData;
        private Dictionary<string, string> m_Headers;
        public WWWFormInfo()
        {
            m_WWWForm = null;
            m_UserData = null;
            m_Headers = null;
        }

        public WWWForm WWWForm
        {
            get
            {
                return m_WWWForm;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public Dictionary<string, string> Headers
        {
            get
            {
                return m_Headers;
            }
        }


        public static WWWFormInfo Create(WWWForm wwwForm, object userData, Dictionary<string, string> headers)
        {
            WWWFormInfo wwwFormInfo = ReferencePool.Acquire<WWWFormInfo>();
            wwwFormInfo.m_WWWForm = wwwForm;
            wwwFormInfo.m_UserData = userData;
            wwwFormInfo.m_Headers = headers;


            return wwwFormInfo;
        }

        public void Clear()
        {
            m_WWWForm = null;
            m_UserData = null;
            m_Headers = null;

        }
    }
}
