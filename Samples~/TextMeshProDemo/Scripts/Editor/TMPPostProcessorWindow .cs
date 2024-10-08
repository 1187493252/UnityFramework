/*
* FileName:          TMPPostProcessorWindow 
* CompanyName:       
* Author:            relly
* Description:       
*/


using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TMPPostProcessorWindow : EditorWindow
{
    public class TMPPostProcessorSetting
    {
       public TMP_FontAsset TMPFontAsset;//TMPAsset图集资产
       public int MaxSize = 4096;//最大分辨率
      public TextureImporterFormat TextureImporterFormat = TextureImporterFormat.ASTC_6x6;//压缩格式
        public TextureCompressionQuality TextureCompressionQuality = TextureCompressionQuality.Best;//压缩质量
    }

 public List<TMPPostProcessorSetting> TMPPostProcessorSettings = new List<TMPPostProcessorSetting>();//要后处理的TMP字体文件
    //  [ToolStorehouse("TMP字体图集优化工具", ToolStorehouseAttribute.Category.UI, false)]
    [MenuItem("UnityFramework/TMP字体图集优化工具")]
    private static void ShowTMPPostProcessorWindow()
    {
        var tmpWindow = GetWindow<TMPPostProcessorWindow>();
        tmpWindow.Show();
    }

    //一键优化
    public void Execute()
    {
        foreach (var tmpFontAsset in TMPPostProcessorSettings)
        {
            string fontPath = AssetDatabase.GetAssetPath(tmpFontAsset.TMPFontAsset);
            string texturePath = fontPath.Replace(".asset", ".png");
            TMP_FontAsset targeFontAsset = tmpFontAsset.TMPFontAsset;
            Texture2D texture2D = new Texture2D(targeFontAsset.atlasTexture.width,
                targeFontAsset.atlasTexture.height,
                TextureFormat.Alpha8, false);
            Graphics.CopyTexture(targeFontAsset.atlasTexture, texture2D);

            byte[] dataBytes = texture2D.EncodeToPNG();
            FileStream fs = File.Open(texturePath, FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();
            AssetDatabase.Refresh();

            texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            textureImporter.textureType = TextureImporterType.Default;

            TextureImporterPlatformSettings androidSetting = textureImporter.GetPlatformTextureSettings("Android");
            androidSetting.overridden = true;
            androidSetting.format = tmpFontAsset.TextureImporterFormat;
            androidSetting.maxTextureSize = tmpFontAsset.MaxSize;
            androidSetting.compressionQuality = (int)tmpFontAsset.TextureCompressionQuality;

            TextureImporterPlatformSettings iosSetting = textureImporter.GetPlatformTextureSettings("iPhone");
            iosSetting.overridden = true;
            iosSetting.format = tmpFontAsset.TextureImporterFormat;
            iosSetting.maxTextureSize = tmpFontAsset.MaxSize;
            iosSetting.compressionQuality = (int)tmpFontAsset.TextureCompressionQuality;

            textureImporter.SetPlatformTextureSettings(androidSetting);
            textureImporter.SetPlatformTextureSettings(iosSetting);

            textureImporter.mipmapEnabled = false;
            textureImporter.textureType = TextureImporterType.Default;
            textureImporter.SaveAndReimport();
            AssetDatabase.RemoveObjectFromAsset(targeFontAsset.atlasTexture);
            targeFontAsset.atlasTextures[0] = texture2D;
            targeFontAsset.material.mainTexture = texture2D;
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
