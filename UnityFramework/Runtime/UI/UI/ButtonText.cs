/*
* FileName:          ButtonText
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UnityFramework.UI
{
    public class ButtonText : MonoBehaviour
    {
        public bool changeBtnName;
        public string text;

    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ButtonText))]
    public class ButtonTextEditor : UnityEditor.Editor
    {
        private SerializedProperty m_Text = null;
        private SerializedProperty m_ChangeBtnName = null;

        private void OnEnable()
        {
            m_Text = serializedObject.FindProperty("text");
            m_ChangeBtnName = serializedObject.FindProperty("changeBtnName");

        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ButtonText t = (ButtonText)target;
            Text text = t.gameObject.GetComponentInChildren<Text>();
            if (text)
            {
                text.text = m_Text.stringValue;
            }
            if (m_ChangeBtnName.boolValue)
            {
                t.gameObject.name = m_Text.stringValue;
            }
        }
    }
#endif

}
