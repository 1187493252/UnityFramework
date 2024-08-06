/*
* FileName:          CoroutineComponentInspector
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityFramework.Runtime;

namespace UnityFramework.Editor
{
    [CustomEditor(typeof(CoroutineComponent))]
    internal sealed class CoroutineComponentInspector : UnityFrameworkInspector
    {


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CoroutineComponent t = (CoroutineComponent)target;



            if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Coroutine Count", t.Count >= 0 ? t.Count.ToString() : "<Unknown>");
                if (t.Count > 0)
                {
                    List<string> settingNames = t.GetAllCoroutineNames();
                    foreach (string settingName in settingNames)
                    {
                        EditorGUILayout.LabelField(settingName);
                    }
                }
            }



            serializedObject.ApplyModifiedProperties();

            Repaint();
        }


    }
}
