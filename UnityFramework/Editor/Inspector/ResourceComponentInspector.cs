//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using Framework;
using Framework.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityFramework.Runtime;

namespace UnityFramework.Editor
{
    [CustomEditor(typeof(ResourceComponent))]
    internal sealed class ResourceComponentInspector : UnityFrameworkInspector
    {

        private const string NoneOptionName = "<Custom>";
        private SerializedProperty m_ResourceHelperTypeName = null;
        private string[] m_ResourceHelperTypeNames = null;
        private int m_ResourceHelperTypeNameIndex = 0;

        private SerializedProperty m_CustomResourceHelper = null;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            ResourceComponent t = (ResourceComponent)target;


            int resourceHelperSelectedIndex = EditorGUILayout.Popup("ResourceHelper", m_ResourceHelperTypeNameIndex, m_ResourceHelperTypeNames);

            if (resourceHelperSelectedIndex != m_ResourceHelperTypeNameIndex)
            {
                m_ResourceHelperTypeNameIndex = resourceHelperSelectedIndex;
                m_ResourceHelperTypeName.stringValue = resourceHelperSelectedIndex <= 0 ? null : m_ResourceHelperTypeNames[resourceHelperSelectedIndex];
            }


            if (m_ResourceHelperTypeNameIndex <= 0)
            {
                EditorGUILayout.PropertyField(m_CustomResourceHelper);
                if (m_CustomResourceHelper.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox(Utility.Text.Format("You must set Custom Helper."), MessageType.Error);
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {


            m_ResourceHelperTypeName = serializedObject.FindProperty("m_ResourceHelperTypeName");

            m_CustomResourceHelper = serializedObject.FindProperty("m_CustomResourceHelper");


            RefreshTypeNames();
        }




        private void RefreshTypeNames()
        {
            List<string> resourceHelperTypeNames = new List<string>
            {
                NoneOptionName
            };
            resourceHelperTypeNames.AddRange(Type.GetRuntimeTypeNames(typeof(IResourceHelper)));
            m_ResourceHelperTypeNames = resourceHelperTypeNames.ToArray();
            m_ResourceHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(m_ResourceHelperTypeName.stringValue))
            {
                m_ResourceHelperTypeNameIndex = resourceHelperTypeNames.IndexOf(m_ResourceHelperTypeName.stringValue);
                if (m_ResourceHelperTypeNameIndex <= 0)
                {
                    m_ResourceHelperTypeNameIndex = 0;
                    m_ResourceHelperTypeName.stringValue = NoneOptionName;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
