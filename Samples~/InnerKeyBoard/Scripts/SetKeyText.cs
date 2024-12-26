/*
* FileName:          SetKeyText
* CompanyName:       
* Author:            
* Description:       
*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static SetKeyText;
using InnerKeyboard;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SetKeyText : MonoBehaviour
{

    [Header("字体大小")]
    public int fontSize = 0;

}
#if UNITY_EDITOR
[CustomEditor(typeof(SetKeyText))]
public class SetTextForNameEditorInspector : Editor
{



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //获取脚本对象
        SetKeyText script = target as SetKeyText;

        if (GUILayout.Button("设置Text内容"))
        {
            ComBtn[] comBtns = script.GetComponentsInChildren<ComBtn>();
            foreach (var comBtn in comBtns)
            {
                comBtn.gameObject.name = comBtn.NorText;
                Text text = comBtn.transform.Find("Text").GetComponent<Text>();
                text.text = comBtn.NorText;
                Text text1 = comBtn.transform.Find("ShiftText").GetComponent<Text>();
                text1.text = comBtn.SfText;
                if (script.fontSize != 0)
                {
                    text.fontSize = script.fontSize;
                    text1.fontSize = script.fontSize;
                }
            }

        }

    }
}

#endif




