/*
* FileName:          EditorSetMaterial
* CompanyName:       
* Author:            relly
* Description:       
* 
*/
#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class EditorSetMaterial : MonoBehaviour
{

    public Material target;
    MeshRenderer[] MeshRenderer;


    public void Start()
    {

        MeshRenderer = GetComponentsInChildren<MeshRenderer>();

        foreach (var item in MeshRenderer)
        {
            List<Material> lsit = new List<Material>();
            for (int i = 0; i < item.sharedMaterials.Length; i++)
            {
                lsit.Add(target);
            }
            item.sharedMaterials = lsit.ToArray();
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(EditorSetMaterial))]
public class EditorEditorSetMaterialInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorSetMaterial script = target as EditorSetMaterial;
        if (GUILayout.Button("设置模型所有材质为target"))
        {
            script.Start();
        }
    }
}
#endif




#endif