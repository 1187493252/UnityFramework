/*
* FileName:          Global
* CompanyName:       
* Author:            
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public partial class Global
{
    public class Network
    {
        public static string NetwokingID;// 模式
        public static string Authentication;//登录验证

        public static string IP_Local;//本机ip

        public static string BGIP;//后台ip,上传数据

        public static string ManagerIP;//管理端



        public static string ExternalLink1;//外部链接
        public static string ExternalLink2;//外部链接
        public static string ExternalLink3;//外部链接


        public static string TokenName = "token";
        public static string TokenValue;

        public static string LoginUrl;//登录
        public static string ChangePasswordUrl;//修改密码
        public static string EnrollUrl;//注册




        //--------------------------------------


        public static string WebUrlDecode(string content)
        {
            return WebUtility.UrlDecode(content);
        }
        public static string WebUrlEncode(string content)
        {
            return WebUtility.UrlEncode(content);
        }

        //获取16位安全验证随机数
        public static string GetSecurityCode()
        {
            List<string> verifyList = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            string temp = "";
            for (int i = 0; i < verifyList.Count; i++)
            {
                int index = UnityEngine.Random.Range(0, 16);
                temp += verifyList[index];
            }
            return temp;
        }

    }

}
