using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CsvHelper;
using CryptoAnalysis.Charts;
using CryptoApis;
using CryptoTools.CryptoFile;
using CryptoTools;
using CryptoTools.Models;
using CryptoTools.MathStat;
using static CryptoTools.Global;

namespace CryptoAnalysis
{
    public class Analyzer
    {
		private CandlestickMaker m_maker;

        public void AnalyzerTest()
		{
			m_maker = new CandlestickMaker();

			//DownloadHistoricalData("BINANCE", "XMRBTC"); return;

			/*var pathname = Path.Combine(Folder.crypto_folder, "analyzer_output.txt");
			OpenCoutFile(pathname);

			var renko = new Renko("BINANCE", "TRXBTC", 12);
			//var renko = new Renko("BINANCE", "NEOUSDT", 12);

			CloseCoutFile();*/


			var xsym = new XSymbol("BINANCE", "BNBUSDT");
			AnalyzeATR(xsym, 30, 12);

			return;

			BinanceRenko("XLMBTC");

			BinanceRenko("ADAUSDT");
			BinanceRenko("BCCUSDT");
			BinanceRenko("BNBUSDT");
			BinanceRenko("BTCUSDT");
			BinanceRenko("ETHUSDT");
			BinanceRenko("LTCUSDT");
			BinanceRenko("NEOUSDT");
			BinanceRenko("QTUMUSDT");
			BinanceRenko("XRPUSDT");

			BinanceRenko("XMRBTC");
			BinanceRenko("ZECBTC");
			BinanceRenko("ZRXBTC");
		}

        public void AnalyzeATR(XSymbol xs, int minutes, int atrLength)
		{
			var candles = m_maker.ReadCandles(xs.Exchange, xs.Symbol, minutes);
			var atr = new AverageTrueRange(candles, atrLength);
			atr.Values.ToList().ForEach(kv => Console.WriteLine("{0,20}   {1:0.00000000}", kv.Key, kv.Value));
		}

		public void BinanceRenko(string symbol, int nbars = 12)
		{
			var pathname = Path.Combine(Folder.crypto_folder, string.Format("analyzer_output_BINANCE_{0}.txt", symbol));
            OpenCoutFile(pathname);
            
			var renko = new Renko("BINANCE", symbol, nbars);

			CloseCoutFile();
		}

        // where exchange like "BINANCE"
        // where symbol like "TRXBTC"
        public void DownloadHistoricalData(XSymbol xs)
		{            
            m_maker.CreateCandlesFile(xs, 60 * 24, 3);
			var it = 5;
            m_maker.CreateCandlesFile(xs, 60, it);
			m_maker.CreateCandlesFile(xs, 30, 2 * it);
			m_maker.CreateCandlesFile(xs, 15, 4 * it);
			m_maker.CreateCandlesFile(xs, 5, 12 * it);
            m_maker.CreateCandlesFile(xs, 1, 60 * it);
		}

        public void CrossExchangeSpreadAnalysis(string datafile)
        {
            using (var textReader = new StreamReader(datafile))
            using (var csv = new CsvReader(textReader))
            {
                //var anonymousTypeDefinition =
                //{
                //    _DateTime = default(DateTime),
                //    Exchange = string.Empty,
                //    Symbol = string.Empty,
                //    Bid = default(decimal),
                //    Ask = default(decimal)
                //};

                //var records = csv.GetRecords<MyClass>();
                /*var record = new MyClass();
                var records = csv.EnumerateRecords(record);
                foreach (var r in records)
                {
                    Console.WriteLine(r);
                }*/

                var tdata = new List<TickerData>();

                string[] filterSymbols = { "BTC-USD", "BTCUSD", "USDT_BTC", "BTCUSDT", "USDT-BTC" };

                Console.Write("Reading data file...");
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var t = csv.GetRecord<TickerData>();
                    //if (t.Exchange)
                    if (filterSymbols.Contains(t.Symbol))
                    {
                        tdata.Add(t);
                    }
                }
                Console.WriteLine("Done.\n");

                var diffsX = new List<decimal[]>();
                var diffsY = new List<decimal[]>();

                var tickers = new Dictionary<string, TickerData>();
                foreach (var t in tdata)
                {
                    tickers[t.Symbol] = t;
                    if (tickers.Count == 5)
                    {
                        var timeString = t.DateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        /*var m1 = tickers["BTC-USD"].MidPrice;
                        var m2 = tickers["BTCUSD"].MidPrice;
                        var m3 = tickers["USDT_BTC"].MidPrice;
                        var m4 = tickers["BTCUSDT"].MidPrice;
                        var m5 = tickers["USDT-BTC"].MidPrice;*/

                        var b1 = tickers["BTC-USD"].Bid;        // GDAX (1)
                        var b2 = tickers["BTCUSD"].Bid;         // Bitfinex (2)
                        var b3 = tickers["USDT_BTC"].Bid;       // Poloniex (3)
                        var b4 = tickers["BTCUSDT"].Bid;        // Binance (4)
                        var b5 = tickers["USDT-BTC"].Bid;       // Bittrex (5)

                        var a1 = tickers["BTC-USD"].Ask;
                        var a2 = tickers["BTCUSD"].Ask;
                        var a3 = tickers["USDT_BTC"].Ask;
                        var a4 = tickers["BTCUSDT"].Ask;
                        var a5 = tickers["USDT-BTC"].Ask;

                        var a = new decimal[10];
                        a[0] = a1 - b2;
                        a[1] = a1 - b3;
                        a[2] = a1 - b4;
                        a[3] = a1 - b5;
                        a[4] = a2 - b3;
                        a[5] = a2 - b4;
                        a[6] = a2 - b5;
                        a[7] = a3 - b4;
                        a[8] = a3 - b5;
                        a[9] = a4 - b5;

                        var b = new decimal[10];
                        b[0] = b1 - a2;
                        b[1] = b1 - a3;
                        b[2] = b1 - a4;
                        b[3] = b1 - a5;
                        b[4] = b2 - a3;
                        b[5] = b2 - a4;
                        b[6] = b2 - a5;
                        b[7] = b3 - a4;
                        b[8] = b3 - a5;
                        b[9] = b4 - a5;

                        Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}", timeString, a[0], b[0], a[1], b[1], a[2], b[2], a[3], b[3], a[4], b[4], a[5], b[5], a[6], b[6], a[7], b[7], a[8], b[8], a[9], b[9]);

                        diffsX.Add(a);
                        diffsY.Add(b);
                    }
                }

                var maxX = new decimal[10];
                var minX = new decimal[10];
                var maxY = new decimal[10];
                var minY = new decimal[10];

                for (int i = 0; i < 10; ++i)
                {
                    maxX[i] = diffsX.Max(x => x[i]);
                    minX[i] = diffsX.Min(x => x[i]);
                    maxY[i] = diffsY.Max(y => y[i]);
                    minY[i] = diffsY.Min(y => y[i]);
                    var avgX = diffsX.Average(x => x[i]);
                    var avgY = diffsY.Average(y => y[i]);
                    var stdX = diffsX.Select(x => x[i]).StdDev();
                    var stdY = diffsY.Select(y => y[i]).StdDev();

                    Console.WriteLine("{0,3}   min:{1:0.00} max:{2:0.00} ({3:0.00}) avg:{4:0.00} std:{5:0.00}", i, minX[i], maxX[i], maxX[i] - minX[i], avgX, stdX);
                    Console.WriteLine("{0,3}   min:{1:0.00} max:{2:0.00} ({3:0.00}) avg:{4:0.00} std:{5:0.00}", "", minY[i], maxY[i], maxY[i] - minY[i], avgY, stdY);
                    Console.WriteLine();
                }


                Console.WriteLine("here!!!");
            }
        }

    } // end of class Analyzer

    public class Diffs
    {
        public decimal d1 { get; set; }
        public decimal d2 { get; set; }
        public decimal d3 { get; set; }
        public decimal d4 { get; set; }
        public decimal d5 { get; set; }
        public decimal d6 { get; set; }
        public decimal d7 { get; set; }
        public decimal d8 { get; set; }
        public decimal d9 { get; set; }
        public decimal d10 { get; set; }
    }

    public class TickerData
    {
        public DateTime DateTime { get; set; }
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Last { get; set; }
        public decimal BaseVolume { get; set; }
        public decimal ConvertedVolume { get; set; }
        public string BaseSymbol { get; set; }
        public string ConvertedSymbol { get; set; }

        public decimal MidPrice => (Bid + Ask) / 2M;

        public override string ToString()
        {
            return string.Format("{0} [{1}|{2}] b:{3} a:{4} l:{5} vol:{6}", DateTime, Exchange, Symbol, Bid, Ask, Last, BaseVolume);
        }
    }

} // end of namespace
