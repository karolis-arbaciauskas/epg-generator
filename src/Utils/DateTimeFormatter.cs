using System;

namespace EpgGenerator.Utils
{
    public static class DateTimeFormatter
    {
        public static string TimestampToString(int timestamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp);
            return dtDateTime.ToString("yyyyMMddHHmmss zzz").Replace(":", "");
        }
    }
}