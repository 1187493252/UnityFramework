/*
* FileName:          AutoSetAddressables
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

public class AutoSetAddressables : ScriptableObject
{
    public List<AddressablesGroup> addressablesGroupsList = new List<AddressablesGroup>();



    [Serializable]
    public class AddressablesGroup
    {
        [TextArea(1, 2)]
        [Header("文件夹路径")]
        public string path;
        [Header("组名")]
        public string groupName;
        [Header("标签")]
        public string lableName;
        [Header("是否简化文件名")]
        public bool isSimplied;
    }



#if UNITY_EDITOR

    [MenuItem("Assets/UnityFramework/Addressables/生成自动设置分组配置文件", false, 0)]
    public static void CreateMyAsset()
    {
        UnityEngine.Object obj = Selection.activeObject;
        if (obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            AutoSetAddressables asset = CreateInstance<AutoSetAddressables>();
            if (asset)
            {
                int index = 0;
                string confName = "";
                UnityEngine.Object obj1 = null;
                do
                {
                    confName = path + "/" + typeof(AutoSetAddressables).Name + "_" + index + ".asset";
                    obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(AutoSetAddressables));
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

    [MenuItem("Assets/UnityFramework/Addressables/获取当前选中资源路径到剪贴板", false, 0)]
    public static void GetFilePath()
    {
        UnityEngine.Object obj = Selection.activeObject;
        if (obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            GUIUtility.systemCopyBuffer = path;
        }
    }

    public void AutoSetGroup(string groupName, string lableName, string assetPath, bool isSimplied = false)
    {
        var set = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetGroup Group = set.FindGroup(groupName);
        if (Group == null)
        {
            Group = set.CreateGroup(groupName, false, false, false, new List<AddressableAssetGroupSchema>
            { set.DefaultGroup.Schemas[0], set.DefaultGroup.Schemas[1]}, typeof(AddressableAssetGroupSchema));
        }
        string[] parh = Directory.GetFiles(assetPath);
        foreach (var item in parh)
        {
            if (item.EndsWith(".meta"))
            {
                continue;
            }
            string[] tmp = item.Split(new string[] { "Assets" }, StringSplitOptions.None);
            string tmp1 = "Assets" + tmp[1];
            string Guid = AssetDatabase.AssetPathToGUID(tmp1);  //获取指定路径下资源的 GUID（全局唯一标识符）
            AddressableAssetEntry asset = set.CreateOrMoveEntry(Guid, Group);
            if (isSimplied)
            {
                asset.address = Path.GetFileNameWithoutExtension(tmp1);
            }
            else
            {
                asset.address = AssetDatabase.GUIDToAssetPath(Guid);
            }
            asset.SetLabel(lableName, true, true);
        }
    }

#endif

}
#if UNITY_EDITOR
[CustomEditor(typeof(AutoSetAddressables))]
public class AutoSetAddressablesEditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //获取脚本对象
        AutoSetAddressables script = target as AutoSetAddressables;
        GUILayout.Space(20);
        if (GUILayout.Button("自动标记资源到AddressablesGroups"))
        {
            var set = AddressableAssetSettingsDefaultObject.Settings;
            foreach (var item in script.addressablesGroupsList)
            {
                script.AutoSetGroup(item.groupName, item.lableName, item.path, item.isSimplied);
            }
        }

    }
}
#endif