/*
* FileName:          SetWebglMemorySize
* CompanyName:       
* Author:            
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UnityFramework.Editor
{
    public class WebglMemorySize : EditorWindow
    {
        string size = "";
        private void OnGUI()
        {
            GUILayout.Space(10);


            GUILayout.Label($"当前大小:{PlayerSettings.WebGL.memorySize / 1024 / 1024}", EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(300));
            GUILayout.Label("设置大小(单位M)", EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(300));
            Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
            size = regex.Match(EditorGUILayout.TextArea(size, EditorStyles.textArea, GUILayout.MaxWidth(295), GUILayout.MinHeight(20))).Value;


            if (GUILayout.Button("设置", GUILayout.Width(295), GUILayout.Height(20)))
            {
                PlayerSettings.WebGL.memorySize = int.Parse(size) * 1024 * 1024;
            }


            EditorGUILayout.Space();
            if (GUILayout.Button("关闭", GUILayout.Width(295), GUILayout.Height(20)))
            {
                Close();
            }

        }



    }
    public partial class UnityEditorTools
    {
        [MenuItem("UnityFramework/设置打包Webgl内存大小")]
        static void CreateWebglMemorySizeWindows()
        {
            WebglMemorySize window = EditorWindow.GetWindow<WebglMemorySize>();//获取指定类型的窗口.
            window.titleContent = new GUIContent("设置打包Webgl内存大小");
            window.maxSize = new Vector2(300, 300);
            window.minSize = new Vector2(300, 300);
            window.Show();
        }
    }
}