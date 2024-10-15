/*
* FileName:          DeleteMissingScript
* CompanyName:       
* Author:            relly
* Description:       删除丢失脚本
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace UnityFramework.Editor
{
    public class DeleteMissingScript
    {
        [MenuItem("UnityFramework/删除丢失脚本(慎用)")]
        static void CleanupMissingScript()
        {
            //可以获取当前场景及Project下所有物体
            GameObject[] gameObjects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
            //GameObject[] gameObjects = Selection.gameObjects;//获取选择的物体

            foreach (GameObject item in gameObjects)
            {
                RemoveRecursively(item);
            }
        }
        private static void RemoveRecursively(GameObject g)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);

            foreach (Transform childT in g.transform)
            {
                RemoveRecursively(childT.gameObject);
            }
        }
    }

}
