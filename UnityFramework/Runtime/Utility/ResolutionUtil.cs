using UnityEngine;


/// <summary>
/// 屏幕分辨率工具类
/// </summary>
public static class ResolutionUtil
{
    /// <summary>
    /// 分辨率 1024/768
    /// </summary>
    public static void Resolution_1024x768(bool fullScreen = false) { Screen.SetResolution(1024, 768, fullScreen); }

    /// <summary>
    /// 分辨率 1280/720
    /// </summary>
    public static void Resolution_1280x720(bool fullScreen = false) { Screen.SetResolution(1280, 720, fullScreen); }

    /// <summary>
    /// 分辨率 960/600
    /// </summary>
    public static void Resolution_960x600(bool fullScreen = false) { Screen.SetResolution(960, 600, fullScreen); }

    /// <summary>
    /// 分辨率 1902/1080
    /// </summary>
    public static void Resolution_1920x1080(bool fullScreen = false) { Screen.SetResolution(1920, 1080, fullScreen); }

    /// <summary>
    /// 自定义分辨率
    /// </summary>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <param name="fullScreen">是否全屏</param>
    public static void CustomResolution(int width, int height, bool fullScreen = false) { Screen.SetResolution(width, height, fullScreen); }
}
