/*
* FileName:          ConfigHelper
* CompanyName:       
* Author:            relly
* Description:       
*/

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

            InitOperation_LoadConfig();
        }


        void InitOperation_LoadConfig()
        {
            DateTime startTime = global::System.DateTime.Now;

            foreach (var tableName in TableName)
            {
#if !UNITY_WEBGL
                List<ConfigInfo> tableDataList = ComponentEntry.Data.GetDataByTableName<List<ConfigInfo>>(tableName);
#else


                List<ConfigInfo> tableDataList = ComponentEntry.Data.GetDataByTableName<List<ConfigInfo>>(tableName);

#endif


                foreach (var item in tableDataList)
                {
                    configComponent.AddConfig(item.key, item.value);

                }
            }
            DateTime endTime = global::System.DateTime.Now;
            TimeSpan duration = endTime.Subtract(startTime);
            Log.Info($"load all config success,cout {configComponent.CinfigInfoCount},time {duration.TotalMilliseconds / 1000.0f:f2}s");
        }
    }

}
