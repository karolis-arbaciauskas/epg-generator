using System;
namespace awscsharp.Utils
{
    public static class DateTimeFormater
    {
        public static string TimestampToString(string timestamp)
        {
            double num = double.Parse(timestamp);

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(num).ToLocalTime();
            return dtDateTime.ToString("yyyyMMddHHmmss zzz").Replace(":", "");
        }
    }
}
