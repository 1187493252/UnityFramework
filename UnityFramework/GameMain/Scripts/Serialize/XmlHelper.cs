using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


/// <summary>
/// xml 解析助手
/// </summary>
public static class XmlHelper
{
    /// <summary>
    /// Xml 序列化
    /// </summary>
    /// <param name="arg">参数：对象</param>
    /// <returns></returns>
    public static string ToXml<T>(object arg)
    {
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(T));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, arg);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        string xmlStr = Encoding.UTF8.GetString(memoryStream.ToArray());
        return xmlStr;
    }

    /// <summary>
    /// Xml 反序列化
    /// </summary>
    /// <typeparam name="T">转化类型</typeparam>
    /// <param name="xmlText">Xml文本内容</param>
    /// <returns></returns>
    public static T ToObject<T>(string xmlText)
    {
        Stream stream = StreamUtil.BytesToStream(Encoding.UTF8.GetBytes(xmlText));
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        T type = (T)serializer.Deserialize(stream);
        return type;
    }
}

