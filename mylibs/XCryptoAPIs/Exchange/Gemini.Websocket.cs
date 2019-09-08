using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
using System.IO;
using Newtonsoft.Json;
//using CryptoAPIs.Exchange.Clients.Gemini;

namespace CryptoAPIs.Exchange
{
    // https://docs.gemini.com/websocket-api/

    public partial class Gemini
    {
        JsonSerializerSettings m_settings = new JsonSerializerSettings();

        #region --------------------------------------------------------------------------------------------------------
        public void WebSocketMessageHandler(MessageArgs e)
        {
            m_settings.NullValueHandling = NullValueHandling.Ignore;

            try
            {
                //cout(e.Text);
                string text = e.Text.Replace("\u0000", string.Empty);

                /*JObject jo = (JObject)JsonConvert.DeserializeObject(text);
                string type = (string)jo["type"];
                int eventId = (int)jo["eventId"];
                int socketSequence = (int)jo["socket_sequence"];
                JArray ja = (JArray)jo["events"];*/

                JObject jo = (JObject)JsonConvert.DeserializeObject(text);
                string msgType = (string)jo["type"];

                // We expect to handle 'update' events
                if (msgType != "update")
                {
                    cout("Gemini::WebSocketMessageHandler=> Unexpected message type: {0}", msgType);
                    return;
                }

                GeminiWebSocketMessage msg = JsonConvert.DeserializeObject<GeminiWebSocketMessage>(text, m_settings);

                foreach (var ge in msg.events)
                {
                    if (ge.type == "change")
                    {
                        if (ge.side == "bid")
                        {
                            m_orderBook.UpdateBid(ge.price, ge.remaining);
                        }
                        else if (ge.side == "ask")
                        {
                            m_orderBook.UpdateAsk(ge.price, ge.remaining);
                        }
                        else
                        {
                            ErrorMessage("Gemini::WebSocketMessageHandler=> unknown 'side' in event: {0}", ge.side);
                        }
                    }
                    else if (ge.type == "trade")
                    {
                        dout("Gemini::WebSocketMessageHandler=> Received 'trade' event");
                    }
                    else if (ge.type == "auction")
                    {
                        cout("Gemini::WebSocketMessageHandler=> Received 'auction' event, which is currently unsupported.");
                    }
                }

                FireOrderBookUpdate();

                /*JObject msg = JObject.Parse(e.Text);
                //dynamic dict = JContainer.Parse(e.Text);
                //dynamic list = JArray.Parse(e.Text);
                if (msg["type"] != null && msg["type"].ToString() == "update")
                {

                    //cout(msg["type"]);
                    long eventId = msg["eventId"].Value<long>();
                    int socketSequence = msg["socket_sequence"].Value<int>();
                    //var gemEvents = msg["events"].Value<List<GeminiWSEvent>>();
                    var gemEvents = msg["events"] as JArray;
                    //m_orderBook.Clear();
                    for (int i = 0; i < gemEvents.Count(); ++i)
                    {
                        //cout(gemEvents[i]);
                        var ge = gemEvents[i] as JObject;
                        //foreach (var child in ge.Children())
                        //{
                        //    if (child. StartsWith("\u0000"))
                        //    {
                        //        cout(child); 
                        //    }
                        //}

                        string type = ge["type"].Value<string>();
                        string reason = ge["reason"].Value<string>();
                        string price = ge["price"].Value<string>();
                        string delta = ge["delta"].Value<string>();
                        string remaining = ge["remaining"].Value<string>();
                        string side = ge["side"].Value<string>();
                        //cout("{0} {1} {2} {3} {4} {5}", type, reason, price, delta, remaining, side);
                        if (side == "bid")
                        {
                            m_orderBook.UpdateBid(price, remaining);
                        }
                        else if (side == "ask")
                        {
                            m_orderBook.UpdateAsk(price, remaining);
                        }
                    }
                    UpdateOrderBook();
                }
                else
                {
                    ErrorMessage("Unknown message type: ", Str(msg));
                }*/
            }
            catch (Exception ex)
            {
                ErrorMessage("Error parsing gemEvent: {0}", ex.Message);
            }
            return;
        }

        // pass in args[0] as symbol (like "BTCUSD")
        public void StartWebSocket(string[] args = null)
        {
            string socketUrl;
            if (args == null)
                socketUrl = "wss://api.gemini.com/v1/marketdata/btcusd";
            else
                socketUrl = string.Format("wss://api.gemini.com/v1/marketdata/{0}", args[0]);

            m_socket = new ZWebSocket(socketUrl, this.WebSocketMessageHandler);
        }

        // TODO: handle different args
        public void SubscribeWebSocket(string[] args = null)
        {
            /*string json = ":ethbtc";
            m_socket.SendMessage(json);*/
        }
        #endregion -----------------------------------------------------------------------------------------------------

    } // end of class Gemini


    public class GeminiWebSocketMessage
    {
        public string type { get; set; }
        public long eventId { get; set; }
        public int timestamp { get; set; }
        public long timestampms { get; set; }
        public int socket_sequence { get; set; }
        public List<GeminiWebSocketEvent> events { get; set; }

        public override string ToString()
        {
            return events.ToString();
        }
    } // end of class GeminiWebSocketResponse

    // This object's properties are a superset of an "update" event and a "trade" event (so it can represent either)
    public class GeminiWebSocketEvent
    {
        public string type { get; set; }
        public string side { get; set; }
        public decimal price { get; set; }
        public decimal remaining { get; set; }
        public decimal delta { get; set; }
        public string reason { get; set; }

        public long tid { get; set; }
        public decimal amount { get; set; }
        public string makerSide { get; set; }

        public override string ToString()
        {
            return string.Format("[type={0}, side={1}, price={2}, remaining={3}, delta={4}, reason={5}]", type, side, price, remaining, delta, reason);
        }
    } // end of class GeminiWebSocketEvent

} // end of namespace

