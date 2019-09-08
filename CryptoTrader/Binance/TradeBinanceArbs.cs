using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Binance.Net;
using CryptoApis.SharedModels;
using CryptoTools;
using CryptoTools.Cryptography;

namespace CryptoTrader
{
    // ---- QUANTITIES ----
    // NEO/USDT qty=0.20
    // BNB/USDT qty=1.00
    // BCC/USDT qty=0.014
    // QTUM/USDT qty=0.75
    // LTC/USDT qty = 0.09
    // ADA/USDT qty = 50.0
    
    public class TradeBinanceArbs
    {
        // https://github.com/binance-exchange/binance-official-api-docs/blob/master/web-socket-streams.md

        static Dictionary<string, Ticker> m_tickers = new Dictionary<string, Ticker>();
        static Dictionary<string, decimal> m_lastBuy = new Dictionary<string, decimal>();
        static Dictionary<string, decimal> m_lastSell = new Dictionary<string, decimal>();

        static BinanceExchangeInfo m_exchangeInfo;
        static Dictionary<string, BinanceSymbolInfo> m_symbolInfo = new Dictionary<string, BinanceSymbolInfo>();

        static Dictionary<string, decimal> m_tradeQty = new Dictionary<string, decimal>();
        static Dictionary<string, int> m_position = new Dictionary<string, int>();

        static int m_minPosition, m_maxPosition;

        static Credentials m_creds;
        static BinanceApi m_api;
        static BinanceClient m_client;

        public TradeBinanceArbs()
        {
            SetTradeSizes(1.0M);
            InitializeArbPositions();
            InitializeApi();
        }

        public void StartTrading()
        {
            //BuyMinimums(10); return;            

            List<string> symbols = new List<string>();
            symbols.AddRange(new List<string>() { "ethusdt", "btcusdt" });
            symbols.AddRange(new List<string>() { "neousdt", "neoeth", "neobtc" });
            symbols.AddRange(new List<string>() { "bnbusdt", "bnbeth", "bnbbtc" });
            symbols.AddRange(new List<string>() { "qtumusdt", "qtumeth", "qtumbtc" });
            symbols.AddRange(new List<string>() { "ltcusdt", "ltceth", "ltcbtc" });
            symbols.AddRange(new List<string>() { "bccusdt", "bcceth", "bccbtc" });
            symbols.AddRange(new List<string>() { "adausdt", "adaeth", "adabtc" });
            symbols.AddRange(new List<string>() { "xrpusdt", "xrpeth", "xrpbtc" });

            List<string> streams = new List<string>();
            streams.Add("ticker");
            streams.Add("aggTrade");
            BinanceStreams(streams.ToArray(), symbols.ToArray());
        }

        // NOTE: THIS IS A ONE-OFF METHOD TO BE CALLED ONCE TO "LOAD UP" (BUY) THE INITIAL COIN NEEDED TO TRADE THE ARBS
        private void BuyMinimums(decimal multiplier)
        {
            foreach (var kv in m_tradeQty)
            {
                var symbol = kv.Key;
                decimal qty = kv.Value * multiplier;
                Console.Write("{0} {1}", symbol, qty);
                var t = m_api.GetOrderBookTicker(symbol);
                Console.WriteLine("    {0}:{1}", t.bidPrice, t.askPrice);
                m_api.Buy(symbol, qty, t.bidPrice);
            }
        }

        private void InitializeApi()    //string encryptedCredentialsFile, string password)
        {
            m_creds = Credentials.LoadEncryptedCsv(Credentials.CredentialsFile, Credentials.CredentialsPassword);
            var cred = m_creds["BINANCE"];
            m_api = new BinanceApi(cred.Key, cred.Secret);

            BinanceClientOptions options = new BinanceClientOptions();
            options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(cred.Key, cred.Secret);
            m_client = new BinanceClient(options);

            //m_api.Test();
            //PrintAllBalances();
            //Ping(new string[] { "api.binance.com" });
            //Rebalance();

            m_api.StartUserDataStream();
        }

        public void Test()
        {
            var ai = m_client.GetAccountInfo();
            var balances = ai.Data.Balances;
            foreach (var b in balances)
            {
                if (b.Free != 0.0M || b.Locked != 0.0M)
                    Console.WriteLine("{0} {1} {2} {3}", b.Asset, b.Free, b.Locked, b.Total);
            }

            string asset = "BTC";
            var btcDeposit = m_client.GetDepositAddress(asset).Data;
            var withdrawalFee = m_client.GetWithdrawalFee(asset).Data;

            var depositHistory = m_client.GetDepositHistory(null).Data;      // null for ALL assets
            var withdrawHistory = m_client.GetWithdrawHistory(null).Data;    // null for ALL assets
            /*string address = "";
            decimal amount = 0.0M;
            m_client.Withdraw(asset, address, amount);*/
        }
        

        // For each symbol (currency pair), set the trade qty to be used in our ARBS
        // where multiplier like 1.0M that will adjust the trade quantity for each symbol (currency pair)
        private void SetTradeSizes(decimal multiplier = 1.0M)
        {
            m_tradeQty["neousdt"] = 0.20M;
            m_tradeQty["bnbusdt"] = 1.00M;
            m_tradeQty["bccusdt"] = 0.020M;
            m_tradeQty["qtumusdt"] = 0.75M;
            m_tradeQty["ltcusdt"] = 0.09M;
            m_tradeQty["adausdt"] = 50.0M;
            m_tradeQty["xrpusdt"] = 20.0M;

            foreach (var k in m_tradeQty.Keys.ToList())
            {
                m_tradeQty[k] = m_tradeQty[k] * multiplier;
            }
        }

        // For each arb (i.e. "neobtc", "neoeth", "qtumbtc", etc.), set the position to zero
        private void InitializeArbPositions()
        {
            m_position["neobtc"] = 0;
            m_position["bnbbtc"] = 0;
            m_position["bccbtc"] = 0;
            m_position["qtumbtc"] = 0;
            m_position["ltcbtc"] = 0;
            m_position["adabtc"] = 0;
            m_position["xrpbtc"] = 0;

            m_position["neoeth"] = 0;
            m_position["bnbeth"] = 0;
            m_position["bcceth"] = 0;
            m_position["qtumeth"] = 0;
            m_position["ltceth"] = 0;
            m_position["adaeth"] = 0;
            m_position["xrpeth"] = 0;

            // Set minimum and maximum allowable positions for the arbs
            m_minPosition = -3;
            m_maxPosition = +3;
        }

        // Where streamName like "trade", "aggTrade", "ticker", ...
        // Where symbols like "btcusdt", "ethusdt", "ethbtc", ...
        public void BinanceStreams(string[] streamNames, string[] symbols)
        {
            const int BYTE_SIZE = 4096;

            Console.WriteLine("\nLAUNCH: Binance\n");

            m_exchangeInfo = BinanceModels.GetExchangeInfo();

            // Get SymbolInfo for a set of symbols (will be stored in m_symbolInfo dictionary)
            var symbolList = new List<string>() { "ethusdt", "btcusdt", "neousdt", "neoeth", "neobtc", "bnbusdt", "bnbeth", "bnbbtc", "qtumusdt", "qtumeth", "qtumbtc", "ltcusdt", "ltceth", "ltcbtc", "bccusdt", "bcceth", "bccbtc", "adausdt", "adaeth", "adabtc", "xrpusdt", "xrpeth", "xrpbtc" };
            GetSymbolInfo(symbolList);
            m_symbolInfo.Values.ToList().ForEach(si => Console.WriteLine(si.ToString()));   // output SymbolInfo to console
            Console.WriteLine("\n");

            // Create a string like the following (for use when connecting to websocket):
            // "btcusdt@trade/ethusdt@trade/ethbtc@trade"
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < symbols.Length; ++i)
            {
                for (int j = 0; j < streamNames.Length; ++j)
                {
                    if (! (i == 0 && j==0)) sb.Append('/');
                    sb.Append(symbols[i]);
                    sb.Append('@');
                    sb.Append(streamNames[j]);
                }
            }
            string symbolText = sb.ToString();

            var socket = new ClientWebSocket();
            Task task = socket.ConnectAsync(new Uri("wss://stream.binance.com:9443/stream?streams=" + symbolText), CancellationToken.None);
            task.Wait();

            Thread readThread = new Thread(
                delegate (object obj)
                {
                    byte[] recBytes = new byte[BYTE_SIZE];
                    int count = 0;
                    while (true)
                    {
                        ArraySegment<byte> t = new ArraySegment<byte>(recBytes);
                        Task<WebSocketReceiveResult> receiveAsync = socket.ReceiveAsync(t, CancellationToken.None);
                        receiveAsync.Wait();
                        string jsonString = Encoding.UTF8.GetString(recBytes);
                        //Console.WriteLine("jsonString = {0}", jsonString);
                        if (++count % 20 == 0) Console.Write(".");

                        JObject jo = JsonConvert.DeserializeObject<JObject>(jsonString);
                        string stream = jo["stream"].Value<string>();
                        jo = jo["data"].Value<JObject>();
                        string type = jo["e"].Value<string>();
                        //Console.WriteLine("stream: {0}   eventType: {1}", stream, type);


                        if (type == "24hrTicker")               // "24hrTicker":
                        {
                            int i = stream.IndexOf("@");
                            string pid = stream.Substring(0, i);
                            Ticker ticker = new Ticker(jo);
                            /*long eventTime = jo["E"].Value<long>();
                            string bid = jo["b"].Value<string>();
                            string bidSize = jo["B"].Value<string>();
                            string ask = jo["a"].Value<string>();
                            string askSize = jo["A"].Value<string>();*/
                            //Console.WriteLine("{0}: {1} {2}   {3} {4}      {5}", pid, ticker.Bid, ticker.BidSize, ticker.Ask, ticker.AskSize, ticker.Time);

                            m_tickers[pid] = ticker;

                            CheckArbs();
                        }
                        else if (type == "trade")                // "trade": 
                        {
                            //string pid = jo["product_id"].Value<string>();
                            int i = stream.IndexOf("@");
                            string pid = stream.Substring(0, i);
                            long eventTime = jo["E"].Value<long>();
                            string price = jo["p"].Value<string>();
                            string size = jo["q"].Value<string>();
                            Console.WriteLine("{0}  {1} {2}  {3}", pid, price, size, eventTime);
                        }
                        else if (type == "aggTrade")            // "aggTrade"
                        {
                            // TODO: handle aggTrade here
                        }
                        else
                        {
                            Console.WriteLine("uknown stream type: '{0}'", type);
                        }
                        /*else if (type == "update")            // "update"
                        {
                            //Console.WriteLine(jsonString);
                        }
                        else if (type == "error")               // "error"
                        {
                        }
                        else if (type == "heartbeat")           // "heartbeat"
                        {
                        }*/

                        Array.Clear(recBytes, 0, BYTE_SIZE);
                        //recBytes = new byte[BYTE_SIZE];
                    }
                });
            readThread.Start();

            //Console.ReadLine();
        }

        private void CheckArbs()
        {
            decimal minPercentage = 0.15M;      // minimum percentage that should be available in the arb before it will be executed
            CheckArb1("neo", minPercentage);
            CheckArb1("bnb", minPercentage);
            CheckArb1("qtum", minPercentage);
            CheckArb1("ltc", minPercentage);
            CheckArb1("bcc", minPercentage);
            CheckArb1("ada", minPercentage);
            CheckArb1("xrp", minPercentage);
        }

        private void CheckArb1(string currency, decimal minimum)
        {
            CheckArb1(currency + "usdt", currency + "eth", "ethusdt", minimum);
            CheckArb1(currency + "usdt", currency + "btc", "btcusdt", minimum);
        }

        private void CheckArb1(string symbol1, string symbol2, string symbol3, decimal minimum)
        {
            if (m_tickers.ContainsKey(symbol1) && m_tickers.ContainsKey(symbol2) && m_tickers.ContainsKey(symbol3))
            {
                // BUY xxxusdt
                decimal a1 = m_tickers[symbol1].Ask;
                decimal b2 = m_tickers[symbol2].Bid;
                decimal a3 = m_tickers[symbol3].Ask;
                decimal arb1 = ((b2 * a3) - a1) / a1 * 100.0M;

                // SELL xxxusdt
                decimal b1 = m_tickers[symbol1].Bid;
                decimal a2 = m_tickers[symbol2].Ask;
                decimal b3 = m_tickers[symbol3].Bid;
                decimal arb2 = (b1 - (a2 * b3)) / b1 * 100.0M;

                string dt = DateTime.Now.ToString("HH:mm:ss");

                string signal1, signal2;
                if (arb1 > minimum)
                {
                    signal1 = "BUY ";
                    if ((!m_lastBuy.ContainsKey(symbol2) || m_lastBuy[symbol2] != a1) && m_position[symbol2] > m_minPosition && m_position[symbol2] < m_maxPosition)    // TODO: Update this method to avoid duplicating previous trade
                    {
                        decimal aq1 = m_tickers[symbol1].AskSize;
                        decimal bq2 = m_tickers[symbol2].BidSize;
                        decimal aq3 = m_tickers[symbol3].AskSize;
                        //decimal qty1 = Math.Min(Math.Min(aq1, m_tradeQty[symbol1]), bq2);
                        decimal qty1 = m_tradeQty[symbol1];
                        decimal qty2 = qty1 * b2;
                        qty2 = symbol3.StartsWith("btc") ? Math.Round(qty2, 6) : Math.Round(qty2, 5);   // round BTC qty to 6 places, ETH qty to 5 places

                        m_api.Buy(symbol1, qty1, a1);
                        m_api.Sell(symbol2, qty1, b2);
                        m_api.Buy(symbol3, qty2, a3);

                        Console.WriteLine("\n{0} {1,8}/{2,7}:    {3}  {4:0.00}%", dt, symbol1, symbol2, signal1, arb1);
                        Console.WriteLine("      BUY {0} {1} @ {2}      SELL {3} {4} @ {5}      BUY {6} {7} @ {8}\n", symbol1, qty1, a1, symbol2, qty1, b2, symbol3, qty2, a3);

                        ++m_position[symbol2];          // add 1 to the stored position for this arb

                        m_lastBuy[symbol2] = a1;
                    }
                }

                if (arb2 > minimum)
                {
                    signal2 = "SELL";
                    if ((!m_lastSell.ContainsKey(symbol2) || m_lastSell[symbol2] != b1) && m_position[symbol2] > m_minPosition && m_position[symbol2] < m_maxPosition)  // TODO: Update this method to avoid duplicating previous trade
                    {
                        decimal bq1 = m_tickers[symbol1].BidSize;
                        decimal aq2 = m_tickers[symbol2].AskSize;
                        decimal bq3 = m_tickers[symbol3].BidSize;
                        //decimal qty1 = Math.Min(Math.Min(bq1, m_tradeQty[symbol1]), aq2);
                        decimal qty1 = m_tradeQty[symbol1];
                        decimal qty2 = qty1 * a2;
                        qty2 = symbol3.StartsWith("btc") ? Math.Round(qty2, 6) : Math.Round(qty2, 5);   // round BTC qty to 6 places, ETH qty to 5 places

                        
                        m_api.Sell(symbol1, qty1, b1);
                        m_api.Buy(symbol2, qty1, a2);
                        m_api.Sell(symbol3, qty2, b3);

                        Console.WriteLine("\n{0} {1,8}/{2,7}:    {3}  {4:0.00}%", dt, symbol1, symbol2, signal2, arb2);
                        Console.WriteLine("      SELL {0} {1} @ {2}      BUY {3} {4} @ {5}      SELL {6} {7} @ {8}\n", symbol1, qty1, b1, symbol2, qty1, a2, symbol3, qty2, b3);

                        --m_position[symbol2];          // subtract 1 from the stored position for this arb

                        m_lastSell[symbol2] = b1;
                    }
                }
            }
            /*if (!string.IsNullOrWhiteSpace(signal1) || !string.IsNullOrWhiteSpace(signal2))
            {
                Console.WriteLine("{0,8}/{1,7}:   {2}  {3:0.00}       {4}  {5:0.00}", symbol1, symbol2, signal1, arb1, signal2, arb2);
            }*/
        }

        // For each symbol in the list, retrieve the BinanceSymbolInfo object
        private void GetSymbolInfo(List<string> symbolList)
        {
            foreach (var symbol in symbolList)
            {
                m_symbolInfo[symbol] = GetSymbolInfo(symbol);
            }
        }

        // Return the BinanceSymbolInfo object for the specified symbol
        private BinanceSymbolInfo GetSymbolInfo(string symbol)
        {
            BinanceSymbolInfo symbolInfo = null;
            for (int i = 0; i < m_exchangeInfo.symbols.Count; ++i)
            {
                var si = m_exchangeInfo.symbols[i];
                if (si.symbol.ToLower() == symbol)
                {
                    symbolInfo = si;
                }
            }
            return symbolInfo;
        }

        private void Rebalance()
        {
            var tickers = m_api.GetOrderBookTickers();

            // NEO, QTUM, BNB, LTC, BCC, ADA, XRP => 0.004 BTC
            // ETH => 0.02 BTC
            // BTC => 0.02 BTC
            // USDT => 0.04 BTC

        }

        public void GetBalances(out decimal totalBtc, out decimal totalUsdt, bool display = false)
        {
            totalBtc = 0.0M;
            totalUsdt = 0.0M;

            var tickers = m_api.GetOrderBookTickers();

            m_api.UpdateAccountBalances();

            // NEO, QTUM, BNB, LTC, BCC, ADA, XRP => 0.004 BTC
            // ETH => 0.02 BTC
            // BTC => 0.02 BTC
            // USDT => 0.04 BTC

            List<string> assets = new List<string>() { "ADA", "BCC", "BNB", "LTC", "NEO", "QTUM", "XRP", "BTC", "ETH", "USDT" };

            if (display) Console.WriteLine("");

            decimal btc;
            foreach (var k in m_api.Balances.Keys)
            {
                if (assets.Contains(k))     // only display balances for the selected asset list
                {
                    if (k == "BTC")
                        btc = m_api.Balances[k].Total;
                    else if (k == "USDT")
                        btc = m_api.Balances[k].Total / tickers["BTCUSDT"].MidPrice;
                    else
                    {
                        string symbol = k + "BTC";
                        btc = m_api.Balances[k].Total * tickers[symbol].MidPrice;
                    }
                    totalBtc += btc;

                    if (display)
                    {
                        Console.Write("{0:0.00000000,14} BTC     ", btc);
                        Console.WriteLine(m_api.Balances[k]);
                    }
                }
            }

            totalBtc = Math.Round(totalBtc, 8);
            totalUsdt = Math.Round(totalBtc * tickers["BTCUSDT"].MidPrice, 2);
        }

        public void StartPositionDisplayThread()
        {
            GetBalances(out decimal initTotalBtc, out decimal initTotalUsdt, true);
            Console.WriteLine("\n{0}    {1} BTC   {2} USDT\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), initTotalBtc, initTotalUsdt);

            int n = 0;
            while (true)
            {
                Thread.Sleep(30000);

                ++n;

                decimal totalBtc, totalUsdt;
                if (n % 10 == 0)
                    GetBalances(out totalBtc, out totalUsdt, true);
                else
                    GetBalances(out totalBtc, out totalUsdt);

                Console.WriteLine("{0}    {1} BTC   {2} USDT            change: {3} BTC   {4} USDT", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), totalBtc, totalUsdt, totalBtc - initTotalBtc, totalUsdt - initTotalUsdt);
            }
        }

    } // end of class TradeBinanceArbs

} // end of namespace

