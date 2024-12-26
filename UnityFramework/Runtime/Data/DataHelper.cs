/*
* FileName:          DataHelper
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Framework;
using Framework.Event;
using LitJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityFramework.Runtime
{
    public class DataHelper : MonoBehaviour
    {
        DataComponent dataComponent;
        [Header("StreamingAssets/Data路径")]
        public List<DataFileInfo> DataFilePaths;

        public virtual void Init()
        {

            dataComponent = ComponentEntry.Data;
            ComponentEntry.Event.Subscribe(LoadDataFailEventArgs.EventId, LoadDataFail);
            ComponentEntry.Event.Subscribe(LoadDataSuccessEventArgs.EventId, LoadDataSuccess);
            StartCoroutine(InitOperation());

        }

        public void Clear()
        {
            ComponentEntry.Event.Unsubscribe(LoadDataSuccessEventArgs.EventId, LoadDataSuccess);
            ComponentEntry.Event.Unsubscribe(LoadDataFailEventArgs.EventId, LoadDataFail);
        }

        private void LoadDataSuccess(object sender, GameEventArgs e)
        {
            LoadDataSuccessEventArgs loadDataSuccessEventArgs = (LoadDataSuccessEventArgs)e;
            Log.Info(loadDataSuccessEventArgs.UserData);

        }

        private void LoadDataFail(object sender, GameEventArgs e)
        {
            LoadDataFailEventArgs loadDataFailEventArgs = (LoadDataFailEventArgs)e;
            Log.Error(loadDataFailEventArgs.UserData);
        }

        IEnumerator InitOperation()
        {
            string dataContent = "";
            DateTime startTime = global::System.DateTime.Now;

            foreach (var item in DataFilePaths)
            {
                string url = Application.streamingAssetsPath + "/Data/" + item.dataName;
                switch (item.dataType)
                {
                    case DataType.Json:
                        url += ".json";
                        break;
                    case DataType.Txt:
                        url += ".txt";
                        break;
                    case DataType.Xml:
                        url += ".xml";
                        break;
                    case DataType.Base64:
                        url += ".base";
                        break;
                    case DataType.Binary:
                        url += ".sc";
                        break;
                    case DataType.Excel:
                        url += ".xlsx";
                        break;
                }
                UnityWebRequest request = UnityWebRequest.Get(url);
                yield return request.SendWebRequest();
#if UNITY_2020_1_OR_NEWER
                if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
#else
			if (request.isHttpError || request.isNetworkError)
#endif
                {
                    ComponentEntry.Event.Fire(this, LoadDataFailEventArgs.Create($"load data {item.dataName} fail : {request.error}"));
                }
                else
                {
                    byte[] datas;

                    switch (item.dataType)
                    {
                        case DataType.Base64:
                            datas = Convert.FromBase64String(request.downloadHandler.text);
                            dataContent = Encoding.Unicode.GetString(datas);
                            break;
                        case DataType.Binary:
                            Stream stream = new MemoryStream(request.downloadHandler.data);
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            datas = (byte[])binaryFormatter.Deserialize(stream);
                            dataContent = Encoding.Unicode.GetString(datas);
                            stream.Dispose();
                            break;
                        case DataType.Excel:
                            datas = request.downloadHandler.data;
                            List<DataTable> dataTableList = ExcelHelper.GetDataTablesFromExcel(datas);
                            dataContent = ExcelHelper.DataTableToJson(dataTableList);
                            break;
                        default:
                            dataContent = request.downloadHandler.text;
                            break;
                    }
                    if (!dataContent.IsNullOrEmpty())
                    {

                        dataComponent.AddJsonData(item.dataName, dataContent);

#if !UNITY_WEBGL
                        Dictionary<string, JsonData> temp = JsonMapper.ToObject<Dictionary<string, JsonData>>(dataContent);
#else
                        JObject temp = JObject.Parse(dataContent);
#endif
                        foreach (var item1 in temp)
                        {
                            dataComponent.AddDataTable(item1.Key, item1.Value);
                        }
                    }
                }
            }

            DateTime endTime = global::System.DateTime.Now;
            TimeSpan duration = endTime.Subtract(startTime);
            ComponentEntry.Event.Fire(this, LoadDataSuccessEventArgs.Create($"load all data success,cout {dataComponent.DataTableCout},time {duration.TotalMilliseconds / 1000.0f:f2}s"));

        }





    }

    [Serializable]
    public class DataFileInfo
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public string dataName;

        /// <summary>
        /// 文件类型
        /// </summary>
        public DataType dataType;
    }
    public enum DataType
    {
        Json,           //Json
        Txt,            //Txt
        Xml,            //Xml
        Base64,         //Base64位字符串
        Binary,       //二进制
        Excel
    }
}
