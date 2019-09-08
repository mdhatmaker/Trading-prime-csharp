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
using CryptoApis.RestApi;
using CryptoTools.Cryptography;
using CryptoApis;

namespace CryptoTrader
{
    public class TraderCrossExchange
    {

        private ExchangeSharpApi m_api;
        //private Credentials m_creds;
        private List<IDisposable> m_sockets;

        private StreamWriter m_writer;

        //private ConcurrentQueue<TickerOutput> m_outputQ = new ConcurrentQueue<TickerOutput>();

        // m_tickers[global_symbol][exchange] ==> ConcurrentStack<ExchangeTicker>
        // ex: m_tickers["BTC-USD"]["BINANCE"] ==> ConcurrentStack<ExchangeTicker>
        private ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentStack<ExchangeTicker>>> m_tickers;

        public TraderCrossExchange()
        {
            m_api = new ExchangeSharpApi();
            m_api.LoadCredentials(Credentials.CredentialsFile, Credentials.CredentialsPassword);
            m_tickers = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentStack<ExchangeTicker>>>();
        }

        /*// where encryptedFile is the full pathname of the encrypted CSV file like "/Users/david/Documents/myapis.csv.enc"
        // where password is 8-char password like "12345678"
        public TraderCrossExchange(string encryptedFile, string password) : this()
        {
            m_creds = Credentials.LoadEncryptedCsv(encryptedFile, password);
            //m_apiMap = GetAllExchangeApis();
            //m_apiMap = GetPrimaryExchangeApis();
        }*/

        public void Start()   //string exchange, string symbol)
        {
            string filename = string.Format("xexchange_{0}.DF.csv", DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));
            string pathname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            m_writer = new StreamWriter(pathname);
            m_writer.WriteLine(this.CsvHeaders);

            StartWebSockets();
            Console.WriteLine("Press ENTER to shutdown.");
            Console.ReadLine();
            //m_writer.Flush();
            m_writer.Close();
            foreach (var s in m_sockets)
            {
                s.Dispose();
            }
            return;
        }

        private void StartWebSockets()
        {
            m_sockets = new List<IDisposable>();
            m_sockets.Add(StartWebsocketsBinance());
            m_sockets.Add(StartWebsocketsBittrex());
            m_sockets.Add(StartWebsocketsPoloniex());
            m_sockets.Add(StartWebsocketsGdax());
            m_sockets.Add(StartWebsocketsBitfinex());
        }

        /*public void WriteOutputQueue(string pathname = null)
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
        }*/

        public void HandleTickerUpdate(string exchange, IReadOnlyCollection<KeyValuePair<string, ExchangeTicker>> tickers)
        {
            foreach (var kv in tickers)
            {
                /*string symbol;
                if (api is ExchangeBitfinexAPI)
                    symbol = kv.Key.Substring(0, 3) + "-" + kv.Key.Substring(3);
                else
                    symbol = api.ExchangeSymbolToGlobalSymbol(kv.Key);
                string exchange = api.Name;*/

                string symbol = kv.Key;

                // 1st time for SYMBOL: Create a dictionary <exchange, stack<ExchangeTicker>>
                if (!m_tickers.ContainsKey(symbol))
                    m_tickers[symbol] = new ConcurrentDictionary<string, ConcurrentStack<ExchangeTicker>>();
                // 1st time for SYMBOL|EXCHANGE: Create a stack<ExchangeTicker>
                if (!m_tickers[symbol].ContainsKey(exchange))
                    m_tickers[symbol][exchange] = new ConcurrentStack<ExchangeTicker>();

                var ticker = kv.Value;
                m_tickers[symbol][exchange].Push(ticker);
            }
        }

        /*private void update(string sym)
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
        }*/

        private bool TickersExistForAllExchanges => (
                m_tickers.Keys.Contains("BTCUSD") &&
                m_tickers.Keys.Contains("BTC-USD") &&
                m_tickers.Keys.Contains("USDT_BTC") &&
                m_tickers.Keys.Contains("BTCUSDT") &&
                m_tickers.Keys.Contains("USDT-BTC"));
        
        public IDisposable StartWebsocketsBitfinex(bool display = false)
        {
            IExchangeAPI a = new ExchangeBitfinexAPI();
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                if (display) Console.WriteLine("BITFINEX {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                HandleTickerUpdate("BITFINEX", tickers);
                //m_outputQ.Enqueue(new TickerOutput("BITFINEX", tickers));
                if (tickers.Select(s => s.Key).Contains("BTCUSD"))
                {
                    //Console.WriteLine("BITFINEX");
                    if (TickersExistForAllExchanges) DisplaySpreads("BITFINEX");
                }
            });
            return socket;
        }

        public IDisposable StartWebsocketsGdax(bool display = false)
        {
            IExchangeAPI a = new ExchangeGdaxAPI();
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                if (display) Console.WriteLine("GDAX     {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                HandleTickerUpdate("GDAX", tickers);
                //m_outputQ.Enqueue(new TickerOutput("GDAX", tickers));
                if (tickers.Select(s => s.Key).Contains("BTC-USD"))
                {
                    //Console.WriteLine("GDAX");
                    if (TickersExistForAllExchanges) DisplaySpreads("GDAX");
                }
            });
            return socket;
        }

        public IDisposable StartWebsocketsPoloniex(bool display = false)
        {
            IExchangeAPI a = new ExchangePoloniexAPI();
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                if (display) Console.WriteLine("POLONIEX {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                HandleTickerUpdate("POLONIEX", tickers);
                //m_outputQ.Enqueue(new TickerOutput("POLONIEX", tickers));
                var sym = "USDT_BTC";
                if (tickers.Select(s => s.Key).Contains(sym))
                {
                    //Console.WriteLine("POLONIEX");
                    if (TickersExistForAllExchanges) DisplaySpreads("POLONIEX");
                }
            });
            return socket;
        }

        public IDisposable StartWebsocketsBinance(bool display = false)
        {
            IExchangeAPI a = new ExchangeBinanceAPI();
            var socket = a.GetTickersWebSocket((tickers) =>
            {
                if (display) Console.WriteLine("BINANCE  {0,4} tickers, first: {1}", tickers.Count, tickers.First());
                HandleTickerUpdate("BINANCE", tickers);
                //m_outputQ.Enqueue(new TickerOutput("BINANCE", tickers));
                if (tickers.Select(s => s.Key).Contains("BTCUSDT"))
                {
                    //Console.WriteLine("BINANCE");
                    if (TickersExistForAllExchanges) DisplaySpreads("BINANCE");
                }
            });
            return socket;
        }

        public IDisposable StartWebsocketsBittrex(bool display = false)
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
                HandleTickerUpdate("BITTREX", tickers);
                //m_outputQ.Enqueue(new TickerOutput("BITTREX", tickers));
                if (tickers.Select(s => s.Key).Contains("USDT-BTC"))
                {
                    //Console.WriteLine("BITTREX");
                    if (TickersExistForAllExchanges) DisplaySpreads("BITTREX");
                }
            });
            return socket;
        }

        private void DisplaySpreads(string exchange)
        {
            var s1 = m_tickers["BTCUSD"]["BITFINEX"];
            var s2 = m_tickers["BTC-USD"]["GDAX"];
            var s3 = m_tickers["USDT_BTC"]["POLONIEX"];
            var s4 = m_tickers["BTCUSDT"]["BINANCE"];
            var s5 = m_tickers["USDT-BTC"]["BITTREX"];

            ExchangeTicker t1, t2, t3, t4, t5;
            if (s1.TryPeek(out t1) && s2.TryPeek(out t2) && s3.TryPeek(out t3) && s4.TryPeek(out t4) && s5.TryPeek(out t5))
            {
                var a1 = t1.Ask - t2.Bid;
                var a2 = t1.Ask - t3.Bid;
                var a3 = t1.Ask - t4.Bid;
                var a4 = t1.Ask - t5.Bid;
                var a5 = t2.Ask - t3.Bid;
                var a6 = t2.Ask - t4.Bid;
                var a7 = t2.Ask - t5.Bid;
                var a8 = t3.Ask - t4.Bid;
                var a9 = t3.Ask - t5.Bid;
                var a10 = t4.Ask - t5.Bid;

                var b1 = t1.Bid - t2.Ask;
                var b2 = t1.Bid - t3.Ask;
                var b3 = t1.Bid - t4.Ask;
                var b4 = t1.Bid - t5.Ask;
                var b5 = t2.Bid - t3.Ask;
                var b6 = t2.Bid - t4.Ask;
                var b7 = t2.Bid - t5.Ask;
                var b8 = t3.Bid - t4.Ask;
                var b9 = t3.Bid - t5.Ask;
                var b10 = t4.Bid - t5.Ask;

                string timeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if ((a10 < -38 || b10 > 32) ||
                    (a9 < -26 || b9 > 45) ||
                    (a1 < -90 || b1 > -42) ||
                    (a2 < -98 || b2 > -45) ||
                    (a3 < -98 || b3 > -33) ||
                    (a4 < -104 || b4 > -31) ||
                    (a5 < -46 || b5 > 15) ||
                    (a6 < -18 || b6 > 21) ||
                    (a7 < -36 || b7 > 33) ||
                    (a8 < -13 || b8 > 25) ||
                    (a9 < -26 || b9 > 45))
                {
                    Console.WriteLine(">>> {0} {1,-8} a1:{2:0.00} a2:{3:0.00} a3:{4:0.00} a4:{5:0.00} a5:{6:0.00} a6:{7:0.00} a7:{8:0.00} a8:{9:0.00} a9:{10:0.00} a10:{11:0.00}", timeString, exchange, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                    Console.WriteLine("    {0} {1,-8} b1:{2:0.00} b2:{3:0.00} b3:{4:0.00} b4:{5:0.00} b5:{6:0.00} b6:{7:0.00} b7:{8:0.00} b8:{9:0.00} b9:{10:0.00} b10:{11:0.00}", timeString, exchange, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10);
                }

                // Write cross-exchange spread bid/ask data to file
                m_writer.WriteLineAsync(string.Format("{0},{1},{2:0.00},{3:0.00},{4:0.00},{5:0.00},{6:0.00},{7:0.00},{8:0.00},{9:0.00},{10:0.00},{11:0.00},{12:0.00},{13:0.00},{14:0.00},{15:0.00},{16:0.00},{17:0.00},{18:0.00},{19:0.00},{20:0.00},{21:0.00}", timeString, exchange, a1, b1, a2, b2, a3, b3, a4, b4, a5, b5, a6, b6, a7, b7, a8, b8, a9, b9, a10, b10));
            }
        }

        public string CsvHeaders => "DateTime,Exchange,a1,b1,a2,b2,a3,b3,a4,b4,a5,b5,a6,b6,a7,b7,a8,b8,a9,b9,a10,b10";

    } // end of class TraderCrossExchange



} // end of namespace
