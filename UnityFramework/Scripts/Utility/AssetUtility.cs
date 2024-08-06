/*
* FileName:          AssetUtility
* CompanyName:  
* Author:            relly
* Description:       配置资源路径
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class AssetUtility
{
    public static string GetAudioAsset(string assetName)
    {
        return string.Format("Assets/Sounds/{0}.wav", assetName);
    }
    public static string GetVideoAsset(string assetName)
    {
        return string.Format("Assets/Videos/{0}.wav", assetName);
    }
}




