/*
* FileName:          ExternalCall
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileType
    {
        txt,
        json,
        xml,
        doc,
        docx,
        xls,
        xlsx,
        mp3,
        wav,
        mp4,
        jpg,
        png,
        html,
        all
    }

    [DisallowMultipleComponent]
    public class ExternalCallComponent : UnityFrameworkComponent
    {

        public void GetCurrentURL()
        {

            Application.ExternalCall("GetCurrentURL");

        }

        /// <summary>
        /// 立即刷新IndexedDB
        /// </summary>
        public void SyncDB()
        {
            Application.ExternalCall("SyncDB");
        }

        /// <summary>
        /// 设置ciikie 本地服务器不可用
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="saveDays">保存时间,单位天数</param>
        public void SetCookie(string key, string value, int saveDays)
        {
            Application.ExternalCall("SetCookie", key, value, saveDays);
        }

        public void GetCookie(string key)
        {
            Application.ExternalCall("GetCookie", key);
        }

        /// <summary>
        /// 下载到本地
        /// </summary>
        /// <param name="base64Str">base64位字符串</param>
        /// <param name="fileName">文件名称,带后缀</param>
        public void Download(string base64Str, string fileName)
        {
            Application.ExternalCall("Download", base64Str, fileName);
        }

        /// <summary>
        /// 新页面打开url
        /// </summary>
        /// <param name="url">链接地址</param>
        public void OpenURL(string url)
        {
            Application.ExternalCall("OpenURL", url);
        }
        public void OpenURL1(string url)
        {
            Application.ExternalCall("OpenURL1", url);
        }

        /// <summary>
        /// 浏览器弹框信息
        /// </summary>
        /// <param name="content">信息内容</param>
        public void AlertInfo(string content)
        {
            Application.ExternalCall("AlertInfo", content);
        }

        /// <summary>
        /// Web退出游戏
        /// </summary>
        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
            Exit();
#endif

        }

        void Exit()
        {
#if UNITY_WEBGL
            Application.ExternalCall("Exit");
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        public void ReadFile()
        {
            Application.ExternalCall("SelectFile");
        }


        public Action<FileType, object> ReceiveCallBack { get; set; }           //接受数据委托

        /// <summary>
        /// 接受文件数据
        /// </summary>
        /// <param name="json">结束的base64位字符串</param>
        private void ReceiveFileData(string json)
        {
            ReceiveJson receiveJson = Seriallize.JsonDeserialization<ReceiveJson>(json);

            switch (receiveJson.FileType)
            {
                case FileType.txt:
                case FileType.xml:
                case FileType.json:
                case FileType.html:
                    byte[] datas = Convert.FromBase64String(receiveJson.receiveContent);
                    string resultText = Encoding.UTF8.GetString(datas);
                    ReceiveCallBack?.Invoke(receiveJson.FileType, resultText);
                    break;
                case FileType.png:
                case FileType.jpg:
                case FileType.xls:
                    ReceiveCallBack?.Invoke(receiveJson.FileType, receiveJson.receiveContent);
                    break;
            }
        }

        /// <summary>
        /// 接受的Json数据
        /// </summary>
        class ReceiveJson
        {
            /// <summary>
            /// 文件类型
            /// </summary>
            public string fileType { set => FileType = (FileType)Enum.Parse(typeof(FileType), value); }

            /// <summary>
            /// 接受的数据内容
            /// </summary>
            public string receiveContent;

            public FileType FileType { get; private set; }
        }
    }
}
