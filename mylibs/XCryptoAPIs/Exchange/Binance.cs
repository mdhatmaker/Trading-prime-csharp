using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Tools;
using static Tools.G;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using CryptoAPIs.Exchange.Clients.Binance;
using System.Threading;

namespace CryptoAPIs.Exchange
{
    // https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md
    // https://github.com/morpheums/Binance.API.Csharp.Client

    public enum BinanceOHLCInterval { _1m=1, _3m=3, _5m=5, _15m=15, _30m=30, _1h = 60, _2h = 120, _4h = 240, _6h = 360, _8h = 480, _12h = 720, _1d=1440, _3d=4320, _1w = 10800, _1M=43200 }

    public partial class Binance : BaseExchange, IExchangeWebSocket, IOrderExchange
    {
        public bool EnableLiveOrders { get; set; }

        public override string BaseUrl => "https://www.binance.com/api/v1";
        public override string Name => "BINANCE";
        public override CryptoExch Exch => CryptoExch.BINANCE;

        //private static readonly string BaseUrlV3 = "https://www.binance.com/api/v3";

        //wss://stream.binance.com:9443/ws/[symbol in lower case]@depth   (e.g. wss://stream.binance.com:9443/ws/ethbtc@depth)
        public override string WebsocketUrl => "wss://stream.binance.com:9443/ws";

        BinanceClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static Binance m_instance;
        public static Binance Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static Binance Create(string apikey="", string apisecret="")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new Binance(apikey, apisecret);
        }
        private Binance(string apikey, string apisecret) 
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            var apiclient = new Exchange.Clients.Binance.ApiClient(ApiKey, ApiSecret, "https://www.binance.com", "wss://stream.binance.com:9443/ws/", true);
            m_api = new Exchange.Clients.Binance.BinanceClient(apiclient, loadTradingRules: false);
            m_instance = this;

            Task.Run(() => TaskUpdateOrders());
        }

        public static Dictionary<string, string> ChartIntervals = new Dictionary<string, string>() { { "1m", "1" }, { "5m", "5" }, { "15m", "15" }, { "30m", "30" }, { "1h", "60" }, { "1d", "1440" } };

        private static readonly HttpClient m_httpClient = new HttpClient();

        private Dictionary<string, int> m_channelIds = new Dictionary<string, int>();


        private BinanceExchangeInfo m_exchangeInfo;
        private BinanceExchangeInfo ExchangeInfo { 
            get { 
                if (m_exchangeInfo == null) m_exchangeInfo = GetExchangeInfo();
                return m_exchangeInfo; 
            }}

        private Dictionary<string, BinanceAssetInfo> m_assetInfo;
        private Dictionary<string, BinanceAssetInfo> AssetInfo { 
            get {
                if (m_assetInfo == null) m_assetInfo = GetAssetInfo();
                return m_assetInfo;
            }}
        
        // This dictionary maps decimal "LOT_SIZE" Binance filters to "places" (decimal places to round quantity)
        private static Dictionary<decimal, int> m_binanceRoundLotSize = new Dictionary<decimal, int>() {
            { 0.00000001M, 8 }, { 0.00000010M, 7 }, { 0.00000100M, 6 }, { 0.00001000M, 5 }, { 0.00010000M, 4 },
            { 0.00100000M, 3 }, { 0.01000000M, 2 }, { 0.10000000M, 1 }, { 1.00000000M, 0 }
        };

        // Given a Binance symbol, return the number of decimal places a valid "LOT_SIZE" (qty) should be rounded
        public int RoundSize(string symbol)
        {
            return m_binanceRoundLotSize[AssetInfo[symbol].stepSize];
        }

        public override List<string> SymbolList    //bool forceUpdate=false)
        {
            get
            {
                if (m_symbolList == null)   // || forceUpdate == true)
                {
                    if (CurrencyPairs.Count > 0)
                    {
                        UpdateSymbolsFromCurrencyPairs();
                        return m_symbolList;
                    }

                    var bookTickers = GET<List<OrderBookTicker>>("https://api.binance.com/api/v1/ticker/allBookTickers");
                    if (bookTickers.Count > 0)
                    {
                        m_symbolList = new List<string>();
                        foreach (var pt in bookTickers)
                        {
                            m_symbolList.Add(pt.symbol);
                        }
                    }
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;    //.Clone() as List<string>;
            }
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();

            //var obtickers = await m_api.GetOrderBookTicker();
            var obtickers = AsyncHelpers.RunSync<IEnumerable<Clients.Binance.OrderBookTicker>>(() => m_api.GetOrderBookTicker());
            foreach (var obt in obtickers)
            {
                result[obt.Symbol] = new BinanceTicker(obt);
            }
            /*string json = QueryPublic("ticker/24hr");
            var tickers = JsonConvert.DeserializeObject<List<BinanceTicker>>(json);

            //var symbols = GetSymbolList();
            foreach (var ticker in tickers)
            {
                result[ticker.Symbol] = ticker;
            }*/

            // Sort List<custom_class> with a lambda expression
            // list.Sort((a, b) => a.date.CompareTo(b.date));

            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            //int limit = 100;
            // where limit is 5, 10, 20, 50, 100, 500, 1000 (WARNING: Using limit=0 can return a LOT of data)
            //var request = GET<BinanceOrderBook>(string.Format("{0}/depth/{1}?limit={2}", BaseUrl, symbol, limit));
            var request = GET<BinanceOrderBook>(string.Format("{0}/depth?symbol={1}", BaseUrl, symbol));
            return request as ZCryptoOrderBook;

            /*string json = GETJSON(string.Format("{0}/depth?symbol={1}", BaseUrl, symbol));
            var obj = JsonConvert.DeserializeObject<BinanceOrderBook>(json);
            return null;*/
        }

        /*public List<BinanceSymbolDetail> GetSymbolDetails()
        {
            var request = GET<List<BinanceSymbolDetail>>("{0}/symbols_details", BaseUrl);
            return request;
        }*/

        public override ZTicker GetTicker(string symbol)
        {
            //var pt = GET<PriceTicker>(string.Format("https://api.binance.com/api/v3/ticker/price?symbol={0}", symbol));
            //var obt = GET<OrderBookTicker>(string.Format("https://api.binance.com/api/v3/ticker/price?symbol={0}", symbol));
            var p24 = m_api.GetPriceChange24H(symbol).Result;
            return new BinanceTicker(p24.First());
        }

        public ZTicker GetBookTicker(string symbol)
        {
            //var pt = GET<PriceTicker>(string.Format("https://api.binance.com/api/v3/ticker/price?symbol={0}", symbol));
            //var obt = GET<OrderBookTicker>(string.Format("https://api.binance.com/api/v3/ticker/price?symbol={0}", symbol));
            //var p24 = m_api.GetPriceChange24H(symbol).Result;
            var ob = m_api.GetOrderBook(symbol, 5).Result;
            var bid = ob.Bids.First();
            var ask = ob.Asks.First();
            return new BinanceTicker(bid.Price, bid.Quantity, ask.Price, ask.Quantity, ob.LastUpdateId);
        }

        public BinanceExchangeInfo GetExchangeInfo()
        {
            var request = GET<BinanceExchangeInfo>("https://api.binance.com/api/v1/exchangeInfo");
            return request;
        }

        // Where pair like "TRXETH" and interval like "1m", "5m", "1h", ...
        public ZCandlestickMap GetCandlesticks(string pair, string interval)
        {
            var result = new ZCandlestickMap(CryptoExch.BINANCE, pair, interval);
            var res = GetOHLC(pair, interval, 500);
            foreach (var dp in res)
            {
                result.Add(dp.time, dp);
            }
            return result;
        }

        // Where symbol like "TRXETH"
        // m -> minutes; h -> hours; d -> days; w -> weeks; M -> months
        // 1m, 3m, 5m, 15m, 30m, 1h, 2h, 4h, 6h, 8h, 12h, 1d, 3d, 1w, 1M
        // Where limit is max 500
        public List<KlineCandleStickResponse> GetOHLC(string symbol, string interval="1h", int limit=125)
        {
            var request = GET<List<KlineCandleStickResponse>>(string.Format("https://api.binance.com/api/v1/klines?symbol={0}&interval={1}&limit={2}", symbol, interval, limit));
            return request;
        }

        // Latest price for all symbols
        public IEnumerable<SymbolPrice> GetAllPrices()
        {
            var res = m_api.GetAllPrices().Result;
            return res;            
        }

        public IEnumerable<Exchange.Clients.Binance.Order> GetCurrentOpenOrders(string symbol)
        {
            var res = m_api.GetCurrentOpenOrders(symbol).Result;
            return res;
        }

        public IEnumerable<Exchange.Clients.Binance.Order> GetAllOrders(string symbol, long? orderId = null, int limit = 500)
        {
            var res = m_api.GetAllOrders(symbol, orderId, limit).Result;
            return res;
        }

        public IEnumerable<Exchange.Clients.Binance.Trade> GetBinanceTrades(string symbol)
        {
            var res = m_api.GetTradeList(symbol).Result;
            return res;
        }

        // Aggregate trades that fill at the same time, at the same price, from the same order
        // Can provide some compression of trade data
        public IEnumerable<AggregateTrade> GetAggregateTrades(string symbol, int limit = 500)
        {
            var res = m_api.GetAggregateTrades(symbol, limit).Result;
            return res;
        }

        public Dictionary<string, BinanceAssetInfo> GetAssetInfo()
        {
            var result = new Dictionary<string, BinanceAssetInfo>();
            //m_api._tradingRules.RateLimits;
            foreach (var s in ExchangeInfo.symbols)
            {
                var bai = new BinanceAssetInfo(s);
                result[s.symbol] = bai;
            }
            return result;
        }

        public AccountInfo GetAccountInfo()
        {
            var res = m_api.GetAccountInfo().Result;
            return res;
        }

        public WithdrawHistory GetWithdrawHistory(string symbol)
        {
            //var drawStatus = WithdrawStatus.AwaitingApproval | WithdrawStatus.Cancelled | WithdrawStatus.Completed |
            // WithdrawStatus.EmailSent | WithdrawStatus.Failure | WithdrawStatus.Processing | WithdrawStatus.Rejected         
            //DateTime startTime, endTime;
            var res = m_api.GetWithdrawHistory(symbol).Result;  // , drawStatus, startTime, endTime)
            return res;
        }

        public DepositHistory GetDepositHistory(string symbol)
        {
            //var depStatus = DepositStatus.Pending | DepositStatus.Success
            //DateTime startTime, endTime;
            var res = m_api.GetDepositHistory(symbol).Result;  // , depStatus, startTime, endTime)
            return res;
        }

        // Output historical data for given symbol at each major available interval
        // Where symbol like "TRXETH"
        public void WriteHistorical(string symbol)
        {
            WriteHistorical(symbol, "1m");
            WriteHistorical(symbol, "5m");
            WriteHistorical(symbol, "15m");
            WriteHistorical(symbol, "30m");
            WriteHistorical(symbol, "1h");
            WriteHistorical(symbol, "1d");
        }

        // Output historical data for given symbol/interval combination
        // Where symbol like "TRXETH"
        // m -> minutes; h -> hours; d -> days; w -> weeks; M -> months
        // 1m, 3m, 5m, 15m, 30m, 1h, 2h, 4h, 6h, 8h, 12h, 1d, 3d, 1w, 1M
        public void WriteHistorical(string symbol, string interval)
        {
            string pathname = Folders.crypto_path(string.Format("{0}.{1}.DF.csv", symbol, interval));
            var hist = GetOHLC(symbol, interval);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathname))
            {
                file.WriteLine("DateTime,Open,High,Low,Close,Volume");
                foreach (var h in hist)
                {
                    string dt = h.OpenTime.ToString("s").Replace('T',' ');
                    file.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", dt, h.Open, h.High, h.Low, h.Close, h.Volume));
                }
            }
        }

        // Output historical data for ALL altcoins (at several different timeframes)
        // Where altSymbol like "TRX" (would output for "TRXBTC" and "TRXETH" and any other "TRX..." symbol)
        public void WriteAltCoinsHistorical()
        {
            var symbols = SymbolList;

            var query = from s in symbols
                        where s.EndsWith("BTC") || s.EndsWith("ETH")
                        select s.Substring(0, s.Length - 3);

            foreach (var altSymbol in query.Distinct())
            {
                dout(altSymbol);
                foreach (var s in symbols)
                {
                    if (s.StartsWith(altSymbol))
                    {
                        WriteHistorical(s);
                    }
                }
            }
        }

        public override void SubscribeOrderBookUpdates(ZCurrencyPair pair, bool subscribe)
        {
            StartWebSocket();
            SubscribeWebSocket();
        }

        public override void SubscribeOrderUpdates(ZCurrencyPair pair, bool subscribe)
        {
            m_subscribedPairs.Add(pair);
        }

        public override void SubscribeTickerUpdates(ZCurrencyPair pair, bool subscribe)
        {
            
        }

        
        private void TaskUpdateOrders()
        {
            while (true)
            {
                Thread.Sleep(200);

                if (m_subscribedPairs.Count == 0) continue;   // wait until one or more currency pairs are subscribed to

                m_orders.Clear();
                foreach (var zcp in m_subscribedPairs)
                {
                    var orders = GetWorkingOrders(zcp.BinanceSymbol);
                    m_orders.Add(orders);
                }

                // TODO: Only fire orders update event if there has been a change in one or more of the orders
                FireOrdersUpdate();
            }
        }

        #region ---------- IOrderExchange IMPLEMENTATION -----------------------------------------------------------------------
        public OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty)
        {
            if (!EnableLiveOrders)
            {
                dout("\nBinance::SubmitLimitOrder=> '{0}' {1} {2} {3}\n", pair, side, price, qty);
                return null;
            }
            var orderSide = (side == OrderSide.Buy) ? Clients.Binance.OrderSide.BUY : Clients.Binance.OrderSide.SELL;
            //var res = m_api.PostNewOrder(pair, qty, price, orderSide, Exchange.Clients.Binance.OrderType.LIMIT, TimeInForce.GTC).Result;
            var res = AsyncHelpers.RunSync<NewOrder>(() => m_api.PostNewOrder(pair, qty, price, orderSide, Exchange.Clients.Binance.OrderType.LIMIT, TimeInForce.GTC));
            return (res == null ? null : new OrderNew(pair, res));
        }

        public OrderCxl CancelOrder(string pair, string strOrderId)
        {
            if (!EnableLiveOrders)
            {
                dout("\nBinance::CancelOrder=> {0} [{1}]\n", pair, strOrderId);
                return null;
            }
            long orderId = long.Parse(strOrderId);
            //var res = m_api.CancelOrder(pair, orderId).Result;
            //var res = m_api.CancelOrder(wo.Pair, (long?)wo.Id, wo.OrderId).Result;
            var res = AsyncHelpers.RunSync<CanceledOrder>(() => m_api.CancelOrder(pair, orderId));
            return (res == null ? null : new OrderCxl(res));
        }

        public IEnumerable<ZOrder> GetWorkingOrders(string pair)
        {
            var result = new List<ZOrder>();

            if (pair == null)
            {
                foreach (var s in SymbolList)
                {
                    var res = m_api.GetCurrentOpenOrders(s).Result;
                    if (res != null)
                    {
                        foreach (var o in res)
                        {
                            result.Add(new ZOrder(o));
                        }
                    }
                }
            }
            else
            {
                var res = m_api.GetCurrentOpenOrders(pair).Result;
                foreach (var o in res)
                {
                    result.Add(new ZOrder(o));
                }
            }
            return result;
        }

        public IEnumerable<ZTrade> GetTrades(string pair)
        {
            var result = new List<ZTrade>();
            var res = m_api.GetTradeList(pair).Result;
            if (res != null)
            {
                foreach (var t in res)
                {
                    result.Add(new ZTrade(pair, t));
                }
            }
            return result;
        }

        public IDictionary<string, ZAccountBalance> GetAccountBalances()
        {
            var result = new Dictionary<string, ZAccountBalance>();
            var res = m_api.GetAccountInfo().Result;
            if (res != null)
            {
                foreach (var b in res.Balances)
                {
                    result.Add(b.Currency, new ZAccountBalance(b.Currency, b.Free, b.Locked));
                }
            }
            return result;
        }
        #endregion -------------------------------------------------------------------------------------------------------------


        //==============================================================================================================

        public class BinanceAssetInfo
        {
            // TODO: change these to GET accessors only
            public string symbol, baseAsset, quoteAsset, status;
            public int baseAssetPrecision, quoteAssetPrecision;
            public List<string> orderTypes;
            public decimal minPrice, maxPrice, tickSize;
            public decimal minQty, maxQty, stepSize;
            public decimal minNotional;

            public BinanceAssetInfo(SymbolDetail s)
            {
                symbol = s.symbol;
                baseAsset = s.baseAsset;
                baseAssetPrecision = s.baseAssetPrecision;
                quoteAsset = s.quoteAsset;
                quoteAssetPrecision = s.quotePrecision;
                status = s.status;
                orderTypes = s.orderTypes.ToList();

                foreach (var f in s.filters)
                {
                    string filterType = f.filterType;
                    if (filterType == "PRICE_FILTER")
                    {
                        minPrice = f.minPrice;
                        maxPrice = f.maxPrice;
                        tickSize = f.tickSize;
                    }
                    else if (filterType == "LOT_SIZE")
                    {
                        minQty = f.minQty;
                        maxQty = f.maxQty;
                        stepSize = f.stepSize;
                    }
                    else if (filterType == "MIN_NOTIONAL")
                    {
                        minNotional = f.minNotiona;
                    }
                }
            }

            public string ToDisplay()
            {
                string str = string.Format("{0,-10} {1,-4} {2,-4}  min_qty:{3} step_size:{4}     min_price:{5} tick_size:{6}     min_notional:{7}", symbol, baseAsset, quoteAsset, minQty, stepSize, minPrice, tickSize, minNotional);
                return str;
            }

        } // end of class BinanceAssetInfo

        public class ServerTime
        {
            public long serverTime { get; set; }
        }

        public class Ticker24hr
        {
            public string symbol { get; set; }
            public decimal priceChange { get; set; }
            public decimal priceChangePercent { get; set; }
            public decimal weightedAvgPrice { get; set; }
            public decimal prevClosePrice { get; set; }
            public decimal lastPrice { get; set; }
            public decimal lastQty { get; set; }
            public decimal bidPrice { get; set; }
            public decimal askPrice { get; set; }
            public decimal openPrice { get; set; }
            public decimal highPrice { get; set; }
            public decimal lowPrice { get; set; }
            public decimal volume { get; set; }
            public decimal quoteVolume { get; set; }
            public long openTime { get; set; }
            public long closeTime { get; set; }
            public int firstId { get; set; }            // first tradeId
            public int lastId { get; set; }             // last tradeId
            public int count { get; set; }              // trade count
        }

        public class KlineCandleSticksConverter : JsonConverter
        {
            private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var klineCandlesticks = JArray.Load(reader);
                return new KlineCandleStickResponse
                {
                    OpenTime = Epoch.AddMilliseconds((long)klineCandlesticks[0]), //.ElementAt(0)),
                    Open = (decimal)klineCandlesticks[1],  //.ElementAt(1),
                    High = (decimal)klineCandlesticks[2],  //.ElementAt(2),
                    Low = (decimal)klineCandlesticks[3],   //.ElementAt(3),
                    Close = (decimal)klineCandlesticks[4], //.ElementAt(4),
                    Volume = (decimal)klineCandlesticks[5],    //.ElementAt(5),
                    CloseTime = Epoch.AddMilliseconds((long)klineCandlesticks[6]),  //.ElementAt(6)),
                    QuoteAssetVolume = (decimal)klineCandlesticks[7],  //.ElementAt(7),
                    NumberOfTrades = (int)klineCandlesticks[8],    //.ElementAt(8),
                    TakerBuyBaseAssetVolume = (decimal)klineCandlesticks[9],   //.ElementAt(9),
                    TakerBuyQuoteAssetVolume = (decimal)klineCandlesticks[10],  //.ElementAt(10),
                };
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }
        }

        public interface IResponse
        {
        }

        /// <summary>
        /// Response following a call to the Get Klines Candlesticks endpoint
        /// </summary>
        [DataContract]
        [JsonConverter(typeof(KlineCandleSticksConverter))]
        public class KlineCandleStickResponse : ZCandlestick, IResponse
        {            
            [DataMember(Order = 1)]
            public DateTime OpenTime { get; set; }
            [DataMember(Order = 2)]
            public decimal Open { get; set; }
            [DataMember(Order = 3)]
            public decimal High { get; set; }
            [DataMember(Order = 4)]
            public decimal Low { get; set; }
            [DataMember(Order = 5)]
            public decimal Close { get; set; }
            [DataMember(Order = 6)]
            public decimal Volume { get; set; }
            [DataMember(Order = 7)]
            public DateTime CloseTime { get; set; }
            [DataMember(Order = 7)]
            public decimal QuoteAssetVolume { get; set; }
            [DataMember(Order = 7)]
            public int NumberOfTrades { get; set; }
            [DataMember(Order = 8)]
            public decimal TakerBuyBaseAssetVolume { get; set; }
            [DataMember(Order = 9)]
            public decimal TakerBuyQuoteAssetVolume { get; set; }

            public KlineCandleStickResponse() { }

            public override decimal open => Open;
            public override decimal high => High;
            public override decimal low => Low;
            public override decimal close => Close;
            public override decimal volume => Volume;
            public override int count => NumberOfTrades;
            public override DateTime time => CloseTime;
        }

        public class RateLimit
        {
            public string rateLimitType { get; set; }           // "REQUESTS" | "ORDERS"
            public string interval { get; set; }                // "MINUTE" | "SECOND" | "DAY"
            public int limit { get; set; }                      // 1200
        }

        public class SymbolDetail
        {
            public string symbol { get; set; }                  // "ETHBTC"
            public string status { get; set; }                  // "TRADING"
            public string baseAsset { get; set; }               // "ETH"
            public int baseAssetPrecision { get; set; }         // 8
            public string quoteAsset { get; set; }              // "BTC"
            public int quotePrecision { get; set; }             // 8
            public List<string> orderTypes { get; set; }        // ["LIMIT", "MARKET"]
            public bool icebergAllowed { get; set; }            // false
            public List<Filter> filters { get; set; }
        }

        public class Filter
        {
            public string filterType { get; set; }              // "PRICE_FILTER", "LOT_SIZE", "MIN_NOTIONAL"
            // PRICE_FILTER
            public decimal minPrice { get; set; }               // "0.00000100"
            public decimal maxPrice { get; set; }               // "100000.00000000"
            public decimal tickSize { get; set; }               // "0.00000100"
            // LOT_SIZE
            public decimal minQty { get; set; }                 // "0.00100000"
            public decimal maxQty { get; set; }                 // "100000.00000000"
            public decimal stepSize { get; set; }               // "0.00100000"
            // MIN_NOTIONAL
            public decimal minNotiona { get; set; }             // "0.00100000"
        }

        public class BinanceExchangeInfo
        {

            public string timezone { get; set; }                // "UTC"
            public long serverTime { get; set; }                // 1508631584636
            public List<RateLimit> rateLimits { get; set; }
            public List<string> exchangeFilters { get; set; }
            public List<SymbolDetail> symbols { get; set; }
        }

    } // end of class Binance

    //======================================================================================================================================

    public class PriceTicker
    {
        public string symbol { get; set; }
        public decimal price { get; set; }
    }

    public class OrderBookTicker
    {
        public string symbol { get; set; }
        public decimal bidPrice { get; set; }
        public decimal bidQty { get; set; }
        public decimal askPrice { get; set; }
        public decimal askQty { get; set; }
    }

    public class BinanceTicker : ZTicker
    {
        private decimal m_bid, m_bidQty, m_ask, m_askQty, m_last, m_high, m_low, m_volume;
        private string m_timestamp;

        public override decimal Bid { get { return m_bid; } }
        public override decimal BidSize { get { return m_bidQty; }}
        public override decimal Ask { get { return m_ask; } }
        public override decimal AskSize { get { return m_askQty; }}
        public override decimal Last { get { return m_last; } }
        public override decimal High { get { return m_high; } }
        public override decimal Low { get { return m_low; } }
        public override decimal Volume { get { return m_volume;  } }
        public override string Timestamp { get { return m_timestamp; } }

        public BinanceTicker(PriceChangeInfo pinfo)
        {
            if (pinfo != null)
            {
                m_bid = pinfo.BidPrice;
                m_ask = pinfo.AskPrice;
                m_last = pinfo.LastPrice;
                m_high = pinfo.HighPrice;
                m_low = pinfo.LowPrice;
                m_volume = pinfo.Volume;
                m_timestamp = pinfo.CloseTime.ToDateTimeString();
            }
        }

        public BinanceTicker(Exchange.Clients.Binance.OrderBookTicker obt)
        {            
            m_bid = obt.BidPrice;
            m_ask = obt.AskPrice;
            m_bidQty = obt.BidQuantity;
            m_askQty = obt.AskQuantity;
            m_timestamp = DateTime.Now.ToDateTimeString();
        }

        public BinanceTicker(decimal bid, decimal bidSize, decimal ask, decimal askSize, long timeid)
        {
            m_bid = bid;
            m_ask = ask;
            m_bidQty = bidSize;
            m_askQty = askSize;
            m_timestamp = timeid.ToDateTimeString();
        }
    } // end of class BinanceTicker

    /*public class BinancePrice
    {
        public string symbol { get; set; }
        public string price { get; set; }
    }*/

    public class BinanceSymbolDetail
    {
        public string pair { get; set; }
        public int price_precision { get; set; }
        public string initial_margin { get; set; }
        public string minimum_margin { get; set; }
        public string maximum_order_size { get; set; }
        public string minimum_order_size { get; set; }
        public string expiration { get; set; }
    } // end of class BinanceSymbolDetail

    [JsonConverter(typeof(BinanceOrderBookEntryConverter))]
    public class BinanceOrderBookEntry : ZCryptoOrderBookEntry
    {
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public string timestamp { get; set; }

        public override decimal Price { get { return price; } }
        public override decimal Amount { get { return amount; } }
        public override string Timestamp { get { return ""; } }
        //public DateTime Timestamp { get { return GDate.UnixTimeStampToDateTime(double.Parse(timestamp)); } }
    } // end of class BinanceOrderBookEntry

    public class BinanceOrderBook : ZCryptoOrderBook
    {
        public long lastUpdateId { get; set; }
        public List<BinanceOrderBookEntry> bids { get; set; }
        public List<BinanceOrderBookEntry> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("BIDS\n");
            int ix = 0;
            foreach (var b in bids)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}  {3}\n", ++ix, b.price, b.amount, b.timestamp));
            }
            sb.Append("ASKS\n");
            ix = 0;
            foreach (var a in asks)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}  {3}\n", ++ix, a.price, a.amount, a.timestamp));
            }
            return sb.ToString();
        }
    } // end of class BinanceOrderBook

    class BinanceOrderBookEntryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(BinanceOrderBookEntry));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            return new BinanceOrderBookEntry
            {
                price = (decimal)array[0],
                amount = (decimal)array[1],
                //Command = (string)array[2]
            };
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { throw new NotImplementedException(); }
    } // end of class BinanceOrderBookEntryConverter


} // end of namespace
