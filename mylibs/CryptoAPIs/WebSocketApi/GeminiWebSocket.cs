using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using CryptoWebSocketApis.GeminiModels;

namespace CryptoApis.WebsocketApi
{
    public class GeminiWebSocket
    {
        public static void TestGeminiTickers_marketdata()
        {
            const int BYTE_SIZE = 2048;

            // TODO: Be able to handle the LONG string that is the first update
            uint count = 0;     // simple update count (so we can skip first update for now)

            Console.WriteLine("LAUNCH: Gemini");

            var socket = new ClientWebSocket();
            Task task = socket.ConnectAsync(new Uri("wss://api.gemini.com/v1/marketdata/btcusd"), CancellationToken.None);
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
                        jsonString = jsonString.TrimEnd('\0');                  // trim trailing zeros in array

                        //Console.WriteLine("jsonString = {0}", jsonString);

                        int i1 = jsonString.IndexOf(@"""type"":");
                        if (i1 < 0)
                        {
                            Array.Clear(recBytes, 0, BYTE_SIZE);
                            continue;
                        }

                        int i2 = jsonString.IndexOf('"', i1 + 8);
                        if (i2 < 0)
                        {
                            Array.Clear(recBytes, 0, BYTE_SIZE);
                            continue;
                        }

                        string type = jsonString.Substring(i1 + 8, i2 - i1 - 8);

                        //JObject jo = JsonConvert.DeserializeObject<JObject>(jsonString);
                        //string type = jo["type"].Value<string>();
                        //Console.WriteLine("type: {0}", type);


                        if (type == "trade")                // "ticker": 
                        {
                            //JObject jo = JsonConvert.DeserializeObject<JObject>(jsonString);
                            //string pid = jo["product_id"].Value<string>();
                            //string price = jo["price"].Value<string>();
                            //string time = (jo["time"] == null ? "" : jo["time"].Value<string>());
                            //string size = (jo["last_size"] == null ? "" : jo["last_size"].Value<string>());
                            //Console.WriteLine("{0}  {1} {2}  {3}", pid, price, size, time);
                            Console.WriteLine(jsonString);
                        }
                        else if (type == "update")          // "update"
                        {
                            if (count > 0)
                            {
                                //Console.WriteLine(jsonString);
                                var update = JsonConvert.DeserializeObject<GeminiUpdate>(jsonString);
                                Console.WriteLine(update.ToString());
                            }
                            count++;
                        }
                        else if (type == "error")           // "error"
                        {
                        }
                        else if (type == "heartbeat")       // "heartbeat"
                        {
                        }

                        Array.Clear(recBytes, 0, BYTE_SIZE);
                        //recBytes = new byte[BYTE_SIZE];
                    }
                });
            readThread.Start();

            //string[] product_ids = { "BCH-BTC", "BCH-EUR", "BCH-USD", "BTC-EUR", "BTC-GBP", "BTC-USD", "ETH-BTC", "ETH-EUR", "ETH-USD", "LTC-BTC", "LTC-EUR", "LTC-USD" };
            //string json = GeminiRequestSubscribeTicker(product_ids);

            //byte[] bytes = Encoding.UTF8.GetBytes(json);
            //ArraySegment<byte> subscriptionMessageBuffer = new ArraySegment<byte>(bytes);
            //socket.SendAsync(subscriptionMessageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            Console.ReadLine();
        }

    } // end of class Gemini
} // end of namespace
