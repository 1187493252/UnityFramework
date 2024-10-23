/*
* FileName:          UnityInitialzeOnLoad
* CompanyName:       
* Author:            relly
* Description:       Unity在启动时执行操作
* 
*/
#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace UnityFramework.Editor
{
    public partial class UnityInitialzeOnLoad
    {
        [InitializeOnLoadMethod]
        static void InitialzeOnLoad()
        {
            ScriptingDefineSymbolsSettingWindows.UnityInitialzeOnLoadAddMacros();
            AutoSave();
        }
        /// <summary>
        /// 自动保存
        /// </summary>
        static void AutoSave()
        {
            EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
                {
                    Debug.Log("正在保存所有已打开的场景  " + state);
                    EditorSceneManager.SaveOpenScenes();
                    AssetDatabase.SaveAssets();
                }
            };
        }
    }
}

#endif