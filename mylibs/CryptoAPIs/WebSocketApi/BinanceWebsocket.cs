using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoTools.Models;
using Binance.Net.Objects.Spot.MarketStream;
using Binance.Net.Objects.Spot.UserStream;
using Binance.Net.Objects.Spot.SpotData;
using Binance.Net.Objects.Spot;
using Binance.Net.Enums;

namespace CryptoApis.WebsocketApi
{
    // https://github.com/JKorf/Binance.Net

    public class BinanceWebsocket
    {
        public delegate void TickerUpdateHandler(TickerUpdate update);
        public event TickerUpdateHandler UpdateTickers;

        public ConcurrentDictionary<string, BinanceStreamTick> BinanceTickers => m_tick;
        public ConcurrentDictionary<string, BinanceStreamOrderUpdate> BinanceOrders => m_order;
        public BinanceStreamAccountInfo BinanceAccountInfo => m_account;

        public static string ApiKey { get; set; }
        public static string ApiSecret { get; set; }

        //private CryptoTools.SymbolManager m_symbolManager;

        private static BinanceClient m_client;
        private static BinanceSocketClient m_socketClient;

        private string m_listenKey;
        private Timer m_timer;

        private ConcurrentDictionary<string, BinanceStreamTick> m_tick;
        private ConcurrentDictionary<string, BinanceStreamOrderUpdate> m_order;
        private ConcurrentDictionary<string, XBalance> m_balance;   //BinanceStreamBalance> m_balance;
        private BinanceStreamAccountInfo m_account;

        private static BinanceWebsocket m_instance;


        public static BinanceWebsocket Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new BinanceWebsocket(ApiKey, ApiSecret);
                }
                return m_instance;
            }
        }

        public static void SetCredentials(string apiKey, string apiSecret)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        private BinanceWebsocket(string apiKey, string apiSecret)
        {
            var options = new BinanceClientOptions();
            options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(apiKey, apiSecret);
            m_client = new BinanceClient(options);
            var soptions = new BinanceSocketClientOptions();
            soptions.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(apiKey, apiSecret);
            m_socketClient = new BinanceSocketClient(soptions);

            m_tick = new ConcurrentDictionary<string, BinanceStreamTick>();
            m_balance = new ConcurrentDictionary<string, XBalance>();   //BinanceStreamBalance>();
            m_order = new ConcurrentDictionary<string, BinanceStreamOrderUpdate>();

            InitializeBinanceTickers();
            InitializeBinanceAccountInfo();
            //InitializeBinanceOrderInfo();

            StartWebsockets();

            m_timer = new Timer(HandleTimerCallback, m_listenKey, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15));
        }

        private void HandleTimerCallback(object state)
        {
            //Console.WriteLine("HandleTimerCallback FIRED!!!");
            m_client.Spot.UserStream.KeepAliveUserStreamAsync(m_listenKey);
        }

        private async Task<string> StartUserStream()
        {
            var res = await m_client.Spot.UserStream.StartUserStreamAsync();
            return res.Data;    //.Data.ListenKey;
        }

        public async Task StopUserStream()
        {
            await m_client.Spot.UserStream.StopUserStreamAsync(m_listenKey);
            return;
        }

        private void InitializeBinanceTickers()
        {
            var res = m_client.Spot.Market.GetAllBookPrices();
            Console.Write("--- Initializing BINANCE BookPrice ");
            int count = 0;
            foreach (var bp in res.Data)
            {
                //Console.WriteLine("BINANCE BookPrice {0}", bp.Symbol);
                if (count++ % 10 == 0) Console.Write(".");
                // put BookPrice data into m_binaTick
            }
            Console.WriteLine(" ({0})", res.Data.Count());
        }

        private BinanceStreamAccountInfo Convert(BinanceAccountInfo ai)
        {
            var bai = new BinanceStreamAccountInfo();
            bai.Balances = Convert(ai.Balances);
            bai.BuyerCommission = ai.BuyerCommission;
            bai.CanDeposit = ai.CanDeposit;
            bai.CanTrade = ai.CanTrade;
            bai.CanWithdraw = ai.CanWithdraw;
            bai.MakerCommission = ai.MakerCommission;
            bai.SellerCommission = ai.SellerCommission;
            bai.TakerCommission = ai.TakerCommission;
            bai.EventTime = ai.UpdateTime;
            //bai.Event = 
            return bai;
        }

        private BinanceStreamBalance Convert(BinanceBalance bb)
        {
            var bsb = new BinanceStreamBalance();
            bsb.Asset = bb.Asset;
            bsb.Free = bb.Free;
            bsb.Locked = bb.Locked;
            return bsb;
        }

        private List<BinanceStreamBalance> Convert(IEnumerable<BinanceBalance> list)
        {
            var result = new List<BinanceStreamBalance>();
            foreach (var bb in list)
            {
                result.Add(Convert(bb));
            }
            return result;
        }

        private void InitializeBinanceAccountInfo()
        {
            var res = m_client.General.GetAccountInfo();
            Console.Write("--- Initializing BINANCE AccountInfo ");
            int count = 0;
            if (res.Success)
            {
                m_account = Convert(res.Data);
                /*foreach (var b in res.Data.Balances)
                {
                    //Console.WriteLine("BINANCE AccountInfo {0} free:{1} lock:{2} total:{3}", b.Asset, b.Free, b.Locked, b.Total);
                    if (count++ % 10 == 0) Console.Write(".");
                    // Put Balance data into m_balances
                    //var xbal = new XBalance(b.Asset, b.Free, b.Locked, b.Total);
                    m_balance[b.Asset] = b;
                }
                Console.WriteLine(" ({0})", res.Data.Balances.Count);*/
            }
            else
            {
                Console.WriteLine("ERROR {0}: {1}", res.Error.Code, res.Error.Message);
            }
        }

        private BinanceStreamOrderUpdate OrderToOrderUpdate(BinanceOrder o)
        {
            var oup = new BinanceStreamOrderUpdate();
            oup.ClientOrderId = o.ClientOrderId;
            oup.IcebergQuantity = o.IcebergQuantity;
            oup.IsWorking = o.IsWorking;
            oup.OrderId = o.OrderId;
            oup.Price = o.Price;
            oup.Quantity = o.Quantity;          //o.OriginalQuantity
            oup.Side = o.Side;
            oup.Status = o.Status;
            oup.StopPrice = o.StopPrice;
            oup.Symbol = o.Symbol;
            oup.CreateTime = o.CreateTime;
            oup.TimeInForce = o.TimeInForce;
            oup.Type = o.Type;
            return oup;
        }

        private void InitializeBinanceOrderInfo()
        {
            var res = m_client.Spot.Order.GetOpenOrders();
            Console.Write("--- Initializing BINANCE OrderInfo ");
            int count = 0;
            if (res.Success)
            {
                foreach (var o in res.Data)
                {
                    //Console.WriteLine("BINANCE AccountInfo {0} free:{1} lock:{2} total:{3}", b.Asset, b.Free, b.Locked, b.Total);
                    if (count++ % 10 == 0) Console.Write(".");
                    // Put Order data into m_order
                    var oup = OrderToOrderUpdate(o);
                    m_order[o.OrderId.ToString()] = oup;
                }
                Console.WriteLine(" ({0})", res.Data.Count());
            }
            else
            {
                Console.WriteLine("ERROR {0}: {1}", res.Error.Code, res.Error.Message);
            }
        }

        private void StartWebsockets()
        {
            var successSymbols = m_socketClient.Spot.SubscribeToAllSymbolTickerUpdates((data) =>
            {
                //Console.WriteLine(">>>>> {0} BINANCE tickers", data.Length);
                for (int i = 0; i < data.Count(); ++i)
                {
                    m_tick[data.ElementAt(i).Symbol] = data.ElementAt(i) as BinanceStreamTick;
                }
            });

            var lk = m_client.Spot.UserStream.StartUserStream();
            if (lk.Success)
            {
                m_listenKey = lk.Data;  //.ListenKey;
                var successsUser = m_socketClient.Spot.SubscribeToUserDataUpdates(m_listenKey,
                    UpdateAccount,
                    UpdateOrder,
                    UpdateOcoOrder,
                    UpdateAccountPosition,
                    UpdateAccountBalance
                );
            }
            else
            {
                Console.WriteLine("Error {0} getting BINANCE ListenKey from StartUserStream(): {1}", lk.Error.Code, lk.Error.Message);
            }
        }

        private void UpdateAccount(BinanceStreamAccountInfo accountInfo)
        {
            foreach (var b in accountInfo.Balances)
            {
                var xbal = new XBalance(b.Asset, b.Free, b.Locked, b.Total);

                if (m_balance.ContainsKey(b.Asset))
                {
                    if (IsUpdated(b))
                    {
                        m_balance[b.Asset] = xbal;  // balance is updated
                    }
                }
                else
                {
                    m_balance[b.Asset] = xbal;         // balance is added
                }
            }
            m_account = accountInfo;
        }

        private void UpdateOrder(BinanceStreamOrderUpdate orderUpdate)
        {
            string oid = orderUpdate.OrderId.ToString();
            if (m_order.ContainsKey(oid))
            {
                if (IsUpdated(orderUpdate))
                {
                    m_order[oid] = orderUpdate;     // order is updated
                }
            }
            else
            {
                m_order[oid] = orderUpdate;         // order is added
            }
        }

        private void UpdateOcoOrder(BinanceStreamOrderList orderUpdate)
        {
            Console.WriteLine("Binance UpdateOcoOrder");
            /*string oid = orderUpdate.OrderId.ToString();
            if (m_order.ContainsKey(oid))
            {
                if (IsUpdated(orderUpdate))
                {
                    m_order[oid] = orderUpdate;     // order is updated
                }
            }
            else
            {
                m_order[oid] = orderUpdate;         // order is added
            }*/
        }

        private void UpdateAccountPosition(BinanceStreamPositionsUpdate posUpdate)
        {
            Console.WriteLine("Binance UpdateAccountPosition");
        }

        private void UpdateAccountBalance(BinanceStreamBalanceUpdate balUpdate)
        {
            Console.WriteLine("Binance UpdateAccountBalance");
        }

        private bool IsUpdated(BinanceStreamBalance balance)
        {
            //BinanceStreamBalance b = m_balance[balance.Asset];
            XBalance b = m_balance[balance.Asset];
            bool match = (b.Free == balance.Free && b.Locked == balance.Locked && b.Total == balance.Total);
            return !match;
        }

        private bool IsUpdated(BinanceStreamOrderUpdate orderUpdate)
        {
            BinanceStreamOrderUpdate ou = m_order[orderUpdate.OrderId.ToString()];
            //bool match = ()
            return true;
        }

        private void BinanceWebsocketPublic()
        {
            var successDepth = m_socketClient.Spot.SubscribeToOrderBookUpdates("bnbbtc", updateInterval:1000, (data) =>     //SubscribeToDepthStream("bnbbtc", (data) =>
            {
                // handle data
            });
            var successTrades = m_socketClient.Spot.SubscribeToTradeUpdates("bnbbtc", (data) =>     //SubscribeToTradesStream("bnbbtc", (data) =>
            {
                // handle data
            });
            var successKline = m_socketClient.Spot.SubscribeToKlineUpdates("bnbbtc", KlineInterval.OneMinute, (data) =>   //SubscribeToKlineStream("bnbbtc", KlineInterval.OneMinute, (data) =>
            {
                // handle data
            });
            var successSymbol = m_socketClient.Spot.SubscribeToSymbolTickerUpdates("bnbbtc", (data) =>  //SubscribeToSymbolTicker("bnbbtc", (data) =>
            {
                // handle data
            });
            var successSymbols = m_socketClient.Spot.SubscribeToAllSymbolTickerUpdates((data) =>    //SubscribeToAllSymbolTicker((data) =>
            {
                // handle data
            });
            var successOrderBook = m_socketClient.Spot.SubscribeToPartialOrderBookUpdates("bnbbtc", levels:10, updateInterval:1000, (data) =>   //SubscribeToPartialBookDepthStream("bnbbtc", 10, (data) =>
            {
                // handle data
            });
        }

        // For the private endpoint a user stream has to be started on the Binance server. This can be done
        // using the StartUserStream() method in the BinanceClient. This command will return a listen key which
        // can then be provided to the private socket subscription.
        // When no longer listening to private endpoints the client.StopUserStream method in BinanceClient
        // should be used to signal the Binance server the stream can be closed.
        private void BinanceWebsocketPrivate(string listenKey)
        {
            var successOrderBook = m_socketClient.Spot.SubscribeToUserDataUpdates(listenKey,    //SubscribeToUserStream(listenKey,
            (accountInfo) =>
            {
                // handle account info update
            },
            (orderUpdate) =>
            {
                // handle order update
            },
            (orderList) =>
            {
                // handle OCO order update
            },
            (positionsUpdate) =>
            {
                // handle account position update
            },
            (balanceUpdate) =>
            {
                // handle account balance update
            });
        }

        private void BinanceWebsocketHandleEvents()
        {
            // SUBSCRIBE
            var sub = m_socketClient.Spot.SubscribeToAllSymbolTickerUpdates(data =>
            {
                Console.WriteLine("Reveived list update");
            });

            // HANDLE EVENTS
            //sub.Data.Closed += () =>
            sub.Data.ConnectionLost += () =>
            {
                Console.WriteLine("Connection lost");
            };

            sub.Data.ConnectionRestored += (timeOffline) =>
            {
                Console.WriteLine($"Connection restored after {timeOffline}");
            };

            sub.Data.Exception += (e) =>
            {
                Console.WriteLine("Socket error " + e.Message);
            };

            Thread.Sleep(15000);

            // UNSUBSCRIBE
            m_socketClient.Unsubscribe(sub.Data);   //UnsubscribeFromStream(sub.Data);

            // Additionaly, all sockets can be closed with the UnsubscribeAllStreams method.
            m_socketClient.UnsubscribeAll();
        }

        // where symbols like "bnbbtc", "ethbtc", ...
        private void SubscribeToAll(string[] symbols)
        {
            var successDepth = m_socketClient.Spot.SubscribeToOrderBookUpdates(symbols, updateInterval:1000, (data) =>
            {
                // handle data
            });
            var successTrades = m_socketClient.Spot.SubscribeToTradeUpdates(symbols, (data) =>
            {
                // handle data
            });
            var successKline = m_socketClient.Spot.SubscribeToKlineUpdates(symbols, KlineInterval.OneMinute, (data) =>
            {
                // handle data
            });
            /*var successSymbol = m_socketClient.SubscribeToSymbolTicker(symbol, (data) =>
            {
                // handle data
            });*/
            var successSymbols = m_socketClient.Spot.SubscribeToAllSymbolTickerUpdates((data) =>
            {
                // handle data
            });
            var successOrderBook = m_socketClient.Spot.SubscribeToPartialOrderBookUpdates(symbols, levels:10, updateInterval:1000, (data) =>
            {
                // handle data
            });
        }



        public static void TestBinanceTickers()
        {
            const int BYTE_SIZE = 1024;

            Console.WriteLine("LAUNCH: Binance");

            var socket = new ClientWebSocket();
            Task task = socket.ConnectAsync(new Uri("wss://stream.binance.com:9443/stream?streams=btcusdt@trade/ethusdt@trade/ethbtc@trade"), CancellationToken.None);
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

                        JObject jo = JsonConvert.DeserializeObject<JObject>(jsonString);
                        string stream = jo["stream"].Value<string>();
                        jo = jo["data"].Value<JObject>();
                        string type = jo["e"].Value<string>();
                        //Console.WriteLine("stream: {0}   eventType: {1}", stream, type);


                        if (type == "trade")                // "trade": 
                        {
                            //string pid = jo["product_id"].Value<string>();
                            int i = stream.IndexOf("@");
                            string pid = stream.Substring(0, i);
                            string price = jo["p"].Value<string>();
                            long time = jo["E"].Value<long>();
                            string size = jo["q"].Value<string>();
                            Console.WriteLine("{0}  {1} {2}  {3}", pid, price, size, time);
                        }
                        /*else if (type == "update")          // "update"
                        {
                            //Console.WriteLine(jsonString);
                        }
                        else if (type == "error")           // "error"
                        {
                        }
                        else if (type == "heartbeat")       // "heartbeat"
                        {
                        }*/

                        Array.Clear(recBytes, 0, BYTE_SIZE);
                        //recBytes = new byte[BYTE_SIZE];
                    }
                });
            readThread.Start();

            //Console.ReadLine();
        }

    } // end of class Binance
} // end of namespace
