using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using CryptoApis.ExchangeX.CoinMarketCap;
using CryptoApis.RestApi;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CryptoTools;
using CryptoTools.CryptoFile;
using static CryptoTools.Global;
using CryptoApis;
using CryptoTools.Models;
using CryptoTools.MathStat;

namespace CryptoTrader
{
    public class Sploders
    {
		//private List<string> m_workingExchanges = new List<string>() { "Cryptopia", "YoBit", "Livecoin", "HitBTC", "Poloniex", "Bleutrade", "Kucoin", "Bittrex", "Huobi", "Binance", "Bithumb", "Kraken", "Bitfinex", "Tux Exchange", "Nanex", "EtherDelta (ForkDelta)", "Waves Decentralized Exchange" };
		private List<string> m_workingExchanges = new List<string>() { "Cryptopia", "YoBit", "Livecoin", "HitBTC", "Poloniex", "Bleutrade", "Kucoin", "Bittrex", "Huobi", "Binance", "Bithumb", "Kraken", "Bitfinex", "Tux Exchange" };

        private ExchangeSharpApi m_api;
        //private Dictionary<string, List<string>> m_symbols;
		private SortedDictionary<DateTime, Dictionary<string, CoinMarketCapTicker>> m_rawGainers24;

		private CoinMarketCapApi m_cmc;
        private CandlestickMaker m_maker;

        public Sploders()
        {
            m_api = new ExchangeSharpApi(ExchangeSet.All);
			m_rawGainers24 = new SortedDictionary<DateTime, Dictionary<string, CoinMarketCapTicker>>();
			m_cmc = new CoinMarketCapApi();
            m_maker = new CandlestickMaker();
		}

        public void Test()
        {
            //DisplayGainers(25);
            //return;

            DisplayRealizedVolatility(25);
            return;

            var xs = new XSymbol("BINANCE", "IOTXBTC");
            FindBreakout(xs);
            //FindAllBreakouts("HITBTC");
        }

        // Check ALL symbols on a specified exchange for a breakout
        public void FindAllBreakouts(string exchange)
        {
            var symbols = m_api[exchange].GetSymbols().OrderBy(x => x);
            FindBreakout(new XSymbol(exchange, symbols.Last()));
            return;
            foreach (var s in symbols)
            {
                FindBreakout(new XSymbol(exchange, s));
            }
        }

        // Check a given ExchangeSymbol for breakout
        public void FindBreakout(XSymbol xs)
        {
            try
            {
                int periodMinutes = 1;
                var candles = m_api[xs.Exchange].GetCandles(xs.Symbol, periodMinutes * 60, null, null, 1000);
                Console.WriteLine("[{0} {1}] candles count: {2}", xs.Exchange, xs.Symbol, candles.Count());
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
        }

        // Display realized volatility for top-ranked (by market cap) cryptos from CoinMarketCap
        private void DisplayRealizedVolatility(int limit = 50)
        {
            int numberOfBars = 12;
            var top = m_cmc.GetRankings(limit);
            foreach (var t in top)
            {
                var exchange = "BINANCE";
                string symbol;
                string[] mainSymbols = { "ADA", "BCC", "BNB", "BTC", "EOS", "ETH", "IOTA", "LTC", "NEO", "QTUM", "TUSD", "XLM", "XRP" };
                if (mainSymbols.Contains(t.symbol))
                    symbol = t.symbol + "USDT";
                else
                    symbol = t.symbol + "BTC";
                var xs = new XSymbol(exchange, symbol);
                var candles = m_maker.GetCandles(xs, 1440, 1);
                if (candles.Count < numberOfBars) continue;
                var rvol = new RealizedVolatility(candles, numberOfBars);
                //var mr = CoinMarketCapApi.GetMarkets(t.name);
                //var marketExchanges = mr.Select(m => m.exchange).Distinct();
                Console.WriteLine("{0,4} {1,-7}  {2,6:0.00}", t.rank, t.symbol, rvol.Value);
            }
        }

        // Display biggest gainers from CoinMarketCap
        private void DisplayGainers(int limit = 50)
        {
            var g24 = m_cmc.GetBiggestGainers24h(limit);
            foreach (var t in g24)
            {
                var mr = CoinMarketCapApi.GetMarkets(t.name);
                var marketExchanges = mr.Select(m => m.exchange).Distinct();
                Console.WriteLine("{0,4} {1,-7}  {2,10:0}  1h:{3,6:0.00}%  24h:{4,6:0.00}%  7d:{5,6:0.00}%  {6}", t.rank, t.symbol, t.market_cap_usd, t.percent_change_1h, t.percent_change_24h, t.percent_change_7d, string.Join('|', marketExchanges));
            }
        }

        public async void OldTest()
        {
            /*m_api["HITBTC"].DisplaySymbols();
			m_api["YOBIT"].DisplaySymbols();
			return;*/

            //var exch = "HITBTC";
            //var sym = "BTC-IOTX";
            //var exchanges = new string[] { "BINANCE", "POLONIEX", "BITTREX", "HITBTC", "YOBIT", "CRYPTOPIA" };
            var exchanges = new string[] { "CRYPTOPIA" };
            var cmcCoins = m_cmc.GetRankings().Select(c => c.symbol);
            Console.WriteLine("Checking {0} currencies from CoinMarketCap", cmcCoins.Count());
            foreach (var exch in exchanges) //.OrderBy(e => e))
            {
                Console.Write("\nGetting symbols for {0} exchange...", exch);
                var symbols = m_api[exch].GetSymbols().OrderBy(s => s);
                Console.WriteLine("{0} symbols retrieved.", symbols.Count());
                //symbols.ToList().ForEach(s => Console.Write("{0} ", s));
                //Console.WriteLine();
                foreach (var coin in cmcCoins)
                {
                    try
                    {
                        if (coin == "LTC" || coin == "DOGE" || coin == "ETH" || coin == "BTC" || coin == "USDT")
                            continue;
                        var ethGlobalSymbol = "ETH-" + coin;
                        var btcGlobalSymbol = "BTC-" + coin;
                        if (exch == "HITBTC" || exch == "YOBIT" || exch == "CRYPTOPIA")
                        {
                            ethGlobalSymbol = coin + "-ETH";
                            btcGlobalSymbol = coin + "-BTC";
                        }
                        //Console.WriteLine("Checking {0}/{1} on {2}", ethGlobalSymbol, btcGlobalSymbol, exch);
                        var ethSymbol = m_api[exch].GlobalSymbolToExchangeSymbol(ethGlobalSymbol);
                        var btcSymbol = m_api[exch].GlobalSymbolToExchangeSymbol(btcGlobalSymbol);
                        //Console.WriteLine("{0} {1}", ethSymbol, btcSymbol);
                        if (symbols.Contains(ethSymbol) || symbols.Contains(btcSymbol))
                        {
                            var match = symbols.Where(s => s.Contains(ethSymbol) || s.Contains(btcSymbol));
                            Console.Write("--- {0} currency {1}:  ", exch, coin);
                            foreach (var m in match)
                            {
                                Console.Write("{0} ", m);
                            }
                            Console.WriteLine();
                            //match.Print(string.Format("--- {0}: currency {1} ---", exch, coin));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: {0}", ex.Message);
                    }
                }
            }
            return;
            /*symbols.Print(string.Format("--- symbols for exchange {0} ---", exch));
            var candles = m_api[exch].GetCandles(symbol, periodSeconds: 5 * 60);
            foreach (var c in candles)
            {
                Console.WriteLine(c);
            }
            return;*/

            var g1 = m_cmc.GetBiggestGainers1h();
            var g24 = m_cmc.GetBiggestGainers24h();
            g1.Print("\n--- Biggest Gainers 1-Hour ---");
            g24.Print("\n--- Biggest Gainers 24-Hour ---");

			//BacktestSplodersFile();

			//Task.Run(() => GainersTask());

			// TODO: Move this symbol code to the ExchangeSharpRestApi class
			/*m_symbols = new Dictionary<string, List<string>>();
            foreach (var exch in m_api.ExchangeIds)
            {
                var symbols = await m_api[exch].GetSymbolsAsync();
                m_symbols[exch] = symbols.ToList();
            }*/

			//Console.ReadLine();

			return;

            foreach (var exchId in m_api.ExchangeIds)
            {
                try
                {
                    var tickers = await m_api[exchId].GetTickersAsync();
                    var timeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    foreach (var kv in tickers)
                    {
                        //var symbol = kv.Key;
                        //var ticker = kv.Value;
                        Console.WriteLine("{0},{1},{2},{3}", timeString, exchId, kv.Key, kv.Value.ToCsv());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[{0}] {1}", exchId, ex.Message);
                }
            }

            Thread.Sleep(20000);
        }
        
        public void RecordData()
		{
			Task.Run(() => GainersTask());
		}

		struct symbol_move
        {
            public string symbol { get; set; }
            public decimal min { get; set; }
            public decimal max { get; set; }
            public decimal move => max - min;
        }

        public void BacktestSplodersFile()
		{
			var dataMap = new Dictionary<string, SortedDictionary<DateTime, SplodersRawCsvRecord>>();

            string path = "/Users/michael/Dropbox/dev/csharp/CryptoTrader/bin/debug/netcoreapp2.0";
            string filename = "sploders_2018-05-10_002343.DF.csv";
            string pathname = Path.Combine(path, filename);
            using (var reader = new StreamReader(pathname))
            using (var csv = new CsvReader(reader))
            {
                csv.Read();
                csv.ReadHeader();

				//int count = 0;         
                while (csv.Read())
                {
                    var record = csv.GetRecord<SplodersRawCsvRecord>();
					if (!dataMap.ContainsKey(record.Symbol))
						dataMap[record.Symbol] = new SortedDictionary<DateTime, SplodersRawCsvRecord>();
					var data = dataMap[record.Symbol];
					data[DateTime.Parse(record.Timestamp)] = record;
                    //if (++count % 1000 == 0) Console.Write(".");
                }

                //Console.WriteLine("\n\n{0} records ({1} skipped)", recordList.Count, skipCount);
                Console.WriteLine("\n\n{0} symbols in map\n", dataMap.Keys.Count);
            }

			//var sorted = set.OrderBy(r => r.Timestamp);
			//sorted.Take(1250).ToList().ForEach(r => Console.WriteLine(r.ToCsv()));

			var moves = new List<symbol_move>();
			foreach (var symbol in dataMap.Keys)
			{
				var sm = new symbol_move();
				var records = dataMap[symbol];
				sm.symbol = symbol;
				sm.max = records.Max(r => decimal.Parse(r.Value.PercentChg24h));
                sm.min = records.Min(r => decimal.Parse(r.Value.PercentChg24h));
				moves.Add(sm);
			}

			var symbols = moves.OrderByDescending(m => m.move).Select(m => m.symbol);
			foreach (var symbol in symbols) // dataMap.Keys)
			{
				var records = dataMap[symbol];

				/*foreach (var kv in records)
				{
					var timestamp = kv.Key;
					var record = kv.Value;
					Console.WriteLine("{0}  {1,20} {2,6:0.00}%", symbol, timestamp, decimal.Parse(record.PercentChg24h));
				}*/

				var rf = records.First();
				var rl = records.Last();
				var max = records.Max(r => decimal.Parse(r.Value.PercentChg24h));
				var min = records.Min(r => decimal.Parse(r.Value.PercentChg24h));
				var hours = rl.Key.Subtract(rf.Key).TotalHours;
				Console.WriteLine("{0,-6} {1,4:0.0} hours   first:{2,6:0.00}  last:{3,6:0.00}  min:{4,6:0.00}  max:{5,6:0.00}   max-min:{6,6:0.00}     {7}", symbol, hours, rf.Value.PercentChg24h, rl.Value.PercentChg24h, min, max, max-min, rf.Value.Exchanges);
			}  

			DisplayRecords(dataMap["BAS"]);
			DisplayRecords(dataMap["LANA"]);
			DisplayRecords(dataMap["ATX"]);
			DisplayRecords(dataMap["XBL"]);
			DisplayRecords(dataMap["AIB"]);
		}
        
        private void DisplayRecords(SortedDictionary<DateTime, SplodersRawCsvRecord> records)
		{
			Console.WriteLine();
            foreach (var kv in records)
            {
                var timestamp = kv.Key;
                var record = kv.Value;
                Console.WriteLine("{0}  {1,20} {2,6:0.00}%", kv.Value.Symbol, timestamp, decimal.Parse(record.PercentChg24h));
            }
		}
             
		public void GainersTask(bool display = false)
		{
			var headers = CoinMarketCapTicker.CsvHeaders + ",Exchanges";
			var writer = GFile.CreateOutputFile("sploders_raw", headers);

			if (display) Console.WriteLine(headers);
            
			while (true)
			{
				var g24 = m_cmc.GetBiggestGainers24h();

				var now = DateTime.Now;
				m_rawGainers24[now] = new Dictionary<string, CoinMarketCapTicker>();
				int count = 0;
				foreach(var t in g24)
				{
					m_rawGainers24[now][t.symbol] = t;

                    var mr = CoinMarketCapApi.GetMarkets(t.name);
					var marketExchanges = mr.Select(m => m.exchange).Distinct();
                    //var mr = CoinMarketCapApi.GetMarkets(t.name, removeDuplicates: true);
                    //var marketExchanges = mr.Select(x => x.pair);
					var exchanges = marketExchanges.Intersect(m_workingExchanges);
                    if (exchanges.Count() > 0)
                    {
						var exchangesString = string.Join('|', exchanges);
						var output = string.Format("{0},{1}", t.ToCsv(), exchangesString);
						if (display) Console.WriteLine(output);
						writer.WriteLine(output);
						++count;
                    }
				}
				writer.FlushAsync();
				var timeString = now.ToString("yyyy-MM-dd HH:mm:ss");
				Console.WriteLine("     {0}  {1} symbols output", timeString, count);

				Thread.Sleep(40000);
			}
		}

        /*public void GainersTask()
        {
            var myExchanges = new List<string>() { "Cryptopia", "YoBit", "Livecoin", "HitBTC", "Poloniex", "Bleutrade", "Kucoin", "Bittrex", "Huobi", "Binance", "Bithumb", "Kraken", "Bitfinex", "Okex", "Tux Exchange", "Nanex", "EtherDelta (ForkDelta)", "Waves Decentralized Exchange" };

            while (true)
            {
                Console.WriteLine("\n---Biggest Gainers 1h---");
                var gainers1h = BiggestGainers1h();
                foreach (var t in gainers1h)
                {
                    var mr = GetMarkets(t.name);
                    var exchanges = mr.Select(m => m.exchange).Distinct();
                    if (exchanges.Intersect(myExchanges).Count() > 0)
                    {
                        Console.WriteLine(t.ToString());
                    }
                }

                Console.WriteLine("\n---Biggest Gainers 24h---");
                var gainers24h = BiggestGainers24h();
                foreach (var t in gainers24h)
                {
                    var mr = GetMarkets(t.name);
                    var exchanges = mr.Select(m => m.exchange).Distinct();
                    if (exchanges.Intersect(myExchanges).Count() > 0)
                    {
                        Console.WriteLine(t.ToString());
                    }
                }

                Thread.Sleep(60000);
            }
        }*/
        
        private void DisplayExchangesForGainers()
        {
            var uniqueExchanges = new HashSet<string>();

            // Biggest 1-HOUR gainers from CoinMarketCap
            var gainers1h = m_cmc.GetBiggestGainers1h();
            uniqueExchanges.Clear();
            foreach (var t in gainers1h)
            {
                var mr = CoinMarketCapApi.GetMarkets(t.name);
                mr.Select(m => m.exchange).ToList().ForEach(exch => uniqueExchanges.Add(exch));
            }
            Console.WriteLine("\n---Exchanges for Biggest Gainers 1h---");
            uniqueExchanges.ToList().ForEach(exch => Console.WriteLine(exch));
            
            // Biggest 24-HOUR gainers from CoinMarketCap
            var gainers24h = m_cmc.GetBiggestGainers24h();
            uniqueExchanges.Clear();
            foreach (var t in gainers24h)
            {
				var mr = CoinMarketCapApi.GetMarkets(t.name);
                mr.Select(m => m.exchange).ToList().ForEach(exch => uniqueExchanges.Add(exch));
            }
            Console.WriteLine("\n---Exchanges for Biggest Gainers 24h---");
            uniqueExchanges.ToList().ForEach(exch => Console.WriteLine(exch));
        }

        

        /*private void PrintTicker(CryptoRestApis.ExchangeX.CoinMarketCap.CoinMarketCapTicker t)
        {
            Console.WriteLine("{0,-6} {1,-25} {2,7:0.00}% {3,7:0.00}% {4,7:0.00}%  {5:0.00000000}btc {6:0.00000000}usd", t.symbol, t.name, t.percent_change_1h, t.percent_change_24h, t.percent_change_7d, t.price_btc, t.price_usd);
        }*/



        // The past version of the CoinMarketCap Gainers monitor did NOT account for duplicates
        public void RemoveSplodersRawFileDuplicates()
        {
            var set = new HashSet<SplodersRawCsvRecord>();

            string path = "/Users/michael/Dropbox/dev/csharp/CryptoTrader/bin/debug/netcoreapp2.0";
            string filename = "sploders_raw_2018-05-10_002343.DF.csv";
            string pathname = Path.Combine(path, filename);
            using (var reader = new StreamReader(pathname))
            using (var csv = new CsvReader(reader))
            {
                csv.Read();
                csv.ReadHeader();

                var recordList = new List<SplodersRawCsvRecord>();
                int count = 0;

                while (csv.Read())
                {
                    var record = csv.GetRecord<SplodersRawCsvRecord>();
                    set.Add(record);
                    if (++count % 1000 == 0) Console.Write(".");
                }

                //Console.WriteLine("\n\n{0} records ({1} skipped)", recordList.Count, skipCount);
                Console.WriteLine("\n\n{0} records out of original {1}\n", set.Count, count);
            }

            var sorted = set.OrderBy(r => r.Timestamp);
            sorted.Take(1250).ToList().ForEach(r => Console.WriteLine(r.ToCsv()));

            var outputPathname = pathname.Replace("_raw", "");
            using (var writer = new StreamWriter(outputPathname))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteHeader<SplodersRawCsvRecord>();
                csv.NextRecord();
                csv.WriteRecords<SplodersRawCsvRecord>(sorted);
            }
        }

		// We *shouldn't* need this method again: It was created to remove extra commas in the ToCsv() method of CoinMarketCapTicker 
		// where pathname like "/Users/michael/Dropbox/dev/csharp/CryptoTrader/bin/debug/netcoreapp2.0/sploders_raw_2018-05-10_002343.DF.csv"
        private void RemoveExtraCsvCommas(string pathname)
		{
			using (var reader = new StreamReader(pathname))
			using (var csv = new CsvReader(reader))
			using (var writer = new StreamWriter(pathname + ".txt"))
			{
				var commaCounts = new HashSet<int>();
				while (true)
				{
					var line = reader.ReadLine();
					if (line == null) break;
					int commaCount = line.Count(ch => ch == ',');
					if (commaCount == 14)
					{
						Console.WriteLine(line);
						writer.WriteLine(line);
					}
					else if (commaCount > 14)
					{
						var split = line.Split(',');
						var beg = split.Take(8);
						var end = split.TakeLast(6);
						var mid = split.Skip(8).Take(commaCount - 13);
						//split[8] + split[9] + split[10] + split[11]; // 17
						//split[8] + split[9] + split[10]; // 16
						//split[8] + split[9]; // 15
						// last 6
						var line_beg = string.Join(',', beg);
						var line_mid = string.Concat(mid);
						var line_end = string.Join(',', end);
						var updated_line = string.Join(',', line_beg, line_mid, line_end);
						Console.WriteLine("*****" + updated_line);
						writer.WriteLine(updated_line);
					}
					else
					{
						throw new FormatException(string.Format("Line must have at least 15 fields (has {0})", commaCount + 1));
					}

					commaCounts.Add(commaCount);
				}
				Console.WriteLine("\nComma counts: {0}", string.Join(',', commaCounts));
				Console.WriteLine("Updated file output: {0}", pathname + ".txt");
			}
		}


    } // end of class Sploders

} // end of namespace
