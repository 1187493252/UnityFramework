using System.Diagnostics;
using UnityEngine;


/// <summary>
/// 系统工具
/// </summary>
public static class SystemUtil
{
    static TextEditor textEditor;
    public static void TextEditorCopy(string content)
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = content;
        textEditor.OnFocus();
        textEditor.SelectAll();
        textEditor.Copy();
    }

    public static string TextEditorPaste()
    {
        return textEditor.text;
    }

    public static void SystemCopy(string content)
    {
        GUIUtility.systemCopyBuffer = content;
    }

    public static string SystemPaste()
    {
        return GUIUtility.systemCopyBuffer;
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public static void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    /// <summary>
    /// 进程级重启
    /// </summary>
    public static void Restart()
    {
        Process p = new Process();
        p.StartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
        p.StartInfo.UseShellExecute = false;
        p.Start();
        Process.GetCurrentProcess().Kill();
    }
}

