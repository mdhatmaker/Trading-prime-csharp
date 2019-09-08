using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
using CryptoAPIs.Exchange.Clients.BitFinex;

namespace CryptoAPIs.Exchange
{
    // https://docs.bitfinex.com/docs/ws-general

    public partial class Bitfinex
    {
        private Dictionary<int, string> m_tickerChannelIds = new Dictionary<int, string>();
        private Dictionary<int, string> m_tradeChannelIds = new Dictionary<int, string>();

        public void WebSocketMessageHandler(MessageArgs e)
        {
            //cout(e.Text);
            string text = e.Text.Replace("\u0000", string.Empty);
            if (text.StartsWith("["))
            {
                JArray array = JArray.Parse(text);
                int chanId = array[0].Value<int>();

                // Check the ChannelId of the message
                if (m_tickerChannelIds.ContainsKey(chanId))     // is this channel id in our TICKER list?
                {
                    string symbol = m_tickerChannelIds[chanId];
                    JArray ja = array[1] as JArray;
                    if (ja == null)
                    {
                        string msgtype = array[1].Value<string>();
                        if (msgtype == "hb")
                        {
                            //dout("BITFINEX:HEARTBEAT");
                        }
                        else
                        {
                            ErrorMessage("Bitfinex::WebSocketMessageHandler=> Unknown ticker message type: {0}", msgtype);
                        }
                        return;
                    }
                    var tim = new TickerMessage(ja);
                    // TODO: Fire event with this ticker update
                    cout("BITFINEX Ticker: {0} {1}", symbol, tim.ToString());
                    return;
                }
                else if (m_tradeChannelIds.ContainsKey(chanId)) // is this channel id in our TRADE list?
                {
                    string symbol = m_tradeChannelIds[chanId];
                    JArray ja = array[1] as JArray;
                    if (ja == null)
                    {
                        string msgtype = array[1].Value<string>();
                        if (msgtype == "hb")
                        {
                            //dout("BITFINEX:HEARTBEAT");
                        }
                        else if (msgtype == "te")
                        {
                            ja = array[2] as JArray;
                            var trm = new TradeMessage(ja);
                            // TODO: Fire event with this trade execution
                            cout("BITFINEX Trade Execution: {0} {1}", symbol, trm.ToString());
                        }
                        else if (msgtype == "tu")
                        {
                            ja = array[2] as JArray;
                            var trm = new TradeMessage(ja);
                            // TODO: Fire event with this trade execution update
                            cout("BITFINEX Trade Execution Update: {0} {1}", symbol, trm.ToString());
                        }
                        else
                        {
                            ErrorMessage("Bitfinex::WebSocketMessageHandler=> Unknown trade message type: {0}", msgtype);
                        }
                        return;
                    }
                    foreach (JArray t in ja)
                    {
                        var trm = new TradeMessage(t);
                        // TODO: Fire event with this trade update
                        cout("BITFINEX Trade: {0} {1}", symbol, trm.ToString());
                    }
                    return;
                }

                if (array[1] is JArray)
                {
                    JArray book = array[1].Value<JArray>();
                    if (book[0] is JArray)
                    {
                        //m_orderBook.Clear();
                        for (int i = 0; i < book.Count; ++i)
                        {
                            var bookEntry = book[i].Value<JArray>();
                            float price = bookEntry[0].Value<float>();
                            int ordersAtLevel = bookEntry[1].Value<int>();
                            float amount = bookEntry[2].Value<float>();
                            //cout("{0} {1} {2}", price, ordersAtLevel, amount);
                            if (ordersAtLevel == 0)
                            {
                                m_orderBook.UpdateBid(price.ToString(), 0);
                                m_orderBook.UpdateAsk(price.ToString(), 0);
                            }
                            else
                            {
                                if (amount >= 0)     // bid
                                {
                                    m_orderBook.UpdateBid(price.ToString(), amount.ToString());
                                }
                                if (amount <= 0)    // ask
                                {
                                    m_orderBook.UpdateAsk(price.ToString(), (-amount).ToString());
                                }
                            }
                        }
                        FireOrderBookUpdate();
                    }
                    else        // only contains a SINGLE book entry (a single list containing 3 values)
                    {
                        float price = book[0].Value<float>();
                        int ordersAtLevel = book[1].Value<int>();
                        float amount = book[2].Value<float>();
                        //cout("{0} {1} {2}", price, ordersAtLevel, amount);
                        if (ordersAtLevel == 0)
                        {
                            m_orderBook.UpdateBid(price.ToString(), 0);
                            m_orderBook.UpdateAsk(price.ToString(), 0);
                        }
                        else
                        {
                            if (amount >= 0)     // bid
                            {
                                m_orderBook.UpdateBid(price.ToString(), amount.ToString());
                            }
                            if (amount <= 0)    // ask
                            {
                                m_orderBook.UpdateAsk(price.ToString(), (-amount).ToString());
                            }
                        }
                        FireOrderBookUpdate();
                    }
                }
                else
                {
                    if (array[1].Type == JTokenType.String && array[1].Value<string>() == "hb")
                    {
                        dout("{0} BITFINEX:HEARTBEAT", DateTime.Now.ToString("HH:mm:ss"));
                    }
                    else
                    {
                        ErrorMessage("BITFINEX: Unknown message: {0}", array[1]);
                    }
                }
            }
            else
            {
                JObject msg = JObject.Parse(text);
                if (msg["event"] != null)
                {
                    string msgType = msg["event"].Value<string>();
                    if (msgType == "info")
                    {
                        var version = msg["version"];
                        if (version != null)
                        {
                            int iversion = version.Value<int>();
                            JObject platform = msg["platform"] as JObject;
                            int status = platform["status"].Value<int>();
                            dout("BITFINEX: info  version={0} status={1}", iversion, status);
                        }
                        var code = msg["code"];
                        if (code != null)
                        {
                            int icode = code.Value<int>();
                            string message = msg["msg"].Value<string>();
                            dout("BITFINEX: info  code={0} message:{1}", icode, message);
                        }
                    }
                    else if (msgType == "subscribed")
                    {
                        //dout("BITFINEX: subscribed");
                        string channel = msg["channel"].Value<string>();
                        int chanId = msg["chanId"].Value<int>();
                        string symbol = msg["symbol"].Value<string>();
                        string pair = msg["pair"].Value<string>();
                        if (channel == "ticker")
                            m_tickerChannelIds[chanId] = pair;
                        else if (channel == "trades")
                            m_tradeChannelIds[chanId] = pair;
                        else
                            ErrorMessage("Bitfinex::WebSocketMessageHandler=> Unknown channel in 'subscribed' message: {0}", channel);
                        dout("BITFINEX:subscribed  channel={0} chanId={1} symbol={2} pair={3}", channel, chanId, symbol, pair);
                    }
                }
                else
                {
                    ErrorMessage("Unknown message type: ", Str(msg));
                }
            }

            return;
        }

        public void StartWebSocket(string[] args = null)
        {
            m_socket = new ZWebSocket("wss://api.bitfinex.com/ws/2", this.WebSocketMessageHandler);
        }

        // TODO: handle different args
        public void SubscribeWebSocket(string[] args = null)
        {
            string pair = "BTCUSD";
            string json = @"{ ""event"":""subscribe"", ""channel"":""book"", ""pair"":""" + pair + @""", ""prec"":""P0"" }";
            m_socket.SendMessage(json);
        }

        public void SubscribeChannel(string channel, string pairSymbol)
        {
            if (m_socket == null) StartWebSocket();
            string json = $@"{{ ""event"":""subscribe"", ""channel"":""{channel}"", ""pair"":""{pairSymbol}"" }}";
            m_socket.SendMessage(json);
        }

        public void UnsubscribeChannel(string channel, string pairSymbol)
        {
            if (m_socket == null) return;
            string json = $@"{{ ""event"":""unsubscribe"", ""channel"":""{channel}"", ""pair"":""{pairSymbol}"" }}";
            m_socket.SendMessage(json);
        }

        //-----------------------------------------------------------------------------------------
        class TickerMessage
        {
            public decimal bid { get; private set; }
            public decimal bidSize { get; private set; }
            public decimal ask { get; private set; }
            public decimal askSize { get; private set; }
            public decimal dailyChg { get; private set; }
            public decimal dailyChgPct { get; private set; }
            public decimal lastPrice { get; private set; }
            public decimal volume { get; private set; }
            public decimal high { get; private set; }
            public decimal low { get; private set; }

            public TickerMessage(JArray ja)
            {
                bid = (decimal)ja[0];
                bidSize = (decimal)ja[1];
                ask = (decimal)ja[2];
                askSize = (decimal)ja[3];
                dailyChg = (decimal)ja[4];
                dailyChgPct = (decimal)ja[5];
                lastPrice = (decimal)ja[6];
                volume = (decimal)ja[7];
                high = (decimal)ja[8];
                low = (decimal)ja[9];
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", bid, bidSize, ask, askSize, dailyChg, dailyChgPct, lastPrice, volume, high, low);
            }
        } // end of class Bitfinex.TickerMessage

        //-----------------------------------------------------------------------------------------

        class TradeMessage
        {
            public int tradeid { get; private set; }
            public long timestamp { get; private set; }
            public decimal amount { get; private set; }
            public decimal price { get; private set; }
            public OrderSide side { get; private set; }

            public TradeMessage(JArray ja)
            {
                tradeid = (int)ja[0];
                timestamp = (long)ja[1];
                decimal amt = (decimal)ja[2];   // amount can be positive (BUY) or negative (SELL)
                side = (amt >= 0 ? OrderSide.Buy : OrderSide.Sell);
                amount = Math.Abs(amt);
                price = (decimal)ja[3];
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4}", tradeid, timestamp, amount, price, side);
            }
        } // end of class Bitfinex.TickerMessage

    } // end of class Bitfinex

} // end of namespace
