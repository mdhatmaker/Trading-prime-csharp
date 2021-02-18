using System;
using System.Linq;
using System.Linq.Expressions;
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
using Binance.Net;
using Binance.Net.Objects.Spot;
using CsvHelper;
using System.Globalization;
using Bittrex.Net.Objects;
using Bittrex.Net;
using CryptoExchange.Net.Authentication;
using Bitfinex.Net;
using Bitfinex.Net.Objects;
using Confluent.Kafka;
using CryptoExchange.Net.Sockets;
using System.Diagnostics;

namespace CryptoConsole
{
    class Program
    {
        static ApiFactory m_factory;

        //static Dictionary<string, List<HitBtcTicker>> m_hitTickers;             // HitBTC tickers

        static string SymbolFolder = "C:\\cryptomania\\";


        static async Task Main(string[] args)
        {
            Console.WriteLine("\n=== WELCOME TO CRYPTO CONSOLE ===\n");


            // --- FOR DEBUGGING ONLY: CAN SET COMMAND-LINE ARGUMENTS ---
#if DEBUG
            if (Debugger.IsAttached)
            {
                //args = new string[] { "balance", "BINANCE", "btcusdt" };
                //args = new string[] { "balance", "ALL", "*" };
                args = new string[] { "demo" };
            }
#endif

            if (args.Length == 0)
            {
                Console.WriteLine("usage: dotnet CryptoConsole.dll gator <symbol> <amount> <bips> (#display)");
                Console.WriteLine("       dotnet CryptoConsole.dll balance <exchange> <btcsymbol>");
                Console.WriteLine("       dotnet CryptoConsole.dll encrypt <csvfile> <8-char-password>");
                Console.WriteLine();
            }
#if DEBUG
            else if (args[0].ToUpper() == "DEMO")
            {
                await DemoExchanges();
                await DemoKafka();

                Console.WriteLine("\n\nDone...Press ENTER to exit");
                Console.ReadLine();
            }
#endif
            else
            {
                m_factory = new ApiFactory(Credentials.CredentialsFile, Credentials.CredentialsPassword);

                if (args[0].ToUpper() == "GATOR")
                    ExecuteGator(args);
                else if (args[0].ToUpper() == "BALANCE")
                    ExecuteBalance(args);
                else if (args[0].ToUpper() == "ENCRYPT")
                    ExecuteEncrypt(args);
            }

            //System.Environment.Exit(0);

        } // end of Main


        #region ========== FILE HELPER METHODS ==========================================
        static void WriteObjectsToCsv<T>(IEnumerable<T> data, string filepath, string singleColumnHeader = null)
        {
            using (var writer = new StreamWriter(filepath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }
        }

        static void WriteStringsToCsv(IEnumerable<string> data, string filepath, string singleColumnHeader)
        {
            using (var writer = new StreamWriter(filepath))
            {
                writer.WriteLine(singleColumnHeader);
                data.ForEach(s => writer.WriteLine(s));
            }
        }

        // Given an exchange name
        // Return the full filepath of the .csv symbols file
        static string SymbolFilepath(string exchName)
        {
            var filepath = Path.Join(SymbolFolder, $"symbols.{exchName}.csv");
            return filepath;
        }
#endregion ======================================================================


        #region ========== CREATE CLIENT/SOCKET FOR EACH EXCHANGE =======================
        static void CreateBinanceExchange(out BinanceClient exch, out BinanceSocketClient sock)
        {
            var evKeys = Environment.GetEnvironmentVariable( "BINANCE_KEY", EnvironmentVariableTarget.User);
            var keys = evKeys.Split('|');

            var clientOptions = new BinanceClientOptions();
            clientOptions.ApiCredentials = new ApiCredentials(keys[0], keys[1]);
            exch = new BinanceClient(clientOptions);
            //----------
            var socketOptions = new BinanceSocketClientOptions();
            socketOptions.ApiCredentials = clientOptions.ApiCredentials;
            sock = new BinanceSocketClient(socketOptions);
        }

        static void CreateBittrexExchange(out BittrexClient exch, out BittrexSocketClient sock)
        {
            var evKeys = Environment.GetEnvironmentVariable("BITTREX_KEY", EnvironmentVariableTarget.User);
            var keys = evKeys.Split('|');

            var clientOptions = new BittrexClientOptions();
            clientOptions.ApiCredentials = new ApiCredentials(keys[0], keys[1]);
            exch = new BittrexClient(clientOptions);
            //----------
            var socketOptions = new BittrexSocketClientOptions();
            socketOptions.ApiCredentials = clientOptions.ApiCredentials;
            sock = new BittrexSocketClient(socketOptions);
        }

        static void CreateBitfinexExchange(out BitfinexClient exch, out BitfinexSocketClient sock)
        {
            var evKeys = Environment.GetEnvironmentVariable("BITFINEX_KEY", EnvironmentVariableTarget.User);
            var keys = evKeys.Split('|');

            var clientOptions = new BitfinexClientOptions();
            clientOptions.ApiCredentials = new ApiCredentials(keys[0], keys[1]);
            exch = new BitfinexClient(clientOptions);
            //----------
            var socketOptions = new BitfinexSocketClientOptions();
            socketOptions.ApiCredentials = clientOptions.ApiCredentials;
            sock = new BitfinexSocketClient(socketOptions);
        }
        #endregion ======================================================================

        #region ========== BINANCE METHODS ==============================================
        static async Task DemoBinanceSimple(BinanceClient exch)
        {
            string exchName = "BINANCE";
            var eiRes = await exch.Spot.System.GetExchangeInfoAsync();
            var ei = eiRes.Data;
            var symbols = ei.Symbols;
            Console.WriteLine($"[{exchName}]   {symbols.Count()} symbols");
            /*var aiRes = await exch.General.GetAccountInfoAsync();
            var ai = aiRes.Data;
            var symbol = exch.GetSymbolName("BTC", "USDT");
            var price24h = await exch.Spot.Market.Get24HPriceAsync(symbol);
            for (int i = 0; i < 10; ++i)
            {
                var resBook = await exch.Spot.Market.GetBookPriceAsync(symbol);
                var book = resBook.Data;
                Console.WriteLine($"[{exchName} {symbol}]   {book.BestBidQuantity:n6} x {book.BestBidPrice:n2}   {book.BestAskPrice:n2} x {book.BestAskQuantity:n6}");
                Thread.Sleep(1000);
            }*/
        }

        static async Task DemoBinanceSymbolTickerUpdates(BinanceSocketClient sock, int sleepSeconds = 20)
        {
            Console.WriteLine($"--- Running BINANCE SymbolTickerUpdates thread for {sleepSeconds} seconds ---");
            var subscription = BinanceSubscribe(sock);
            Thread.Sleep(sleepSeconds * 1000);
            await BinanceUnsubscribe(sock, subscription);
        }

        static UpdateSubscription BinanceSubscribe(BinanceSocketClient sock)
        {
            string exchName = "BINANCE";

            /*sock.Spot.SubscribeToAllBookTickerUpdates((binanceStreamBookPrice) =>
            {

            });*/
            var crSubSymbolTicker = sock.Spot.SubscribeToAllSymbolTickerUpdates((binanceTick) =>
            {
                Console.WriteLine($"[{exchName}]   {binanceTick.Count()} symbol ticker updates received");
                var tick = binanceTick.First();
                Console.WriteLine($"[{exchName} {tick.Symbol}]   {tick.BidQuantity}x{tick.BidPrice}  {tick.AskPrice}x{tick.AskQuantity}   (example 1st update)");
            });

            var subSymbolTicker = crSubSymbolTicker.Data;
            return subSymbolTicker;
        }

        static async Task BinanceUnsubscribe(BinanceSocketClient sock, UpdateSubscription subscription)
        {
            await sock.Unsubscribe(subscription);
        }

        static async Task BinanceWriteSymbolsCsv(BinanceClient exch)
        {
            string exchName = "BINANCE";
            var eiRes = await exch.Spot.System.GetExchangeInfoAsync();
            var ei = eiRes.Data;
            var symbols = ei.Symbols;
            Console.WriteLine($"[{exchName}]   {symbols.Count()} symbols");
            WriteObjectsToCsv(symbols, SymbolFilepath(exchName));
        }
#endregion ======================================================================

        #region ========== BITTREX METHODS ==============================================
        static async Task DemoBittrexSimple(BittrexClient exch)
        {
            string exchName = "BITTREX";
            var resSymbols = await exch.GetSymbolsAsync();
            var symbols = resSymbols.Data;
            Console.WriteLine($"[{exchName}]   {symbols.Count()} symbols");
        }

        static async Task DemoBittrexSymbolTickerUpdates(BittrexSocketClient sock, int sleepSeconds = 20)
        {
            string exchName = "BITTREX";
            Console.WriteLine($"--- Running {exchName} SymbolTickerUpdates thread for {sleepSeconds} seconds ---");
            var subscription = BittrexSubscribe(sock);
            Thread.Sleep(sleepSeconds * 1000);
            await BittrexUnsubscribe(sock, subscription);
        }

        static UpdateSubscription BittrexSubscribe(BittrexSocketClient sock)
        {
            string exchName = "BITTREX";

            /*var crSubSymbolTicker = sock.Spot.SubscribeToAllSymbolTickerUpdates((binanceTick) =>
            {
                Console.WriteLine($"[{exchName}]   {binanceTick.Count()} symbol ticker updates received");
                var tick = binanceTick.First();
                Console.WriteLine($"[{exchName} {tick.Symbol}]   {tick.BidQuantity}x{tick.BidPrice}  {tick.AskPrice}x{tick.AskQuantity}   (example 1st update)");
            });

            var subSymbolTicker = crSubSymbolTicker.Data;
            return subSymbolTicker;*/
            return null;
        }

        static async Task BittrexUnsubscribe(BittrexSocketClient sock, UpdateSubscription subscription)
        {
            await sock.Unsubscribe(subscription);
        }

        static async Task BittrexWriteSymbolsCsv(BittrexClient exch)
        {
            string exchName = "BITTREX";
            var resSymbols = await exch.GetSymbolsAsync();
            var symbols = resSymbols.Data;
            Console.WriteLine($"[{exchName}]   {symbols.Count()} symbols");
            WriteObjectsToCsv(symbols, SymbolFilepath(exchName));
        }
        #endregion ======================================================================

        #region ========== BITFINEX METHODS ==============================================
        static async Task DemoBitfinexSimple(BitfinexClient exch)
        {
            var exchName = "BITFINEX";
            var resSymbols = await exch.GetSymbolsAsync();
            var symbols = resSymbols.Data;
            Console.WriteLine($"[{exchName}]   {symbols.Count()} symbols");
        }

        static async Task DemoBitfinexSymbolTickerUpdates(BitfinexSocketClient sock, int sleepSeconds = 20)
        {
            Console.WriteLine($"--- Running BINANCE SymbolTickerUpdates thread for {sleepSeconds} seconds ---");
            var subscription = BitfinexSubscribe(sock);
            Thread.Sleep(sleepSeconds * 1000);
            await BitfinexUnsubscribe(sock, subscription);
        }

        static UpdateSubscription BitfinexSubscribe(BitfinexSocketClient sock)
        {
            string exchName = "BITFINEX";

            /*var crSubSymbolTicker = sock.Spot.SubscribeToAllSymbolTickerUpdates((binanceTick) =>
            {
                Console.WriteLine($"[{exchName}]   {binanceTick.Count()} symbol ticker updates received");
                var tick = binanceTick.First();
                Console.WriteLine($"[{exchName} {tick.Symbol}]   {tick.BidQuantity}x{tick.BidPrice}  {tick.AskPrice}x{tick.AskQuantity}   (example 1st update)");
            });

            var subSymbolTicker = crSubSymbolTicker.Data;
            return subSymbolTicker;*/
            return null;
        }

        static async Task BitfinexUnsubscribe(BitfinexSocketClient sock, UpdateSubscription subscription)
        {
            await sock.Unsubscribe(subscription);
        }

        static async Task BitfinexWriteSymbolsCsv(BitfinexClient exch)
        {
            string exchName = "BITFINEX";
            var resSymbols = await exch.GetSymbolsAsync();
            var symbols = resSymbols.Data;
            WriteStringsToCsv(symbols, SymbolFilepath(exchName), "Symbol");
        }
        #endregion ======================================================================


        static async Task DemoExchanges(int sleepSeconds = 2)
        {
            Console.WriteLine($"\n--- Running Exchange Demos in {sleepSeconds} second(s) ---");
            Thread.Sleep(sleepSeconds * 1000);

            // BINANCE exchange
            CreateBinanceExchange(out BinanceClient exchBinance, out BinanceSocketClient sockBinance);
            await DemoBinanceSimple(exchBinance);
            //await DemoBinanceSymbolTickerUpdates(sockBinance);

            // BITTREX exchange
            CreateBittrexExchange(out BittrexClient exchBittrex, out BittrexSocketClient sockBittrex);
            await DemoBittrexSimple(exchBittrex);
            //await DemoBittrexSymbolTickerUpdates(sockBittrex);

            // BITFINEX exchange
            CreateBitfinexExchange(out BitfinexClient exchBitfinex, out BitfinexSocketClient sockBitfinex);
            await DemoBitfinexSimple(exchBitfinex);
            //await DemoBitfinexSymbolTickerUpdates(sockBitfinex);

            return;
        }


        #region ========== KAFKA ========================================================
        static async Task DemoKafka(int sleepSeconds = 2)
        {
            Console.WriteLine($"\n--- Running Kafka Demos in {sleepSeconds} second(s) ---");
            Thread.Sleep(sleepSeconds * 1000);

            // Kafka configuration strings
            string bootstrapServers = "localhost:9092";
            string topic = "crypto-marketdata-symbols";
            string groupId = "marketdata-consumer-group";

            await DemoKafkaProducer(bootstrapServers, topic, payload: "This is a sample payload.");
            await DemoKafkaConsumer(groupId, bootstrapServers, topic);

            return;
        }

        // where bootstrapServers like "localhost:9092" (CSV list if more than one server)
        // where topic like "crypto-marketdata-symbols"
        // where msg like "This is a sample payload."
        static async Task DemoKafkaProducer(string bootstrapServers, string topic, string payload)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync(topic, new Message<Null, string> { Value = payload });
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
            Console.WriteLine();

            // Note that a server round-trip is slow (3ms at a minimum; actual latency depends
            // on many factors). In highly concurrent scenarios you will achieve high overall
            // throughput out of the producer using the above approach, but there will be a delay
            // on each await call. In stream processing applications, where you would like to process
            // many messages in rapid succession, you would typically use the Produce method instead:
            var conf = new ProducerConfig { BootstrapServers = bootstrapServers };

            Action<DeliveryReport<Null, string>> handler = r =>
                Console.WriteLine(!r.Error.IsError
                    ? $"Delivered message to {r.TopicPartitionOffset}"
                    : $"Delivery Error: {r.Error.Reason}");

            using (var p = new ProducerBuilder<Null, string>(conf).Build())
            {
                for (int i = 0; i < 10; ++i)
                {
                    p.Produce(topic, new Message<Null, string> { Value = i.ToString() }, handler);
                }

                // wait for up to 10 seconds for any inflight messages to be delivered.
                p.Flush(TimeSpan.FromSeconds(10));
            }
            Console.WriteLine();
        }

        // where GroupId like "test-consumer-group"
        // where bootstrapServers like "localhost:9092" (CSV list if more than one server)
        // where topic like "crypto-marketdata-symbols"
        static async Task DemoKafkaConsumer(string groupId, string bootstrapServers, string topic)
        {
            // Basic Consumer example
            var conf = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(topic);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
                Console.WriteLine();
            }
        }
        #endregion ======================================================================



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
            //DisplayBalances("KRAKEN", "btcusd");
            DisplayBalances("BITFINEX", "btcusd");
            DisplayBalances("BINANCE", "btcusdt");
            DisplayBalances("BITTREX", "btcusdt");
            //DisplayBalances("POLONIEX", "btcusdt");
            //DisplayBalances("GDAX", "btcusd");

            /*try
            {
                GdaxRestApi api = m_factory.Get("GDAX") as GdaxRestApi;
                api.PrintCoinbaseAccounts().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nGDAX Coinbase Error: {0}", ex.Message);
            }*/
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
                    if (f is Binance.Net.Objects.Spot.MarketData.BinanceSymbolLotSizeFilter)
                    {
                        var lotSizeFilter = f as Binance.Net.Objects.Spot.MarketData.BinanceSymbolLotSizeFilter;
                        //Console.WriteLine("LOT FILTER: {0} {1} {2} {3}", sym.Name, lotSizeFilter.MinQuantity, lotSizeFilter.MaxQuantity, lotSizeFilter.StepSize);
                        xs.MinQuantity = lotSizeFilter.MinQuantity;
                        xs.MaxQuantity = lotSizeFilter.MaxQuantity;
                        xs.StepSize = lotSizeFilter.StepSize;
                    }
                    else if (f is Binance.Net.Objects.Spot.MarketData.BinanceSymbolPriceFilter)
                    {
                        var priceFilter = f as Binance.Net.Objects.Spot.MarketData.BinanceSymbolPriceFilter;
                        //Console.WriteLine("PRICE FILTER: {0} {1} {2} {3}", sym.Name, priceFilter.MinPrice, priceFilter.MaxPrice, priceFilter.TickSize);
                        xs.MinPrice = priceFilter.MinPrice;
                        xs.MaxPrice = priceFilter.MaxPrice;
                        xs.TickSize = priceFilter.TickSize;
                    }
                    else if (f is Binance.Net.Objects.Spot.MarketData.BinanceSymbolMinNotionalFilter)
                    {
                        var notionalFilter = f as Binance.Net.Objects.Spot.MarketData.BinanceSymbolMinNotionalFilter;
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


        //=========================== CODE GRAVEYARD ==========================

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
        
            // Send a sample message via Kafka
            var sendMessage = new Thread(() => {
                KafkaNet.Protocol.Message msg = new KafkaNet.Protocol.Message(payload);
                var options = new KafkaOptions(kafkaUri);
                var router = new BrokerRouter(options);
                var client = new Producer(router);
                client.SendMessageAsync(topic, new List<KafkaNet.Protocol.Message> { msg }).Wait();
            });
            sendMessage.Start();
            // Get all the Kafka messages
            var options = new KafkaOptions(kafkaUri);    
            var brokerRouter = new BrokerRouter(options);    
            var consumer = new Consumer(new ConsumerOptions(topicName, brokerRouter));    
            foreach (var msg in consumer.Consume())    
              {    
                  Console.WriteLine(Encoding.UTF8.GetString(msg.Value));    
               }    
            Console.ReadLine();

        */

    } // end of class Program


    public class SymbolTickerStream
    {
        public string e { get; set; }   // Event type (i.e. "24hrTicker")
        public long E { get; set; }     // Event time
        public string s { get; set; }   // Symbol
        public decimal p { get; set; }  // Price change
        public decimal P { get; set; }  // Price change percent
        public decimal w { get; set; }  // Weighted average price
        public decimal x { get; set; }  // First trade(F)-1 price (first trade before the 24hr rolling window)
        public decimal c { get; set; }  // Last price
        public decimal Q { get; set; }  // Last quantity
        public decimal b { get; set; }  // Best bid price
        public decimal B { get; set; }  // Best bid quantity
        public decimal a { get; set; }  // Best ask price
        public decimal A { get; set; }  // Best ask quantity
        public decimal o { get; set; }  // Open price
        public decimal h { get; set; }  // High price
        public decimal l { get; set; }  // Low price
        public decimal v { get; set; }  // Total traded base asset volume
        public decimal q { get; set; }  // Total traded quote asset volume
        public long O { get; set; }     // Statistics open time
        public long C { get; set; }     // Statistics close time
        public long F { get; set; }     // First trade ID
        public long L { get; set; }     // Last trade ID
        public int n { get; set; }      // Total number of trades
    }

    public class SymbolBookTickerStream
    {
        public long u { get; set; }     // order book updateId
        public string s { get; set; }   // symbol
        public decimal b { get; set; }  // best bid price
        public decimal B { get; set; }  // best bid qty
        public decimal a { get; set; }  // best ask price
        public decimal A { get; set; }  // best ask qty
    }


} // end of namespace
