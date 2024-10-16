/*
* FileName:          ConfigHelper
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public class ConfigHelper : MonoBehaviour
    {
        [Header("配置表名称:同类型的表格不能同名")]
        public List<string> TableName = new List<string>();
        ConfigComponent configComponent;

        public virtual void Init()
        {
            configComponent = ComponentEntry.Config;
            ComponentEntry.Event.Subscribe(LoadConfigFailEventArgs.EventId, LoadConfigFail);
            ComponentEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, LoadConfigSuccess);
            InitOperation_LoadConfig();
        }

        private void LoadConfigSuccess(object sender, GameEventArgs e)
        {
            LoadConfigSuccessEventArgs eventArgs = (LoadConfigSuccessEventArgs)e;
            Log.Info(eventArgs.UserData);
        }

        private void LoadConfigFail(object sender, GameEventArgs e)
        {
            LoadConfigFailEventArgs eventArgs = (LoadConfigFailEventArgs)e;
            Log.Error(eventArgs.UserData);
        }

        void InitOperation_LoadConfig()
        {
            DateTime startTime = global::System.DateTime.Now;

            foreach (var tableName in TableName)
            {
                List<ConfigInfo> tableDataList;
                try
                {
#if !UNITY_WEBGL
                    tableDataList = ComponentEntry.Data.GetDataByTableName<List<ConfigInfo>>(tableName);
#else
               tableDataList = ComponentEntry.Data.GetDataByTableName<List<ConfigInfo>>(tableName);
#endif
                    foreach (var item in tableDataList)
                    {
                        configComponent.AddConfig(item.key, item.value);

                    }
                }
                catch (Exception)
                {
                    ComponentEntry.Event.Fire(this, LoadConfigFailEventArgs.Create($"load config table[{tableName}]  fail"));
                }


            }
            DateTime endTime = global::System.DateTime.Now;
            TimeSpan duration = endTime.Subtract(startTime);

            ComponentEntry.Event.Fire(this, LoadConfigSuccessEventArgs.Create($"load all config success,cout {configComponent.CinfigInfoCount},time {duration.TotalMilliseconds / 1000.0f:f2}s"));
        }
    }

}
