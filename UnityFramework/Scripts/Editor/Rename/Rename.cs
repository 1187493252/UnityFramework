/*
* FileName:          Rename
* CompanyName:  
* Author:            relly
* Description:       在Hierarchy窗口对选中的物体重命名,使用场景:对多个物体下子物体的批量同名命名
* 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{
    public class Rename : EditorWindow
    {
        public enum Affix
        {
            prefix,//前缀
            suffix
        }
        public enum Position
        {
            start,
            end
        }
        string rechristenobjName = "";
        string renameobjName = "4";
        string index = "0";

        string affix = "";
        Affix mAffix;
        Position mPosition;


        private void RechristenObj()
        {
            if (string.IsNullOrEmpty(rechristenobjName))
            {
                return;
            }
            rechristenobjName = rechristenobjName.Trim();
            foreach (UnityEngine.Object obj in Selection.objects)
            {
                obj.name = rechristenobjName;
            }
            Close();

        }
        private void RenameObj()
        {
            if (string.IsNullOrEmpty(renameobjName))
            {
                return;
            }
            int num = Convert.ToInt32(renameobjName);
            renameobjName = renameobjName.Trim();
            if (mPosition == Position.start)
            {
                foreach (UnityEngine.Object obj in Selection.objects)
                {
                    List<char> list = obj.name.ToCharArray().ToList();
                    list.RemoveRange(0, num);
                    obj.name = new string(list.ToArray());
                }
            }
            else
            {
                foreach (UnityEngine.Object obj in Selection.objects)
                {
                    List<char> list = obj.name.ToCharArray().ToList();
                    list.RemoveRange(list.Count - num, num);
                    obj.name = new string(list.ToArray());
                }
            }

            Close();

        }
        private void AddAffix()
        {
            if (string.IsNullOrEmpty(affix))
            {
                return;
            }
            affix = affix.Trim();
            if (mAffix == Affix.prefix)
            {
                foreach (UnityEngine.Object obj in Selection.objects)
                {
                    obj.name = affix + obj.name;
                }
            }
            else
            {
                foreach (UnityEngine.Object obj in Selection.objects)
                {
                    obj.name = obj.name + affix;
                }
            }

            Close();

        }

        private void RenameObjToNum()
        {
            if (string.IsNullOrEmpty(index))
            {
                return;
            }
            index = index.Trim();
            int num = 0;

            if (!int.TryParse(index, out num))
            {
                num = 0;
            }
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                int temp = num + i;
                Selection.objects[i].name = temp.ToString();
            }

            Close();


        }

        private void RenameObjToNumAffix()
        {
            if (string.IsNullOrEmpty(index))
            {
                return;
            }
            index = index.Trim();
            int num = 0;

            if (!int.TryParse(index, out num))
            {
                num = 0;
            }

            if (mAffix == Affix.prefix)
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    int temp = num + i;
                    string name = Selection.objects[i].name;
                    name = temp + name;
                    Selection.objects[i].name = name;

                }
            }
            else
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    int temp = num + i;
                    string name = Selection.objects[i].name;

                    name = name + temp;

                    Selection.objects[i].name = name;


                }
            }
            Close();

        }

        void OnGUI()
        {

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("更改多个物体为同名", EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(100));

            rechristenobjName = EditorGUILayout.TextArea(rechristenobjName, EditorStyles.textArea, GUILayout.MaxWidth(260), GUILayout.MinHeight(50));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("更改", GUILayout.Width(295), GUILayout.Height(20)))
            {
                RechristenObj();
            }
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            GUILayout.Label("删除Name首位/末位N个字符", EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(100));
            Regex regex = new Regex("^[0-9]*$");
            renameobjName = regex.Match(EditorGUILayout.TextArea(renameobjName, EditorStyles.textArea, GUILayout.MaxWidth(260), GUILayout.MinHeight(50))).Value;
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("删除首位", GUILayout.Width(146), GUILayout.Height(20)))
            {
                mPosition = Position.start;
                RenameObj();
            }
            if (GUILayout.Button("删除末位", GUILayout.Width(146), GUILayout.Height(20)))
            {
                mPosition = Position.end;
                RenameObj();
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();

            GUILayout.Label("增加前/后缀", EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(100));

            affix = EditorGUILayout.TextArea(affix, EditorStyles.textArea, GUILayout.MaxWidth(260), GUILayout.MinHeight(50));

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("增加前缀", GUILayout.Width(146), GUILayout.Height(20)))
            {
                mAffix = Affix.prefix;
                AddAffix();
            }
            if (GUILayout.Button("增加后缀", GUILayout.Width(146), GUILayout.Height(20)))
            {
                mAffix = Affix.suffix;
                AddAffix();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();


            GUILayout.BeginHorizontal();

            GUILayout.Label("多个物体编号命名递增,默认从0开始", EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(100));


            index = regex.Match(EditorGUILayout.TextArea(index, EditorStyles.textArea, GUILayout.MaxWidth(260), GUILayout.MinHeight(50))).Value;

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();



            if (GUILayout.Button("更改", GUILayout.Width(99), GUILayout.Height(20)))
            {
                RenameObjToNum();
            }

            if (GUILayout.Button("增加前缀", GUILayout.Width(99), GUILayout.Height(20)))
            {
                mAffix = Affix.prefix;
                RenameObjToNumAffix();
            }
            if (GUILayout.Button("增加后缀", GUILayout.Width(99), GUILayout.Height(20)))
            {
                mAffix = Affix.suffix;
                RenameObjToNumAffix();
            }

            GUILayout.EndHorizontal();




            EditorGUILayout.Space();
            if (GUILayout.Button("关闭", GUILayout.Width(295), GUILayout.Height(20)))
            {
                Close();
            }
        }

    }
    public partial class UnityEditorTools
    {
        [MenuItem("GameObject/重命名 Ctrl+Alt+R %&R", false, 1)]
        private static void RenameWindow()
        {
            Rename window = EditorWindow.GetWindow<Rename>();
            window.titleContent = new GUIContent("重命名");
            window.maxSize = new Vector2(300, 400);
            window.minSize = new Vector2(300, 400);
            window.Show();
        }
    }
}


