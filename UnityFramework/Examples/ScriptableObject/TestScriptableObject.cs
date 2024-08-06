/*
* FileName:          TestScriptableObject
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

//只能在Assets/Create下
[CreateAssetMenu(fileName = "TestScriptableObject", menuName = "ScriptableObjects/TestScriptableObject", order = 0)]
public class TestScriptableObject : ScriptableObject
{
    public string prefabName;
    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
#if UNITY_EDITOR

    //如果存在同名的不会创建
    [MenuItem("Assets/Create/ScriptableObjects/My Scriptable Object", false, 0)]
    public static void CreateMyAsset()
    {
        UnityEngine.Object obj = Selection.activeObject;
        if (obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            TestScriptableObject asset = CreateInstance<TestScriptableObject>();
            if (asset)
            {
                int index = 0;
                string confName = "";
                UnityEngine.Object obj1 = null;
                do
                {
                    confName = path + "/" + typeof(TestScriptableObject).Name + "_" + index + ".asset";
                    obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(TestScriptableObject));
                    index++;
                } while (obj1);
                AssetDatabase.CreateAsset(asset, confName);
                AssetDatabase.SaveAssets();
                //聚焦
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
        }
    }
#endif


}


