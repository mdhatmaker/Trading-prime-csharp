using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoAPIs.Exchange.Clients.Binance.Websocket;
using Tools;

namespace CryptoAPIs.Exchange.Clients.Binance
{

    /// <summary>
    /// Class to define extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Extension method to get a enum description.
        /// </summary>
        /// <param name="value">Enum to get the description from.</param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            return ((DescriptionAttribute)Attribute.GetCustomAttribute(
                value.GetType().GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Single(x => x.GetValue(null).Equals(value)),
                typeof(DescriptionAttribute)))?.Description ?? value.ToString();
        }

        /// <summary>
        /// Gets a timestamp in milliseconds.
        /// </summary>
        /// <returns>Timestamp in milliseconds.</returns>
        public static string GetUnixTimeStamp(this DateTime baseDateTime)
        {
            //var dtOffset = new DateTimeOffset(baseDateTime);
            //return dtOffset.ToUnixTimeMilliseconds().ToString();
            return GDate.DateTimeToUnixTimeMilliseconds(baseDateTime).ToString();
        }

        /// <summary>
        /// Validates if string is a valid JSON
        /// </summary>
        /// <param name="stringValue">String to validate</param>
        /// <returns></returns>
        public static bool IsValidJson(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            var value = stringValue.Trim();

            if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                (value.StartsWith("[") && value.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(value);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }

            return false;
        }
    } // end of class ExtensionMethods



    /// <summary>
    /// Class to parse some specific entities.
    /// </summary>
    public class CustomParser
    {
        /// <summary>
        /// Gets the orderbook data and generates an OrderBook object.
        /// </summary>
        /// <param name="orderBookData">Dynamic containing the orderbook data.</param>
        /// <returns></returns>
        public OrderBook GetParsedOrderBook(dynamic orderBookData)
        {
            var result = new OrderBook
            {
                LastUpdateId = orderBookData.lastUpdateId.Value
            };

            var bids = new List<OrderBookOffer>();
            var asks = new List<OrderBookOffer>();

            foreach (JToken item in ((JArray)orderBookData.bids).ToArray())
            {
                bids.Add(new OrderBookOffer() { Price = decimal.Parse(item[0].ToString()), Quantity = decimal.Parse(item[1].ToString()) });
            }

            foreach (JToken item in ((JArray)orderBookData.asks).ToArray())
            {
                asks.Add(new OrderBookOffer() { Price = decimal.Parse(item[0].ToString()), Quantity = decimal.Parse(item[1].ToString()) });
            }

            result.Bids = bids;
            result.Asks = asks;

            return result;
        }

        /// <summary>
        /// Gets the candlestick data and generates an Candlestick object.
        /// </summary>
        /// <param name="candlestickData">Dynamic containing the candlestick data.</param>
        /// <returns></returns>
        public IEnumerable<Candlestick> GetParsedCandlestick(dynamic candlestickData)
        {
            var result = new List<Candlestick>();

            foreach (JToken item in ((JArray)candlestickData).ToArray())
            {
                result.Add(new Candlestick()
                {
                    OpenTime = long.Parse(item[0].ToString()),
                    Open = decimal.Parse(item[1].ToString()),
                    High = decimal.Parse(item[2].ToString()),
                    Low = decimal.Parse(item[3].ToString()),
                    Close = decimal.Parse(item[4].ToString()),
                    Volume = decimal.Parse(item[5].ToString()),
                    CloseTime = long.Parse(item[6].ToString()),
                    QuoteAssetVolume = decimal.Parse(item[7].ToString()),
                    NumberOfTrades = int.Parse(item[8].ToString()),
                    TakerBuyBaseAssetVolume = decimal.Parse(item[9].ToString()),
                    TakerBuyQuoteAssetVolume = decimal.Parse(item[10].ToString())
                });
            }

            return result;
        }

        public DepthMessage GetParsedDepthMessage(dynamic messageData)
        {
            var result = new DepthMessage
            {
                EventType = messageData.e,
                EventTime = messageData.E,
                Symbol = messageData.s,
                UpdateId = messageData.u
            };

            var bids = new List<OrderBookOffer>();
            var asks = new List<OrderBookOffer>();

            foreach (JToken item in ((JArray)messageData.b).ToArray())
            {
                bids.Add(new OrderBookOffer() { Price = decimal.Parse(item[0].ToString()), Quantity = decimal.Parse(item[1].ToString()) });
            }

            foreach (JToken item in ((JArray)messageData.a).ToArray())
            {
                asks.Add(new OrderBookOffer() { Price = decimal.Parse(item[0].ToString()), Quantity = decimal.Parse(item[1].ToString()) });
            }

            result.Bids = bids;
            result.Asks = asks;

            return result;
        }
    } // end of class CustomParser



    /// <summary>
    /// Public class to store all end points.
    /// </summary>
    public static class EndPoints
    {
        #region General Endpoints
        public static readonly string TestConnectivity = "/api/v1/ping";
        public static readonly string CheckServerTime = "/api/v1/time";
        #endregion

        #region Market Data Endpoints
        public static readonly string OrderBook = "/api/v1/depth";
        public static readonly string AggregateTrades = "/api/v1/aggTrades";
        public static readonly string Candlesticks = "/api/v1/klines";
        public static readonly string TickerPriceChange24H = "/api/v1/ticker/24hr";
        public static readonly string AllPrices = "/api/v1/ticker/allPrices";
        public static readonly string OrderBookTicker = "/api/v1/ticker/allBookTickers";
        public static readonly string TradingRules = "https://gist.githubusercontent.com/Ninj0r/3029b9d635f8f81f5ffab9cc9df5cc61/raw/810530a2118e5d8cdcfcc4d220349976a0acf131/tradingRules_20171022.json";
        #endregion

        #region Account Endpoints
        public static readonly string NewOrder = "/api/v3/order";
        public static readonly string NewOrderTest = "/api/v3/order/test";
        public static readonly string QueryOrder = "/api/v3/order";
        public static readonly string CancelOrder = "/api/v3/order";
        public static readonly string CurrentOpenOrders = "/api/v3/openOrders";
        public static readonly string AllOrders = "/api/v3/allOrders";
        public static readonly string AccountInformation = "/api/v3/account";
        public static readonly string TradeList = "/api/v3/myTrades";

        public static readonly string Withdraw = "/wapi/v1/withdraw.html";
        public static readonly string DepositHistory = "/wapi/v1/getDepositHistory.html";
        public static readonly string WithdrawHistory = "/wapi/v1/getWithdrawHistory.html";
        #endregion

        #region User Stream Endpoints
        public static readonly string StartUserStream = "/api/v1/userDataStream";
        public static readonly string KeepAliveUserStream = "/api/v1/userDataStream";
        public static readonly string CloseUserStream = "/api/v1/userDataStream";
        #endregion
    } // end of class EndPoints



    /// <summary>
    /// Utility class for common processes. 
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Gets a HMACSHA256 signature based on the API Secret.
        /// </summary>
        /// <param name="apiSecret">Api secret used to generate the signature.</param>
        /// <param name="message">Message to encode.</param>
        /// <returns></returns>
        public static string GenerateSignature(string apiSecret, string message)
        {
            var key = Encoding.UTF8.GetBytes(apiSecret);
            string stringHash;
            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                stringHash = BitConverter.ToString(hash).Replace("-", "");
            }

            return stringHash;
        }

        /// <summary>
        /// Gets a timestamp in milliseconds.
        /// </summary>
        /// <returns>Timestamp in milliseconds.</returns>
        public static string GenerateTimeStamp(DateTime baseDateTime)
        {
            //var dtOffset = new DateTimeOffset(baseDateTime);
            //return dtOffset.ToUnixTimeMilliseconds().ToString();
            return GDate.DateTimeToUnixTimeMilliseconds(baseDateTime).ToString();
        }

        /// <summary>
        /// Gets an HttpMethod object based on a string.
        /// </summary>
        /// <param name="method">Name of the HttpMethod to create.</param>
        /// <returns>HttpMethod object.</returns>
        public static HttpMethod CreateHttpMethod(string method)
        {
            switch (method.ToUpper())
            {
                case "DELETE":
                    return HttpMethod.Delete;
                case "POST":
                    return HttpMethod.Post;
                case "PUT":
                    return HttpMethod.Put;
                case "GET":
                    return HttpMethod.Get;
                default:
                    throw new NotImplementedException();
            }
        }
    } // end of class Utilities

} // end of namespace
