/*
* FileName:          DemoWindow
* CompanyName:       
* Author:            relly
* Description:       
* 
*/
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{
    public class DemoWindow : EditorWindow
    {
        private Vector2 LeftMenuSlider;
        private Vector2 RightMenuSlider;

        private int NowTask = -1;
        /// <summary>
        /// 全部任务
        /// </summary>
        private List<EditorTaskConfigData> AllTask = new List<EditorTaskConfigData>();

        /// <summary>
        /// Left菜单拷贝数据
        /// </summary>
        private List<TaskData> LeftCopyTaskItem = new List<TaskData>();
        private string[] AllCSName;

        private void OnEnable()
        {

        }

        private void OnGUI()
        {

            LeftMenu();
            RightMenu();
            ButtomMenu();

        }
        private void ButtomMenu()
        {
            GUILayout.BeginArea(new Rect(0, this.position.height * 0.805f, this.position.width, this.position.height * 0.195f), new GUIStyle(EditorStyles.helpBox) { });
            ShowFileIcon();
            ShowFileInfo();
            GUILayout.EndArea();
        }
        private void ShowFileIcon()
        {
            GUILayout.BeginArea(new Rect(this.position.width * 0.07f, this.position.height * 0.01f, this.position.width * 0.2f, this.position.height * 0.195f));
            if (NowTask >= 0)
            {

                GUILayout.Label(" ———", GUILayout.Width(60), GUILayout.Height(10));
                GUILayout.Label(@"|  ---- |_\", GUILayout.Width(60), GUILayout.Height(10));
                GUILayout.Label("|  -----   |", GUILayout.Width(60), GUILayout.Height(10));
                GUILayout.Label("|  -----   |", GUILayout.Width(60), GUILayout.Height(10));
                GUILayout.Label("|  -----   |", GUILayout.Width(60), GUILayout.Height(10));
                GUILayout.Label(" ————", GUILayout.Width(60), GUILayout.Height(10));

            }
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(this.position.width * 0.005f, this.position.height * 0.15f, this.position.width * 0.2f, this.position.height * 0.195f));
            if (NowTask >= 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                AllTask[NowTask].TaskName = GUILayout.TextField(AllTask[NowTask].TaskName, GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
        private void ShowFileInfo()
        {
            GUILayout.BeginArea(new Rect(this.position.width * 0.22f, this.position.height * 0.01f, this.position.width * 0.78f, this.position.height * 0.195f));
            if (NowTask >= 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("任务序号", GUILayout.Width(60));
                AllTask[NowTask].TaskID = GUILayout.TextField(AllTask[NowTask].TaskID, GUILayout.Width(100));
                GUILayout.EndHorizontal();
                GUILayout.Label("任务备注:", GUILayout.Width(60));
                AllTask[NowTask].Annotation = GUILayout.TextArea(AllTask[NowTask].Annotation, GUILayout.Height(85));
            }
            GUILayout.EndArea();
        }
        private void RightMenu()
        {
            GUILayout.BeginArea(new Rect(this.position.width * 0.205f, 0, this.position.width * 0.795f, this.position.height * 0.8f), new GUIStyle(EditorStyles.helpBox) { });
            RightMenuRightMenu();
            ShowTip();
            RightMenuSlider = GUILayout.BeginScrollView(RightMenuSlider);
            ShowAllTask();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        private void ShowAllTask()
        {
            if (NowTask < 0) return;
            List<TaskData> AllTaskItem = AllTask[NowTask].AllTaskItem;
            for (int i = 0; i < AllTaskItem.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                AllTaskItem[i].ID = GUILayout.TextField(AllTaskItem[i].ID, GUILayout.Width(80));
                GUILayout.Space(5);
                AllTaskItem[i].IsHighlight = (EditorGUILayout.Popup(AllTaskItem[i].IsHighlight ? 1 : 0, new string[] { "否", "是" }, GUILayout.Width(100)) == 0);
                GUILayout.Space(5);
                AllTaskItem[i].CSName = AllCSName[EditorGUILayout.Popup(NowCSNameIndex(AllTaskItem[i].CSName), AllCSName, GUILayout.Width(150))];
                GUILayout.Space(10);
                if (GUILayout.Button("+^", GUILayout.Width(24)))
                { AllTaskItem.Insert(i, new TaskData()); AllTask[NowTask].AllItemAnnotation.Insert(i, ""); }
                if (GUILayout.Button("+", GUILayout.Width(20)))
                { AllTaskItem.Insert(i + 1, new TaskData()); AllTask[NowTask].AllItemAnnotation.Insert(i + 1, ""); }
                if (GUILayout.Button("-", GUILayout.Width(20)))
                { AllTaskItem.RemoveAt(i); AllTask[NowTask].AllItemAnnotation.RemoveAt(i); }
                GUILayout.Space(10);
                try
                {
                    AllTask[NowTask].AllItemAnnotation[i] = GUILayout.TextField(AllTask[NowTask].AllItemAnnotation[i]);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }
                GUILayout.EndHorizontal();
            }
        }
        private int NowCSNameIndex(string name)
        {
            for (int i = 0; i < AllCSName.Length; i++)
            {
                if (string.Compare(AllCSName[i], name) == 0)
                    return i;
            }
            return 0;
        }
        private void ShowTip()
        {
            if (NowTask < 0) return;
            GUILayout.BeginHorizontal();
            GUILayout.Label("物体序号", GUILayout.Width(80));
            GUILayout.Space(5);
            GUILayout.Label("是否显示高亮", GUILayout.Width(100));
            GUILayout.Space(5);
            GUILayout.Label("脚本名称", GUILayout.Width(150));
            GUILayout.Space(60);
            GUILayout.FlexibleSpace();
            GUILayout.Label("备注");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        private void RightMenuRightMenu()
        {
            if (NowTask < 0) return;
            if (UnityEngine.Event.current.type == EventType.ContextClick)
            {
                GenericMenu genericMenu = new GenericMenu();
                genericMenu.AddItem(new GUIContent("存储数据"), false, RightSave);
                genericMenu.AddItem(new GUIContent("添加模块"), false, RightMenuAddItem);
                genericMenu.AddItem(new GUIContent("删除模块"), false, RightMenuRemoveItem);
                genericMenu.AddItem(new GUIContent("复制任务模块"), false, LeftMenuCopyTask);
                genericMenu.AddItem(new GUIContent("粘贴任务模块"), false, LeftMenuPasteTask);
                genericMenu.ShowAsContext();
            }
        }
        private void RightSave()
        {

        }
        private void RightMenuRemoveItem()
        {
            if (AllTask[NowTask].AllTaskItem.Count <= 0) return;
            try
            {
                AllTask[NowTask].AllTaskItem.RemoveAt(AllTask[NowTask].AllTaskItem.Count - 1);
                AllTask[NowTask].AllItemAnnotation.RemoveAt(AllTask[NowTask].AllTaskItem.Count - 1);
            }
            catch (System.Exception e)
            {
                Console.Write(e);
            }
        }

        private void RightMenuAddItem()
        {
            AllTask[NowTask].AllTaskItem.Add(new TaskData
            {
                TaskID = AllTask[NowTask].TaskID
            });
            AllTask[NowTask].AllItemAnnotation.Add("");
        }
        private void LeftMenu()
        {
            GUILayout.BeginArea(new Rect(0, 0, this.position.width * 0.2f, this.position.height * 0.8f), new GUIStyle(EditorStyles.helpBox) { });

            if (UnityEngine.Event.current.type == EventType.ContextClick)
            {
                GenericMenu genericMenu = new GenericMenu();
                genericMenu.AddItem(new GUIContent("添加任务模块"), false, LeftMenuAddItem);
                genericMenu.AddItem(new GUIContent("删除任务模块"), false, LeftMenuRemoveItem);

                if (NowTask >= 0)
                {
                    genericMenu.AddItem(new GUIContent("复制任务模块"), false, LeftMenuCopyTask);
                    genericMenu.AddItem(new GUIContent("粘贴任务模块"), false, LeftMenuPasteTask);
                }

                genericMenu.ShowAsContext();
            }
            LeftMenuSlider = GUILayout.BeginScrollView(LeftMenuSlider);

            for (int i = 0; i < AllTask.Count; i++)
            {
                GUILayout.Space(2);
                if (i == NowTask)
                {
                    if (GUILayout.Button(AllTask[i].TaskName, new GUIStyle(EditorStyles.helpBox) { }, GUILayout.Height(30)))
                    {
                        NowTask = -1;
                    }
                }
                else
                {
                    if (GUILayout.Button(AllTask[i].TaskName, new GUIStyle(EditorStyles.helpBox) { }, GUILayout.Height(30)))
                    {
                        NowTask = i;
                    }
                }
            }

            GUILayout.EndScrollView();
            ShowLeftToolsPanel();

            GUILayout.EndArea();
        }
        private void ShowLeftToolsPanel()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Height(20), GUILayout.Width(20))) LeftMenuAddItem();
            if (GUILayout.Button("-", GUILayout.Height(20), GUILayout.Width(20))) LeftMenuRemoveItem();
            GUILayout.EndHorizontal();
        }
        private void LeftMenuAddItem()
        {
            int Index = 0;
            string ID = "";
            if (AllTask.Count != 0)
            {
                ID = AllTask[AllTask.Count - 1].TaskID;
                if (ID.Length == 5) Index = 1;
                else if (ID.Length > 5)
                {
                    if (int.TryParse(ID.Remove(0, 5), out int i))
                    {
                        Index = i + 1;

                    }
                }
            }
            if (Index > 0)
            {
                AllTask.Add(new EditorTaskConfigData { TaskName = "NewTask", TaskID = ID.Substring(0, 5) + Index.ToString() });
            }
            else
                AllTask.Add(new EditorTaskConfigData { TaskName = "NewTask" });
        }
        /// <summary>
        /// 删除任务模块
        /// </summary>
        private void LeftMenuRemoveItem()
        {
            if (AllTask.Count <= 0) return;
            AllTask.RemoveAt(AllTask.Count - 1);
            if (NowTask >= AllTask.Count) NowTask = -1;
        }

        /// <summary>
        /// 复制当前任务模块
        /// </summary>
        private void LeftMenuCopyTask()
        {
            if (NowTask < 0) return;
            LeftCopyTaskItem = AllTask[NowTask].AllTaskItem;
        }

        private void LeftMenuPasteTask()
        {
            if (NowTask < 0 || LeftCopyTaskItem.Count <= 0) return;
            List<TaskData> NewDatas = AllTask[NowTask].AllTaskItem;
            foreach (var item in LeftCopyTaskItem)
            {
                NewDatas.Add(new TaskData
                {
                    ID = item.ID.ToString(),
                    IsHighlight = (item.IsHighlight == false),
                    CSName = item.CSName.ToString()
                });
                AllTask[NowTask].AllItemAnnotation.Add("");

            }
        }
        private void OnDestroy()
        {

        }

        /// <summary>
        /// 获取Texture2D
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>Texture2D带上自定义颜色</returns>
        private Texture2D GetTexture2D(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(1, 1, color);
            texture.Apply();
            return texture;
        }
    }
    /// <summary>
    /// 自定义菜单类.
    /// </summary>
    public partial class UnityEditorTools
    {
        [MenuItem("UnityFramework/测试窗口")]
        public static void DemoWindowWindows()
        {
            DemoWindow window = EditorWindow.GetWindow<DemoWindow>();//获取指定类型的窗口.
            window.titleContent = new GUIContent("测试窗口");

            window.Show();
        }
    }
    public class TaskData
    {
        /// <summary>
        /// 对应配置文件UIID;
        /// </summary>
        public string TaskID;

        /// <summary>
        /// 物体序号
        /// </summary>
        public string ID;

        /// <summary>
        /// 是否高亮
        /// </summary>
        public bool IsHighlight;

        /// <summary>
        /// 物体脚本名称
        /// </summary>
        public string CSName;
    }
    public class EditorTaskConfigData
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName;
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID;
        /// <summary>
        /// 注释
        /// </summary>
        public string Annotation;

        /// <summary>
        /// 每行数据的注释
        /// </summary>
        public List<string> AllItemAnnotation = new List<string>();

        /// <summary>
        /// 全部任务模块
        /// </summary>
        public List<TaskData> AllTaskItem = new List<TaskData>();
    }
}
#endif