/*
* FileName:          EntityHelper
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

namespace UnityFramework.Runtime
{
    public class EntityHelper : MonoBehaviour
    {
        [Header("资源加载模式")]
        public ResourceLoadMode ResourceLoadMode;
        [Header("配置表名称:同类型的表格不能同名")]
        public List<string> TableName = new List<string>();
        EntityComponent entityComponent;
        public virtual void Init()
        {
            entityComponent = ComponentEntry.Entity;

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
                List<EntityInfo> tableDataList = ComponentEntry.Data.GetDataByTableName<List<EntityInfo>>(tableName);
#else
                //  List<EntityInfo> tableDataList = ComponentEntry.Data.GetDataByFileName<List<EntityInfo>>(tableName);
                List<EntityInfo> tableDataList = ComponentEntry.Data.GetDataByTableName<List<EntityInfo>>(tableName);

#endif


                foreach (var item in tableDataList)
                {
                    int id = item.Id;
                    string name = item.EntityName;
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

                    entityComponent.AddEntityInfo(item.Id, item);
                }
            }
            DateTime endTime = global::System.DateTime.Now;
            TimeSpan duration = endTime.Subtract(startTime);

            Log.Info($"load all entity success,cout {entityComponent.EntityInfoCout},time {duration.TotalMilliseconds / 1000.0f:f2}s");

        }
    }
}
