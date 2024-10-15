/*
* FileName:          ProtoToCS
* CompanyName:  
* Author:            relly
* Description:       
* 
*/



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{

    public class ProtoToCS : EditorWindow
    {
        /// <summary>
        /// CS保存目录
        /// </summary>
        internal static string csSaveDir;
        /// <summary>
        /// proto文件路径
        /// </summary>
        internal static string protoPath;
        /// <summary>
        /// proto文件目录
        /// </summary>
        internal static string protoDir;

        internal static string protoEXE;


        private void OnGUI()
        {


            GUILayout.Label("说明:Proto示例在Framework/ExternalLibrary/Protobuf ");
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical();
            GUILayout.Label("protoc.exe路径: ");
            GUILayout.Space(5);
            protoEXE = GUILayout.TextField(protoEXE);
            GUILayout.Space(5);

            if (GUILayout.Button("选择protoc.exe路径"))
            {
                SelectProtoEXEPath();
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Proto文件路径: ");
            GUILayout.Space(5);

            protoPath = GUILayout.TextField(protoPath);
            GUILayout.Space(5);

            if (GUILayout.Button("选择Proto文件"))
            {
                SelectProtoFile();
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(5);

            EditorGUILayout.BeginVertical();
            GUILayout.Label("Proto文件目录: ");
            GUILayout.Space(5);
            protoDir = GUILayout.TextField(protoDir);
            GUILayout.Space(5);

            if (GUILayout.Button("选择Proto文件目录"))
            {
                SelectProtoDir();
            }
            EditorGUILayout.EndVertical();





            GUILayout.Space(5);
            EditorGUILayout.BeginVertical();
            GUILayout.Label("CS保存路径: ");
            GUILayout.Space(5);
            csSaveDir = GUILayout.TextField(csSaveDir);
            GUILayout.Space(5);

            if (GUILayout.Button("选择CS保存目录"))
            {
                SelectCSSaveDirectory();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(5);



            if (GUILayout.Button("生成CS"))
            {
                CreateCS(protoPath);
            }
            GUILayout.Space(5);

            if (GUILayout.Button("批量生成CS"))
            {
                BatchCreateCS(protoDir);
            }
            GUILayout.Space(5);



            if (GUILayout.Button("默认路径"))
            {
                csSaveDir = string.Concat(Application.dataPath, "/Framework/ExternalLibrary/Protobuf/ProtobufCS");
                protoPath = string.Concat(Application.dataPath, "/Framework/ExternalLibrary/Protobuf/Proto/ProtobufTest.proto");
                protoDir = string.Concat(Application.dataPath, "/Framework/ExternalLibrary/Protobuf/Proto");
                protoEXE = string.Concat(Application.dataPath, "/Framework/ExternalLibrary/Protobuf/protoc.exe");

            }
            GUILayout.Space(5);

            if (GUILayout.Button("关闭"))
            {
                Close();
            }
            GUILayout.Space(5);


        }
        private void BatchCreateCS(string _directory)
        {
            if (Directory.Exists(_directory))
            {
                DirectoryInfo folder = new DirectoryInfo(_directory);
                FileInfo[] files = folder.GetFiles("*.proto");
                List<string> cmds = new List<string>();
                foreach (var file in files)
                {
                    string cmd = protoEXE + " --csharp_out=" + csSaveDir + " -I " + _directory + " " + file.FullName;
                    cmds.Add(cmd);
                }
                Cmd(cmds);
                AssetDatabase.Refresh();

            }
        }
        void CreateCS(string _directory)
        {
            FileInfo file = new FileInfo(_directory);
            List<string> cmds = new List<string>();

            string cmd = protoEXE + " --csharp_out=" + csSaveDir + " -I " + file.DirectoryName + " " + file.FullName;
            cmds.Add(cmd);

            Cmd(cmds);
            AssetDatabase.Refresh();
        }

        static public void Cmd(List<string> cmds)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = ".";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardError = true;
            process.ErrorDataReceived += ErrorDataHandler;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            for (int i = 0; i < cmds.Count; i++)
            {
                process.StandardInput.WriteLine(cmds[i]);
            }
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();

        }


        static void ErrorDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                UnityEngine.Debug.LogError(outLine.Data);
            }
        }

        /// <summary>
        /// 选择proto文件
        /// </summary>
        private void SelectProtoFile()
        {
            string path = EditorUtility.OpenFilePanelWithFilters("选择Proto文件", protoPath, new string[] { "*", "proto" });
            if (path.Length != 0)
            {
                protoPath = path;

            }
        }


        /// <summary>
        /// 选择proto.exe路径
        /// </summary>
        private void SelectProtoEXEPath()
        {
            string path = EditorUtility.OpenFilePanelWithFilters("选择proto.exe路径", Application.dataPath, new string[] { "*", "exe" });
            if (path.Length != 0)
            {
                protoEXE = path;
            }
        }

        /// <summary>
        /// 选择proto文件夹
        /// </summary>
        private void SelectProtoDir()
        {
            string path = EditorUtility.OpenFolderPanel("选择Proto文件夹", Application.dataPath, "");
            if (path.Length != 0)
            {
                protoDir = path;

            }
        }

        /// <summary>
        /// cs保存目录
        /// </summary>
        private void SelectCSSaveDirectory()
        {
            string path = EditorUtility.OpenFolderPanel("选择CS保存目录", Application.dataPath, "");
            if (path.Length != 0)
            {
                csSaveDir = path;
            }
        }



        [MenuItem("Assets/Proto转CS", false, 0)]
        static void BatchCreateCSBySelection()
        {
            Dictionary<string, string> PathDic = new Dictionary<string, string>();

            string protoEXE = string.Concat(Application.dataPath, "/Framework/ExternalLibrary/Protobuf/protoc.exe");
            if (!File.Exists(protoEXE))
            {
                UnityEngine.Debug.LogError($"文件不存在:{protoEXE}");
                return;
            }
            List<string> cmds = new List<string>();
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                string path = AssetDatabase.GetAssetPath(obj);

                if (Path.GetExtension(path) == ".proto")
                {
                    FileInfo file = new FileInfo(path);
                    string cmd = protoEXE + " --csharp_out=" + file.DirectoryName + " -I " + file.DirectoryName + " " + file.FullName;
                    cmds.Add(cmd);
                }
            }
            Cmd(cmds);
            AssetDatabase.Refresh();
        }
    }

    //---------------------------------------

    public partial class UnityEditorTools
    {
        [MenuItem("UnityFramework/Proto转CS")]
        public static void CreateProtoToCSWindows()
        {
            ProtoToCS window = EditorWindow.GetWindow<ProtoToCS>();//获取指定类型的窗口.
            window.titleContent = new GUIContent("Proto转CS");
            window.maxSize = new Vector2(500, 600);
            window.minSize = new Vector2(500, 600);
            window.Show();

            ClearConsoleLog();

        }
        public static void ClearConsoleLog()
        {
            System.Type log = typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries");
            System.Reflection.MethodInfo clearMethod = log.GetMethod("Clear");
            clearMethod.Invoke(null, null);
        }
    }

}