using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using RestSharp;
using Tools;
using static Tools.G;
using System.Security.Cryptography;
using System.Numerics;
using System.Globalization;
//using PureSocketCluster;
using CryptoAPIs.Exchange.Clients.Poloniex;

namespace CryptoAPIs.Exchange
{
    // https://poloniex.com/support/api/
    public partial class Poloniex : BaseExchange, IOrderExchange, IExchangeWebSocket
    {
        public bool EnableLiveOrders { get; set; }

        public override string BaseUrl { get { return "https://poloniex.com/public"; } }
        public override string Name { get { return "POLONIEX"; } }
        public override CryptoExch Exch => CryptoExch.POLONIEX;

        public override string WebsocketUrl => "wss://api2.poloniex.com";

        PoloniexClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static Poloniex m_instance;
        public static Poloniex Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static Poloniex Create(string apikey = "", string apisecret = "")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new Poloniex(apikey, apisecret);
        }
        private Poloniex(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            m_api = new PoloniexClient(ApiKey, ApiSecret);
            m_instance = this;
        }


        public int Depth = 20;

      
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

                    m_symbolList = GetPoloniexTickers().Keys.ToList();
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            var tickers = GetAllTickers().Result;
            if (tickers.TryGetValue(symbol, out ZTicker t))
                return t;
            else
                return null;
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            //var symbols = GetSymbolList();
            var tickers = GetPoloniexTickers();
            foreach (var k in tickers.Keys)
            {
                result[k] = tickers[k];
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            //https://poloniex.com/public?command=returnOrderBook&currencyPair=BTC_NXT&depth=10
            string url = BaseUrl + "?command=returnOrderBook&currencyPair={0}&depth={1}";
            var book = GET<PoloniexOrderBook>(new ItemConverter(), url, symbol, this.Depth);
            return book as ZCryptoOrderBook;
        }

        public Dictionary<string, PoloniexTicker> GetPoloniexTickers()
        {
            string url = BaseUrl + "?command=returnTicker";
            return GET<Dictionary<string, PoloniexTicker>>(url);
        }

        public void GetChartData(string currencyPair = "BTC_XMR", long startTimestamp = 1405699200, long endTimestamp = 9999999999, int period = 14400)
        {
            string url = string.Format("https://poloniex.com/public?command=returnChartData&currencyPair={0}&start={1}&end={2}&period={3}", currencyPair, startTimestamp, endTimestamp, period);
            /*var chartData = GET<PoloniexChartData>(null, url, currencyPair)
            {

            }*/
        }

        public Dictionary<string, Exchange.Clients.Poloniex.UserBalance> GetBalances()
        {
            var res = m_api.GetBalances().Result;
            return res;
        }

        // Gets past 7 days of data for the given currency pair symbol and chart period (bar interval)
        public List<PublicChart> GetOHLC(string pair, ChartPeriod period = ChartPeriod.Minutes5)
        {
            var currency_pair = CurrencyPair.Parse(pair);
            var end_time = DateTime.Now;
            var start_time = end_time.Subtract(TimeSpan.FromDays(7));
            var res = m_api.GetChartData(currency_pair, period, start_time, end_time).Result;
            return res;
        }

        private CurrencyPair GetCurPair(string pair)
        {
            //var cp = ZCurrencyPair.GetLeftRight(pair, CryptoExch.POLONIEX);
            var zcp = ZCurrencyPair.FromSymbol(pair, CryptoExch.POLONIEX);
            Clients.Poloniex.CurrencyPair curpair = new CurrencyPair(currency_name: zcp.Right, coin_name: zcp.Left);   // NOTICE these are in reverse order (currency, coin)
            return curpair;
        }

        public override void SubscribeOrderBookUpdates(ZCurrencyPair pair, bool subscribe)
        {
            StartWebSocket(new string[] { pair.Symbol });
            SubscribeWebSocket();
        }

        #region ---------- IOrderExchange IMPLEMENTATION -----------------------------------------------------------------------
        public OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty)
        {
            if (!EnableLiveOrders)
            {
                cout("\nPOLONIEX::SubmitLimitOrder=> '{0}' {1} {2} {3}\n", pair, side, price, qty);
                return null;
            }
            var orderSide = (side == OrderSide.Buy ? Clients.Poloniex.OrderType.Buy : Clients.Poloniex.OrderType.Sell);
            //var res = m_api.PlaceOrder(GetCurPair(pair), orderSide, price, qty).Result;
            var res = AsyncHelpers.RunSync<ulong>(() => m_api.PlaceOrder(GetCurPair(pair), orderSide, price, qty));
            return new OrderNew(pair, res);
        }

        public OrderCxl CancelOrder(string pair, string orderId)
        {
            if (!EnableLiveOrders)
            {
                cout("\nPOLONIEX::CancelOrder=> {0} [{1}]\n", pair, orderId);
                return null;
            }
            var res = m_api.DeleteOrder(GetCurPair(pair), ulong.Parse(orderId)).Result;
            return new OrderCxl(pair, res);
        }

        // Pass pair=null to get working orders for ALL pairs
        public IEnumerable<ZOrder> GetWorkingOrders(string pair)
        {
            var result = new List<ZOrder>();
            if (pair == null)
            {
                foreach (var s in SymbolList)
                {
                    var res = m_api.OpenOrders(GetCurPair(s)).Result;
                    foreach (var to in res)
                    {
                        result.Add(new ZOrder(pair, to));
                    }
                }
            }
            else
            {
                var res = m_api.OpenOrders(GetCurPair(pair)).Result;
                foreach (var to in res)
                {
                    result.Add(new ZOrder(pair, to));
                }
            }
            return result;
        }

        public IEnumerable<ZTrade> GetTrades(string pair)
        {
            var result = new List<ZTrade>();
            var dt1 = DateTime.Now;                             // end is current time
            var dt0 = dt1.Subtract(TimeSpan.FromHours(24));     // start is back 24 hours
            var res = m_api.GetTrades(GetCurPair(pair), dt0, dt1).Result;
            foreach (var to in res)
            {
                result.Add(new ZTrade(pair, to));
            }
            return result;
        }

        public IDictionary<string, ZAccountBalance> GetAccountBalances()
        {
            var result = new Dictionary<string, ZAccountBalance>();
            var res = m_api.GetBalances().Result;
            foreach (var kv in res)
            {
                result.Add(kv.Key, new ZAccountBalance(kv.Key, kv.Value.QuoteAvailable, kv.Value.BitcoinValue - kv.Value.QuoteAvailable));
            }
            return result;
        }
        #endregion -------------------------------------------------------------------------------------------------------------


        /*//private readonly PureSocketClusterSocket socket;
        private static readonly ManualResetEvent resetEvent = new ManualResetEvent(false);
        private PureSocketClusterSocket m_pureSocket;

        public void TestWebsocket()
        {
            //var ws = new ClientWebSocket();
            //ws.ConnectAsync(new Uri("wss://api2.poloniex.com"), CancellationToken.None).RunSynchronously();
            //ws.SendAsync()

            //this.credentials = credentials;
            //this.socket = new PureSocketClusterSocket("wss://sc-02.coinigy.com/socketcluster/");
            m_pureSocket = new PureSocketClusterSocket("wss://api2.poloniex.com");

            m_pureSocket.OnMessage += Socket_OnMessage;
            m_pureSocket.OnError += Socket_OnError;
            m_pureSocket.OnClosed += Socket_OnClosed;
            m_pureSocket.OnOpened += Socket_OnOpened;

            m_pureSocket.Connect();

            resetEvent.WaitOne();
        }

        private void Socket_OnError(Exception ex)
        {
            cout("Socket_OnError=> {0}", ex.Message);
        }

        private void Socket_OnMessage(string message)
        {
            cout("Socket_OnMessage=> {0}", message);
        }

        private void Socket_OnClosed(WebSocketCloseStatus reason)
        {
            cout("Socket_OnClosed");
        }

        private void Socket_OnOpened()
        {
            cout("Socket_OnOpened");

            //ws.send(json.dumps({ 'command':'subscribe','channel':1001}))
            //ws.send(json.dumps({ 'command':'subscribe','channel':1002}))
            //ws.send(json.dumps({ 'command':'subscribe','channel':1003}))
            //ws.send(json.dumps({ 'command':'subscribe','channel':'BTC_ETH'}))
            m_pureSocket.Subscribe("1001");
            m_pureSocket.Subscribe("1002");
            m_pureSocket.Subscribe("1003");
            m_pureSocket.Subscribe("BTC_ETH");
            //socket.Emit("auth", this.credentials, ack: (string name, object error, object data) =>
            //{
            //    // We can now start listening to trade information
            //});
        }*/


    } // end of class Poloniex

    //======================================================================================================================================

    public class PoloniexOrderBook : ZCryptoOrderBook
    {
        public List<List<Item>> asks { get; set; }
        public List<List<Item>> bids { get; set; }
        public string isFrozen { get; set; }
        public long seq { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }
    }

    public class PoloniexTicker : ZTicker
    {
        public string last { get; set; }
        public string lowestAsk { get; set; }
        public string highestBid { get; set; }
        public string percentChange { get; set; }
        public string baseVolume { get; set; }
        public string quoteVolume { get; set; }

        public override decimal Bid { get { return decimal.Parse(highestBid); } }
        public override decimal Ask { get { return decimal.Parse(lowestAsk); } }
        public override decimal Last { get { return decimal.Parse(last); } }
        public override decimal High { get { return decimal.Parse("0"); } }
        public override decimal Low { get { return decimal.Parse("0"); } }
        public override decimal Volume { get { return decimal.Parse(quoteVolume); } }
        public override string Timestamp { get { return DateTime.Now.ToString(); } }
    } // end of class PoloniexTicker

    class Client
    {
        private static object consoleLock = new object();
        private const int sendChunkSize = 256;
        private const int receiveChunkSize = 256;
        private const bool verbose = true;
        private static readonly TimeSpan delay = TimeSpan.FromMilliseconds(30000);

        public static void Test()
        {
            Thread.Sleep(1000);
            Connect("wss://api.poloniex.com").Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static async Task Connect(string uri)
        {
            ClientWebSocket webSocket = null;

            try
            {
                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                await Task.WhenAll(Receive(webSocket), Send(webSocket));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();
                Console.WriteLine();

                lock (consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("WebSocket closed.");
                    Console.ResetColor();
                }
            }
        }
        static UTF8Encoding encoder = new UTF8Encoding();

        private static async Task Send(ClientWebSocket webSocket)
        {

            //byte[] buffer = encoder.GetBytes("{\"op\":\"blocks_sub\"}"); //"{\"op\":\"unconfirmed_sub\"}");
            byte[] buffer = encoder.GetBytes("{\"op\":\"unconfirmed_sub\"}");
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

            while (webSocket.State == WebSocketState.Open)
            {
                LogStatus(false, buffer, buffer.Length);
                await Task.Delay(delay);
            }
        }

        private static async Task Receive(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[receiveChunkSize];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    LogStatus(true, buffer, result.Count);
                }
            }
        }

        private static void LogStatus(bool receiving, byte[] buffer, int length)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = receiving ? ConsoleColor.Green : ConsoleColor.Gray;
                //Console.WriteLine("{0} ", receiving ? "Received" : "Sent");

                if (verbose)
                    Console.WriteLine(encoder.GetString(buffer));

                Console.ResetColor();
            }
        }
    } // end of class


    class ItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Item));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            return new Item
            {
                amount = (string)array[0],
                price = (int)array[1]
            };
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class Item
    {
        public string amount { get; set; }
        public float price { get; set; }
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


    public class PoloniexOpenOrder
    {
        //{"orderNumber":"120466","type":"sell","rate":"0.025","amount":"100","total":"2.5"}
        public int orderNumber { get; set; }
        public string type { get; set; }
        public decimal rate { get; set; }
        public decimal amount { get; set; }
        public decimal total { get; set; }
    }

} // end of namespace
