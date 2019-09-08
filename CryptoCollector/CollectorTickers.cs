using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using CryptoTools;
using CryptoTools.CryptoFile;
using ExchangeSharp;
using System.Text;
using System.Reflection;
using System.Threading;
using CryptoApis.SharedModels;
using CryptoApis.RestApi;
using CryptoApis;

namespace CryptoCollector
{
    public class CollectorTickers
    {
        private ExchangeSharpApi m_api;
        private Credentials m_creds;
        private List<IDisposable> m_sockets;
        private string m_outputPathname;
        private StreamWriter m_writer;
        private bool m_pauseFileOutput;

        private ConcurrentQueue<TickerOutput> m_outputQ = new ConcurrentQueue<TickerOutput>();

        // m_tickers[global_symbol][exchange] ==> ConcurrentStack<ExchangeTicker>
        // ex: m_tickers["BTC-USD"]["BINANCE"] ==> ConcurrentStack<ExchangeTicker>
        private ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentStack<ExchangeTicker>>> m_tickers;

        public CollectorTickers()
        {
            m_api = new ExchangeSharpApi();
            m_tickers = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentStack<ExchangeTicker>>>();
        }

        // where encryptedFile is the full pathname of the encrypted CSV file like "/Users/david/Documents/myapis.csv.enc"
        // where password is 8-char password like "12345678"
        public CollectorTickers(string encryptedFile, string password) : this()
        {
            m_creds = Credentials.LoadEncryptedCsv(encryptedFile, password);
            //m_apiMap = GetAllExchangeApis();
            //m_apiMap = GetPrimaryExchangeApis();
        }

        public void Start()   //string exchange, string symbol)
        {
            StartWebSockets();
            Console.WriteLine("Press ENTER to shutdown.");
            Console.ReadLine();
            foreach (var s in m_sockets)
            {
                s.Dispose();
            }
            return;
        }

        public void StartWebSockets(string outputPathname = null)
        {
            m_outputPathname = outputPathname;
            Task.Run(() => WriteOutputQueueTask(m_outputPathname));
            Task.Run(() => PartitionOutputFilesTask());

            m_sockets = new List<IDisposable>();
            m_sockets.Add(TestWebsocketsBinance(true));
            m_sockets.Add(TestWebsocketsBittrex(true));
            m_sockets.Add(TestWebsocketsPoloniex(true));
            m_sockets.Add(TestWebsocketsGdax(true));
            m_sockets.Add(TestWebsocketsBitfinex(true));
        }

        // Every hour, close the existing output file and create a new one (so the file size does not become too large).
        public void PartitionOutputFilesTask()
        {
            while (true)
            {
                var now = DateTime.Now;
                //if (now.Minute == 0 && now.Second == 0)     // if minutes and seconds indicate the beginning of a new hour
                if (now.Minute % 15 == 0 && now.Second == 0)     // if minutes and seconds indicate the beginning of a new hour
                {
                    m_pauseFileOutput = true;
                    Thread.Sleep(500);
                    m_writer.Close();
                    CreateOutputFile(m_outputPathname);
                    Thread.Sleep(500);
                    m_pauseFileOutput = false;
                }
                else
                {
                    Thread.Sleep(250);
                }
            }
        }

        // Create the output file (StreamWriter) that will be used.
        public void CreateOutputFile(string pathname)
        {
			m_writer = GFile.CreateOutputFile(pathname, TickerOutput.CsvHeaders);
            /*//var loc = Assembly.GetExecutingAssembly().Location;
            if (pathname != null)
            {
                m_writer = new StreamWriter(pathname, append: true);
            }
            else
            {
                string filename = string.Format("tickers_{0}.DF.csv", DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));
                pathname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
                m_writer = new StreamWriter(pathname, append: false);
                m_writer.WriteLine(TickerOutput.CsvHeaders);
            }*/
        }

        // If there is data waiting in the output queue, then write it to the output file.
        // where pathname is path+filename of output file (to append) or null to create a new file (with current date/time in name)
        public void WriteOutputQueueTask(string pathname)
        {
            CreateOutputFile(pathname);            

            while (true)
            {
                if (m_pauseFileOutput)
                {
                    Thread.Sleep(500);
                    continue;
                }

                TickerOutput tout;
                if (m_outputQ.TryDequeue(out tout))
                {
                    m_writer.Write(tout.ToCsv());
                    Thread.Sleep(1);
                }
                else
                {
                    Thread.Sleep(5);
                }
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

    } // end of class CollectorTickers

   
    /*
    //================================================================================================
    public static class ExchangeSharpExtensionMethods
    {
        // Extension Method: Display contents of an ExchangeTrade object
        public static void Print(this ExchangeTrade t)
        {
            Console.WriteLine("{0} {1} {2}", t.Timestamp.ToDisplay(), t.Price, t.Amount);
        }
    } // end of class ExchangeSharpExtensionMethods
    //================================================================================================
    */

} // end of namespace
