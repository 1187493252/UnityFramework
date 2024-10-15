/*
* FileName:          
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

using LitJson;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UnityFramework.Editor
{

    public class TimeLock : EditorWindow
    {

        string fileName = "UsageSettings";
        DateTime dateTimeStart;
        DateTime dateTimeEnd;
        string start;
        string end;
        string SecurityCode;
        private void OnEnable()
        {
            dateTimeStart = DateTime.Now;
            start = dateTimeStart.ToString("yyyy/MM/dd HH:mm");
            SecurityCode = UtilityTools.GetSecurityCode();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label("日期格式:yyyy/MM/dd HH:mm");


            GUILayout.Space(5);

            GUILayout.Label("随机16位密钥:");
            GUILayout.TextField(SecurityCode, EditorStyles.wordWrappedLabel);


            GUILayout.Space(5);


            GUILayout.Label("开始时间");
            start = GUILayout.TextField(start, GUILayout.MinWidth(200f));


            GUILayout.Space(5);

            GUILayout.Label("结束时间");
            end = GUILayout.TextField(end, GUILayout.MinWidth(200f));



            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (GUILayout.Button("生成"))
            {
                CreateFile();
            }
            GUILayout.Space(5);


            if (GUILayout.Button("关闭"))
            {
                Close();
            }
            EditorGUILayout.EndVertical();

        }
        private void CreateFile()
        {
            string path = Application.streamingAssetsPath + "/Data/" + fileName + ".json";

            if (DateTime.TryParse(start, out dateTimeStart) && DateTime.TryParse(end, out dateTimeEnd))
            {
                JsonData jsonData = new JsonData();
                jsonData["Start"] = start;
                jsonData["End"] = end;
                string content = EncryptionUtil.Encrypt(jsonData.ToJson(), SecurityCode);
                ExcelTool.SaveJsonData(content, path);
                ExcelTool.SaveJsonData(SecurityCode, string.Format("{0}{1}", Application.streamingAssetsPath, "/Data/SecurityCode.json"));
                Debug.Log("时间锁文件生成成功");
            }



            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }






        public partial class UnityEditorTools
        {
            [MenuItem("UnityFramework/程序时间锁设置")]
            public static void CreateTimeLockWindows()
            {
                TimeLock window = EditorWindow.GetWindow<TimeLock>();//获取指定类型的窗口.
                window.titleContent = new GUIContent("程序时间锁设置");
                window.minSize = new Vector2(300, 230);
                window.Show();
            }
        }
    }
}
