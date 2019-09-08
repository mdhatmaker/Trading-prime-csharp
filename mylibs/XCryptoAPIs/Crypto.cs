using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Timers;
//using PusherClient;
using CryptoAPIs.Exchange;
using CryptoAPIs.ExchangeX;
using Tools;
using static Tools.G;
using Tools.Messaging;
using System.IO;
using CryptoAPIs.Exchange.Clients.Kraken;
using CryptoAPIs.Exchange.Clients.Bittrex;
using CryptoAPIs.Exchange.Clients.BitFlyer;
using System.Diagnostics;


namespace CryptoAPIs
{
    // OneChain cryptocurrencies: "BCH", "BTC", "BTG", "DCR", "ETC", "ETH", "LTC", "NEO", "XMR", "ZEC" (and maybe "XBT" for ItBit, "BCC" for alternate BitCoinCash)
    // OneChain exchanges: Bitstamp, Bitfinex, Kraken, ItBit, GDAX, Bittrex, BitFlyer, Poloniex, Binance (added: B2C2)

    public static class Crypto
    {
        public static event OrderBookUpdateHandler UpdateOrderBookEvent;
        public static event OrdersUpdateHandler UpdateOrdersEvent;
        public static event TickerUpdateHandler UpdateTickerEvent;

        /*// Static constructor is called at most one time, before any
        // instance constructor is invoked or member is accessed.
        static Crypto()
        {
            //PopulateExchanges(securityFilename);
        }*/

        public static Binance binance { get; private set; }
        public static Bitstamp bitstamp { get; private set; }
        public static Bittrex bittrex { get; private set; }
        public static GDAX gdax { get; private set; }
        public static HitBTC hitbtc { get; private set; }
        public static ItBit itbit { get; private set; }
        public static Kraken kraken { get; private set; }
        public static Poloniex poloniex { get; private set; }
        public static Bitfinex bitfinex { get; private set; }
        public static BitFlyer bitflyer { get; private set; }
        public static Gemini gemini { get; private set; }
        /*public static B2C2 b2c2 { get; private set; }
        public static Bleutrade bleutrade { get; private set; }
        public static BlinkTrade blinktrade { get; private set; }
        public static OkCoin okcoin { get; private set; }
        public static Bithumb bithumb { get; private set; }
        public static BTCC btcc { get; private set; }*/

        private static bool m_initialized = false;          // set to true when the exchanges are initialized

        public static ZCurrencyPairMap CurrencyPairs => m_currencyPairs;

        public static Dictionary<CryptoExch, BaseExchange> Exchanges = new Dictionary<CryptoExch, BaseExchange>();
        public static ApiKeyMap ApiKeys { get { return m_security.ApiKeys; }}

        public static System.Timers.Timer aggregateTimer = new System.Timers.Timer();
        public static ZOrderBook orderBook = new ZOrderBook();

        public static BaseExchange[] ExchangeList = {
            binance, bitfinex, bitflyer, bitstamp, bittrex,
            gdax, Gemini.Instance, itbit, kraken, poloniex
        };

        private static SortedDictionary<string, HashSet<BaseExchange>> m_exchangesForCurrencyPairs = new SortedDictionary<string, HashSet<BaseExchange>>();
        private static ZCurrencyPairMap m_currencyPairs = new ZCurrencyPairMap();

        private static void PopulateExchanges(string securityJsonFilename)
        {
            m_security = new ApiSecurity(securityJsonFilename);
            foreach (var exch in m_security.ApiKeys.Keys)
            {
                var c = m_security.ApiKeys[exch];
                //Console.WriteLine("{0} '{1}' '{2}'", exch, c.ApiKey, c.ApiSecret);
            }

            ApiCredentials creds;

            /*creds = m_security.ApiKeys["B2C2"];                             // BINANCE
            b2c2 = B2C2.Create(creds);
            Exchanges[CryptoExch.B2C2] = b2c2;*/

            creds = m_security.ApiKeys["BINANCE"];                          // BINANCE
            binance = Binance.Create(creds);
            Exchanges[CryptoExch.BINANCE] = binance;

            creds = m_security.ApiKeys["BITFINEX"];                         // BITFINEX
            bitfinex = Bitfinex.Create(creds);
            Exchanges[CryptoExch.BITFINEX] = bitfinex;

            creds = m_security.ApiKeys["BITFLYER"];                         // BITFLYER
            bitflyer = BitFlyer.Create(creds);
            Exchanges[CryptoExch.BITFLYER] = bitflyer;

            creds = m_security.ApiKeys["BITSTAMP"];                         // BITSTAMP
            bitstamp = Bitstamp.Create(creds, "client_id");
            Exchanges[CryptoExch.BITSTAMP] = bitstamp;

            creds = m_security.ApiKeys["BITTREX"];                          // BITTREX
            bittrex = Bittrex.Create(creds);
            Exchanges[CryptoExch.BITTREX] = bittrex;

            creds = m_security.ApiKeys["GDAX"];                             // GDAX
            gdax = GDAX.Create(creds, "mickey+mouse");
            Exchanges[CryptoExch.GDAX] = gdax;

            creds = m_security.ApiKeys["HITBTC"];                           // HITBTC
            hitbtc = HitBTC.Create(creds);
            Exchanges[CryptoExch.HITBTC] = hitbtc;

            creds = m_security.ApiKeys["ITBIT"];                            // ITBIT
            itbit = ItBit.Create(creds);
            Exchanges[CryptoExch.ITBIT] = itbit;

            creds = m_security.ApiKeys["KRAKEN"];                           // KRAKEN
            kraken = Kraken.Create(creds);
            Exchanges[CryptoExch.KRAKEN] = kraken;

            creds = m_security.ApiKeys["POLONIEX"];                         // POLONIEX
            poloniex = Poloniex.Create(creds);
            Exchanges[CryptoExch.POLONIEX] = poloniex;

            /*creds = m_security.ApiKeys["BLEUTRADE"];                        // BLEUTRADE
            bleutrade = Bleutrade.Create(creds);
            Exchanges[CryptoExch.BLEUTRADE] = bleutrade;

            creds = m_security.ApiKeys["BLINKTRADE"];                       // BLINKTRADE
            blinktrade = BlinkTrade.Create(creds);
            Exchanges[CryptoExch.BLINKTRADE] = blinktrade;*/

            gemini = Gemini.Instance;
            Exchanges[CryptoExch.GEMINI] = Gemini.Instance;

            // For each exchange, subscribe to the various events
            foreach (var exchange in Exchanges.Values)
            {
                exchange.UpdateOrderBookEvent += Exchanges_UpdateOrderBookEvent;
                exchange.UpdateOrdersEvent += Exchanges_UpdateOrdersEvent;
                exchange.UpdateTickerEvent += Exchanges_UpdateTickerEvent;

                /*var ws = exchange as IExchangeWebSocket;
                if (ws != null)
                    ws.StartWebSocket(null);*/
            }

            /*Exchanges[CryptoExch.VAULTORO] = Vaultoro.Instance;
            Exchanges[CryptoExch.CHANGELLY] = Changelly.Instance;
            Exchanges[CryptoExch.BITHUMB] = Bithumb.Instance;
            Exchanges[CryptoExch.BITMEX] = BitMEX.Instance;
            Exchanges[CryptoExch.BITSQUARE] = Bitsquare.Instance;
            Exchanges[CryptoExch.BTCC] = BTCC.Instance;
            //Exchanges[CryptoExch.BTER] = BTER.Instance;
            Exchanges[CryptoExch.CEX] = Cex.Instance;
            //Exchanges[CryptoExch.COINIGY] = Coinigy.Instance;
            Exchanges[CryptoExch.COINONE] = Coinone.Instance;
            Exchanges[CryptoExch.GATEIO] = GateIO.Instance;
            Exchanges[CryptoExch.HUOBI] = Huobi.Instance;
            Exchanges[CryptoExch.KORBIT] = Korbit.Instance;
            Exchanges[CryptoExch.KUCOIN] = Kucoin.Instance;
            Exchanges[CryptoExch.OKCOIN] = OkCoin.Instance;
            Exchanges[CryptoExch.OKEX] = OKEx.Instance;
            Exchanges[CryptoExch.WEX] = Wex.Instance;
            Exchanges[CryptoExch.XCRYPTO] = XCrypto.Instance;*/
        }


        #region ---------- SUBSCRIPTION EVENTS --------------------------------------------------------------------------------------------
        private static void Exchanges_UpdateOrderBookEvent(object sender, OrderBookUpdateArgs e)
        {
            UpdateOrderBookEvent?.Invoke(sender, e);
        }

        private static void Exchanges_UpdateOrdersEvent(object sender, OrdersUpdateArgs e)
        {
            UpdateOrdersEvent?.Invoke(sender, e);
        }

        private static void Exchanges_UpdateTickerEvent(object sender, TickersUpdateArgs e)
        {
            UpdateTickerEvent?.Invoke(sender, e);
        }

        public static void SubscribeOrderBookUpdates(ZCurrencyPair pair, bool subscribe = true)
        {
            foreach (var exchange in Exchanges.Values)
            {
                if (exchange.HasPair(pair))
                {
                    dout("Crypto::SubscribeOrderBookUpdates=> {0}", exchange.Name);
                    exchange.SubscribeOrderBookUpdates(pair, true);
                }
            }
        }

        public static void SubscribeOrderUpdates(ZCurrencyPair pair, bool subscribe = true)
        {
            foreach (var exchange in Exchanges.Values)
            {
                if (exchange.HasPair(pair))
                {
                    dout("Crypto::SubscribeOrderUpdates=> {0}", exchange.Name);
                    exchange.SubscribeOrderUpdates(pair, subscribe);
                }
            }
        }

        public static void SubscribeTickerUpdates(ZCurrencyPair pair, bool subscribe = true)
        {
            foreach (var exchange in Exchanges.Values)
            {
                if (exchange.HasPair(pair))
                {
                    dout("Crypto::SubscribeTickerUpdates=> {0}", exchange.Name);
                    exchange.SubscribeTickerUpdates(pair, subscribe);
                }
            }
        }

        public static void SubscribeTradeUpdates(ZCurrencyPair pair, bool subscribe = true)
        {
            foreach (var exchange in Exchanges.Values)
            {
                if (exchange.HasPair(pair))
                {
                    dout("Crypto::SubscribeTradeUpdates=> {0}", exchange.Name);
                    exchange.SubscribeTradeUpdates(pair, subscribe);
                }
            }
        }
        #endregion ------------------------------------------------------------------------------------------------------------------------

        public static void InitializeExchanges()
        {
            if (m_initialized) return;

            PopulateExchanges(Settings.Instance["SECURITY_FILENAME"]);

            // Populate the CurrencyPairs property of each exchange
            var loadedPairs = LoadCurrencyPairs();
            foreach (var exch in Exchanges.Keys)
            {
                if (Exchanges.ContainsKey(exch) && loadedPairs.ContainsKey(exch))
                {
                    Exchanges[exch].CurrencyPairs = loadedPairs[exch];
                    m_currencyPairs.Add(Exchanges[exch].CurrencyPairs);
                    dout("Loaded CurrencyPairs for exchange: {0}", exch);
                    foreach (var pair in loadedPairs[exch].Pairs)
                    {
                        string symbol = pair.Symbol;
                        if (!m_exchangesForCurrencyPairs.ContainsKey(symbol))
                        {
                            m_exchangesForCurrencyPairs[symbol] = new HashSet<BaseExchange>();
                        }
                        m_exchangesForCurrencyPairs[symbol].Add(Exchanges[exch]);
                    }
                }
            }

            m_initialized = true;
        }

        // Given a (OneChain) pair symbol (ex: "BTC_USD")
        // Return a set of all exchanges that support the corresponding currency pair
        public static HashSet<BaseExchange> GetExchangesForPair(string pairSymbol)
        {
            if (!m_exchangesForCurrencyPairs.ContainsKey(pairSymbol))
                return null;
            else
                return m_exchangesForCurrencyPairs[pairSymbol];
        }

        // Load the CURRENCY_PAIRS file as a ZCurrencyPairMap (one for each exchange)
        public static IDictionary<CryptoExch, ZCurrencyPairMap> LoadCurrencyPairs()
        {
            Dictionary<CryptoExch, ZCurrencyPairMap> result = new Dictionary<CryptoExch, ZCurrencyPairMap>();
            try
            {
                string pathname = Folders.system_path("CURRENCY_PAIRS.DF.csv");
                using (var f = new StreamReader(pathname))
                {
                    string line;
                    // Skip first line of file (column headers)
                    f.ReadLine();
                    // Read currency pair information from file
                    while ((line = f.ReadLine()) != null)
                    {
                        //"Exchange,Symbol,ExchangeSymbol,ExchangeLeft,ExchangeRight"
                        var split = line.Split(',');
                        var exch = Crypto.GetExch(split[0]);
                        var zcp = new ZCurrencyPair(split[1], split[2], split[3], split[4]);
                        if (!result.ContainsKey(exch))
                            result.Add(exch, new ZCurrencyPairMap());
                        result[exch].Add(zcp);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("Crypto::LoadCurrencyPairs=> {0}", ex.Message);
            }
            return result;
        }

        // Within this method, put any tasks that should be run periodically (i.e. daily)
        static void RunDaily()
        {
            CoinMarketCap.WriteToFile();            // output to a file the CoinMarketCap rankings and other data
        }


        // Convert a string exchange name (ex: "Binance") to a CryptoExch enum
        public static CryptoExch GetExch(string name)
        {
            return (CryptoExch) Enum.Parse(typeof(CryptoExch), name, ignoreCase: true);
        }

        // Given a string exchange name (ex: "Binance"), return the BaseExchange object
        public static BaseExchange GetExchange(string name)
        {
            return Exchanges[GetExch(name)];
        }

        static void HandleKline(Exchange.Clients.Binance.Websocket.KlineMessage msg)
        {
            Console.WriteLine("{0} {1} {2} {3} {4}", msg.Symbol, msg.KlineInfo.Open, msg.KlineInfo.High, msg.KlineInfo.Low, msg.KlineInfo.Close);
        }
                
        private static TelegramBot m_tbot;
        private static ApiSecurity m_security;
        public static async void Test()   //Dictionary<string, string> settings)
        {
            // Read all the ApiKeys/ApiSecrets and use
            // them to create the crypto exchanges
            //PopulateExchanges(Settings.Instance["SECURITY_FILENAME"]);
            InitializeExchanges();

            // --------------- FUN STARTS HERE!!! --------------- 

            //TestAccountBalances();

            // Create the TelegramBot object (singleton) using its ApiKey
            //TelegramBot.Create(m_security.ApiKeys["TELEGRAM"].ApiKey);
            //TelegramBot.Instance.AddChatId(410181031);                      // TODO: A better way to figure out these "correct" chat Ids

            //BacktestBinanceTrades();
            //TestBittrex();
            //TestGDAX();
            //TestBinance(); 
            //TestPoloniex();
            //TestKraken();
            //TestBitFlyer();
            //TestCoinbaseBittrex("BTC-NEO");
            //TestExchangeSymbols();
            //TestBittrex("BTC-NEO", 0.001M);
            //TestKraken("BTCEUR", 0.001M);
            //TestOrderBooks();
            //TestWebSockets();
            //TestCryptoCompare();

            //Task.Run(TestAll);

            TestWebsockets();

            cout("(back in MAIN...trade threads continue to run)");
            for (; ; ) Thread.Sleep(5000);
        }

        public static async Task TestBitstampAsync()
        {
            var tickers = await bitstamp.GetAllTickers();
            tickers.Keys.ToList().ForEach(k => cout("BITSTAMP: {0} {1}", k, tickers[k]));
        }

        public static async Task TestBitfinexAsync()
        {
            var tickers = await bitfinex.GetAllTickers();
            tickers.Keys.ToList().ForEach(k => cout("BITFINEX: {0} {1}", k, tickers[k]));
        }

        public static async Task TestKrakenAsync()
        {
            var tickers = await kraken.GetAllTickers();
            tickers.Keys.ToList().ForEach(k => cout("KRAKEN: {0} {1}", k, tickers[k]));
        }

        public static async Task TestItbitAsync()
        {
            var tickers = await itbit.GetAllTickers();
            tickers.Keys.ToList().ForEach(k => cout("ITBIT: {0} {1}", k, tickers[k]));
        }

        public static async Task TestGdaxAsync()
        {
            var tickers = await gdax.GetAllTickers();
            tickers.Keys.ToList().ForEach(k => cout("GDAX: {0} {1}", k, tickers[k]));
        }

        public static async Task TestBittrexAsync()
        {
            var tickers = await gdax.GetAllTickers();
            tickers.Keys.ToList().ForEach(k => cout("BITTREX: {0} {1}", k, tickers[k]));
        }

        public static async Task TestBitflyerAsync()
        {
            var tickers = await bitflyer.GetAllTickers();
            tickers.Keys.ToList().ForEach(k => cout("BITFLYER: {0} {1}", k, tickers[k]));
        }

        public static async Task TestPoloniexAsync()
        {
            var tickers = await poloniex.GetAllTickers();
            tickers.Keys.ToList().ForEach(k => cout("POLONIEX: {0} {1}", k, tickers[k]));
        }

        public static async Task TestBinanceAsync()
        {
            var tickers = await binance.GetAllTickers();
            tickers.Keys.ToList().ForEach(k => cout("BINANCE: {0} {1}", k, tickers[k]));
        }

        // Test these exchanges: Bitstamp, Bitfinex, Kraken, ItBit, GDAX, Bittrex, BitFlyer, Poloniex, Binance
        public static async Task TestAll()
        {
            //var wst = new WebsocketTests();
            //wst.Subscribe_And_Listen();

            Task.Run(TestBitstampAsync);
            Task.Run(TestBitfinexAsync);
            Task.Run(TestKrakenAsync);
            Task.Run(TestItbitAsync);
            Task.Run(TestGdaxAsync);
            Task.Run(TestBittrexAsync);
            Task.Run(TestBitflyerAsync);
            Task.Run(TestPoloniexAsync);
            Task.Run(TestBinanceAsync);
        }

        public static void TestCryptoCompare()
        {
            CryptoCompare.Instance.GetCoinSnapshots();
        }

        public static void TestKrakenTrades()
        {
            var k1 = new KrakenTradeMACD(kraken, "BTCEUR");    // symbol="BTCEUR"
            k1.StartTrade(0.001M, 60, true);                   // unitsize=0.001M, barinterval=60-minute, tradelive=true/false
            Thread.Sleep(7000);
            return;

            /*var k2 = new KrakenTradeMACD(kraken);
            k2.StartTrade("ZECXBT", 0.0301M);
            Thread.Sleep(7000);
            var k3 = new KrakenTradeMACD(kraken);
            k3.StartTrade("ETHXBT", 0.002M);
            Thread.Sleep(7000);
            var k4 = new KrakenTradeMACD(kraken);
            k4.StartTrade("BCHEUR", 0.010M);
            Thread.Sleep(7000);
            var k5 = new KrakenTradeMACD(kraken);
            k5.StartTrade("XMREUR", 0.100M);
            Thread.Sleep(7000);
            var k6 = new KrakenTradeMACD(kraken);
            k6.StartTrade("XRPEUR", 30.0M);
            Thread.Sleep(7000);
            var k7 = new KrakenTradeMACD(kraken);
            k7.StartTrade("LTCUSD", 0.020M);
            Thread.Sleep(7000);*/
        }

        public static void GetSymbolsOnMultipleExchanges()
        {
            var dict = new Dictionary<string, List<ZCurrencyPair>>();
            //var exchanges = new string[] { "BINANCE", "BITFINEX", "BITSTAMP", "HITBTC", "GEMINI", "ITBIT", "BITTREX", "GDAX", "POLONIEX" };
            var exchanges = new string[] { "BINANCE", "BITFINEX", "BITSTAMP", "HITBTC", "GEMINI", "ITBIT", "BITTREX", "POLONIEX" };
            foreach (var e in exchanges)
            {
                cout("\n---{0}---", e);
                var exchEnum = (CryptoExch)Enum.Parse(typeof(CryptoExch), e);
                var exch = Exchanges[exchEnum];
                foreach (var s in exch.SymbolList)
                {
                    var zcp = ZCurrencyPair.FromSymbol(s, exchEnum);
                    //cout(zcp.Symbol);
                    Console.Write("{0}, ", zcp.Symbol);
                    if (dict.ContainsKey(zcp.Symbol))
                    {
                        dict[zcp.Symbol].Add(zcp);
                    }
                    else
                    {
                        dict[zcp.Symbol] = new List<ZCurrencyPair>() { zcp };
                    }
                }
                Console.WriteLine();
            }
            cout("\n");
            foreach (var s in dict.Keys)
            {
                int n = dict[s].Count();
                if (n >= 4)
                {
                    decimal maxbid = decimal.MinValue;
                    decimal minask = decimal.MaxValue;
                    cout("---{0} ({1})", s, n);
                    foreach (var zcp in dict[s])
                    {
                        /*var ticker = Exchanges[zcp.Exchange].GetTicker(zcp.ExchangeSymbol);
                        cout("{0} {1}", zcp.Exchange.ToString(), ticker.ToString());
                        if (ticker.Bid > maxbid) maxbid = ticker.Bid;
                        if (ticker.Ask < minask) minask = ticker.Ask;*/
                    }
                    cout("b:{0}  a:{1}     {2}\n", maxbid, minask, maxbid > minask ? (maxbid - minask).ToString() : "");
                }
            }
            return;
        }

        public static void TestBittrex()
        {
            var d = bittrex.GetAllTickers().Result;

            var ticker = d["USDT-BTC"];
            ticker.Display();

            bittrex.EnableLiveOrders = true;
            var price = ticker.Ask + (ticker.Ask * 0.25M);
            bittrex.SubmitLimitOrder("USDT-BTC", OrderSide.Sell, price, .0001M);

            var balances = bittrex.GetAccountBalances();
            balances.Values.ToList().ForEach(b => cout("{0} free:{1} locked:{2}", b.Asset, b.Free, b.Locked));

            var orders = bittrex.GetWorkingOrders("USDT-BTC");
            if (orders.Count() == 0)
            {
                cout("(no working BITTREX orders)");
            }
            else
            {
                orders.ToList().ForEach(o => cout(o.ToDisplay()));
                foreach (var o in orders)
                {
                    bittrex.CancelOrder(o.ExchangeSymbol, o.OrderId);
                }
            }
            return;
        }

        public static void TestGDAX()
        {
            //var d = gdax.GetAllTickers();

            gdax.EnableLiveOrders = true;
            //gdax.SubmitLimitOrder("BCH-USD", OrderSide.Sell, 1380, .01M);

            var balances = gdax.GetAccountBalances();
            balances.Values.ToList().ForEach(b => cout("{0} free:{1} locked:{2}", b.Asset, b.Free, b.Locked));

            var orders = gdax.GetWorkingOrders("BCH-USD");
            if (orders.Count() == 0)
            {
                cout("(no working GDAX orders)");
            }
            else
            {
                orders.ToList().ForEach(o => cout(o.ToDisplay()));
                foreach (var o in orders)
                {
                    gdax.CancelOrder(o.ExchangeSymbol, o.OrderId);
                }
            }
            return;
        }

        public static void TestPoloniex()
        {
            //var d = poloniex.GetAllTickers();

            poloniex.EnableLiveOrders = true;
            var onew = poloniex.SubmitLimitOrder("BTC_DASH", OrderSide.Sell, 0.06338489M, .01M);

            var balances = poloniex.GetAccountBalances();
            balances.Values.ToList().ForEach(b => cout("{0} free:{1} locked:{2}", b.Asset, b.Free, b.Locked));

            var orders = poloniex.GetWorkingOrders("BTC_DASH");
            if (orders.Count() == 0)
            {
                cout("(no working POLONIEX orders)");
            }
            else
            {
                orders.ToList().ForEach(o => cout(o.ToDisplay()));
                foreach (var o in orders)
                {
                    poloniex.CancelOrder(o.ExchangeSymbol, o.OrderId);
                }
            }

            /*var msg1 = new ConsoleMessenger();
            var p1 = new PoloniexTradeMACD(poloniex, "BTC_XRP", msg1);
            p1.StartTrade(0.001M, 5, false);*/
        }


        public static void TestBitFlyer()
        {
            
            // Test the real-time API
            var api = new RealtimeApi();
            api.Subscribe<Exchange.Clients.BitFlyer.Ticker>(PubnubChannel.TickerFxBtcJpy, OnReceive, OnConnect, OnError);
            Console.ReadKey();
        }

        static void OnConnect(string message)
        {
            Console.WriteLine(message);
        }

        static void OnReceive(Exchange.Clients.BitFlyer.Ticker data)
        {
            Console.WriteLine(data);
        }

        static void OnError(string message, Exception ex)
        {
            Console.WriteLine(message);
            if (ex != null)
            {
                Console.WriteLine(ex);
            }
        } 


        public static void TestCoinbaseBittrex(string pair)
        {
            /*KrakenOHLC(pair, false);                        // get the initial set of OHLC data (and calculate latest MACD values)
            var hist = m_indicator.Last()[2];               // get the most recent "histogram" MACD value (watch this going positive/negative)
            if (hist > 0)                                   // set our initial "position" according to current "histogram" MACD value
                m_pos = 1;
            else
                m_pos = -1;*/

            //var creds = m_security.ApiKeys["COINBASE"];
            //var coin = new Coinbase(creds.ApiKey, creds.ApiSecret);
            //coin.TryMe();

            /*bittrex.GetBalances().Display("BITTREX Account Balances:");
            string currency = "BTC";
            cout(bittrex.GetDepositAddress(currency).ToString());
            cout(bittrex.GetBalance(currency).ToString());

            // MARKETS, CURRENCIES and TICKERS (MarketSummary=>SymbolDetails)
            //var markets = bittrex.GetMarkets();
            //var currencies = bittrex.GetCurrencies();
            var ticker = bittrex.GetTicker(pair);
            var summary = bittrex.GetMarketSummary(pair);
            var summaries = bittrex.GetMarketSummaries();

            var history = bittrex.GetMarketHistory(pair);*/
        }


        public static void TestTelegram()
        {
            //var tm = new TelegramMessaging();
            //bool success = tm.Connect();
            //tm.Authenticate();

            //var t = bot.Send("HatBotTrades", "This is a test message from HATMAN_BOT");

            TelegramBot.Instance.Send(410181031, "This is a message for YOU!");

            /*cout("Looping forever...");
            for(; ;)
            {
                Thread.Sleep(10000);
                bot.Send(string.Format("nerd alert! {0}", DateTime.Now));
            }*/

            cout("Sleeping...");
            Thread.Sleep(30000);
        }

        /*public static async void SendTelegram(string user, string message)
        {
            if (m_tbot == null) m_tbot = new TelegramBot("511987996:AAFlFjHD1oSAjcS_WgXoWc2GtWyCp-kG5KU");
            await m_tbot.Send(user, message);
        }*/

        public static void TestOrderBooks()
        {
            cout("---------------------------------------");
            cout("Testing Consolidated Order Books (REST)");
            cout("---------------------------------------");

            List<string> symbols;

            /*symbols = BitZ.Instance.SymbolList;
            symbols.ForEach(System.Console.WriteLine);
            System.Console.WriteLine("\n\n");
            symbols.Where(s => s.Contains("eos")).ToList().ForEach(System.Console.WriteLine);*/

            // QTUM
            var qtumExch = new Dictionary<BaseExchange, List<string>>();
            qtumExch[bitfinex] = new List<string> { "qtmusd", "qtmbtc", "qtmeth" };
            qtumExch[hitbtc] = new List<string> { "QTUMUSD", "QTUMBTC", "QTUMETH" };
            qtumExch[bittrex] = new List<string> { "BTC-QTUM", "ETH-QTUM" };
            qtumExch[binance] = new List<string> { "QTUMBTC", "QTUMETH" };
            /*qtumExch[Bithumb.Instance] = new List<string> { "QTUM/KRW" };
            qtumExch[Huobi.Instance] = new List<string> { "qtumbtc", "qtumeth", "qtumusdt" };
            qtumExch[CHBTC.Instance] = new List<string> { "qtum_cny" };
            qtumExch[OKEx.Instance] = new List<string> { "qtum_btc", "qtum_eth" };
            //coinegg
            qtumExch[BitZ.Instance] = new List<string> { "qtum_btc" };
            //Exx
            qtumExch[GateIO.Instance] = new List<string> { "qtum_btc", "qtum_eth", "qtum_usdt" };
            //Coinnest
            qtumExch[Kucoin.Instance] = new List<string> { "QTUM-BTC", "QTUM-ETH" };
            //bigone
            //Livecoin
            //CoinExchange
            qtumExch[bleutrade] = new List<string> { "QTUM_BTC", "QTUM_ETH", "QTUM_DOGE" };
            //COSS
            //Qryptos
            //Novaexchange
            //Cryptopia
            //Liqui
            //qtumExch[BTER.Instance] = new List<string> { "qtum_btc", "qtum_eth" };*/
            foreach (var exch in qtumExch.Keys)
            {
                System.Console.WriteLine("==========================================================================");
                foreach (var s in qtumExch[exch])
                {
                    cout("{0}|{1} order book:\n{2}", exch.ToString(), s, exch.GetOrderBook(s));
                }
            }

            /*// POE
            var poeExch = new Dictionary<BExchangeApi, List<string>>();
            poeExch[Binance.Instance] = new List<string> { "POEBTC", "POEETH" };
            poeExch[OKEx.Instance] = new List<string> { "poe_btc", "poe_eth", "poe_usdt" };
            poeExch[Kucoin.Instance] = new List<string> { "POE-BTC", "POE-ETH" };
            poeExch[HitBTC.Instance] = new List<string> { "POEBTC", "POEETH" };
            poeExch[EtherDelta.Instance] = new List<string> { "ETH_POE" };
            foreach (var exch in poeExch.Keys)
            {
                System.Console.WriteLine("==========================================================================");
                foreach (var s in poeExch[exch])
                {
                    cout("{0}|{1} order book:\n{2}", exch.ToString(), s, exch.GetOrderBook(s));
                }
            }*/

            /*// EOS
            var eosExch = new Dictionary<BExchangeApi, List<string>>();
            eosExch[OKEx.Instance] = new List<string  { "eos_btc", "eos_eth", "eos_usdt", "eos_bch" };
            eosExch[Bitfinex.Instance] = new List<string> { "eosusd", "eosbtc", "eoseth" };
            eosExch[Huobi.Instance] = new List<string> { "eodbtc", "eoseth", "eosusdt" };
            //Ethfinex
            eosExch[Binance.Instance] = new List<string> { "EOSBTC", "EOSETH" };
            eosExch[GateIO.Instance] = new List<string> { "eos_btc", "eos_eth", "eos_usdt" };
            eosExch[BitZ.Instance] = new List<string> { "eos_btc" };
            eosExch[CHBTC.Instance] = new List<string> { "eos_cny" };
            eosExch[HitBTC.Instance] = new List<string> { "EOSBTC", "EOSETH" };
            eosExch[kraken] = new List<string> { "EOSXBT", "EOSETH" };
            //bigone
            //bibox
            //Liqui
            eosExch[Kucoin.Instance] = new List<string> { "EOS-BTC", "EOS-ETH" };
            //Exx
            eosExch[EtherDelta.Instance] = new List<string> { "ETH_EOS" };
            //YoBit
            //Cobinhood
            //Livecoin
            //Idex
            //Mercatox
            //nexchange.io
            //COSS
            //Tidex
            eosExch[BTER.Instance] = new List<string> { "" };
            foreach (var exch in eosExch.Keys)
            {
                System.Console.WriteLine("==========================================================================");
                foreach (var s in eosExch[exch])
                {
                    cout("{0}|{1} order book:\n{2}", exch.ToString(), s, exch.GetOrderBook(s));
                }
            }*/
        }

        public static void TestWebSockets()
        {
            /*var coins = CryptoCompare.Instance.GetCoinList();
            cout(coins);

            var coinSnapshot = CryptoCompare.Instance.GetCoinSnapshot();
            cout(coinSnapshot);

            var indexValue = Crypto.MarketCapAnalysis();*/


            /*var ob = kraken.GetOrderBook("XXBTZUSD");
            cout(ob.ToString());
            return;*/

            /*var tickers = Binance.Instance.GetTickers();
            foreach (var t in tickers)
            {
                cout(t);
            }*/

            //Binance.Instance.WriteHistorical("ETHUSDT");
            //Binance.Instance.WriteHistorical("BTCUSDT");

            var symbols = binance.SymbolList;


            binance.StartWebSocket();
            binance.SubscribeWebSocket();

            return;

            // WORKS!!!
            hitbtc.StartWebSocket();
            hitbtc.SubscribeWebSocket();

            // WORKS!!!
            gdax.StartWebSocket();
            string[] args = { @"""type"":""subscribe""", @"""product_ids"":[""btc-usd""]", @"""channels"":[""level2""]" };
            gdax.SubscribeWebSocket(args);

            // WORKS!!!
            bitfinex.StartWebSocket();
            bitfinex.SubscribeWebSocket();

            // WORKS!!!
            //BitMEX.Instance.StartWebSocket();
            //BitMEX.Instance.SubscribeWebSocket();

            // WORKS!!!
            Gemini.Instance.StartWebSocket(new string[] { "BTCUSD" });
            Gemini.Instance.SubscribeWebSocket();
                     
            // WORKS!!!
            //Cex.Instance.StartWebSocket();
            //Cex.Instance.SubscribeWebSocket();


            aggregateTimer.Interval = 2000;
            aggregateTimer.Elapsed += AggregateTimer_Elapsed;
            aggregateTimer.Enabled = true;
            //CryptoCompare.Instance.StartWebSocket();
            //CryptoCompare.Instance.SubscribeWebSocket();

            //Thread.Sleep(30000);

            //Cex.Instance.StartWebSocket();
            //Cex.Instance.SubscribeWebSocket();

            /*// ETH/CNY、ETC/CNY   wss://be.huobi.com/ws
            // BTC/CNY、LTC/CNY   wss://api.huobi.com/ws
            // ETH/BTC、LTC/BTC、ETC/BTC, BCC/BTC   wss://api.huobi.pro/ws
            var sock0 = StartWebSocket("wss://be.huobi.com/ws");
            string json = @"{
                ""sub"": ""market.btccny.depth.step0"",
                ""id"": ""id1""
            }";
            SendWebSocketMessage(sock0, json);*/

            /*string authRequest = Cex.Instance.CreateAuthRequest();
            var sock1 = StartWebSocket("wss://ws.cex.io/ws/");
            SendWebSocketMessage(sock1, authRequest);*/


            /*_pusher = new Pusher("de504dc5763aeef9ff52");
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


            _pusher.Connect();*/


            //var socket3 = StartWebSocket("wss://streamer.cryptocompare.com");
            //SendWebSocketMessage(socket3, "'SubAdd', { subs: ['0~Poloniex~BTC~USD'] }");

            //var socket = StartWebSocket("wss://ws-feed.gdax.com");
            //SendWebSocketMessage(socket, @"{""product_ids"":[""btc-usd""], ""type"":""subscribe""}");

            // Gemini WebSocket
            //var sock2 = StartWebSocket("wss://api.gemini.com/v1/marketdata/btcusd");

            // Bitfinex WebSocket
            /*var sock3 = StartWebSocket("wss://api.bitfinex.com/ws/2");
            string pair = "BTCUSD";
            string json = @"{ ""event"":""subscribe"", ""channel"":""book"", ""pair"":""" + pair + @""", ""prec"":""P0"" }";
            SendWebSocketMessage(sock3, json);*/


            //var socket2 = StartWebSocket("wss://ws.blockchain.info/inv");

            // Debugging operations
            //SendWebSocketMessage(socket2, "{'op':'ping'}");           // echo
            //SendWebSocketMessage(socket2, "{'op':'ping_block'}");       // responds with the latest block
            //SendWebSocketMessage(socket2, "{'op':'ping_tx'}");          // responds with the latest transaction (if subscribed to any addresses it will respond with the latest transaction involving those addresses)

            // Subscribe to notifications for all new bitcoin transactions
            //SendWebSocketMessage(socket2, "{'op':'unconfirmed_sub'}");
            //SendWebSocketMessage(socket2, "{'op':'unconfirmed_unsub'}");

            // Receive new transactions for a specific bitcoin address
            //SendWebSocketMessage(socket2, "{'op':'addr_sub', 'addr':'$bitcoin_address'}");     
            //SendWebSocketMessage(socket2, "{'op':'addr_unsub', 'addr':'$bitcoin_address'}");

            // Receive notifications when a new block is found
            //SendWebSocketMessage(socket2, "{'op':'blocks_sub'}");
            //SendWebSocketMessage(socket2, "{'op':'blocks_unsub'}");



            //var clientTask1 = Client();
            //Console.ReadLine();


            /*using (var ws = new WebSocket("ws://dragonsnest.far/Laputa"))
            {
                ws.OnMessage += (sender, e) =>
                    Console.WriteLine("Laputa says: " + e.Data);

                ws.Connect();
                ws.Send("BALUS");
                Console.ReadKey(true);
            }*/

            //BlockchainInfoTicker myDeserializedObj = (BlockchainInfoTicker)JavaScriptConvert.DeserializeObject(json, typeof(BlockchainInfoTicker));
            //List<BlockchainInfoTicker> myDeserializedObj = (List<BlockchainInfoTicker>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<BlockchainInfoTicker>));
            //List<BlockchainInfoTicker> myDeserializedObj = (List<BlockchainInfoTicker>)Jayrock.Json.Conversion.JsonConvert.Import<List<BlockchainInfoTicker>>(json);
        }


        /*private void ChartCoindeskBPI()
        {
            DateTime dt1 = new DateTime(2017, 1, 1);
            DateTime dt2 = DateTime.Now;
            var bpiHistorical = Coindesk.GetBPIHistorical(dt1, dt2);
            dout(Str(bpiHistorical));
            chart.Series[0].Name = "Coindesk BPI";
            this.chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
            this.chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
            foreach (string sDate in bpiHistorical.bpi.Keys)
            {
                chart.Series[0].Points.AddXY(sDate, bpiHistorical.bpi[sDate]);
            }
        }*/

        // Prime Weighted Crypto Index ("Prime Crypto 20" == "PC20")
        public static float MarketCapAnalysis()
        {
            var tickers = CoinMarketCap.Instance.GetTickers(20);
            var elements = from t in tickers
                           select (long)float.Parse(t.market_cap_usd);
            long marketCapSum = elements.Sum();

            var mcPercentage = new Dictionary<string, float>();
            float indexValue = 0.0f;
            foreach (var t in tickers)
            {
                long mc = (long)float.Parse(t.market_cap_usd);
                mcPercentage[t.symbol] = mc / (float)marketCapSum;
                float contribution = mcPercentage[t.symbol] * float.Parse(t.price_usd);
                indexValue += contribution;
                cout("{0,-5} ({1,5:0.00}%)  index_value:${2,15:n0}  market_cap:${3,15:n0}  price:${4,8:0.00}  contribution_to_index:${5,8:0.00}", t.symbol, mcPercentage[t.symbol] * 100, marketCapSum, (long)float.Parse(t.market_cap_usd), float.Parse(t.price_usd), contribution);
            }
            cout("TOTAL INDEX VALUE: $ {0:0.00}", indexValue);
            return indexValue;
        }

        //public static CoinSymbol

        static void AggregateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            aggregateTimer.Enabled = false;
            orderBook.Clear();
            //orderBook.AggregateRows(BitMEX.Instance.OrderBook);
            orderBook.AggregateRows(gdax.OrderBook);
            cout(orderBook.ToString());
            aggregateTimer.Enabled = true;
        }


        // TODO: Create an IBalance interface and expose it in each of the API calls for balances/accounts
        /*public static void TestAccountBalances()
        {
            var kbal = kraken.GetAccountBalance();      // ACCOUNT BALANCES: KRAKEN
            string sKbal = (kbal == null) ? "" : string.Join(" ", kbal.Select(kvp => string.Format("[{0}={1}]", kvp.Key, kvp.Value)));

            var bitbal = bittrex.GetBalances();         // ACCOUNT BALANCES: BITTREX
            string sBitbal = (bitbal == null) ? "" : string.Join(" ", bitbal.Select(s => string.Format("[{0}={1} {2} {3}]", s.Currency, s.Available, s.Balance, s.CryptoAddress)));

            var gbal = gdax.GetAccounts();              // ACCOUNT BALANCES: GDAX
            string sGbal = (gbal == null) ? "" : string.Join(" ", gbal.Select(s => string.Format("[{0}={1} {2}]", s.Currency, s.Available, s.Balance)));

            var acctInfo = binance.GetAccountInfo();    // ACCOUNT BALANCES: BINANCE
            var binbal = acctInfo.Balances.ToList();
            string sBinbal = (binbal == null) ? "" : string.Join(" ", binbal.Where(s => s.Free != 0 || s.Locked != 0).Select(s => string.Format("[{0}={1} {2}]", s.Currency, s.Free, s.Locked)));

            var bsbal = bitstamp.GetBalances();
            string sBsbal = (bsbal == null) ? "" : string.Join(" ", bsbal.Select(s => string.Format("[{0}={1} {2} {3}]", s.Currency, s.Available, s.Balance, s.Reserved)));

            var bfbal = bitfinex.GetBalances();
            string sBfbal = (bfbal == null) ? "" : string.Join(" ", bfbal.Select(s => string.Format("[{0}={1} {2} {3}]", s.Currency, s.Available, s.Balance, s.Reserved)));

            var pbal = poloniex.GetBalances();
            string sPbal = (pbal == null) ? "" : string.Join(" ", pbal.Where(kvp => kvp.Value.BitcoinValue != 0 || kvp.Value.QuoteAvailable != 0 || kvp.Value.QuoteOnOrders != 0).Select(kvp => string.Format("[{0}={1}BTC {2} {3}]", kvp.Key, kvp.Value.BitcoinValue, kvp.Value.QuoteAvailable, kvp.Value.QuoteOnOrders)));

            cout("\nACCOUNT BALANCES:\nKRAKEN> {0}\nBITTREX> {1}\nGDAX> {2}\nBINANCE> {3}\nBITSTAMP> {4}\nBITFINEX> {5}\nPOLONIEX> {6}\n", sKbal, sBitbal, sGbal, sBinbal, sBsbal, sBfbal, sPbal);
        }*/

        public static void TestWebsockets()
        {
            // GDAX: WORKS!!!
            gdax.StartWebSocket(null);
            string[] args = { @"""type"":""subscribe""", @"""product_ids"":[""btc-usd""]", @"""channels"":[""level2""]" };
            gdax.SubscribeWebSocket(args);
            gdax.UpdateOrderBookEvent += Handle_UpdateOrderBook;
            

            /*// BitMEX: WORKS!!!
            bitmex.StartWebSocket();
            bitmex.SubscribeWebSocket();
            bitmex.UpdateOrderBookEvent += Handle_UpdateOrderBook;*/

            // HitBTC: WORKS!!!
            hitbtc.StartWebSocket();
            hitbtc.SubscribeWebSocket();
            hitbtc.UpdateOrderBookEvent += Handle_UpdateOrderBook;

            // Bitfinex: WORKS!!!
            bitfinex.StartWebSocket();
            bitfinex.SubscribeWebSocket();
            bitfinex.UpdateOrderBookEvent += Handle_UpdateOrderBook;

            // Gemini: WORKS!!!
            Gemini.Instance.StartWebSocket(new string[] { "BTCUSD" });
            Gemini.Instance.SubscribeWebSocket();
            Gemini.Instance.UpdateOrderBookEvent += Handle_UpdateOrderBook;

            /*// Cex: WORKS!!! ... uh, cannot connect to socket?!?
            cex.StartWebSocket();
            cex.SubscribeWebSocket();
            cex.UpdateOrderBookEvent += Handle_UpdateOrderBook;*/
            
        }

        private static void Handle_UpdateOrderBook(object sender, OrderBookUpdateArgs e)
        {
            cout(e.OrderBook);
            //m_orderBookGrid.UpdatePrices(e.OrderBook);
            //m_lastGridUpdate = DateTime.Now;
            
        }
    }   // end of class Crypto

} // end of namespace
