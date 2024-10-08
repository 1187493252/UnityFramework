/*
* FileName:          Main
* CompanyName:       
* Author:            
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


namespace AddressablesTest
{
    public class Main : MonoBehaviour
    {
        public AssetReference assetReference;
        public Image Image;

        private void Start()
        {
            //通过名称/标签单个加载
            //Addressables.LoadAssetAsync<GameObject>("Assets/Framework/Framework/Prefabs/UnityFramework.prefab").Completed += (handle) =>
            //{
            //    GameObject gameObject = Instantiate(handle.Result);
            //};
            //assetReference.InstantiateAsync().Completed += (handle) =>
            //{
            //    GameObject gameObject = handle.Result;
            //};
            //Addressables.InstantiateAsync("Assets/Framework/Framework/VR/VRIF/Prefabs/TeleportPoint_VRIF.prefab").Completed += (handle) =>
            //{
            //    GameObject gameObject = handle.Result;
            //};


            //通过标签批量加载
            Addressables.LoadAssetsAsync<GameObject>("default", (handle) =>
            {
                Debug.Log(handle.gameObject);
            });


            Addressables.LoadAssetAsync<Sprite>("Assets/Framework/ExternalLibrary/Addressable/0.jpg").Completed += (handle) =>
            {
                Sprite sprite = handle.Result;
                Image.sprite = sprite;
            };
        }
    }
}
