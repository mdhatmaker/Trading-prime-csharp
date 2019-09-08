using System;
using Tools;
using static Tools.G;
using Newtonsoft.Json.Linq;
using CryptoAPIs.Exchange.Clients.HitBTC;

namespace CryptoAPIs.Exchange
{
    public partial class HitBTC
    {
        #region WebSocket ----------------------------------------------------------------------------------------------
        public void WebSocketMessageHandler(MessageArgs e)
        {
            //cout(e.Text);
            try
            {
                JObject msg = JObject.Parse(e.Text);

                if (msg["method"] != null)
                {
                    string msgType = msg["method"].ToString();
                    if (msgType == "snapshotOrderbook")
                    {
                        dout("HitBTC: snapshotOrderbook");
                        //cout(msg["method"]);
                        JObject param = msg["params"] as JObject;
                        JArray ask = param["ask"] as JArray;
                        JArray bid = param["bid"] as JArray;
                        string symbol = param["symbol"].Value<string>();
                        long sequence = param["sequence"].Value<long>();
                        //cout(ask.Count);
                        m_orderBook.Clear();
                        for (int i = 0; i < ask.Count; ++i)
                        {
                            JObject a = ask[i] as JObject;
                            if (a["price"] != null && a["size"] != null)
                            {
                                string price = a["price"].Value<string>();
                                string size = a["size"].Value<string>();
                                //cout("[{0}] {1} {2}", i, price, size);
                                m_orderBook.UpdateAsk(price, size);
                            }
                        }
                        //cout(bid.Count);
                        for (int i = 0; i < bid.Count; ++i)
                        {
                            JObject b = bid[i] as JObject;
                            if (b["price"] != null && b["size"] != null)
                            {
                                string price = b["price"].Value<string>();
                                string size = b["size"].Value<string>();
                                //cout("[{0}] {1} {2}", i, price, size);
                                m_orderBook.UpdateBid(price, size);
                            }
                        }
                        FireOrderBookUpdate();
                    }
                    else if (msgType == "updateOrderbook")
                    {
                        //cout("HitBTC: updateOrderbook");
                        //cout(msg["method"]);
                        JObject param = msg["params"] as JObject;
                        //JObject data = param["data"] as JObject;
                        JArray ask = param["ask"] as JArray;
                        JArray bid = param["bid"] as JArray;
                        string symbol = param["symbol"].Value<string>();
                        long sequence = param["sequence"].Value<long>();
                        m_orderBook.Clear();
                        for (int i = 0; i < ask.Count; ++i)
                        {
                            JObject a = ask[i] as JObject;
                            if (a["price"] != null && a["size"] != null)
                            {
                                string price = a["price"].Value<string>();
                                string size = a["size"].Value<string>();
                                m_orderBook.UpdateAsk(price, size);
                            }
                        }
                        for (int i = 0; i < bid.Count; ++i)
                        {
                            JObject b = bid[i] as JObject;
                            if (b["price"] != null && b["size"] != null)
                            {
                                string price = b["price"].Value<string>();
                                string size = b["size"].Value<string>();
                                m_orderBook.UpdateBid(price, size);
                            }
                        }
                        FireOrderBookUpdate();
                    }
                    else
                    {
                        ErrorMessage("Unknown HitBTC msg type: {0}", msgType);
                        dout(e.Text);
                    }
                }
                else
                {
                    ErrorMessage("Unknown HitBTC message type: ", Str(msg));
                    dout(e.Text);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("ERROR: Exception occurred parsing HitBTC message: ", ex.Message);
                dout(e.Text);
            }
            /*if (msg["table"] != null && msg["table"].ToString() == "orderBook10")
            {
                cout(msg["table"]);
                JArray array = msg["data"] as JArray;
                cout(array.Count);
                var a1 = array[0];
                var bids = a1["bids"] as JArray;
                cout("\n--------BIDS---------");
                int bidCount = bids.Count;
                for (int i = 0; i < bidCount; ++i)
                {
                    var b = bids[i] as JArray;
                    string price = b[0].Value<string>();
                    int quantity = b[1].Value<int>();
                    cout("[{0}]  {1} {2}", i, price, quantity);
                }
                var asks = a1["asks"] as JArray;
                int askCount = asks.Count;
                cout("--------ASKS---------");
                for (int i = 0; i < askCount; ++i)
                {
                    var a = asks[i] as JArray;
                    string price = a[0].Value<string>();
                    int quantity = a[1].Value<int>();
                    cout("[{0}]  {1} {2}", i, price, quantity);
                }
            }
            else
            {
                cout("Unknown message type: ", Str(msg));
            }*/
            return;
        }

        public void StartWebSocket(string[] args = null)
        {
            m_socket = new ZWebSocket("wss://api.hitbtc.com/api/2/ws", this.WebSocketMessageHandler);
        }

        public void SubscribeWebSocket(string[] args = null)
        {
            string json = @"{
                ""method"": ""subscribeOrderbook"",
                ""params"": {
                    ""symbol"": ""BTCUSD""
                },
                ""id"": 123
            }";
            m_socket.SendMessage(json);
        }
        #endregion -----------------------------------------------------------------------------------------------------
    }

} // end of namespace
