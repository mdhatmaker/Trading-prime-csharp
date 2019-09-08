using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Tools;
using static Tools.G;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using CryptoAPIs.Exchange.Clients.Binance;

namespace CryptoAPIs.Exchange
{
    public partial class Binance
    {
        
        public void WebSocketMessageHandler(MessageArgs e)
        {
            //cout(e.Text);
            if (e.Text.StartsWith("["))
            {
                JArray array = JArray.Parse(e.Text);
                int chanId = array[0].Value<int>();
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
                        dout("HEARTBEAT!");
                    }
                    else
                    {
                        ErrorMessage("BINANCE: Unknown message: {0}", array[1]);
                    }
                }
            }
            else
            {
                JObject msg = JObject.Parse(e.Text);
                //dynamic dict = JContainer.Parse(e.Text);
                //dynamic list = JArray.Parse(e.Text);
                if (msg["event"] != null)
                {
                    string msgType = msg["event"].Value<string>();
                    if (msgType == "info")
                    {
                        dout("BINANCE: info");
                    }
                    else if (msgType == "subscribed")
                    {
                        dout("BINANCE: subscribed");
                        string symbol = msg["symbol"].Value<string>();
                        int chanId = msg["chanId"].Value<int>();
                        m_channelIds[symbol] = chanId;
                        dout("symbol={0}    chanId={1}", symbol, chanId);
                    }
                }
                /*else
                {
                    //JArray arr = msg.Values();
                }*/
                else
                {
                    ErrorMessage("Unknown message type: ", Str(msg));
                }
            }

            return;
        }

        public void StartWebSocket(string[] args = null)
        {
            m_socket = new ZWebSocket("wss://api.binance.com/ws/2", this.WebSocketMessageHandler);
            //wss://stream.binance.com:9443
            //Raw streams are accessed at /ws/<streamName>
            //Combined streams are accessed at /stream?streams=<streamName1>/<streamName2>/<streamName3>
            //Combined stream events are wrapped as follows: {"stream":"<streamName>","data":<rawPayload>}
            //All symbols for streams are lowercase
            //A single connection to stream.binance.com is only valid for 24 hours; expect to be disconnected at the 24 hour mark
        }

        // TODO: handle different args
        public void SubscribeWebSocket(string[] args = null)
        {
            string pair = "BTCUSD";
            //string pair = "TRXETH";
            string json = $@"{{ ""event"":""subscribe"", ""channel"":""book"", ""pair"":""{pair}"", ""prec"":""P0"" }}";
            m_socket.SendMessage(json);
        }

      

    } // end of class Binance

} // end of namespace
