using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ScClient;
using SuperSocket.ClientEngine;
using Tools.Websockets.Models;

namespace Tools.Websockets
{
    internal class MyListener : IBasicListener
    {
        private WebsocketApiCredentials credentials;

        public MyListener(WebsocketApiCredentials credentials)
        {
            this.credentials = credentials;
        }

        public void OnConnected(Socket socket)
        {
            Console.WriteLine("connected got called");
            Console.WriteLine(credentials);

            socket.Emit("auth", credentials, (evemtname, error, data) => { });

            /*new Thread(() =>
            {
                Thread.Sleep(2000);
                socket.Emit("chat", "Hi sachin", (evemtname, error, data) => { });
                socket.GetChannelByName("yell").Publish("Hi there,How are you");
            }).Start();*/
        }

        public void OnDisconnected(Socket socket)
        {
            Console.WriteLine("disconnected got called");
        }

        public void OnConnectError(Socket socket, ErrorEventArgs e)
        {
            Console.WriteLine("on connect error got called");
        }

        public void OnAuthentication(Socket socket, bool status)
        {
            Console.WriteLine(status ? "Socket is authenticated" : "Socket is not authenticated");
        }

        public void OnSetAuthToken(string token, Socket socket)
        {
            socket.SetAuthToken(token);
            Console.WriteLine("on set auth token got called");
        }
    } // end of class MyListener
} // end of namespace
