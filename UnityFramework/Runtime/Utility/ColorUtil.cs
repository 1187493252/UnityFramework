using UnityEngine;


/// <summary>
/// Color 工具
/// </summary>
public static class ColorUtil
{
    /// <summary>
    /// 颜色转换 16进制转Color
    /// </summary>
    /// <param name="htmlString">颜色16进制</param>
    /// <returns></returns>
    public static Color HtmlStringToColor(string htmlString)
    {
        ColorUtility.TryParseHtmlString(htmlString, out Color color);

        return color;
    }

    /// <summary>
    /// 颜色转换 Color转16进制
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string ColorToHtmlString(Color color) { return ColorUtility.ToHtmlStringRGB(color); }

    /// <summary>
    /// 颜色转换为0-255
    /// </summary>
    /// <param name="T"></param>
    /// <returns></returns>
    public static Color32 Color_Change_0_255(Color T)
    {
        Color32 color = T;
        return T;
    }

    /// <summary>
    /// 0-255转换为颜色
    /// </summary>
    /// <param name="T"></param>
    /// <returns></returns>
    public static Color Color_Change_0_255(Color32 T)
    {
        Color color = T;
        return T;
    }
}

