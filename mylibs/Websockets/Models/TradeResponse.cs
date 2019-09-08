using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Tools.Websockets.Models
{
    // Now we know what the exchange is and what our primary/secondary currencies are.
    // We still need to parse our trade info using the message. Let's create a TradeResponse-entity
    // that will contain all that info:
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

} // end of namespace
