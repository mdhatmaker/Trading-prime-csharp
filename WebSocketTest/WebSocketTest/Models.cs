using System;
using Newtonsoft.Json;

namespace WebSocketX
{

    public class ApiCredentials
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("apiSecret")]
        public string ApiSecret { get; set; }
    }

    // One of the functions that parse the message into a trade data entity.
    // Class with a function which takes a channel name as parameter and returns a MarketInfo-entity
    internal class MarketInfo
    {
        internal string Exchange { get; set; }
        internal string PrimaryCurrency { get; set; }
        internal string SecondaryCurrency { get; set; }

        // A function to parse a string and return our MarketInfo
        internal static MarketInfo ParseMarketInfo(string data)
        {
            var str = data.Replace("--", "-");
            var strArr = str.Split('-');
            return new MarketInfo()
            {
                Exchange = strArr[1],
                PrimaryCurrency = strArr[2],
                SecondaryCurrency = strArr[3]
            };
        }
    }

    #region TradeResponse-entity ----------------------------------------------------------------
    // Once we know what the exchange is and what our primary/secondary currencies are,
    // we still need to parse our trade info using the message.
    // This TradeResponse-entity will contain all that info:
    public class TradeResponse
    {
        [JsonProperty("data")]
        public TradeData TradeData { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }
    }

    public class TradeData
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("data")]
        public TradeItem Trade { get; set; }
    }

    public class TradeItem
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("exchId")]
        public long ExchId { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("marketid")]
        public long Marketid { get; set; }

        [JsonProperty("market_history_id")]
        public long MarketHistoryId { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("time_local")]
        public string TimeLocal { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("tradeid")]
        public string TradeId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
    #endregion ----------------------------------------------------------------------------------

    // We still need to deserialize our message into a TradeResponse.
    // This Helper class with a generic "ToEntity" function takes a string as parameter
    // and returns an entity:
    internal static class Helper
    {
        internal static T ToEntity<T>(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
        }
    }


} // end of namespace
