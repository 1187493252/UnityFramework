/*
* FileName:          CreateCustomScript
* CompanyName:  
* Author:            relly
* Description:       创建自定义脚本带窗口
* 
*/

using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{
    public class CreateCustomScripts : EditorWindow
    {
        //作者
        string author = "";
        //公司
        string companyName = "";
        //脚本名
        string scriptName = "";
        //父类
        string baseForName = "";
        //描述
        string description = "";
        //命名空间
        string nameSpaceName = "";
        //是否继承MonoBehaviour
        bool isBaseForMonoBehaviour;

        bool isPartial;
        ScriptType scriptType;

        string message = "";
        public enum ScriptType
        {
            Class,
            Enum,
            Struct,
            Interface,
            Null
        }

        void CreateScript()
        {
            if (scriptName == "")
            {
                message = "脚本名不能为空!!!";
                return;
            }
            StringBuilder sb = new StringBuilder();
            //添加脚本描述
            sb.AppendLine("/*");
            sb.AppendLine($"* FileName:          {scriptName}");
            sb.AppendLine($"* CompanyName:       {companyName}");
            sb.AppendLine($"* Author:            {author}");
            //  sb.AppendLine($"* CreateTime:        {DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss")}");
            //    sb.AppendLine($"* UnityVersion:      {Application.unityVersion}");
            sb.AppendLine($"* Description:       {description}");
            sb.AppendLine("*/");
            sb.AppendLine();
            //添加引用
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Collections;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using UnityEngine.UI;");
            sb.AppendLine();
            //添加命名空间
            if (!string.IsNullOrEmpty(nameSpaceName))
            {
                sb.AppendLine($"namespace {nameSpaceName}");
                sb.AppendLine("{");

                switch (scriptType)
                {
                    case ScriptType.Class:
                        sb.Append("    public ");
                        if (isPartial)
                        {
                            sb.Append("partial ");
                        }
                        sb.Append($"class {scriptName}");

                        if (isBaseForMonoBehaviour)
                        {
                            sb.Append($" : MonoBehaviour");
                            sb.AppendLine();
                            sb.AppendLine("    {");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(baseForName))
                            {
                                sb.Append($" : {baseForName}");
                                sb.AppendLine();
                                sb.AppendLine("    {");
                            }
                            else
                            {
                                sb.AppendLine();

                                sb.AppendLine("    {");
                            }
                        }
                        sb.AppendLine();
                        sb.AppendLine("    }");
                        break;
                    case ScriptType.Enum:
                        sb.AppendLine("    public enum " + scriptName);
                        sb.AppendLine("    {");
                        sb.AppendLine();
                        sb.AppendLine("    }");

                        break;
                    case ScriptType.Struct:
                        sb.AppendLine("    public struct " + scriptName);
                        sb.AppendLine("    {");
                        sb.AppendLine();
                        sb.AppendLine("    }");
                        break;
                    case ScriptType.Interface:
                        sb.AppendLine("    public interface " + scriptName);
                        sb.AppendLine("    {");
                        sb.AppendLine();
                        sb.AppendLine("    }");
                        break;
                    case ScriptType.Null:
                        sb.AppendLine();
                        break;

                }
                sb.AppendLine("}");
            }
            else
            {
                switch (scriptType)
                {
                    case ScriptType.Class:
                        sb.Append("public ");
                        if (isPartial)
                        {
                            sb.Append("partial ");
                        }
                        sb.Append($"class {scriptName}");

                        if (isBaseForMonoBehaviour)
                        {
                            sb.Append($" : MonoBehaviour");
                            sb.AppendLine();
                            sb.AppendLine("{");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(baseForName))
                            {
                                sb.Append($" : {baseForName}");
                                sb.AppendLine();
                                sb.AppendLine("{");
                            }
                            else
                            {
                                sb.AppendLine();

                                sb.AppendLine("{");
                            }
                        }
                        sb.AppendLine();
                        sb.AppendLine("}");
                        break;
                    case ScriptType.Enum:
                        sb.AppendLine("public enum " + scriptName);
                        sb.AppendLine("{");
                        sb.AppendLine();
                        sb.AppendLine("}");
                        break;
                    case ScriptType.Struct:
                        sb.AppendLine("public struct " + scriptName);
                        sb.AppendLine("{");
                        sb.AppendLine();
                        sb.AppendLine("}");
                        break;
                    case ScriptType.Interface:
                        sb.AppendLine("public interface " + scriptName);
                        sb.AppendLine("{");
                        sb.AppendLine();
                        sb.AppendLine("}");
                        break;
                    case ScriptType.Null:
                        break;
                }
            }

            var assetPath = $"{GetSelectedPathOrFallback()}/{scriptName}.cs";
            var filePath = Application.dataPath.Replace("Assets", assetPath);
            File.WriteAllText(assetPath, sb.ToString(), Encoding.UTF8);

            //AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.Refresh();

            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));
            //在Project面板标记高亮显示
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;

            Close();

        }
        private string CreateCustomScript()
        {
            if (scriptName == "")
            {
                message = "脚本名不能为空!!!";
                return null;
            }
            string codeSrc;
            string path = GetPath();
            if (string.IsNullOrEmpty(path))
            {
                message = "缺少脚本模板!!!";
                return null;
            }
            if (string.IsNullOrEmpty(nameSpaceName))
            {
                codeSrc = File.ReadAllText(path + "/83-C# Script-CustomScriptTemplate-No NameSpace.cs.txt", Encoding.UTF8);
            }
            else
            {
                codeSrc = File.ReadAllText(path + "/82-C# Script-CustomScriptTemplate.cs.txt", Encoding.UTF8);
                codeSrc = codeSrc.Replace("#NameSpace#", nameSpaceName);

            }
            if (string.IsNullOrEmpty(codeSrc))
            {
                message = "自定义脚本模板没有内容!!!";
                return null;
            }
            switch (scriptType)
            {
                case ScriptType.Class:
                    string temp = "class";

                    if (isBaseForMonoBehaviour)
                    {
                        codeSrc = codeSrc.Replace("#BASE#", ": MonoBehaviour");
                    }
                    else
                    {

                        if (string.IsNullOrEmpty(baseForName))
                        {
                            codeSrc = codeSrc.Replace("#BASE#", baseForName);
                        }
                        else
                        {
                            codeSrc = codeSrc.Replace("#BASE#", ":" + baseForName);
                        }
                    }

                    if (isPartial)
                    {
                        temp = "partial class";
                    }
                    codeSrc = codeSrc.Replace("#ScriptType#", temp);

                    break;
                case ScriptType.Enum:
                    codeSrc = codeSrc.Replace("#ScriptType#", "enum");
                    codeSrc = codeSrc.Replace("#BASE#", "");

                    break;
                case ScriptType.Struct:
                    codeSrc = codeSrc.Replace("#ScriptType#", "struct");
                    codeSrc = codeSrc.Replace("#BASE#", "");

                    break;
                case ScriptType.Interface:
                    codeSrc = codeSrc.Replace("#ScriptType#", "interface");
                    codeSrc = codeSrc.Replace("#BASE#", "");
                    break;
            }


            codeSrc = codeSrc.Replace("#SCRIPTNAME#", scriptName);
            codeSrc = codeSrc.Replace("#CompanyName#", companyName);
            codeSrc = codeSrc.Replace("#Author#", author);
            codeSrc = codeSrc.Replace("#Description#", description);
            codeSrc = codeSrc.Replace("#Version#", "1.0");
            codeSrc = codeSrc.Replace("#UnityVersion#", Application.unityVersion);
            codeSrc = codeSrc.Replace("#CreateTime#", System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));

            var assetPath = string.Format("{0}/{1}{2}", GetSelectedPathOrFallback(), scriptName, ".cs");
            var filePath = Application.dataPath.Replace("Assets", assetPath);
            File.WriteAllText(filePath, codeSrc, Encoding.UTF8);


            AssetDatabase.ImportAsset(assetPath);
            Close();
            return filePath;
        }

        //添加函数 无命名空间
        private string AddFunctionNoNameSpace(string _function)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format("    private void {0}()", _function));
            builder.AppendLine("    {");
            builder.AppendLine();
            builder.AppendLine("    }");
            builder.AppendLine();
            return builder.ToString();
        }

        //添加函数 有命名空间
        private string AddFunctionWithNameSpace(string _function)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format("        private void {0}()", _function));
            builder.AppendLine("        {");
            builder.AppendLine();
            builder.AppendLine("        }");
            builder.AppendLine();
            return builder.ToString();
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
                message = "有同名文件:" + name;
                return null;
            }
            string m_path = AssetDatabase.GUIDToAssetPath(paths[0]);
            m_path = Path.GetDirectoryName(m_path);
            return m_path;
        }
        //获取当前选择路径
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
            GUILayout.Label("类型", EditorStyles.wordWrappedLabel, GUILayout.MinWidth(100));
            scriptType = (ScriptType)EditorGUILayout.EnumPopup(scriptType, GUILayout.MaxWidth(260));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            isPartial = EditorGUILayout.Toggle("是否Partial", isPartial);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            isBaseForMonoBehaviour = EditorGUILayout.Toggle("是否继承MonoBehaviour", isBaseForMonoBehaviour);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("公司", EditorStyles.wordWrappedLabel, GUILayout.MinWidth(100));
            companyName = EditorGUILayout.TextArea(companyName, EditorStyles.textArea, GUILayout.MaxWidth(260));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("作者", EditorStyles.wordWrappedLabel, GUILayout.MinWidth(100));
            author = EditorGUILayout.TextArea(author, EditorStyles.textArea, GUILayout.MaxWidth(260));
            GUILayout.EndHorizontal();


            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("脚本名称", EditorStyles.wordWrappedLabel, GUILayout.MinWidth(100));
            scriptName = EditorGUILayout.TextArea(scriptName, EditorStyles.textArea, GUILayout.MaxWidth(260));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("父类,可为空", EditorStyles.wordWrappedLabel, GUILayout.MinWidth(100));
            baseForName = EditorGUILayout.TextArea(baseForName, EditorStyles.textArea, GUILayout.MaxWidth(260));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("命名空间,可为空", EditorStyles.wordWrappedLabel, GUILayout.MinWidth(100));
            nameSpaceName = EditorGUILayout.TextArea(nameSpaceName, EditorStyles.textArea, GUILayout.MaxWidth(260));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("脚本描述", EditorStyles.wordWrappedLabel, GUILayout.MinWidth(100));
            description = EditorGUILayout.TextArea(description, EditorStyles.textArea, GUILayout.MaxWidth(260), GUILayout.MinHeight(50));
            GUILayout.EndHorizontal();

            //EditorGUILayout.Space();
            GUILayout.Label(message, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(260));

            //EditorGUILayout.Space();
            if (GUILayout.Button("创建", GUILayout.Width(295), GUILayout.Height(25)))
            {
                //CreateCustomScript();
                CreateScript();
            }
            GUILayout.Space(5);

            if (GUILayout.Button("关闭", GUILayout.Width(295), GUILayout.Height(25)))
            {
                Close();
            }
        }
        void OnInspectorUpdate()  //该方法每秒调用 10 帧
        {
            this.Repaint();
        }

    }
    public partial class UnityEditorTools
    {
        [MenuItem("Assets/创建自定义脚本", false, 1)]
        private static void CreateCustomScriptWindow()
        {
            CreateCustomScripts CreateCustomScripts = EditorWindow.GetWindow<CreateCustomScripts>();
            CreateCustomScripts.titleContent = new GUIContent("创建自定义脚本");
            CreateCustomScripts.maxSize = new Vector2(300, 400);
            CreateCustomScripts.minSize = new Vector2(300, 400);
            CreateCustomScripts.Show();
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
    }
}



