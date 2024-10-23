/*
* FileName:          EditorSetMesh
* CompanyName:       杭州中锐
* Author:            relly
* Description:       
* 
*/
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
public class EditorSetMesh : MonoBehaviour
{

    public GameObject target;
    MeshFilter[] meshFilter;
    string content;

    public void Start()
    {

        meshFilter = GetComponentsInChildren<MeshFilter>();
        MeshFilter[] meshFilters = target.GetComponentsInChildren<MeshFilter>();
        foreach (var item in meshFilter)
        {
            content = item.gameObject.name.Split('_')[0];
            foreach (var item1 in meshFilters)
            {
                if (item1.sharedMesh.name == content)
                {
                    item.sharedMesh = item1.sharedMesh;
                }
            }
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(EditorSetMesh))]
public class EditorSetMeshEditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorSetMesh script = target as EditorSetMesh;
        if (GUILayout.Button("从目标模型上获取同名Mesh并设置"))
        {
            script.Start();
        }
    }
}
#endif




#endif