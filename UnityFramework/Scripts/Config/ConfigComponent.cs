/*
* FileName:          ConfigComponent
* CompanyName:       
* Author:            relly
* Description:       全局配置
*/


using Framework;
using Framework.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 全局配置组件。
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ConfigComponent : UnityFrameworkComponent
    {
        private const int DefaultPriority = 0;
        private IConfigManager m_ConfigManager = null;
        private EventComponent m_EventComponent = null;
        ConfigHelper helper;
        Dictionary<string, string> configDic = new Dictionary<string, string>();

        public int CinfigInfoCount
        {
            get
            {
                return configDic.Count;
                //   return m_ConfigManager.Count;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            m_ConfigManager = FrameworkEntry.GetModule<IConfigManager>();
            if (m_ConfigManager == null)
            {
                Log.Fatal("Config manager is invalid.");
                return;
            }
        }
        public void Init()
        {
            helper = GetComponent<ConfigHelper>();



            if (helper)
            {
                helper.Init();
            }
        }

        public void AddConfig(string key, string value)
        {
            configDic.AddOrReplace(key, value);
            //   return m_ConfigManager.AddConfig(key, value);
        }
        public string GetConfig(string key)
        {
            return configDic.GetValue(key);
        }
        //public T GetConfig<T>(string key)
        //{
        //    return (T)configDic.GetValue(key);
        //}
        //public string GetString(string key)
        //{
        //    return configDic.GetValue(key);
        //}
        //public int GetInt(string key)
        //{
        //    return (int)configDic.GetValue(key);
        //}
        //public float GetFloat(string key)
        //{
        //    return (float)configDic.GetValue(key);
        //}

        //public bool GetBool(string key)
        //{
        //    return (bool)configDic.GetValue(key);
        //}

        //---------------------------------
        [StructLayout(LayoutKind.Auto)]
        private struct ConfigData
        {
            private readonly bool m_BoolValue;
            private readonly int m_IntValue;
            private readonly float m_FloatValue;
            private readonly string m_StringValue;

            public ConfigData(bool boolValue, int intValue, float floatValue, string stringValue)
            {
                m_BoolValue = boolValue;
                m_IntValue = intValue;
                m_FloatValue = floatValue;
                m_StringValue = stringValue;
            }

            public bool BoolValue
            {
                get
                {
                    return m_BoolValue;
                }
            }

            public int IntValue
            {
                get
                {
                    return m_IntValue;
                }
            }

            public float FloatValue
            {
                get
                {
                    return m_FloatValue;
                }
            }

            public string StringValue
            {
                get
                {
                    return m_StringValue;
                }
            }
        }
    }

    public class ConfigInfo
    {
        public string key;
        public string value;
    }
}
