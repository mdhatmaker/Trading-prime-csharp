using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoApis.WebsocketApi
{
    public class GdaxWebSocket
    {
        public static void TestGdaxTickers()
        {
            //const int BYTE_SIZE = 65536;
            const int BYTE_SIZE = 2048;

            Console.WriteLine("LAUNCH: GDAX");

            var socket = new ClientWebSocket();
            Task task = socket.ConnectAsync(new Uri("wss://ws-feed.gdax.com"), CancellationToken.None);
            task.Wait();

            Thread readThread = new Thread(
                delegate (object obj)
                {
                    byte[] recBytes = new byte[BYTE_SIZE];
                    while (true)
                    {
                        ArraySegment<byte> t = new ArraySegment<byte>(recBytes);
                        Task<WebSocketReceiveResult> receiveAsync = socket.ReceiveAsync(t, CancellationToken.None);
                        receiveAsync.Wait();
                        string jsonString = Encoding.UTF8.GetString(recBytes);
                        //Console.WriteLine("jsonString = {0}", jsonString);

                        JObject jo = JsonConvert.DeserializeObject<JObject>(jsonString);
                        string type = jo["type"].Value<string>();
                        //Console.WriteLine("type: {0}", type);


                        if (type == "ticker")      // "ticker": "product_id", "price", "time", "last_size"
                        {
                            string pid = jo["product_id"].Value<string>();
                            string price = jo["price"].Value<string>();
                            string time = (jo["time"] == null ? "" : jo["time"].Value<string>());
                            string size = (jo["last_size"] == null ? "" : jo["last_size"].Value<string>());
                            Console.WriteLine("{0}  {1} {2}  {3}", pid, price, size, time);
                        }
                        else if (type == "error")            // "error"
                        {
                        }
                        else if (type == "subscriptions")   // "subscriptions"
                        {
                        }

                        Array.Clear(recBytes, 0, BYTE_SIZE);
                        //recBytes = new byte[BYTE_SIZE];
                    }
                });
            readThread.Start();

            string[] product_ids = { "BCH-BTC", "BCH-EUR", "BCH-USD", "BTC-EUR", "BTC-GBP", "BTC-USD", "ETH-BTC", "ETH-EUR", "ETH-USD", "LTC-BTC", "LTC-EUR", "LTC-USD" };
            string json = GdaxRequestSubscribeTicker(product_ids);

            byte[] bytes = Encoding.UTF8.GetBytes(json);
            ArraySegment<byte> subscriptionMessageBuffer = new ArraySegment<byte>(bytes);
            socket.SendAsync(subscriptionMessageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            Console.ReadLine();
        }

        //-------------------------------------------------------------------------------------------------------------

        /*static string GdaxRequestSubscribe()
        {
            // Request
            // Subscribe to ETH-USD and ETH-EUR with the level2, heartbeat and ticker channels
            string req = @"{
                ""type"": ""subscribe"",
                ""product_ids"": [
                    ""ETH-USD"",
                    ""ETH-EUR""
                ],
                ""channels"": [
                    ""level2"",
                    ""heartbeat"",
                    ""ticker""
                ]
            }";
            return req;
        }*/

        // Where productIds like "BCH-BTC", "ETH-USD", "BTC-USD", ...
        static string GdaxRequestSubscribeTicker(string[] productIds)
        {
            // Request
            // Subscribe to the ticker channel for the specified product ids
            string req = @"{
                ""type"": ""subscribe"",
                ""product_ids"": [
                    ?????
                ],
                ""channels"": [
                    ""ticker""
                ]
            }";

            var sb = new StringBuilder();
            foreach (string pid in productIds)
            {
                sb.Append('"');
                sb.Append(pid);
                sb.Append('"');
                sb.Append(',');
            }
            sb.Remove(sb.Length - 1, 1);

            return req.Replace("?????", sb.ToString());
        }

    } // end of class Gdax
} // end of namespace
