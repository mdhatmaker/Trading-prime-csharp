using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using static Tools.G;

namespace Tools
{
    public class ZWebSocket
    {
        private ClientWebSocket m_socket;

        // connectionUrl like "wss://ws-feed.gdax.com"
        public ZWebSocket(string connectionUrl, OutputHandler onMessage)
        {
            dout("Starting Web Socket: {0}", connectionUrl);
            try
            {
                m_socket = new ClientWebSocket();
                Task task = m_socket.ConnectAsync(new Uri(connectionUrl), CancellationToken.None);
                task.Wait();
                Thread readThread = new Thread(
                    delegate (object obj)
                    {
                        byte[] recBytes = new byte[1024];
                        StringBuilder sb = new StringBuilder();
                        while (true)
                        {
                            ArraySegment<byte> t = new ArraySegment<byte>(recBytes);
                            Task<WebSocketReceiveResult> receiveAsync = m_socket.ReceiveAsync(t, CancellationToken.None);
                            receiveAsync.Wait();
                            if (receiveAsync.Result.EndOfMessage)
                            {
                                sb = sb.Append(Encoding.UTF8.GetString(recBytes, 0, receiveAsync.Result.Count));
                                string jsonString = sb.ToString();
                            //dout("\n{0}", jsonString);
                            onMessage?.Invoke(new MessageArgs(jsonString));
                            //cout("\njsonString = {0}\n", jsonString);
                            recBytes = new byte[1024];
                                sb.Clear();
                            }
                            else
                            {
                                sb = sb.Append(Encoding.UTF8.GetString(recBytes));
                                recBytes = new byte[1024];
                            //dout("StringBuilder: {0}\n\n", sb.ToString());
                            //cout("Not end of message");
                        }
                        }
                    });
                readThread.Start();
            }
            catch (Exception ex)
            {
                ErrorMessage("ZWebSocket::Ctor=> {0}", ex.Message);
            }
        }

        // jsonString like "{\"product_ids\":[\"btc-usd\"],\"type\":\"subscribe\"}"
        public void SendMessage(string json)
        {
            dout("Sending Message to web socket: {0}", json);
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                ArraySegment<byte> subscriptionMessageBuffer = new ArraySegment<byte>(bytes);
                m_socket.SendAsync(subscriptionMessageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                ErrorMessage("ZWebSocket::SendMessage=> {0}", ex.Message);
            }
        }

        // TODO: Can we improve our WebSocket handler code (possibly using some of the code below)?
        /*static async Task Client()
        {
            ClientWebSocket ws = new ClientWebSocket();
            //var uri = new Uri("ws://localhost:8000/ws/");
            var uri = new Uri("wss://ws.blockchain.info/inv");

            await ws.ConnectAsync(uri, CancellationToken.None);

            var buffer = new byte[1024];
            while (true)
            {
                var segment = new ArraySegment<byte>(buffer);
                var result = await ws.ReceiveAsync(segment, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "I don't do binary", CancellationToken.None);
                    return;
                }

                int count = result.Count;
                while (!result.EndOfMessage)
                {
                    if (count >= buffer.Length)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "That's too long", CancellationToken.None);
                        return;
                    }
                    segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                    result = await ws.ReceiveAsync(segment, CancellationToken.None);
                    count += result.Count;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, count);
                Console.WriteLine(">" + message);
            }
        }

        static void blah()
        {
            ClientWebSocket ws = new ClientWebSocket();
            var uri = new Uri("wss://ws.blockchain.info/inv");
            ws.ConnectAsync(uri, CancellationToken.None);
            await ws.ConnectAsync(uri, CancellationToken.None);
        }*/

    } // end of class
} // end of namespace
