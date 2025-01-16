/*
* FileName:          AudioHelper
* CompanyName:       
* Author:            relly
* Description:       加载AudioConfig,AudioData等辅助脚本
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Event;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 数据加载模式
    /// </summary>
    public enum AudioLoadMode
    {
        /// <summary>
        /// 从数据缓存中读取
        /// </summary>
        DataCache,
        /// <summary>
        /// 从单独路径读取
        /// </summary>
        Path
    }
    public class AudioHelper : MonoBehaviour
    {
        [Header("配置加载模式")]
        public AudioLoadMode AudioLoadMode;
        [Header("资源加载模式")]
        public ResourceLoadMode ResourceLoadMode;
        [Header("配置表名称:同类型的表格不能同名")]
        public List<string> TableName = new List<string>();
        AudioComponent audioComponent;
        [Header("仅限ConfigLoadMode:DataCache")]
        public List<AudioClip> ClipsList = new List<AudioClip> { };

        [Header("是否初始化加载所有Audio")]
        public bool InitLoadAllAudio = false;
        public virtual void Init()
        {
            audioComponent = ComponentEntry.Audio;
            ComponentEntry.Event.Subscribe(LoadAudioInfoFailEventArgs.EventId, LoadAudioFail);
            ComponentEntry.Event.Subscribe(LoadAudioInfoSuccessEventArgs.EventId, LoadAudioSuccess);

            LoadAudio();

        }

        private void LoadAudioSuccess(object sender, GameEventArgs e)
        {
            LoadAudioInfoSuccessEventArgs eventArgs = (LoadAudioInfoSuccessEventArgs)e;
            Log.Info(eventArgs.UserData);
        }

        private void LoadAudioFail(object sender, GameEventArgs e)
        {
            LoadAudioInfoFailEventArgs eventArgs = (LoadAudioInfoFailEventArgs)e;
            Log.Error(eventArgs.UserData);
        }

        public void Clear()
        {

        }


        void LoadAudio()
        {
            if (AudioLoadMode == AudioLoadMode.Path)
            {
                StartCoroutine(InitOperation_LoadAudioForPath());
            }
            else
            {
                InitOperation_LoadAudioForDataCache();
            }
        }
        void InitOperation_LoadAudioForDataCache()
        {
            for (int i = 0; i < ClipsList.Count; i++)
            {
                AudioInfo audioInfo = new AudioInfo
                {
                    Id = i,
                    AudioName = ClipsList[i].name,
                    Path = null,
                    TextContent = ClipsList[i].name,
                    Clip = ClipsList[i],
                    AudioActionDic = new Dictionary<float, Action>(),
                    TimerList = new List<Timer>()
                };
                audioComponent.AddAudioInfo(audioInfo);
            }

            Log.Info($"load audio success");
        }
        //从路径加载
        IEnumerator InitOperation_LoadAudioForPath()
        {
            DateTime startTime = global::System.DateTime.Now;

            foreach (var tableName in TableName)
            {
#if !UNITY_WEBGL
                List<AudioInfo> tableDataList = ComponentEntry.Data.GetDataByTableName<List<AudioInfo>>(tableName);
#else
                // List<AudioInfo> tableDataList = ComponentEntry.Data.GetAudioInfo(tableName);
                List<AudioInfo> tableDataList = ComponentEntry.Data.GetDataByTableName<List<AudioInfo>>(tableName);

#endif


                foreach (var item in tableDataList)
                {
                    int id = item.Id;
                    string name = item.AudioName;
                    string path = item.Path + name;
                    string textContent = item.TextContent;
                    AudioClip clip = null;
                    if (InitLoadAllAudio)
                    {
                        switch (ResourceLoadMode)
                        {
                            case ResourceLoadMode.Resource:
                                clip = ComponentEntry.Resource.LoadAsync<AudioClip>(path, ResourceLoadMode);
                                break;
                            case ResourceLoadMode.StreamingAssets:
                            case ResourceLoadMode.WebRequest:
                                string audiopath = Application.streamingAssetsPath + path + ".wav";

                                UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audiopath, AudioType.WAV);
                                yield return request.SendWebRequest();
#if UNITY_2020_1_OR_NEWER
                                if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
#else
			                if (request.isHttpError || request.isNetworkError)
#endif
                                {
                                    ComponentEntry.Event.Fire(this, LoadAudioInfoFailEventArgs.Create($"load audio {name} fail : {request.error}"));
                                }
                                else
                                {
                                    clip = DownloadHandlerAudioClip.GetContent(request);
                                }
                                request.Dispose();
                                break;
                        }
                    }

                    AudioInfo audioInfo = new AudioInfo
                    {
                        Id = id,
                        AudioName = name,
                        Path = path,
                        TextContent = textContent,
                        Clip = clip,
                        AudioActionDic = new Dictionary<float, Action>(),
                        TimerList = new List<Timer>()
                    };
                    audioComponent.AddAudioInfo(audioInfo);
                }
            }
            DateTime endTime = global::System.DateTime.Now;
            TimeSpan duration = endTime.Subtract(startTime);

            ComponentEntry.Event.Fire(this, LoadAudioInfoSuccessEventArgs.Create($"load all audio success,cout {audioComponent.AudioInfoCout},time {duration.TotalMilliseconds / 1000.0f:f2}s"));
        }
    }
}