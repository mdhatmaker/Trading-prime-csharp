using Newtonsoft.Json;
using XCT.BaseLib.Configuration;

namespace XCT.BaseLib.API.Poloniex.Public
{
    public interface IPublicTicker
    {
        decimal PriceLast { get; }
        decimal PriceChangePercentage { get; }

        decimal Volume24HourBase { get; }
        decimal Volume24HourQuote { get; }

        decimal OrderTopBuy { get; }
        decimal OrderTopSell { get; }
        decimal OrderSpread { get; }
        decimal OrderSpreadPercentage { get; }

        bool IsFrozen { get; }
    }

    public class PublicTicker : IPublicTicker
    {
        [JsonProperty("last")]
        public decimal PriceLast { get; internal set; }
        [JsonProperty("percentChange")]
        public decimal PriceChangePercentage { get; internal set; }

        [JsonProperty("baseVolume")]
        public decimal Volume24HourBase { get; internal set; }
        [JsonProperty("quoteVolume")]
        public decimal Volume24HourQuote { get; internal set; }

        [JsonProperty("highestBid")]
        public decimal OrderTopBuy { get; set; }
        [JsonProperty("lowestAsk")]
        public decimal OrderTopSell { get; set; }
        public decimal OrderSpread
        {
            get { return (OrderTopSell - OrderTopBuy).Normalize(); }
        }
        public decimal OrderSpreadPercentage
        {
            get { return OrderTopSell / OrderTopBuy - 1; }
        }

        [JsonProperty("isFrozen")]
        internal byte IsFrozenInternal
        {
            set { IsFrozen = value != 0; }
        }
        public bool IsFrozen { get; private set; }
    }
}