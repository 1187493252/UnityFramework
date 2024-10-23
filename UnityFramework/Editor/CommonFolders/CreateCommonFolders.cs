/*
* FileName:          CreateCommonFolders
* CompanyName:  
* Author:            
* Description:       在根目录下创建常见文件夹
* 
*/
#if UNITY_EDITOR

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{
    public class CreateCommonFolder : EditorWindow
    {
        string textArea = "Editor/Prefabs/Scripts/Plugins/Animations/UI/Resources/Scenes/StreamingAssets/Models/Materials";
        string path;
        private void OnEnable()
        {
            path = GetPath() + "/CommonFolders.txt";
            textArea = File.ReadAllText(path);

        }
        private void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();

            GUILayout.Label("打开为默认配置,自定义请先配置完保存到本地,方便下次直接载入,多个以'/'隔开", EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(140), GUILayout.Height(100));
            CreateCommonFolder sd = GetWindow<CreateCommonFolder>();

            textArea = EditorGUILayout.TextArea(textArea, EditorStyles.textArea, GUILayout.MaxWidth(360), GUILayout.MaxHeight(360));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("载入本地配置", GUILayout.Width(490), GUILayout.Height(30)))
            {
                textArea = File.ReadAllText(path);
            }

            if (GUILayout.Button("保存到本地配置", GUILayout.Width(490), GUILayout.Height(30)))
            {
                File.WriteAllText(path, textArea);
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("创建", GUILayout.Width(490), GUILayout.Height(30)))
            {
                CreateFolders(textArea);
                AssetDatabase.Refresh();
                Close();
            }
            if (GUILayout.Button("关闭", GUILayout.Width(490), GUILayout.Height(30)))
            {
                Close();
            }
        }
        void CreateFolders(string textArea)
        {
            string dirpath = Application.dataPath + "/";
            string[] content = textArea.Split('/');
            foreach (var item in content)
            {
                if (!Directory.Exists(dirpath + item))
                {
                    Directory.CreateDirectory(dirpath + item);
                }
            }
        }
        //获取本脚本路径
        string GetPath()
        {

            string[] temp = this.ToString().Split('.');
            string name = temp.Last().Replace(")", "");
            //AssetDatabase.FindAssets查找资源,Name为CreateCustomScript,只要名字包含CreateCustomScript就会被找到,比如CreateCustomScripts,CreateCustomScriptTemplate
            string[] paths = AssetDatabase.FindAssets(name);
            if (paths.Length > 1)
            {
                //按照上述规则可能存在"同名"文件
                Debug.LogError("有同名文件:" + this.name);

                return null;
            }
            string m_path = AssetDatabase.GUIDToAssetPath(paths[0]);
            m_path = Path.GetDirectoryName(m_path);
            return m_path;
        }
    }
    public partial class UnityEditorTools
    {
        [MenuItem("UnityFramework/创建常见文件夹")]
        public static void CreateCommonFoldersSettingWindows()
        {
            CreateCommonFolder window = EditorWindow.GetWindow<CreateCommonFolder>();//获取指定类型的窗口.
            window.titleContent = new GUIContent("创建常见文件夹");
            window.maxSize = new Vector2(500, 500);
            window.minSize = new Vector2(500, 500);
            window.Show();
        }
    }
}

#endif