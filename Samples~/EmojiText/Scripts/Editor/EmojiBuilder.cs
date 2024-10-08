/*

	Description:Create the Atlas of emojis and its data texture.

	
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class EmojiBuilder : EditorWindow
{

    // internal static string OutputPath = "Assets/EmojiText/Emoji/Output/";
    // internal static string InputPath = "/EmojiText/Emoji/Input/";
    internal static string OutputPath;
    internal static string InputPath;
    static string size = "32";
    Regex regex = new Regex("^[0-9]*$");

    private static readonly Vector2[] AtlasSize = new Vector2[]{
        new Vector2(32,32),
        new Vector2(64,64),
        new Vector2(128,128),
        new Vector2(256,256),
        new Vector2(512,512),
        new Vector2(1024,1024),
        new Vector2(2048,2048)
    };

    struct EmojiInfo
    {
        public string key;
        public string x;
        public string y;
        public string size;
    }
    private static int EmojiSize = 32;//the size of emoji.

    private Vector2 scrollPos;
    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Space(5);
        EditorGUILayout.BeginVertical();
        GUILayout.Label("表情路径: ");
        GUILayout.Space(5);
        InputPath = GUILayout.TextField(InputPath);
        GUILayout.Space(5);
        if (GUILayout.Button("选择表情路径"))
        {
            InputPath = SelectDir();
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(5);
        EditorGUILayout.BeginVertical();
        GUILayout.Label("表情输出路径: ");
        GUILayout.Space(5);
        OutputPath = GUILayout.TextField(OutputPath);
        GUILayout.Space(5);
        if (GUILayout.Button("选择表情输出路径"))
        {
            OutputPath = SelectDir();
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(5);
        EditorGUILayout.BeginVertical();
        GUILayout.Label("表情尺寸必须全部统一:2的倍数 最小32");
        GUILayout.Space(5);

        size = regex.Match(GUILayout.TextField(size)).Value;


        EditorGUILayout.EndVertical();


        GUILayout.Label("_________________________________________________________________________________________________");



        GUILayout.Space(5);

        if (GUILayout.Button("build"))
        {
            CreateData();
        }
        GUILayout.Space(10);

        if (GUILayout.Button("默认路径"))
        {
            SettingDefaultPaths();


        }
        GUILayout.Space(5);

        if (GUILayout.Button("关闭"))
        {

            Close();
        }
        GUILayout.Space(5);

        EditorGUILayout.EndScrollView();
    }

    void SettingDefaultPaths()
    {
        DirectoryInfo path = new DirectoryInfo(GetPath());

        OutputPath = path.Parent.Parent.FullName + "/Emoji/Output";
        OutputPath = OutputPath.Replace("\\", "/");
        InputPath = path.Parent.Parent.FullName + "/Emoji/Input";
        InputPath = InputPath.Replace("\\", "/");

    }

    /// <summary>
    /// 选择文件
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public string SelectFile(string filepath, string filter = "*")
    {
        string path = "";
        path = EditorUtility.OpenFilePanelWithFilters("选择文件", filepath, new string[] { "*", filter });
        return path;
    }

    /// <summary>
    /// 选择Excel文件夹
    /// </summary>
    public string SelectDir()
    {
        string path = "";
        path = EditorUtility.OpenFolderPanel("选择文件夹", Application.dataPath, "");
        return path;
    }

    //获取本脚本路径
    string GetPath()
    {
        string[] temp = this.ToString().Split('.');
        string name = temp.Last().Replace(")", "");
        name = name.Replace("(", "");
        //AssetDatabase.FindAssets查找资源,Name为CreateCustomScript,只要名字包含CreateCustomScript就会被找到,比如CreateCustomScripts,CreateCustomScriptTemplate
        string[] paths = AssetDatabase.FindAssets(name);
        if (paths.Length > 1)
        {
            //按照上述规则可能存在"同名"文件
            Debug.LogError("有同名文件:" + name);
            return null;
        }
        string m_path = AssetDatabase.GUIDToAssetPath(paths[0]);
        m_path = Path.GetDirectoryName(m_path);
        return m_path;
    }

    void CreateData()
    {
        if (!Directory.Exists(InputPath))
        {
            Debug.LogError("路径不存在");
            return;
        }
        int.TryParse(size, out EmojiSize);

        //search all emojis and compute they frames.
        Dictionary<string, int> sourceDic = new Dictionary<string, int>();
        string[] files = Directory.GetFiles(InputPath, "*.png");
        for (int i = 0; i < files.Length; i++)
        {


            string[] strs = files[i].Replace("\\", "/").Split('/');
            string[] strs2 = strs[strs.Length - 1].Split('.');
            string filename = strs2[0];

            string[] t = filename.Split('_');
            string id = t[0];
            if (sourceDic.ContainsKey(id))
            {
                sourceDic[id]++;
            }
            else
            {
                sourceDic.Add(id, 1);
            }
        }

        //create the directory if it is not exist.
        if (!Directory.Exists(OutputPath))
        {
            Directory.CreateDirectory(OutputPath);
        }

        Dictionary<string, EmojiInfo> emojiDic = new Dictionary<string, EmojiInfo>();

        int totalFrames = 0;
        foreach (int value in sourceDic.Values)
        {
            totalFrames += value;
        }
        Vector2 texSize = ComputeAtlasSize(totalFrames);
        Texture2D newTex = new Texture2D((int)texSize.x, (int)texSize.y, TextureFormat.ARGB32, false);
        Texture2D dataTex = new Texture2D((int)texSize.x / EmojiSize, (int)texSize.y / EmojiSize, TextureFormat.ARGB32, false);
        int x = 0;
        int y = 0;
        int keyindex = 0;
        string tmp = InputPath.Replace(Application.dataPath, "");

        foreach (string key in sourceDic.Keys)
        {

            for (int index = 0; index < sourceDic[key]; index++)
            {
                string path = "Assets" + tmp + "/" + key;
                if (sourceDic[key] == 1)
                {
                    path += ".png";
                }
                else
                {
                    path += "_" + (index + 1).ToString() + ".png";
                }

                Texture2D asset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                Color[] colors = asset.GetPixels(0);

                for (int i = 0; i < EmojiSize; i++)
                {
                    for (int j = 0; j < EmojiSize; j++)
                    {
                        newTex.SetPixel(x + i, y + j, colors[i + j * EmojiSize]);
                    }
                }

                string t = System.Convert.ToString(sourceDic[key] - 1, 2);
                float r = 0, g = 0, b = 0;
                if (t.Length >= 3)
                {
                    r = t[2] == '1' ? 0.5f : 0;
                    g = t[1] == '1' ? 0.5f : 0;
                    b = t[0] == '1' ? 0.5f : 0;
                }
                else if (t.Length >= 2)
                {
                    r = t[1] == '1' ? 0.5f : 0;
                    g = t[0] == '1' ? 0.5f : 0;
                }
                else
                {
                    r = t[0] == '1' ? 0.5f : 0;
                }

                dataTex.SetPixel(x / EmojiSize, y / EmojiSize, new Color(r, g, b, 1));

                if (!emojiDic.ContainsKey(key))
                {
                    EmojiInfo info;
                    // if (keyindex < keylist.Count)
                    // {
                    // 	info.key = "[" + char.ToString(keylist[keyindex]) + "]";
                    // }else
                    // {
                    // 	info.key = "[" + char.ToString(keylist[keyindex / keylist.Count]) + char.ToString(keylist[keyindex % keylist.Count]) + "]";
                    // }
                    info.key = "[" + keyindex + "]";
                    info.x = (x * 1.0f / texSize.x).ToString();
                    info.y = (y * 1.0f / texSize.y).ToString();
                    info.size = (EmojiSize * 1.0f / texSize.x).ToString();

                    emojiDic.Add(key, info);
                    keyindex++;
                }

                x += EmojiSize;
                if (x >= texSize.x)
                {
                    x = 0;
                    y += EmojiSize;
                }
            }
        }

        byte[] bytes1 = newTex.EncodeToPNG();
        string outputfile1 = OutputPath + "/emoji_tex.png";
        File.WriteAllBytes(outputfile1, bytes1);

        byte[] bytes2 = dataTex.EncodeToPNG();
        string outputfile2 = OutputPath + "/emoji_data.png";
        File.WriteAllBytes(outputfile2, bytes2);

        using (StreamWriter sw = new StreamWriter(OutputPath + "/emoji.txt", false))
        {
            sw.WriteLine("Name\tKey\tFrames\tX\tY\tSize");
            foreach (string key in emojiDic.Keys)
            {
                sw.WriteLine("{" + key + "}\t" + emojiDic[key].key + "\t" + sourceDic[key] + "\t" + emojiDic[key].x + "\t" + emojiDic[key].y + "\t" + emojiDic[key].size);
            }
            sw.Close();
        }


        if (!Directory.Exists(Application.dataPath + "/Resources"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources");
        }
        File.Copy(OutputPath + "/emoji.txt", "Assets/Resources/emoji.txt", true);

        AssetDatabase.Refresh();
        FormatTexture();

        EditorUtility.DisplayDialog("Success", "Generate Emoji Successful!", "OK");

    }


    private static Vector2 ComputeAtlasSize(int count)
    {
        long total = count * EmojiSize * EmojiSize;
        for (int i = 0; i < AtlasSize.Length; i++)
        {
            if (total <= AtlasSize[i].x * AtlasSize[i].y)
            {
                return AtlasSize[i];
            }
        }
        return Vector2.zero;
    }

    private static void FormatTexture()
    {
        string tmp = OutputPath.Replace(Application.dataPath, "");
        // string path = "Assets" + tmp + "/" + "emoji_tex.png";

        TextureImporter emojiTex = AssetImporter.GetAtPath("Assets" + tmp + "/" + "emoji_tex.png") as TextureImporter;
        emojiTex.filterMode = FilterMode.Point;
        emojiTex.mipmapEnabled = false;
        emojiTex.sRGBTexture = true;
        emojiTex.alphaSource = TextureImporterAlphaSource.FromInput;
        emojiTex.textureCompression = TextureImporterCompression.Uncompressed;
        emojiTex.SaveAndReimport();

        TextureImporter emojiData = AssetImporter.GetAtPath("Assets" + tmp + "/" + "emoji_data.png") as TextureImporter;
        emojiData.filterMode = FilterMode.Point;
        emojiData.mipmapEnabled = false;
        emojiData.sRGBTexture = false;
        emojiData.alphaSource = TextureImporterAlphaSource.None;
        emojiData.textureCompression = TextureImporterCompression.Uncompressed;
        emojiData.SaveAndReimport();
    }


    public partial class UnityEditorTools
    {
        [MenuItem("UnityFramework/Build Emoji")]
        public static void BuildEmoji()
        {

            EmojiBuilder window = EditorWindow.GetWindow<EmojiBuilder>();//获取指定类型的窗口.
            window.titleContent = new GUIContent("Build Emoji");
            window.maxSize = new Vector2(500, 600);
            window.minSize = new Vector2(500, 600);
            window.Show();




            //  CreateData();



        }
    }




}
