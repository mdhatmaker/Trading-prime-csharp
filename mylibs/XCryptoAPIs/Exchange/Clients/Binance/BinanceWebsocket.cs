using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CryptoAPIs.Exchange.Clients.Binance.Websocket
{
    #region ------------------------------------ BINANCE WEBSOCKET ------------------------------------------------------------------------
    public class AccountUpdatedMessage
    {
        [JsonProperty("e")]
        public string EventType { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("m")]
        public int MakerCommission { get; set; }
        [JsonProperty("t")]
        public int TakerCommission { get; set; }
        [JsonProperty("b")]
        public int BuyerCommission { get; set; }
        [JsonProperty("s")]
        public int SellerCommission { get; set; }
        [JsonProperty("t")]
        public bool CanTrade { get; set; }
        [JsonProperty("w")]
        public bool CanWithdraw { get; set; }
        [JsonProperty("d")]
        public bool CanDeposit { get; set; }
        [JsonProperty("B")]
        public IEnumerable<Balance> Balances { get; set; }
    }
    public class Balance
    {
        [JsonProperty("a")]
        public string Asset { get; set; }
        [JsonProperty("f")]
        public decimal Free { get; set; }
        [JsonProperty("l")]
        public decimal Locked { get; set; }
    }

    public class AggregateTradeMessage
    {
        [JsonProperty("e")]
        public string EventType { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("a")]
        public int AggregatedTradeId { get; set; }
        [JsonProperty("p")]
        public decimal Price { get; set; }
        [JsonProperty("q")]
        public decimal Quantity { get; set; }
        [JsonProperty("f")]
        public int FirstBreakdownTradeId { get; set; }
        [JsonProperty("l")]
        public int LastBreakdownTradeId { get; set; }
        [JsonProperty("T")]
        public long TradeTime { get; set; }
        [JsonProperty("m")]
        public bool BuyerIsMaker { get; set; }
    }

    public class DepthMessage
    {
        public string EventType { get; set; }
        public long EventTime { get; set; }
        public string Symbol { get; set; }
        public int UpdateId { get; set; }
        public IEnumerable<OrderBookOffer> Bids { get; set; }
        public IEnumerable<OrderBookOffer> Asks { get; set; }
    }

    public class KlineMessage
    {
        [JsonProperty("e")]
        public string EventType { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("k")]
        public KlineData KlineInfo { get; set; }

        public class KlineData
        {
            [JsonProperty("t")]
            public long StartTime { get; set; }
            [JsonProperty("T")]
            public long EndTime { get; set; }
            [JsonProperty("s")]
            public string Symbol { get; set; }
            [JsonProperty("i")]
            public string Interval { get; set; }
            [JsonProperty("f")]
            public int FirstTradeId { get; set; }
            [JsonProperty("L")]
            public int LastTradeId { get; set; }
            [JsonProperty("o")]
            public decimal Open { get; set; }
            [JsonProperty("c")]
            public decimal Close { get; set; }
            [JsonProperty("h")]
            public decimal High { get; set; }
            [JsonProperty("l")]
            public decimal Low { get; set; }
            [JsonProperty("v")]
            public decimal Volume { get; set; }
            [JsonProperty("n")]
            public int NumberOfTrades { get; set; }
            [JsonProperty("x")]
            public bool IsFinal { get; set; }
            [JsonProperty("q")]
            public decimal QuoteVolume { get; set; }
            [JsonProperty("V")]
            public decimal ActiveBuyVolume { get; set; }
            [JsonProperty("Q")]
            public decimal ActiveBuyQuoteVolume { get; set; }
        }
    }

    public class OrderOrTradeUpdatedMessage
    {
        [JsonProperty("e")]
        public string OrderOrTradeReport { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("c")]
        public string NewClientOrderId { get; set; }
        [JsonProperty("S")]
        public string Side { get; set; }
        [JsonProperty("o")]
        public string Type { get; set; }
        [JsonProperty("f")]
        public string TimeInForce { get; set; }
        [JsonProperty("q")]
        public decimal OriginalQuantity { get; set; }
        [JsonProperty("p")]
        public decimal Price { get; set; }
        [JsonProperty("x")]
        public string ExecutionType { get; set; }
        [JsonProperty("X")]
        public string Status { get; set; }
        [JsonProperty("r")]
        public string RejectReason { get; set; }
        [JsonProperty("i")]
        public int Orderid { get; set; }
        [JsonProperty("l")]
        public decimal LastFilledTradeQuantity { get; set; }
        [JsonProperty("z")]
        public decimal FilledTradesAccumulatedQuantity { get; set; }
        [JsonProperty("L")]
        public decimal LastFilledTradePrice { get; set; }
        [JsonProperty("n")]
        public decimal Commission { get; set; }
        [JsonProperty("N")]
        public string CommissionAsset { get; set; }
        [JsonProperty("T")]
        public long TradeTime { get; set; }
        [JsonProperty("t")]
        public int TradeId { get; set; }
        [JsonProperty("m")]
        public bool BuyerIsMaker { get; set; }
    }
    #endregion ----------------------------------------------------------------------------------------------------------------------------

} // end of namespace
