using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptoAPIs.Exchange.Clients.ItBit
{
    public enum TickerSymbol
    {
        XBTUSD,
        XBTSGD,
        XBTEUR
    }

    // ISO 4217 currency code
    public enum CurrencyCode
    {
        USD,
        SGD,
        EUR,
        XBT
    }

    public enum OrderStatus
    {
        submitted,
        open,
        filled,
        cancelled,
        rejected
    }

    public enum OrderSide
    {
        buy,
        sell
    }

    public enum OrderType
    {
        limit
    }

    public enum ErrorCodes
    {
        InvalidSignature = 10002,
        Unathorized = 10003,
        ValidationErrors = 10005,
        InvalidWalletName = 80001,
        WalletNameAlreadyInUse = 80003,
        WalletNotFound = 80004,
        OrderNotFound = 80005,
        NoMatchingCurrency = 80008,
        InvalidDatetimeRange = 80011,
        TickerSymbolNotFound = 80012,
        WithdrawalAmoutOutOfRange = 80013,
        OrderDisplayGreaterThanAmount = 80020,
        OrderDisplayLessThanAmount = 80021,
        InsufficientWalletFunds = 81001,
        OrderCannotBeCancelled = 81002,
        WalletDailyDepositLimitReached = 85001,
        WalletDailyWithdrawalLimitReached = 85002,
        WalletMonthlyWithdrawalLimitReached = 85003,
        OrderTooManyMetadataEntries = 86001,
        OrderTooLongMetadataValue = 86002
    }


    internal interface IMessageBuilder
    {
        RequestMessage Build();
    }

    internal class RequestMessage
    {
        public HttpMethod Method { get; set; }
        public Uri RequestUri { get; set; }
        public string Content { get; set; }
    }

    public class OrderBook
    {
        // currency pair for market. e.g. XBTUSD for USD Bitcoin market.
        [JsonProperty("asks")]
        public Trade[] Asks { get; set; }

        // highest bid price
        [JsonProperty("bids")]
        public Trade[] Bids { get; set; }
    }

    public class RecentTrade
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("mutchNumber")]
        public int MatchNumber { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }

    public class RecentTrades
    {
        [JsonProperty("recentTrades")]
        public RecentTrade[] Trades { get; set; }
    }

    public class Ticker
    {
        // currency pair for market. e.g. XBTUSD for USD Bitcoin market.
        [JsonProperty("pair")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TickerSymbol Pair { get; set; }

        // highest bid price
        [JsonProperty("bid")]
        public decimal Bid { get; set; }

        // highest bid amount
        [JsonProperty("bidAmt")]
        public decimal BidAmount { get; set; }

        // lowest ask price
        [JsonProperty("ask")]
        public decimal Ask { get; set; }

        // lowest ask amount
        [JsonProperty("askAmt")]
        public decimal AskAmount { get; set; }

        // last traded price
        [JsonProperty("lastPrice")]
        public decimal LastPrice { get; set; }

        // last traded amount
        [JsonProperty("lastAmt")]
        public decimal LastAmount { get; set; }

        // total traded volume in the last 24 hours
        [JsonProperty("volume24h")]
        public decimal Volume24Hs { get; set; }

        // total traded volume since midnight UTC
        [JsonProperty("volumeToday")]
        public decimal VolumeToday { get; set; }

        // highest traded price in the last 24 hours
        [JsonProperty("high24h")]
        public decimal HighestPrice24Hs { get; set; }

        // lowest traded price in the last 24 hours
        [JsonProperty("low24h")]
        public decimal LowestPrice24Hs { get; set; }

        // highest traded price since midnight UTC
        [JsonProperty("highToday")]
        public decimal HighestPriceToday { get; set; }

        // lowest traded price since midnight UTC
        [JsonProperty("lowToday")]
        public decimal LowestPriceToday { get; set; }

        // first traded price since midnight UTC
        [JsonProperty("openToday")]
        public decimal OpenToday { get; set; }

        // volume weighted average price traded since midnight UTC
        [JsonProperty("vwapToday")]
        public string vwapToday { get; set; }

        // volume weighted average price traded in the last 24 hours
        [JsonProperty("vwap24h")]
        public string vwap24h { get; set; }

        // server time in UTC
        [JsonProperty("serverTimeUTC")]
        public DateTime ServerTimeUtc { get; set; }
    }

    public class Trade
    {
        public Trade(decimal price, decimal amount)
        {
            Price = price;
            Amount = amount;
        }

        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }

    public class Balance
    {
        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCode Currency { get; set; }

        [JsonProperty("availableBalance")]
        public decimal AvailableBalance { get; set; }

        [JsonProperty("totalBalance")]
        public decimal TotalBalance { get; set; }
    }

    public class CryptoCurrencyDepositResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("walletID")]
        public string WalletId { get; set; }

        [JsonProperty("depositAddress")]
        public string DepositAddress { get; set; }
    }

    public class CryptoCurrencyWithdrawalResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("completionDate")]
        public DateTime CompletionDate { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCode Currency { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }

    public class NewCryptoCurrencyDeposit
    {
        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCode Currency { get; set; }

        public NewCryptoCurrencyDeposit(CurrencyCode currency)
        {
            Currency = currency;
        }
    }

    public class NewCryptoCurrencyWithdrawal
    {
        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCode Currency { get; set; }

        [JsonProperty("amount")]
        [JsonConverter(typeof(NumberToStringConverter))]
        public decimal Amount { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public NewCryptoCurrencyWithdrawal(CurrencyCode currency, decimal amount, string address)
        {
            Currency = currency;
            Amount = amount;
            Address = address;
        }
    }

    public class NewOrder
    {
        [JsonProperty("side")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderSide Side { get; private set; }

        [JsonProperty("instrument")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TickerSymbol Instrument { get; private set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType Type { get; private set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCode Currency { get; private set; }

        [JsonProperty("amount")]
        [JsonConverter(typeof(NumberToStringConverter))]
        public decimal Amount { get; private set; }

        [JsonProperty("display")]
        [JsonConverter(typeof(NumberToStringConverter))]
        public decimal Display { get; set; }

        [JsonProperty("price")]
        [JsonConverter(typeof(NumberToStringConverter))]
        public decimal Price { get; private set; }

        [JsonProperty("clientOrderIdentifier", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientOrderIdentifier { get; set; }

        public static NewOrder Buy(TickerSymbol instrument, CurrencyCode currency, decimal amount, decimal price)
        {
            return new NewOrder
            {
                Side = OrderSide.buy,
                Instrument = instrument,
                Type = OrderType.limit,
                Currency = currency,
                Price = price,
                Amount = amount,
                Display = amount
            };
        }

        public static NewOrder Sell(TickerSymbol instrument, CurrencyCode currency, decimal amount, decimal price)
        {
            return new NewOrder
            {
                Side = OrderSide.sell,
                Instrument = instrument,
                Type = OrderType.limit,
                Currency = currency,
                Price = price,
                Amount = amount,
                Display = amount
            };
        }

        private NewOrder()
        {
        }
    }

    public class Order
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("walletId")]
        public Guid WalletId { get; set; }

        [JsonProperty("side")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderSide Side { get; set; }

        [JsonProperty("instrument")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TickerSymbol Instrument { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCode Currency { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("amountFilled")]
        public decimal AmountFilled { get; set; }

        [JsonProperty("VolumeWeightedAveragePrice")]
        public decimal VolumeWeightedAveragePrice { get; set; }

        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus Status { get; set; }

        [JsonProperty("clientOrderIdentifier")]
        public string ClientOrderIdentifier { get; set; }
    }

    public class Wallet
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("balances")]
        public Balance[] Balances { get; set; }
    }

    public class Error
    {
        [JsonProperty("code")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorCodes Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("requestId")]
        public Guid RequestId { get; set; }
    }

    public class ValidationsError : Error
    {
        [JsonProperty("validationErrors")]
        public ValidationError[] ValidationErrors { get; set; }
    }

    public class ValidationError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    [Serializable]
    public class ItBitApiException : Exception
    {
        public Error Error { get; private set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Reason { get; set; }

        public ItBitApiException(Error error, HttpStatusCode httpStatusCode, string reason)
            : base(error.Description)
        {
            Error = error;
            HttpStatusCode = httpStatusCode;
            Reason = reason;
        }
    }

    internal class GetOrderBookMessageBuilder : IMessageBuilder
    {
        private readonly TickerSymbol _symbol;

        public GetOrderBookMessageBuilder(TickerSymbol symbol)
        {
            _symbol = symbol;
        }

        public RequestMessage Build()
        {
            var symbol = Enum.GetName(typeof(TickerSymbol), _symbol);

            return new RequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/v1/markets/{0}/order_book".Uri(symbol), UriKind.Relative)
            };
        }
    }

    internal class GetRecentTradesMessageBuilder : IMessageBuilder
    {
        private readonly TickerSymbol _symbol;
        private readonly int? _since;

        public GetRecentTradesMessageBuilder(TickerSymbol symbol, int? since)
        {
            _symbol = symbol;
            _since = since;
        }

        public RequestMessage Build()
        {
            var symbol = Enum.GetName(typeof(TickerSymbol), _symbol);

            var uri = _since.HasValue
                ? "/v1/markets/{0}/trades?since={1}".Uri(symbol, _since)
                : "/v1/markets/{0}/trades".Uri(symbol);

            return new RequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri, UriKind.Relative)
            };
        }
    }

    internal class GetTickerMessageBuilder : IMessageBuilder
    {
        private readonly TickerSymbol _symbol;

        public GetTickerMessageBuilder(TickerSymbol symbol)
        {
            _symbol = symbol;
        }

        public RequestMessage Build()
        {
            var symbol = Enum.GetName(typeof(TickerSymbol), _symbol);

            return new RequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/v1/markets/{0}/ticker".Uri(symbol), UriKind.Relative)
            };
        }
    }

    internal class CancelOrderMessageBuilder : IMessageBuilder
    {
        private readonly Guid _walletId;
        private readonly Guid _orderId;

        public CancelOrderMessageBuilder(Guid walletId, Guid orderId)
        {
            _walletId = walletId;
            _orderId = orderId;
        }

        public RequestMessage Build()
        {
            return new RequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("/v1/wallets/{0}/orders/{1}".Uri(_walletId, _orderId), UriKind.Relative)
            };
        }
    }

    internal class GetAllWalletsMessageBuilder : IMessageBuilder
    {
        private readonly Guid _userId;
        private readonly Page _page;

        public GetAllWalletsMessageBuilder(Guid userId)
            : this(userId, Page.Default)
        {
        }

        public GetAllWalletsMessageBuilder(Guid userId, Page page)
        {
            _userId = userId;
            _page = page;
        }

        public RequestMessage Build()
        {
            return new RequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/v1/wallets?userId={0}&{1}"
                    .Uri(_userId, _page.ToQueryString()), UriKind.Relative)
            };
        }
    }

    internal class GetOrderMessageBuilder : IMessageBuilder
    {
        private readonly Guid _walletId;
        private readonly Guid _orderId;

        public GetOrderMessageBuilder(Guid walletId, Guid orderId)
        {
            _walletId = walletId;
            _orderId = orderId;
        }

        public RequestMessage Build()
        {
            return new RequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/v1/wallets/{0}/orders/{1}".Uri(_walletId, _orderId), UriKind.Relative)
            };
        }
    }

    internal class GetOrdersMessageBuilder : IMessageBuilder
    {
        private readonly Guid _walletId;
        private readonly Page _page;
        private readonly string _instrument;
        private readonly string _status;

        public GetOrdersMessageBuilder(Guid walletId, Page page = null, TickerSymbol? instrument = null, OrderStatus? status = null)
        {
            _walletId = walletId;
            _page = page;
            _instrument = instrument.HasValue
                ? Enum.GetName(typeof(TickerSymbol), instrument)
                : null;
            _status = status.HasValue
                ? Enum.GetName(typeof(OrderStatus), status)
                : null;
        }

        public RequestMessage Build()
        {
            var qs = new List<string>();
            if (_page != null)
                qs.Add(_page.ToQueryString());
            if (_instrument != null)
                qs.Add("instrument={0}".Uri(_instrument));
            if (_status != null)
                qs.Add("status={0}".Uri(_status));

            var qss = qs.Any()
                ? "?" + string.Join("&", qs)
                : string.Empty;

            return new RequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/v1/wallets/{0}/orders{1}".Uri(_walletId, qss), UriKind.Relative)
            };
        }
    }

    internal class GetWalletBalanceMessageBuilder : IMessageBuilder
    {
        private readonly Guid _walletId;
        private readonly string _currencyCode;

        public GetWalletBalanceMessageBuilder(Guid walletId, CurrencyCode currencyCode)
        {
            _walletId = walletId;
            _currencyCode = Enum.GetName(typeof(CurrencyCode), currencyCode);
        }

        public RequestMessage Build()
        {
            return new RequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/v1/wallets/{0}/balances/{1}".Uri(_walletId, _currencyCode), UriKind.Relative)
            };
        }
    }

    internal class GetWalletMessageBuilder : IMessageBuilder
    {
        private readonly Guid _walletId;

        public GetWalletMessageBuilder(Guid walletId)
        {
            _walletId = walletId;
        }

        public RequestMessage Build()
        {
            return new RequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/v1/wallets/{0}".Uri(_walletId), UriKind.Relative)
            };
        }
    }

    internal class NewCryptoCurrencyDepositMessageBuilder : IMessageBuilder
    {
        private readonly Guid _walletId;
        private readonly NewCryptoCurrencyDeposit _deposit;

        public NewCryptoCurrencyDepositMessageBuilder(Guid walletId, NewCryptoCurrencyDeposit deposit)
        {
            _walletId = walletId;
            _deposit = deposit;
        }

        public RequestMessage Build()
        {
            return new RequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/v1/wallets/{0}/cryptocurrency_deposits".Uri(_walletId), UriKind.Relative),
                Content = JsonConvert.SerializeObject(_deposit, Formatting.None)
            };
        }
    }

    internal class NewCryptoCurrencyWithdrawalMessageBuilder : IMessageBuilder
    {
        private readonly Guid _walletId;
        private readonly NewCryptoCurrencyWithdrawal _withdrawal;

        public NewCryptoCurrencyWithdrawalMessageBuilder(Guid walletId, NewCryptoCurrencyWithdrawal withdrawal)
        {
            _walletId = walletId;
            _withdrawal = withdrawal;
        }

        public RequestMessage Build()
        {
            return new RequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/v1/wallets/{0}/cryptocurrency_withdrawals".Uri(_walletId), UriKind.Relative),
                Content = JsonConvert.SerializeObject(_withdrawal, Formatting.None)
            };
        }
    }

    internal class NewOrderMessageBuilder : IMessageBuilder
    {
        private readonly Guid _walletId;
        private readonly NewOrder _order;

        public NewOrderMessageBuilder(Guid walletId, NewOrder order)
        {
            _walletId = walletId;
            _order = order;
        }

        public RequestMessage Build()
        {
            var body = JsonConvert.SerializeObject(_order, Formatting.None);

            return new RequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/v1/wallets/{0}/orders".Uri(_walletId), UriKind.Relative),
                Content = body
            };
        }
    }

    public class Page
    {
        public int Number { get; private set; }
        public int Size { get; private set; }

        public static Page Default = new Page(1, 50);

        public static Page Create(int number, int size)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException("number", "page number must be greater or equal to 1");
            if (size < 1 || size > 50)
                throw new ArgumentOutOfRangeException("size", "page size must be beetwen 1 and 50");

            return new Page(number, size);
        }

        private Page(int number, int size)
        {
            Number = number;
            Size = size;
        }

        public string ToQueryString()
        {
            return "page={0}&perPage={1}".Uri(Number, Size);
        }
    }





} // end of namespace
