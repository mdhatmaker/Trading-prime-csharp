using Newtonsoft.Json;
//using PusherClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using static Tools.G;
using CryptoAPIs.Exchange.Clients.Bitstamp;

namespace CryptoAPIs.Exchange
{
    // https://www.bitstamp.net/api/

    public class Bitstamp : BaseExchange, IOrderExchange
    {
        public bool EnableLiveOrders { get; set; }

        public override string BaseUrl { get { return "https://www.bitstamp.net/api/v2"; } }
        public override string Name { get { return "BITSTAMP"; } }
        public override CryptoExch Exch => CryptoExch.BITSTAMP;

        BitstampClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static Bitstamp m_instance;
        public static Bitstamp Create(ApiCredentials creds, string clientId = null, Tools.Logging.ILogFactory factory = null)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret, clientId, factory);
        }
        public static Bitstamp Create(string apikey = "", string apisecret = "", string clientId = "", Tools.Logging.ILogFactory factory = null)
        {
            if (m_instance != null)
                return m_instance;
            else
            {
                if (factory == null)
                    return new Bitstamp(apikey, apisecret, clientId, new Tools.Logging.NLogFactory());
                else
                    return new Bitstamp(apikey, apisecret, clientId, factory);
            }
        }
        private Bitstamp(string apikey, string apisecret, string clientId, Tools.Logging.ILogFactory factory)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            var auth = new RequestAuthenticator(ApiKey, ApiSecret, clientId);
            m_api = new BitstampClient(auth, factory);
            m_instance = this;
        }

        /*  // SAMPLE USAGE
            ILogFactory logFactory = new NLogFactory();
            IRequestAuthenticator ra = new RequestAuthenticator(_apiKey, _apiSecret, _clientId);
            IBitstampClient client = new BitstampClient(ra, logFactory);

            var ticker = client.GetTicker();

            Console.WriteLine("Last bitcoin market value: " + ticker.last);

            Console.ReadLine();
        */

        /*private static Pusher _pusher;
        private static Channel _tradesChannel;
        private static Channel _ordersChannel;
        private static Channel _orderbookChannel;*/

        private static List<string> currency_pairs = new List<string>() {
                "btcusd", "btceur",
                "eurusd",
                "xrpusd", "xrpeur", "xrpbtc",
                "ltcusd", "ltceur", "ltcbtc",
                "ethusd", "etheur", "ethbtc"
        };

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

                    m_symbolList = currency_pairs;
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;
            }
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var symbols = SymbolList;
            foreach (var s in symbols)
            {
                result[s] = GetTicker(s);
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string currency_pair)
        {
            string url = BaseUrl + "/order_book/{0}/";
            var book = GET<BitstampOrderBook>(url, currency_pair);
            return book as ZCryptoOrderBook;
        }

        /*public List<BitstampTicker> GetTickers()
        {
            List<BitstampTicker> li = new List<BitstampTicker>();
            foreach (string pair in currency_pairs)
            {
                var obj = GetTicker(pair);
                obj.symbol = pair;
                li.Add(obj);
            }
            return li;
        }*/

        public override ZTicker GetTicker(string currency_pair)
        {
            string url = BaseUrl + "/ticker/{0}/";
            var ticker = GET<BitstampTicker>(url, currency_pair);
            return ticker;
        }

        public List<ZAccountBalance> GetBalances()
        {
            var res = m_api.GetBalance();
            var balances = new List<ZAccountBalance>();
            balances.Add(new ZAccountBalance("BTC",  decimal.Parse(res.btc_available), decimal.Parse(res.btc_reserved)));
            balances.Add(new ZAccountBalance("EUR", decimal.Parse(res.eur_available), decimal.Parse(res.eur_reserved)));
            balances.Add(new ZAccountBalance("USD", decimal.Parse(res.usd_available), decimal.Parse(res.usd_reserved)));
            return balances;
        }

        /*public void TestWebSocket()
        {
            _pusher = new Pusher("de504dc5763aeef9ff52");
            //_pusher = new Pusher("YOUR_APP_KEY");
            _pusher.ConnectionStateChanged += _pusher_ConnectionStateChanged;
            _pusher.Error += _pusher_Error;

            _tradesChannel = _pusher.Subscribe("live_trades");
            _tradesChannel.Bind("trade", UpdateLiveTrades);

            _ordersChannel = _pusher.Subscribe("live_orders");
            _ordersChannel.Bind("order_deleted", AddToWsQueue);
            _ordersChannel.Bind("order_created", AddToWsQueue);
            _ordersChannel.Bind("order_changed", AddToWsQueue);

            _orderbookChannel = _pusher.Subscribe("order_book");
            _orderbookChannel.Bind("data", UpdateOrderBook);

            _pusher.Connect();
        }*/

        private static void UpdateLiveTrades(dynamic obj)
        {
            dout(obj);
        }

        private static void AddToWsQueue(dynamic obj)
        {
            dout(obj);
        }

        private static void UpdateOrderBook(dynamic obj)
        {
            dout(obj);
        }

        /*static void _pusher_ConnectionStateChanged(object sender, ConnectionState state)
        {
            Console.WriteLine("Connection state: " + state.ToString());
        }

        static void _pusher_Error(object sender, PusherException error)
        {
            Console.WriteLine("Pusher Error: " + error.ToString());
        }*/

        #region ---------- IOrderExchange IMPLEMENTATION -----------------------------------------------------------------------
        public OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty)
        {
            if (!EnableLiveOrders)
            {
                cout("\nBITSTAMP::SubmitLimitOrder=> '{0}' {1} {2} {3}\n", pair, side, price, qty);
                return null;
            }
            BuySellResponse res;
            if (side == OrderSide.Buy)
                res = m_api.Buy((double)qty, (double)price);
            else
                res = m_api.Sell((double)qty, (double)price);
            return new OrderNew(pair, res);
        }

        public OrderCxl CancelOrder(string pair, string orderId)
        {
            if (!EnableLiveOrders)
            {
                cout("\nBITSTAMP::CancelOrder=> {0} [{1}]\n", pair, orderId);
                return null;
            }
            var res = m_api.CancelOrder(orderId);
            return new OrderCxl(pair, res);
        }

        public IEnumerable<ZOrder> GetWorkingOrders(string pair)
        {
            var result = new List<ZOrder>();
            var res = m_api.GetOpenOrders();
            if (res != null)
            {
                foreach (var or in res)
                {
                    result.Add(new ZOrder(or));
                }
            }
            return result;
        }

        public IEnumerable<ZTrade> GetTrades(string pair)
        {
            var result = new List<ZTrade>();
            var res = m_api.GetTransactions(offset: 0, limit: 500);
            if (res != null)
            {
                foreach (var tr in res)
                {
                    result.Add(new ZTrade(pair, tr));
                }
            }
            return result;
        }

        public IDictionary<string, ZAccountBalance> GetAccountBalances()
        {
            var result = new Dictionary<string, ZAccountBalance>();
            var res = m_api.GetBalance();
            if (res != null)
            {
                result.Add("BTC", new ZAccountBalance("BTC", decimal.Parse(res.btc_available ?? "0"), decimal.Parse(res.btc_reserved ?? "0")));
                result.Add("USD", new ZAccountBalance("USD", decimal.Parse(res.usd_available ?? "0"), decimal.Parse(res.usd_reserved ?? "0")));
                result.Add("EUR", new ZAccountBalance("EUR", decimal.Parse(res.eur_available ?? "0"), decimal.Parse(res.eur_reserved ?? "0")));
            }
            return result;
        }
        #endregion -------------------------------------------------------------------------------------------------------------

    } // end of class Bitstamp

    //======================================================================================================================================

    public class BitstampOrderBook : ZCryptoOrderBook
    {
        public string timestamp { get; set; }
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }
    }

    public class BitstampTicker : ZTicker, IDataRow
    {
        //static public string[] Columns = { "Symbol", "Last", "Bid", "Ask", "VWAP", "Volume", "Open", "High", "Low", "Timestamp" };

        //public string Key { get { return symbol; } set {; } }

        //[JsonProperty(PropertyName = "15m")]        // necessary for property names that begin with a digit (or other invalid starting character)
        public string symbol { get; set; }
        public string last { get; set; }
        public string bid { get; set; }
        public string ask { get; set; }
        public string vwap { get; set; }
        public string volume { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string timestamp { get; set; }

        public override decimal Bid { get { return decimal.Parse(bid ?? "0"); } }
        public override decimal Ask { get { return decimal.Parse(ask ?? "0"); } }
        public override decimal Last { get { return decimal.Parse(last ?? "0"); } }
        public override decimal High { get { return decimal.Parse(high ?? "0"); } }
        public override decimal Low { get { return decimal.Parse(low ?? "0"); } }
        public override decimal Volume { get { return decimal.Parse(volume ?? "0"); } }
        public override string Timestamp { get { return DateTime.Now.ToString(); } }

        public static List<string> currency_pairs = new List<string>() { "btcusd", "btceur", "eurusd", "xrpusd", "xrpeur", "xrpbtc", "ltcusd", "ltceur", "ltcbtc", "ethusd", "etheur", "ethbtc" };

        private string[] cellValues = new string[10];

        /*public override string ToString()
        {
            return "BitstampTicker: " + Str(this);
            //return string.Format("15m: {0}   last: {1}   buy: {2}   sell: {3}   currency: {4}", _15m, last, buy, sell, currency_symbol);
        }*/

        /*public string[] GetCells()
        {
            cellValues[0] = symbol;
            cellValues[1] = last;
            cellValues[2] = bid;
            cellValues[3] = ask;
            cellValues[4] = vwap;
            cellValues[5] = volume;
            cellValues[6] = open;
            cellValues[7] = high;
            cellValues[8] = low;
            cellValues[9] = timestamp;
            return cellValues;
        }*/

        public static List<BitstampTicker> GetList()
        {
            List<BitstampTicker> li = new List<BitstampTicker>();
            foreach (string pair in currency_pairs)
            {
                var obj = GetObject(pair);
                obj.symbol = pair;
                li.Add(obj);
            }
            return li;
        }

        public static BitstampTicker GetObject(string currency_pair)
        {
            //string url = Bitstamp.BaseUrl + "/ticker/{0}/";
            string url = "https://www.bitstamp.net/api/v2/ticker/{0}/";
            var ticker = GET<BitstampTicker>(url, currency_pair);
            return ticker;
        }


    } // end of CLASS BitstampTicker


} // end of NAMESPACE
