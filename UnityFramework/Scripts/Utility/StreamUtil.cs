using System.IO;


/// <summary>
/// 文件流工具
/// </summary>
public static class StreamUtil
{
    /// <summary>
    /// 从本地读取文件 返回读取字符串
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetContentByPath(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);
        string content = reader.ReadToEnd();
        reader.Close();

        return content;
    }

    /// <summary>
    /// 从本地读取文件 返回字节数组
    /// </summary>
    /// <param name="_fielPath"></param>
    /// <returns></returns>
    public static byte[] GetDataByPath(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);
        //获取字节流
        MemoryStream ms = new MemoryStream();
        reader.BaseStream.CopyTo(ms);
        byte[] data = ms.ToArray();
        reader.Close();

        return data;
    }

    /// <summary>
    /// Stream 转 byte[]
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static byte[] StreamToBytes(Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        stream.Seek(0, SeekOrigin.Begin);
        return bytes;
    }

    /// <summary>
    /// byte[] 转Stream
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static Stream BytesToStream(byte[] bytes)
    {
        Stream stream = new MemoryStream(bytes);
        return stream;
    }
}

