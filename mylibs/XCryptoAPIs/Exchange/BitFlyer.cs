using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools;
using static Tools.G;
using CryptoAPIs.Exchange.Clients.BitFlyer;
using PubnubApi;
using Newtonsoft.Json;

namespace CryptoAPIs.Exchange
{
    // https://lightning.bitflyer.jp/docs/api
    // https://lightning.bitflyer.jp/docs/playground?lang=en

    public partial class BitFlyer : BaseExchange, IOrderExchange, IExchangeWebSocket
    {
        public bool EnableLiveOrders { get; set; }

        public override string BaseUrl { get { return "https://api.bitflyer.jp/"; } }
        public override string Name { get { return "BITFLYER"; } }
        public override CryptoExch Exch => CryptoExch.BITFLYER;

        BitflyerClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static BitFlyer m_instance;
        public static BitFlyer Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static BitFlyer Create(string apikey = "", string apisecret = "")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new BitFlyer(apikey, apisecret);
        }
        private BitFlyer(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            m_api = new BitflyerClient(ApiKey, ApiSecret);
            m_instance = this;
        }


        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    if (CurrencyPairs.Count > 0)
                    {
                        UpdateSymbolsFromCurrencyPairs();
                        return m_symbolList;
                    }

                    m_symbolList = new List<string>();
                    var markets = m_api.PublicApi.GetMarkets().Result;
                    foreach (var m in markets)
                    {
                        m_symbolList.Add(m.ProductCode);
                    }
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;
            }
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var symbols = SymbolList;
            foreach (var s in symbols)
            {
                result[s] = GetTicker(s);
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            //var book = m_api.PublicApi.GetMarkets().Result;
            var book = new BitFlyerOrderBook();
            return book as ZCryptoOrderBook;
        }

        public override ZTicker GetTicker(string symbol)
        {
            var res = m_api.PublicApi.GetTicker(symbol).Result;
            return new BitFlyerTicker(res);
        }

        public Dictionary<string, ZTicker> GetTickers(List<string> symbols = null)
        {
            if (symbols == null) symbols = SymbolList;         // passing null gets ALL tickers
            var tickers = GetTickers(symbols);
            return tickers;
        }

        public override void SubscribeOrderBookUpdates(ZCurrencyPair pair, bool subscribe)
        {
            StartWebSocket(new string[] { pair.Symbol });
            SubscribeWebSocket();
        }

        #region ---------- IOrderExchange IMPLEMENTATION -----------------------------------------------------------------------
        public OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty)
        {
            if (!EnableLiveOrders)
            {
                cout("\nBITFLYER::SubmitLimitOrder=> '{0}' {1} {2} {3}\n", pair, side, price, qty);
                return null;
            }
            Side orderSide = (side == OrderSide.Buy ? Side.Buy : Side.Sell);
            var orderArgs = new SendChildOrderParameter
            {
                ProductCode = pair,
                ChildOrderType = ChildOrderType.Limit,
                Side = orderSide,
                Price = (double)price,
                Size = (double)qty,
                MinuteToExpire = 10000,
                TimeInForce = TimeInForce.GoodTilCanceled
            };
            var res = AsyncHelpers.RunSync<PostResult>(() => m_api.PrivateApi.SendChildOrder(orderArgs));
            //var res = m_api.PrivateApi.SendChildOrder(orderArgs).Result;
            return new OrderNew(pair, res);
        }

        public OrderCxl CancelOrder(string pair, string orderId)
        {
            if (!EnableLiveOrders)
            {
                cout("\nBITFLYER::CancelOrder=> {0} [{1}]\n", pair, orderId);
                return null;
            }
            m_api.PrivateApi.CancelChildOrder(new CancelChildOrderParameter
            {
                ProductCode = pair,
                ChildOrderId = orderId
                //ChildOrderAcceptanceId = "?"
            }).RunSynchronously();
            return new OrderCxl(pair, orderId);
        }

        public IEnumerable<ZOrder> GetWorkingOrders(string pair)
        {
            var result = new List<ZOrder>();
            int? count = default(int?), before = default(int?), after = default(int?);
            ChildOrderState state = ChildOrderState.Active;
            var res = m_api.PrivateApi.GetChildOrders(pair, count, before, after, state).Result;
            foreach (var o in res)
            {
                result.Add(new ZOrder(o));
            }
            return result;
        }

        public IEnumerable<ZTrade> GetTrades(string pair)
        {
            var result = new List<ZTrade>();
            int? count = default(int?), before = default(int?), after = default(int?);
            string childOrderId = null, childOrderAcceptanceId = null;
            var res = m_api.PrivateApi.GetExecutions(pair, count, before, after, childOrderId, childOrderAcceptanceId).Result;
            foreach (var t in res)
            {
                result.Add(new ZTrade(pair, t));
            }
            return result;
        }

        public IDictionary<string, ZAccountBalance> GetAccountBalances()
        {
            var result = new Dictionary<string, ZAccountBalance>();
            var res = m_api.PrivateApi.GetBalance().Result;
            foreach (var b in res)
            {
                var currency = b.CurrencyCode.ToString();
                result.Add(currency, new ZAccountBalance(currency, (decimal) b.Available, (decimal) (b.Amount - b.Available)));
            }
            return result;
        }
        #endregion -------------------------------------------------------------------------------------------------------------


        public async void TestWebsocket()
        {
            var board = await m_api.PublicApi.GetBoard("BTC_USD");                  // get the initial order book
            foreach (var b in board.Bids)
            {
                m_orderBook.Bids.Add(b.Price, b.Size);
            }
            foreach (var a in board.Asks)
            {
                m_orderBook.Asks.Add(a.Price, a.Size);
            }

            var config = new PNConfiguration();
            config.SubscribeKey = "sub-c-52a9ab50-291b-11e5-baaa-0619f8945a4f";
            //config.PublishKey = "demo";
            //config.AuthKey = this.ApiKey;
            //config.CipherKey = this.ApiSecret;
            var pubnub = new Pubnub(config);

            SubscribeCallbackExt listenerSubscribeCallack = new SubscribeCallbackExt(
                (pubnubObj, message) => {
                    // Handle new message stored in message.Message 
                    string channel = message.Channel;
                    long timeToken = message.Timetoken;
                    cout("channel: {0}", channel);
                    if (channel == "lightning_ticker_BTC_USD")
                    {
                        var ticker = JsonConvert.DeserializeObject<BitFlyerWebsocketTicker>((string)message.Message);
                        cout(ticker.ToString());
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
                        cout(obupdate.ToString());
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

    } // end of class BitFlyer

    //======================================================================================================================================

    public class BitFlyerTicker : ZTicker
    {
        Ticker ticker;

        public BitFlyerTicker(Ticker ticker)
        {
            this.ticker = ticker;
        }

        public override decimal Bid { get { return (decimal)ticker.BestBid; } }
        public override decimal BidSize { get { return (decimal)ticker.BestBidSize; } }
        public override decimal Ask { get { return (decimal)ticker.BestAsk; } }
        public override decimal AskSize { get { return (decimal)ticker.BestAskSize; } }
        public override decimal Last { get { return (decimal)ticker.LatestPrice; } }
        public override decimal High { get { return 0; } }
        public override decimal Low { get { return 0; } }
        public override decimal Volume { get { return (decimal)ticker.Volume; } }
        public override string Timestamp { get { return ticker.Timestamp.ToString(); } }
    } // end of class BitFlyerTicker

    public class BitFlyerBookEntry : ZCryptoOrderBookEntry
    {
        public override decimal Price { get { return decimal.MinValue; } }
        public override decimal Amount { get { return decimal.MinValue; } }
    }

    public class BitFlyerOrderBook : ZCryptoOrderBook
    {
        // List entries are [price, amount]
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }
    }

} // end of namespace 
