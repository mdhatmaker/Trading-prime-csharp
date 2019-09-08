using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Tools.Websockets;
using Tools.Websockets.Models;

namespace Coinigy.API.Tests
{
    [TestClass]
    public class WebsocketTests
    {
        private static readonly ManualResetEvent resetEvent = new ManualResetEvent(false);
        private static ZWebsocket socket;

        [TestMethod]
        public void Subscribe_And_Listen()
        {
            // Initialize an instance of our socket
            socket = new ZWebsocket(new WebsocketApiCredentials
            {
                apiKey = "402bf810f4d10e99b9bdea8b8f76304e",
                apiSecret = "b18bbbfbd2e8e0828b541603b5bf4613"
            }, "wss://);

            // Subscribe to OnClientReady-event so we know when we can subscribe to trade channels
            socket.OnClientReady += Socket_OnClientReady;

            // Subscribe to the OnTradeMessage-event so we can receive trade messages
            socket.OnTradeMessage += Socket_OnTradeMessage;

            // Finally we can connect to our socket and wait for incoming messages
            socket.Connect();

            // Forces the methods not to exit
            resetEvent.WaitOne();
        }

        private void WriteLog(string message)
        {
            Debug.WriteLine($"{DateTime.UtcNow}: {message}");
        }

        private void Socket_OnTradeMessage(string exchange, string primaryCurrency,
                                           string secondaryCurrency, BitfinexWebsockets.Models.TradeItem trade)
        {
            WriteLog($"Received new trade for {exchange} market{ primaryCurrency}/{ secondaryCurrency}price { trade.Price}");
        }

        private void Socket_OnClientReady()
        {
            // Subscribe to a new trade channel
            socket.SubscribeToTradeChannel("BMEX", "XBT", "USD");
        }
    }

} // end of namespace

