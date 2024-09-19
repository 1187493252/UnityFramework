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
        private static readonly string[] ResourceModeNames = new string[] { "Package", "Updatable", "Updatable While Playing" };

        private const string NoneOptionName = "<None>";


        private SerializedProperty m_ResourceHelperTypeName = null;

        private string[] m_ResourceHelperTypeNames = null;
        private int m_ResourceHelperTypeNameIndex = 0;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            ResourceComponent t = (ResourceComponent)target;


            int resourceHelperSelectedIndex = EditorGUILayout.Popup("ResourceHelper", m_ResourceHelperTypeNameIndex, m_ResourceHelperTypeNames);



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
                    m_ResourceHelperTypeName.stringValue = null;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
