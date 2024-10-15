/*
* FileName:          WebGLTemplates
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{

    public class WebGLTemplates : EditorWindow
    {
        string templatesPath = "/Framework/WebGLTemplates";
        string productName = "";

        string titles = "测试项目";
        private void OnEnable()
        {
            productName = Application.productName;

        }
        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("网页Title");
            titles = GUILayout.TextField(titles, GUILayout.MinWidth(200f), GUILayout.MinHeight(50f));

            GUILayout.Space(5);


            GUILayout.Label("模板内相对路径文件夹名称");

            productName = GUILayout.TextField(productName, GUILayout.MinWidth(200f), GUILayout.MinHeight(50f));
            GUILayout.Label("注意:打包的文件夹名称需要与此名称一致");
            GUILayout.Space(5);

            if (GUILayout.Button("确定"))
            {
                Create();
            }
            GUILayout.Space(5);


            if (GUILayout.Button("关闭"))
            {
                Close();
            }
            EditorGUILayout.EndVertical();

        }


        public void Create()
        {
            string tmpPath = Application.dataPath + templatesPath;
            string copyToPath = Application.dataPath + "/WebGLTemplates";
            CopyDir(tmpPath, copyToPath);
            string path = "";
#if UNITY_2020_1_OR_NEWER
            path = copyToPath + "/LOAD_2020_OR_NEWER/index.html";
            string codeSrc = File.ReadAllText(path);
            codeSrc = codeSrc.Replace("#Title#", titles);
            codeSrc = codeSrc.Replace("#ProductName#", $"\"{productName}\"");
#else
            path = copyToPath + "/LOAD_2019_OR_LATER/index.html";
            string codeSrc = File.ReadAllText(path);
              codeSrc = codeSrc.Replace("#Title#", title);
              codeSrc = codeSrc.Replace("#Title1#", title);
              codeSrc = codeSrc.Replace("#ProductName#", $"\"{productName}\"");

#endif
            Debug.Log("模版生成成功");

            File.WriteAllText(path, codeSrc);

            AssetDatabase.Refresh();
            Close();

        }

        public static void CopyDir(string SourcePath, string CopyToPathPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加
                if (CopyToPathPath[CopyToPathPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                {
                    CopyToPathPath += System.IO.Path.DirectorySeparatorChar;
                }
                // 判断目标目录是否存在如果不存在则新建
                if (!System.IO.Directory.Exists(CopyToPathPath))
                {
                    System.IO.Directory.CreateDirectory(CopyToPathPath);
                }
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(SourcePath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (System.IO.Directory.Exists(file))
                    {
                        CopyDir(file, CopyToPathPath + System.IO.Path.GetFileName(file));
                    }
                    // 否则直接Copy文件
                    else
                    {
                        if (file.EndsWith(".meta") || file.EndsWith(".bak"))
                        {
                            continue;
                        }
                        System.IO.File.Copy(file, CopyToPathPath + System.IO.Path.GetFileName(file), true);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }


        public partial class UnityEditorTools
        {
            [MenuItem("UnityFramework/生成Web项目模板")]
            public static void CreateWebGLTemplatesWindows()
            {
                WebGLTemplates window = EditorWindow.GetWindow<WebGLTemplates>();//获取指定类型的窗口.
                window.titleContent = new GUIContent("生成Web项目模板");
                window.minSize = new Vector2(300, 300);
                window.Show();
            }
        }
    }
}
