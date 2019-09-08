using System;
using PureSocketCluster;
using Tools.Websockets.Models;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Tools.Websockets
{
    public class ZWebsocket
    {
        // Note: Coinigy's channel names need to be in the following format:
        // METHOD-EXCHANGECODE--PRIMARYCURRENCY--SECONDARYCURRENCY

        // The following string represents a specific private channel on the Coinigy 
        // Websocket API that provides real-time streaming data for your account only.
        // To utilize this feature, first connect to the Websocket API and then subscribe
        // to the following channel: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        // Check "COINIGY_WEBSOCKET" in ApiCredentials.

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

        // The user can subscribe to the OnClientReady-event and start subscribing to trade channels once it gets invoked
        public event ClientIsReady OnClientReady;
        public delegate void ClientIsReady();

        // The user can subscribe to the OnTradeMessage to receive our deserialized trade market data:
        public event TradeMessage OnTradeMessage;
        public delegate void TradeMessage(string exchange, string primaryCurrency,
                                          string secondaryCurreny, TradeItem trade);

        private readonly PureSocketClusterSocket socket;
        private readonly WebsocketApiCredentials credentials;

        public ZWebsocket(WebsocketApiCredentials credentials, string url)
        {
            this.credentials = credentials;

            this.socket = new PureSocketClusterSocket(url);

            this.socket.OnOpened += On_Opened;
            this.socket.OnMessage += On_Message;
            this.socket.OnClosed += On_Closed;
            this.socket.OnData += On_Data;
            this.socket.OnError += On_Error;
            this.socket.OnFatality += On_Fatality;
            this.socket.OnSendFailed += On_SendFailed;
            this.socket.OnStateChanged += On_StateChanged;
            // this.socket.OnSubscribe()
        }

        private void On_StateChanged(System.Net.WebSockets.WebSocketState newState, System.Net.WebSockets.WebSocketState prevState)
        {
            Console.WriteLine("---STATECHANGED: {0} => {1}", prevState, newState);
        }

        private void On_SendFailed(string data, Exception ex)
        {
            throw new NotImplementedException();
        }

        private void On_Fatality(string reason)
        {
            throw new NotImplementedException();
        }

        private void On_Error(Exception ex)
        {
            throw new NotImplementedException();
        }

        private void On_Closed(System.Net.WebSockets.WebSocketCloseStatus reason)
        {
            throw new NotImplementedException();
        }

        private void On_Data(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void On_Opened()
        {
            Console.WriteLine("---OPENED");

            //socket.Emit("auth", this.credentials, ack: (string name, object error, object data) =>
            //Console.WriteLine(this.credentials.ToString());
            OnClientReady?.Invoke();
            return;
            socket.Emit("auth", this.credentials, ack: (string name, object err, object data) =>
            { 
                if (!((bool)err) && (data != null))
                {
                    this.socket.SetAuthToken((string)data);
                    this.socket.Connect();
                    OnClientReady?.Invoke();
                    this.socket.Subscribe("ORDER-PLNX--BTC--ETH");
                    /*var scChannel = this.socket.subscribe("ORDER-PLNX--BTC--ETH");
                    scChannel.watch(function(data) {
                        console.log(data);
                    });*/
                }
                else
                {
                    Console.WriteLine(err);
                }
            });
            /*{
                OnClientReady?.Invoke();
            });*/

        }

        // When we receive a message, we first need to check if it is a "publish" message.
        // Once the message has been verified, we can parse it into a JObject and send it
        // to our GetRequestType-function to determine the type of request.
        private void On_Message(string message)
        {
            Console.WriteLine("---MESSAGE: '{0}'", message);

            if (message == "#1")
            {
                socket.Emit("auth", this.credentials, ack: (string name, object err, object data) =>
                {
                    if (!((bool)err) && (data != null))
                    {
                        this.socket.SetAuthToken((string)data);
                        this.socket.Connect();
                        OnClientReady?.Invoke();
                        this.socket.Subscribe("ORDER-PLNX--BTC--ETH");
                        /*var scChannel = this.socket.subscribe("ORDER-PLNX--BTC--ETH");
                        scChannel.watch(function(data) {
                            console.log(data);
                        });*/
                    }
                    else
                    {
                        Console.WriteLine(err);
                    }
                });
                return;
                this.socket.Subscribe("ORDER-PLNX--BTC--ETH");
            }

            string PUBLISH_REGEX = @"^{""event""*.:*.""#publish""";
            string SETAUTH_REGEX = @"^{""event""*.:*.""#setAuthToken""";

            if (Regex.Match(message, SETAUTH_REGEX).Success)
            {
                var jo = JObject.Parse(message);
                string token = jo["data"]["token"].ToString();
                Console.WriteLine("Set auth token: {0}", token);
                this.socket.SetAuthToken(token);

                this.socket.Subscribe("TRADE-PLNX--BTC--ETH");
                return;
            }
            
            // Determine if message is a publish message using regex
            var m = Regex.Match(message, PUBLISH_REGEX);
            if (!m.Success) return;

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

        private void InvokeMessageReceived(string channelName, string requestType, string message)
        {
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

        public bool SubscribeToTradeChannel(string exchange, string primaryCurrency, string secondaryCurrency) =>
            this.socket.Subscribe($"TRADE-{exchange}--{primaryCurrency}--{secondaryCurrency}");

        // Request
        // Subscribe to ETH-USD and ETH-EUR with the level2, heartbeat and ticker channels,
        // plus receive the ticker entries for ETH-BTC and ETH-USD
        private static string strGdaxSub = @"
            {
                ""type"": ""subscribe"",
                ""product_ids"": [
                    ""ETH-USD"",
                    ""ETH-EUR""
                ],
                ""channels"": [
                    ""level2"",
                    ""heartbeat"",
                    {
                        ""name"": ""ticker"",
                        ""product_ids"": [
                            ""ETH-BTC"",
                            ""ETH-USD""
                        ]
                    }
                ]
            }";

        public bool SubscribeToGdax(string[] productIds) =>
            //this.socket.Subscribe(strGdaxSub);
            this.socket.Subscribe(@"{""product_ids"": [""ETH-USD"",""ETH-EUR""],""channels"":[""level2"",""heartbeat""]}");

        private static string GetRequestType(JObject jObj)
        {
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

        // Determine the message type based on the request type
        private static MessageType GetMessageType(string requestType)
        {
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

        // Allow the user to connect to the socket
        public bool Connect()
        {
            return this.socket.Connect();
        }
    } // end of class BitfinexWebsocket

} // end of namespace
