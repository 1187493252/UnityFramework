using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityFramework.Runtime;

[CustomEditor(typeof(TaskComponent))]
public class TaskManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TaskComponent CurTaskManager = (TaskComponent)target;
        TaskConfig CurTaskConf = CurTaskManager.CurrentTask;
        if (CurTaskConf == null)
        {
            return;
        }
        EditorGUILayout.LabelField(CurTaskConf.TaskID.ToString() + " : " + CurTaskConf.TaskName);
    }
}
#endif