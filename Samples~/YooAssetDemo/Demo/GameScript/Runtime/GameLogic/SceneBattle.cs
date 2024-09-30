using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Event;
using UniFramework.Machine;
using YooAsset;
using UnityFramework.Runtime;

internal class SceneBattle : MonoBehaviour
{

    private AssetHandle _entityHandle;


    private IEnumerator Start()
    {

        ComponentEntry.Entity.ShowEntity(1, "Entity", "default");
        yield break;
        _entityHandle = YooAssets.LoadAssetAsync<GameObject>("Entity");
        yield return _entityHandle;
        _entityHandle.InstantiateSync();






    }
    private void OnDestroy()
    {
        if (_entityHandle != null)
        {
            _entityHandle.Release();
            _entityHandle = null;
        }



        // 切换场景的时候释放资源
        if (YooAssets.Initialized)
        {
            var package = YooAssets.GetPackage("DefaultPackage");
            var operation = package.UnloadUnusedAssetsAsync();
            operation.WaitForAsyncComplete();
        }
    }

}