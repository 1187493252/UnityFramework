/*
* FileName:          Main
* CompanyName:       
* Author:            
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;
using static UnityEngine.AddressableAssets.Addressables;

namespace AddressablesTest
{
    public class CheckUpdateAndDownload : MonoBehaviour
    {
        public Image Image;
        /// <summary>
        /// 显示下载状态和进度
        /// </summary>
        public Text updateText;

        /// <summary>
        /// 重试按钮
        /// </summary>
        public Button retryBtn;


        private void Awake()
        {
            retryBtn.gameObject.SetActive(false);
            retryBtn.onClick.AddListener(delegate ()
            {
                StartCoroutine(DoUpdateAddressadble());

            });
            StartCoroutine(DoUpdateAddressadble());


        }

        IEnumerator DoUpdateAddressadble()
        {
            // yield return InitAsync();
            yield return Addressables.InitializeAsync();
            var checkHandle = Addressables.CheckForCatalogUpdates(false);
            yield return checkHandle;
            updateText.text = updateText.text + "正在检测更新Catalogs ";
            if (checkHandle.Status != AsyncOperationStatus.Succeeded)
            {
                OnError(checkHandle.OperationException.ToString());
                yield break;
            }
            if (checkHandle.Result.Count > 0)
            {
                foreach (var item in checkHandle.Result)
                {
                    updateText.text = updateText.text + "\nCatalogs: " + item;
                }
                var updateHandle = Addressables.UpdateCatalogs(checkHandle.Result, false);
                yield return updateHandle;
                if (updateHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    OnError("UpdateCatalogs Error\n" + updateHandle.OperationException.ToString());
                    yield break;
                }
                // 更新列表迭代器
                foreach (var locator in updateHandle.Result)
                {
                    updateText.text = updateText.text + "\n Catalogs locatorId: " + locator.LocatorId;
                    //foreach (var item in locator.Keys)
                    //{
                    //    updateText.text = updateText.text + "\n Catalogs Key:" + item;
                    //}

                    List<object> keys = new List<object>();
                    keys.AddRange(locator.Keys);

                    // 获取待下载的文件总大小
                    var sizeHandle = Addressables.GetDownloadSizeAsync(keys);
                    yield return sizeHandle;
                    if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
                    {
                        OnError("GetDownloadSizeAsync Error\n" + sizeHandle.OperationException.ToString());
                        yield break;
                    }
                    updateText.text = updateText.text + "\n更新文件大小 : " + sizeHandle.Result;
                    if (sizeHandle.Result > 0)
                    {
                        // 下载
                        var downloadHandle = Addressables.DownloadDependenciesAsync(keys, MergeMode.Union);
                        while (!downloadHandle.IsDone)
                        {
                            if (downloadHandle.Status == AsyncOperationStatus.Failed)
                            {
                                OnError("DownloadDependenciesAsync Error\n" + downloadHandle.OperationException.ToString());
                                yield break;
                            }
                            // 下载进度
                            float percentage = downloadHandle.PercentComplete;
                            updateText.text = updateText.text + $"\n已下载: { (percentage * 100).ToString("f2")}%";
                            yield return null;
                        }
                        yield return downloadHandle;

                        if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            updateText.text = updateText.text + "\n下载完毕";

                            foreach (var item in downloadHandle.Result as List<IAssetBundleResource>)
                            {
                                var ab = item.GetAssetBundle();
                                updateText.text = updateText.text + "\n ab包名称:" + ab.name;

                                foreach (var name in ab.GetAllAssetNames())
                                {
                                    updateText.text = updateText.text + "\n ab包中资源名称:" + name;

                                }
                            }
                        }
                        Addressables.Release(downloadHandle);
                    }
                }
                Addressables.Release(updateHandle);
            }
            else
            {
                updateText.text = updateText.text + "\n没有检测到更新";
            }
            Addressables.Release(checkHandle);
            EnterGame();
        }

        // 异常提示
        private void OnError(string msg)
        {
            updateText.text = updateText.text + $"\n{msg}\n请重试! ";
            // 显示重试按钮
            retryBtn.gameObject.SetActive(true);
        }


        // 进入游戏
        void EnterGame()
        {
            Addressables.LoadAssetAsync<Sprite>("Assets/Framework/ExternalLibrary/Addressable/0.jpg").Completed += (handle) =>
            {
                Sprite sprite = handle.Result;
                Image.sprite = sprite;
            };
        }

        IEnumerator LoadScene(string senceName)
        {
            var handle = Addressables.LoadSceneAsync(senceName);
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("场景加载异常: " + handle.OperationException.ToString());
                yield break;
            }
            while (!handle.IsDone)
            {
                // 进度（0~1）
                float percentage = handle.PercentComplete;
                Debug.Log("进度: " + percentage);
                yield return null;
            }
            Debug.Log("场景加载完毕");
        }

        public async Task InitAsync()
        {
            //断点续传用
            string _catalogPath = Application.persistentDataPath + "/com.unity.addressables";
            if (Directory.Exists(_catalogPath))
            {
                try
                {
                    Directory.Delete(_catalogPath, true);
                    Debug.Log("delete catalog cache done!");
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            await Addressables.InitializeAsync().Task;
            Debug.Log("AssetManager init done");
        }
    }
}
