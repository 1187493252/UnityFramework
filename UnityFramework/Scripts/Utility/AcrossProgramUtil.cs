using System;
using System.Collections.Generic;
using System.Diagnostics;


/// <summary>
/// 跨程序工具
/// </summary>
public static class AcrossProgramUtil
{
    /// <summary>
    /// 跨程序 传入参数
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="parameter">参数</param>
    public static void TransferParameter(string filePath, string parameter)
    {
        Process process = new Process();
        process.StartInfo.FileName = filePath;
        process.StartInfo.Arguments = parameter;
        process.StartInfo.UseShellExecute = true;
        process.Start();
    }

    /// <summary>
    /// 跨程序 获取参数列表
    /// </summary>
    /// <returns></returns>
    public static List<string> ReceiveParameters()
    {
        List<string> commandLineArgs = new List<string>(Environment.GetCommandLineArgs());
        commandLineArgs.RemoveAt(0);
        return commandLineArgs;
    }

    /// <summary>
    /// 获取参数
    /// </summary>
    /// <returns></returns>
    public static string ReceiveParameter()
    {
        List<string> commandLineArgs = new List<string>(Environment.GetCommandLineArgs());
        return commandLineArgs[1];
    }
}

