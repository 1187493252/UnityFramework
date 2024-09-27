/*
* FileName:          DataComponent
* CompanyName:       
* Author:            relly
* Description:       
*                    1.非web平台用的LitJson转换为JsonData直接以表名获取List<表名>
*                    2.web版用的Unity特供版Newtonsoft.Json转换为JToken直接以表名获取List<表名>
*                    3.仅当2失效时,web平台不支持IO以文件名为父类,先反序列化成父类再获取List<表名>,每次需要自定义解析方法
*/

using LitJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework.Runtime
{


    [DisallowMultipleComponent]
    public class DataComponent : UnityFrameworkComponent
    {
#if UNITY_WEBGL
        /// <summary>
        ///  key excel的表名,同类型数据不允许同名,value 表格数据
        /// </summary>
        Dictionary<string, JToken> dataDic = new Dictionary<string, JToken>();
#else
        /// <summary>
        ///  key excel的表名,同类型数据不允许同名,value 表格数据
        /// </summary>
        Dictionary<string, JsonData> dataDic = new Dictionary<string, JsonData>();
#endif



        /// <summary>
        /// 所有excel文件的json数据, key excel的文件名,value excel(N个表)的所有json数据
        /// </summary>
        Dictionary<string, string> fileJsonDataDic = new Dictionary<string, string>();

        DataHelper helper;
        //数据表个数
        public int DataTableCout
        {
            get
            {
                return dataDic.Count;
            }
        }
        public void Init()
        {
            dataDic.Clear();
            helper = GetComponent<DataHelper>();
            if (helper)
            {
                helper.Init();
            }

        }

        //添加数据表
#if UNITY_WEBGL
        public void AddDataTable(string tableName, JToken jsonData)
#else
        public void AddDataTable(string tableName, JsonData jsonData)

#endif
        {
            dataDic.TryAdd(tableName, jsonData);
        }

        /// <summary>
        /// 多个数据文件下,存储某个文件的json数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="jsondata"></param>
        public void AddJsonData(string fileName, string jsondata)
        {
            fileJsonDataDic.TryAdd(fileName, jsondata);
        }
        /// <summary>
        /// 多个数据文件下,获取某个文件的json数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetJsonDataByFileName(string fileName)
        {
            return fileJsonDataDic[fileName];
        }
        /// <summary>
        /// 多个数据文件下,获取某个文件的数据
        /// </summary>
        /// <typeparam name="T">转换类对象的变量需要对应表名</typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T GetDataByFileName<T>(string fileName)
        {
            string content = fileJsonDataDic[fileName];
            T data = JsonMapper.ToObject<T>(content);
            return data;
        }

        /// <summary>
        /// 根据表名获取表格数据 web版慎用支持JsonData[name]但不支持JsonWriter/JsonReader的IO操作 即JsonData.ToJson
        /// </summary>
        /// <typeparam name="T">大多情况为list,转换类对象的变量不用对应表名</typeparam>
        /// <param name="tableName">excel表格名称</param>
        /// <returns></returns>
        public T GetDataByTableName<T>(string tableName)
        {
            if (!dataDic.ContainsKey(tableName))
            {
                Log.Error($"dataDic[{tableName}] Data does not exist");
                return default(T);
            }
            T tableDataList = default;
            try
            {

#if UNITY_WEBGL
                tableDataList = JsonConvert.DeserializeObject<T>(dataDic[tableName].ToString());
#else
                tableDataList = JsonMapper.ToObject<T>(dataDic[tableName].ToJson());
#endif
            }
            catch (Exception e)
            {
                Log.Error($"dataDic[{tableName}] Data does not exist");
                throw e;
            }
            if (tableDataList != null)
            {
                return tableDataList;
            }
            return default(T);
        }

        public void Clear()
        {
            dataDic.Clear();
        }



    }
}