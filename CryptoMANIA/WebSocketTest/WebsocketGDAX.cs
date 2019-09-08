using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PureSocketCluster;

namespace WebSocketX
{
    public class WebsocketGDAX
    {
        private static readonly string WssUrl = "wss://ws-feed.gdax.com";

        // Note: Coinigy's channel names need to be in the following format:
        // METHOD-EXCHANGECODE--PRIMARYCURRENCY--SECONDARYCURRENCY
        public enum MessageType
        {
            TradeData,
            OrderData,
            NewsData,
            BlockData,
            FavoriteData,
            NewMarket,
            NotificationData,
            Unknown
        }

        // Now the user can subscribe to the OnClientReady-event and start subscribing to
        // trade channels once it gets invoked.
        public event ClientIsReady OnClientReady;   // let user know when authentication has completed
        public delegate void ClientIsReady();

        // Once we have deserialized our trade market data, we let the user know with an
        // event the user can subscribe to:
        public event TradeMessage OnTradeMessage;
        public delegate void TradeMessage(string exchange, string primaryCurrency,
                                          string secondaryCurreny, TradeItem trade);
        
        // Channel names are in different formats depending on the exchange.
        // Coinigy's channel names, for instance, need to be in the following format: METHOD-EXCHANGECODE--PRIMARYCURRENCY--SECONDARYCURRENCY

        private readonly PureSocketClusterSocket socket;
        private readonly ApiCredentials credentials;

        public WebsocketGDAX(ApiCredentials credentials)
        {
            Console.WriteLine("WebsocketGDAX::ctor()  {0} {1}", credentials.ApiKey, credentials.ApiSecret);

            this.credentials = credentials;
            this.socket = new PureSocketClusterSocket(WssUrl);

            socket.OnOpened += Socket_OnOpened;
            socket.OnMessage += Socket_OnMessage;
            socket.OnData += Socket_OnData;

            socket.OnClosed += Socket_OnClosed;
            socket.OnError += Socket_OnError;
            socket.OnFatality += Socket_OnFatality;
            socket.OnSendFailed += Socket_OnSendFailed;
            socket.OnStateChanged += Socket_OnStateChanged;
            

        }

        private void Socket_OnStateChanged(System.Net.WebSockets.WebSocketState newState, System.Net.WebSockets.WebSocketState prevState)
        {
            throw new NotImplementedException();
        }

        private void Socket_OnSendFailed(string data, Exception ex)
        {
            throw new NotImplementedException();
        }

        private void Socket_OnFatality(string reason)
        {
            throw new NotImplementedException();
        }

        private void Socket_OnError(Exception ex)
        {
            throw new NotImplementedException();
        }

        private void Socket_OnClosed(System.Net.WebSockets.WebSocketCloseStatus reason)
        {
            throw new NotImplementedException();
        }

        // Allow the user to connect to the socket with the following code:
        public bool Connect()
        {
            Console.WriteLine("WebsocketGDAX::Connect()");

            return this.socket.Connect();
        }

        void Socket_OnOpened()
        {
            Console.WriteLine("WebsocketGDAX::Socket_OnOpened()");

            // We first need to authenticate with the socket. We will start receiving information
            // from the socket after the authentication succeeds. To authenticate with the
            // webservice, we need to send the "auth" command along with our credentials.
            // The "auth" command can only be called once the socket has opened.
            /*socket.Emit("auth", this.credentials, ack: (string name, object error, object data) =>
            {
                // We can now start listening to trade information
                Console.WriteLine("WebsocketCoinigy->emitted auth credentials");
                OnClientReady?.Invoke();
            });*/
            Console.WriteLine("WebsocketGDAX->emitting auth credentials...");
            socket.Emit("auth", this.credentials);
        }

        void Socket_OnData(byte[] data)
        {
            Console.WriteLine("WebsocketGDAX::Socket_OnData()");

        }

        // When we receive a message, we first need to check if it is a "publish" message.
        // Once the message has been verified, we can parse it into a JObject and send it
        // to our GetRequestType-function to determine the type of request.
        void Socket_OnMessage(string message)
        {
            Console.WriteLine("WebsocketGDAX::Socket_OnMessage()  {0}", message);

            string PUBLISH_REGEX = @"^{""event""*.:*.""#publish""";
            string AUTHTOKEN_REGEX = @"^{""event""*.:*.""#setAuthToken""";

            // Determine if message is a publish message using regex
            if (Regex.Match(message, PUBLISH_REGEX).Success)
            {
                // If so, parse the string
                var jObj = JObject.Parse(message);

                // Retrieve the channel's name
                string channelName = jObj["data"]["channel"].ToString();

                // Determine request type
                string requestType = GetRequestType(jObj);
                if (string.IsNullOrEmpty(requestType))
                    return;

                InvokeMessageReceived(channelName, requestType, message);
            }
            else if (Regex.Match(message, AUTHTOKEN_REGEX).Success)
            {
                // If so, parse the string
                var jObj = JObject.Parse(message);
                string token = jObj["data"]["token"].ToString();
                Console.WriteLine("token: {0}", token);
                socket.SetAuthToken(token);
                OnClientReady?.Invoke();
            }
        }

        public void SubscribeToPrivateChannel()
        {
            // subscribe to "4A92998C-F6CF-4E33-C17D-4574728471AD"
        }

        public bool SubscribeToTradeChannel(string exchange, string primaryCurrency, string secondaryCurrency) => this.socket.Subscribe($"TRADE-{exchange}--{primaryCurrency}--{secondaryCurrency}");

        // Every message we receive from the socket will be invoked here.
        // We still need to determine what type of message the server has sent us since it
        // can return different types of messsages.
        // To determine the type, we use the following function:
        private static string GetRequestType(JObject jObj)
        {
            Console.WriteLine("WebsocketGDAX::GetRequestType()  {0}", jObj);

            string requestType = string.Empty;
            var channelName = jObj["data"]["channel"].ToString();

            Guid guid;
            if (!Guid.TryParse(channelName, out guid))
                return channelName.Substring(0, channelName.IndexOf('-'));

            Guid channelGuid;
            requestType = channelName;
            if (Guid.TryParse(channelName, out channelGuid))
                if (channelGuid.ToString().ToLower() == channelName.ToLower())
                    requestType = jObj["data"]["data"]["MessageType"].ToString();

            return requestType;
        }

        private void InvokeMessageReceived(string channelName, string requestType, string message)
        {
            Console.WriteLine("WebsocketGDAX::InvokeMessageReceived()  {0} {1} {2}", channelName, requestType, message);

            // Determine the message type using the function we previously created
            MessageType messageType = GetMessageType(requestType);
            switch (messageType)
            {
                case MessageType.TradeData:
                    // Parse the channel name
                    var tradeMarketInfo = MarketInfo.ParseMarketInfo(channelName);

                    // Deserialize the string to a TradeResponse-entity
                    var trade = Helper.ToEntity<TradeResponse>(message);

                    // Invoke an event to let the subscribers know we have received trade information
                    OnTradeMessage?.Invoke(tradeMarketInfo.Exchange, tradeMarketInfo.PrimaryCurrency,
                    tradeMarketInfo.SecondaryCurrency, trade.TradeData.Trade);
                    break;

                    // Other cases for each MessageType...
            }
        }

        // Function to determine what the COINIGY message type is based on the request type:
        private static MessageType GetMessageType(string requestType)
        {
            Console.WriteLine("WebsocketGDAX::GetMessageType()  {0}", requestType);

            switch (requestType.ToUpper())
            {
                case "ORDER":
                    return MessageType.OrderData;
                case "TRADE":
                    return MessageType.TradeData;
                case "BLOCK":
                    return MessageType.BlockData;
                case "FAVORITE":
                    return MessageType.FavoriteData;
                case "NOTIFICATION":
                    return MessageType.NotificationData;
                case "NEWS":
                    return MessageType.NewsData;
                case "NEWMARKET":
                    return MessageType.NewMarket;
                default:
                    return MessageType.Unknown;
            }
        }
    } // end of class WebsocketGDAX




    public static class TestWebsocketGDAX
    {

        private static readonly ManualResetEvent resetEvent = new ManualResetEvent(false);
        private static WebsocketGDAX socket;

        public static void TryMe()
        {
            Console.WriteLine("TestWebsocketGDAX::TryMe()");

            // Initialize an instance of our socket
            socket = new WebsocketGDAX(new ApiCredentials
            {
                ApiKey = "5208f2ce3d22abdbd7aa693dcd7cb9b7",
                ApiSecret = "fa46a996112f17cb35efc4d300e2c4f9"
            });

            // Subscribe to OnClientReady-event so we know when we can subscribe to trade channels
            socket.OnClientReady += Socket_OnClientReady;

            // Subscribe to the OnTradeMessage-event so we can receive trade messages
            socket.OnTradeMessage += Socket_OnTradeMessage;

            // Finally we can connect to our socket and wait for incoming messages
            socket.Connect();

            // Forces the methods not to exit
            resetEvent.WaitOne();
        }

        private static void WriteLog(string message)
        {
            Debug.WriteLine($"{DateTime.UtcNow}: {message}");
            Console.WriteLine($"{DateTime.UtcNow}: {message}");
        }

        private static void Socket_OnTradeMessage(string exchange, string primaryCurrency,
                                           string secondaryCurrency, TradeItem trade)
        {
            WriteLog($"Received new trade for (exchange) market {primaryCurrency}/{secondaryCurrency} price {trade.Price}");

            /*WriteLog($"Received new trade for {exchange} market
                     { primaryCurrency}/{ secondaryCurrency}
            price { trade.Price}
            ");*/
        }

        private static void Socket_OnClientReady()
        {
            Console.WriteLine("TestWebsocketGDAX::Socket_OnClientReady()");
            // Subscribe to a new trade channel
            socket.SubscribeToTradeChannel("BMEX", "XBT", "USD");
        }
    } // end of class TestWebsocketGDAX

} // end of namespace
