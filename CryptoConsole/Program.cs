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
using CryptoApis;
using CryptoApis.Models;
using CryptoApis.ExchangeX.CoinMarketCap;
using CryptoApis.Exchange.HitBtc;
using static CryptoTools.Global;
using CryptoTools.Models;
using CryptoTools;

namespace CryptoConsole
{
    class Program
    {
        static ApiFactory m_factory;

        //static Dictionary<string, List<HitBtcTicker>> m_hitTickers;             // HitBTC tickers

        static void Main(string[] args)
        {
            Console.WriteLine("\n*** WECOME TO CRYPTO CONSOLE ***\n");

            //args = new string[] { "balances", "BINANCE", "btcusdt" };

            m_factory = new ApiFactory(Credentials.CredentialsFile, Credentials.CredentialsPassword);

            if (args.Length == 0)
            {
                Console.WriteLine("usage: dotnet CryptoConsole.dll gator <symbol> <amount> <bips> (#display)");
                Console.WriteLine("       dotnet CryptoConsole.dll balance <exchange> <btcsymbol>");
                Console.WriteLine("       dotnet CryptoConsole.dll encrypt <csvfile> <8-char-password>");
                Console.WriteLine();
            }
            else if (args[0].ToUpper() == "GATOR")
                ExecuteGator(args);
            else if (args[0].ToUpper() == "BALANCE")
                ExecuteBalance(args);
            else if (args[0].ToUpper() == "ENCRYPT")
                ExecuteEncrypt(args);

            //CryptoTools.Cryptography.Cryptography.EncryptFile("X:/Users/Trader/Documents/hat_apis.json", "/Users/michael/Documents/hat_apis.enc", pw);    // encrypt api key file
            //var api = m_factory.Get("HITBTC") as CryptoApis.Exchange.HitBtc.HitBtcRestApi;
            
            /*//var symbols = api.GetSymbols();
            m_factory = new ApiFactory(m_credentialsFile, m_pw);*/

			//string password = "12345678";
			//var apiXS = new ExchangeSharpRestApi("/Users/michael/Documents/hat_apis.csv.enc", password);
			
				
			//DisplayBinanceTotals();
            //BinanceSellAllCurrency(1.0M);

            /*//var api = m_factory.Get("HITBTC") as CryptoRestApis.Exchange.HitBtc.HitBtcRestApi;
            //var symbols = api.GetSymbols();

            //symbols.ForEach(s => Console.WriteLine(s));
            var t1 = api.GetOneTicker("DASHBTC");
            Console.WriteLine(t1);
            var c1 = api.GetCandles("DASHBTC");
            c1.ForEach(cc => Console.WriteLine(cc));*/

            /*Console.Write("\n\nPress any key to exit... ");
            Console.ReadKey();
            return;

            string pathname = Path.Combine(GetExeDirectory(), "data.hitbtc_tickers.txt");
            bool writeHeaders = !File.Exists(pathname);
            var writer = new StreamWriter(pathname, true);
            if (writeHeaders)
            {
                writer.WriteLine(HitBtcTicker.CsvHeaders);
                writer.Flush();
            }
            
            while (true)
            {
                var tickers = api.GetAllTickers();
                if (m_hitTickers == null)           // first time through, create the dictionary to store HitBtc data
                {
                    m_hitTickers = new Dictionary<string, List<HitBtcTicker>>();
                    foreach (var t in tickers)
                    {
                        m_hitTickers[t.symbol] = new List<HitBtcTicker>();
                        m_hitTickers[t.symbol].Add(t);
                    }
                }
                else
                {
                    Console.WriteLine(new string('=', 120));
                    int count = 0;
                    foreach (var t in tickers)
                    {
                        if (!m_hitTickers.ContainsKey(t.symbol))                // if we have not yet stored this symbol...
                        {
                            m_hitTickers[t.symbol] = new List<HitBtcTicker>();  // create a new list for this symbol's tickers
                            m_hitTickers[t.symbol].Add(t);
                        }
                        else
                        {
                            var last = m_hitTickers[t.symbol].Last();           // check the last ticker we added to the list for this symbol
                            if (true)   //last.timestamp != t.timestamp)                  // only add to our stored tickers if the "last_updated" field has changed
                            {
                                m_hitTickers[t.symbol].Add(t);
                                //Console.WriteLine(t);
                                writer.WriteLine(t.ToCsv());
                                writer.Flush();
                                count++;
                            }                            
                        }
                    }
                    Console.WriteLine("{0} ({1} updates)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), count);
                }
                Thread.Sleep(20000);                // sleep for 20 seconds
            }


            //BinanceTotals();
            //var mgr = new CryptoTools.SymbolManager();
            //SellBinance(0.50M, "bnb");


            Console.Write("\n\nPress any key to exit... ");
            Console.ReadKey();
            return;
            */
        }

        static void ExecuteEncrypt(string[] args)
        {
            Console.WriteLine("********** ENCRYPT CSV FILE " + new string('*', 72));

            if (args.Length < 3)
            {
                Console.WriteLine("usage: dotnet CryptoConsole.dll encrypt <csvfile> <8-char-password>");
                Console.WriteLine();
                Console.WriteLine("   ex: dotnet CryptoConsole.dll encrypt /User/barney/Documents/my-apis.csv 12345678");
                Console.WriteLine();
                return;
            }

            string pathname = args[1];
            string pw = args[2];
            Console.Write("Encrypting '{0}'...", pathname);
            CryptoTools.Cryptography.Cryptography.SimpleEncrypt(pathname, pw);
            Console.WriteLine("Done.\n");
        }

        static void ExecuteBalance(string[] args)
        {
            Console.WriteLine("********** BALANCE " + new string('*', 81));

            var apiXS = new ExchangeSharpApi();

            if (args.Length < 3)
            {
                Console.WriteLine("usage: dotnet CryptoConsole.dll balance <exchange> <btcsymbol>");
                Console.WriteLine();
                Console.WriteLine("   ex: dotnet CryptoConsole.dll balance ALL *");
                Console.WriteLine("       dotnet CryptoConsole.dll balance BINANCE btcusdt");
                Console.WriteLine("       dotnet CryptoConsole.dll balance GDAX btcusd");
                Console.WriteLine("       dotnet CryptoConsole.dll balance BITTREX btcusdt");
                Console.WriteLine();
                return;
            }

            string exchange = args[1].ToUpper();
            if (exchange == "ALL")
                DisplayAllExchangeBalances();
            else
            {
                string btcsymbol = args[2].ToLower();
                DisplayBalances(exchange, btcsymbol);
            }

            Console.WriteLine();
        }

        static void ExecuteGator(string[] args)
        {
            Console.WriteLine("********** GATOR " + new string('*', 83));

            var apiXS = new ExchangeSharpApi();

            //apiXS.Gator("ETH-USD", amountRequested:200, bips:125);

            if (args.Length < 4)
            {
                Console.WriteLine("usage: dotnet CryptoConsole.dll gator <symbol> <amount> <bips> (#display)");
                Console.WriteLine();
                Console.WriteLine("   ex: dotnet CryptoConsole.dll gator ETH-USD 200 125");
                Console.WriteLine("       dotnet CryptoConsole.dll gator BTC-USD 90 125 20");
                Console.WriteLine("       dotnet CryptoConsole.dll gator BTC-USDT 75 150");
                Console.WriteLine();
                return;
            }

            string symbol = args[1];
            decimal amount = decimal.Parse(args[2]);
            int bips = int.Parse(args[3]);
            int displayBook = 0;
            if (args.Length > 3)
                displayBook = int.Parse(args[4]);
            apiXS.Gator(symbol, amount, bips, displayBook);
        }



        // Display the list of CoinMarketCap rankings by MARKET CAP (ignoring any zero-market-cap coins)
        static void DisplayCoinMarketCapRankings()
        {
            // Get CoinMarketCap rankings
            var cmcap = new CoinMarketCapApi();
            cmcap.Test();
        }

        // Sort the CoinMarketCap coins by their % change (1h, 24h, 7d)
        static void CoinMarketCapGainersLosers()
        {
            var cmcap = new CoinMarketCapApi();
            var rank = cmcap.GetRankings();
            var gainers1h = rank.OrderByDescending(t => t.percent_change_1h);
            var gainers24h = rank.OrderByDescending(t => t.percent_change_24h);
            var gainers7d = rank.OrderByDescending(t => t.percent_change_7d);
            var losers1h = rank.OrderBy(t => t.percent_change_1h);
            var losers24h = rank.OrderBy(t => t.percent_change_24h);
            var losers7d = rank.OrderBy(t => t.percent_change_7d);
        }

        static void TradeArbs()
        {
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
        }

        // Display account balances for each exchange
        static void DisplayAllExchangeBalances()
        {
            DisplayBalances("KRAKEN", "btcusd");
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
            }
        }

        // Display all balances for the specified exchange
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

        // Display Binance balances (along with the appropriate conversitions to BTC and USD)
        static void DisplayBinanceTotals()
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

        // Display info such as price/quantity/notional filters using Binance ExchangeInfo API call
        // (displayed in a format that can be copied directly into "system.symbol_ids.DF.csv" file)
        static void DisplayBinanceExchangeInfo()
        {
            Console.WriteLine("{0}", new string('=', 80));
            var api = m_factory.Get("BINANCE") as BinanceRestApi;
            var res = api.GetExchangeInfo();
            res.Wait();
            var info = res.Result;
            var symbols = new SortedDictionary<string, XSymbol>();
            foreach (var sym in info.Symbols.OrderBy(s => s.Name))
            {
                //BinanceSymbolFilter: BinanceSymbolLotSizeFilter, BinanceSymbolMinNotionalFilter, BinanceSymbolPriceFilter
                foreach (var f in sym.Filters)
                {
                    if (!symbols.ContainsKey(sym.Name))
                        symbols[sym.Name] = new XSymbol("BINANCE", sym.Name);

                    var xs = symbols[sym.Name];
                    if (f is Binance.Net.Objects.BinanceSymbolLotSizeFilter)
                    {
                        var lotSizeFilter = f as Binance.Net.Objects.BinanceSymbolLotSizeFilter;
                        //Console.WriteLine("LOT FILTER: {0} {1} {2} {3}", sym.Name, lotSizeFilter.MinQuantity, lotSizeFilter.MaxQuantity, lotSizeFilter.StepSize);
                        xs.MinQuantity = lotSizeFilter.MinQuantity;
                        xs.MaxQuantity = lotSizeFilter.MaxQuantity;
                        xs.StepSize = lotSizeFilter.StepSize;
                    }
                    else if (f is Binance.Net.Objects.BinanceSymbolPriceFilter)
                    {
                        var priceFilter = f as Binance.Net.Objects.BinanceSymbolPriceFilter;
                        //Console.WriteLine("PRICE FILTER: {0} {1} {2} {3}", sym.Name, priceFilter.MinPrice, priceFilter.MaxPrice, priceFilter.TickSize);
                        xs.MinPrice = priceFilter.MinPrice;
                        xs.MaxPrice = priceFilter.MaxPrice;
                        xs.TickSize = priceFilter.TickSize;
                    }
                    else if (f is Binance.Net.Objects.BinanceSymbolMinNotionalFilter)
                    {
                        var notionalFilter = f as Binance.Net.Objects.BinanceSymbolMinNotionalFilter;
                        //Console.WriteLine("NOTIONAL FILTER: {0} {1}", sym.Name, notionalFilter.MinNotional);
                        xs.MinNotional = notionalFilter.MinNotional;
                    }
                }
            }
            foreach (var kv in symbols)
            {
                var symbol = kv.Key;
                var v = kv.Value;
                Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", "BINANCE", symbol.ToLower(), symbol, v.MinPrice, v.MaxPrice, v.TickSize, v.MinQuantity, v.MaxQuantity, v.StepSize, v.MinNotional);
            }
            Console.WriteLine();
        }

        // Sell a portion (multiplier) of ALL current Binance positions
        static void BinanceSellAllCurrency(decimal multiplier)
        {
            var api = m_factory.Get("BINANCE");
            var br = api.GetBalances();
            br.Wait();
            var balances = br.Result;
            foreach (string currency in balances.Keys)
            {
                if (currency == "btc" || currency == "usdt") continue;
                BinanceSellCurrency(1.0M, currency);
            }
        }

        // Sell a portion (multiplier) of my current position in the specified asset (currency)
        // where multiplier like 0.01, 0.25, 0.50, ..., 1.0
        // where currency like "bnb", "qtum", "eth", ...
        static async void BinanceSellCurrency(decimal multiplier, string currency)
        {
            try
            {
                var api = m_factory.Get("BINANCE") as BinanceRestApi;
                string symbolId = currency + "btc";
                var t = api.GetTicker(symbolId);
                t.Wait();
                t.Result.Print(api.Exchange);
                var br = api.GetBalances();
                br.Wait();
                var balances = br.Result;
                var b = balances[currency];
                var qty = b.Available * multiplier;
                var price = t.Result.Ask;   // t.Result.MidPrice;
                var xs = api.GetXSymbol(symbolId);
                decimal uprice, uqty;
                bool changed = xs.CheckPriceQty(price, qty, out uprice, out uqty);
                if (uqty <= 0.0M) return;
                Console.WriteLine("SELL {0} qty:{1} price:{2} ==> {3} {4} {5}", symbolId, qty, price, changed, uprice, uqty);
                var res = await api.Sell(symbolId, uqty, uprice);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
        }

        // Test run of the Gemini websockets code
        static void TestGeminiWebsockets()
        {
            CryptoApis.WebsocketApi.GeminiWebSocket.TestGeminiTickers_marketdata();
            Console.WriteLine("(Back in MAIN)");
            Console.ReadKey();
            return;
        }

        // NOTE: This is likely not very useful on 20-second intervals because it appears the CMC data updates only once every 5 minutes
        static Dictionary<string, List<CoinMarketCapTicker>> m_cmcTickers;      // CoinMarketCap tickers
        static void ThreadUpdateCoinMarketCapRankings()
        {
            var cmcapi = new CoinMarketCapApi();     // create CoinMarketCap API object

            var g1 = cmcapi.GetGlobalData();
            Console.WriteLine(g1);

            while (true)
            {
                var rank = cmcapi.GetRankings();
                //var gainers1h = rank.OrderByDescending(t => t.percent_change_1h);
                if (m_cmcTickers == null)           // first time through, create the dictionary to store CoinMarketCap data
                {
                    m_cmcTickers = new Dictionary<string, List<CoinMarketCapTicker>>();
                    foreach (var t in rank)
                    {
                        m_cmcTickers[t.id] = new List<CoinMarketCapTicker>();
                        m_cmcTickers[t.id].Add(t);
                    }
                }
                else
                {
                    Console.WriteLine(new string('=', 180));
                    foreach (var t in rank)
                    {
                        if (!m_cmcTickers.ContainsKey(t.id))                        // if we have not yet stored this symbol...
                        {
                            m_cmcTickers[t.id] = new List<CoinMarketCapTicker>();   // create a new list for this symbol's tickers
                            m_cmcTickers[t.id].Add(t);
                        }
                        else
                        {
                            var last = m_cmcTickers[t.id].Last();       // check the last ticker we added to the list for this symbol
                            if (last.last_updated != t.last_updated)        // only add to our stored tickers if the "last_updated" field has changed
                            {
                                m_cmcTickers[t.id].Add(t);
                                Console.WriteLine("{0} {1} {2}", last.last_updated, t.last_updated, t);
                            }
                        }
                    }
                }
                Thread.Sleep(20000);                // sleep for 20 seconds
            }
        }


    } // end of class Program
} // end of namespace
