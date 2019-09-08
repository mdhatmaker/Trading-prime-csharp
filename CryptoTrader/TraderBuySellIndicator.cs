using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoApis.RestApi;
using CryptoTools;
using CryptoTools.MathStat;
using CryptoTools.Messaging;
using ExchangeSharp;
using CryptoTools.Cryptography;
using CryptoApis;

namespace CryptoTrader
{
    public class TraderBuySellIndicator
    {
		ExchangeSharpApi m_api;
		private List<IDisposable> m_sockets;
		private ProwlPub m_prowl;
		private Credentials m_creds;
		private bool m_notify = false;
        
		public TraderBuySellIndicator()
        {
            m_api = new ExchangeSharpApi();
			m_api.LoadCredentials(Credentials.CredentialsFile, Credentials.CredentialsPassword);
            m_creds = Credentials.LoadEncryptedCsv(Credentials.CredentialsFile, Credentials.CredentialsPassword);
        }

        public void Test()
        {

        }

        // where exchange like "BINANCE" and symbol like "ETHUSDT"
        public void StartBuySellIndicator(string exchange, string symbol, bool notify = false)
        {
            m_prowl = new ProwlPub(m_creds["PROWL"].Key, symbol);
            var t1 = Task.Run(() => BuySellIndicatorTask(exchange, symbol));
            //t1.Wait();
        }

		public void StartOrderWebsockets()
        {
            m_sockets = new List<IDisposable>();
            m_sockets.Add(TestWebsocketsGdax(true));
            //m_sockets.Add(TestWebsocketsBinance(true));
            //m_sockets.Add(TestWebsocketsBittrex(true));
            //m_sockets.Add(TestWebsocketsPoloniex(true));
            //m_sockets.Add(TestWebsocketsBitfinex(true));
        }

        public IDisposable TestWebsocketsGdax(bool display = false)
        {
            IExchangeAPI a = new ExchangeGdaxAPI();
            var socket = a.GetCompletedOrderDetailsWebSocket((or) =>
            {
                if (display) Console.WriteLine("[GDAX {0,-9}] {1} at {2}", or.Symbol, or.AmountFilled, or.Price);
                
                //await Task.Run(async () => HandleTickerUpdate( a, tickers));
                //HandleTickerUpdate(a, tickers);
                //m_outputQ.Enqueue(new TickerOutput("GDAX", tickers));
            });
            return socket;
        }

        public void TestGdaxOrder()
        {
            var order = new ExchangeOrderRequest();
            order.OrderType = OrderType.Limit;
            order.Symbol = "ETH-USD";
            order.Price = 390.19M;
            order.Amount = 0.1M;
            order.IsBuy = true;
            //order.ShouldRoundAmount = true;
            //order.RoundAmount();
            //var parameters = order.ExtraParameters;
            order.ExtraParameters["post_only"] = true;

            //var api = new ExchangeGdaxAPI();
            //api.LoadAPIKeysUnsecure(m_creds["GDAX"].Key, m_creds["GDAX"].Secret, m_creds["GDAX"].Passphrase);
            //var api = m_apiMap["GDAX"];
            m_api.gdax.PlaceOrder(order);
        }

        private string GetIndicators(decimal c1, decimal c5, decimal c15, decimal c30, decimal c60)
        {
            string c60x = "+", c30x = "+", c15x = "+", c5x = "+", c1x = "+";
            if (Math.Sign(c60) < 0) c60x = "-";
            if (Math.Sign(c30) < 0) c30x = "-";
            if (Math.Sign(c15) < 0) c15x = "-";
            if (Math.Sign(c5) < 0) c5x = "-";
            if (Math.Sign(c1) < 0) c1x = "-";
            var indicators = string.Format("{0}{1}{2}{3}{4}", c60x, c30x, c15x, c5x, c1x);
            return indicators;
        }

        private string GetIndicators(decimal c1, decimal c5, decimal c30, decimal c60)
        {
            string c60x = "+", c30x = "+", c5x = "+", c1x = "+";
            if (Math.Sign(c60) < 0) c60x = "-";
            if (Math.Sign(c30) < 0) c30x = "-";
            if (Math.Sign(c5) < 0) c5x = "-";
            if (Math.Sign(c1) < 0) c1x = "-";
            var indicators = string.Format("{0}{1}{2}{3}", c60x, c30x, c5x, c1x);
            return indicators;
        }


        // Given 4-char or 5-char string of "+" and "-"
        // Return an indicator of short/long directional strength (or "(no bias)" if neither long nor short)
        private string GetSignal(string indicators)
        {
            var signal = "(no bias)";

            if (indicators.Length == 4)
            {
                if (indicators.EndsWith("++++"))
                    signal = "**LONG** ";
                else if (indicators.EndsWith("+++"))
                    signal = " *LONG*  ";
                else if (indicators.EndsWith("++"))
                    signal = "  long   ";
                else if (indicators.EndsWith("----"))
                    signal = "**SHORT**";
                else if (indicators.EndsWith("---"))
                    signal = " *SHORT* ";
                else if (indicators.EndsWith("--"))
                    signal = "  short  ";
            }
            else if (indicators.Length == 5)
            {
                if (indicators.EndsWith("+++++"))
                    signal = "**LONG** ";
                else if (indicators.EndsWith("++++"))
                    signal = " *LONG*  ";
                else if (indicators.EndsWith("+++"))
                    signal = "  LONG   ";
                else if (indicators.EndsWith("++"))
                    signal = "  long   ";
                else if (indicators.EndsWith("-----"))
                    signal = "**SHORT**";
                else if (indicators.EndsWith("----"))
                    signal = " *SHORT* ";
                else if (indicators.EndsWith("---"))
                    signal = "  SHORT  ";
                else if (indicators.EndsWith("--"))
                    signal = "  short  ";
            }
            else
            {
                throw new ArgumentException("GetSignal requires a 4-char or 4-char string of '+'/'-'. Provided '{0}'", indicators);
            }
            return signal;
        }

        // where exchange like "BINANCE"
        // where symbol like "ETHUSDT"
        private void BuySellIndicatorTask(string exchange, string symbol)
        {
            //Console.WriteLine("DateTime,Exchange,Symbol,C60,C30,C15,C5,C1,Indicators,Bid,Ask");
            Console.WriteLine("DateTime,Exchange,Symbol,C60,C30,C5,C1,Indicators,Bid,Ask");

            string prevIndicators = ".........";
            string prevSignal = "";

            while (true)
            {
                var now = DateTime.Now;
                var timeString = now.ToString("yyyy-MM-dd HH:mm:ss");

                var c60 = CandlesMACD(exchange, symbol, 60);
                var c30 = CandlesMACD(exchange, symbol, 30);
                //var c15 = CandlesMACD(exchange, symbol, 15);
                var c5 = CandlesMACD(exchange, symbol, 5);
                var c1 = CandlesMACD(exchange, symbol, 1);

                // TODO: Also use spikes in "bar volume" to indicate significant bars (i.e. beginning/end of longer trends)
                //var indicators = GetIndicators(c1.Last(), c5.Last(), c15.Last(), c30.Last(), c60.Last());
                var indicators = GetIndicators(c1.Last(), c5.Last(), c30.Last(), c60.Last());
                var signal = GetSignal(indicators);

                var ticker = m_api[exchange].GetTicker(symbol);

                //Console.WriteLine("{0} [{1,-8} {2}]  |{3,6:0.00} |{4,6:0.00} |{5,6:0.00} |{6,6:0.00} |{7,6:0.00} |   {8}  {9}  b:{10} a:{11}", timeString, exchange, symbol, c60.Last(), c30.Last(), c15.Last(), c5.Last(), c1.Last(), indicators, signal, ticker.Bid, ticker.Ask);
                //Console.WriteLine("{0},{1},{2},{3:0.00},{4:0.00},{5:0.00},{6:0.00},{7:0.00},{8},{9},{10}", timeString, exchange, symbol, c60.Last(), c30.Last(), c15.Last(), c5.Last(), c1.Last(), indicators, ticker.Bid, ticker.Ask);
				Console.WriteLine("{0},{1},{2},{3:0.000},{4:0.000},{5:0.000},{6:0.000},{7},{8},{9},{10}", timeString, exchange, symbol, c60.Last(), c30.Last(), c5.Last(), c1.Last(), indicators, ticker.Bid, ticker.Ask, signal);

                // If the indicators have changed, publish a Prowl message
                if (signal != prevSignal && signal != "(no bias)")
                {
                    var shortTimeString = now.ToString("MMM-dd HH:mm:ss");
                    if (m_notify) m_prowl.Send(signal, string.Format("{0} {1}:{2}", shortTimeString, ticker.Bid, ticker.Ask));
                }

                prevIndicators = indicators;
                prevSignal = signal;
                Thread.Sleep(20000);
            }
        }

        private List<decimal> CandlesMACD(string exchange, string symbol, int periodMinutes = 1, DateTime? startDate = null, DateTime? endDate = null, int? limit = null)
        {
            int periodSeconds = periodMinutes * 60;
            var candles = m_api[exchange].GetCandles(symbol, periodSeconds, startDate, endDate, limit);
            
			var macd = new iMACD(12, 26, 9);
            var histList = new List<decimal>();
            foreach (var c in candles)
            {
                var timeString = c.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                //Console.WriteLine("{0} {1} {2} o:{3} h:{4} l:{5} c:{6} vol:{7} period:{8} wavg:{9}", timeString, c.ExchangeName, c.Name, c.OpenPrice, c.HighPrice, c.LowPrice, c.ClosePrice, c.BaseVolume, c.PeriodSeconds, c.WeightedAverage);
                macd.ReceiveTick(c.ClosePrice);
                if (macd.IsPrimed)
                {
					/*decimal macdValue, signalValue, histValue;
                    macd.DecimalValue(out macdValue, out signalValue, out histValue);
                    //Console.WriteLine("{0:0.00}", histValue);   // {1:0.00} {2:0.00}", macdValue, signalValue, histValue);
                    histList.Add(histValue);*/
					histList.Add(macd.Value);
                }
            }
            return histList;
        }
    }

} // end of namespace
