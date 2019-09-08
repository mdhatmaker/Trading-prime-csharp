using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Numerics;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Tools;
using static Tools.G;
using System.IO;
using System.Net;
using System.Threading;
using System.Runtime.Serialization;
using CryptoAPIs.Exchange.Clients.Kraken;

namespace CryptoAPIs.Exchange
{
    // https://www.kraken.com/help/api
    public class Kraken : BaseExchange, IOrderExchange
    {
        public bool EnableLiveOrders { get; set; }

        public override string BaseUrl { get { return "https://api.kraken.com/0/public"; } }
        public override string Name { get { return "KRAKEN"; } }
        public override CryptoExch Exch => CryptoExch.KRAKEN;

        KrakenClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static Kraken m_instance;
        public static Kraken Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static Kraken Create(string apikey="", string apisecret="")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new Kraken(apikey, apisecret);
        }
        private Kraken(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            m_api = new KrakenClient(ApiKey, ApiSecret, 1000);
            m_instance = this;
        }

        public static Dictionary<string, string> ChartIntervals = new Dictionary<string, string>() { { "1m", "1" }, { "5m", "5" }, { "15m", "15" }, { "30m", "30" }, { "1h", "60" }, { "1d", "1440" } };

        decimal m_leverage = 1.0M;

        private List<string> m_errors;



        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    if (CurrencyPairs.Count > 0)
                    {
                        UpdateSymbolsFromCurrencyPairs();
                        return m_symbolList;
                    }

                    var pairs = GetAssetPairs();
                    if (pairs != null)
                        m_symbolList = pairs.Keys.ToList();
                    else
                        m_symbolList = new List<string>();
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            var res = GET<KrakenResult<Dictionary<string, KrakenTicker>>>("https://api.kraken.com/0/public/Ticker?pair={0}", symbol);
            if (res.result == null)
            {
                ErrorMessage("Kraken GetTickers has a NULL result!");
                return null;
            }
            else
            {
                return res.result.Values.First();
            }
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            //if (symbols == null) symbols = GetSymbolList();         // passing null gets ALL tickers
            var symbols = SymbolList;
            //List<string> errors;
            Dictionary<string, KrakenTicker> tickers = GetTickers(symbols, out List<string> errors);
            if (tickers != null)
            {
                foreach (var k in tickers.Keys)
                {
                    result[k] = tickers[k];
                }
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            //https://api.kraken.com/0/public/Depth?pair=BCHUSD
            var res = GET<KrakenResult<Dictionary<string,KrakenOrderBook>>>("https://api.kraken.com/0/public/Depth?pair={0}", symbol);
            m_errors = res.error;
            return res.result.Values.First();
        }

        /*public string GetZSymbolPair(string symbol)
        {
            if (TranslateSymbols.ContainsKey(symbol))
                return TranslateSymbols[symbol];
            else
                return null;
        }*/

        // The assetPairs List should contain pairs such as "BCHUSD", "DASHEUR", etc. Use KrakenAssetPair class to get available.
        public Dictionary<string, KrakenTicker> GetTickers(List<string> assetPairs, out List<string> errors)
        {
            if (assetPairs == null)
            {
                errors = new List<string>();
                return new Dictionary<string, KrakenTicker>();
            }
            string pairs = string.Join(",", assetPairs);
            var res = GET<KrakenResult<Dictionary<string, KrakenTicker>>>("https://api.kraken.com/0/public/Ticker?pair={0}", pairs);
            if (res.result == null)
            {
                ErrorMessage("Kraken GetTickers has a NULL result!");
            }
            else
            {
                foreach (var k in res.result.Keys)
                {
                    var ticker = res.result[k];
                    ticker.UpdateProperties();
                }
            }
            errors = res.error;
            return res.result;
        }


        public GetServerTimeResult GetServerTime()
        {
            var res = m_api.GetServerTime();
            return res;
        }

        public Dictionary<string, AssetInfo> GetAssetInfo()
        {
            var res = m_api.GetAssetInfo();
            return res;
        }

        public Dictionary<string, AssetPair> GetAssetPairs()
        {
            var res = m_api.GetAssetPairs();
            return res;
        }

        public Dictionary<string, Ticker> GetKrakenTicker(string pair)
        {
            var res = m_api.GetTicker(pair);
            return res;
        }

        // Where pair like "BCHEUR" and interval like "1m", "5m", "1h", ...
        public ZCandlestickMap GetCandlesticks(string pair, string interval)
        {
            var result = new ZCandlestickMap(CryptoExch.KRAKEN, pair, interval);
            int minutes = int.Parse(ChartIntervals[interval]);
            var res = GetOHLC(pair, minutes);
            var ohlcList = res.Pairs[pair];
            foreach (var dp in ohlcList)
            {
                result.Add(dp.time, dp); 
            }
            return result;
        }

        // Where interval = minutes: 1 (default), 5, 15, 30, 60 (hour), 240 (4-hour), 1440 (24-hour), 10080 (7-day), 21600 (15-day)
        // and since = return committed OHLC data since given id (optional, exclusive)
        public GetOHLCResult GetOHLC(string pair, int? interval=default(int?), int? since=default(int?))
        {
            try
            {
                var res = m_api.GetOHLC(pair, interval, since);
                return res;
            }
            catch (Exception ex)
            {
                cout("[{0}] Error occurred attempting KRAKEN GetOHLC: {1}", DateTime.Now.ToShortTimeString(), ex.Message);
            }
            return null;
        }

        public Dictionary<string, Exchange.Clients.Kraken.OrderBook> GetKrakenOrderBook(string pair, int? count=default(int?))
        {
            var res = m_api.GetOrderBook(pair, count);
            return res;
        }

        public GetRecentTradesResult GetRecentTrades(string pair)
        {
            var res = m_api.GetRecentTrades(pair);
            return res;
        }

        public GetRecentSpreadResult GetRecentInsideMarkets(string pair)
        {
            var res = m_api.GetRecentSpread(pair);
            return res;
        }

        public Dictionary<string, decimal> GetAccountBalance()
        {
            var res = m_api.GetAccountBalance();
            return res;
        }

        public TradeBalanceInfo GetTradeBalance()
        {
            var res = m_api.GetTradeBalance();
            return res;
        }

        public Dictionary<string, OrderInfo> GetOpenOrders()
        {
            try
            {
                var res = m_api.GetOpenOrders();
                return res;
            }
            catch (Exception ex)
            {
                cout("[{0}] Error occurred attempting KRAKEN GetOpenOrders: {1}", DateTime.Now.ToShortTimeString(), ex.Message);
            }
            return null;
        }

        public Dictionary<string, OrderInfo> GetClosedOrders()
        {
            try
            {
                var res = m_api.GetClosedOrders();
                return res;
            }
            catch (Exception ex)
            {
                cout("[{0}] Error occurred attempting KRAKEN GetOHLC: {1}", DateTime.Now.ToShortTimeString(), ex.Message);
            }
            return null;
        }

        public Dictionary<string, OrderInfo> QueryOrders(IEnumerable<string> txids)
        {
            var res = new Dictionary<string, OrderInfo>();
            try
            {
                if (txids != null)
                {
                    res = m_api.QueryOrders(txids);
                    //return res;
                }
            }
            catch (Exception ex)
            {
                cout("[{0}] Error occurred attempting KRAKEN QueryOrders: {1}", DateTime.Now.ToShortTimeString(), ex.Message);
            }
            return res;
        }

        public GetTradesHistoryResult GetTradesHistory()
        {
            var res = m_api.GetTradesHistory();
            return res;
        }

        public Dictionary<string, TradeInfo> QueryTrades(IEnumerable<string> txids)
        {
            var res = new Dictionary<string, TradeInfo>();
            try
            {
                if (txids != null)
                {
                    res = m_api.QueryTrades(txids);
                    //return res;
                }
            }
            catch (Exception ex)
            {
                cout("[{0}] Error occurred attempting KRAKEN QueryTrades: {1}", DateTime.Now.ToShortTimeString(), ex.Message);
            }
            return res;
        }

        public Dictionary<string, TradeInfo> QueryTrades(string txid)
        {
            var res = new Dictionary<string, TradeInfo>();
            try
            {
                if (txid != null)
                {
                    res = m_api.QueryTrades(txid);
                    //return res;
                }
            }
            catch (Exception ex)
            {
                cout("[{0}] Error occurred attempting KRAKEN QueryTrades: {1}", DateTime.Now.ToShortTimeString(), ex.Message);
            }
            return res;
        }

        public Dictionary<string, PositionInfo> GetOpenPositions(IEnumerable<string> txids, bool? docalcs=default(bool?))
        {
            var res = new Dictionary<string, PositionInfo>();
            try
            {
                if (txids != null)
                {
                    res = m_api.GetOpenPositions(txids, docalcs);
                    //return res;
                }
            }
            catch (Exception ex)
            {
                cout("[{0}] Error occurred attempting KRAKEN GetOpenPositions: {1}", DateTime.Now.ToShortTimeString(), ex.Message);
            }
            return res;
        }

        // TODO: may have to check for null txids in this method (and other similar methods)
        // TODO: Add additional parameters to GetLedgers (AND other KrakenApi methods)
        public GetLedgerResult GetLedgers()
        {
            var res = m_api.GetLedgers();
            return res;
        }

        public Dictionary<string, LedgerInfo> QueryLedgers(IEnumerable<string> ids)
        {
            var res = m_api.QueryLedgers(ids);
            return res;
        }

        public GetTradeVolumeResult GetTradeVolume(IEnumerable<string> pairs=null)
        {
            var res = m_api.GetTradeVolume(pairs);
            return res;
        }

        // Override of existing GetTradeVolume method to take a single pair (rather than IEnumerable)
        public GetTradeVolumeResult GetTradeVolume(string pair)
        {
            var res = m_api.GetTradeVolume(new string[] { pair });
            return res;
        }



        public void SetLeverage(decimal leverage)
        {
            m_leverage = leverage;
        }

        #region ---------- IOrderExchange IMPLEMENTATION -----------------------------------------------------------------------
        public OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty)
        {
            if (!EnableLiveOrders)
            {
                cout("\nKRAKEN::SubmitLimitOrder=> '{0}' {1} {2} {3}\n", pair, side, price, qty);
                return null;
            }
            string orderSide = side.ToString().ToLower();   // "buy" or "sell"
            int userRef = (int)DateTime.Now.ToUnixTimestamp();
            var addorder = AddOrder(pair, orderSide, price, qty, m_leverage, userRef);
            return new OrderNew(pair, addorder);
        }

        public OrderCxl CancelOrder(string pair, string orderId)
        {
            if (!EnableLiveOrders)
            {
                cout("\nKRAKEN::CancelOrder=> {0} [{1}]\n", pair, orderId);
                return null;
            }
            var res = CancelOrder(orderId);
            return new OrderCxl(pair, res);
        }

        public IEnumerable<ZOrder> GetWorkingOrders(string pair)
        {
            var result = new List<ZOrder>();
            var res = m_api.GetOpenOrders();
            foreach (var o in res)
            {
                // o.Key, o.Value
                result.Add(new ZOrder(o.Key, o.Value));
            }
            return result;
        }

        public IEnumerable<ZTrade> GetTrades(string pair)
        {
            var result = new List<ZTrade>();
            var res = m_api.GetTradesHistory();
            foreach (var t in res.Trades)
            {   
                // t.Key, t.Value
                result.Add(new ZTrade(t.Key, t.Value));
            }
            return result;
        }

        public IDictionary<string, ZAccountBalance> GetAccountBalances()
        {
            var result = new Dictionary<string, ZAccountBalance>();
            var res = m_api.GetAccountBalance();            
            foreach (var b in res)
            {
                // b.Key, b.Value                
                result.Add(b.Key, new ZAccountBalance(b.Key, b.Value));
            }
            return result;
        }
        #endregion -------------------------------------------------------------------------------------------------------------

        // Where orderSide is "buy"|"sell" and userRef (optional) is a 32-bit signed number to server as a user order reference
        public AddOrderResult AddOrder(string pair, string orderSide, decimal price, decimal volume, decimal? leverage=default(decimal?), int? userRef=default(int?))
        {
            try
            {
                KrakenOrder order = new KrakenOrder();
                order.Pair = pair;              // "XXBTZUSD";
                order.OrderType = "limit";      // TODO: Look at all the available order types ("market", "stop-loss", "take-profit",...)
                order.Price = price;            // limit order price
                order.Type = orderSide;         // "buy" | "sell"
                order.Volume = volume;          // order volume in lots
                //order.StartTm = 0;              // default (zero) = now
                //order.ExpireTm = 0;             // default (zero) = no expiration
                order.UserRef = userRef;          // user reference id (32-bit signed number)
                //order.Validate = true;          // if true, validate inputs only (do not send order)
                //order.Price2 = 
                order.Leverage = leverage;      // amount of leverage desired
                // OFlags = comma-delimited list of order flags
                // "viqc" : volume in quote currency (not available for leveraged orders)
                // "fcib" : prefer fee in base currency
                // "fciq" : prefer fee in quote currency
                // "nompp" : no market price protection
                // "post" : post only order (available when OrderType is "limit")
                //order.OFlags = 
                // Close = optional closing order to add to system when order gets filled:
                //var close = new Dictionary<string, string>();
                //close["ordertype"] = "limit";
                //close["price"] = closePrice;
                //close["price2"] = secondaryClosePrice;
                //order.Close = close;
                cout("Add KRAKEN Order: {0}", order.ToString());
                var res = m_api.AddOrder(order);
                return res;
            }
            catch (Exception ex)
            {
                cout("[{0}] Error occurred while attempting to add KRAKEN order: {1}", DateTime.Now.ToShortTimeString(), ex.Message);
                return null;
            }
        }

        public CancelOrderResult CancelOrder(string txid)
        {
            var res = m_api.CancelOrder(txid);
            return res;
        }


        /*public Dictionary<string, ZCryptoSymbol> GetSymbols()
        {
            Dictionary<string, ZCryptoSymbol> result = new Dictionary<string, ZCryptoSymbol>();
            List<string> errors;
            var pairs = GetAssetPairs(out errors);
            foreach (string symbol in pairs.Keys)
            {
                result.Add(symbol, new ZCryptoSymbol(symbol, CryptoExchange.Kraken));
            }
            return result;
        }*/

       
        public void getit()
        {
            try
            {
                var api = new KrakenClient(ApiKey, ApiSecret, 500);

              
                string symbol = "XETHXXBT";

                var ticker = api.GetTicker(symbol);
                var t = ticker[symbol];
                var bid = t.Bid[0];
                var bidSize = t.Bid[2];
                var ask = t.Ask[0];
                var askSize = t.Ask[2];
                cout("{0}=> bid:{1}x{2} ask:{3}x{4}", symbol, bid, bidSize, ask, askSize);

                var pairs = api.GetAssetPairs();
                foreach (var s in pairs.Keys)
                {
                    if (s == symbol)
                        cout("{0}=> lot:{1} lot_decimals:{2} lot_multiplier:{3} pair_decimals:{4}", s, pairs[s].Lot, pairs[s].LotDecimals, pairs[s].LotMultiplier, pairs[s].PairDecimals);
                }
                var roundPrice = pairs[symbol].PairDecimals;
            }
            catch (Exception ex)
            {
                cout("[{0}] Error occurred attempting to submit Kraken order: {1}", DateTime.Now.ToShortTimeString(), ex.Message);
            }
            //var _params = new Dictionary<string, object>();
            //var res = ExecuteCommand<Dictionary<string, decimal>>("/0/private/Balance", _params);
        }


        /*// command like ...
        private T ExecuteCommand<T>(string command, Dictionary<string, object> _params)
        {
            var client = new RestClient("https://api.kraken.com");          // "https://poloniex.com/tradingApi");
            var request = new RestRequest(command, Method.POST);

            foreach (var _p in _params)
                request.AddParameter(_p.Key, _p.Value);

            // POST data:
            // nonce = always increasing unsigned 64 bit integer
            // otp = two-factor password(if two-factor enabled, otherwise not required)
            //request.AddParameter("command", command);
            request.AddParameter("nonce", GetCurrentHttpPostNonce());

            var _post_data = HttpPostString(request.Parameters);
            var _post_bytes = Encoding.UTF8.GetBytes(_post_data);
            var _post_hash = Encryptor.ComputeHash(_post_bytes);

            // POST Http headers:
            // API-Key = API key
            // API-Sign = Message signature using HMAC - SHA512 of (URI path + SHA256(nonce + POST data)) and base64 decoded secret API key
            var _signature = ConvertHexString(_post_hash);
            {
                request.AddHeader("API-Key", ApiKey);
                request.AddHeader("API-Sign", _signature);
            }

            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            var res = JsonConvert.DeserializeObject<T>(response.Content);
            return res;
        }

        private HMACSHA512 __encryptor = null;
        public HMACSHA512 Encryptor
        {
            get
            {
                if (__encryptor == null)
                    __encryptor = new HMACSHA512(Encoding.UTF8.GetBytes(ApiSecret));

                return __encryptor;
            }
        }

        private BigInteger CurrentHttpPostNonce
        {
            get;
            set;
        }

        private static DateTime UnixEpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        private string GetCurrentHttpPostNonce()
        {
            var _ne_nonce = new BigInteger(
                                    Math.Round(
                                        DateTime.UtcNow.Subtract(
                                            //CUnixTime.DateTimeUnixEpochStart
                                            UnixEpochStart
                                        )
                                        .TotalMilliseconds * 1000,
                                        MidpointRounding.AwayFromZero
                                    )
                                );

            if (_ne_nonce > CurrentHttpPostNonce)
            {
                CurrentHttpPostNonce = _ne_nonce;
            }
            else
            {
                CurrentHttpPostNonce += 1;
            }

            return CurrentHttpPostNonce.ToString(CultureInfo.InvariantCulture);
        }

        private string HttpPostString(List<Parameter> dictionary)
        {
            var _result = "";

            foreach (var _entry in dictionary)
            {
                var _value = _entry.Value as string;
                if (_value == null)
                    _result += "&" + _entry.Name + "=" + _entry.Value;
                else
                    _result += "&" + _entry.Name + "=" + _value.Replace(' ', '+');
            }

            return _result.Substring(1);
        }

        private string ConvertHexString(byte[] value)
        {
            var _result = "";

            for (var i = 0; i < value.Length; i++)
                _result += value[i].ToString("x2", CultureInfo.InvariantCulture);

            return _result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public new async Task<T> CallApiPostAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _request = CreateJsonRequest(endpoint, Method.POST);
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("nonce", GetCurrentHttpPostNonce());

                    if (args != null)
                    {
                        foreach (var a in args)
                            _params.Add(a.Key, a.Value);
                    }
                }

                foreach (var _p in _params)
                    _request.AddParameter(_p.Key, _p.Value);

                var _post_data = HttpPostString(_request.Parameters);
                var _post_bytes = Encoding.UTF8.GetBytes(_post_data);
                var _post_hash = Encryptor.ComputeHash(_post_bytes);

                var _signature = ConvertHexString(_post_hash);
                {
                    _request.AddHeader("Key", ApiKey);
                    _request.AddHeader("Sign", _signature);
                }
            }

            var _client = CreateJsonClient(__api_url);
            {
                var _tcs = new TaskCompletionSource<IRestResponse>();
                var _handle = _client.ExecuteAsync(_request, response =>
                {
                    _tcs.SetResult(response);
                });

                var _response = await _tcs.Task;
                return JsonConvert.DeserializeObject<T>(_response.Content);
            }
        }*/


    } // end of class Kraken

    //======================================================================================================================================

    public class KrakenResult<T>
    {
        public List<string> error { get; set; }
        public T result { get; set; }
    }

    /*public class KrakenServerTime
    {
        public long unixtime { get; set; }
        public string rfc1123 { get; set; }

        public override string ToString()
        {
            return "KrakenServerTime::" + Str(this);
        }
    } // end of class KrakenServerTime

    public class KrakenAssetInfo
    {
        public string altname { get; set; }
        public string aclass { get; set; }
        public int decimals { get; set; }
        public int display_decimals { get; set; }

        public override string ToString()
        {
            return "KrakenAssetInfo::" + Str(this);
        }
    } // end of class KrakenAssetInfo

    // Kraken Tradable Asset Pair
    public class KrakenAssetPair
    {
        public string altname { get; set; }
        public string aclass_base { get; set; }
        [JsonProperty(PropertyName = "base")]        // necessary for property names that are invalid in C#
        public string _base { get; set; }
        public string aclass_quote { get; set; }
        public string quote { get; set; }
        public string lot { get; set; }
        public int pair_decimals { get; set; }
        public int lot_decimals { get; set; }
        public int lot_multiplier { get; set; }
        public List<float> leverage_buy { get; set; }
        public List<float> leverage_sell { get; set; }
        public List<List<string>> fees { get; set; }
        public List<List<string>> fees_maker { get; set; }
        public string fee_volume_currency { get; set; }
        public int margin_call { get; set; }
        public int margin_stop { get; set; }

        //public int decimals { get; set; }
        //public int display_decimals { get; set; }

        public override string ToString()
        {
            return "KrakenAssetPair::" + Str(this);
        }
    } // end of class KrakenAssetPair*/

    public class PriceArray
    {
        public float Price { get; private set; }
        public int WholeLotVolume { get; private set; }
        public float Volume { get; private set; }

        public PriceArray(List<string> li)
        {
            this.Price = float.Parse(li[0]);
            this.WholeLotVolume = int.Parse(li[1]);
            this.Volume = float.Parse(li[2]);
        }
    }

    public class KrakenOrderBook : ZCryptoOrderBook
    {
        public override List<ZCryptoOrderBookEntry> Bids
        {
            get
            {
                var li = new List<ZCryptoOrderBookEntry>();
                foreach (var ja in bids)
                {
                    string price = ja.Value<string>(0);
                    string amount = ja.Value<string>(1);
                    long timestamp = ja.Value<long>(2);
                    li.Add(new ZCryptoOrderBookEntry(price, amount, timestamp));
                }
                return li;
            }
        }
        public override List<ZCryptoOrderBookEntry> Asks
        {
            get
            {
                var li = new List<ZCryptoOrderBookEntry>();
                foreach (var ja in asks)
                {
                    string price = ja.Value<string>(0);
                    string amount = ja.Value<string>(1);
                    long timestamp = ja.Value<long>(2);
                    li.Add(new ZCryptoOrderBookEntry(price, amount, timestamp));
                }
                return li;
            }
        }

        public List<JArray> bids { get; set; }
        public List<JArray> asks { get; set; }

    }

    // Kraken Ticker
    public class KrakenTicker : ZTicker
    {
        public List<string> a { get; set; }
        public List<string> b { get; set; }
        public List<string> c { get; set; }
        public List<string> v { get; set; }
        public List<string> p { get; set; }
        public List<int> t { get; set; }
        public List<string> l { get; set; }
        public List<string> h { get; set; }
        public float o { get; set; }

        private PriceArray m_ask;
        private PriceArray m_bid;
        private float m_lastTradePrice;
        private float m_lastTradeVolume;
        private float m_volumeToday;
        private float m_volume24h;
        private int m_tradesToday;
        private int m_trades24h;
        private float m_vwapToday;
        private float m_vwap24h;
        private float m_lowToday;
        private float m_low24h;
        private float m_highToday;
        private float m_high24h;
        private DateTime m_timestamp;

        /*public PriceArray Ask { get { return m_ask; } }
        public PriceArray Bid { get { return m_bid; } }
        public float LastTradePrice { get { return m_lastTradePrice; } }
        public float LastTradeVolume { get { return m_lastTradeVolume; } }
        public float VolumeToday { get { return m_volumeToday; } }
        public float Volume24h { get { return m_volume24h; } }
        public float VwapToday { get { return m_vwapToday; } }
        public float Vwap24h { get { return m_vwap24h; } }
        public int TradesToday { get { return m_tradesToday; } }
        public int Trades24h { get { return m_trades24h; } }
        public float LowToday { get { return m_lowToday; } }
        public float Low24h { get { return m_low24h; } }
        public float HighToday { get { return m_highToday; } }
        public float High24h { get { return m_high24h; } }
        public float OpenPrice { get { return this.o; } }*/

        public override decimal Bid { get { return (m_bid == null ? 0 : (decimal)m_bid.Price); } }
        public override decimal Ask { get { return (m_bid == null ? 0 : (decimal)m_ask.Price); } }
        public override decimal Last { get { return (decimal)m_lastTradePrice; } }
        public override decimal High { get { return (decimal)m_high24h; } }
        public override decimal Low { get { return (decimal)m_low24h; } }
        public override decimal Volume { get { return (decimal)m_volume24h; } }
        public override string Timestamp { get { return m_timestamp.ToString(); } }

        //public List<float> leverage_buy { get; set; }
        //public List<float> leverage_sell { get; set; }
        //public List<List<string>> fees { get; set; }
        //public List<List<string>> fees_maker { get; set; }

        /*public override string ToString()
        {
            return "KrakenTicker::" + Str(this);
        }*/

        public void UpdateProperties()
        {
            m_ask = new PriceArray(this.a);
            m_bid = new PriceArray(this.b);
            m_lastTradePrice = float.Parse(this.c[0]);
            m_lastTradeVolume = float.Parse(this.c[1]);
            m_volumeToday = float.Parse(this.v[0]);
            m_volume24h = float.Parse(this.v[1]);
            m_vwapToday = float.Parse(this.p[0]);
            m_vwap24h = float.Parse(this.p[1]);
            m_tradesToday = t[0];
            m_trades24h = t[1];
            m_lowToday = float.Parse(this.l[0]);
            m_low24h = float.Parse(this.l[1]);
            m_highToday = float.Parse(this.h[0]);
            m_high24h = float.Parse(this.h[1]);
            m_timestamp = DateTime.Now;
        }

    } // end of class KrakenTicker

    /*public class KrakenOHLC
    {
        public string Symbol { get; set; }
        public long Timestamp { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string VWAP { get; set; }
        public string Volume { get; set; }
        public int Count { get; set; }
    } // end of class KrakenOHLC*/



} // end of NAMESPACE
