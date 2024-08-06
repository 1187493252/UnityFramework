/*
* FileName:          WebRequestComponent
* CompanyName:       
* Author:            relly
* Description:       
*/


using Framework;
using Framework.WebRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;


namespace UnityFramework.Runtime
{
    /// <summary>
    /// MIME类型
    /// </summary> 
    public enum MIMEType
    {
        Json,           //json
        Xml             //xml
    }

    /// <summary>
    /// web请求助手
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class WebRequestComponent : UnityFrameworkComponent
    {
        private const int DefaultPriority = 0;

        private IWebRequestManager m_WebRequestManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private string m_WebRequestAgentHelperTypeName = "UnityFramework.Runtime.UnityWebRequestAgentHelper";

        [SerializeField]
        private WebRequestAgentHelperBase m_CustomWebRequestAgentHelper = null;

        [SerializeField]
        private int m_WebRequestAgentHelperCount = 1;

        [SerializeField]
        private float m_Timeout = 30f;

        /// <summary>
        /// 获取 Web 请求代理总数量。
        /// </summary>
        public int TotalAgentCount
        {
            get
            {
                return m_WebRequestManager.TotalAgentCount;
            }
        }

        /// <summary>
        /// 获取可用 Web 请求代理数量。
        /// </summary>
        public int FreeAgentCount
        {
            get
            {
                return m_WebRequestManager.FreeAgentCount;
            }
        }

        /// <summary>
        /// 获取工作中 Web 请求代理数量。
        /// </summary>
        public int WorkingAgentCount
        {
            get
            {
                return m_WebRequestManager.WorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待 Web 请求数量。
        /// </summary>
        public int WaitingTaskCount
        {
            get
            {
                return m_WebRequestManager.WaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置 Web 请求超时时长，以秒为单位。
        /// </summary>
        public float Timeout
        {
            get
            {
                return m_WebRequestManager.Timeout;
            }
            set
            {
                m_WebRequestManager.Timeout = m_Timeout = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            m_WebRequestManager = FrameworkEntry.GetModule<IWebRequestManager>();
            if (m_WebRequestManager == null)
            {
                Log.Fatal("Web request manager is invalid.");
                return;
            }

            m_WebRequestManager.Timeout = m_Timeout;
            m_WebRequestManager.WebRequestStart += OnWebRequestStart;
            m_WebRequestManager.WebRequestSuccess += OnWebRequestSuccess;
            m_WebRequestManager.WebRequestFailure += OnWebRequestFailure;
        }

        private void Start()
        {
            m_EventComponent = UnityFrameworkEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = new GameObject("Web Request Agent Instances").transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_WebRequestAgentHelperCount; i++)
            {
                AddWebRequestAgentHelper(i);
            }
        }

        /// <summary>
        /// 根据 Web 请求任务的序列编号获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="serialId">要获取信息的 Web 请求任务的序列编号。</param>
        /// <returns>Web 请求任务的信息。</returns>
        public TaskInfo GetWebRequestInfo(int serialId)
        {
            return m_WebRequestManager.GetWebRequestInfo(serialId);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="tag">要获取信息的 Web 请求任务的标签。</param>
        /// <returns>Web 请求任务的信息。</returns>
        public TaskInfo[] GetWebRequestInfos(string tag)
        {
            return m_WebRequestManager.GetWebRequestInfos(tag);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="tag">要获取信息的 Web 请求任务的标签。</param>
        /// <param name="results">Web 请求任务的信息。</param>
        public void GetAllWebRequestInfos(string tag, List<TaskInfo> results)
        {
            m_WebRequestManager.GetAllWebRequestInfos(tag, results);
        }

        /// <summary>
        /// 获取所有 Web 请求任务的信息。
        /// </summary>
        /// <returns>所有 Web 请求任务的信息。</returns>
        public TaskInfo[] GetAllWebRequestInfos()
        {
            return m_WebRequestManager.GetAllWebRequestInfos();
        }

        /// <summary>
        /// 获取所有 Web 请求任务的信息。
        /// </summary>
        /// <param name="results">所有 Web 请求任务的信息。</param>
        public void GetAllWebRequestInfos(List<TaskInfo> results)
        {
            m_WebRequestManager.GetAllWebRequestInfos(results);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, null, null, DefaultPriority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, postData, null, null, DefaultPriority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, null, DefaultPriority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, null, tag, DefaultPriority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, int priority, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, null, null, priority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, null, null, DefaultPriority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, postData, null, tag, DefaultPriority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, string tag, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, tag, DefaultPriority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, int priority, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, postData, null, null, priority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, int priority, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, null, priority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, postData, null, null, DefaultPriority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, null, DefaultPriority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, int priority, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, null, tag, priority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, null, tag, DefaultPriority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, int priority, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, null, null, priority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, int priority, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, postData, null, tag, priority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, string tag, int priority, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, tag, priority, null, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, postData, null, tag, DefaultPriority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, string tag, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, tag, DefaultPriority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, int priority, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, postData, null, null, priority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, int priority, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, null, priority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, int priority, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, null, tag, priority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, int priority, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, postData, null, tag, priority, userData, headers);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, string tag, int priority, object userData, Dictionary<string, string> headers = null)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, tag, priority, userData, headers);
        }

        /// <summary>
        /// 根据 Web 请求任务的序列编号移除 Web 请求任务。
        /// </summary>
        /// <param name="serialId">要移除 Web 请求任务的序列编号。</param>
        /// <returns>是否移除 Web 请求任务成功。</returns>
        public bool RemoveWebRequest(int serialId)
        {
            return m_WebRequestManager.RemoveWebRequest(serialId);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签移除 Web 请求任务。
        /// </summary>
        /// <param name="tag">要移除 Web 请求任务的标签。</param>
        /// <returns>移除 Web 请求任务的数量。</returns>
        public int RemoveWebRequests(string tag)
        {
            return m_WebRequestManager.RemoveWebRequests(tag);
        }

        /// <summary>
        /// 移除所有 Web 请求任务。
        /// </summary>
        /// <returns>移除 Web 请求任务的数量。</returns>
        public int RemoveAllWebRequests()
        {
            return m_WebRequestManager.RemoveAllWebRequests();
        }

        /// <summary>
        /// 增加 Web 请求代理辅助器。
        /// </summary>
        /// <param name="index">Web 请求代理辅助器索引。</param>
        private void AddWebRequestAgentHelper(int index)
        {
            WebRequestAgentHelperBase webRequestAgentHelper = Helper.CreateHelper(m_WebRequestAgentHelperTypeName, m_CustomWebRequestAgentHelper, index);
            if (webRequestAgentHelper == null)
            {
                Log.Error("Can not create web request agent helper.");
                return;
            }

            webRequestAgentHelper.name = Framework.Utility.Text.Format("Web Request Agent Helper - {0}", index.ToString());
            Transform transform = webRequestAgentHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            m_WebRequestManager.AddWebRequestAgentHelper(webRequestAgentHelper);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        private int AddWebRequest(string webRequestUri, byte[] postData, WWWForm wwwForm, string tag, int priority, object userData, Dictionary<string, string> headers)
        {
            return m_WebRequestManager.AddWebRequest(webRequestUri, postData, tag, priority, WWWFormInfo.Create(wwwForm, userData, headers));
        }

        private void OnWebRequestStart(object sender, Framework.WebRequest.WebRequestStartEventArgs e)
        {
            m_EventComponent.Fire(this, WebRequestStartEventArgs.Create(e));
        }

        private void OnWebRequestSuccess(object sender, Framework.WebRequest.WebRequestSuccessEventArgs e)
        {
            m_EventComponent.Fire(this, WebRequestSuccessEventArgs.Create(e));
        }

        private void OnWebRequestFailure(object sender, Framework.WebRequest.WebRequestFailureEventArgs e)
        {
            Log.Warning("Web request failure, web request serial id '{0}', web request uri '{1}', error message '{2}'.", e.SerialId.ToString(), e.WebRequestUri, e.ErrorMessage);
            m_EventComponent.Fire(this, WebRequestFailureEventArgs.Create(e));
        }

        #region[HttpWebRequest-不支持WebGL平台和本地文件读取]

        /// <summary>
        /// HttpWebRequest Get请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="headerDic">头文件字典</param>
        /// <param name="paramsDic">参数字典</param>
        /// <param name="textCallBack">文本内容回调</param>
        /// <param name="dataCallBack">二进制回调</param>
        /// <param name="failCallBack">请求失败回调</param>
        /// <returns></returns>
        public void HttpGet(string url, Dictionary<string, string> headerDic = null, Dictionary<string, string> paramsDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        {
            string tempUrl;
            if (paramsDic != null && paramsDic.Count >= 1)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(url);
                builder.Append("?");
                int i = 0;
                foreach (var item in paramsDic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }

                tempUrl = builder.ToString();
            }
            else
            {
                tempUrl = url;
            }

            HttpWebRequest request = (HttpWebRequest)global::System.Net.WebRequest.Create(tempUrl);

            if (headerDic != null && headerDic.Count >= 1)
            {
                foreach (var item in headerDic)
                {
                    request.Headers[item.Key] = item.Value;
                }
            }

            request.Method = "GET";
            request.ContentType = "application/json;charset=utf-8";

            try
            {
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());

                string text = reader.ReadToEnd();

                MemoryStream ms = new MemoryStream();
                reader.BaseStream.CopyTo(ms);
                byte[] data = ms.ToArray();
                reader.Close();
                ms.Close();

                textCallBack?.Invoke(text);
                dataCallBack?.Invoke(data);

            }
            catch (WebException ex)
            {
                Debug.LogError(ex.Message);
                failCallBack?.Invoke();
            }
        }

        /// <summary>
        /// HttpWebRequest Post请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="requestParam">请求参数</param>
        /// <param name="mimeType">MIME类型</param>
        /// <param name="headerDic">头文件字典</param>
        /// <param name="textCallBack">文本内容回调</param>
        /// <param name="dataCallBack">二进制回调</param>
        /// <param name="failCallBack">请求失败回调</param>
        /// <returns></returns>
        public void HttpPost(string url, string requestParam, MIMEType mimeType = MIMEType.Json, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        {
            HttpWebRequest request = (HttpWebRequest)global::System.Net.WebRequest.Create(url);

            if (headerDic != null && headerDic.Count >= 1)
            {
                foreach (var item in headerDic)
                {
                    request.Headers[item.Key] = item.Value;
                }
            }

            request.Method = "POST";
            switch (mimeType)
            {
                case MIMEType.Json: request.ContentType = "application/json;charset=utf-8"; break;
                case MIMEType.Xml: request.ContentType = "application/xml;charset=utf-8"; break;
            }

            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            requestParam = reg.Replace(requestParam, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

            byte[] jsonData = Encoding.UTF8.GetBytes(requestParam);
            request.ContentLength = jsonData.Length;

            try
            {
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(jsonData, 0, jsonData.Length);
                reqStream.Close();

                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());

                string text = reader.ReadToEnd();

                MemoryStream ms = new MemoryStream();
                reader.BaseStream.CopyTo(ms);
                byte[] data = ms.ToArray();
                reader.Close();
                ms.Close();

                textCallBack?.Invoke(text);
                dataCallBack?.Invoke(data);

            }
            catch (WebException ex)
            {
                Debug.LogError(ex.Message);
                failCallBack?.Invoke();
            }
        }

        /// <summary>
        /// HttpWebRequest Post请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="formDic">表单键值对</param>
        /// <param name="headerDic">头文件字典</param>
        /// <param name="textCallBack">文本内容回调</param>
        /// <param name="dataCallBack">二进制回调</param>
        /// <param name="failCallBack">请求失败回调</param>
        /// <returns></returns>
        public void HttpPost(string url, Dictionary<string, string> formDic, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        {
            HttpWebRequest request = (HttpWebRequest)global::System.Net.WebRequest.Create(url);

            if (headerDic != null && headerDic.Count >= 1)
            {
                foreach (var item in headerDic)
                {
                    request.Headers[item.Key] = item.Value;
                }
            }

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in formDic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] formData = Encoding.UTF8.GetBytes(builder.ToString());
            request.ContentLength = formData.Length;

            try
            {
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(formData, 0, formData.Length);
                reqStream.Close();

                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                string text = reader.ReadToEnd();
                MemoryStream ms = new MemoryStream();
                reader.BaseStream.CopyTo(ms);
                byte[] data = ms.ToArray();
                reader.Close();
                ms.Close();

                textCallBack?.Invoke(text);
                dataCallBack?.Invoke(data);

            }
            catch (WebException ex)
            {
                Debug.LogError(ex.Message);
                failCallBack?.Invoke();
            }
        }

        /// <summary>
        /// HttpWebRequest Put请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="requestParam">请求参数</param>
        /// <param name="mimeType">MIME类型</param>
        /// <param name="headerDic">头文件字典</param>
        /// <param name="textCallBack">文本内容回调</param>
        /// <param name="dataCallBack">二进制回调</param>
        /// <param name="failCallBack">请求失败回调</param>
        /// <returns></returns>
        public void HttpPut(string url, string requestParam, MIMEType mimeType = MIMEType.Json, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        {
            HttpWebRequest request = (HttpWebRequest)global::System.Net.WebRequest.Create(url);

            if (headerDic != null && headerDic.Count >= 1)
            {
                foreach (var item in headerDic)
                {
                    request.Headers[item.Key] = item.Value;
                }
            }

            request.Method = "PUT";
            switch (mimeType)
            {
                case MIMEType.Json: request.ContentType = "application/json;charset=utf-8"; break;
                case MIMEType.Xml: request.ContentType = "application/xml;charset=utf-8"; break;
            }

            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            requestParam = reg.Replace(requestParam, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

            byte[] jsonData = Encoding.UTF8.GetBytes(requestParam);
            request.ContentLength = jsonData.Length;

            try
            {
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(jsonData, 0, jsonData.Length);
                reqStream.Close();

                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());

                string text = reader.ReadToEnd();

                MemoryStream ms = new MemoryStream();
                reader.BaseStream.CopyTo(ms);
                byte[] data = ms.ToArray();
                reader.Close();
                ms.Close();

                textCallBack?.Invoke(text);
                dataCallBack?.Invoke(data);

            }
            catch (WebException ex)
            {
                Debug.LogError(ex.Message);
                failCallBack?.Invoke();
            }
        }

        /// <summary>
        /// HttpWebRequest Delete请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="requestParam">请求参数</param>
        /// <param name="mimeType">MIME类型</param>
        /// <param name="headerDic">头文件字典</param>
        /// <param name="textCallBack">文本内容回调</param>
        /// <param name="dataCallBack">二进制回调</param>
        /// <param name="failCallBack">请求失败回调</param>
        /// <returns></returns>
        public void HttpDelete(string url, string requestParam, MIMEType mimeType = MIMEType.Json, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        {
            HttpWebRequest request = (HttpWebRequest)global::System.Net.WebRequest.Create(url);

            if (headerDic != null && headerDic.Count >= 1)
            {
                foreach (var item in headerDic)
                {
                    request.Headers[item.Key] = item.Value;
                }
            }

            request.Method = "DELETE";
            switch (mimeType)
            {
                case MIMEType.Json: request.ContentType = "application/json;charset=utf-8"; break;
                case MIMEType.Xml: request.ContentType = "application/xml;charset=utf-8"; break;
            }

            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            requestParam = reg.Replace(requestParam, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

            byte[] jsonData = Encoding.UTF8.GetBytes(requestParam);
            request.ContentLength = jsonData.Length;

            try
            {
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(jsonData, 0, jsonData.Length);
                reqStream.Close();

                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());

                string text = reader.ReadToEnd();

                MemoryStream ms = new MemoryStream();
                reader.BaseStream.CopyTo(ms);
                byte[] data = ms.ToArray();
                reader.Close();
                ms.Close();

                textCallBack?.Invoke(text);
                dataCallBack?.Invoke(data);

            }
            catch (WebException ex)
            {
                Debug.LogError(ex.Message);
                failCallBack?.Invoke();
            }
        }

        #endregion

        #region[UnityWebRequest-支持全平台和本地文件读取]

        /// <summary>
        /// UnityWebRequest Get请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="headerDic">头文件字典</param>
        /// <param name="paramsDic">参数字典</param>
        /// <param name="textCallBack">文本内容回调</param>
        /// <param name="dataCallBack">二进制回调</param>
        /// <param name="failCallBack">请求失败回调</param>
        /// <returns></returns>
        public void RequestGet(string url, Dictionary<string, string> headerDic = null, Dictionary<string, string> paramsDic = null, Action<string, byte[]> callBack = null, Action failCallBack = null)
        { StartCoroutine(UnityWebRequestGet(url, headerDic, paramsDic, callBack, failCallBack)); }




        /// <summary>
        /// UnityWebRequest Post请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="requestParam">请求参数</param>
        /// <param name="mimeType">MIME类型</param>
        /// <param name="headerDic">头文件字典</param>
        /// <param name="textCallBack">文本内容回调</param>
        /// <param name="dataCallBack">二进制回调</param>
        /// <param name="failCallBack">请求失败回调</param>
        /// <returns></returns>
        public void RequestPost(string url, string requestParam, MIMEType mimeType = MIMEType.Json, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        { StartCoroutine(UnityWebRequestPost(url, requestParam, mimeType, headerDic, textCallBack, dataCallBack, failCallBack)); }

        /// <summary>
        /// UnityWebRequest Post请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="formDic">表单参数键值对</param>
        /// <param name="headerDic">头文件字典</param>
        /// <param name="textCallBack">文本内容回调</param>
        /// <param name="dataCallBack">二进制回调</param>
        /// <param name="failCallBack">请求失败回调</param>
        /// <returns></returns>
        public void RequestPost(string url, Dictionary<string, string> formDic, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        { StartCoroutine(UnityWebRequestPost(url, formDic, headerDic, textCallBack, dataCallBack, failCallBack)); }

        /// <summary>
        /// UnityWebRequest Put请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="requestParam">请求参数</param>
        /// <param name="mimeType">MIME类型</param>
        /// <param name="headerDic">头文件字典</param>
        /// <param name="textCallBack">文本内容回调</param>
        /// <param name="dataCallBack">二进制回调</param>
        /// <param name="failCallBack">请求失败回调</param>
        /// <returns></returns>
        public void RequestPut(string url, string requestParam, MIMEType mimeType = MIMEType.Json, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        { StartCoroutine(UnityWebRequestPut(url, requestParam, mimeType, headerDic, textCallBack, dataCallBack, failCallBack)); }

        /// <summary>
        /// UnityWebRequest Delete请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="requestParam">请求参数</param>
        /// <param name="mimeType">MIME类型</param>
        /// <param name="headerDic">头文件字典</param>
        /// <param name="textCallBack">文本内容回调</param>
        /// <param name="dataCallBack">二进制回调</param>
        /// <param name="failCallBack">请求失败回调</param>
        /// <returns></returns>
        public void RequestDelete(string url, string requestParam, MIMEType mimeType = MIMEType.Json, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        { StartCoroutine(UnityWebRequestDelete(url, requestParam, mimeType, headerDic, textCallBack, dataCallBack, failCallBack)); }

        IEnumerator UnityWebRequestGet(string url, Dictionary<string, string> headerDic = null, Dictionary<string, string> paramsDic = null, Action<string, byte[]> callBack = null, Action failCallBack = null)
        {
            string tempUrl;
            if (paramsDic != null && paramsDic.Count >= 1)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(url);
                builder.Append("?");
                int i = 0;
                foreach (var item in paramsDic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }

                tempUrl = builder.ToString();
            }
            else
            {
                tempUrl = url;
            }

            UnityWebRequest request = UnityWebRequest.Get(tempUrl);

            if (headerDic != null && headerDic.Count >= 1)
            {
                foreach (var item in headerDic)
                {
                    request.SetRequestHeader(item.Key, item.Value);
                }
            }

            if (paramsDic != null && paramsDic.Count >= 1)
            { request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); }

            yield return request.SendWebRequest();


#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
#else
			if (request.isHttpError || request.isNetworkError)
#endif
            {
                Debug.LogError(request.error);
                failCallBack?.Invoke();
            }
            else
            {
                string text = request.downloadHandler.text;
                byte[] data = request.downloadHandler.data;

                callBack?.Invoke(text, data);
            }
        }

        IEnumerator UnityWebRequestPost(string url, string requestParam, MIMEType mimeType = MIMEType.Json, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(requestParam);
            UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw(bytes),
                downloadHandler = new DownloadHandlerBuffer()
            };

            if (headerDic != null && headerDic.Count >= 1)
            {
                foreach (var item in headerDic)
                {
                    request.SetRequestHeader(item.Key, item.Value);
                }
            }

            switch (mimeType)
            {
                case MIMEType.Json: request.SetRequestHeader("Content-Type", "application/json;charset=utf-8"); break;
                case MIMEType.Xml: request.SetRequestHeader("Content-Type", "application/xml;charset=utf-8"); break;
            }

            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
                failCallBack?.Invoke();
            }
            else
            {
                string text = request.downloadHandler.text;
                byte[] data = request.downloadHandler.data;
                textCallBack?.Invoke(text);
                dataCallBack?.Invoke(data);
            }
        }

        IEnumerator UnityWebRequestPost(string url, Dictionary<string, string> formDic, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        {
            WWWForm form = new WWWForm();

            foreach (var item in formDic) { form.AddField(item.Key, item.Value); }

            UnityWebRequest request = UnityWebRequest.Post(url, form);
            if (headerDic != null && headerDic.Count >= 1)
            {
                foreach (var item in headerDic)
                {
                    request.SetRequestHeader(item.Key, item.Value);
                }
            }
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded;");
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
                failCallBack?.Invoke();
            }
            else
            {
                string text = request.downloadHandler.text;
                byte[] data = request.downloadHandler.data;
                textCallBack?.Invoke(text);
                dataCallBack?.Invoke(data);
            }
        }

        IEnumerator UnityWebRequestPut(string url, string requestParam, MIMEType mimeType = MIMEType.Json, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(requestParam);
            UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT)
            {
                uploadHandler = new UploadHandlerRaw(bytes),
                downloadHandler = new DownloadHandlerBuffer()
            };

            if (headerDic != null && headerDic.Count >= 1)
            {
                foreach (var item in headerDic)
                {
                    request.SetRequestHeader(item.Key, item.Value);
                }
            }

            switch (mimeType)
            {
                case MIMEType.Json: request.SetRequestHeader("Content-Type", "application/json;charset=utf-8"); break;
                case MIMEType.Xml: request.SetRequestHeader("Content-Type", "application/xml;charset=utf-8"); break;
            }

            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
                failCallBack?.Invoke();
            }
            else
            {
                string text = request.downloadHandler.text;
                byte[] data = request.downloadHandler.data;
                textCallBack?.Invoke(text);
                dataCallBack?.Invoke(data);
            }
        }

        IEnumerator UnityWebRequestDelete(string url, string requestParam, MIMEType mimeType = MIMEType.Json, Dictionary<string, string> headerDic = null, Action<string> textCallBack = null, Action<byte[]> dataCallBack = null, Action failCallBack = null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(requestParam);
            UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbDELETE)
            {
                uploadHandler = new UploadHandlerRaw(bytes),
                downloadHandler = new DownloadHandlerBuffer()
            };

            if (headerDic != null && headerDic.Count >= 1)
            {
                foreach (var item in headerDic)
                {
                    request.SetRequestHeader(item.Key, item.Value);
                }
            }

            switch (mimeType)
            {
                case MIMEType.Json: request.SetRequestHeader("Content-Type", "application/json;charset=utf-8"); break;
                case MIMEType.Xml: request.SetRequestHeader("Content-Type", "application/xml;charset=utf-8"); break;
            }

            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
                failCallBack?.Invoke();
            }
            else
            {
                string text = request.downloadHandler.text;
                byte[] data = request.downloadHandler.data;
                textCallBack?.Invoke(text);
                dataCallBack?.Invoke(data);
            }
        }

        #endregion
    }
}

