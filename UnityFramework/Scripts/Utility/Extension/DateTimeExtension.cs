using System;

namespace K_UnityGF
{
    public static class DateTimeExtension
    {

        /// <summary>
        /// 获取国际标准化 日期
        /// </summary>
        /// <param name="dateTime">实例</param>
        /// <returns></returns>
        public static string GetISO8601Time(this DateTime dateTime)
        {
            return DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        /// <summary>
        /// 获取10位时间戳
        /// </summary>
        /// <param name="dateTime">实例</param>
        /// <returns></returns>
        public static long GetUTC10(this DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(timeSpan.TotalSeconds);
        }

        /// <summary>
        /// 获取13位时间戳
        /// </summary>
        /// <param name="dateTime">实例</param>
        /// <returns></returns>
        public static long GetUTC13(this DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(timeSpan.TotalMilliseconds);
        }

        /// <summary>
        /// 10位时间戳转化为时间
        /// </summary>
        /// <param name="utc10Time">10位时间戳字符串</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromUTC10(this string utc10Time)
        {
            DateTime dateTime = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1));
            long lTime = long.Parse(utc10Time + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTime.Add(toNow);
        }

        /// <summary>
        /// 13位时间戳转化为时间
        /// </summary>
        /// <param name="utc13Time">13位时间戳字符串</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromUTC13(this string utc13Time)
        {
            DateTime dateTime = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1));
            long longTime = long.Parse(utc13Time + "0000");
            TimeSpan toNow = new TimeSpan(longTime);
            return dateTime.Add(toNow);
        }

        /// <summary>
        /// 获取当前日期-星期几
        /// </summary>
        /// <param name="dateTime">实例</param>
        /// <returns></returns>
        public static string GetDayOfWeek(this DateTime dateTime)
        {
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: return "星期一";
                case DayOfWeek.Tuesday: return "星期二";
                case DayOfWeek.Wednesday: return "星期三";
                case DayOfWeek.Thursday: return "星期四";
                case DayOfWeek.Friday: return "星期五";
                case DayOfWeek.Saturday: return "星期六";
                case DayOfWeek.Sunday: return "星期日";
                default: return null;
            }
        }

        /// <summary>
        /// 转换格式 yyyy/MM/dd
        /// </summary>
        /// <param name="dateTime">实例</param>
        /// <returns>yyyy/MM/dd</returns>
        public static string GetDateA(this DateTime dateTime) { return dateTime.ToString("yyyy/MM/dd"); }

        /// <summary>
        /// 转换格式 yyyy-MM-dd
        /// </summary>
        /// <param name="dateTime">实例</param>
        /// <returns>yyyy-MM-dd</returns>
        public static string GetDateB(this DateTime dateTime) { return dateTime.ToString("yyyy-MM-dd"); }

        /// <summary>
        /// 转换格式 yyyy年MM月dd日
        /// </summary>
        /// <param name="dateTime">实例</param>
        /// <returns>yyyy年MM月dd日</returns>
        public static string GetDateC(this DateTime dateTime) { return dateTime.ToString("yyyy年MM月dd日"); }

        /// <summary>
        /// 转换格式 yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="dateTime">实例</param>
        /// <returns>yyyy/MM/dd HH:mm:ss</returns>
        public static string GetTimeA(this DateTime dateTime) { return dateTime.ToString("yyyy/MM/dd HH:mm:ss"); }

        /// <summary>
        /// 转换格式 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dateTime">实例</param>
        /// <returns>yyyy-MM-dd HH:mm:ss</returns>
        public static string GetTimeB(this DateTime dateTime) { return dateTime.ToString("yyyy-MM-dd HH:mm:ss"); }

        /// <summary>
        /// 转换格式 yyyy年MM月dd日 HH:mm:ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>yyyy年MM月dd日 HH:mm:ss</returns>
        public static string GetTimeC(this DateTime dateTime) { return dateTime.ToString("yyyy年MM月dd日 HH:mm:ss"); }

        /// <summary>
        /// 计算两时间差
        /// </summary>
        /// <param name="fromDate">实例</param>
        /// <param name="toDate">另一个时间实例</param>
        /// <returns></returns>
        public static TimeSpan GetTimeDuration(this DateTime fromDate, DateTime toDate)
        {
            TimeSpan span1 = new TimeSpan(fromDate.Ticks);
            TimeSpan span2 = new TimeSpan(toDate.Ticks);

            return span1.Subtract(span2).Duration();
        }

        /// <summary>
        /// 计算两时间差
        /// </summary>
        /// <param name="fromTime">实例</param>
        /// <param name="toTime">另一个时间戳实例</param>
        /// <returns></returns>
        public static long GetTimeDuration(this long fromTime, long toTime) { return Math.Abs(fromTime - toTime); }
    }
}
