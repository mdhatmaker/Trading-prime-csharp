using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
using CryptoAPIs.Exchange.Clients.Poloniex;
using System.IO;
using Newtonsoft.Json;

namespace CryptoAPIs.Exchange
{
    // https://poloniex.com/support/api/

    public partial class Poloniex
    {
        private static Dictionary<int, string> m_idToSymbol = new Dictionary<int, string>();
        private static Dictionary<string, int> m_symbolToId = new Dictionary<string, int>();

        private Dictionary<int, ZOrderBook> m_orderBooks = new Dictionary<int, ZOrderBook>();   // order books indexed by Poloniex symbolId
        private Dictionary<int, PoloniexWebsocketTicker> m_tickers = new Dictionary<int, PoloniexWebsocketTicker>();    // tickers indexed by Poloniex symbolId
        private Dictionary<string, decimal> m_currencyVolume24hr = new Dictionary<string, decimal>();
        private HashSet<string> m_orderBookMessageStart = new HashSet<string>();
        private string m_waitForSymbolId = null;
        private string m_subscribeToSymbol = null;

        // Static CTOR that reads the Poloniex symbol-to-symbolId mapping (int to string)
        static Poloniex()
        {
            string filename = "poloniex_symbol_ids.txt";
            try
            {
                string pathname = Folders.system_path(filename);
                using (var f = new StreamReader(pathname))
                {
                    string line;
                    while ((line = f.ReadLine()) != null)
                    {
                        var split = line.Split(':');
                        int id = int.Parse(split[0]);
                        string symbol = split[1].Trim();
                        m_idToSymbol[id] = symbol;
                        m_symbolToId[symbol] = id;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("Could not load Poloniex symbol mapping file for websockets from SYSTEM data folder: {0}", filename);
            }
        }

        #region --------------------------------------------------------------------------------------------------------
        public void WebSocketMessageHandler(MessageArgs e)
        {
            string text = e.Text.Replace("\u0000", string.Empty);       // remove any weird "\0" chars from string
            try
            {
                if (text == "[1010]")                 // heartbeat
                {
                    cout("{0} PELONIEX HEARTBEAT", DateTime.Now.ToString("HH:mm:ss"));
                    return;
                }
                else if (text == "[1001,1]")           // ACK: subscribe trollbox (WTF is trollbox?)
                {
                    base.m_socket.SendMessage(@"{""command"":""subscribe"",""channel"":1002}");
                    return;
                }
                else if (text == "[1002,1]")          // ACK: subscribe tickers
                {
                    base.m_socket.SendMessage(@"{""command"":""subscribe"",""channel"":1003}");
                    return;
                }
                else if (text == "[1003,1]")          // ACK: subscribe base coin 24h volume stats
                {
                    if (m_subscribeToSymbol != null)
                    {
                        string symbol = m_subscribeToSymbol;
                        m_subscribeToSymbol = null;
                        base.m_socket.SendMessage($@"{{""command"":""subscribe"",""channel"":""{symbol}""}}");
                        m_waitForSymbolId = string.Format("[{0},", m_symbolToId[symbol]);
                    }
                    return;
                }
                else if (text.StartsWith("[1002,"))   // ticker like [1002,null,[182,"0.00068310","0.00068994","0.00068333","-0.00824646","69.32703568","101264.75087174",0,"0.00070885","0.00066582"]]
                {
                    JArray ja = (JArray)JsonConvert.DeserializeObject(text);
                    ja = (JArray)ja[2];
                    var ticker = new PoloniexWebsocketTicker(ja);
                    m_tickers[ticker.SymbolId] = ticker;
                    //dout(ticker.ToString());
                    return;
                }
                else if (text.StartsWith("[1003,"))   // base coin 24h volume stats like [1003,null,["2018-03-02 13:52",15637,{"BTC":"8717.397","ETH":"1471.431","XMR":"317.235","USDT":"52612033.549"}]]
                {
                    // ["2018-03-02 13:52",15637,{"BTC":"8717.397","ETH":"1471.431","XMR":"317.235","USDT":"52612033.549"}]
                    JArray ja = (JArray)JsonConvert.DeserializeObject(text);
                    ja = (JArray)ja[2];
                    string datetime = (string)ja[0];
                    decimal volume = (decimal)ja[1];
                    var volumes = ja[2].Children<JProperty>();
                    foreach (var prop in volumes)
                    {
                        m_currencyVolume24hr[prop.Name] = (decimal)prop.Value;
                    }
                    dout("PoloniexWebsocket=> volume update ({0})", volumes.Count());
                    return;
                }
                else if (m_waitForSymbolId != null && text.StartsWith(m_waitForSymbolId))     // initial order book populate
                {
                    m_orderBookMessageStart.Add(m_waitForSymbolId);     // contains strings like "[191," or "[50," (poloniex symbolId)
                    m_waitForSymbolId = null;
                    //int id = int.Parse(str.Substring(1, str.Length - 2));
                    //string text = e.Text.Replace("\u0000", string.Empty);       // remove any weird "\0" chars from string
                    JArray ja = (JArray)JsonConvert.DeserializeObject(text);
                    int id = (int)ja[0];
                    //m_orderBooks[id] = new ZOrderBook();
                    long timestamp = (long)ja[1];
                    ja = (JArray)ja[2];
                    JArray jx = (JArray)ja[0];
                    string x = (string)jx[0];       // "i"
                    JObject y = (JObject)jx[1];
                    string pair = y["currencyPair"].ToString();
                    ja = (JArray)y["orderBook"];
                    var askArray = ja[0].Children<JProperty>();
                    var bidArray = ja[1].Children<JProperty>();
                    int i = 0;
                    foreach (var prop in bidArray)
                    {
                        //m_orderBooks[id].UpdateBid(prop.Name, (string)prop.Value);
                        m_orderBook.UpdateBid(prop.Name, (string)prop.Value);
                        //if (i < 10) cout("{0} BID: {1}    {2}", ++i, prop.Name, (string)prop.Value);
                    }
                    i = 0;
                    foreach (var prop in askArray)
                    {
                        //m_orderBooks[id].UpdateAsk(prop.Name, (string)prop.Value);
                        m_orderBook.UpdateAsk(prop.Name, (string)prop.Value);
                        //if (i < 10) cout("{0} ASK: {1}    {2}", ++i, prop.Name, (string)prop.Value);
                    }
                    FireOrderBookUpdate();
                    return;
                }

                if (m_orderBookMessageStart.Any(s => text.StartsWith(s)))     // order book update
                {
                    JArray ja = (JArray)JsonConvert.DeserializeObject(text);
                    int id = (int)ja[0];
                    long timestamp = (long)ja[1];
                    ja = (JArray)ja[2];
                    for (int i = 0; i < ja.Count; ++i)      // process each order book update (or trade) in the array list
                    {
                        JArray msg = (JArray)ja[i];
                        string ch = (string)msg[0];            // "o" (order book update) or "t" (trade)
                        if (ch == "o")                              // update the order book
                        {
                            int bidAsk = (int)msg[1];              // 0=ASK, 1=BID
                            string price = (string)msg[2];         // "1041.40000000"
                            string amount = (string)msg[3];        // "0.00269367"

                            MarketSide side = (bidAsk == 1 ? MarketSide.Bid : MarketSide.Ask);
                            if (side == MarketSide.Bid)
                            {
                                //m_orderBooks[id].UpdateBid(price, amount);
                                m_orderBook.UpdateBid(price, amount);
                            }
                            else
                            {
                                //m_orderBooks[id].UpdateAsk(price, amount);
                                m_orderBook.UpdateAsk(price, amount);
                            }
                        }
                        else if (ch == "t")                         // trade message
                        {
                            string tradeId = (string)msg[1];        // "19783474"
                            int buySell = (int)msg[2];              // 0=SELL, 1=BUY
                            string price = (string)msg[3];          // "10870.00000000"
                            string amount = (string)msg[4];         // "0.75781518"
                            long tradetime = (long)msg[5];          // 1520000155
                            cout("Poloniex.Websocket=> TRADE: {0} {1} {2} {3} {4} {5}", m_idToSymbol[id], tradeId, buySell, price, amount, tradetime);
                        }
                        else
                        {
                            ErrorMessage("Poloniex.Websocket=> unknown order book update type '{0}'", ch);
                        }
                    }
                    FireOrderBookUpdate();
                    //dout("Poloniex {0}=> {1} order book update(s)", m_idToSymbol[id], ja.Count);
                    return;
                }
            } catch (Exception exc)
            {
               
                ErrorMessage("Exception:\n{0}", exc.ToString());
            }
            ErrorMessage("{0} Poloniex.Websocket=> unknown message type:\n{1}", DateTime.Now.ToString("HH:mm:ss"), text);
            return;
        }

        public void StartWebSocket(string[] args = null)
        {
            base.m_socket = new ZWebSocket(this.WebsocketUrl, this.WebSocketMessageHandler);
        }

        // Where pair like "BTC_USD"
        // TODO: change interface so SubscribeWebSocket takes (at least) a currency pair argument
        public void SubscribeWebSocket(string[] args = null)
        {
            // These are the channels:
            // 1001 = trollbox (you will get nothing but a heartbeat)
            // 1002 = ticker
            // 1003 = base coin 24h volume stats
            // 1010 = heartbeat
            // 'MARKET_PAIR' = market order books

            m_subscribeToSymbol = "USDT_BTC";
            base.m_socket.SendMessage(@"{""command"":""subscribe"",""channel"":1001}");

            //string json = @"{ ""event"":""subscribe"", ""channel"":""book"", ""pair"":""" + pair + @""", ""prec"":""P0"" }";
            //[SUBSCRIBE, Request|id, Options|dict, Topic|uri]
            //base.m_socket.SendMessage(@"{""command"":""subscribe"",""channel"":1001}");
            //base.m_socket.SendMessage(@"{""command"":""subscribe"",""channel"":1002}");
            //base.m_socket.SendMessage(@"{""command"":""subscribe"",""channel"":1003}");
            //base.m_socket.SendMessage(@"{""command"":""subscribe"",""channel"":""BTC_USD""}");
        }
        #endregion -----------------------------------------------------------------------------------------------------

    } // end of class Poloniex

    public class PoloniexWebsocketTicker : ZTicker
    {
        int symbolId { get; set; }
        decimal last { get; set; }
        decimal ask { get; set; }
        decimal bid { get; set; }
        decimal pctChange { get; set; }
        decimal baseVolume { get; set; }
        decimal quoteVolume { get; set; }
        bool isFrozen { get; set; }
        decimal high24hr { get; set; }
        decimal low24hr { get; set; }
        long timestamp { get; set; }

        public override decimal Bid => bid;
        public override decimal Ask => ask;
        public override decimal Last => last;
        public override decimal High => high24hr;
        public override decimal Low => low24hr;
        public override decimal Volume => baseVolume;                       // TODO: should this be quoteVolume?
        public override string Timestamp => timestamp.ToDateTimeString();

        public int SymbolId => symbolId;

        public PoloniexWebsocketTicker(JArray ja)
        {
            symbolId = (int)ja[0];
            last = (decimal)ja[1];
            ask = (decimal)ja[2];
            bid = (decimal)ja[3];
            pctChange = (decimal)ja[4];
            baseVolume = (decimal)ja[5];
            quoteVolume = (decimal)ja[6];
            isFrozen = ((int)ja[7] == 1);
            high24hr = (decimal)ja[8];
            low24hr = (decimal)ja[9];
            timestamp = DateTime.Now.ToUnixTimestamp();
        }
    } // end of class PoloniexWebsocketTicker

} // end of namespace

