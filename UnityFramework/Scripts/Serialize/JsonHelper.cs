using LitJson;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;


/// <summary>
/// Json 解析助手
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// 切割读取的json数据
    /// 在读取本地文件时会多出三个字节
    /// </summary>
    /// <param name="content">需要切割的json字符串</param>
    /// <returns></returns>
    public static string SplitJson(string content)
    {
        byte[] datas = System.Text.Encoding.UTF8.GetBytes(content);
        content = System.Text.Encoding.UTF8.GetString(datas, 3, datas.Length - 3);
        return content;
    }

    /// <summary>
    /// Json 反序列化
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="jsonContent">读取的Json字符串</param>
    /// <returns></returns>
    public static T ToObject<T>(string jsonContent)
    {
        return JsonMapper.ToObject<T>(jsonContent);
    }
    public static JsonData ToObject(string jsonContent)
    {
        return JsonMapper.ToObject(jsonContent);
    }
    /// <summary>
    /// Json 序列化
    /// </summary>
    /// <param name="arg">对象</param>
    /// <returns></returns>
    public static string ToJson(object arg, bool isEncoding = false)
    {
        string jsonStr;
        jsonStr = JsonMapper.ToJson(arg);
        if (isEncoding)
        {
            jsonStr = TransCoding(jsonStr);
        }
        return jsonStr;
    }

    public static T ToObjectByNewtonsoftJson<T>(string jsonContent)
    {
        return JsonConvert.DeserializeObject<T>(jsonContent);
    }

    public static string ToJsonByNewtonsoftJson(object arg)
    {
        string jsonStr;
        jsonStr = JsonConvert.SerializeObject(arg);
        return jsonStr;
    }

    /// <summary>
    /// Json字符串转码
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static string TransCoding(string json)
    {
        string content = "";
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        content = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        return content;
    }
}

