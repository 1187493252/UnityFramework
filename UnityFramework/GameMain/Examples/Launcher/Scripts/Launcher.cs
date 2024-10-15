/*
* FileName:          Launcher
* CompanyName:       
* Author:            relly
* Description:       
*/

using K_UnityGF;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityFramework;
using UnityFramework.Runtime;

public class Launcher : MonoBehaviour
{
    public string NextSceneName;

    private void Awake()
    {
        // 自动获取网络时间
        StartCoroutine(GetWebTime());
        StartCoroutine(GetTimeLockKey());


        ComponentEntry.OnInitFinish += delegate
        {
            CheckTimeLock();

        };
    }
    private void Update()
    {

        //ComponentEntry.InitFinish(delegate
        //{
        //    if (!NextSceneName.IsNullOrEmpty())
        //    {
        //        SceneManager.LoadScene(NextSceneName);
        //    }
        //});

    }
    void CheckTimeLock()
    {
        bool timelock = bool.Parse(ComponentEntry.Config.GetConfig("TimeLock"));
        if (timelock)
        {
            if (Global.Setting.TimeLockKey.IsNullOrEmpty())
            {
                ComponentEntry.ExternalCall.ExitGame();
            }
            else
            {
                ComponentEntry.WebRequest.RequestGet(Application.streamingAssetsPath + "/Data/UsageSettings.json", callBack: delegate (string content, byte[] data)
                {
                    string tmp = EncryptionUtil.Decrypt(content, Global.Setting.TimeLockKey);
                    JsonData jsonData = JsonHelper.ToObject(tmp);
                    string start = jsonData["Start"].ToString();
                    string end = jsonData["End"].ToString();
                    DateTime config_StartTime;
                    DateTime config_EndTime;
                    if (DateTime.TryParse(start, out config_StartTime) && DateTime.TryParse(end, out config_EndTime))
                    {
                        //要比初始时间大
                        if (Global.Setting.WebTime > config_StartTime)
                        {
                            //比预定时间小
                            if (Global.Setting.WebTime < config_EndTime)
                            {
                                string lastPassVerificationTime = PlayerPrefs.GetString("LastPassVerificationTime");
                                if (!lastPassVerificationTime.IsNullOrEmpty())
                                {
                                    //第N次打开
                                    if (DateTime.TryParse(lastPassVerificationTime, out Global.Setting.LastPassVerificationTime))
                                    {
                                        Debug.Log($"上一次通过验证时间:{lastPassVerificationTime}");

                                        //要比上次通过验证的时间大
                                        if (Global.Setting.WebTime > Global.Setting.LastPassVerificationTime)
                                        {
                                            //验证通过,重新设置通过验证的时间
                                            PlayerPrefs.SetString("LastPassVerificationTime", Global.Setting.WebTime.ToString("yyyy/MM/dd HH:mm"));
                                            if (!NextSceneName.IsNullOrEmpty())
                                            {
                                                SceneManager.LoadScene(NextSceneName);
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("WebTime比上次通过验证时间小");

                                            ComponentEntry.ExternalCall.ExitGame();
                                        }
                                    }
                                }
                                else
                                {
                                    //说明第一次打开
                                    PlayerPrefs.SetString("LastPassVerificationTime", Global.Setting.WebTime.ToString("yyyy/MM/dd HH:mm"));
                                    if (!NextSceneName.IsNullOrEmpty())
                                    {
                                        SceneManager.LoadScene(NextSceneName);
                                    }

                                }
                            }
                            else
                            {
                                Debug.LogError("WebTime比预定时间大");

                                ComponentEntry.ExternalCall.ExitGame();

                            }
                        }
                        else
                        {
                            Debug.LogError("WebTime比初始时间小");
                            ComponentEntry.ExternalCall.ExitGame();

                        }

                    }

                });
            }

        }
        else
        {
            if (!NextSceneName.IsNullOrEmpty())
            {
                SceneManager.LoadScene(NextSceneName);
            }
        }

    }

    IEnumerator GetTimeLockKey()
    {

        UnityWebRequest WebRequest = UnityWebRequest.Get(string.Format("{0}{1}", Application.streamingAssetsPath, "/Data/SecurityCode.json"));

        // 等待请求完成
        yield return WebRequest.SendWebRequest();
        if (WebRequest.isHttpError || WebRequest.isNetworkError)
        {

        }
        else
        {
            Global.Setting.TimeLockKey = WebRequest.downloadHandler.text;

        }
    }
    /// <summary>
    /// 获取当前网络时间
    /// </summary>
    /// <returns></returns>
    IEnumerator GetWebTime()
    {
        // 获取时间地址
        string url = "https://www.baidu.com"; // 百度 //http://www.beijing-time.org/"; // 北京时间


        DateTime _webNowTime = DateTime.Now;
        // 获取时间
        UnityWebRequest WebRequest = new UnityWebRequest(url);

        // 等待请求完成
        yield return WebRequest.SendWebRequest();

        //网页加载完成  并且下载过程中没有错误   string.IsNullOrEmpty 判断字符串是否是null 或者是" ",如果是返回true
        //WebRequest.error  下载过程中如果出现下载错误  会返回错误信息 如果下载没有完成那么将会阻塞到下载完成
        if (WebRequest.isDone && string.IsNullOrEmpty(WebRequest.error))
        {
            // 将返回值存为字典
            Dictionary<string, string> resHeaders = WebRequest.GetResponseHeaders();
            string key = "DATE";
            string value = null;
            // 获取key为"DATE" 的 Value值
            if (resHeaders != null && resHeaders.ContainsKey(key))
            {
                resHeaders.TryGetValue(key, out value);
            }

            if (value == null)
            {
                Debug.LogError("没有获取到key为DATE对应的Value值...WebTime设置为当前系统时间");
                Global.Setting.WebTime = _webNowTime;

                yield break;
            }

            // 取到了value，则进行转换为本地时间
            _webNowTime = FormattingGMT(value);
            Debug.Log(value + " ，转换后的网络时间：" + _webNowTime);
            Global.Setting.WebTime = _webNowTime;
            Debug.Log("网络时间转时间戳：" + _webNowTime.GetUTC10());

        }
        else
        {
            Global.Setting.WebTime = _webNowTime;
            Debug.LogError("网络时间获取失败...WebTime设置为当前系统时间");

        }
    }

    /// <summary>
    /// GMT(格林威治时间)时间转成本地时间
    /// </summary>
    /// <param name="gmt">字符串形式的GMT时间</param>
    /// <returns></returns>
    private DateTime FormattingGMT(string gmt)
    {
        DateTime dt = DateTime.Now;
        try
        {
            string pattern = "";
            if (gmt.IndexOf("+0") != -1)
            {
                gmt = gmt.Replace("GMT", "");
                pattern = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
            }

            if (gmt.ToUpper().IndexOf("GMT") != -1)
            {
                pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
            }

            if (pattern != "")
            {
                dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AdjustToUniversal);
                dt = dt.ToLocalTime();
            }
            else
            {
                dt = Convert.ToDateTime(gmt);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.LogError("时间转换错误...WebTime设置为当前系统时间");
        }
        return dt;
    }

}
