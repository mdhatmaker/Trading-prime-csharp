using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
using CryptoAPIs.Exchange.Clients.GDAX;

namespace CryptoAPIs.Exchange
{
	public partial class GDAX
	{
        
        public void WebSocketMessageHandler(MessageArgs e)
        {
            //cout(e.Text);
            JObject msg = JObject.Parse(e.Text);
            //dynamic dict = JContainer.Parse(e.Text);
            //dynamic list = JArray.Parse(e.Text);
            if (msg["type"] != null)
            {
                string type = msg["type"].ToString();
                if (type == "snapshot")
                {
                    dout("GDAX: snapshot");
                    JArray bids = msg["bids"] as JArray;
                    JArray asks = msg["asks"] as JArray;
                    //cout(bids.Count);
                    for (int i = 0; i < bids.Count(); ++i)
                    {
                        JArray b = bids[i] as JArray;
                        string price = b[0].Value<string>();
                        string amount = b[1].Value<string>();
                        m_orderBook.UpdateBid(price, amount);
                        //cout("{0} {1}", b[0], b[1]);
                    }
                    //cout("\n\n");
                    for (int i = 0; i < asks.Count(); ++i)
                    {
                        JArray a = asks[i] as JArray;
                        string price = a[0].Value<string>();
                        string amount = a[1].Value<string>();
                        m_orderBook.UpdateAsk(price, amount);
                        //cout("{0} {1}", a[0], a[1]);
                    }
                    //cout("\n\n");
                    FireOrderBookUpdate();
                }
                else if (type == "l2update")
                {
                    //cout("GDAX: l2update");
                    JArray changes = msg["changes"] as JArray;
                    for (int i = 0; i < changes.Count(); ++i)
                    {
                        JArray chg = changes[i] as JArray;
                        string buySell = chg[0].Value<string>();
                        string price = chg[1].Value<string>();
                        string amount = chg[2].Value<string>();
                        if (buySell == "buy")
                        {
                            m_orderBook.UpdateBid(price, amount);
                        }
                        else if (buySell == "sell")
                        {
                            m_orderBook.UpdateAsk(price, amount);
                        }
                        else
                        {
                            ErrorMessage("ERROR: GDAX: expecting 'buy' or 'sell'");
                        }
                    }
                    FireOrderBookUpdate();
                }
                else
                {
                    ErrorMessage("Unknown GDAX msg type: ", Str(msg));
                    dout(e.Text);
                }
                /*cout(msg["table"]);
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
                }*/

            }
            else
            {
                ErrorMessage("Unknown GDAX message type: ", Str(msg));
                dout(e.Text);
            }

        }

        public void StartWebSocket(string[] args = null)
        {
            m_socket = new ZWebSocket("wss://ws-feed.gdax.com", this.WebSocketMessageHandler);
        }

        // Request (unsubscribe ticker for product_ids)
        /*{
            "type": "unsubscribe",
            "product_ids": [
                "ETH-USD",
                "ETH-EUR"
            ],
            "channels": ["ticker"]
        }*/

        // Request (unsubscribe heartbeat)
        /*{
            "type": "unsubscribe",
            "channels": ["heartbeat"]
        }*/

        // Request
        // Subscribe to ETH-USD and ETH-EUR with the level2, heartbeat and ticker channels,
        // plus receive the ticker entries for ETH-BTC and ETH-GBP
        /*{
            "type": "subscribe",
            "product_ids": [
                "ETH-USD",
                "ETH-EUR"
            ],
            "channels": [
                "level2",
                "heartbeat",
                {
                    "name": "ticker",
                    "product_ids": [
                        "ETH-BTC",
                        "ETH-GBP"
                    ]
            },
            ]
        }*/

        // TODO: handle different args
        public void SubscribeWebSocket(string[] args = null)
        {
            //string json = "{\"product_ids\":[\"btc-usd\"],\"type\":\"subscribe\"}";
            string json = "{" + string.Join(",", args) + "}";
            m_socket.SendMessage(json);
        }

        /*public void StartWebSocket()
        {
            m_socket = new ClientWebSocket();
            Task task = m_socket.ConnectAsync(new Uri("wss://ws-feed.gdax.com"), CancellationToken.None);
            task.Wait();
            Thread readThread = new Thread(
                delegate (object obj)
                {
                    byte[] recBytes = new byte[1024];
                    while (true)
                    {
                        ArraySegment<byte> t = new ArraySegment<byte>(recBytes);
                        Task<WebSocketReceiveResult> receiveAsync = m_socket.ReceiveAsync(t, CancellationToken.None);
                        receiveAsync.Wait();
                        string jsonString = Encoding.UTF8.GetString(recBytes);
                        Console.Out.WriteLine("jsonString = {0}", jsonString);
                        recBytes = new byte[1024];
                    }
                });
            readThread.Start();
        }

        public void SubscribeWebSocket(string productIds)
        {
            string json = "{\"product_ids\":[\"btc-usd\"],\"type\":\"subscribe\"}";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            ArraySegment<byte> subscriptionMessageBuffer = new ArraySegment<byte>(bytes);
            m_socket.SendAsync(subscriptionMessageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            Console.ReadLine();
            //string json = string.Format("{\"product_ids\":[{0}],\"type\":\"subscribe\"}", productIds);
        }*/


	} // end of class GDAX

} // end of namespace
