/*
* FileName:          CreateCustomScriptTemplate
* CompanyName:  
* Author:            relly
* Description:       创建自定义脚本带窗口
* 已放弃使用2021.7.26
*/
#if UNITY_EDITOR

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace UnityFramework.Editor
{
    public class CreateCustomScriptTemplate : EditorWindow
    {
        string scriptName = "NewBehaviourScript";
        private string CreateCustomScripts()
        {
            //资源放在Editor Default Resources
            // TextAsset templateSource = EditorGUIUtility.Load("80-C# CustomScript 不继承MonoBehaviour-CustomScript.cs.txt") as TextAsset;

            //if (templateSource == null)
            //{
            //    Debug.LogError("未找到自定义脚本模板");
            //    return;
            //}
            //string codeSrc = templateSource.text;
            //--------------------------------------

            string codeSrc = File.ReadAllText(GetPath() + "/80-C# CustomScript 不继承MonoBehaviour-CustomScript.cs.txt");
            if (string.IsNullOrEmpty(codeSrc))
            {
                Debug.Log("自定义脚本模板没有内容");
                return null;
            }
            codeSrc = codeSrc.Replace("#NOTRIM#", "");
            codeSrc = codeSrc.Replace("#SCRIPTNAME#", scriptName);
            codeSrc = codeSrc.Replace("#CompanyName#", "");
            codeSrc = codeSrc.Replace("#Author#", "relly");
            codeSrc = codeSrc.Replace("#Version#", "1.0");
            codeSrc = codeSrc.Replace("#UnityVersion#", Application.unityVersion);
            codeSrc = codeSrc.Replace("#CreateTime#", System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            codeSrc = codeSrc.Replace("#NameSpace#", "SC");

            var assetPath = string.Format("{0}/{1}{2}", GetSelectedPathOrFallback(), scriptName, ".cs");
            var filePath = Application.dataPath.Replace("Assets", assetPath);
            File.WriteAllText(filePath, codeSrc);

            AssetDatabase.ImportAsset(assetPath);
            return filePath;
        }

        //获取本脚本路径
        string GetPath()
        {
            string[] temp = this.ToString().Split('.');
            string name = temp.Last().Replace(")", "");
            string[] paths = AssetDatabase.FindAssets(name);
            if (paths.Length > 1)
            {
                Debug.LogError("有同名文件:" + this.name);
                return null;
            }
            string m_path = AssetDatabase.GUIDToAssetPath(paths[0]);
            m_path = Path.GetDirectoryName(m_path);
            return m_path;
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

        void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("当前脚本名称", EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(100));

            scriptName = EditorGUILayout.TextArea(scriptName, EditorStyles.textArea, GUILayout.MaxWidth(260), GUILayout.MinHeight(50));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("创建", GUILayout.Width(295), GUILayout.Height(20)))
            {
                CreateCustomScripts();
                Close();
            }
            if (GUILayout.Button("关闭", GUILayout.Width(295), GUILayout.Height(20)))
            {
                Close();
            }
        }
    }
    public partial class UnityEditorTools
    {
        //	[MenuItem("Assets/CustomScript 不继承MonoBehaviour", false, 1)]
        private static void CreateCustomScriptTemplateWindow()
        {
            CreateCustomScriptTemplate CreateCustomScriptTemplate = EditorWindow.GetWindow<CreateCustomScriptTemplate>();
            CreateCustomScriptTemplate.titleContent = new GUIContent("自定义脚本");
            CreateCustomScriptTemplate.maxSize = new Vector2(300, 110);
            CreateCustomScriptTemplate.minSize = new Vector2(300, 110);
            CreateCustomScriptTemplate.Show();
        }
    }
}



#endif