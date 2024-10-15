using System;
using System.IO;
using UnityEngine;


/// <summary>
/// Sprite 工具类
/// </summary>
public static class SpriteUtil
{
    /// <summary>
    /// 从本地读取文件 返回Sprite
    /// </summary>
    /// <param name="filePath">图片路径</param>
    /// <param name="textureWidth">图片宽度</param>
    /// <param name="textureHeight">图片高度</param>
    /// <returns></returns>
    public static Sprite GetSpriteByPath(string filePath, int textureWidth, int textureHeight)
    {
        //读取路径下文件
        StreamReader reader = new StreamReader(filePath);
        //获取字节流
        MemoryStream ms = new MemoryStream();
        reader.BaseStream.CopyTo(ms);
        byte[] bytes = ms.ToArray();

        Sprite sprite = ConverSpriteByData(bytes, textureWidth, textureHeight);

        reader.Close();

        return sprite;
    }

    /// <summary>
    /// 字节转化为精灵图片
    /// </summary>
    /// <param name="datas"></param>
    /// <returns></returns>
    public static Sprite ConverSpriteByData(byte[] datas, int textureWidth, int textureHeight)
    {

        //定义Textrue2D并转化为精灵图片          
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        texture.LoadImage(datas);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);

        return sprite;
    }

    /// <summary>
    /// 64位字符串转化为精灵图片
    /// </summary>
    /// <param name="spriteBase64Str"></param>
    /// <returns></returns>
    public static Sprite GetSpriteBy64Str(string spriteBase64Str, int textureWidth, int textureHeight)
    {
        byte[] bytes = Convert.FromBase64String(spriteBase64Str);
        Sprite sprite = ConverSpriteByData(bytes, textureWidth, textureHeight);
        return sprite;
    }
}

