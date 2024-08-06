/*
* FileName:          Seriallize
* CompanyName:       
* Author:            relly
* Description:       
*/

using K_UnityGF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Seriallize
{

    /// <summary>
    /// json 序列化
    /// </summary>
    /// <param name="sender">序列化对象</param>
    /// <param name="isEncoding">转码</param>
    /// <returns></returns>
    public static string JsonSeriallize(object sender, bool isEncoding = false)
    {
        return JsonHelper.ToJson(sender, isEncoding);
    }

    /// <summary>
    /// json 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化类型</typeparam>
    /// <param name="json">json字符串</param>
    /// <returns></returns>
    public static T JsonDeserialization<T>(string json)
    {
        return JsonHelper.ToObject<T>(json);
    }

    /// <summary>
    /// xml序列化
    /// </summary>
    /// <typeparam name="T">序列化类型</typeparam>
    /// <param name="sender">序列化对象</param>
    /// <returns></returns>
    public static string XmlSeriallize<T>(object sender)
    {
        return XmlHelper.ToXml<T>(sender);
    }

    /// <summary>
    /// xml 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化类型</typeparam>
    /// <param name="xmlStr">xml字符串</param>
    /// <returns></returns>
    public static T XmlDeserialization<T>(string xmlStr)
    {
        return XmlHelper.ToObject<T>(xmlStr);
    }

    /// <summary>
    /// 二进制 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化类型</typeparam>
    /// <param name="stream">流</param>
    /// <returns></returns>
    public static T BinaryDeserialization<T>(Stream stream)
    {
        return BinaryHelper.Deserialize<T>(stream);
    }
}
