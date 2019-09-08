using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Tools;

namespace CryptoAPIs.Exchange.Clients.Poloniex
{
    /// <summary>Represents a time frame of a market.</summary>
    public enum ChartPeriod : int
    {
        /// <summary>A time interval of 5 minutes.</summary>
        Minutes5 = 300,

        /// <summary>A time interval of 15 minutes.</summary>
        Minutes15 = 900,

        /// <summary>A time interval of 30 minutes.</summary>
        Minutes30 = 1800,

        /// <summary>A time interval of 2 hours.</summary>
        Hours2 = 7200,

        /// <summary>A time interval of 4 hours.</summary>
        Hours4 = 14400,

        /// <summary>A time interval of a day.</summary>
        Day = 86400
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CoinType
    {
        /// <summary>
        /// 
        /// </summary>
        btc,

        /// <summary>
        /// 
        /// </summary>
        ltc,

        /// <summary>
        /// 
        /// </summary>
        bgc,

        /// <summary>
        /// 
        /// </summary>
        nvc,

        /// <summary>
        /// 
        /// </summary>
        xpm,

        /// <summary>
        /// 
        /// </summary>
        nmc,

        /// <summary>
        /// 
        /// </summary>
        ppc,

        /// <summary>
        /// 
        /// </summary>
        dash,

        /// <summary>
        /// 
        /// </summary>
        doge
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CurrencyType
    {
        /// <summary>
        /// 
        /// </summary>
        cny,

        /// <summary>
        /// 
        /// </summary>
        krw,

        /// <summary>
        /// 
        /// </summary>
        usd,

        /// <summary>
        /// 
        /// </summary>
        btc
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DealerType
    {
        /// <summary>
        /// 
        /// </summary>
        bitstamp,

        /// <summary>
        /// 
        /// </summary>
        bitgold,

        /// <summary>
        /// 
        /// </summary>
        btce,

        /// <summary>
        /// 
        /// </summary>
        btcchina,

        /// <summary>
        /// 
        /// </summary>
        huobi,

        /// <summary>
        /// 
        /// </summary>
        korbit,

        /// <summary>
        /// 
        /// </summary>
        okcoin,

        /// <summary>
        /// 
        /// </summary>
        xcoin,

        /// <summary>
        /// 
        /// </summary>
        bitfinex,

        /// <summary>
        /// 
        /// </summary>
        coinone,

        /// <summary>
        /// 
        /// </summary>
        poloniex
    }

    /// <summary>
    /// 
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// bid
        /// </summary>
        Sell,

        /// <summary>
        /// ask
        /// </summary>
        Buy
    }

    /// <summary>
    /// 
    /// </summary>
    public class OrderTypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static OrderType FromString(string s)
        {
            switch (s)
            {
                case "sell":
                    return OrderType.Sell;

                case "buy":
                    return OrderType.Buy;

                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(OrderType v)
        {
            return Enum.GetName(typeof(OrderType), v).ToLowerInvariant();
        }
    }




    public interface IPublicChart
    {
        DateTime Time { get; }

        decimal Open { get; }
        decimal Close { get; }

        decimal High { get; }
        decimal Low { get; }

        decimal VolumeBase { get; }
        decimal VolumeQuote { get; }

        decimal WeightedAverage { get; }
    }

    public class PublicChart : ZCandlestick, IPublicChart
    {
        [JsonProperty("date")]
        private ulong TimeInternal
        {
            set { Time = value.UnixTimeStampToDateTime(); }
        }
        public DateTime Time { get; private set; }

        [JsonProperty("open")]
        public decimal Open { get; private set; }
        [JsonProperty("close")]
        public decimal Close { get; private set; }

        [JsonProperty("high")]
        public decimal High { get; private set; }
        [JsonProperty("low")]
        public decimal Low { get; private set; }

        [JsonProperty("volume")]
        public decimal VolumeBase { get; private set; }
        [JsonProperty("quoteVolume")]
        public decimal VolumeQuote { get; private set; }

        [JsonProperty("weightedAverage")]
        public decimal WeightedAverage { get; private set; }

        public override decimal open => Open;
        public override decimal high => High;
        public override decimal low => Low;
        public override decimal close => Close;
        public override decimal volume => VolumeQuote;
        public override decimal vwap => WeightedAverage;

        public override string ColumnText => "";

        public PublicChart(decimal o, decimal h, decimal l, decimal c, int timestamp, decimal volume, decimal vwap)
            : base(o, h, l, c, timestamp)
        {
            this.v = volume;
            this.vw = vwap;
        }
    }

    public interface IPublicOrder
    {
        decimal PricePerCoin { get; }

        decimal AmountQuote { get; }
        decimal AmountBase { get; }
    }

    public class PublicOrder : IPublicOrder
    {
        public decimal PricePerCoin { get; private set; }

        public decimal AmountQuote { get; private set; }
        public decimal AmountBase
        {
            get { return (AmountQuote * PricePerCoin).Normalize(); }
        }

        internal PublicOrder(decimal pricePerCoin, decimal amountQuote)
        {
            PricePerCoin = pricePerCoin;
            AmountQuote = amountQuote;
        }

        internal PublicOrder()
        {

        }
    }

    public interface IPublicOrderBook
    {
        IList<IPublicOrder> BuyOrders { get; }
        IList<IPublicOrder> SellOrders { get; }
    }

    public class PublicOrderBook : IPublicOrderBook
    {
        [JsonProperty("bids")]
        private IList<string[]> BuyOrdersInternal
        {
            set { BuyOrders = ParseOrders(value); }
        }
        public IList<IPublicOrder> BuyOrders { get; private set; }

        [JsonProperty("asks")]
        private IList<string[]> SellOrdersInternal
        {
            set { SellOrders = ParseOrders(value); }
        }
        public IList<IPublicOrder> SellOrders { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IList<IPublicOrder> ParseOrders(IList<string[]> orders)
        {
            var output = new List<IPublicOrder>(orders.Count);
            for (var i = 0; i < orders.Count; i++)
            {
                output.Add(
                    new PublicOrder(
                        decimal.Parse(orders[i][0], NumberStyles.Float),
                        decimal.Parse(orders[i][1], NumberStyles.Float)
                    )
                );
            }
            return output;
        }
    }

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

    public interface IPublicTrade
    {
        DateTime Time { get; }

        OrderType Type { get; }

        decimal PricePerCoin { get; }

        decimal AmountQuote { get; }
        decimal AmountBase { get; }
    }

    public class PublicTrade : IPublicTrade
    {
        [JsonProperty("date")]
        private string TimeInternal
        {
            set { Time = value.ParseDateTime(); }
        }

        public DateTime Time { get; private set; }

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

    public interface ITrade : ITradeOrder
    {
        DateTime Time { get; }
    }

    public class Trade : TradeOrder, ITrade
    {
        [JsonProperty("date")]
        private string TimeInternal
        {
            set { Time = value.ParseDateTime(); }
        }
        public DateTime Time { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IUserBalance
    {
        decimal QuoteAvailable
        {
            get;
        }

        decimal QuoteOnOrders
        {
            get;
        }

        decimal BitcoinValue
        {
            get;
        }
    }

    /// <summary>
    /// poloniex 거래소 회원 지갑 정보
    /// </summary>
    public class UserBalance : IUserBalance
    {
        [JsonProperty("available")]
        public decimal QuoteAvailable
        {
            get;
            private set;
        }

        [JsonProperty("onOrders")]
        public decimal QuoteOnOrders
        {
            get;
            private set;
        }

        [JsonProperty("btcValue")]
        public decimal BitcoinValue
        {
            get;
            private set;
        }
    }

    public interface IDeposit
    {
        string Currency { get; }
        string Address { get; }
        decimal Amount { get; }

        DateTime Time { get; }
        string TransactionId { get; }
        uint Confirmations { get; }

        string Status { get; }
    }

    public class Deposit : IDeposit
    {
        [JsonProperty("currency")]
        public string Currency { get; private set; }

        [JsonProperty("address")]
        public string Address { get; private set; }

        [JsonProperty("amount")]
        public decimal Amount { get; private set; }

        [JsonProperty("confirmations")]
        public uint Confirmations { get; private set; }

        [JsonProperty("txid")]
        public string TransactionId { get; private set; }

        [JsonProperty("timestamp")]
        private ulong TimeInternal
        {
            set { Time = value.UnixTimeStampToDateTime(); }
        }

        public DateTime Time { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }

        public bool IsCompleted
        {
            get
            {
                return Status.ToUpper() == "COMPLETE";
            }
        }
    }

    public interface IUserDepositAddress
    {
        bool IsGenerationSuccessful { get; }

        string Address { get; }
    }

    public class UserDepositAddress : IUserDepositAddress
    {
        [JsonProperty("success")]
        private byte IsGenerationSuccessfulInternal
        {
            set { IsGenerationSuccessful = value == 1; }
        }
        public bool IsGenerationSuccessful { get; private set; }

        [JsonProperty("response")]
        public string Address { get; private set; }
    }

    public interface IDepositWithdrawal
    {
        IList<Deposit> Deposits { get; }

        IList<Withdrawal> Withdrawals { get; }
    }

    public class DepositWithdrawal : IDepositWithdrawal
    {
        [JsonProperty("deposits")]
        public IList<Deposit> Deposits { get; private set; }

        [JsonProperty("withdrawals")]
        public IList<Withdrawal> Withdrawals { get; private set; }
    }

    public interface IWithdrawal
    {
        ulong Id { get; }

        string Currency { get; }
        string Address { get; }
        decimal Amount { get; }

        DateTime Time { get; }
        string IpAddress { get; }

        string Status { get; }
    }

    public class Withdrawal : IWithdrawal
    {
        [JsonProperty("withdrawalNumber")]
        public ulong Id { get; private set; }

        [JsonProperty("currency")]
        public string Currency { get; private set; }

        [JsonProperty("address")]
        public string Address { get; private set; }

        [JsonProperty("amount")]
        public decimal Amount { get; private set; }

        [JsonProperty("fee")]
        public decimal Fee { get; private set; }

        [JsonProperty("timestamp")]
        private ulong TimeInternal
        {
            set { Time = value.UnixTimeStampToDateTime(); }
        }

        public DateTime Time { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }

        public bool IsCompleted
        {
            get
            {
                var _completed = false;

                var _states = Status.Split(':');
                if (_states.Length > 0)
                    _completed = _states[0].ToUpper() == "COMPLETE";

                return _completed;
            }
        }

        public string TransactionId
        {
            get
            {
                var _result = "";

                if (IsCompleted == true)
                {
                    var _states = Status.Split(' ');
                    _result = _states.Length > 1 ? _states[1].ToLower() : "";
                }

                return _result;
            }
        }

        [JsonProperty("ipAddress")]
        public string IpAddress { get; private set; }
    }

    public class CurrencyPair
    {
        private const char SeparatorCharacter = '_';

        public string CurrencyName { get; private set; }
        public string CoinName { get; private set; }

        public CurrencyPair(string currency_name, string coin_name)
        {
            CurrencyName = currency_name;
            CoinName = coin_name;
        }

        public static CurrencyPair Parse(string currency_pair)
        {
            var _split = currency_pair.Split(SeparatorCharacter);
            return new CurrencyPair(_split[0], _split[1]);
        }

        public override string ToString()
        {
            return CurrencyName + SeparatorCharacter + CoinName;
        }

        public static bool operator ==(CurrencyPair a, CurrencyPair b)
        {
            if (ReferenceEquals(a, b)) return true;
            if ((object)a == null ^ (object)b == null) return false;

            return a.CurrencyName == b.CurrencyName && a.CoinName == b.CoinName;
        }

        public static bool operator !=(CurrencyPair a, CurrencyPair b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            var b = obj as CurrencyPair;
            return (object)b != null && Equals(b);
        }

        public bool Equals(CurrencyPair b)
        {
            return b.CurrencyName == CurrencyName && b.CoinName == CoinName;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }



} // end of namespace
