/*
* FileName:          ScriptingDefineSymbols
* CompanyName:       
* Author:            relly
* Description:       自定义Unity宏
* 
*/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace UnityFramework.Editor
{
    public class ScriptingDefineSymbolsSettingWindows : EditorWindow
    {
        Rect leftRect;
        Rect rightRect;
        Rect bottomtRect;
        private Vector2 LeftMenuSlider;
        private Vector2 RightMenuSlider;

        static string UnityMacro = null;
        static string Macro_Vive = "ViveInputUtility";
        static string Macro_VRTK4_OR_NEWER = "VRTK_VERSION_4_0_0_OR_NEWER";
        static string Macro_VRTK4_OR_OLDER = "VRTK_VERSION_3_3_0_OR_OLDER";
        static string Macro_StreamVR2_OR_NEWER = "VIU_STEAMVR_2_0_0_OR_NEWER";
        static string Macro_StreamVR2_OR_OLDER = "VIU_STEAMVR_1_2_3_OR_OLDER";

        static string Directory_VRTK4_OR_NEWER = "VRTK.Tilia.Package.Importer";
        static string Directory_VRTK4_OR_OLDER = "VRTK";
        static string ViveDirectory = "HTC.UnityPlugin";
        static string SteamVRDirectory = "SteamVR";
        static string AssetsPath;
        static string PluginsPath;
        static string LibrariesPath;

        /// <summary>
        /// 每打开一次窗口就会执行一次
        /// </summary>
        private void OnEnable()
        {
            Init();
            Rect position1 = position;
            position1.width = 500;
            position1.height = 500;
            position = position1;
            leftRect = new Rect(0, 0, this.position.width * 0.25f, this.position.height * 0.75f);
            rightRect = new Rect(this.position.width * 0.25f, 0, this.position.width * 0.75f, this.position.height * 0.8f);
            bottomtRect = new Rect(0, this.position.height * 0.805f, this.position.width, this.position.height * 0.195f);
            RightMenuSlider = new Vector2(rightRect.width, rightRect.height);
        }
        static void Init()
        {
            GetMacro();
            AssetsPath = Application.dataPath + "/";
            PluginsPath = Application.dataPath + "/Plugins/";
            LibrariesPath = Application.dataPath + "/Libraries/";

        }
        /// <summary>
        /// 绘制窗口条目.
        /// </summary>
        private void OnGUI()
        {
            //左边
            GUILayout.BeginArea(leftRect, new GUIStyle() { });
            GUILayout.Space(5);

            GUILayout.Label("当前宏定义", new GUIStyle(EditorStyles.wordWrappedLabel) { });

            GUILayout.Space(10);

            if (GUILayout.Button("HTC.UnityPlugin", GUILayout.Width(120), GUILayout.Height(30)))
            {
                AddCustomMacros(Macro_Vive);
            }
            GUILayout.Space(10);

            if (GUILayout.Button("SteamVR2.0以上", GUILayout.Width(120), GUILayout.Height(30)))
            {
                AddCustomMacros(Macro_StreamVR2_OR_NEWER);

            }
            EditorGUILayout.Space(10);
            if (GUILayout.Button("SteamVR2.0以下", GUILayout.Width(120), GUILayout.Height(30)))
            {
                AddCustomMacros(Macro_StreamVR2_OR_OLDER);

            }
            EditorGUILayout.Space(10);
            if (GUILayout.Button("VRTK4.0以上", GUILayout.Width(120), GUILayout.Height(30)))
            {
                AddCustomMacros(Macro_VRTK4_OR_NEWER);

            }
            EditorGUILayout.Space(10);
            if (GUILayout.Button("VRTK4.0以下", GUILayout.Width(120), GUILayout.Height(30)))
            {
                AddCustomMacros(Macro_VRTK4_OR_OLDER);

            }
            GUILayout.EndArea();

            //右边
            GUILayout.BeginArea(rightRect, new GUIStyle() { });

            RightMenuSlider = GUILayout.BeginScrollView(RightMenuSlider);

            UnityMacro = EditorGUILayout.TextArea(UnityMacro, new GUIStyle(EditorStyles.helpBox) { fontSize = 13 }, GUILayout.MaxWidth(rightRect.width), GUILayout.MaxHeight(RightMenuSlider.y), GUILayout.MaxHeight(rightRect.height));

            GUILayout.EndScrollView();

            GUILayout.EndArea();



            //底部

            GUILayout.BeginArea(bottomtRect, new GUIStyle() { });
            EditorGUILayout.Space();
            if (GUILayout.Button("保存并关闭", GUILayout.Width(495), GUILayout.Height(30)))
            {
                SetMacro();
                Close();
            }
            if (GUILayout.Button("关闭", GUILayout.Width(495), GUILayout.Height(30)))
            {
                Close();
            }
            GUILayout.EndArea();

            //GUI.skin.box
        }

        #region 添加自定义宏
        //脚本编译完事件
        //[UnityEditor.Callbacks.DidReloadScripts()]
        public static void UnityInitialzeOnLoadAddMacros()
        {
            GetMacro();

            if (IsDirectoryExists(ViveDirectory) || IsDirectoryExists(Directory_VRTK4_OR_OLDER) || IsDirectoryExists(Directory_VRTK4_OR_NEWER) || IsDirectoryExists(SteamVRDirectory))
            {
                if (!IsMacroExists(Macro_Vive) || !IsMacroExists(Macro_VRTK4_OR_NEWER) || !IsMacroExists(Macro_VRTK4_OR_OLDER) || !IsMacroExists(Macro_StreamVR2_OR_NEWER) || !IsMacroExists(Macro_StreamVR2_OR_OLDER))
                {
                    Debug.LogError("请根据VR插件添加宏定义: UnityFramework-->自定义宏设置");
                }
            }


        }
        static bool IsDirectoryExists(string name)
        {
            if (Directory.Exists(AssetsPath + name) || Directory.Exists(PluginsPath + name) || Directory.Exists(LibrariesPath + name))
            {
                return true;
            }
            return false;
        }
        static bool IsMacroExists(string macro)
        {
            string[] Macros = UnityMacro.Split(';');
            if (!((IList)Macros).Contains(macro))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 添加自定义宏
        /// </summary>
        static void AddCustomMacros(params string[] macro)
        {
            string[] Macros = UnityMacro.Split(';');
            foreach (var item in macro)
            {
                if (!((IList)Macros).Contains(item))
                {
                    UnityMacro += ";" + item;
                    Debug.Log($"{item} 宏定义添加成功");
                }
                else
                {
                    Debug.Log($"{item} 宏定义已存在");

                }
            }
            SetMacro();
        }
        static void GetMacro()
        {
            UnityMacro = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);//获取宏信息  参数:获取哪个平台下的
        }
        /// <summary>
        /// 保存宏信息
        /// </summary>
        static void SetMacro()
        {

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, UnityMacro);//将信息保存到宏信息里. 参数1:保存到哪个平台  参数2:要保存的内容.
        }
        #endregion
    }

    /// <summary>
    /// 自定义菜单类.
    /// </summary>
    public partial class UnityEditorTools
    {
        [MenuItem("UnityFramework/自定义宏设置")]
        public static void ScriptingDefineSymbolsSettingWindows()
        {
            ScriptingDefineSymbolsSettingWindows window = EditorWindow.GetWindow<ScriptingDefineSymbolsSettingWindows>();//获取指定类型的窗口.
            window.titleContent = new GUIContent("自定义宏设置窗口");
            window.maxSize = new Vector2(500, 500);
            window.minSize = new Vector2(500, 500);



            window.Show();
        }
    }


    /// <summary>
    /// 脚本宏定义。
    /// </summary>
    public static class ScriptingDefineSymbols
    {
        private static readonly BuildTargetGroup[] BuildTargetGroups = new BuildTargetGroup[]
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.iOS,
            BuildTargetGroup.Android,
            BuildTargetGroup.WSA,
            BuildTargetGroup.WebGL
        };

        /// <summary>
        /// 检查指定平台是否存在指定的脚本宏定义。
        /// </summary>
        /// <param name="buildTargetGroup">要检查脚本宏定义的平台。</param>
        /// <param name="scriptingDefineSymbol">要检查的脚本宏定义。</param>
        /// <returns>指定平台是否存在指定的脚本宏定义。</returns>
        public static bool HasScriptingDefineSymbol(BuildTargetGroup buildTargetGroup, string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return false;
            }

            string[] scriptingDefineSymbols = GetScriptingDefineSymbols(buildTargetGroup);
            foreach (string i in scriptingDefineSymbols)
            {
                if (i == scriptingDefineSymbol)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 为指定平台增加指定的脚本宏定义。
        /// </summary>
        /// <param name="buildTargetGroup">要增加脚本宏定义的平台。</param>
        /// <param name="scriptingDefineSymbol">要增加的脚本宏定义。</param>
        public static void AddScriptingDefineSymbol(BuildTargetGroup buildTargetGroup, string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            if (HasScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol))
            {
                return;
            }

            List<string> scriptingDefineSymbols = new List<string>(GetScriptingDefineSymbols(buildTargetGroup))
            {
                scriptingDefineSymbol
            };

            SetScriptingDefineSymbols(buildTargetGroup, scriptingDefineSymbols.ToArray());
        }

        /// <summary>
        /// 为指定平台移除指定的脚本宏定义。
        /// </summary>
        /// <param name="buildTargetGroup">要移除脚本宏定义的平台。</param>
        /// <param name="scriptingDefineSymbol">要移除的脚本宏定义。</param>
        public static void RemoveScriptingDefineSymbol(BuildTargetGroup buildTargetGroup, string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            if (!HasScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol))
            {
                return;
            }

            List<string> scriptingDefineSymbols = new List<string>(GetScriptingDefineSymbols(buildTargetGroup));
            while (scriptingDefineSymbols.Contains(scriptingDefineSymbol))
            {
                scriptingDefineSymbols.Remove(scriptingDefineSymbol);
            }

            SetScriptingDefineSymbols(buildTargetGroup, scriptingDefineSymbols.ToArray());
        }

        /// <summary>
        /// 为所有平台增加指定的脚本宏定义。
        /// </summary>
        /// <param name="scriptingDefineSymbol">要增加的脚本宏定义。</param>
        public static void AddScriptingDefineSymbol(string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            foreach (BuildTargetGroup buildTargetGroup in BuildTargetGroups)
            {
                AddScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol);
            }
        }

        /// <summary>
        /// 为所有平台移除指定的脚本宏定义。
        /// </summary>
        /// <param name="scriptingDefineSymbol">要移除的脚本宏定义。</param>
        public static void RemoveScriptingDefineSymbol(string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            foreach (BuildTargetGroup buildTargetGroup in BuildTargetGroups)
            {
                RemoveScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol);
            }
        }

        /// <summary>
        /// 获取指定平台的脚本宏定义。
        /// </summary>
        /// <param name="buildTargetGroup">要获取脚本宏定义的平台。</param>
        /// <returns>平台的脚本宏定义。</returns>
        public static string[] GetScriptingDefineSymbols(BuildTargetGroup buildTargetGroup)
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';');
        }

        /// <summary>
        /// 设置指定平台的脚本宏定义。
        /// </summary>
        /// <param name="buildTargetGroup">要设置脚本宏定义的平台。</param>
        /// <param name="scriptingDefineSymbols">要设置的脚本宏定义。</param>
        public static void SetScriptingDefineSymbols(BuildTargetGroup buildTargetGroup, string[] scriptingDefineSymbols)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", scriptingDefineSymbols));
        }
    }

}


