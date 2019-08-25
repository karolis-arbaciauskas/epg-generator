using System;
namespace awscsharp.Utils
{
    public static class DateTimeFormater
    {
        public static string TimestampToString(int timestamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp);
            //TimeZoneInfo lithuaniaTimezone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Vilnius");
            //DateTime lithuaniaTime = TimeZoneInfo.ConvertTimeFromUtc(dtDateTime, lithuaniaTimezone);
            return dtDateTime.ToString("yyyyMMddHHmmss zzz").Replace(":", "");
        }
    }
}
