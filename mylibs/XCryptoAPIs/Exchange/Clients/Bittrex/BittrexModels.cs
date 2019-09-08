using System;
using Newtonsoft.Json;

namespace CryptoAPIs.Exchange.Clients.Bittrex
{
    /// <summary>
    /// Contains the account balance for a particular currency
    /// </summary>
    public class AccountBalance
    {
        public String Currency { get; set; }
        public Decimal Balance { get; set; }
        public Decimal Available { get; set; }
        public Decimal Pending { get; set; }
        public String CryptoAddress { get; set; }
        public bool Requested { get; set; }
        public String Uuid { get; set; }

        public override string ToString()
        {
            return string.Format("[AccountBalance: Currency={0}, Balance={1}, Available={2}, Pending={3}, CryptoAddress={4}, Requested={5}, Uuid={6}]", Currency, Balance, Available, Pending, CryptoAddress, Requested, Uuid);
        }
    }

    /// <summary>
    /// A general result wrapper for the bittrex api end points
    /// Every end point provides results in the same format containing a success flag, a message field to return any errors that may have occurred and the actual json result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        public ApiResult(bool success, String message, T result)
        {
            Success = success;
            Message = message;
            Result = result;
        }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
        [JsonProperty(PropertyName = "message")]
        public String Message { get; set; }
        [JsonProperty(PropertyName = "result")]
        public T Result { get; set; }
    }

    public enum BookOrderType
    {
        Buy,
        Sell
    }

    /// <summary>
    /// The result of the /public/getorderbook end point
    /// This contains a single book order for the request
    /// </summary>
    public class BookOrder
    {
        public BookOrderType OrderType { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Rate { get; set; }
    }

    /// <summary>
    /// The result of the /public/getcurrencies end point
    /// This contains the details of a tradeable currency on the bittrex trading platform
    /// </summary>
    public class CryptoCurrency
    {
        public String Currency { get; set; }
        public String CurrencyLong { get; set; }
        public int MinConfirmation { get; set; }
        public Decimal TxFee { get; set; }
        public bool IsActive { get; set; }
        public String CoinType { get; set; }
        public String BaseAddress { get; set; }
        public String Notice { get; set; }
    }

    /// <summary>
    /// The result of the /account/getdepositaddress/ end point
    /// </summary>
    public class DepositAddress
    {
        /// <summary>
        /// The currency of the deposit address, i.e. BTC
        /// </summary>
        public String Currency { get; set; }
        /// <summary>
        /// The wallet address
        /// </summary>
        public String Address { get; set; }

        public override string ToString()
        {
            return string.Format("[DepositAddress: Currency={0}, Address={1}]", Currency, Address);
        }
    }

    public class HistoricAccountOrder
    {
        public Guid OrderUuid { get; set; }
        public String Exchange { get; set; }
        public DateTime TimeStamp { get; set; }
        public String OrderType { get; set; }
        public Decimal Limit { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal QuantityRemaining { get; set; }
        public Decimal Commission { get; set; }
        public Decimal Price { get; set; }
        public Decimal PricePerUnit { get; set; }
        public bool IsConditional { get; set; }
        public String Condition { get; set; }
        public String ConditionTarget { get; set; }
        public bool ImmediateOrCancel { get; set; }
    }

    public class HistoricTrade
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Price { get; set; }
        public Decimal Total { get; set; }
        public String FillType { get; set; }
        public String OrderType { get; set; }
    }

    /// <summary>
    /// The result of the /public/getmarkets end point
    /// This contains the details of a tradeable market
    /// </summary>
    public class Market
    {
        public String MarketCurrency { get; set; }
        public String MarketCurrencyLong { get; set; }
        public String BaseCurrency { get; set; }
        public String BaseCurrencyLong { get; set; }
        public Decimal MinTradeSize { get; set; }
        public String MarketName { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
    }

    /// <summary>    
    /// The result of the /public/getmarketsummaries end point
    /// This contains a summary of the last 24 hours trading for the market
    /// </summary>
    public class MarketSummary
    {
        public String MarketName { get; set; }
        public Decimal High { get; set; }
        public Decimal Low { get; set; }
        public Decimal Volume { get; set; }
        public Decimal Last { get; set; }
        public Decimal BaseVolume { get; set; }
        public DateTime TimeStamp { get; set; }
        public Decimal Bid { get; set; }
        public Decimal Ask { get; set; }
        public int OpenBuyOrders { get; set; }
        public int OpenSellOrders { get; set; }
        public Decimal PrevDay { get; set; }
        public DateTime Created { get; set; }
    }

    /// <summary>
    /// An open order result from the /market/getopenorders end point
    /// </summary>
    public class OpenOrder
    {
        public Guid? Uuid { get; set; }
        public Guid OrderUuid { get; set; }
        public String Exchange { get; set; }
        public String OrderType { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal QuantityRemaining { get; set; }
        public Decimal Limit { get; set; }
        public Decimal CommissionPaid { get; set; }
        public Decimal Price { get; set; }
        public Decimal? PricePerUnit { get; set; }
        public DateTime Opened { get; set; }
        public DateTime? Closed { get; set; }
        public bool CancelInitiated { get; set; }
        public bool ImmediateOrCancel { get; set; }
        public bool IsConditional { get; set; }
        public String Condition { get; set; }
        public String ConditionTarget { get; set; }
    }

    /// <summary>
    /// An order result from the /account/getorder end point
    /// </summary>
    public class Order
    {
        public Guid? AccountId { get; set; }
        public Guid OrderUuid { get; set; }
        public String Exchange { get; set; }
        public String Type { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal QuantityRemaining { get; set; }
        public Decimal Limit { get; set; }
        public Decimal Reserved { get; set; }
        public Decimal ReservedRemaining { get; set; }
        public Decimal CommissionReservedPaid { get; set; }
        public Decimal CommissionReserveRemaining { get; set; }
        public Decimal CommissionPaid { get; set; }
        public Decimal Price { get; set; }
        public Decimal? PricePerUnit { get; set; }
        public DateTime Opened { get; set; }
        public DateTime? Closed { get; set; }
        public bool IsOpen { get; set; }
        public bool CancelInitiated { get; set; }
        public bool ImmediateOrCancel { get; set; }
        public bool IsConditional { get; set; }
        public String Condition { get; set; }
        public String ConditionTarget { get; set; }
    }

    /// <summary>
    /// The result of the /market/buylimit and /market/selllimit end points.   
    /// </summary>
    public class OrderResult
    {
        /// <summary>
        /// The unique identifier of the order created by the end point
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public Guid Uuid { get; set; }
    }

    /// <summary>
    /// The result of the /public/getticker
    /// </summary>
    public class Ticker
    {
        public Decimal Bid { get; set; }
        public Decimal Ask { get; set; }
        public Decimal Last { get; set; }

        public decimal BidPrice { get { return Bid; } }
        public decimal AskPrice { get { return Ask; } }
        public decimal BidVolume { get { return 0; } }          // Bittrex Ticker object has no bid size
        public decimal AskVolume { get { return 0; } }          // Bittrex Ticker object has no ask size

        public override string ToString()
        {
            return string.Format("[Ticker: Bid={0}, Ask={1}, Last={2}]", Bid, Ask, Last);
        }
    }

} // end of namespace
