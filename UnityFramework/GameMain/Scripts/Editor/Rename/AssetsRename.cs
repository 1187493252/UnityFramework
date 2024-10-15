/*
* FileName:          AssetsRename
* CompanyName:  
* Author:            Relly
* Description:       在Project窗口对选中的资源重命名
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
    public class AssetsRename : EditorWindow
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
                ChangName(obj, rechristenobjName);
            }
            AssetDatabase.Refresh();
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

                    ChangName(obj, new string(list.ToArray()));

                }
            }
            else
            {
                foreach (UnityEngine.Object obj in Selection.objects)
                {
                    List<char> list = obj.name.ToCharArray().ToList();
                    list.RemoveRange(list.Count - num, num);
                    ChangName(obj, new string(list.ToArray()));

                }
            }
            AssetDatabase.Refresh();

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

                    ChangName(obj, affix + obj.name);

                }
            }
            else
            {
                foreach (UnityEngine.Object obj in Selection.objects)
                {
                    ChangName(obj, obj.name + affix);

                }
            }
            AssetDatabase.Refresh();

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
            //for (int i = 0; i < Selection.objects.Length; i++)
            //{
            //    ChangName(Selection.objects[i], "temp" + i);
            //}
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                int temp = num + i;

                ChangName(Selection.objects[i], temp.ToString());

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
                    ChangName(Selection.objects[i], temp + name);

                }
            }
            else
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    int temp = num + i;
                    string name = Selection.objects[i].name;
                    ChangName(Selection.objects[i], name + temp);

                }
            }
        }

        void ChangName(UnityEngine.Object obj, string newName)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            AssetDatabase.RenameAsset(path, newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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

            GUILayout.Label("删除Name末位N个字符", EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(100));
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
        [MenuItem("Assets/资源重命名 ", false, 1)]
        private static void AssetsRenameWindow()
        {
            AssetsRename window = EditorWindow.GetWindow<AssetsRename>();
            window.titleContent = new GUIContent("重命名");
            window.maxSize = new Vector2(300, 400);
            window.minSize = new Vector2(300, 400);
            window.Show();
        }

    }
}


