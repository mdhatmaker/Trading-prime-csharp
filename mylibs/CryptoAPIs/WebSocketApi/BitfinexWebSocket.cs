using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CryptoTools.Net.Net;

namespace CryptoApis.WebsocketApi
{
    public class BitfinexWebSocket
    {
        static Dictionary<int, string> m_chanNames = new Dictionary<int, string>();
        static Dictionary<string, int> m_chanIds = new Dictionary<string, int>();
        public static void TestBitfinexTickers()
        {
            const int BYTE_SIZE = 1024;

            Console.WriteLine("LAUNCH: Bitfinex");

            var socket = new ClientWebSocket();
            Task task = socket.ConnectAsync(new Uri("wss://api.bitfinex.com/ws/1"), CancellationToken.None);
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

                        if (jsonString.StartsWith("{"))     // JObject
                        {
                            JObject jo = JsonConvert.DeserializeObject<JObject>(jsonString);
                            string type = jo["event"].Value<string>();
                            if (type == "subscribed")       // "subscribed"
                            {
                                string channel = jo["channel"].Value<string>();
                                int chanId = jo["chanId"].Value<int>();
                                string pair = jo["pair"].Value<string>();
                                string channelName = pair + "|" + channel;
                                m_chanNames[chanId] = channelName;
                                m_chanIds[channelName] = chanId;
                                Console.WriteLine("SUBSCRIBED: {0} {1} {2}", channel, chanId, pair);
                            }
                        }
                        else                                // JArray
                        {
                            JArray ja = JsonConvert.DeserializeObject<JArray>(jsonString);
                            if (ja.Last.Value<string>() == "hb")
                            {
                                // heartbeat...do nothing
                            }
                            else                            // assume ticker message, yes?
                            {
                                int chanId = ja[0].Value<int>();
                                string channelName = m_chanNames[chanId];
                                decimal bid = ja[1].Value<decimal>();
                                decimal bidSize = ja[2].Value<decimal>();
                                decimal ask = ja[3].Value<decimal>();
                                decimal askSize = ja[4].Value<decimal>();
                                decimal dailyChange = ja[5].Value<decimal>();
                                decimal dailyChangePercent = ja[6].Value<decimal>();
                                decimal lastPrice = ja[7].Value<decimal>();
                                decimal volume = ja[8].Value<decimal>();
                                decimal high = ja[9].Value<decimal>();
                                decimal low = ja[10].Value<decimal>();
                                Console.WriteLine("{0}: {1} {2}   {3} {4}", channelName, bid, bidSize, ask, askSize);
                            }
                        }

                        Array.Clear(recBytes, 0, BYTE_SIZE);
                    }
                });
            readThread.Start();

            Thread.Sleep(2000);
            SendJson(socket, BitfinexRequestSubscribeTicker("BTCUSD"));
            SendJson(socket, BitfinexRequestSubscribeTicker("ETHUSD"));
            SendJson(socket, BitfinexRequestSubscribeTicker("ETHBTC"));
            //Console.ReadLine();
        }

        //-------------------------------------------------------------------------------------------------------------

        // Where symbol like "BTCUSD", "ETHUSD", ...
        static string BitfinexRequestSubscribeTicker(string symbol)
        {
            //string req = @"{ event: ""subscribe"", channel: ""ticker"", symbol: ""?????"" }";
            //return req.Replace("?????", "t" + symbol.ToUpper());
            string req = @"{ ""event"": ""subscribe"", ""channel"": ""ticker"", ""pair"": ""?????"" }";
            return req.Replace("?????", symbol.ToUpper());
        }


    } // end of class Bitfinex
} // end of namespace
