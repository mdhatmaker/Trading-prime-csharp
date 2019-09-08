using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using CryptoTools;
using ExchangeSharp;
using System.Text;
using System.Reflection;
using System.Threading;
using CryptoApis.SharedModels;

namespace CryptoApis
{
	// https://github.com/jjxtra/ExchangeSharp

    public enum ExchangeSet { Primary, All };

	public class ExchangeSharpApi
	{
        #region ---------- Exchange Shortcuts -------------------------------------------------------------------------
        public ExchangeAbucoinsAPI abuc => m_apiMap["ABUCOINS"] as ExchangeAbucoinsAPI;
        public ExchangeBinanceAPI bina => m_apiMap["BINANCE"] as ExchangeBinanceAPI;
        public ExchangeBitfinexAPI bitf => m_apiMap["BITFINEX"] as ExchangeBitfinexAPI;
        public ExchangeBithumbAPI bith => m_apiMap["BITHUMB"] as ExchangeBithumbAPI;
        public ExchangeBitstampAPI bits => m_apiMap["BITSTAMP"] as ExchangeBitstampAPI;
        public ExchangeBittrexAPI bitt => m_apiMap["BITTREX"] as ExchangeBittrexAPI;
        public ExchangeBleutradeAPI bleu => m_apiMap["BLEUTRADE"] as ExchangeBleutradeAPI;
        public ExchangeCryptopiaAPI cryp => m_apiMap["CRYPTOPIA"] as ExchangeCryptopiaAPI;
        public ExchangeGdaxAPI gdax => m_apiMap["GDAX"] as ExchangeGdaxAPI;
        public ExchangeGeminiAPI gemi => m_apiMap["GEMINI"] as ExchangeGeminiAPI;
        public ExchangeHitbtcAPI hitb => m_apiMap["HITBTC"] as ExchangeHitbtcAPI;
        public ExchangeHuobiAPI huob => m_apiMap["HUOBI"] as ExchangeHuobiAPI;
        public ExchangeKrakenAPI krak => m_apiMap["KRAKEN"] as ExchangeKrakenAPI;
        public ExchangeKucoinAPI kuco => m_apiMap["KUCOIN"] as ExchangeKucoinAPI;
        public ExchangeLivecoinAPI live => m_apiMap["LIVECOIN"] as ExchangeLivecoinAPI;
        public ExchangeOkexAPI okex => m_apiMap["OKEX"] as ExchangeOkexAPI;
        public ExchangePoloniexAPI polo => m_apiMap["POLONIEX"] as ExchangePoloniexAPI;
        public ExchangeTuxExchangeAPI tux => m_apiMap["TUX"] as ExchangeTuxExchangeAPI;
        public ExchangeYobitAPI yobi => m_apiMap["YOBIT"] as ExchangeYobitAPI;
        #endregion ----------------------------------------------------------------------------------------------------

        public ICollection<string> ExchangeIds => m_apiMap.Keys;
        
        public Credentials Credentials => m_creds;

        public IExchangeAPI this[string exchange] => m_apiMap[exchange];

		private IDictionary<string, IExchangeAPI> m_apiMap;
		private Credentials m_creds;
        private List<IDisposable> m_sockets;

        private ConcurrentQueue<TickerOutput> m_outputQ = new ConcurrentQueue<TickerOutput>();
        
		// Exchange symbols and Global symbols (each in a dictionary with key "EXCHANGE")
        private static IDictionary<string, IEnumerable<string>> m_symbols = new SortedDictionary<string, IEnumerable<string>>();
        private static IDictionary<string, IEnumerable<string>> m_gsymbols = new SortedDictionary<string, IEnumerable<string>>();

        // m_tickers[global_symbol][exchange] ==> ConcurrentStack<ExchangeTicker>
        // ex: m_tickers["BTC-USD"]["BINANCE"] ==> ConcurrentStack<ExchangeTicker>
        private ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentStack<ExchangeTicker>>> m_tickers;
        
        public ExchangeSharpApi(ExchangeSet exchangeSet = ExchangeSet.Primary)
		{
            if (exchangeSet == ExchangeSet.Primary)
                m_apiMap = GetPrimaryExchangeApis();
            else
                m_apiMap = GetAllExchangeApis();

            m_tickers = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentStack<ExchangeTicker>>>();
        }

        // where encryptedFile is the full pathname of the encrypted CSV file like "/Users/david/Documents/myapis.csv.enc"
        // where password is 8-char password like "12345678"
		public ExchangeSharpApi(string encryptedFile, string password, ExchangeSet exchangeSet = ExchangeSet.Primary) : this(exchangeSet)
        {
            LoadCredentials(encryptedFile, password);
			//m_apiMap = GetAllExchangeApis();
			//m_apiMap = GetPrimaryExchangeApis();
        }

        public void LoadCredentials(string encryptedFile, string password)
        {
            m_creds = Credentials.LoadEncryptedCsv(encryptedFile, password);

            foreach (var kv in m_apiMap)
            {
                if (m_creds[kv.Key] == null) continue;

                if (kv.Key == "GDAX")
                    m_apiMap[kv.Key].LoadAPIKeysUnsecure(m_creds[kv.Key].Key, m_creds[kv.Key].Secret, m_creds[kv.Key].Passphrase);
                else
                    m_apiMap[kv.Key].LoadAPIKeysUnsecure(m_creds[kv.Key].Key, m_creds[kv.Key].Secret);
            }
        }

        private IDictionary<string, IExchangeAPI> GetPrimaryExchangeApis(bool forceUpdate = false)
        {
            if (m_apiMap == null || forceUpdate)
            {
                m_apiMap = new SortedDictionary<string, IExchangeAPI>();

                m_apiMap.Add("BINANCE", new ExchangeBinanceAPI());
                m_apiMap.Add("BITFINEX", new ExchangeBitfinexAPI());
                //m_apiMap.Add("BITHUMB", new ExchangeBithumbAPI());  // TODO: What's wrong with Bithumb?
                m_apiMap.Add("BITSTAMP", new ExchangeBitstampAPI());
                m_apiMap.Add("BITTREX", new ExchangeBittrexAPI());
                m_apiMap.Add("GDAX", new ExchangeGdaxAPI());
                m_apiMap.Add("GEMINI", new ExchangeGeminiAPI());
                m_apiMap.Add("KRAKEN", new ExchangeKrakenAPI());
                m_apiMap.Add("POLONIEX", new ExchangePoloniexAPI());
            }
            return m_apiMap;
        }

        private IDictionary<string, IExchangeAPI> GetAllExchangeApis(bool forceUpdate = false)
        {
            if (m_apiMap == null || forceUpdate)
            {
                m_apiMap = new SortedDictionary<string, IExchangeAPI>();

                m_apiMap.Add("ABUCOINS", new ExchangeAbucoinsAPI());
                m_apiMap.Add("BINANCE", new ExchangeBinanceAPI());
                m_apiMap.Add("BITFINEX", new ExchangeBitfinexAPI());
                m_apiMap.Add("BITHUMB", new ExchangeBithumbAPI());      // TODO: What's wrong with Bithumb?
                m_apiMap.Add("BITSTAMP", new ExchangeBitstampAPI());
                m_apiMap.Add("BITTREX", new ExchangeBittrexAPI());
                m_apiMap.Add("BLEUTRADE", new ExchangeBleutradeAPI());
                m_apiMap.Add("CRYPTOPIA", new ExchangeCryptopiaAPI());
                m_apiMap.Add("GDAX", new ExchangeGdaxAPI());
                m_apiMap.Add("GEMINI", new ExchangeGeminiAPI());
                m_apiMap.Add("HITBTC", new ExchangeHitbtcAPI());
                m_apiMap.Add("HUOBI", new ExchangeHuobiAPI());
                m_apiMap.Add("KRAKEN", new ExchangeKrakenAPI());
                m_apiMap.Add("KUCOIN", new ExchangeKucoinAPI());
                m_apiMap.Add("LIVECOIN", new ExchangeLivecoinAPI());
                //m_apiMap.Add("OKEX", new ExchangeOkexAPI());
                m_apiMap.Add("POLONIEX", new ExchangePoloniexAPI());
                m_apiMap.Add("TUX", new ExchangeTuxExchangeAPI());
                m_apiMap.Add("YOBIT", new ExchangeYobitAPI());
            }
            return m_apiMap;
        }

		// Get (sorted) list of symbols for the exchange (Exchange symbols)
        public IEnumerable<string> Symbols(string exchange, bool force = false)
        {
            if (!m_symbols.ContainsKey(exchange) || force)
            {
                m_symbols[exchange] = this[exchange].GetSymbols().OrderBy(s => s);
                m_gsymbols[exchange] = m_symbols[exchange].Select(s => this[exchange].ExchangeSymbolToGlobalSymbol(s)).OrderBy(s => s);
            }
            return m_symbols[exchange];
        }
        
        // Get (sorted) list of global symbols for the exchange (Global symbols)
        public IEnumerable<string> GlobalSymbols(string exchange, bool force = false)
        {
            if (!m_gsymbols.ContainsKey(exchange) || force)
            {
                m_symbols[exchange] = this[exchange].GetSymbols().OrderBy(s => s);
                m_gsymbols[exchange] = m_symbols[exchange].Select(s => this[exchange].ExchangeSymbolToGlobalSymbol(s)).OrderBy(s => s);
				// TODO: Why is BITTREX returning "inverted" GLOBAL symbols?
				if (exchange == "BITTREX")
				{
					var gs = m_gsymbols[exchange].Select(s => { var split = s.Split('-'); return split[1] + "-" + split[0]; }).OrderBy(s => s);
					//gs.ToList().ForEach(s => Console.WriteLine(s));
					m_gsymbols[exchange] = gs;
				}
			}
            return m_gsymbols[exchange];
        }

        /*public static string[] GetArgs(string commandLineArgs = null)
		{
			if (commandLineArgs == null)
				commandLineArgs = "bitfinex BTCUSDT 5 /Users/michael/Documents/hat_apis.csv.enc mywookie";

			string[] args = commandLineArgs.Split(' ');
			// Pass FIVE arguments: <exchange> <symbol> <minutes> <encrypted_api_file> <8_char_password>
			if (args.Length < 5)
			{
				Console.WriteLine("usage: dotnet vwap.dll <exchange> <symbol> <minutes> <encrypted_api_file> <8_char_password>");
				Console.WriteLine("\n   ex: dotnet vwap.dll binance BTCUSDT 60 /Users/david/apis.csv.enc A5s78rQz\n");
				return null;
			}
			return args;
		}*/

        public void Test()   //string exchange, string symbol)
		{
            StartWebSockets();
            Console.WriteLine("Press ENTER to shutdown.");
            Console.ReadLine();
            foreach (var s in m_sockets)
            {
                s.Dispose();
            }

            /*m_apiMap = GetAllExchangeApis(true);
            var api = m_apiMap["HITBTC"];
            var tsymbols = GetSymbols(api);
            tsymbols.Wait();
            var symbols = tsymbols.Result;
            var global_symbols = symbols.Select(s => api.ExchangeSymbolToGlobalSymbol(s));
            global_symbols.ToList().ForEach(s => Console.WriteLine(s));
            //Candles(api, "BTC-USD");

            //BalancesForAllExchanges();

            //Gator("BTC-USD", 20, 125, 20);
            //Gator("ETH-USD", 200, 125, 20);
            //Gator("ETH-USD", 200, 125);

            //OrderBookForAllExchanges("BTC-USDT");
            //OrderBookForAllExchanges("ETH-USD");
            //OrderBookForAllExchanges("ETH-USDT");

            return;

			RecentTradesForAllExchanges("BTC-USD");
            //RecentTradesForAllExchanges("BTC-USDT");
            //RecentTradesForAllExchanges("ETH-USD");
            //RecentTradesForAllExchanges("ETH-USDT");
            //RecentTradesForAllExchanges("BTC-ETH");

			//TickerForAllExchanges("BTC-USD");
			//TickerForAllExchanges("BTC-USDT");
			TickerForAllExchanges("ETH-BTC");
			//TickerForAllExchanges("XMR-ETH");
			//TickerForAllExchanges("XRP-ETH");

            // -----GetHistoricalTrades-----
			//var sinceDateTime = trades.First().Timestamp;
			//api.GetHistoricalTrades(callback, symbol, sinceDateTime);*/
		}

        /*private bool callback(IEnumerable<ExchangeTrade> trades)
		{
			trades.ToList().ForEach(t => t.Print());
			return true;
		}*/


        public void StartWebSockets()
        {
            Task.Run(() => WriteOutputQueue());

            m_sockets = new List<IDisposable>();
            m_sockets.Add(TestWebsocketsBinance(true));
            m_sockets.Add(TestWebsocketsBittrex(true));
            m_sockets.Add(TestWebsocketsPoloniex(true));
            m_sockets.Add(TestWebsocketsGdax(true));
            m_sockets.Add(TestWebsocketsBitfinex(true));
        }

        public void WriteOutputQueue(string pathname = null)
        {
            //var loc = Assembly.GetExecutingAssembly().Location;
            StreamWriter writer;
            if (pathname != null)
            {
                writer = new StreamWriter(pathname, append: true);
            }
            else
            {
                string filename = string.Format("tickers_{0}.DF.csv", DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));
                pathname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
                writer = new StreamWriter(pathname, append: false);
                writer.WriteLine(TickerOutput.CsvHeaders);
            }

            while (true)
            {
                TickerOutput tout;
                if (m_outputQ.TryDequeue(out tout))
                {
                    writer.Write(tout.ToCsv());
                    Thread.Sleep(1);
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        public void Candles(IExchangeAPI api, string global_symbol)
        {
            var symbol = api.GlobalSymbolToExchangeSymbol(global_symbol);
            int? limit = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            var candles = api.GetCandles(symbol, 60, startDate, endDate, limit);
            foreach (var c in candles)
            {
                Console.WriteLine("{0} {1} [{2} {3}] o:{4} h:{5} l:{6} c:{7} vol:{8}", c.Timestamp, c.PeriodSeconds, c.ExchangeName, c.Name, c.OpenPrice, c.HighPrice, c.LowPrice, c.ClosePrice, c.BaseVolume);
            }
        }

        public void HandleTickerUpdate(IExchangeAPI api, IReadOnlyCollection<KeyValuePair<string, ExchangeTicker>> tickers)
        {
            var symbolsInUpdate = new HashSet<string>();
            foreach (var kv in tickers)
            {
                string symbol;
                if (api is ExchangeBitfinexAPI)
                    symbol = kv.Key.Substring(0, 3) + "-" + kv.Key.Substring(3);
                else
                    symbol = api.ExchangeSymbolToGlobalSymbol(kv.Key);
                string exchange = api.Name;

                // 1st time for SYMBOL: Create a dictionary <exchange, stack<ExchangeTicker>>
                if (!m_tickers.ContainsKey(symbol))
                    m_tickers[symbol] = new ConcurrentDictionary<string, ConcurrentStack<ExchangeTicker>>();
                // 1st time for SYMBOL|EXCHANGE: Create a stack<ExchangeTicker>
                if (!m_tickers[symbol].ContainsKey(exchange))
                    m_tickers[symbol][exchange] = new ConcurrentStack<ExchangeTicker>();

                var ticker = kv.Value;
                m_tickers[symbol][exchange].Push(ticker);

                symbolsInUpdate.Add(symbol);
            }

            /*if (symbolsInUpdate.Contains("BTC-USD")) update("BTC-USD");
            if (symbolsInUpdate.Contains("BTC-USDT")) update("BTC-USDT");
            if (symbolsInUpdate.Contains("ETH-USD")) update("ETH-USD");
            if (symbolsInUpdate.Contains("ETH-USDT")) update("ETH-USDT");*/
        }

        private void update(string sym)
        {
            var tickerStackExchanges = m_tickers[sym].Keys;
            foreach (var exch in tickerStackExchanges)
            {
                ExchangeTicker t;
                if (m_tickers[sym][exch].TryPeek(out t))
                    Console.WriteLine("[{0,8} {1}] id:{2} b:{3} a:{4} l:{5}    vol: {6} / {7}", exch, sym, t.Id, t.Bid, t.Ask, t.Last, t.Volume.BaseVolume, t.Volume.ConvertedVolume);
                else
                    Console.WriteLine("couldn't peek: [{0,8} {1}]", exch, sym);
            }
        }

        public IDisposable TestWebsocketsBitfinex(bool display = false)
        {
            IExchangeAPI a = new ExchangeBitfinexAPI();
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                if (display) Console.WriteLine("BITFINEX {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                //HandleTickerUpdate(a, tickers);
                m_outputQ.Enqueue(new TickerOutput("BITFINEX", tickers));
            });
            return socket;
        }

        public IDisposable TestWebsocketsGdax(bool display = false)
        {
            IExchangeAPI a = new ExchangeGdaxAPI();
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                if (display) Console.WriteLine("GDAX     {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                //await Task.Run(async () => HandleTickerUpdate( a, tickers));
                //HandleTickerUpdate(a, tickers);
                m_outputQ.Enqueue(new TickerOutput("GDAX", tickers));
            });
            return socket;
        }

        public IDisposable TestWebsocketsPoloniex(bool display = false)
        {
            IExchangeAPI a = new ExchangePoloniexAPI();
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                if (display) Console.WriteLine("POLONIEX {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                //HandleTickerUpdate(a, tickers);
                m_outputQ.Enqueue(new TickerOutput("POLONIEX", tickers));
            });
            return socket;
        }

        public IDisposable TestWebsocketsBinance(bool display = false)
        {
            IExchangeAPI a = new ExchangeBinanceAPI();
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                if (display) Console.WriteLine("BINANCE  {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                //HandleTickerUpdate(a, tickers);
                m_outputQ.Enqueue(new TickerOutput("BINANCE", tickers));
            });
            return socket;
        }

        public IDisposable TestWebsocketsBittrex(bool display = false)
        {
            // create a web socket connection to the exchange. Note you can Dispose the socket anytime to shut it down.
            // the web socket will handle disconnects and attempt to re-connect automatically.
            IExchangeAPI a = new ExchangeBittrexAPI();
            /*using (var socket = b.GetTickersWebSocket((tickers) =>
            {
                Console.WriteLine("{0} tickers, first: {1}", tickers.Count, tickers.First());
            }))
            {
                Console.WriteLine("Press ENTER to shutdown.");
                Console.ReadLine();
            }*/
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                if (display) Console.WriteLine("BITTREX  {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                //HandleTickerUpdate(a, tickers);
                m_outputQ.Enqueue(new TickerOutput("BITTREX", tickers));
            });
            return socket;
        }

        public void BalancesForAllExchanges()
        {
            Console.WriteLine();

            List<Task> taskList = new List<Task>();
            foreach (var kv in m_apiMap)
            {
                var exchange = kv.Key;
                var api = kv.Value;
                var tb = Balance(api, exchange);
                taskList.Add(tb);
            }
            Task.WaitAll(taskList.ToArray());
        }

        private async Task Balance(IExchangeAPI api, string exchange)
        {
            try
            {
                var tamounts = await api.GetAmountsAsync();
                //var ticker = await api.GetTickerAsync(symbol);
                foreach (var kv in tamounts)
                {
                    var asset = kv.Key;
                    var amount = kv.Value;
                    Console.WriteLine("[{0,-10}]  {1}  {2:0.00000000}", exchange, asset, amount);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("***[{0}] ERROR: {1}***", exchange, ex.Message);
            }
        }


        public void Gator(string global_symbol, decimal amountRequested, decimal bips, int displayBest = 0)
		{
			AggregatedOrderBook ob;
            ob = OrderBookForAllExchanges(global_symbol);

			if (displayBest > 0)
			{
				Console.WriteLine("\n---BIDS---");
				for (int i = 0; i < displayBest; ++i)
					Console.WriteLine("{0,3} {1}", i, ob.Bids[i]);
				Console.WriteLine("\n---ASKS---");
				for (int i = 0; i < displayBest; ++i)
					Console.WriteLine("{0,3} {1}", i, ob.Asks[i]);
			}

			decimal mybid = 0.00M, myask = 0.00M;

			Console.WriteLine(new string('-', 120));
			Console.WriteLine("For amount {0} {1}:", amountRequested, global_symbol);
			decimal atotal = 0.0M;
            for (int i = 0; i < ob.AskCount; ++i)
            {
                atotal += ob.Asks[i].Amount;
                if (atotal >= amountRequested)
                {
                    var range = ob.Asks.GetRange(0, i + 1);
                    var sumPQ = range.Sum(x => x.Price * x.Amount);
                    var sumQ = range.Sum(x => x.Amount);
                    var avgPrice = sumPQ / sumQ;
					var excess = sumQ - amountRequested;
					myask = avgPrice + (avgPrice * bips / 10000);
                    Console.WriteLine("\nasks[{0}]   avg_price:{1:0.00}   {2}", i, avgPrice, PlusBipsString(avgPrice));  // ob.Asks[i].Price);
                    var aexchanges = ob.Asks.GetRange(0, i + 1).Select(x => x.Exchange).Distinct();
					Console.WriteLine("CUSTOMER BUYS");
					foreach (var e in aexchanges)
					{
						var exch = range.Where(x => x.Exchange == e);
						var price = exch.Max(x => x.Price);
						var amount = exch.Sum(x => x.Amount);
						if (e == range.Last().Exchange)
                            amount -= excess;
						Console.WriteLine("{0,-10} BUY  {1} at price {2}", e, amount, price);
					}
                    break;
                }
            }
			decimal btotal = 0.0M;
			for (int i = 0; i < ob.BidCount; ++i)
			{
				btotal += ob.Bids[i].Amount;
				if (btotal >= amountRequested)
				{
					var range = ob.Bids.GetRange(0, i + 1);
					var sumPQ = range.Sum(x => x.Price * x.Amount);
					var sumQ = range.Sum(x => x.Amount);
					var avgPrice = sumPQ / sumQ;
					var excess = sumQ - amountRequested;
					mybid = avgPrice - (avgPrice * bips / 10000M);
					Console.WriteLine("\nbids[{0}]   avg_price:{1:0.00}   {2}", i, avgPrice, MinusBipsString(avgPrice));   // ob.Bids[i].Price);
					var bexchanges = range.Select(x => x.Exchange).Distinct();
					Console.WriteLine("CUSTOMER SELLS");
					foreach (var e in bexchanges)
                    {
						var exch = range.Where(x => x.Exchange == e);
                        var price = exch.Min(x => x.Price);
                        var amount = exch.Sum(x => x.Amount);
						if (e == range.Last().Exchange)
							amount -= excess;
						Console.WriteLine("{0,-10} SELL {1} at price {2}", e, amount, price);
                    }
					break;
				}
			}
			Console.WriteLine();
			Console.WriteLine("at {0} bips:\n   MY ASK={1:0.00}\n   MY BID={2:0.00}", bips, myask, mybid);
			Console.WriteLine();
		}
        
		private string MinusBipsString(decimal amount)
		{
			return string.Format("-25={0:0.00}  -50={1:0.00}  -75={2:0.00}  -100={3:0.00}  -125={4:0.00}  -150={5:0.00}  -175={6:0.00}", amount - (amount * 0.0025M), amount - (amount * 0.0050M), amount - (amount * 0.0075M), amount - (amount * 0.0100M), amount - (amount * 0.0125M), amount - (amount * 0.0150M), amount - (amount * 0.0175M));
		}
        
        private string PlusBipsString(decimal amount)
        {
			return string.Format("+25={0:0.00}  +50={1:0.00}  +75={2:0.00}  +100={3:0.00}  +125={4:0.00}  +150={5:0.00}  +175={6:0.00}", amount + (amount * 0.0025M), amount + (amount * 0.0050M), amount + (amount * 0.0075M), amount + (amount * 0.0100M), amount + (amount * 0.0125M), amount + (amount * 0.0150M), amount + (amount * 0.0175M));
        }
        
		public void RecentTradesForAllExchanges(string global_symbol)
		{
			Console.WriteLine();
			foreach (var kv in m_apiMap)
			{
				var exchange = kv.Key;
				var api = kv.Value;
				RecentTrades(api, exchange, global_symbol);
			}
		}
        
		private void RecentTrades(IExchangeAPI api, string exchange, string global_symbol, bool displayTrades = false)
		{
			try
			{
				var symbol = api.GlobalSymbolToExchangeSymbol(global_symbol);       // "BTC-KRW" for Bithumb?

				var trades = api.GetRecentTrades(symbol);
				if (displayTrades) trades.ToList().ForEach(t => t.Print());
				int tradeCount = trades.Count();
				if (tradeCount == 0)
				{
					//Console.WriteLine("[{0,-10} {1,9}]  {2,5} trades", exchange, symbol, tradeCount);
					return;
				}
				Console.WriteLine("[{0,-10} {1,9}]  {2,5} trades", exchange, symbol, tradeCount);
			}
			catch (Exception ex)
			{
				//Console.WriteLine("***[{0} {1}] ERROR: {2}***", exchange, global_symbol, ""); //ex.Message);
			}
		}

        public void SymbolsForAllExchanges()
		{
			List<Task> taskList = new List<Task>();
            foreach (var kv in m_apiMap)
            {
                var exchange = kv.Key;
                var api = kv.Value;
                var s = GetSymbols(api);
                taskList.Add(s);
            }
            Task.WaitAll(taskList.ToArray());
			Console.WriteLine("here");

            /*m_symbols = new Dictionary<string, List<string>>();
            foreach (var exch in m_api.ExchangeIds)
            {
                var symbols = await m_api[exch].GetSymbolsAsync();
                m_symbols[exch] = symbols.ToList();
            }*/
		}
        
		private async Task<IEnumerable<string>> GetSymbols(IExchangeAPI api)
        {
            try
            {
				var symbols = await api.GetSymbolsAsync();
				return symbols;
            }
            catch (Exception ex)
            {
				//Console.WriteLine("***[{0} {1}] ERROR: {2}***", exchange, global_symbol, ""); //ex.Message);
				return null;
			}
        }

		public void PrintSymbols(IExchangeAPI api)
		{
			api.GetSymbols().ToList().ForEach(s => Console.WriteLine(s));
		}

		public void TickerForAllExchanges(string global_symbol)
		{
			Console.WriteLine();

			List<Task> taskList = new List<Task>();
			foreach (var kv in m_apiMap)
			{
				var exchange = kv.Key;
				var api = kv.Value;
				var t = Ticker(api, exchange, global_symbol);
				taskList.Add(t);
			}
			Task.WaitAll(taskList.ToArray());
		}
        
		private async Task Ticker(IExchangeAPI api, string exchange, string global_symbol)
		{
			try
			{
				var symbol = api.GlobalSymbolToExchangeSymbol(global_symbol);
				var ticker = await api.GetTickerAsync(symbol);
				//var ticker = api.GetTicker(symbol);
				Console.WriteLine("[{0,-10} {1,9}]      b:{2:0.00000000}      a:{3:0.00000000}", exchange, symbol, ticker.Bid, ticker.Ask);
			}
			catch (Exception ex)
			{
				//Console.WriteLine("***[{0} {1}] ERROR: {2}***", exchange, global_symbol, ""); //ex.Message);
			}
		}
           
		public AggregatedOrderBook OrderBookForAllExchanges(string global_symbol)
        {
            //Console.WriteLine();

			var tasks = new Dictionary<string, Task<ExchangeOrderBook>>();
			//var tasks = new List<Task<ExchangeOrderBook>>();
            foreach (var kv in m_apiMap)
            {
                var exchange = kv.Key;
                var api = kv.Value;
                var tob = OrderBook(api, exchange, global_symbol);
				tasks[exchange] = tob;
                //tasks.Add(tob);
                //tob.Result.
            }
			Task.WaitAll(tasks.Values.ToArray());
			//Task.WaitAll(tasks.ToArray());
			var bids = new List<OrderBookEntry>();
			var asks = new List<OrderBookEntry>();
			foreach (var kv in tasks)
			{
				var exchange = kv.Key;
				var book = kv.Value.Result;
				if (book == null || book.Bids == null || book.Asks == null) continue;
                var bb = book.Bids.Values.ToList();
                var aa = book.Asks.Values.ToList();
				bb.ForEach(b => bids.Add(new OrderBookEntry(exchange, b.Price, b.Amount)));
				aa.ForEach(a => asks.Add(new OrderBookEntry(exchange, a.Price, a.Amount)));
			}
			Console.WriteLine("{0} bids  {1} asks", bids.Count, asks.Count);
			return new AggregatedOrderBook(bids, asks);
        }
        
        private async Task<ExchangeOrderBook> OrderBook(IExchangeAPI api, string exchange, string global_symbol)
        {
            try
            {
                var symbol = api.GlobalSymbolToExchangeSymbol(global_symbol);
				var ob = await api.GetOrderBookAsync(symbol);
				Console.WriteLine("[{0,-10} {1,9}]    {2,5} bids: {3:0.00000000}    {4,5} asks: {5:0.00000000} ", exchange, symbol, ob.Bids.Count, ob.Bids.First().Value.Price, ob.Asks.Count, ob.Asks.First().Value.Price);
				return ob;
			}
            catch (Exception ex)
            {
                //Console.WriteLine("***[{0} {1}] ERROR: {2}***", exchange, global_symbol, ""); //ex.Message);
            }
			return null;
        }

	} // end of class ExchangeSharpRestApi

    /*public class TickerOutput
    {
        public string exchange { get; private set; }
        public DateTime timestamp { get; private set; }
        public IReadOnlyCollection<KeyValuePair<string, ExchangeTicker>> tickers { get; private set; }
        
        public TickerOutput(string exchange, IReadOnlyCollection<KeyValuePair<string, ExchangeTicker>> tickers)
        {
            this.exchange = exchange;
            this.tickers = tickers;
            this.timestamp = DateTime.Now;
        }

        public static string CsvHeaders => "DateTime,Exchange,Bid,Ask,Last,BaseVolume,ConvertedVolume,BaseSymbol,ConvertedSymbol";

        public string ToCsv()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kv in tickers)
            {
                var symbol = kv.Key;
                //var globalSymbol = 
                var t = kv.Value;
                var timeString = timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                sb.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", timeString, exchange, symbol, t.Bid, t.Ask, t.Last, t.Volume.BaseVolume, t.Volume.ConvertedVolume, t.Volume.BaseSymbol, t.Volume.ConvertedSymbol));
            }
            return sb.ToString();
        }
    } // end of class TickerOutput*/


    //================================================================================================
    public static class ExchangeSharpExtensionMethods
	{
		// Extension Method: Display contents of an ExchangeTrade object
        public static void Print(this ExchangeTrade t)
        {
            Console.WriteLine("{0} {1} {2}", t.Timestamp.ToDisplay(), t.Price, t.Amount);
        }

        public static string ToCsv(this ExchangeTicker t)
        {
            //var timeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return string.Format("{0},{1},{2},{3}", t.Bid, t.Ask, t.Last, t.Volume.BaseVolume);
        }

        public static string CsvHeaders(this ExchangeTicker t)
        {
            return "Bid,Ask,Last,Volume";
        }

	} // end of class ExchangeSharpExtensionMethods
	//================================================================================================

} // end of namespace
