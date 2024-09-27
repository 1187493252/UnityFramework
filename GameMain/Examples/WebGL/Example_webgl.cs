/*
* FileName:          Example_webgl
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityFramework;
using UnityFramework.Runtime;
using Application = UnityEngine.Application;

public class Example_webgl : MonoBehaviour
{
    public InputField InputField_URL;


    public InputField InputField_Hint;

    public InputField InputField_Txt;




    public RectTransform scrollViewcontent;
    public Image image;
    public InputField InputField_X;
    public InputField InputField_Y;
    public InputField InputField_W;
    public InputField InputField_H;

    public AudioSource audioSource;

    private void Start()
    {
        UnityFramework.Runtime.ComponentEntry.OnInitFinish += delegate { Init(); };
    }
    void Init()
    {
        UnityFramework.Runtime.ComponentEntry.ExternalCall.ReceiveCallBack = (fileType, data) =>
        {
            switch (fileType)
            {
                case FileType.txt:
                case FileType.json:
                case FileType.xml:
                case FileType.html:
                    scrollViewcontent.GetComponentInChildren<Text>().text = data.ToString();
                    scrollViewcontent.ForceRebuildLayoutImmediate();
                    break;
                case FileType.png:
                case FileType.jpg:
                    RectTransform rectTransform = image.GetComponent<RectTransform>();
                    Sprite sprite = SpriteUtil.GetSpriteBy64Str(data.ToString(), (int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y);
                    image.sprite = sprite;
                    break;
                case FileType.mp3:
                case FileType.wav:
                    AudioClip clip = (AudioClip)data;
                    if (clip)
                    {
                        audioSource.clip = clip;
                        audioSource.Play();
                        scrollViewcontent.GetComponentInChildren<Text>().text = clip.name;
                        scrollViewcontent.ForceRebuildLayoutImmediate();
                    }
                    break;
                default:
                    break;


            }
        };
    }

    public void PlayParseAudio()
    {
        if (audioSource.clip)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }


        }
    }

    public void GetCurrentURL()
    {
        PlatformExcute(
            () => { },
            () => UnityFramework.Runtime.ComponentEntry.ExternalCall.GetCurrentURL());
    }

    /// <summary>
    /// 打开网址
    /// </summary>
    public void OpenUrl()
    {
        PlatformExcute(
            () => Application.OpenURL(InputField_URL.text),
            () => UnityFramework.Runtime.ComponentEntry.ExternalCall.OpenURL(InputField_URL.text));
    }

    /// <summary>
    /// 弹框信息
    /// </summary>
    public void AlertInfo()
    {
        PlatformExcute(delegate
        {
#if UNITY_EDITOR||UNITY_STANDALONE

            int index = FileOperation.MessgeBox(InputField_Hint.text + "", "提示");

            if (index == 1)
            {
                Debug.Log("确定");
            }

#endif



        }, () => UnityFramework.Runtime.ComponentEntry.ExternalCall.AlertInfo(InputField_Hint.text));
    }


    public void SaveFile()
    {
        PlatformExcute(delegate
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            FileOperation.GetSaveFileFullPath(FileType.txt, delegate (string path)
            {
                File.WriteAllText(path, InputField_Txt.text);
                Debug.Log("保存完成，路径为:" + path);
            });
#endif
        }, delegate
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(InputField_Txt.text);
            string content = Convert.ToBase64String(b);
            UnityFramework.Runtime.ComponentEntry.ExternalCall.Download(content, "新建文本.txt");

        });
    }

    public void SavePic()
    {
        float capX = float.Parse(InputField_X.text);
        float capY = float.Parse(InputField_Y.text);
        int capWidth = int.Parse(InputField_W.text);
        int capHeight = int.Parse(InputField_H.text);
        PlatformExcute(delegate
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            FileOperation.GetSaveFileFullPath(FileType.png, delegate (string path)
            {
                ScreenUtil.CaptureCamera(Camera.main, new Rect(capX, capY, capWidth, capHeight), delegate (byte[] data)
                {

                    File.WriteAllBytes(path, data);
                    Debug.Log("保存完成，路径为:" + path);
                });

            });
#endif
        }, delegate
        {
            ScreenUtil.CaptureCamera(Camera.main, new Rect(capX, capY, capWidth, capHeight), delegate (byte[] data)
            {

                UnityFramework.Runtime.ComponentEntry.ExternalCall.Download(Convert.ToBase64String(data), "picture.png");
                Debug.LogError("测试截图 转Base64String");


            });

        });
    }

    /// <summary>
    /// 读取文件
    /// </summary>
    public void ReadFile()
    {
        PlatformExcute(() =>
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            FileOperation.GetOpenFileFullPath(callBack: (filetype, filepath) =>
             {
                 byte[] data = File.ReadAllBytes(filepath.ToString());

                 switch (filetype)
                 {
                     case FileType.txt:
                     case FileType.json:
                     case FileType.xml:
                     case FileType.html:
                         string resultText = Encoding.UTF8.GetString(data);
                         scrollViewcontent.GetComponentInChildren<Text>().text = resultText;
                         scrollViewcontent.ForceRebuildLayoutImmediate();
                         break;
                     case FileType.png:
                     case FileType.jpg:
                         RectTransform rectTransform = image.GetComponent<RectTransform>();
                         Sprite sprite = SpriteUtil.ConverSpriteByData(data, (int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y);
                         image.sprite = sprite;
                         break;
                     case FileType.mp3:
                     case FileType.wav:
                         //用unitywebrequest下载
                         ComponentEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, LoadWebRequestSuccess);
                         ComponentEntry.WebRequest.AddWebRequest(filepath.ToString());
                         break;
                     default:
                         break;
                 }
             });


#endif
            ;
        }, () =>
        {
            UnityFramework.Runtime.ComponentEntry.ExternalCall.ReadFile();

        });
    }

    private void LoadWebRequestSuccess(object sender, GameEventArgs e)
    {
        WebRequestSuccessEventArgs webRequestSuccessEventArgs = (WebRequestSuccessEventArgs)e;
        AudioClip clip = UtilityTools.ConvertBytesToClip(webRequestSuccessEventArgs.GetWebResponseBytes());
        if (clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
            scrollViewcontent.GetComponentInChildren<Text>().text = clip.name;
            scrollViewcontent.ForceRebuildLayoutImmediate();
        }
    }





    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        PlatformExcute(SystemUtil.Exit, UnityFramework.Runtime.ComponentEntry.ExternalCall.ExitGame);
    }

    /// <summary>
    /// 平台逻辑执行
    /// </summary>
    /// <param name="commonAction"></param>
    /// <param name="webAction"></param>
    private void PlatformExcute(Action commonAction, Action webAction)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            webAction?.Invoke();
#else
        commonAction?.Invoke();
#endif
    }


}
