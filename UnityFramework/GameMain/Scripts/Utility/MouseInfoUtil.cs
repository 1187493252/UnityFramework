using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 鼠标信息工具类
/// </summary>
public static class MouseInfoUtil
{
    /// <summary>
    /// 获取鼠标X轴
    /// </summary>
    /// <returns></returns>
    public static float GetAixs_X() { return Input.GetAxis("Mouse X"); }

    /// <summary>
    /// 获取鼠标X轴
    /// </summary>
    /// <returns></returns>
    public static float GetAixs_Y() { return Input.GetAxis("Mouse Y"); }

    /// <summary>
    /// 自定义鼠标光标
    /// </summary>
    /// <param name="cursorTex"></param>
    /// <param name="hotsPot"></param>
    /// <param name="cursorMode"></param>
    public static void CustomCursor(Texture2D cursorTex, Vector2 hotsPot, CursorMode cursorMode = CursorMode.Auto)
    {
        Cursor.SetCursor(cursorTex, hotsPot, cursorMode);
    }
}
