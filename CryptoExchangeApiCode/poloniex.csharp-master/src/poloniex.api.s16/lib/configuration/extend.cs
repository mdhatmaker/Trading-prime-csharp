using OdinSdk.BaseLib.Configuration;
using System;
using System.Globalization;
using XCT.BaseLib.Types;

namespace XCT.BaseLib.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class TExtend 
    {
        private const int DoubleRoundingPrecisionDigits = 8;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Normalize(this decimal value)
        {
            return Math.Round(value, DoubleRoundingPrecisionDigits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(this ulong unixTimeStamp)
        {
            return CUnixTime.DateTimeUnixEpochStart.AddSeconds(unixTimeStamp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static ulong DateTimeToUnixTimeStamp(this DateTime dateTime)
        {
            return (ulong)Math.Floor(dateTime.Subtract(CUnixTime.DateTimeUnixEpochStart).TotalSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringNormalized(this OrderType value)
        {
            switch (value)
            {
                case OrderType.Buy:
                    return "buy";

                case OrderType.Sell:
                    return "sell";
            }

            throw new ArgumentOutOfRangeException("value");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringNormalized(this decimal value)
        {
            return value.ToString("0." + new string('#', DoubleRoundingPrecisionDigits), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(this string dateTime)
        {
            return DateTime.SpecifyKind(DateTime.ParseExact(dateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), DateTimeKind.Utc).ToLocalTime();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static OrderType ToOrderType(this string value)
        {
            switch (value)
            {
                case "buy":
                    return OrderType.Buy;

                case "sell":
                    return OrderType.Sell;
            }

            throw new ArgumentOutOfRangeException("value");
        }
    }
}