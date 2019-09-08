using System;
using System.Collections.Generic;
using System.Linq;
using CryptoApis.RestApi;
using CryptoTools;
using CryptoTools.MathStat;
using CryptoTools.CryptoFile;
using CryptoTools.Cryptography;
using CryptoTools.CryptoSystem;
using static CryptoTools.Global;
using ExchangeSharp;
//using XPlot.Plotly;
//using Microsoft.FSharp.Core;
using CryptoTools.Backtest;
using CryptoTools.Models;

namespace CryptoApis
{
    public enum Minutes { Hour=60, Day=1440, Week=10080 }

	public class CandlestickMaker
	{
		ExchangeSharpApi m_api;
		//private ProwlPub m_prowl;
		//private Credentials m_creds;
		//private bool m_notify = false;

		public CandlestickMaker()
		{
			m_api = new ExchangeSharpApi();
            m_api.LoadCredentials(Credentials.CredentialsFile, Credentials.CredentialsPassword);
		}

		public void Test()
		{
			BacktestMACD(-0.2M, 0.0M);
			BacktestMACD(-0.2M, 0.5M);
			BacktestMACD(-0.2M, 0.7M);
			BacktestMACD(-0.3M, 0.0M);
			BacktestMACD(-0.4M, 0.0M);

			/*// 12/26 "slow" moving average for BNBUSD...buy when slow ma hits -0.2 and scale in more at -0.4
			var candles = ReadCandles("BINANCE", "BNBUSDT", 60);
			//var ema = GMath.GetCandlesEMA(candles, 26);
			var macd = GMath.GetCandlesMACD(candles, 12, 26, 9);
			int count = 0;
			foreach (var kv in macd)
			{
				if (kv.Value.Signal <= -0.2M)
				{
					if (kv.Value.Signal <= -0.4M)
						Console.Write("**** ");
					else
						Console.Write("** ");
					count++;
					Console.WriteLine("{0} {1:0.0000}", kv.Key, kv.Value.Signal);
				}
			}
			Console.WriteLine("Count: {0} out of {1}", count, macd.Count());

			//XPlot.Plotly.PlotlyChart chart = new PlotlyChart();
			//var layout = new Layout.Layout();
			////layout.barmode = 
			////var trace = new XPlot.Plotly.Graph.Candlestick();
			////trace.open = 
			//var trace = new List<XPlot.Plotly.Graph.Scatter>();
			//foreach (var kv in macd)
			//{
			//	var datum = new Graph.Scatter();
			//	datum.x = kv.Key.Timestamp;
			//	datum.y = kv.Value.Signal;
			//	trace.Add(datum);
			//}
			//chart.Plot<Graph.Scatter>(trace, FSharpOption<Layout.Layout>.Some(layout), FSharpOption<IEnumerable<string>>.Some(macd.Select(p => p.Key.Timestamp.ToString())));
			//chart.Show();

			//CreateCandlesFile("BINANCE", "BNBUSDT", 60);
			//CreateCandlesFile("BINANCE", "BTCUSDT", 60);*/
		}

        public void BacktestMACD(decimal entryPrice, decimal exitPrice)
		{
			// 12/26 "slow" moving average for BNBUSD...buy when slow ma hits -0.2 and scale in more at -0.4
            var candles = ReadCandles("BINANCE", "BNBUSDT", 60);
            //var ema = GMath.GetCandlesEMA(candles, 26);
            var macd = GMath.GetCandlesMACD(candles, 12, 26, 9);
			var backtest = new Backtest(string.Format("Entry:{0} Exit:{1}", entryPrice, exitPrice));
			BacktestRoundTrip inTrade = null;
            foreach (var kv in macd)
            {
				if (inTrade != null)
				{
					if (kv.Value.Signal >= exitPrice)
					{
						inTrade.Exit = new BacktestTrade(OrderSide.Sell, exitPrice, kv.Key);
						//Console.WriteLine("EXIT : {0} {1:0.0000}", kv.Key, kv.Value.Signal);
						backtest.Add(inTrade);
						inTrade = null;
					}
				}
				else
				{
					if (kv.Value.Signal <= entryPrice)
					{
						inTrade = new BacktestRoundTrip();
						inTrade.Entry = new BacktestTrade(OrderSide.Buy, entryPrice, kv.Key);
						//Console.WriteLine("ENTRY: {0} {1:0.0000}", kv.Key, kv.Value.Signal);
					}
				}
            }
			backtest.PrintTrades();
		}

		public List<XCandle> ReadCandles(string exchange, string symbol, int minutes)
		{
			var result = new List<XCandle>();
			string filename = string.Format("candles_{0}_{1}_{2}", exchange, symbol, GetBarPeriod(minutes * 60));
            var fin = new InputFile<XCandle>(filename, Folder.crypto_folder, false);
			string line;
			while ((line = fin.ReadLine()) != null)
			{
				var candle = new XCandle();
				Reflection.SetPropertyValues<XCandle>(candle, line);
				result.Add(candle);
				//Console.WriteLine(candle);
			}
			fin.Close();
			return result;
		}

        // where exchange like "BINANCE" and symbol like "ETHUSDT"
        public void CreateCandlesFiles(XSymbol xs)
        {
            CreateCandlesFile(xs, 24 * 60, 3);
            CreateCandlesFile(xs, 60, 5);
            CreateCandlesFile(xs, 30, 10);
            CreateCandlesFile(xs, 5, 20);
            CreateCandlesFile(xs, 1, 40);
        }

        // where exchange like "BINANCE" and symbol like "BNBUSDT"
        public void CreateCandlesFile(XSymbol xs, int minutes, int iterationCount = 10)
		{
            var candles = GetCandles(xs, minutes, iterationCount, true);
            Console.WriteLine("Total candles: {0}", candles.Count);
			WriteCandles(xs, minutes, candles);
		}
        
		public void WriteCandles(XSymbol xs, int minutes, List<XCandle> candles)
		{
			string filename = string.Format("candles_{0}_{1}_{2}", xs.Exchange, xs.Symbol, GetBarPeriod(minutes * 60));
			var fout = new OutputFile<MarketCandle>(filename, Folder.crypto_folder, false);
			// Write candles to file
			foreach (var c in candles)
			{
				var propValues = Reflection.GetPropertyValues<XCandle>(c);
				var svalues = propValues.Select(p => string.Format("{0}", p));
				var csv = string.Join(",", svalues);
				//Console.WriteLine(csv);
                fout.WriteLine(csv);
			}
			fout.Close();
		}

        // Get the most recent candles with the specified bar period (minutes)
        // Returns List of candles sorted by Timestamp
        public List<XCandle> GetRecentCandles(XSymbol xs, int minutes = 60)
        {
            var candles = Candles(xs, minutes).ToList();
            return candles.OrderBy(c => c.Timestamp).ToList();
        }

        // Returns List of candles sorted by Timestamp
        public List<XCandle> GetCandles(XSymbol xs, int minutes = 60, int iterationCount = 1, bool force = false, bool display = false)
		{
			var candles = Candles(xs, minutes).ToList();
            int count = candles.Count;
            if (count == 0) return candles;
            DateTime firstTime = candles.First().Timestamp;
            
            for (int i = 0; i < iterationCount; ++i)
            {
				if (display) Console.WriteLine("[{0,-8} {1,-6}] Retrieved {2} candles", xs.Exchange, xs.Symbol, count);
                //Console.WriteLine("first: {0}", firstTime);
                var st = firstTime.Subtract(TimeSpan.FromMinutes(count * minutes));
                var c  = Candles(xs, minutes, st);
				var lastTime = c.Last().Timestamp;
				if (lastTime >= firstTime)
				{
					var subset = c.Where(r => r.Timestamp < firstTime);
					candles.InsertRange(0, subset.ToList());
				}
				else
                    candles.InsertRange(0, c);
                if (xs.Exchange == "BINANCE" && c.Count() < 1000)
                    break;
				firstTime = candles.First().Timestamp;
            }
			return candles.OrderBy(c => c.Timestamp).ToList();
		}

        private IEnumerable<XCandle> Candles(XSymbol xs, int periodMinutes = 1, DateTime? startDate = null, DateTime? endDate = null, int? limit = null)
        {
			try
			{
				int periodSeconds = periodMinutes * 60;
				var candles = m_api[xs.Exchange].GetCandles(xs.Symbol, periodSeconds, startDate, endDate, limit);
				return candles.ToXCandles();
			}
            catch (Exception ex)
			{
				Console.WriteLine("{0}", ex.Message);
				return new List<XCandle>();
			}
        }

		public void RealizedVolTest()
		{
            var xs = new XSymbol("BINANCE", "TRXBTC");
			var rvol0 = RVol(xs, 60, 12, 252 * 24);    // 12 x 1-hour bars
            
			//var rvol1 = RVol("BINANCE", "TRXBTC", 1440, 12);          // 12 x 1-day bars
			//var rvol2 = RVol("BINANCE", "BTCUSDT", 1440, 12);
			//var rvol3 = RVol("BINANCE", "ETHUSDT", 1440, 12);
		}

        public void RangeHeightsTest()
		{
            var xs = new XSymbol("BINANCE", "TRXBTC");
			RangeHeights(xs, 60, 12, 252 * 24);    // 12 x 1-hour bars
		}

		public void RangeHeights(XSymbol xs, int minutes = 1440, int numberOfBars = 12, int numerator = 252, int iterationCount = 1)
		{
			var candles = GetCandles(xs, minutes, iterationCount, true);
            var rvol = new RealizedVolatility(candles, numberOfBars, numerator);
			var heights = rvol.RangeStdValues;
			var candleMap = rvol.CandleMap;
            
			var minuteCandles = GetCandles(xs, 1, 10, true);

			var nstddev = 2.0M;                 // two standard deviations
			foreach (var kv in heights)
			{
				var ts = kv.Key;
				var stdValue = kv.Value;
				var candle = candleMap[ts];
				Console.WriteLine("{0}    {1:0.00000000}", ts, 2 * nstddev * stdValue); // stddev goes up and down, so multiply by 2
			}
            //var t = m_api.bina.GetTicker(symbol);
		}

		public decimal RVol(XSymbol xs, int minutes = 1440, int numberOfBars = 12, int numerator = 252, int iterationCount = 1)
		{
			var candles = GetCandles(xs, minutes, iterationCount, true);
            var rvol = new RealizedVolatility(candles, numberOfBars, numerator);
			var value = rvol.Value;
			var t = m_api.bina.GetTicker(xs.Symbol);
			//rvol.Range(out var low, out var high, t.MidPrice(), 2.0M);
			//Console.WriteLine("[{0,-8} {1,-6}]  minutes:{2}  nbars:{3}      Range: {4:0.00000000} - {5:0.00000000}", exchange, symbol, minutes, numberOfBars, low, high);
			//Console.WriteLine("[{0,-8} {1,-6}]  minutes:{2}  nbars:{3}      RealizedVol={4:0.00000000}", exchange, symbol, minutes, numberOfBars, value);
			var rangeHeight = rvol.RangeHeight(t.MidPrice(), 2.0M);         // height of +/- 2 stddev range around current Ticker price
			Console.WriteLine("[{0,-8} {1,-6}]  minutes:{2}  nbars:{3}      RangeHeight: {4:0.00000000}", xs.Exchange, xs.Symbol, minutes, numberOfBars, rangeHeight);
			return rangeHeight;
		}

    } // end of class CandlestickMaker

} // end of namespace
