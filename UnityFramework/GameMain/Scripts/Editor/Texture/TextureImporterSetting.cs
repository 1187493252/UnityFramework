/*
* FileName:          TextureImporterSetting
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{
    /// <summary>
    /// Texture导入设置
    /// </summary>
    public class TextureImporterSetting : EditorWindow
    {


        TextureImporterType textureType = TextureImporterType.Sprite;
        SpriteImportMode spriteMode = SpriteImportMode.Single;
        bool read_write_enabled;
        int maxTextureSize_Default = 2048;
        int maxTextureSize_Standalone = 2048;
        int maxTextureSize_Android = 2048;
        int maxTextureSize_WebGL = 512;
        bool override_Standalone;
        bool override_Android;
        bool override_WebGL;

        void OnGUI()
        {
            EditorGUILayout.LabelField("说明:对选中文件夹的Texture和子文件夹的Texture进行设置,除Default设置,其他平台需勾选Override,否则不生效", EditorStyles.wordWrappedLabel);
            EditorGUILayout.Space();//空一行


            textureType = (TextureImporterType)EditorGUILayout.EnumPopup("Texture Type", textureType);
            EditorGUILayout.Space();//空一行

            spriteMode = (SpriteImportMode)EditorGUILayout.EnumPopup("Sprite Mode", spriteMode);
            EditorGUILayout.Space();//空一行

            read_write_enabled = EditorGUILayout.Toggle("Read/Write Enabled", read_write_enabled);
            EditorGUILayout.Space();//空一行

            EditorGUILayout.LabelField("Default设置");
            maxTextureSize_Default = EditorGUILayout.IntField("maxTextureSize", maxTextureSize_Default);
            EditorGUILayout.Space();//空一行

            EditorGUILayout.LabelField("Standalone设置");
            override_Standalone = EditorGUILayout.Toggle("Override for Standalone", override_Standalone);
            maxTextureSize_Standalone = EditorGUILayout.IntField("maxTextureSize", maxTextureSize_Standalone);
            EditorGUILayout.Space();//空一行

            EditorGUILayout.LabelField("Android设置");
            override_Android = EditorGUILayout.Toggle("Override for Android", override_Android);
            maxTextureSize_Android = EditorGUILayout.IntField("maxTextureSize", maxTextureSize_Android);
            EditorGUILayout.Space();//空一行

            EditorGUILayout.LabelField("WebGL设置");
            override_WebGL = EditorGUILayout.Toggle("Override for WebGL", override_WebGL);
            maxTextureSize_WebGL = EditorGUILayout.IntField("maxTextureSize", maxTextureSize_WebGL);
            EditorGUILayout.Space();//空一行
            EditorGUILayout.Space();//空一行


            if (GUILayout.Button("设置Sprite", GUILayout.ExpandWidth(true)))
            {
                Object[] targetObj = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);//得到选中的文件，包括选中文件夹的子文件和子文件夹
                if (targetObj != null && targetObj.Length > 0)
                {
                    for (int i = 0; i < targetObj.Length; i++)
                    {
                        if (targetObj[i] is Texture)                                  //检测是不是Texture
                        {
                            string path = AssetDatabase.GetAssetPath(targetObj[i]);   //得到资源的路径
                            TextureImporter texture = AssetImporter.GetAtPath(path) as TextureImporter; //通过路径得到资源
                            texture.textureType = textureType;                        //TextureType (Enum)
                            texture.spriteImportMode = spriteMode;                    //SpriteMode  (Enum)
                            texture.isReadable = read_write_enabled;

                            //默认
                            //TextureImporterPlatformSettings defaultSetting = texture.GetDefaultPlatformTextureSettings();
                            //defaultSetting.maxTextureSize = maxTextureSize_Default;
                            //texture.SetPlatformTextureSettings(defaultSetting);
                            texture.maxTextureSize = maxTextureSize_Default;

                            if (override_Standalone)
                            {
                                TextureImporterPlatformSettings platformSettingStandalone = texture.GetPlatformTextureSettings("Standalone");
                                if (platformSettingStandalone != null)
                                {
                                    platformSettingStandalone.overridden = true;
                                    platformSettingStandalone.maxTextureSize = maxTextureSize_Standalone;
                                    texture.SetPlatformTextureSettings(platformSettingStandalone);
                                }
                            }

                            if (override_Android)
                            {
                                TextureImporterPlatformSettings platformSettingAndroid = texture.GetPlatformTextureSettings("Android");
                                if (platformSettingAndroid != null)
                                {
                                    platformSettingAndroid.overridden = true;
                                    platformSettingAndroid.maxTextureSize = maxTextureSize_Android;
                                    texture.SetPlatformTextureSettings(platformSettingAndroid);
                                }
                            }

                            if (override_WebGL)
                            {
                                TextureImporterPlatformSettings platformSettingWebGL = texture.GetPlatformTextureSettings("WebGL");
                                if (platformSettingWebGL != null)
                                {
                                    platformSettingWebGL.overridden = true;
                                    platformSettingWebGL.maxTextureSize = maxTextureSize_WebGL;
                                    texture.SetPlatformTextureSettings(platformSettingWebGL);
                                }
                            }
                          //  texture.SaveAndReimport();
                            AssetDatabase.ImportAsset(path);
                        }
                    }
                    AssetDatabase.Refresh();
                }
            }

            if (GUILayout.Button("关闭", GUILayout.ExpandWidth(true)))
            {
                Close();
            }
        }



        public partial class UnityEditorTools
        {
            [MenuItem("UnityFramework/Texture导入设置")]
            public static void CreateTextureImporterSettingWindows()
            {
                TextureImporterSetting window = EditorWindow.GetWindow<TextureImporterSetting>();//获取指定类型的窗口.
                window.titleContent = new GUIContent("Texture导入设置");
                window.minSize = new Vector2(300, 230);
                window.Show();
            }
        }
    }

}


