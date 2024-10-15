/*
* FileName:          FontWindow
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UnityFramework.Editor
{
    /// <summary>
    /// 字体工具 一键更换场景内所有字体
    /// </summary>
    public class FontWindow : EditorWindow
    {


        Font toChange;
        static Font toChangeFont;
        FontStyle toFontStyle;
        static FontStyle toChangeFontStyle;
        static string prefabPath = "Assets";
        string fontSize;
        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("字体");
            toChange = (Font)EditorGUILayout.ObjectField(toChange, typeof(Font), true, GUILayout.MinWidth(150f));
            toChangeFont = toChange;
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("样式");
            toFontStyle = (FontStyle)EditorGUILayout.EnumPopup(toFontStyle, GUILayout.MinWidth(150f));
            toChangeFontStyle = toFontStyle;
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("字号");
            fontSize = GUILayout.TextField(fontSize, GUILayout.MinWidth(200f));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (GUILayout.Button("替换Hierarchy所有Text的字体"))
            {
                ChangeHierarchyTextFont();
            }
            GUILayout.Space(5);
            //GUILayout.Label("Prefab文件夹:");
            //GUILayout.Space(5);

            //prefabPath = GUILayout.TextField(prefabPath);
            //if (GUILayout.Button("选择Prefab文件夹"))
            //{
            //    SelectPrefabDir();
            //}
            //GUILayout.Space(5);
            //if (GUILayout.Button("替换指定文件夹Prefab所有Text的字体"))
            //{
            //    ChangePrefabTextFont();
            //}
            //GUILayout.Space(5);

            if (GUILayout.Button("关闭"))
            {
                Close();
            }
            EditorGUILayout.EndVertical();

        }
        private void SelectPrefabDir()
        {
            string path = EditorUtility.OpenFolderPanel("选择Prefab文件夹", Application.dataPath, "");
            if (path.Length != 0)
            {
                path = "Assets" + path.Split(new string[] { "Assets" }, StringSplitOptions.None)[1];
                prefabPath = path;
            }
        }

        public void ChangeHierarchyTextFont()
        {

            //寻找Hierarchy面板下所有的Text
            var tArray = Resources.FindObjectsOfTypeAll(typeof(Text));
            for (int i = 0; i < tArray.Length; i++)
            {
                Text t = tArray[i] as Text;
                Undo.RecordObject(t, t.gameObject.name);
                if (toChangeFont != null)
                {
                    t.font = toChangeFont;
                }
                t.fontStyle = toChangeFontStyle;
                if (!string.IsNullOrEmpty(fontSize))
                {
                    t.fontSize = int.Parse(fontSize);
                }
                EditorUtility.SetDirty(t);
            }

        }

        public void ChangePrefabTextFont()
        {
            if (prefabPath == "")
            {
                prefabPath = "Assets";
            }
            //获取Asset文件夹下所有Prefab的GUID
            string[] GUID = AssetDatabase.FindAssets("t:Prefab", new string[] { prefabPath });
            for (int i = 0; i < GUID.Length; i++)
            {
                string tmpPath;
                GameObject tmpObj;
                //根据GUID获取路径
                tmpPath = AssetDatabase.GUIDToAssetPath(GUID[i]);
                if (!string.IsNullOrEmpty(tmpPath))
                {
                    //根据路径获取Prefab(GameObject)
                    tmpObj = AssetDatabase.LoadAssetAtPath(tmpPath, typeof(GameObject)) as GameObject;
                    if (tmpObj != null)
                    {
                        //获取Prefab及其子物体孙物体.......的所有Text组件
                        Text[] tmpArr = tmpObj.GetComponentsInChildren<Text>(true);
                        if (tmpArr != null && tmpArr.Length > 0)
                            foreach (var item in tmpArr)
                            {
                                if (toChangeFont != null)
                                {
                                    item.font = toChangeFont;
                                }
                                item.fontStyle = toChangeFontStyle;
                                if (!string.IsNullOrEmpty(fontSize))
                                {
                                    item.fontSize = int.Parse(fontSize);
                                }
                            }

                        EditorUtility.SetDirty(tmpObj);

                    }
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();


        }
        public partial class UnityEditorTools
        {
            [MenuItem("UnityFramework/替换字体")]
            public static void CreateFontWindows()
            {
                FontWindow window = EditorWindow.GetWindow<FontWindow>();//获取指定类型的窗口.
                window.titleContent = new GUIContent("替换字体");
                window.minSize = new Vector2(300, 230);
                window.Show();
            }
        }
    }
}
