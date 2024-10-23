/*
* FileName:          ChildObjSort
* CompanyName:       
* Author:            relly
* Description:       子物体排序
*/
#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace UnityFramework.Editor
{
    public partial class UnityEditorTools
    {
        [MenuItem("GameObject/子物体排序/从小到大", false, 1)]
        static void Sort()
        {
            if (Selection.activeGameObject == null)
            {
                return;
            }
            List<Transform> list = Selection.activeGameObject.GetComponentsInChildren<Transform>(true).ToList();
            list.Remove(Selection.activeGameObject.transform);
            list.Sort(delegate (Transform a, Transform b) { return a.gameObject.name.CompareTo(b.gameObject.name); });
            foreach (var item in list)
            {
                item.SetAsLastSibling();
            }
        }
        [MenuItem("GameObject/子物体排序/从大到小", false, 1)]
        static void Sort1()
        {
            if (Selection.activeGameObject == null)
            {
                return;
            }
            List<Transform> list = Selection.activeGameObject.GetComponentsInChildren<Transform>(true).ToList();
            list.Remove(Selection.activeGameObject.transform);
            list.Sort(delegate (Transform a, Transform b) { return a.gameObject.name.CompareTo(b.gameObject.name); });
            foreach (var item in list)
            {
                item.SetAsFirstSibling();
            }
        }
    }
}
#endif