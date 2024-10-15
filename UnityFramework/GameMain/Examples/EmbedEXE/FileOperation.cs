/*
* FileName:          FileOperate
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

#if UNITY_EDITOR||UNITY_STANDALONE

using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityFramework;
public class FileOperation
{


    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] FileDialog ofn);

    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] FileDialog ofd);

    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    private static extern int MessageBox(IntPtr handle, string message, string title, int type);

    /// <summary>
    /// 获取文件类型
    /// </summary>
    /// <param name="fileType">文件类型</param>
    /// <returns></returns>
    static string GetFilter(FileType fileType)
    {
        string filter = "";
        if (fileType == FileType.all)
        {
            filter = "All Files()\0*.*\0\0";
        }
        else
        {
            filter = $"{fileType}文件(*.{fileType})\0*.{fileType}";
        }

        return filter;
    }



    public static FileDialog GetOpenFileFullPath(FileType fileType = FileType.all, Action<FileType, object> callBack = null)
    {
        FileDialog ofn = new FileDialog();
        ofn.structSize = System.Runtime.InteropServices.Marshal.SizeOf(ofn);
        // ofn.filter = "All Files()\0*.*\0\0";//所有文件
        ofn.filter = GetFilter(fileType);

        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        //  ofn.initialDir = Application.streamingAssetsPath;
        ofn.title = "选择文件";
        //  ofn.defExt = "jpg";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008; // 0x00000200多选 
        if (GetOpenFileName(ofn))
        {
            FileType type;
            if (fileType != FileType.all)
            {
                type = fileType;
            }
            else
            {
                type = (FileType)Enum.Parse(typeof(FileType), Path.GetExtension(ofn.file).Split('.')[1]);
            }

            callBack?.Invoke(type, ofn.file);
        }
        return ofn;
    }


    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="fileType">文件类型</param>
    /// <param name="callBack">回调</param>
    /// <returns></returns>
    public static void GetSaveFileFullPath(FileType fileType = FileType.all, Action<string> callBack = null)
    {
        FileDialog ofn = new FileDialog();
        ofn.structSize = System.Runtime.InteropServices.Marshal.SizeOf(ofn);
        // ofn.filter = "All Files()\0*.*\0\0";//所有文件
        ofn.filter = GetFilter(fileType);

        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.title = "保存文件";

        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetSaveFileName(ofn))
        {
            string filePath = "";

            if (string.IsNullOrEmpty(Path.GetExtension(ofn.file)))
            {
                filePath = $"{ofn.file}.{fileType}";
            }
            else
            {
                filePath = ofn.file;
            }

            callBack?.Invoke(filePath);
        }
    }


    /// <summary>
    /// 提示框
    /// </summary>
    /// <param name="message">提示信息</param>
    /// <param name="title">标题</param>
    public static int MessgeBox(string message, string title, int style = 0)
    {
        return MessageBox(IntPtr.Zero, message, title, style);
    }
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class FileDialog
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public string filter = null;
    public string custonFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public string file = null;//路径
    public int maxFile = 0;
    public string fileTitle = null;//文件名带后缀
    public int maxFileTitle = 0;
    public string initialDir = null;
    public string title = null;
    public int flags = 0;
    public short fileOffect = 0;
    public short fileExtension = 0;
    public string defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public string templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}


#endif