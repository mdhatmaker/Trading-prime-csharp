using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Drawing;
using static Tools.G;

namespace Tools
{
    public static class GExtensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        // Convert a DateTime to string in "YYYYMMDD" format
        public static string ToYYYYMMDD(this DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }

        // Add a more descriptive name for the existing ToIQString method (yyMMddHHmmss)
        public static string ToCompactDateTime(this DateTime dt)
        {
            return dt.ToIQString();
        }

        // IQFeed uses this compact format of datetime (2-digit year and no separators: yyMMddHHmmss)
        public static string ToIQString(this DateTime dt)
        {
            return dt.ToString("yyMMddHHmmss");
        }

        // Return datetime in YYYY-MM-DD format
        public static string ToSortableDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        // Return datetime in YYYY-MM-DD HH:MM:SS format
        public static string ToSortableDateTime(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static long ToUnixTimestamp(this DateTime dt)
        {
            return (long)GDate.DateTimeToUnixTimestamp(dt);
        }

        /*// Code similar to the following could be used to handle either SECONDS or MILLISECONDS
        static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        static readonly double MaxUnixSeconds = (DateTime.MaxValue - UnixEpoch).TotalSeconds;

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
           return unixTimeStamp > MaxUnixSeconds
              ? UnixEpoch.AddMilliseconds(unixTimeStamp)
              : UnixEpoch.AddSeconds(unixTimeStamp);
        }*/

        /*// Shortcut for ToUnixTimeSeconds
        public static long ToUnixTime(this DateTimeOffset dto)
        {
            return dto.ToUnixTimeSeconds();
        }

        // Shortcut for FromUnixTimeSeconds
        public static DateTimeOffset FromUnixTime(this long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime);
        }*/

        public static string ToBase64(this string str)
        {
            return Base64Encode(str);
        }

        public static byte[] ToBase64Bytes(this string str)
        {
            byte[] ret = System.Text.Encoding.Unicode.GetBytes(str);
            string s = Convert.ToBase64String(ret);
            ret = System.Text.Encoding.Unicode.GetBytes(s);
            return ret;
        }

        /*public static string ToBase64Bytes(this string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }*/

        // Given dataframe selected column string like "filename.DF.csv::ColumnName"
        // Return the filename portion of the string
        public static string DfFilename(this string str)
        {
            int i = str.IndexOf("::");
            if (i <= 0) return "";

            return str.Substring(0, i);
        }

        // Given dataframe selected column string like "filename.DF.csv::ColumnName"
        // Return the filename portion of the string
        public static string DfColumn(this string str)
        {
            int i = str.IndexOf("::");
            if ((i < 0) || (i+2 >= str.Length)) return "";

            return str.Substring(i + 2);
        }

        // Given a Unix timestamp (as an int)
        // Return a DateTime
        public static DateTime ToDateTime(this int x)
        {
            return GDate.UnixTimeStampToDateTime(x);
        }

        // Given a Unix timestamp (as an int)
        // Return a formatted DateTime string
        public static string ToDateTimeString(this int x)
        {
            return GDate.UnixTimeStampToDateTime(x).ToString("yyyy-MM-dd HH:mm:ss");
        }

        // Given a Unix timestamp (as a long)
        // Return a DateTime
        public static DateTime ToDateTime(this long x)
        {
            return GDate.UnixTimeStampToDateTime(x);
        }

        // Given a Unix timestamp (as a long)
        // Return a formatted DateTime string
        public static string ToDateTimeString(this long x)
        {
            return GDate.UnixTimeStampToDateTime(x).ToString("yyyy-MM-dd HH:mm:ss");
        }

        // Given a DateTime
        // Return a formatted DateTime string
        public static string ToDateTimeString(this DateTime x)
        {
            return x.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static byte[] HexStringToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string ByteArrayToHexString(this byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

    } // end of class
} // end of namespace