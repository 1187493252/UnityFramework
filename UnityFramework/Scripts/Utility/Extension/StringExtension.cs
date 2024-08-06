/*
* FileName:          StringExtension
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace UnityFramework
{
    public static class StringExtension
    {



        public static string WebUrlDecode(this string content)
        {
            return WebUtility.UrlDecode(content);
        }
        public static string WebUrlEncode(this string content)
        {
            return WebUtility.UrlEncode(content);
        }

        /// <summary>
        /// 是否为日期型字符串
        /// </summary>
        /// <param name="StrSource">日期字符串(2008-05-08)</param>
        /// <returns></returns>
        public static bool IsDate(this string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }


        /// <summary>
        /// 是否为时间型字符串
        /// </summary>
        /// <param name="source">时间字符串(15:00:00)</param>
        /// <returns></returns>
        public static bool IsTime(this string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }


        /// <summary>
        /// 是否为日期+时间型字符串
        /// </summary>
        /// <param name="StrSource">格式:2023-06-18 06:06:06</param>
        /// <returns></returns>
        public static bool IsDateTime(this string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }


        /// <summary>
        /// 从指定字符串中的指定位置处开始读取一行。
        /// </summary>
        /// <param name="rawString">指定的字符串。</param>
        /// <param name="position">从指定位置处开始读取一行，读取后将返回下一行开始的位置。</param>
        /// <returns>读取的一行字符串。</returns>
        public static string ReadLine(this string rawString, ref int position)
        {
            if (position < 0)
            {
                return null;
            }

            int length = rawString.Length;
            int offset = position;
            while (offset < length)
            {
                char ch = rawString[offset];
                switch (ch)
                {
                    case '\r':
                    case '\n':
                        if (offset > position)
                        {
                            string line = rawString.Substring(position, offset - position);
                            position = offset + 1;
                            if ((ch == '\r') && (position < length) && (rawString[position] == '\n'))
                            {
                                position++;
                            }

                            return line;
                        }

                        offset++;
                        position++;
                        break;

                    default:
                        offset++;
                        break;
                }
            }

            if (offset > position)
            {
                string line = rawString.Substring(position, offset - position);
                position = offset;
                return line;
            }

            return null;
        }

        /// <summary>
        /// 字符串首字母大写,只转换第一个位置 例:my lsit =>My list
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ToUpperFirst(this string content)
        {
            //Regex.Replace(content, "^[a-z]", m => m.Value.ToUpper())
            return Regex.Replace(content, @"^\w", t => t.Value.ToUpper());
        }

        /// <summary>
        /// 字符串首字母大写,包含空格后的 例:my lsit =>My List
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ToUpperALLFirst(this string content)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(content.ToLower());
        }

        /// <summary>
        /// 执行dos命令   "del c:\\t1.txt".ExecuteDOS();
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public static string ExecuteDOS(this string cmd, out string error)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.WriteLine(cmd);
            process.StandardInput.WriteLine("exit");
            error = process.StandardError.ReadToEnd();
            return process.StandardOutput.ReadToEnd();
        }

        public static void DeleteFile(this string path)
        {
            if (File.Exists(path)) File.Delete(path);
        }

        public static void WriteAllText(this string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public static void CreateDirectory(this string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary> 
        /// 打开文件或网址 "c:\\t.txt".Open();  "http://www.baidu.com/".Open();
        /// </summary>
        /// <param name="s"></param>
        public static void Open(this string s)
        {
            Process.Start(s);
        }



        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }

        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }

        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null) return false;
            else return Regex.IsMatch(s, pattern);
        }

        public static string Match(this string s, string pattern)
        {
            if (s == null) return "";
            return Regex.Match(s, pattern).Value;
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// 转全角(SBC case)
        /// </summary>
        /// <param name="content"></param>
        public static string ToSBC(this string content)
        {
            char[] c = content.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 转半角(DBC case)
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ToDBC(this string content)
        {
            char[] c = content.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
    }
}