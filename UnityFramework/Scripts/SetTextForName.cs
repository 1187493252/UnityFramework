/*
* FileName:          SetTextForName
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static SetTextForName;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SetTextForName : MonoBehaviour
{
    public FollowTarget followTarget;
    [Header("多少字数换行")]
    public int cout = 0;
    [Header("字体大小")]
    public int fontSize = 0;
    public enum FollowTarget
    {
        ThisChild,
        This
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(SetTextForName))]
public class SetTextForNameEditorInspector : Editor
{




    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //获取脚本对象
        SetTextForName script = target as SetTextForName;


        if (GUILayout.Button("设置Text内容"))
        {
            if (script.followTarget == FollowTarget.ThisChild)
            {
                foreach (Transform item in script.transform)
                {
                    Text text = item.GetComponentInChildren<Text>();
                    if (text)
                    {
                        string content = item.gameObject.name;
                        if (content == "Text")
                        {
                            content = item.parent.gameObject.name;
                        }
                        Char[] temp = content.ToCharArray();
                        List<Char> list = temp.ToList();
                        int num = temp.Length / 2;
                        if (script.fontSize != 0)
                        {
                            if (list.Count > script.cout)
                            {
                                list.Insert(num, '\n');
                            }
                        }

                        text.text = new string(list.ToArray());
                        if (script.fontSize != 0)
                        {
                            text.fontSize = script.fontSize;

                        }
                    }
                }
            }
            else
            {
                Text text = script.GetComponentInChildren<Text>();
                if (text)
                {
                    string content = script.gameObject.name;

                    Char[] temp = content.ToCharArray();
                    List<Char> list = temp.ToList();
                    int num = temp.Length / 2;
                    if (script.fontSize != 0)
                    {
                        if (list.Count > script.cout)
                        {
                            list.Insert(num, '\n');
                        }
                    }

                    text.text = new string(list.ToArray());
                    if (script.fontSize != 0)
                    {
                        text.fontSize = script.fontSize;

                    }
                }
            }

        }

    }
}

#endif


