/*
* FileName:          CreateFolders
* CompanyName:  
* Author:            
* Description:       在当前选择目录下创建文件夹
* 
*/
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{
    public class CreateFolders : EditorWindow
    {
        string Name;
        [TextArea]
        string tips = @"可创建单个或多个文件夹,支持一,二,三级嵌套
支持一级目录,以'/'隔开
支持二级目录,以'-'隔开
支持三级目录,以'+'隔开
例: 支持复制粘贴
1-1.1+1.11+1.12-1.2+1.21+1.22/2-2.1+2.11+2.12-2.2+2.21+2.22";




        private void OnGUI()
        {
            GUILayout.Space(10);
            //	GUILayout.BeginHorizontal();
            GUILayout.TextField(tips, EditorStyles.wordWrappedLabel);

            EditorGUILayout.Space();

            Name = EditorGUILayout.TextArea(Name, EditorStyles.textArea, GUILayout.MaxWidth(395), GUILayout.MinHeight(50));
            //	GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("创建", GUILayout.Width(395), GUILayout.Height(20)))
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return;
                }
                CreateDirectories(Name, GetSelectedPathOrFallback());
                Close();
            }
            if (GUILayout.Button("关闭", GUILayout.Width(395), GUILayout.Height(20)))
            {
                Close();
            }
        }
        void CreateDirectories(string textArea, string path)
        {
            string[] directoryOne = SplitString(textArea, '/');
            DirectoryInfo directoryInfo = null;
            List<string> dirpath = new List<string>();
            dirpath.Clear();
            foreach (var item in directoryOne)
            {
                string[] directoryTwo = SplitString(item, '-');

                for (int i = 0; i < directoryTwo.Length; i++)
                {
                    if (i == 0)
                    {
                        //控制一级目录创建
                        dirpath.Add(path + "/" + directoryTwo[i]);
                    }
                    else
                    {
                        string[] directoryThree = SplitString(directoryTwo[i], '+');

                        for (int j = 0; j < directoryThree.Length; j++)
                        {
                            if (j == 0)
                            {
                                //控制二级目录创建
                                dirpath.Add(path + "/" + directoryTwo[0] + "/" + directoryThree[j]);

                            }
                            else
                            {
                                dirpath.Add(path + "/" + directoryTwo[0] + "/" + directoryThree[0] + "/" + directoryThree[j]);
                            }
                        }
                    }
                }

            }

            dirpath.ForEach(item =>
            {
                directoryInfo = Directory.CreateDirectory(item);
                Debug.Log($"Assets{directoryInfo.FullName.Split(new string[] { "Assets" }, StringSplitOptions.None)[1]} 创建成功");
            });

            AssetDatabase.Refresh();

            string path1 = $"Assets{directoryInfo.FullName.Split(new string[] { "Assets" }, StringSplitOptions.None)[1]}";
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path1.Replace("\\\\", "/"), typeof(UnityEngine.Object));
            //在Project面板标记高亮显示
            EditorGUIUtility.PingObject(obj);
            //标记选中
            Selection.activeObject = obj;

        }


        string[] SplitString(string content, char split)
        {
            string[] contents;
            content = content.Trim();
            contents = content.Split(split);
            return contents;
        }
        public static string GetSelectedPathOrFallback()
        {
            string path = "Assets";

            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    //如果是目录获得目录名，如果是文件获得文件所在的目录名
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }

            return path;
        }
        void OnInspectorUpdate()  //该方法每秒调用 10 帧
        {
            this.Repaint();
        }
    }
    public partial class UnityEditorTools
    {
        [MenuItem("Assets/创建文件夹", false, 3)]
        public static void CreateFoldersSettingWindows()
        {
            CreateFolders window = EditorWindow.GetWindow<CreateFolders>();//获取指定类型的窗口.
            window.titleContent = new GUIContent("创建文件夹");
            window.maxSize = new Vector2(400, 220);
            window.minSize = new Vector2(400, 220);
            window.Show();
        }
    }
}

#endif