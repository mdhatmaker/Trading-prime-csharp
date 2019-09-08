using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Tools.Websockets.Models;

namespace Tools.Websockets
{
    public class WebsocketTests
    {
        private static readonly ManualResetEvent resetEvent = new ManualResetEvent(false);
        private static ZWebsocket zsocket;

        private static int m_exch = 1;  // 1=COINIGY, 2=BITFINEX, 3=GDAX

        public void Subscribe_And_Listen()
        {
            // Initialize an instance of our socket
            /*socket = new BitfinexWebsocket(new ApiCredentials
            {
                ApiKey = "402bf810f4d10e99b9bdea8b8f76304e",
                ApiSecret = "b18bbbfbd2e8e0828b541603b5bf4613"
            });*/
            //socket = new BitfinexWebsocket(new WebsocketApiCredentials("lp1W7lgC8CesrgBUCLdL5XBgXHXh7hdv0Txu9TIjeDg", "cbZfggsZNLXoZaqY3sBRvj9ZXuOYb960mrVH7zibVUy"));    // BITFINEX 1
            //this.socket = new PureSocketClusterSocket("wss://api2.bitfinex.com:3000/ws");
            //this.socket = new PureSocketClusterSocket("wss://api.bitfinex.com/ws/2");
            //this.socket = new PureSocketClusterSocket("wss://api.bitfinex.com/ws");
            WebsocketApiCredentials creds;
            if (m_exch == 1)
            {
                creds = new WebsocketApiCredentials("402bf810f4d10e99b9bdea8b8f76304e", "b18bbbfbd2e8e0828b541603b5bf4613");
                zsocket = new ZWebsocket(creds, "wss://sc-02.coinigy.com/socketcluster/");   // COINIGY
            }
            else if (m_exch == 2)
            {
                creds = new WebsocketApiCredentials("4vRpraeVZQnet7ooxpAkul2fsInJFsxfasqDN9bHhLx", "MAyDsRlv3eQLZ7aqAYUTzExNGSSzOEFVdqrU2RWBVMW");
                zsocket = new ZWebsocket(creds, "wss://api.bitfinex.com/ws");    // BITFINEX 2
            }
            else if (m_exch == 3)
            {
                creds = new WebsocketApiCredentials("856ce8d659cf8f32a365037e62d06219", "29KJJ5aS2MYWbplvNr2OC1y9qpudruV4PWiC6BNdEuEnSrYWQNBJnoSFXaLmpJND7ysrxF7PM6p6fJDdxZRCDw==");
                zsocket = new ZWebsocket(creds, "wss://ws-feed.gdax.com");    // GDAX
            }
            // Subscribe to OnClientReady-event so we know when we can subscribe to trade channels
            zsocket.OnClientReady += Socket_OnClientReady;

            // Subscribe to the OnTradeMessage-event so we can receive trade messages
            zsocket.OnTradeMessage += Socket_OnTradeMessage;

            // Finally we can connect to our socket and wait for incoming messages
            zsocket.Connect();

            // Forces the methods not to exit
            resetEvent.WaitOne();
        }

        private void WriteLog(string message)
        {
            Debug.WriteLine($"{DateTime.UtcNow}: {message}");
        }

        private void Socket_OnTradeMessage(string exchange, string primaryCurrency,
                                           string secondaryCurrency, Tools.Websockets.Models.TradeItem trade)
        {
            WriteLog($"Received new trade for {exchange} market{ primaryCurrency}/{ secondaryCurrency}price { trade.Price}");
        }

        private void Socket_OnClientReady()
        {
            if (m_exch == 1)
            {
                // Subscribe to a new trade channel
                Console.WriteLine("Subscribing to new Coinigy channel...");
                zsocket.SubscribeToTradeChannel("BMEX", "XBT", "USD");
            }
            else if (m_exch == 3)
            {
                Console.WriteLine("Subscribing to new GDAX channel...");
                //zsocket.SubscribeToGdax(new string[] { });
            }
        }
    } // end of class WebsocketsTests
} // end of namespace
