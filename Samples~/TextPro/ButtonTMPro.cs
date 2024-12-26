/*
* FileName:          ButtonTextPro
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
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UnityFramework.UI
{
    public class ButtonTMPro : MonoBehaviour
    {
        public bool changeBtnName;
        public string text;

    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ButtonTMPro))]
    public class ButtonTMProEditor : UnityEditor.Editor
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

            ButtonTMPro t = (ButtonTMPro)target;
            TMP_Text text = t.gameObject.GetComponentInChildren<TMP_Text>();
            if (text)
            {
                text.text = m_Text.stringValue;
                if (m_Text.stringValue != "" && m_ChangeBtnName.boolValue)
                {
                    t.gameObject.name = m_Text.stringValue;
                }
            }

        }
    }
#endif

}
