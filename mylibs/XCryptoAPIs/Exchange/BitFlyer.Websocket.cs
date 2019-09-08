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
using CryptoAPIs.Exchange.Clients.Poloniex;
using System.IO;
using Newtonsoft.Json;
using PubnubApi;

namespace CryptoAPIs.Exchange
{
    // https://lightning.bitflyer.jp/docs?lang=en

    public partial class BitFlyer
    {

        #region --------------------------------------------------------------------------------------------------------
        public void WebSocketMessageHandler(MessageArgs e)
        {
            string text = e.Text.Replace("\u0000", string.Empty);

            ErrorMessage("{0} BitFlyer.Websocket=> unknown message type:\n{1}", DateTime.Now.ToString("HH:mm:ss"), text);
            return;
        }

        // TODO: pass in the currency pair
        public void StartWebSocket(string[] args = null)
        {
            try
            {
                //base.m_socket = new ZWebSocket(this.WebsocketUrl, this.WebSocketMessageHandler);
                var board = m_api.PublicApi.GetBoard("BTC_USD").Result;                  // get the initial order book
                foreach (var b in board.Bids)
                {
                    m_orderBook.Bids.Add(b.Price, b.Size);
                }
                foreach (var a in board.Asks)
                {
                    m_orderBook.Asks.Add(a.Price, a.Size);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("An error occurred starting websocket for BitFlyer: {0}", ex.Message);
            }
        }

        // Where pair like "BTC_USD"
        // TODO: change interface so SubscribeWebSocket takes (at least) a currency pair argument
        public void SubscribeWebSocket(string[] args = null)
        {
            var config = new PNConfiguration();
            config.SubscribeKey = "sub-c-52a9ab50-291b-11e5-baaa-0619f8945a4f";
            //config.AuthKey = this.ApiKey;
            //config.CipherKey = this.ApiSecret;
            var pubnub = new Pubnub(config);

            SubscribeCallbackExt listenerSubscribeCallack = new SubscribeCallbackExt(
                (pubnubObj, message) => {
                    // Handle new message stored in message.Message 
                    string channel = message.Channel;
                    long timeToken = message.Timetoken;
                    //cout("channel: {0}", channel);
                    if (channel == "lightning_ticker_BTC_USD")
                    {
                        var ticker = JsonConvert.DeserializeObject<BitFlyerWebsocketTicker>((string)message.Message);
                        //cout(ticker.ToString());
                    }
                    else if (channel == "lightning_board_BTC_USD")
                    {
                        var obupdate = JsonConvert.DeserializeObject<BitFlyerOrderBookUpdate>((string)message.Message);
                        foreach (var b in obupdate.bids)
                        {
                            m_orderBook.UpdateBid(b.price, b.size);
                        }
                        foreach (var a in obupdate.asks)
                        {
                            m_orderBook.UpdateAsk(a.price, a.size);
                        }
                        FireOrderBookUpdate();
                        //cout(obupdate.ToString());
                    }
                },
                (pubnubObj, presence) => {
                    // handle incoming presence data 
                },
                (pubnubObj, status) => {
                    // the status object returned is always related to subscribe but could contain
                    // information about subscribe, heartbeat, or errors
                    // use the PNOperationType to switch on different options
                    switch (status.Operation)
                    {
                        // let's combine unsubscribe and subscribe handling for ease of use
                        case PNOperationType.PNSubscribeOperation:
                        case PNOperationType.PNUnsubscribeOperation:
                            // note: subscribe statuses never have traditional
                            // errors, they just have categories to represent the
                            // different issues or successes that occur as part of subscribe
                            switch (status.Category)
                            {
                                case PNStatusCategory.PNConnectedCategory:
                                    // this is expected for a subscribe, this means there is no error or issue whatsoever
                                    break;
                                case PNStatusCategory.PNReconnectedCategory:
                                    // this usually occurs if subscribe temporarily fails but reconnects. This means
                                    // there was an error but there is no longer any issue
                                    break;
                                case PNStatusCategory.PNDisconnectedCategory:
                                    // this is the expected category for an unsubscribe. This means there
                                    // was no error in unsubscribing from everything
                                    break;
                                case PNStatusCategory.PNUnexpectedDisconnectCategory:
                                    // this is usually an issue with the internet connection, this is an error, handle appropriately
                                    break;
                                case PNStatusCategory.PNAccessDeniedCategory:
                                    // this means that PAM does allow this client to subscribe to this
                                    // channel and channel group configuration. This is another explicit error
                                    break;
                                default:
                                    // More errors can be directly specified by creating explicit cases for other
                                    // error categories of `PNStatusCategory` such as `PNTimeoutCategory` or `PNMalformedFilterExpressionCategory` or `PNDecryptionErrorCategory`
                                    break;
                            }
                            break;
                        case PNOperationType.PNHeartbeatOperation:
                            // heartbeat operations can in fact have errors, so it is important to check first for an error.
                            if (status.Error)
                            {
                                // There was an error with the heartbeat operation, handle here
                            }
                            else
                            {
                                // heartbeat operation was successful
                            }
                            break;
                        default:
                            // Encountered unknown status type
                            break;
                    }
                });

            pubnub.AddListener(listenerSubscribeCallack);

            pubnub.Subscribe<string>()
                .Channels(new string[] {
                    //"lightning_ticker_BTC_JPY",
                    "lightning_ticker_BTC_USD",
                    "lightning_board_BTC_USD"
                })
                .Execute();
        }
        #endregion -----------------------------------------------------------------------------------------------------

    } // end of class BitFlyer

    
    public class BitFlyerWebsocketTicker
    {
        public string product_code { get; set; }
        public string timestamp { get; set; }
        public int tick_id { get; set; }
        public decimal best_bid { get; set; }
        public decimal best_ask { get; set; }
        public decimal best_bid_size { get; set; }
        public decimal best_ask_size { get; set; }
        public decimal total_bid_depth { get; set; }
        public decimal total_ask_depth { get; set; }
        public decimal ltp { get; set; }
        public decimal volume { get; set; }
        public decimal volume_by_product { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}:{4}-{5}:{6} {7} {8} {9} {10} {11}", product_code, timestamp, tick_id, best_bid_size, best_bid, best_ask, best_ask_size, total_bid_depth, total_ask_depth, ltp, volume, volume_by_product);
        }
    } // end of class BItFlyerWebsocketTicker

    public class BitFlyerOrderBookEntry
    {
        public decimal price { get; set; }
        public decimal size { get; set; }
    } // end of class BitFlyerOrderBookEntry
    public class BitFlyerOrderBookUpdate
    {
        public decimal mid_price { get; set; }
        public List<BitFlyerOrderBookEntry> bids { get; set; }
        public List<BitFlyerOrderBookEntry> asks { get; set; }

        public override string ToString()
        {
            return string.Format("mid:{0}   {1} bid(s)  {2} ask(s)", mid_price, bids.Count, asks.Count);
        }
    } // end of class BitFlyerOrderBookUpdate

} // end of namespace

