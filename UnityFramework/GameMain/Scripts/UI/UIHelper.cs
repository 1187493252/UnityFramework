/*
* FileName:          UIFormHelper
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityFramework.Runtime.UI
{
    public class UIHelper : MonoBehaviour
    {
        [Header("资源加载模式")]
        public ResourceLoadMode ResourceLoadMode;
        [Header("配置表名称:同类型的表格不能同名")]
        public List<string> TableName = new List<string>();
        UIComponent uiComponent;
        public virtual void Init()
        {
            uiComponent = ComponentEntry.UI;

            LoadEntity();

        }



        public void Clear()
        {

        }
        void LoadEntity()
        {
            StartCoroutine(InitOperation());
        }
        IEnumerator InitOperation()
        {
            DateTime startTime = global::System.DateTime.Now;

            foreach (var tableName in TableName)
            {
#if !UNITY_WEBGL
                List<UIFormInfo> tableDataList = ComponentEntry.Data.GetDataByTableName<List<UIFormInfo>>(tableName);
#else
                //  List<UIFormInfo> tableDataList = ComponentEntry.Data.GetDataByFileName<List<UIFormInfo>>(tableName);
                List<UIFormInfo> tableDataList = ComponentEntry.Data.GetDataByTableName<List<UIFormInfo>>(tableName);

#endif


                foreach (var item in tableDataList)
                {
                    int id = item.Id;
                    string name = item.UIFormName;
                    string path = item.Path + name;

                    GameObject entity = null;
                    switch (ResourceLoadMode)
                    {
                        case ResourceLoadMode.Resource:
                            entity = ComponentEntry.Resource.Load<GameObject>(path, ResourceLoadMode);
                            break;
                        case ResourceLoadMode.StreamingAssets:
                        case ResourceLoadMode.WebRequest:
                            path = Application.streamingAssetsPath + "/" + path;
                            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
                            yield return request.SendWebRequest();
#if UNITY_2020_1_OR_NEWER
                            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
#else
			                if (request.isHttpError || request.isNetworkError)
#endif
                            {
                                Log.Error($"Load {path} fail");

                            }
                            else
                            {
                                AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                                entity = assetBundle.LoadAsset<GameObject>(name);
                            }
                            request.Dispose();
                            break;
                    }
                    item.GameObject = entity;
                    item.Path = path;

                    uiComponent.AddUIFormInfo(item.Id, item);
                }
            }
            DateTime endTime = global::System.DateTime.Now;
            TimeSpan duration = endTime.Subtract(startTime);
            Log.Info($"load all uiform success,cout {uiComponent.UIFormInfoCout},time {duration.TotalMilliseconds / 1000.0f:f2}s");

        }
    }
}
