using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
//using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Drawing;
using static Tools.G;

namespace Tools
{
    public enum MonthCodes { F = 1, G, H, J, K, M, N, Q, U, V, X, Z }          // CME month codes
    public enum Interval { Minute = 60, Hour = 3600, Day = 86400 }             // IQFeed requires the Interval to be in seconds

    public static class GDate
    {
        // Translate MonthCode char into TT-style 3-character month string
        static public Dictionary<char, string> MonthCodeTranslate = new Dictionary<char, string>()
        {
            { 'F', "Jan" },
            { 'G', "Feb" },
            { 'H', "Mar" },
            { 'J', "Apr" },
            { 'K', "May" },
            { 'M', "Jun" },
            { 'N', "Jul" },
            { 'Q', "Aug" },
            { 'U', "Sep" },
            { 'V', "Oct" },
            { 'X', "Nov" },
            { 'Z', "Dec" }
        };

        // Given month code char (string like "F" or "F18")
        // Return the MonthCodes enum representing this month code
        public static MonthCodes GetMonthCode(string m)
        {
            return (MonthCodes)Enum.Parse(typeof(MonthCodes), m[0].ToString());
        }
        
        // Given string in "mYY" format representing a monthcode/year
        // Return a DateTime representing 1st of the month for the given monthcode/year
        public static DateTime GetDateTimeMYY(string mYY)
        {            
            int yy = int.Parse(mYY.Substring(1, 2));            
            string m = mYY.Substring(0, 1);
            var mc = GetMonthCode(m);
            return new DateTime(2000 + yy, (int) mc, 1);
        }

        // Given string in "YYYYMMDD" format
        // Return a DateTime representing the given date
        public static DateTime GetDateTimeYYYYMMDD(string yyyymmdd)
        {
            int yyyy = int.Parse(yyyymmdd.Substring(0, 4));
            int mm = int.Parse(yyyymmdd.Substring(4, 2));
            int dd = int.Parse(yyyymmdd.Substring(6, 2));
            return new DateTime(yyyy, mm, dd);
        }

        // Given a begin and end date (each in "YYYYMMDD" format)
        // Return a List of MYY MonthCode strings within the range of these two dates (inclusive)
        public static List<string> GetMonthCodeList(string beginYYYYMMDD, string endYYYYMMDD)
        {
            var result = new List<string>();
            var dt1 = GetDateTimeYYYYMMDD(beginYYYYMMDD);
            var dt2 = GetDateTimeYYYYMMDD(endYYYYMMDD);
            var mYY1 = GetmYY(dt1.Month, dt1.Year);
            var mYY2 = GetmYY(dt2.Month, dt2.Year);

            string mYY = mYY1;
            result.Add(mYY);
            while (mYY != mYY2)
            {
                mYY = AddMonths(mYY, 1);
                result.Add(mYY);
            }

            return result;
        }

        // Given a "mYY" monthcode and 2-digit year ("J16", "U16", "K17")
        // Return a corresponding TT month/year combination ("Apr16", "Sep16", "May17")
        public static string GetTTMonthYear(string mYY)
        {
            char m = mYY[0];
            string YY = mYY.Substring(1, 2);
            string month = MonthCodeTranslate[m];
            return month + YY;
        }

        // Given an IQFeed Future Symbol (that ends in 'mYY' monthcode and 2-digit year)
        // Return only the 'mYY' from the rightmost portion of the string. (Return null if symbol does not end with 'mYY'.)
        public static string GetMYY(string IQSymbol)
        {
            int len = IQSymbol.Length;
            string mYY = IQSymbol.Substring(len - 3);
            if (MonthCodeTranslate.Keys.Contains(mYY[0]) && char.IsDigit(mYY[1]) && char.IsDigit(mYY[2]))
                return mYY;
            else
                return null;
        }

        public static DateTime DateTimeUnixEpochStart { get { return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); }}

        // Given a (nullable) DateTime
        // Return the (long) timestamp in milliseconds from 1/1/1970
        private static long GetTimestamp(DateTime? dt=null)
        {
            //var dateTime = DateTime.Now;    // new DateTime(2015, 05, 24, 10, 2, 0, DateTimeKind.Local);
            DateTime dateTime;
            if (dt == null)
                dateTime = DateTime.Now;
            else
                dateTime = dt.Value;
            //var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var epoch = DateTimeUnixEpochStart;
            var unixDateTime = (dateTime.ToUniversalTime() - epoch).TotalMilliseconds;
            return (long)unixDateTime;
            //string timestamp = ((long)unixDateTime).ToString();
            //return timestamp;
        }

        // Get DateTime from Unix timestamp (double)
        // uses milliseconds
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            //System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            var dtDateTime = DateTimeUnixEpochStart;
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        // Get DateTime from Unix timestamp (long)
        // uses milliseconds
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            //System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            var dtDateTime = DateTimeUnixEpochStart;
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        // Get DateTime from Unix timestamp (long)
        // uses seconds
        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            //System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            var dtDateTime = DateTimeUnixEpochStart;
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        // Get Unix timestamp from DateTime
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            //return (TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) - DateTimeUnixEpochStart).TotalSeconds;
        }

        // Get Unix timestamp from DateTime (functions like DateTimeOFfset.ToUnixTimeMilliseconds in Framework 4.6+)
        public static System.Int64 DateTimeToUnixTimeMilliseconds(DateTime dateTime)
        {
            //return (TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
            var ms = (TimeZoneInfo.ConvertTimeToUtc(dateTime) - DateTimeUnixEpochStart).TotalMilliseconds;
            return (System.Int64) ms;
        }

        // Get DateTime from Java timestamp (which is different because the timestamp is in milliseconds, not seconds)
        public static DateTime JavaTimeStampToDateTime(double javaTimeStamp)
        {
            // Java timestamp is milliseconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(javaTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static bool IsFutureSymbol(string symbol)
        {
            if (symbol == null) return false;

            int n = symbol.Length;
            string mYY = symbol.Substring(n - 3);
            return (GetMonth(mYY) != -1) && (GetYear(mYY) != -1);
        }

        public static string GetmYY(int m, int y)
        {
            string mc = ((MonthCodes)m).ToString();
            string ystr = y.ToString();
            int len = ystr.Length;
            return mc + ystr.Substring(len - 2);
        }

        // Return integer year (4-digit) from a given "mYY" string or -1 if not valid "mYY" format
        public static int GetYear(string mYY)
        {
            string yearStr = mYY.Substring(1, 2);
            int y;
            if (int.TryParse(yearStr, out y))
                return 2000 + y;
            else
                return -1;
        }

        // Return the integer month (1-12) from a given "mYY" string or -1 if not valid "mYY" format
        public static int GetMonth(string mYY)
        {
            string monthCode = mYY.Substring(0, 1);
            //int m = (int) Enum.Parse(typeof(MonthCodes), monthCode);
            MonthCodes mc;
            if (Enum.TryParse<MonthCodes>(monthCode, out mc))
                return (int)mc;
            else
                return -1;
            //var allValues = (MonthCodes[]) Enum.GetValues(typeof(MonthCodes));
            //var array = allValues.Select(value => new object[] { value, value.ToString() }).ToArray();
            //return m;
        }

        public static string GetIQDate(DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }

        public static string GetIQDateTime(DateTime dt)
        {
            return dt.ToString("yyyyMMdd HHmmss");
        }

        // Given integer month (1-12) and year (like YYYY) and number of months to add (negative to subtract months)
        // Return the resulting month/year in "mYY" format (like "H17", "Q18", "Z18", ...)
        public static string AddMonths(int m, int y, int monthCount)
        {
            int m2 = (m - 1) + monthCount;
            int addYears = (int)Math.Floor(m2 / 12.0);
            m2 = m2 % 12;
            if (m2 < 0)
                return GetmYY(13 + m2, y + addYears);
            else
                return GetmYY(m2 + 1, y + addYears);
        }

        // Given a month/year in "mYY" format and number of months to add (negative to subtract months)
        // Return the resulting month/year in "mYY" format (like "H17", "Q18", "Z18", ...)
        public static string AddMonths(string mYY, int monthCount)
        {
            return AddMonths(GetMonth(mYY), GetYear(mYY), monthCount);
        }

    } // end of class

} // end of namespace
