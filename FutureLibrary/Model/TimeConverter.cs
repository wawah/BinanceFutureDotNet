using System;
using System.Collections.Generic;
using System.Text;

namespace FutureLibrary.Model
{
    static public class TimeConverter
    {
        static public DateTime hbTimeConvertor(long hb_ts)
        {
            var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Local);
            var utc_time = posixTime.AddMilliseconds(hb_ts);
            return utc_time;
        }
        static public DateTime hbTimeConvertorToCN(long hb_ts)
        {
            var posixTime = DateTime.SpecifyKind(TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)), DateTimeKind.Local);
            var cn_time = posixTime.AddMilliseconds(hb_ts);
            return cn_time;
        }

        static public DateTime okTimeConvertor(string ok_ts)
        {
            return DateTime.Parse(ok_ts, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
        static public DateTime hb_CandleToLocalTime(long unixsecondformat)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            DateTime dt = startTime.AddSeconds(unixsecondformat);
            return dt;
        }
        static public DateTime hb_CandleToUTCTime(long unixsecondformat)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            DateTime dt = startTime.AddSeconds(unixsecondformat);
            return dt.ToUniversalTime();
        }

        static public DateTime kc_UnixmsToDT(string unix_ms)
        {
            return hbTimeConvertor(Convert.ToInt64(unix_ms));
        }

        static public DateTime kc_UnixmsToDT_CN(string unix_ms)
        {
            return hbTimeConvertorToCN(Convert.ToInt64(unix_ms));
        }

        static public double kc_dtToUnixms(DateTime dt)
        {
            return dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
