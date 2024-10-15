/*
* FileName:          UnityInitialzeOnLoad.AssetModificationProcessor
* CompanyName:       
* Author:            relly
* Description:       Project视图资源变化
* 
*/

using UnityEditor;
using UnityEngine;
namespace UnityFramework.Editor
{
    public partial class UnityInitialzeOnLoad
    {
        public class AssetModificationProcessor : UnityEditor.AssetModificationProcessor
        {
            //[InitializeOnLoadMethod]
            //static void AssetModificationProcessorInitialzeOnLoad()
            //{
            //    //全局监听Project视图下的资源是否发生变化（添加、删除、移动）
            //    EditorApplication.projectChanged += delegate ()
            //    {
            //        Debug.LogWarningFormat("change");
            //    };
            //}

            //监听双击鼠标左键打开资源事件
            public static bool IsOpenForEdit(string assetPath, out string message)
            {
                message = null;
                //  Debug.LogWarningFormat("AssetPath : {0} ", assetPath);
                //true表示该资源可以打开，false表示不允许在unity中打开该资源,设置都被禁用
                return true;
            }
            //监听资源即将被创建事件
            public static void OnWillCreateAsset(string path)
            {
                //   Debug.LogWarningFormat("path : {0}", path);
            }
            //监听资源即将被保存事件
            public static string[] OnWillSaveAssets(string[] paths)
            {
                if (paths != null)
                {
                    //  Debug.LogWarningFormat("path : {0}", string.Join(",", paths));
                }
                return paths;
            }
            //监听资源即将被移动事件
            public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
            {
                Debug.LogWarningFormat("from : {0} to : {1}", oldPath, newPath);
                //AssetMoveResult.DidNotMove表示该资源可以移动
                return AssetMoveResult.DidNotMove;
            }
            //监听资源即将被删除事件
            public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
            {
                Debug.LogWarningFormat("delete : {0}", assetPath);
                //AssetDeleteResult.DidNotDelete表示该资源可以被删除
                return AssetDeleteResult.DidNotDelete;
            }
        }
    }
}

