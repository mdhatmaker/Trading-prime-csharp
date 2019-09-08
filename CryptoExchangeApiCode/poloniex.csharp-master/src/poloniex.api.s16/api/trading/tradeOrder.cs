using XCT.BaseLib.Configuration;
using XCT.BaseLib.Types;
using Newtonsoft.Json;

namespace XCT.BaseLib.API.Poloniex.Trade
{
    public interface ITradeOrder
    {
        ulong IdOrder { get; }

        OrderType Type { get; }

        decimal PricePerCoin { get; }
        decimal AmountQuote { get; }
        decimal AmountBase { get; }
    }

    public class TradeOrder : ITradeOrder
    {
        [JsonProperty("orderNumber")]
        public ulong IdOrder { get; private set; }

        [JsonProperty("type")]
        private string TypeInternal
        {
            set { Type = value.ToOrderType(); }
        }
        public OrderType Type { get; private set; }

        [JsonProperty("rate")]
        public decimal PricePerCoin { get; private set; }
        [JsonProperty("amount")]
        public decimal AmountQuote { get; private set; }
        [JsonProperty("total")]
        public decimal AmountBase { get; private set; }
    }
}