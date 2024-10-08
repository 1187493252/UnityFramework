using System.Text.RegularExpressions;

namespace ZRKFramework
{
    /// <summary>
    /// 字符串规则 拓展
    /// </summary>
    public static class StringRegulationExtension
    {
        /// <summary>
        /// 字符串规则限制
        /// </summary>
        /// <param name="content">需要限制的字符串</param>
        /// <param name="regulation">规则</param>
        /// <returns></returns>
        public static string RegulationLimit(this string content,string regulation)
        {
            if (regulation.Equals(StringRegulation.NoRegulation))
            {
                return content;
            }
            return Regex.Replace(content, regulation, "");
        }
    }

    /// <summary>
    /// 结构体 字符串规则 正则表达式
    /// </summary>
    public struct StringRegulation
    {
        /// <summary>
        /// 无规则
        /// </summary>
        public const string NoRegulation = "";

        /// <summary>
        /// 规则-纯数字
        /// </summary>
        public const string Regulation1 = @"[^0-9]";

        /// <summary>
        /// 规则-大小写字母+数字结合
        /// </summary>
        public const string Regulation2 = @"[^A-Za-z0-9]";
    }
}
