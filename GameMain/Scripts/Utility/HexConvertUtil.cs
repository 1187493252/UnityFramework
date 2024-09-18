
using System;


/// <summary>
/// 进制转换工具
/// </summary>
public static class HexConvertUtil
{
    /// <summary>
    /// 十进制转化为i进制
    /// </summary>
    /// <param name="T"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public static string Hex_decimal_i(int T, int i)
    {
        string str = Convert.ToString(T, i);
        return str;
    }

    /// <summary>
    ///  i进制转换为十进制
    /// </summary>
    /// <param name="T"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public static int Hex_decimal_i(string T, int i)
    {
        int str = Convert.ToInt32(T, i);
        return str;
    }
}

