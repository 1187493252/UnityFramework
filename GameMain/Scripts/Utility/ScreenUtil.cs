using System;
using System.Collections;
using System.IO;
using System.Threading;
using UnityEngine;


/// <summary>
/// 截屏工具
/// </summary>
public static class ScreenUtil
{
    /// <summary>
    /// 截图(全屏)
    /// </summary>
    /// <param name="path">保存路径</param>
    /// <returns></returns>
    public static void FullScreenShot(string path)
    {
        ScreenCapture.CaptureScreenshot(path);
    }

    /// <summary>
    /// 自定义截图
    /// </summary>
    /// <param name="capX"></param>
    /// <param name="capY"></param>
    /// <param name="capWidth"></param>
    /// <param name="capHeight"></param>
    /// <param name="data"></param>
    public static IEnumerator ScreenShot(float capX, float capY, int capWidth, int capHeight, Action<byte[]> data)
    {
        yield return new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(capWidth, capHeight, TextureFormat.RGB24, true);
        tex.ReadPixels(new Rect(capX, capY, capWidth, capHeight), 0, 0, false);
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        data?.Invoke(bytes);
    }

    /// <summary>
    /// 从摄像机截图,如果Canvas没有指定相机,截图则没有UI
    /// </summary>
    /// <param name="camera">摄像机</param>
    /// <param name="rect">大小 new Rect(原点x, 原点y, 宽, 高)</param>
    /// <returns></returns>
    public static Texture2D CaptureCamera(Camera camera, Rect rect, Action<byte[]> data = null)
    {
        // 建一个RenderTexture对象  
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 1);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        camera.targetTexture = rt;
        camera.Render();

        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();

        // 重置相关参数，以使用camera继续在屏幕上显示  
        camera.targetTexture = null;
        //ps: camera2.targetTexture = null;  
        RenderTexture.active = null; // JC: added to avoid errors  
        GameObject.Destroy(rt);
        // 最后将这些纹理数据，成一个png图片文件  
        byte[] bytes = screenShot.EncodeToPNG();

        data?.Invoke(bytes);

        return screenShot;
    }
}

