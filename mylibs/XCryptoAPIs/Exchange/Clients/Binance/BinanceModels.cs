using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CryptoAPIs.Exchange.Clients.Binance
{

    public class AccountInfo
    {
        [JsonProperty("makerCommission")]
        public int MakerCommission { get; set; }
        [JsonProperty("takerCommission")]
        public int TakerCommission { get; set; }
        [JsonProperty("buyerCommission")]
        public int BuyerCommission { get; set; }
        [JsonProperty("sellerCommission")]
        public int SellerCommission { get; set; }
        [JsonProperty("canTrade")]
        public bool CanTrade { get; set; }
        [JsonProperty("canWithdraw")]
        public bool CanWithdraw { get; set; }
        [JsonProperty("canDeposit")]
        public bool CanDeposit { get; set; }
        [JsonProperty("balances")]
        public IEnumerable<Balance> Balances { get; set; }
    }
    public class Balance
    {
        [JsonProperty("asset")]
        public string Currency { get; set; }
        [JsonProperty("free")]
        public decimal Free { get; set; }
        [JsonProperty("locked")]
        public decimal Locked { get; set; }
    }

    public class CanceledOrder
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
        [JsonProperty("origClientOrderId ")]
        public string OrigClientOrderId { get; set; }
    }

    public class Deposit
    {
        [JsonProperty("insertTime")]
        public long InsertTime { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("asset")]
        public string Asset { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public class DepositHistory
    {
        [JsonProperty("depositList")]
        public IEnumerable<Deposit> DepositList { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class NewOrder
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
        [JsonProperty("transactTime")]
        public long TransactTime { get; set; }
    }

    public class Order
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("origQty")]
        public decimal OrigQty { get; set; }
        [JsonProperty("executedQty")]
        public decimal ExecutedQty { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("timeInForce")]
        public string TimeInForce { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("side")]
        public string Side { get; set; }
        [JsonProperty("stopPrice")]
        public decimal StopPrice { get; set; }
        [JsonProperty("icebergQty")]
        public decimal IcebergQty { get; set; }
        [JsonProperty("time")]
        public long Time { get; set; }
    }

    public class Trade
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("qty")]
        public decimal Quantity { get; set; }
        [JsonProperty("commission")]
        public decimal Commission { get; set; }
        [JsonProperty("commissionAsset")]
        public string CommissionAsset { get; set; }
        [JsonProperty("time")]
        public long Time { get; set; }
        [JsonProperty("isBuyer")]
        public bool IsBuyer { get; set; }
        [JsonProperty("isMaker")]
        public bool IsMaker { get; set; }
        [JsonProperty("isBestMatch")]
        public bool IsBestMatch { get; set; }
    }

    public class WithdrawHistory
    {
        [JsonProperty("withdrawList")]
        public IEnumerable<Deposit> WithdrawList { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class Withdraw
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("txId")]
        public string TxId { get; set; }
        [JsonProperty("asset")]
        public string Asset { get; set; }
        [JsonProperty("applyTime")]
        public long ApplyTime { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public class WithdrawResponse
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class ServerInfo
    {
        [JsonProperty("serverTIme")]
        public long ServerTime { get; set; }
    }

    #region ------------------------------------ TRADING RULES ----------------------------------------------------------------------------
    public class Filter
    {
        [JsonProperty("filterType")]
        public string FilterType { get; set; }
        [JsonProperty("minPrice")]
        public decimal MinPrice { get; set; }
        [JsonProperty("maxPrice")]
        public decimal MaxPrice { get; set; }
        [JsonProperty("tickSize")]
        public decimal TickSize { get; set; }
        [JsonProperty("minQty")]
        public decimal MinQty { get; set; }
        [JsonProperty("maxQty")]
        public decimal MaxQty { get; set; }
        [JsonProperty("stepSize")]
        public decimal StepSize { get; set; }
        [JsonProperty("minNotional")]
        public decimal MinNotional { get; set; }
    }

    public class RateLimit
    {
        [JsonProperty("rateLimitType")]
        public string RateLimitType { get; set; }
        [JsonProperty("interval")]
        public string Interval { get; set; }
        [JsonProperty("limit")]
        public int Limit { get; set; }
    }

    public class Symbol
    {
        [JsonProperty("symbol")]
        public string SymbolName { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("baseAsset")]
        public string BaseAsset { get; set; }
        [JsonProperty("baseAssetPrecision")]
        public int BaseAssetPrecision { get; set; }
        [JsonProperty("quoteAsset")]
        public string QuoteAsset { get; set; }
        [JsonProperty("quotePrecision")]
        public int QuotePrecision { get; set; }
        [JsonProperty("orderTypes")]
        public IEnumerable<string> OrderTypes { get; set; }
        [JsonProperty("icebergAllowed")]
        public bool IcebergAllowed { get; set; }
        [JsonProperty("filters")]
        public IEnumerable<Filter> Filters { get; set; }
    }

    public class TradingRules
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
        [JsonProperty("serverTime")]
        public long ServerTime { get; set; }
        [JsonProperty("rateLimits")]
        public IEnumerable<RateLimit> RateLimits { get; set; }
        [JsonProperty("symbols")]
        public IEnumerable<Symbol> Symbols { get; set; }
    }
    #endregion ----------------------------------------------------------------------------------------------------------------------------

    public class AggregateTrade
    {
        [JsonProperty("a")]
        public int AggregateTradeId { get; set; }
        [JsonProperty("p")]
        public decimal Price { get; set; }
        [JsonProperty("q")]
        public decimal Quantity { get; set; }
        [JsonProperty("f")]
        public int FirstTradeId { get; set; }
        [JsonProperty("l")]
        public int LastTradeId { get; set; }
        [JsonProperty("T")]
        public long TimeStamp { get; set; }
        [JsonProperty("m")]
        public bool BuyerIsMaker { get; set; }
        [JsonProperty("M")]
        public bool BestPriceMatch { get; set; }
    }

    public class Candlestick
    {
        public long OpenTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public long CloseTime { get; set; }
        public decimal QuoteAssetVolume { get; set; }
        public int NumberOfTrades { get; set; }
        public decimal TakerBuyBaseAssetVolume { get; set; }
        public decimal TakerBuyQuoteAssetVolume { get; set; }
    }

    public class OrderBook
    {
        public long LastUpdateId { get; set; }
        public IEnumerable<OrderBookOffer> Bids { get; set; }
        public IEnumerable<OrderBookOffer> Asks { get; set; }
    }
    public class OrderBookOffer
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }

    public class OrderBookTicker
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("bidPrice")]
        public decimal BidPrice { get; set; }
        [JsonProperty("bidQty")]
        public decimal BidQuantity { get; set; }
        [JsonProperty("askPrice")]
        public decimal AskPrice { get; set; }
        [JsonProperty("askQty")]
        public decimal AskQuantity { get; set; }
    }

    public class PriceChangeInfo
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("priceChange")]
        public decimal PriceChange { get; set; }
        [JsonProperty("priceChangePercent")]
        public decimal PriceChangePercent { get; set; }
        [JsonProperty("weightedAvgPrice")]
        public decimal WeightedAvgPrice { get; set; }
        [JsonProperty("prevClosePrice")]
        public decimal PrevClosePrice { get; set; }
        [JsonProperty("lastPrice")]
        public decimal LastPrice { get; set; }
        [JsonProperty("bidPrice")]
        public decimal BidPrice { get; set; }
        [JsonProperty("askPrice")]
        public decimal AskPrice { get; set; }
        [JsonProperty("openPrice")]
        public decimal OpenPrice { get; set; }
        [JsonProperty("highPrice")]
        public decimal HighPrice { get; set; }
        [JsonProperty("lowPrice")]
        public decimal LowPrice { get; set; }
        [JsonProperty("volume")]
        public decimal Volume { get; set; }
        [JsonProperty("openTime")]
        public long OpenTime { get; set; }
        [JsonProperty("closeTime")]
        public long CloseTime { get; set; }
        [JsonProperty("firstId")]
        public int FirstId { get; set; }
        [JsonProperty("lastId")]
        public int LastId { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class SymbolPrice
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }

    public class UserStreamInfo
    {
        [JsonProperty("listenKey")]
        public string ListenKey { get; set; }
    }


} // end of namespace
