/*
* FileName:          TestEmbedExe 
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
#if !UNITY_WEBGL

public class TestEmbedExe : MonoBehaviour
{
    #region Win32 Api
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInserAfter, int x, int y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, long dwNewLong);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string className, string windowName);

    [DllImport("user32.dll")]
    private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    [DllImport("user32.dll")]
    static extern IntPtr GetDesktopWindow();
    #endregion

    FileDialog fileInfo;
    Process process;

    private void Start()
    {
        //Screen.SetResolution(1600, 900, false);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Open", GUILayout.Width(200), GUILayout.Height(50)))
        {
            fileInfo = FileOperation.GetOpenFileFullPath();
            if (fileInfo != null)
            {
                if (process != null)
                {
                    if (!process.HasExited)
                        process.Kill();
                    process = null;
                }
                ProcessStartInfo info = new ProcessStartInfo(fileInfo.file);
                process = Process.Start(info);
                process.WaitForInputIdle();
                StartCoroutine(Wait());
                //IntPtr win = FindWindow(null, process.ProcessName);
                //SetWindowLong(win, -16, 1);
                //SetParent(win, GetForegroundWindow());
                //SetWindowPos(win, 0, 0, 0, 300, 200, 0x0040);
            }
        }
        if (GUILayout.Button("去边框", GUILayout.Width(200), GUILayout.Height(50)))
        {
            IntPtr win = FindWindow(null, process.ProcessName);
            SetWindowLong(win, -16, 1);
        }
    }

    IEnumerator Wait()
    {
        yield return 0;
        IntPtr win = FindWindow(null, process.ProcessName);
        SetParent(win, GetForegroundWindow());
        yield return new WaitForSeconds(0.5f);
        SetWindowLong(win, -16, 0x00800000L);
        SetWindowPos(win, 0, 20, 20, 300, 200, 0x0040);
    }

    private void OnDestroy()
    {
        if (process != null)
        {
            if (!process.HasExited)
                process.Kill();
            process = null;
        }
    }
}



#endif