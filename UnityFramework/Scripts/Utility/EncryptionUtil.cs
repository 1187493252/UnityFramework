using System;
using System.Security.Cryptography;
using System.Text;


/// <summary>
/// 加密 解密工具
/// </summary>
public static class EncryptionUtil
{
    /// <summary>
    /// 加密  返回加密后的结果
    /// </summary>
    /// <param name="encryptContent">需要加密的数据内容</param>
    /// <param name="token">令牌</param>
    /// <returns></returns>
    public static string Encrypt(string encryptContent, string token)
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(EncryptMD5_16(token));
        RijndaelManaged rDel = new RijndaelManaged
        {
            Key = keyArray,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        byte[] toEncryptArray = Encoding.UTF8.GetBytes(encryptContent);
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    /// <summary>
    /// 解密  返回解密后的结果
    /// </summary>
    /// <param name="decryptContent">解密的数据内容</param>
    /// <param name="token">令牌</param>
    /// <returns></returns>
    public static string Decrypt(string decryptContent, string token)
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(EncryptMD5_16(token));

        RijndaelManaged rDel = new RijndaelManaged
        {
            Key = keyArray,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        ICryptoTransform cTransform = rDel.CreateDecryptor();

        byte[] toEncryptArray = Convert.FromBase64String(decryptContent);
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Encoding.UTF8.GetString(resultArray);
    }

    /// <summary>
    /// MD5 16位加密
    /// </summary>
    /// <param name="encryptContent">需要加密的内容</param>
    /// <returns></returns>
    public static string EncryptMD5_16(string encryptContent)
    {
        var md5 = new MD5CryptoServiceProvider();
        string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(encryptContent)), 4, 8);
        t2 = t2.Replace("-", "");
        return t2;
    }

    /// <summary>
    /// MD5　32位加密
    /// </summary>
    /// <param name="encryptContent">需要加密的内容</param>
    /// <returns></returns>
    public static string EncryptMD5_32(string encryptContent)
    {
        string content_Normal = encryptContent;
        string content_Encrypt = "";
        MD5 md5 = MD5.Create();

        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(content_Normal));

        for (int i = 0; i < s.Length; i++)
        {
            content_Encrypt = content_Encrypt + s[i].ToString("X2");
        }
        return content_Encrypt;
    }

    /// <summary>
    /// MD5 64位加密
    /// </summary>
    /// <param name="encryptContent">需要加密的内容</param>
    /// <returns></returns>
    public static string EncryptMD5_64(string encryptContent)
    {
        string content = encryptContent;
        MD5 md5 = MD5.Create();
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
        return Convert.ToBase64String(s);
    }

    /// <summary>
    /// 加密算法-HmacSHA256
    /// </summary>
    /// <param name="message"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    public static string HmacSHA256(string message, string secret)
    {
        secret = secret ?? "";
        var encoding = new System.Text.ASCIIEncoding();
        byte[] keyByte = encoding.GetBytes(secret);
        byte[] messageBytes = encoding.GetBytes(message);
        using (var hmacsha256 = new HMACSHA256(keyByte))
        {
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashmessage);
        }
    }

    /// <summary>
    /// 加密算法-SHA256
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string SHA256(string str)
    {
        byte[] SHA256Data = Encoding.UTF8.GetBytes(str);

        SHA256Managed Sha256 = new SHA256Managed();
        byte[] by = Sha256.ComputeHash(SHA256Data);

        return BitConverter.ToString(by).Replace("-", "").ToLower();
    }

    /// <summary>
    /// byte[] 转化为base64位字符串
    /// </summary>
    /// <param name="datas">字节数组</param>
    /// <returns></returns>
    public static string BytesToBase64(byte[] datas)
    {
        return Convert.ToBase64String(datas);
    }

    /// <summary>
    /// Base64位字符串转化为字节数组
    /// </summary>
    /// <param name="base64Str"></param>
    /// <returns></returns>
    public static byte[] Base64ToBytes(string base64Str)
    {
        return Convert.FromBase64String(base64Str);
    }
}

