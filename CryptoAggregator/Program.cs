//using PureSocketCluster;
//using PureWebSockets;
//using WebSocketSharp;
//using WebSocketX;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoApis.RestApi;
using CryptoApis.Models;
using CryptoApis.ExchangeX.CoinMarketCap;
using CryptoApis;

namespace Aggregator
{
    class Program
    {
        // https://bitfinex.readme.io/v1/reference

        static string m_credentialsFile;
        static string m_pw;

        static ApiFactory m_factory;


        static void Main(string[] args)
        {
            Console.WriteLine("\n*** WELCOME TO AGGREGATOR ***\n");

            var apiXS = new ExchangeSharpApi();

            //apiXS.Gator("ETH-USD", amountRequested: 200, bips:125);
            apiXS.Test();
            
            //CryptoWebSocketApis.GeminiWebSocket.TestGeminiTickers_marketdata();
            Console.WriteLine("(Back in MAIN)");
            Console.ReadKey();
            return;

            //CryptoTools.Cryptography.Cryptography.EncryptFile("X:/Users/Trader/Documents/hat_apis.json", "/Users/michael/Documents/hat_apis.enc", pw);    // encrypt api key file
            m_credentialsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "system.apis.enc");   // read ".enc" file from application path
            m_pw = @"myKey123";

            m_factory = new ApiFactory(m_credentialsFile, m_pw);


            /*TradeBittrexArbs.InitializeApi(credentialsFile, pw);
            TradeBittrexArbs.Test();
            TradeBinanceArbs.InitializeApi(credentialsFile, pw);
            TradeBinanceArbs.Test();
            TradeGdaxArbs.InitializeApi(credentialsFile, pw);
            TradeGdaxArbs.Test();
            TradeKraken.InitializeApi(credentialsFile, pw);
            TradeKraken.Test();
            TradeGemini.InitializeApi(credentialsFile, pw);
            TradeGemini.Test();*/

            // Get CoinMarketCap rankings
            var cmcap = new CoinMarketCapApi();
            cmcap.Test();
            /*var rank = cmcap.GetRankings();
            var gainers1h = rank.OrderByDescending(t => t.percent_change_1h);
            var gainers24h = rank.OrderByDescending(t => t.percent_change_24h);
            var gainers7d = rank.OrderByDescending(t => t.percent_change_7d);
            var losers1h = rank.OrderBy(t => t.percent_change_1h);
            var losers24h = rank.OrderBy(t => t.percent_change_24h);
            var losers7d = rank.OrderBy(t => t.percent_change_7d);*/

            //BinanceTotals();
            //var mgr = new CryptoTools.SymbolManager();
            //SellBinance(0.50M, "bnb");

            /*DisplayBalances("KRAKEN", "btcusd");
            DisplayBalances("BITFINEX", "btcusd");
            DisplayBalances("BINANCE", "btcusdt");
            DisplayBalances("BITTREX", "btcusdt");
            DisplayBalances("POLONIEX", "btcusdt");
            DisplayBalances("GDAX", "btcusd");

            try
            {
                GdaxRestApi api = m_factory.Get("GDAX") as GdaxRestApi;
                api.PrintCoinbaseAccounts().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nGDAX Coinbase Error: {0}", ex.Message);
            }*/



            Console.Write("\n\nPress any key to exit... ");
            Console.ReadKey();
            return;

            //BinanceArbs.InitializeApi();

            TradeBinanceArbs.GetBalances(out decimal initTotalBtc, out decimal initTotalUsdt, true);
            Console.WriteLine("\n{0}    {1} BTC   {2} USDT\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), initTotalBtc, initTotalUsdt);

            // [LocalTime, exch1UpdateTime, exch1BitCoinPrice, exch2UpdateTime, exch2BitCoinPrice]

            List<string> symbols = new List<string>();
            symbols.AddRange(new List<string>() { "ethusdt", "btcusdt" });
            symbols.AddRange(new List<string>() { "neousdt", "neoeth", "neobtc" });
            symbols.AddRange(new List<string>() { "bnbusdt", "bnbeth", "bnbbtc" });
            symbols.AddRange(new List<string>() { "qtumusdt", "qtumeth", "qtumbtc" });
            symbols.AddRange(new List<string>() { "ltcusdt", "ltceth", "ltcbtc" });
            symbols.AddRange(new List<string>() { "bccusdt", "bcceth", "bccbtc" });
            symbols.AddRange(new List<string>() { "adausdt", "adaeth", "adabtc" });

            List<string> streams = new List<string>();
            streams.Add("ticker");
            streams.Add("aggTrade");
            TradeBinanceArbs.BinanceStreams(streams.ToArray(), symbols.ToArray());

            //GdaxTickers();
            //GeminiTickers();
            //BinanceTickers();
            //BitfinexTickers();
        }

        // where T like KrakenRestApi and exchange like "KRAKEN" and symbolId like "btcusd"
        static void DisplayBalances(string exchange, string symbolId)
        {
            try
            {
                Console.WriteLine("{0}", new string('=', 80));
                var api = m_factory.Get(exchange);
                var t = api.GetTicker(symbolId);
                t.Wait();
                t.Result.Print(api.Exchange);
                var b = api.GetBalances();
                b.Wait();
                b.Result.PrintNonZero(api.Exchange);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR ({0}, {1}): {2}", exchange, symbolId, ex.Message);
            }
        }

        static void BinanceTotals()
        {
            try
            {
                Console.WriteLine("{0}", new string('=', 80));
                var api = m_factory.Get("BINANCE");
                var t_btc = api.GetTicker("btcusdt");
                t_btc.Wait();
                t_btc.Result.Print(api.Exchange);
                Console.WriteLine();
                var br = api.GetBalances();
                br.Wait();
                var bres = br.Result;
                //bres.PrintNonZero(api.Exchange);
                decimal btcTotal = 0.0M;
                foreach (var currency in bres.Keys)
                {
                    decimal btcAmount = 0.0M;
                    if (!bres[currency].IsZero)
                    {
                        if (currency == "btc")
                        {
                            //Console.WriteLine("BTC");
                            btcAmount = bres[currency].Available;
                            Console.WriteLine("{0,4} {1,14:0.00000000} {2,14:0.00000000} btc:{3,14:0.00000000}", "BTC", bres[currency].Amount, bres[currency].Available, btcAmount);
                        }
                        else if (currency == "usdt")
                        {
                            //Console.WriteLine("USDT");
                            var t = api.GetTicker("btcusdt");
                            t.Wait();
                            btcAmount = bres[currency].Available / t.Result.MidPrice;
                            Console.WriteLine("{0,4} {1,14:0.00000000} {2,14:0.00000000} btc:{3,14:0.00000000}", "USDT", bres[currency].Amount, bres[currency].Available, btcAmount);
                        }
                        else
                        {
                            string symbolId = currency + "btc";
                            var t = api.GetTicker(symbolId);
                            t.Wait();
                            //t.Result.Print(api.Exchange);
                            btcAmount = bres[currency].Available * t.Result.MidPrice;
                            Console.WriteLine("{0,4} {1,14:0.00000000} {2,14:0.00000000} btc:{3,14:0.00000000}", bres[currency].Currency, bres[currency].Amount, bres[currency].Available, btcAmount);
                        }
                        btcTotal += btcAmount;
                    }
                }
                decimal usdTotal = btcTotal * t_btc.Result.MidPrice;
                Console.WriteLine("\nTotal:  btc:{0,-14:0.00000000}  usd:{1,-14:0.00000000}", btcTotal, usdTotal);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
        }

        // where currency like "bnb", "qtum", "eth", ...
        static void SellBinance(decimal multiplier, string currency)
        {
            try
            {
                Console.WriteLine("{0}", new string('=', 80));
                var api = m_factory.Get("BINANCE") as BinanceRestApi;
                api.GetExchangeInfo().Wait();

                return;

                string symbolId = currency + "btc";
                var t = api.GetTicker(symbolId);
                t.Wait();
                t.Result.Print(api.Exchange);
                var br = api.GetBalances();
                br.Wait();
                var bres = br.Result[currency];
                var qty = bres.Available * multiplier;
                var price = t.Result.MidPrice;
                Console.WriteLine("{0} qty:{1} price:{2}", symbolId, qty, price);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
        }



        /*private static void Ws_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("OnOpen: ");
        }

        private static void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("OnError: " + e.Message);
        }

        private static void Ws_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine("OnClose: " + e.Reason);
        }

        private static void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("OnMessage: " + e.Data);
        }*/



        /*private static PureSocketClusterSocket _scc;

        static void TestPureSocketCluster()
        {
            // input credentials if used, different systems use different auth systems this however is the most common (passing 'auth' event with your credentials)
            var creds = new Creds
            {
                apiKey = "your apikey if used",
                apiSecret = "your api secret if used"
            };

            // initialize the client
            //_scc = new PureSocketClusterSocket("wss://yoursocketclusterserver.com/socketcluster/", new ReconnectStrategy(4000, 60000), creds);
            _scc = new PureSocketClusterSocket("wss://ws-feed.gdax.com/", new ReconnectStrategy(4000, 60000), creds);
            

            // hook up to some events
            _scc.OnOpened += Scc_OnOpened;
            _scc.OnMessage += _scc_OnMessage;
            _scc.OnStateChanged += _scc_OnStateChanged;
            _scc.OnSendFailed += _scc_OnSendFailed;
            _scc.OnError += _scc_OnError;
            _scc.OnClosed += _scc_OnClosed;
            _scc.OnData += _scc_OnData;
            _scc.OnFatality += _scc_OnFatality;
            _scc.Connect();

            // subscribe to some channels
            var cn = _scc.CreateChannel("TRADE-PLNX--BTC--ETC").Subscribe();
            cn.OnMessage(TradeData);
            var cn0 = _scc.CreateChannel("TRADE-PLNX--BTC--ETH").Subscribe();
            cn0.OnMessage(TradeData);
            var cn1 = _scc.CreateChannel("TRADE-OK--BTC--USD").Subscribe();
            cn1.OnMessage(TradeData);

            Console.ReadLine();
        }

        private static void _scc_OnFatality(string reason)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Fatality: {reason}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void _scc_OnData(byte[] data)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Binary: {data}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        //private static void _scc_OnClosed(WebSocketCloseStatus reason)
        private static void _scc_OnClosed(System.Net.WebSockets.WebSocketCloseStatus reason)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Socket Closed: {reason}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void _scc_OnError(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Error: {ex}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void _scc_OnSendFailed(string data, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"send failed: {data} Ex: {ex}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        //private static void _scc_OnStateChanged(WebSocketState newState, WebSocketState prevState)
        private static void _scc_OnStateChanged(System.Net.WebSockets.WebSocketState newState, System.Net.WebSockets.WebSocketState prevState)
        {
        Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"State changed from {prevState} to {newState}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void TradeData(string name, object data)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(name + ": " + data);
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void _scc_OnMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void Scc_OnOpened()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Opened");
            Console.ResetColor();
            Console.WriteLine("");
        }*/

    } // end of class Program

} // end of namespace
