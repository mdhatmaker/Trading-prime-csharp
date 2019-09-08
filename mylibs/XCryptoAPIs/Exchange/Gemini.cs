using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    // https://docs.gemini.com/rest-api/

    public partial class Gemini : BaseExchange, IExchangeWebSocket
    {
        public override string BaseUrl { get { return "https://api.gemini.com/v1"; } }
        public override string Name { get { return "GEMINI"; } }
        public override CryptoExch Exch => CryptoExch.GEMINI;

        // SINGLETON
        public static Gemini Instance { get { return m_instance; } }
        private static readonly Gemini m_instance = new Gemini();
        private Gemini() { }

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    if (CurrencyPairs.Count > 0)
                    {
                        UpdateSymbolsFromCurrencyPairs();
                        return m_symbolList;
                    }

                    m_symbolList = GET<List<string>>("https://api.gemini.com/v1/symbols");
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            return GET<GeminiTicker>(string.Format("https://api.gemini.com/v1/pubticker/{0}", symbol));
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var symbols = SymbolList;
            foreach (var s in symbols)
            {
                result[s] = GetTicker(s);
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            // TODO: parameters "limit_bids" and "limit_asks"
            var book = GET<GeminiOrderBook>(string.Format("https://api.gemini.com/v1/book/{0}", symbol));
            return book as ZCryptoOrderBook;
        }


        public override void SubscribeOrderBookUpdates(ZCurrencyPair pair, bool subscribe)
        {
            StartWebSocket(new string[] { pair.GeminiSymbol });
            SubscribeWebSocket();
        }


        // wss://api.gemini.com/v1/marketdata/:symbol
        /*public void StartWebSocket(string symbol = "BTCUSD")
        {
            m_socket = new ClientWebSocket();
            string socketUrl = string.Format("wss://api.gemini.com/v1/marketdata/{0}", symbol);
            Task task = m_socket.ConnectAsync(new Uri(socketUrl), CancellationToken.None);
            task.Wait();
            Thread readThread = new Thread(
                delegate (object obj)
                {
                    byte[] recBytes = new byte[1024];
                    StringBuilder leftOver = new StringBuilder();
                    while (true)
                    {
                        ArraySegment<byte> t = new ArraySegment<byte>(recBytes);
                        Task<WebSocketReceiveResult> receiveAsync = m_socket.ReceiveAsync(t, CancellationToken.None);
                        receiveAsync.Wait();
                        //string jsonString = Encoding.UTF8.GetString(recBytes);
                        leftOver.Append(Encoding.UTF8.GetString(recBytes));
                        // jsonString = {"type":"update","eventId":2080342131,"timestamp":1509147792,
                        // "timestampms":1509147792867,"socket_sequence":86,
                        // "events":[{"type":"change","side":"ask","price":"5765.33","remaining":"49.16874984","delta":"5","reason":"place"}]}
                        string jsonString = leftOver.ToString();

                        leftOver.Clear();

                        //string[] split = jsonString.Split(',');
                        Regex regex = new Regex(@"},{");
                        string[] substrings = regex.Split(jsonString);

                        foreach (var s in substrings)
                        {
                            Console.WriteLine("jsonString = {0}", s);
                        }
                        string lastString = substrings[substrings.Length - 1];
                        if (!lastString.EndsWith("}"))
                            leftOver.Append(lastString);
                        //Console.Out.WriteLine("jsonString = {0}", jsonString);
                        //var response = Deserialize<GeminiWebSocketResponse>(jsonString);
                        
                        recBytes = new byte[1024];
                    }
                });
            readThread.Start();
        }

        public void SubscribeWebSocket(string productIds)
        {
            //string json = "{\"product_ids\":[\"btc-usd\"],\"type\":\"subscribe\"}";
            string json = ":ethbtc";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            ArraySegment<byte> subscriptionMessageBuffer = new ArraySegment<byte>(bytes);
            m_socket.SendAsync(subscriptionMessageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            Console.ReadLine();
        }*/
    } // end of class

    //======================================================================================================================================

    // jsonString = {"type":"update","eventId":2080342131,"timestamp":1509147792,
                        // "timestampms":1509147792867,"socket_sequence":86,
                        // "events":[{"type":"change","side":"ask","price":"5765.33","remaining":"49.16874984","delta":"5","reason":"place"}]}


    public class GeminiTickerVolume
    {
        public string BTC { get; set; }
        public string USD { get; set; }
        public long timestamp { get; set; }
    }

    public class GeminiTicker : ZTicker
    {
        public string ask { get; set; }
        public string bid { get; set; }
        public string last { get; set; }
        public GeminiTickerVolume volume { get; set; }

        public override decimal Bid { get { return decimal.Parse(bid); } }
        public override decimal Ask { get { return decimal.Parse(ask); } }
        public override decimal Last { get { return decimal.Parse(last); } }
        public override decimal High { get { return decimal.Parse("0"); } }
        public override decimal Low { get { return decimal.Parse("0"); } }
        //public override decimal Volume { get { return decimal.Parse(volume.BTC); } }
        public override decimal Volume { get { return decimal.Parse(volume.BTC ?? volume.USD); } }
        public override string Timestamp { get { return volume.timestamp.ToString(); } }
    }

    public class GeminiOrderBookEntry : ZCryptoOrderBookEntry
    {
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public string timestamp { get; set; }       // DO NOT USE!!! This field is included for compatibility reasons
    
        public override decimal Price { get { return price; } }
        public override decimal Amount { get { return amount; } }
        public override string Timestamp { get { return timestamp; } }
    }

    public class GeminiOrderBook : ZCryptoOrderBook
    {
        public List<GeminiOrderBookEntry> bids { get; set; }
        public List<GeminiOrderBookEntry> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get { return bids.Cast<ZCryptoOrderBookEntry>().ToList(); } }
        public override List<ZCryptoOrderBookEntry> Asks { get { return asks.Cast<ZCryptoOrderBookEntry>().ToList(); } }
    }

    public class GeminiWSEvent
    {
        public string type { get; set; }
        public string reason { get; set; }
        public string price { get; set; }
        public string delta { get; set; }
        public string remaining { get; set; }
        public string side { get; set; }
    }
} // end of namespace
