/*
* FileName:          WebRequestComponent
* CompanyName:       
* Author:            relly
* Description:       
*/


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

