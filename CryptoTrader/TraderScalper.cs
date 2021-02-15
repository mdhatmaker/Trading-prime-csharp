using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using CryptoApis.RestApi;
using CryptoTools;
using ExchangeSharp;
using CryptoTools.Models;
using CryptoTools.Messaging;
using CryptoTools.MathStat;
using CryptoTools.CryptoFile;
using CryptoApis;
using CryptoApis.ExchangeX.CoinMarketCap;

namespace CryptoTrader
{
    public class TraderScalper
    {
		ExchangeSharpApi m_api;
        //private List<IDisposable> m_sockets;
        private ProwlPub m_prowl;
        private Credentials m_creds;
        private bool m_notify = false;
        private PubnubPub m_pubnub;

        private BinanceRestApi m_binanceRestApi;
		private ConcurrentBag<ExchangeOrderResult> m_orders;
        private CandlestickMaker m_maker;
        private OrderManager m_om;
        private List<TradeSymbolRawCsvRecord> m_tradeSymbols;
        private List<TradeSymbolRawCsvRecord> m_activeTradeSymbols;

        private bool m_testOnly = true;

        public TraderScalper()
        {
            m_api = new ExchangeSharpApi(ExchangeSet.All);
            m_api.LoadCredentials(Credentials.CredentialsFile, Credentials.CredentialsPassword);
            m_creds = Credentials.LoadEncryptedCsv(Credentials.CredentialsFile, Credentials.CredentialsPassword);
            m_prowl = new ProwlPub(m_creds["PROWL"].Key, "Scalper");
            m_om = new OrderManager(m_creds);
            m_maker = new CandlestickMaker();
			m_orders = new ConcurrentBag<ExchangeOrderResult>();

            m_binanceRestApi = new BinanceRestApi(m_creds["BINANCE"].Key, m_creds["BINANCE"].Secret);

            ReadTradeSymbols();
		}

        public void Test()
		{
            m_testOnly = true;

            /*var bb = new CryptoApis.Exchange.BiboxRestApi(m_creds["BIBOX"].Key, m_creds["BIBOX"].Secret);
            var task = bb.Test();
            task.Wait();*/


            //CryptoTools.Cryptography.Cryptography.SimpleEncrypt("/Users/michael/Documents/cliff_apis.csv", "mywookie");
            //DisplayBalances(true);
            m_pubnub = new PubnubPub("demo", "demo");
            m_pubnub.PubnubConnected += M_Pubnub_PubnubConnected;
            m_pubnub.PubnubMessageReceived += M_Pubnub_PubnubMessageReceived;
            m_pubnub.Subscribe(new string[] { "awesomeChannel" });
            //m_pubnub.Test_original_code();

            while (true)
            {
                Thread.Sleep(5000);
                m_pubnub.Publish("awesomeChannel", string.Format("publish {0}", DateTime.Now));
            }

            //CryptoTools.Cryptography.Cryptography.SimpleEncrypt("/Users/michael/Documents/hat_apis.csv", "mywookie");
            //var bb = new CryptoApis.Exchange.BiboxRestApi(m_creds["BIBOX"].Key, m_creds["BIBOX"].Secret);
            //bb.Test();

            //m_prowl = new ProwlPub("7ea940aadd8f03f89a3e24ecf3e7b7465fcb6a6f", "Scalper"); // test cliff's Prowl
            //m_prowl.Send("Fill ETHUSD SOLD 582.21", "this is another simulated trade message");

            //DisplayAllFills();

            //DisplayBetaCalcs("BINANCE", true);

            //var ranks = DisplayRanks(maxRank: 150);
            //Console.WriteLine("{0} symbols with specified max CoinMarketCap rank", ranks.Count());

            //DisplayProfitTotals();

            //DisplayTradeSymbolATRs();
		}

        void M_Pubnub_PubnubConnected(EventArgs e)
        {
            m_pubnub.Publish("awesomeChannel", "Connected!!!");
        }

        void M_Pubnub_PubnubMessageReceived(PubnubMessageReceivedEventArgs e)
        {
            Console.WriteLine("[{0}] message.Channel: {1} {2} {3}", e.Channel, e.Message, e.Subscription, e.Timetoken);
        }

        // where exchange like "BINANCE"
        public void DisplayBetaCalcs(string exchange, bool convertCandlesToUsd)
        {
            var cBtc = m_maker.GetCandles(new XSymbol(exchange, "BTCUSDT"), (int)Minutes.Day);
            var cLtc = m_maker.GetCandles(new XSymbol(exchange, "LTCUSDT"), (int)Minutes.Day);
            var cEth = m_maker.GetCandles(new XSymbol(exchange, "ETHUSDT"), (int)Minutes.Day);
            var betaLtc = GMath.Beta(cLtc, cBtc);
            var betaEth = GMath.Beta(cEth, cBtc);
            Console.WriteLine("beta_LTCUSDT:{0:0.00}", betaLtc);
            Console.WriteLine("beta_ETHUSDT:{1:0.00}", betaEth);
            foreach (var ts in m_activeTradeSymbols)    //  m_tradeSymbols)
            {
                var symbol = m_api[exchange].GlobalSymbolToExchangeSymbol(ts.GlobalSymbol);
                var cX = m_maker.GetCandles(new XSymbol(exchange, symbol), (int)Minutes.Day);
                var beta = GMath.Beta(cX, cBtc, convertCandlesToUsd);
                Console.WriteLine("beta_{0}:{1:0.00}", symbol, beta);
            }
        }

        // Display all fills for the past day
        public void DisplayAllFills()
        {
            var fills = GetAllFills();
            int count = 0;
            fills.ToList().ForEach(o => Console.WriteLine("{0}  {1}", ++count, o));
        }

        // Display market cap rank (CoinMarketCap) for each trade symbol (both ACTIVE and INACTIVE) in "marketmaker_xsynbols.DF.csv" file
        // where maxRank is the maximum rank # (by market cap) used to filter the returned/displayed CoinMarketCapTicker objects
        public CoinMarketCapTickerList DisplayRanks(int maxRank = 500)
        {
            var tickers = new CoinMarketCapTickerList();
            var cmc = new CoinMarketCapApi();
            foreach (var ts in m_tradeSymbols)
            {
                var currency = ApiHelper.GetBaseCurrency(ts.GlobalSymbol);
                var t = cmc.GetRankingTicker(currency);
                if (t == null)
                    Console.WriteLine("*** Rank not found for {0}", currency);
                else
                {
                    if (t.rank <= maxRank)
                    {
                        t.Print();
                        tickers.Add(t);
                    }
                }
            }
            return tickers;
        }

        public async Task DisplayBalances(bool hideZeroBalances = false)
        {
            //CancelWorkingOrders();
            Thread.Sleep(10000);

            decimal unitSize = 0.01M;   // size of 1 unit (in BTC)
            decimal btcLow = 0.01M;     // indicate BUY if balance < btcLow
            decimal btcHigh = 0.04M;    // indicate SELL if balance > btcHigh
            Console.WriteLine("unit_size={0} BTC     btc_low={1} BTC   btc_high={2} BTC", unitSize, btcLow, btcHigh);

            var balances = await m_binanceRestApi.GetBalances();
            var keys = balances.Keys.OrderBy(x => x);
            decimal btcTotal = 0;
            foreach (var coin in keys)
            {
                try
                {
                    string symbol, indicator = " ";
                    decimal btcValue = 0, bid = 0, ask = 0, unit = 0;
                    if (coin == "btc")
                    {
                        symbol = "BTC";
                        btcValue = balances[coin].Amount;
                        indicator = "*";
                        Console.WriteLine("{0,-5} amt:{1,13} free:{2,13}  {3,-5}  btc:{4:0.00000000} {5}  unit:{6,7:0.00}  {7,8}:{8:0.00000000}-{9:0.00000000}", coin, balances[coin].Amount, balances[coin].Available, balances[coin].Currency, btcValue, indicator, unit, symbol, bid, ask);
                        btcTotal += btcValue;
                    }
                    else if (coin == "usdt")
                    {
                        symbol = "USDT";
                        btcValue = 0;       // TODO: get USDT value in BTC
                        indicator = "*";
                        Console.WriteLine("{0,-5} amt:{1,13} free:{2,13}  {3,-5}  btc:{4:0.00000000} {5}  unit:{6,7:0.00}  {7,8}:{8:0.00000000}-{9:0.00000000}", coin, balances[coin].Amount, balances[coin].Available, balances[coin].Currency, btcValue, indicator, unit, symbol, bid, ask);
                        btcTotal += btcValue;
                    }
                    else
                    {
                        symbol = coin.ToUpper() + "BTC";
                        var t = m_om.BinanceTickers[symbol];
                        bid = t.BidPrice;
                        ask = t.AskPrice;
                        var mid = (bid + ask) / 2;
                        btcValue = balances[coin].Amount * mid;
                        if (!hideZeroBalances || (btcValue > 0.001M))
                        {
                            unit = (unitSize / mid);
                            if (btcValue < btcLow)
                                indicator = "B";
                            else if (btcValue > btcHigh)
                                indicator = "S";
                            Console.WriteLine("{0,-5} amt:{1,13} free:{2,13}  {3,-5}  btc:{4:0.00000000} {5}  unit:{6,7:0.00}  {7,8}:{8:0.00000000}-{9:0.00000000}", coin, balances[coin].Amount, balances[coin].Available, balances[coin].Currency, btcValue, indicator, unit, symbol, bid, ask);
                            btcTotal += btcValue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR [{0}]: {1}", coin, ex.Message);
                }
            }
            var tbtc = m_om.BinanceTickers["BTCUSDT"];
            var btcusdt = (tbtc.BidPrice + tbtc.AskPrice) / 2;
            Console.WriteLine("\nTotal BTC:{0:0.00000000} ${1:0.00}", btcTotal, btcTotal * btcusdt);
            decimal usdTotal = 0;
            Console.WriteLine("Total USD:{0:0.00}", usdTotal);
            Console.WriteLine("TOTAL   $:{0:0.00}", btcTotal * btcusdt + usdTotal);
        }

        // Calculate and display the profit for each traded symbol for the previous nDays days
        // (AND display the daily total profit for all traded symbols)
        public void DisplayProfits(int nDays = 7)
        {
            var now = DateTime.Now;
            var dt = now.Subtract(TimeSpan.FromDays(nDays));
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 12, 0, 0, DateTimeKind.Local); // noon-to-noon

            var btcUsdTicker = m_api.bina.GetTicker("BTCUSDT");

            for (var dt1 = dt; dt1 < now; dt1 = dt1.AddDays(1))
            {
                var dt2 = dt1.AddDays(1);
                decimal btcTotal = DisplayProfitEachSymbol(dt1, dt2, btcUsdTicker);
                var usdTotal = btcUsdTicker.Last * btcTotal;
                Console.WriteLine("\n{0}    TOTAL: {1:0.00000000}    ${2:0.00}", dt1.ToString("ddd yyyy MMM-dd"), btcTotal, btcTotal * btcUsdTicker.Last);
                Console.WriteLine(new string('-', 90));
            }
        }

        // Given the number of (previous) days to display
        // Display/Return DAILY profit (in BTC) for each ACTIVE trade symbol in "marketmaker_xsynbols.DF.csv" file
        private decimal DisplayProfitEachSymbol(DateTime dt1, DateTime dt2, ExchangeTicker btcUsdTicker)
        {
            decimal btcTotal = 0;
            //var btcUsdTicker = m_api.bina.GetTicker("BTCUSDT");
            foreach (var ts in m_activeTradeSymbols)
            {
                var subtotal = DisplaySymbolProfit(ts.GlobalSymbol, btcUsdTicker.Last, dt1, dt2, display: false);
                btcTotal += subtotal;
                Thread.Sleep(500);
            }
            return btcTotal;
        }

        // Given a GlobalSymbol and starting/ending dates (dt1/dt2)
        // Display/Return profit for matched "BTC-*" trades (equal number of buys/sells) in BTC and USD
        private decimal DisplaySymbolProfit(string globalSymbol, decimal btcUsd, DateTime dt1, DateTime dt2, bool display = false)
        {
            var fills = GetFills(globalSymbol, dt1, dt2);
            var buys = fills.Where(o => o.IsBuy == true);
            var sells = fills.Where(o => o.IsBuy == false);

            // SELLS
            decimal sellPQ = 0, sellQ = 0, avgSell = 0;
            if (sells.Count() > 0)
            {
                sellPQ = sells.Sum(o => o.AmountFilled * o.AveragePrice);
                sellQ = sells.Sum(o => o.AmountFilled);
                avgSell = sellPQ / sellQ;
            }
            if (display) { int count = 0; sells.ToList().ForEach(o => Console.WriteLine("{0,3}  {1}", ++count, o)); }

            // BUYS
            decimal buyPQ = 0, buyQ = 0, avgBuy = 0;
            if (buys.Count() > 0)
            {
                buyPQ = buys.Sum(o => o.AmountFilled * o.AveragePrice);
                buyQ = buys.Sum(o => o.AmountFilled);
                avgBuy = buyPQ / buyQ;
            }
            if (display) { int count = 0; buys.ToList().ForEach(o => Console.WriteLine("{0,3}  {1}", ++count, o)); }

            var minQty = Math.Min(buyQ, sellQ);
            var totalDiff = (avgSell - avgBuy) * minQty;
            var totalTrades = buys.Count() + sells.Count();
            if (buys.Count() > 0 || sells.Count() > 0)
            {
                Console.Write("\n{0}  {1} FILLS           ", dt1.ToString("ddd yyyy MMM-dd"), globalSymbol);
                Console.WriteLine("Subtotal: {0,11:0.00000000}    ${1,6:0.00}  ({2} {3} trades)", totalDiff, totalDiff * btcUsd, globalSymbol, totalTrades);
                Console.WriteLine("{0} sells PQ:{1:0.00000000} Q:{2}  avgSell:{3:0.00000000}", sells.Count(), sellPQ, sellQ, avgSell);
                Console.WriteLine("{0} buys  PQ:{1:0.00000000} Q:{2}  avgBuy:{3:0.00000000}", buys.Count(), buyPQ, buyQ, avgBuy);
            }
            return totalDiff;
        }

        // Given a GlobalSymbol and a starting/ending date (dt1/dt2)
        // Return the fills between the two given dates (dt1 <= FillDate < dt2)
        private IEnumerable<XOrder> GetFills(string globalSymbol, DateTime dt1, DateTime dt2)
        {
            //var creds = m_api.Credentials["BINANCE"];
            //var api = new BinanceRestApi(creds.Key, creds.Secret);
            var api = m_binanceRestApi;
            //var orders = api.GetOpenOrders();
            var ts = m_activeTradeSymbols.Where(o => o.GlobalSymbol == globalSymbol).FirstOrDefault();
            if (ts == null) return new List<XOrder>().AsEnumerable();

            //Console.WriteLine("\nGetting fills for {0} starting {1}", ts.GlobalSymbol, dt1);
            var symbol = m_api[ts.Exchange].GlobalSymbolToExchangeSymbol(ts.GlobalSymbol);
            var orders = api.GetAllOrders(symbol);
            var filled = orders.Where(o => o.Result == OrderResult.Filled || o.Result == OrderResult.FilledPartially);
            var query = filled.Where(o => o.OrderDate >= dt1 && o.OrderDate < dt2).OrderBy(o => o.OrderDate);
            return query;
        }

        private IEnumerable<XOrder> GetAllFills(DateTime? afterDate = null)
        {
            //var dt1 = new DateTime(2018, 5, 31, 0, 0, 0);   //, DateTimeKind.Utc);
            DateTime dt1;
            if (afterDate == null)
            {
                var now = DateTime.Now;
                dt1 = now.Subtract(TimeSpan.FromDays(1));
            }
            else
                dt1 = afterDate.Value;

            //var creds = m_api.Credentials["BINANCE"];
            //var api = new BinanceRestApi(creds.Key, creds.Secret);
            var api = m_binanceRestApi;
            //var orders = api.GetOpenOrders();
            var filledOrders = new List<XOrder>();
            foreach (var ts in m_activeTradeSymbols)
            {
                //Console.WriteLine("Getting fills for {0}", ts.GlobalSymbol);
                var symbol = m_api[ts.Exchange].GlobalSymbolToExchangeSymbol(ts.GlobalSymbol);
                var orders = api.GetAllOrders(symbol);
                var filled = orders.Where(o => o.Result == OrderResult.Filled || o.Result == OrderResult.FilledPartially);
                filledOrders.AddRange(filled);
            }
            var query = filledOrders.AsEnumerable().Where(o => o.OrderDate >= dt1).OrderBy(o => o.OrderDate);
            return query;
        }

        /*// Use ExchangeSharpApi to display all Fills for a specified exchange for the past month (31 days)
        private void DisplayFills(string exchange, DateTime? afterDate = null)
		{
            //DateTime? afterDate = new DateTime(2018, 5, 26);
            if (afterDate == null)
            {
                var now = DateTime.Now;
                var dt = now.Subtract(TimeSpan.FromDays(31));
                afterDate = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, DateTimeKind.Utc);
            }

            Console.Write("Getting completed orders...");
			//var orders = m_api[exchange].GetCompletedOrderDetails(null, afterDate);
            var orders = m_api[exchange].GetCompletedOrderDetails();
            //var orders = m_api[exchange].GetOpenOrderDetails(null);

            Console.WriteLine("Done.");
            orders.ToList().ForEach(o => Console.WriteLine("{0} {1}", o.OrderDate.ToLocalTime(), o));

            var query = orders.GroupBy(o => new DateTime(o.OrderDate.Year, o.OrderDate.Month, o.OrderDate.Day));
            foreach (var dayGroup in query)
            {
                var fills = dayGroup.Where(o => o.Result == ExchangeAPIOrderResult.Filled || o.Result == ExchangeAPIOrderResult.FilledPartially);
                //fills.ToList().ForEach(o => Console.WriteLine(o));

                int count = 0;
                if (fills.Count() > 0)
                {
                    Console.WriteLine("---------------------------------- {0} ----------------------------------", dayGroup.Key.ToString("yyyy MMM-dd"));
                    fills.ToList().ForEach(o => Console.WriteLine("{0, 3} {1}", ++count, o));
                    Console.WriteLine("");
                }
            }
            //var d = dt1;
            //for (int d = 1; d <= 31; ++d)
            //{
            //    var dt1 = new DateTime(2018, m, d, 0, 0, 0, DateTimeKind.Utc);
            //    var dt2 = dt1.AddDays(1.0);
            //    //Console.WriteLine("{0}   {1}", dt1, dt2);
            //    var fills = orders.Where(o => o.Result == ExchangeAPIOrderResult.Filled || o.Result == ExchangeAPIOrderResult.FilledPartially)
            //                      .Where(o => o.OrderDate >= dt1 && o.OrderDate < dt2);
            //    //fills.ToList().ForEach(o => Console.WriteLine(o));

            //    int count = 0;
            //    if (fills.Count() > 0)
            //    {
            //        Console.WriteLine("---------------------------------- {0} ----------------------------------", dt1.ToString("yyyy MMM-dd"));
            //        fills.ToList().ForEach(o => Console.WriteLine("{0, 3} {1}", ++count, o));
            //        Console.WriteLine("");
            //    }
            //}
		}*/

        private void ReadTradeSymbols(bool force = false)
		{
            if (m_tradeSymbols != null && force != true) return;

            Console.WriteLine("--- Reading trade symbols:");

            // Read symbols and symbol-related data from file
            using (var fin = new InputFile<TradeSymbolRawCsvRecord>("marketmaker_xsymbols", Folder.misc_folder))
            {
                m_tradeSymbols = fin.ReadAll().ToList();
            }

            /*// Reverse the coins within each symbol name ("XXX-YYY" => "YYY-XXX")
            // I use this because the ExchangeSharp API seems to have a problem keeping its symbol order consistent!
            // TODO: Look into fixing this at the ExchangeSharp level
            foreach (var rec in m_tradeSymbols)
            {
                var gs = rec.GlobalSymbol;
                var split = gs.Split('-');
                rec.GlobalSymbol = split[1] + "-" + split[0];
            }
            using (var fout = new OutputFile<TradeSymbolRawCsvRecord>("marketmaker_xsymbols_reverse", Folder.misc_folder))
            {
                fout.WriteAll(m_tradeSymbols);
            }*/

            m_activeTradeSymbols = new List<TradeSymbolRawCsvRecord>();

            int activeCount = 0;
            foreach (var ts in m_tradeSymbols)
            {
                if (ts.InitialSide != 0 && ts.Amount != 0)
                {
                    Console.WriteLine("[{0,-9} {1,9}]  init_side:{2} size:{3}", ts.Exchange, ts.GlobalSymbol, ts.InitialSide, ts.Amount);
                    m_activeTradeSymbols.Add(ts);
                    ++activeCount;
                }
            }
            Console.WriteLine("--- {0} active out of {1} symbols.", activeCount, m_tradeSymbols.Count);

			/*m_xsymbols = new List<ExchangeSymbol>();

			Console.WriteLine("--- Reading trade symbols:");

			int count = 0, activeCount = 0;
            using (var fin = new InputFile<TradeSymbolRawCsvRecord>("marketmaker_xsymbols", Folder.misc_folder))
			{
				string line;
				while ((line = fin.ReadLine()) != null)
				{
					if (string.IsNullOrWhiteSpace(line)) continue;
					var split = line.Split(',');
					bool active = bool.Parse(split[0]); //  (split[0] == "1" ? true : false);
					var exchange = split[1];
					var gsymbol = split[2];
					var amount = decimal.Parse(split[3]);
					var xs = GetExchangeSymbol(gsymbol);
					if (active)
					{
						Console.WriteLine("[{0,-9} {1,9}]  size:{2}", xs.Exchange, xs.Symbol, amount);
						m_xsymbols.Add(xs);
						++activeCount;
					}
					++count;
				}
			}
			Console.WriteLine("--- {0} active out of {1} symbols.", activeCount, count);*/
		}

        // Given a Global market symbol ("ADA-BTC", "ETH-BTC", "LTC-BTC", ...)
		// Return the ExchangeSymbol for the exchange on which this market trades
        private XSymbol GetExchangeSymbol(string gsymbol)
		{
			XSymbol xs;
			if (m_api.GlobalSymbols("BINANCE").Contains(gsymbol))
                xs = new XSymbol("BINANCE", gsymbol);
            else if (m_api.GlobalSymbols("BITTREX").Contains(gsymbol))
				xs = new XSymbol("BITTREX", gsymbol);
            else if (m_api.GlobalSymbols("HITBTC").Contains(gsymbol))
				xs = new XSymbol("HITBTC", gsymbol);
            //else if (m_api.GlobalSymbols("HUOBI").Contains(gsymbol))
			//    xs = new ExchangeSymbol("HUOBI", symbol, gsymbol);
            else if (m_api.GlobalSymbols("YOBIT").Contains(gsymbol))
				xs = new XSymbol("YOBIT", gsymbol);
            else if (m_api.GlobalSymbols("CRYPTOPIA").Contains(gsymbol))
				xs = new XSymbol("CRYPTOPIA", gsymbol);
            else
				xs = new XSymbol("", gsymbol);
			return xs;
		}

        public void DisplayTradeSymbolATRs(bool showInactive = false)
		{
            List<TradeSymbolRawCsvRecord> tradeSymbols;
            if (showInactive == true)
                tradeSymbols = m_tradeSymbols;
            else
                tradeSymbols = m_activeTradeSymbols;

            foreach (var ts in tradeSymbols)
            {
                var symbol = m_api[ts.Exchange].GlobalSymbolToExchangeSymbol(ts.GlobalSymbol);
                DisplayATRs(new XSymbol(ts.Exchange, symbol));
            }
			/*DisplayATRs(new ExchangeSymbol("BINANCE", "ADABTC"));
            DisplayATRs(new ExchangeSymbol("BINANCE", "XRPBTC"));
            DisplayATRs(new ExchangeSymbol("BINANCE", "BNBBTC"));
            DisplayATRs(new ExchangeSymbol("BINANCE", "QTUMBTC"));
            DisplayATRs(new ExchangeSymbol("BINANCE", "NEOBTC"));
            DisplayATRs(new ExchangeSymbol("BINANCE", "LTCBTC"));
            DisplayATRs(new ExchangeSymbol("BINANCE", "BCCBTC"));*/
		}

        // Display the ATR value for different bar periods (60, 30, 5, 1) along with their value relative to price as %/bips
        private void DisplayATRs(XSymbol xs)
        {
			Console.WriteLine(new string('-', 100));
            //int[] periods = { 60, 30, 5, 1 };
			int[] periods = { 30 };
            foreach (int minutes in periods)
            {
                try
                {
                    var candles = m_maker.GetRecentCandles(xs, minutes);
                    var c = candles.Last();
                    //var atr = GetCurrentATR(xs, minutes, atrLength: 12);
                    var atr = new AverageTrueRange(candles, atrLength: 12);
                    var kv = atr.Values.Last();
                    var pct = (kv.Value / (double)c.ClosePrice) * 100;
                    Console.WriteLine("[{0,-8} {1,8}] {2,2} minutes     ATR: {3}   {4:0.00000000}   {5:0.0000}% ({6:0} bips)", xs.Exchange, xs.Symbol, minutes, kv.Key, kv.Value, pct, pct * 100);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[{0,-8} {1,8}] {2}", xs.Exchange, xs.Symbol, ex.Message);
                }
            }
        }

        // Retrieve the current list of ATR values
        private AverageTrueRange GetCurrentATR(XSymbol xs, int minutes, int atrLength)
        {
            var candles = m_maker.GetRecentCandles(xs, minutes);
            var atr = new AverageTrueRange(candles, atrLength);
            return atr;
        }

        // Retrieve the current candle for the given bar period (minutes)
        private XCandle GetCurrentCandle(XSymbol xs, int minutes)
        {
            var candles = m_maker.GetRecentCandles(xs, minutes);
            return candles.Last();
        }

        public void CancelWorkingOrders()
        {
            Console.Write("--- Canceling working orders...");
            var orders = m_api.bina.GetOpenOrderDetails();
            //var tasks = new List<Task>();
            foreach (var o in orders)
            {
                //tasks.Add(m_api.bina.CancelOrderAsync(o.OrderId, o.Symbol));
                Console.Write(".");
                m_api.bina.CancelOrder(o.OrderId, o.Symbol);
            }
            //Task.WaitAll(tasks.ToArray());
            Console.WriteLine("({0})", orders.Count());
        }

        public void StartTrading(bool testOnly = false)
		{
            m_testOnly = testOnly;

            //decimal initPrice = 0.00002699M;    // BUY: initPrice-scalp     SELL: initPrice+scalp    (top/bottom of ATR-12 Renko box is good place to start)
            //decimal size = 500.0M;              // amount to buy/sell on each scalp
            //decimal scalp = 0.00000024M;        // actually HALF of full scalp price (half height of ATR-12 Renko box is good place to start)

            // CANCEL WORKING ORDERS
            if (m_testOnly == false) CancelWorkingOrders();

            // START TASK TO MONITOR OPEN ORDERS
			Task.Run(() => OpenOrdersTask("BINANCE"));

            // Start the Scalper task for each active symbol
            foreach (var ts in m_activeTradeSymbols)
            {
                if (ts.InitialSide == 0 || ts.Amount == 0) continue;                            // symbol is INACTIVE if InitialSide is zero or Amount is zero
                OrderSide side = (ts.InitialSide == -1 ? OrderSide.Sell : OrderSide.Buy);       // InitialSide: -1 = SELL, 1 = BUY
                var symbol = m_api[ts.Exchange].GlobalSymbolToExchangeSymbol(ts.GlobalSymbol);  // convert from Global to Exchange symbol
                //Console.WriteLine("{0},{1},{2},{3}", side, ts.Exchange, symbol, ts.Amount);
                StartScalper(new XSymbol(ts.Exchange, symbol), side, ts.Amount);     // start the Scalper task
            }
        }

        public void StartScalper(XSymbol xs, OrderSide initSide, decimal size, int minutes = 30)
		{
            var candles = m_maker.GetRecentCandles(xs, minutes);
            var c = candles.Last();
            decimal initPrice = c.ClosePrice;
            //var atr = GetCurrentATR(xs, minutes, atrLength: 12);
            var atr = new AverageTrueRange(candles, atrLength: 12);
            var atrDate = atr.Values.Last().Key;
            var atrValue = atr.Values.Last().Value;
            decimal scalp = (decimal) atrValue / 2.0M;       // scalp is half of ATR
            var kv = atr.Values.Last();
            var pct = (kv.Value / (double)c.ClosePrice) * 100;
            Console.WriteLine("[{0,-8} {1,8}] {2,2} minutes     ATR: {3}   {4:0.00000000}   {5:0.0000}% ({6,3:0} bips)   scalp:{7:0.00000000}  size:{8,5}  init_side:{9}", xs.Exchange, xs.Symbol, minutes, kv.Key, kv.Value, pct, pct * 100, scalp, size, initSide);

            if (m_testOnly == false)
            {
                Task.Run(() => ScalperTask(xs, initSide, initPrice, size, scalp));
            }
            
			Thread.Sleep(m_testOnly ? 3500 : 1500);     // slight delay so we don't choke the API
        }

		private void OpenOrdersTask(string exchange)
		{            
			var api = m_api[exchange];

			int count = 0;
			while (true)
			{
				var openOrders = api.GetOpenOrderDetails();
                openOrders = openOrders.OrderBy(o => o.Symbol);
				m_orders.Clear();
				int orderCount = 0;
				++count;
				if (count % 3 == 0) Console.WriteLine("\n{0}", DateTime.Now);
				foreach (var oo in openOrders)
				{
					m_orders.Add(oo);
					if (count % 3 == 0) Console.WriteLine(">>> {0,2}) {1}", ++orderCount, oo.ToStr());
				}
                
				Thread.Sleep(m_testOnly ? 15000 : 5000);
			}
		}

        private void ScalperTask(XSymbol xs, OrderSide initSide, decimal initPrice, decimal amount, decimal scalp)
        {
			Console.WriteLine("\n===== Starting Scalper  {0}   init_price:{1:0.00000000}  amount:{2}  scalp:{3:0.00000000} =====", xs, initPrice, amount, scalp);
            
            var api = m_api[xs.Exchange];

            string strategyId = "scalper";
            ExchangeOrderResult working;
            
            if (initSide == OrderSide.Sell)
                working = m_om.PlaceOrder(api, xs.Symbol, initSide, initPrice + scalp, amount, strategyId);
            else
                working = m_om.PlaceOrder(api, xs.Symbol, initSide, initPrice - scalp, amount, strategyId);
            Console.WriteLine("\n   NEW working order >>> {0} ", working.ToStr());

            int count = 0;
            while (true)
            {
				Thread.Sleep(m_testOnly ? 25000 : 15000);

                var t = api.GetTicker(xs.Symbol);
                 
                //if (working.Result == ExchangeAPIOrderResult.Filled)
                // If our working order has been filled
                if (m_orders.Where(o => o.OrderId == working.OrderId).Count() == 0)
                {
                    var buySell = working.IsBuy ? "BOT" : "SOLD";
					var prowlEvt = string.Format("Fill {0} {1} {2:0.00000000}", working.Symbol, buySell, working.Price);
                    if (working.IsBuy)
                        working = m_om.PlaceOrder(api, xs.Symbol, OrderSide.Sell, initPrice + scalp, amount, strategyId);
                    else
                        working = m_om.PlaceOrder(api, xs.Symbol, OrderSide.Buy, initPrice - scalp, amount, strategyId);
                    Console.WriteLine("\n*** {0} ***\n   NEW working order >>> {1}", prowlEvt, working.ToStr());
                    m_prowl.Send(prowlEvt, working.ToMsgStr());
                }
                else if (Math.Abs(t.Last - initPrice) > 2 * scalp)  // if price breakout of range
                {
                    string dir = "up";
                    if (t.Last < initPrice) dir = "down";
                    if (working.Side() == initSide)
                        m_om.Cancel(working);
                    initPrice = t.Last;
                    if (initSide == OrderSide.Sell)
                        working = m_om.PlaceOrder(api, xs.Symbol, initSide, initPrice + scalp, amount, strategyId);
                    else
                        working = m_om.PlaceOrder(api, xs.Symbol, initSide, initPrice - scalp, amount, strategyId);
                    Console.WriteLine("\n*** Price Breakout {0} {1} ***\n   NEW working order >>> {2} ", xs.Symbol, dir, working.ToStr());
					//m_prowl.Send(string.Format("Breakout {0} {1}", working.Symbol, dir), working.ToMsgStr());
                }

                if (count % 4 == 0) Console.Write(".");
                //if (count % 10 == 0) Console.WriteLine("{0}", working.ToStr());
            }
        }

        // Send a Prowl notification message
        private void Notify(string evt, string msg)
		{
			var shortTimeString = DateTime.Now.ToString("MMM-dd HH:mm:ss");
            if (m_notify) m_prowl.Send(evt, string.Format("{0} {1}", shortTimeString, msg));
		}
              
        // For each coin in which we have a position, sell to USD (or to BTC then BTC-to-USD)
        public async Task CloseAllPositions(bool sellBtc, decimal minBtcValue = 0.001M)
        {
            CancelWorkingOrders();

            Thread.Sleep(10000);

            decimal unitSize = 0.01M;   // size of 1 unit (in BTC)
            decimal btcLow = 0.01M;     // indicate BUY if balance < btcLow
            decimal btcHigh = 0.04M;    // indicate SELL if balance > btcHigh
            Console.WriteLine("unit_size={0} BTC     btc_low={1} BTC   btc_high={2} BTC", unitSize, btcLow, btcHigh);

            var balances = await m_binanceRestApi.GetBalances();
            var keys = balances.Keys.OrderBy(x => x);
            decimal btcTotal = 0;
            foreach (var coin in keys)
            {
                if (balances[coin].Amount > 0)
                {
                    try
                    {
                        string symbol, indicator = " ";
                        decimal btcValue = 0, bid = 0, ask = 0, unit = 0;
                        if (coin == "btc" && balances[coin].Amount > minBtcValue)
                        {
                            symbol = "BTC";
                            btcValue = balances[coin].Amount;
                            indicator = "*";
                            if (sellBtc)
                            {
                                symbol = "BTCUSDT";
                                var xtbtc = m_om.BinanceTickers["BTCUSDT"];
                                var xbid = xtbtc.BidPrice;
                                Console.WriteLine("{0} SELL {1} at {2}", symbol, balances[coin].Amount, xbid);
                                ApiHelper.Sell(m_api["BINANCE"], symbol, xbid, balances[coin].Amount);
                            }
                        }
                        else if (coin == "usdt")
                        {
                            symbol = "USDT";
                            btcValue = 0;       // TODO: get USDT value in BTC
                            indicator = "*";
                        }
                        else
                        {
                            symbol = coin.ToUpper() + "BTC";
                            var t = m_om.BinanceTickers[symbol];
                            bid = t.BidPrice;
                            ask = t.AskPrice;
                            var mid = (bid + ask) / 2;
                            btcValue = balances[coin].Amount * mid;
                            unit = (unitSize / mid);
                            if (btcValue > minBtcValue)
                            {
                                Console.WriteLine("{0} SELL {1} at {2}", symbol, balances[coin].Amount, bid);
                                ApiHelper.Sell(m_api["BINANCE"], symbol, bid, balances[coin].Amount);
                                //Console.WriteLine("{0,-5} amt:{1,13} free:{2,13}  {3,-5}  btc:{4:0.00000000} {5}  unit:{6,7:0.00}  {7,8}:{8:0.00000000}-{9:0.00000000}", coin, balances[coin].Amount, balances[coin].Available, balances[coin].Currency, btcValue, indicator, unit, symbol, bid, ask);
                            }
                        }
                        btcTotal += btcValue;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR [{0}]: {1}", coin, ex.Message);
                    }
                }
            }
            var tbtc = m_om.BinanceTickers["BTCUSDT"];
            var btcusdt = (tbtc.BidPrice + tbtc.AskPrice) / 2;
            Console.WriteLine("\nTotal BTC:{0:0.00000000}   {1:0.00000000}:{2:0.00000000}", btcTotal, tbtc.BidPrice, tbtc.AskPrice);
            //decimal usdTotal = 0;
            //Console.WriteLine("Total USD:{0:0.00}", usdTotal);
            //Console.WriteLine("TOTAL   $:{0:0.00}", btcTotal * btcusdt + usdTotal);
        }
        /*// where exchange like "BINANCE"
        // where symbol like "ETHUSDT"
        private void BuySellIndicatorTask(string exchange, string symbol)
        {
            Console.WriteLine("DateTime,Exchange,Symbol,Bid,Ask");
            
            while (true)
            {
                var now = DateTime.Now;
                var timeString = now.ToString("yyyy-MM-dd HH:mm:ss");

                var ticker = m_api[exchange].GetTicker(symbol);
                
                Console.WriteLine("{0},{1},{2},{3},{4}", timeString, exchange, symbol, ticker.Bid, ticker.Ask);

                Thread.Sleep(20000);
            }
        }*/

    } // end of class TraderScalper
} // end of namespace
