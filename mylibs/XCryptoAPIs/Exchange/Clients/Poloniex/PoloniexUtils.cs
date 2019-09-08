using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;
using RestSharp.Deserializers;
using Tools;

namespace CryptoAPIs.Exchange.Clients.Poloniex
{
    public class RestSharpJsonNetSerializer : ISerializer
    {
        public string ContentType { get; set; }
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }

        public RestSharpJsonNetSerializer()
        {
            ContentType = "application/json";
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    } // end of class RestSharpJsonNetSerializer

    public class RestSharpJsonNetDeserializer : IDeserializer
    {
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public CultureInfo Culture { get; set; }

        public RestSharpJsonNetDeserializer()
        {
            Culture = CultureInfo.InvariantCulture;
        }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    } // end of class RestSharpJsonNetDeserializer

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
            //return CUnixTime.DateTimeUnixEpochStart.AddSeconds(unixTimeStamp);
            return GDate.DateTimeUnixEpochStart.AddSeconds(unixTimeStamp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static ulong DateTimeToUnixTimeStamp(this DateTime dateTime)
        {
            //return (ulong)Math.Floor(dateTime.Subtract(CUnixTime.DateTimeUnixEpochStart).TotalSeconds);
            return (ulong)Math.Floor(dateTime.Subtract(GDate.DateTimeUnixEpochStart).TotalSeconds);
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
    } // end of class TExtend




    public static class Extension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="balances"></param>
        /// <param name="coin_name"></param>
        /// <returns></returns>
        public static decimal available_qty(this Dictionary<string, UserBalance> balances, string coin_name)
        {
            var _result = 0.0m;

            var _coin_balance = balances.Where(b => b.Key == coin_name).SingleOrDefault();
            if (_coin_balance.Key != null)
                _result = _coin_balance.Value.QuoteAvailable;

            return _result;
        }

        public static ChartPeriod ToChartPeriod(this int minutes)
        {
            if (minutes == 5)
                return ChartPeriod.Minutes5;
            else if (minutes == 15)
                return ChartPeriod.Minutes15;
            else if (minutes == 30)
                return ChartPeriod.Minutes30;
            else if (minutes == 120)
                return ChartPeriod.Hours2;
            else if (minutes == 240)
                return ChartPeriod.Hours4;
            else if (minutes == 1440)
                return ChartPeriod.Day;
            else
                throw new Exception(string.Format("Cannot convert {0} minutes to valid Poloniex ChartPeriod.", minutes));
        }
    } // end of class Extension

} // end of namespace
