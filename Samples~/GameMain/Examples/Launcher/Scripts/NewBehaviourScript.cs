using Framework;
using Framework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UnityFramework.Runtime
{
    public class NewBehaviourScript : MonoBehaviour
    {
        public KeyCodePressEventArgsTest keyCodePressEventArgsTest;
        public List<int> numlist = new List<int>();

        public Text text;

        // Start is called before the first frame update
        void Start()
        {
            Invoke("CeShi", 1);
        }
        void CeShi()
        {
            keyCodePressEventArgsTest.Test1(numlist);
            //     numlist.ForEach(i => Log.Debug(i));
            ComponentEntry.UI.OpenSceneUIForm(10001);


            ComponentEntry.UI.OpenUIForm(10002, "default");

            ComponentEntry.UI.OpenSceneUIForm(10003);



            ComponentEntry.Entity.GetSceneEntity(10001).Show();

            ComponentEntry.Entity.ShowSceneEntity("2");

            ComponentEntry.Entity.ShowEntity(10003, "default");

            ComponentEntry.Audio.PlayAudio("Hello World");
            ComponentEntry.Audio.PlayAudioBGM(10003);


            ComponentEntry.Timer.CreateTimer(delegate
            {
                Log.Info("计时结束");
            }, "1", 10, 1, true, true);
            ComponentEntry.Timer.CreateTimer(delegate
            {
                ComponentEntry.UI.CloseSceneUIForm(10001);

                ComponentEntry.UI.CloseUIForm(10002);
                ComponentEntry.Entity.HideSceneEntity(10001);

                ComponentEntry.Entity.HideEntity(10003);

            }, 5);
            ComponentEntry.Timer.CreateTimer(delegate
            {
                ComponentEntry.UI.GetSceneUIForm(10001).Show();

                ComponentEntry.UI.OpenUIForm(10002, "default");

                ComponentEntry.Entity.GetSceneEntity(10001).Show();

                ComponentEntry.Entity.ShowEntity(10003, "default");

            }, 10);

            ComponentEntry.Setting.SetBool("11", false);

            ComponentEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, WebRequestSuccess);
            ComponentEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, WebRequestFailure);
            ComponentEntry.Event.Subscribe(WebRequestStartEventArgs.EventId, WebRequestStart);

            //   ComponentEntry.WebRequest.AddWebRequest(Application.streamingAssetsPath + "/Data/Config.json");

            byte[] b = Utility.Converter.GetBytes("阿萨德");
            //     ComponentEntry.WebRequest.AddWebRequest("http://127.0.0.1:8082/", b, "1", "");
            //     ComponentEntry.WebRequest.RequestPost("http://127.0.0.1:8082/", "阿斯达1");

            Dictionary<int, CasesInfo> CasesInfoDic = new Dictionary<int, CasesInfo>();
            List<CasesInfo> CasesInfoList = ComponentEntry.Data.GetDataByTableName<List<CasesInfo>>("CasesInfo");
            foreach (var item in CasesInfoList)
            {
                item.Parse();
                CasesInfoDic.Add(item.Id, item);
            }
            int id = UnityEngine.Random.Range(0, CasesInfoList.Count - 1);
            CasesInfo casesInfo = CasesInfoList[id];
            text.text = casesInfo.Name;
        }

        private void WebRequestStart(object sender, GameEventArgs e)
        {
            WebRequestStartEventArgs webRequestSuccessEventArgs = (WebRequestStartEventArgs)e;

            Log.Info("开始请求web:" + webRequestSuccessEventArgs.WebRequestUri);

        }

        private void WebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs webRequestSuccessEventArgs = (WebRequestFailureEventArgs)e;

            Log.Error(webRequestSuccessEventArgs.WebRequestUri + ":" + webRequestSuccessEventArgs.ErrorMessage);

        }

        private void WebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs webRequestSuccessEventArgs = (WebRequestSuccessEventArgs)e;
            Log.Info("web请求成功");
            Log.Info(webRequestSuccessEventArgs.GetWebResponseText());
            Log.Debug(Utility.Converter.GetString(webRequestSuccessEventArgs.GetWebResponseBytes()));
        }
    }
}

public class CasesInfo
{
    public int Id;
    public string Name;
    public List<string> MainAcupoints;//主穴  //原生数据需要再次解析
    public bool UseAdjunct;//包含配穴
    public List<string> AdjunctAcupoints;//配穴
    //解析后的病例的穴位
    public List<List<string>> ParsedAcupoints = new List<List<string>>();

    public void Parse()
    {

        foreach (var item in MainAcupoints)
        {
            List<string> data = item.Split("、").ToList();
            ParsedAcupoints.Add(data);
        }
        if (UseAdjunct)
        {
            foreach (var item in AdjunctAcupoints)
            {
                string[] data = item.Split("、");
                foreach (var item1 in data)
                {
                    ParsedAcupoints[0].Add(item1);
                }
            }
        }


    }
}