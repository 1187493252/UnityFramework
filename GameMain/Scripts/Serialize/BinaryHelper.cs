using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class BinaryHelper
{
    /// <summary>
    /// 保存二进制
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="binaryTarget">二进制对象</param>
    public static void SerializeToFile(string filePath, object binaryTarget)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(filePath);
        binaryFormatter.Serialize(fileStream, binaryTarget);
        fileStream.Close();
    }

    public static T DeserializeByFile<T>(string filePath)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(filePath);
        T temp = default;
        temp = (T)binaryFormatter.Deserialize(fileStream);
        fileStream.Close();
        return temp;
    }


    /// <summary>
    /// 二进制反序列化
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="stream">流</param>
    /// <returns></returns>
    public static T Deserialize<T>(Stream stream)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        return (T)binaryFormatter.Deserialize(stream);
    }

    public static MemoryStream Serialize(object binaryTarget)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(memoryStream, binaryTarget);
        return memoryStream;
    }
}

