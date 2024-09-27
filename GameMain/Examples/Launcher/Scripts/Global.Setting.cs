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
    public class Setting
    {
        public static bool TimeLock;
        public static string TimeLockKey;

        public static bool ShowLogo;
        public static DateTime WebTime;//当前网络/系统时间
        public static DateTime LastPassVerificationTime;//上一次通过验证的时间


    }

}
