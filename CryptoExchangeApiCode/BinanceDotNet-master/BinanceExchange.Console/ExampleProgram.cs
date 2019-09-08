using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.IO;
//using System.Xml;
//using System.Text.RegularExpressions;
using BinanceExchange.API;
using BinanceExchange.API.Caching;
using BinanceExchange.API.Client;
using BinanceExchange.API.Enums;
using BinanceExchange.API.Market;
using BinanceExchange.API.Models.Request;
using BinanceExchange.API.Models.Response;
using BinanceExchange.API.Models.Response.Error;
using BinanceExchange.API.Models.Websocket;
using BinanceExchange.API.Utility;
using BinanceExchange.API.Websockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Extensions.Logging;
using LogLevel = NLog.LogLevel;
using static Tools.G;
using Tools;
using CryptoAPIs;
using CryptoAPIs.Exchange;

namespace BinanceExchange.Console
{
    /// <summary>
    /// This Console app provides a number of examples of utilising the BinanceDotNet library
    /// </summary>
    public class ExampleProgram
    {
        public static async void TestTelegram()
        {
            var bot = new TelegramBot("511987996:AAFlFjHD1oSAjcS_WgXoWc2GtWyCp-kG5KU");
            await bot.Send("@aclifford", "This is a test message from HAT_BOT");
            return;
        }

        public static void TestEncryptedApiCredentials()
        {
            string inputFile = Folders.misc_path("test.txt");

            //string password = "MY_TEST_PASSWORD_12345678";
            //var encrypt = new Tools.Encryption("MY_ENCRYPTION_VECTOR", password);
            var encrypt = new Tools.Encryption("somereallycooliv", "0123456789abcdef");

            //string filename = encrypt.FileEncrypt(inputFile);
            //encrypt.FileDecrypt(filename, filename + ".dec");
            //string text = "text to encrypt";

            /*string text;
            while (true)
            {
                System.Console.Write("Enter text to encrypt: ");
                text = System.Console.ReadLine();
                if (text.Length > 0)
                {
                    var encryptedText = encrypt.EncryptString(text);
                    var decryptedText = encrypt.DecryptString(encryptedText);
                    cout("Original: '{0}'\nEncrypted: '{1}'\nDecrypted: '{2}'\n\n", text, encryptedText, decryptedText);
                }
                else
                {
                    break;
                }
            }
            System.Console.WriteLine("\n\n");*/

            string loginFile = "test.txt.aes.json";
            //ReadXML(Folders.misc_path(loginFile));

            string pathname = Folders.misc_path(loginFile);
            //string json = GFile.ReadAll(pathname);
            //var keys = JArray.Parse(json);
            string json = File.ReadAllText(pathname);
            json = json.Replace("\n", "");
            json = json.Replace("\t", "");
            //json = Regex.Replace(json, @"\[\t]\", "");

            JsonSerializer se = new JsonSerializer();

            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader re = new StreamReader(stream);
            JsonTextReader reader = new JsonTextReader(re);
            var deser = se.Deserialize<ExchangeApiKey>(reader);
            deser.key = encrypt.DecryptString(deser.key);
            deser.secret = encrypt.DecryptString(deser.secret);
            string str = string.Format("'{0}'\n'{1}'\n'{2}'\n", deser.exchange, deser.key, deser.secret);
            cout(str);

            //var o1 = JObject.FromObject(json);

            /*var txtLines = GFile.ReadTextFileLines(Folders.misc_path(loginFile));
            foreach (var txt in txtLines)
            {
                if (txt.Trim() == "") continue;
                var decryptedText = encrypt.DecryptString(txt);
                cout("Encrypted: '{0}'\nDecrypted: '{1}'\n", txt, decryptedText);
            }*/

            return;
        }





        private static BinanceClient client;

        public static async Task Main(string[] args)
        {
            Crypto.Test();
            return;

            cout("----------------------------");
            cout("BinanceExchange API - Tester");
            cout("----------------------------");

            //Logging Configuration. 
            //Ensure that `nlog.config` is configured as you want, and is copied to output directory.
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            //This utilises the nlog.config from the build directory
            loggerFactory.ConfigureNLog("nlog.config");
            //For the sakes of this example we are outputting only fatal logs, debug being the lowest.
            LogManager.GlobalThreshold = LogLevel.Fatal;
            var logger = LogManager.GetLogger("*");

            //Provide your configuration and keys here, this allows the client to function as expected.
            //string apiPathname = @"\\vmware-host\Shared Folders\Downloads\crypto_api.txt";
            string apiPathname = @"/Users/michael/Downloads/crypto_api.txt";
            string[] lines = System.IO.File.ReadAllLines(apiPathname);
            string apiKey = lines[0];   // "YOUR_API_KEY";
            string secretKey = lines[1];    // "YOUR_SECRET_KEY";



            /*// ENTER A TRXETH SELL ORDER (the HARD way!!!)
            //string str = "symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.1&recvWindow=5000&timestamp=1499827319559";
            string timestamp = GetTimestamp();
            string str = "symbol=TRXETH&side=SELL&type=LIMIT&timeInForce=GTC&quantity=200&price=0.00011470&recvWindow=10000000&timestamp=" + timestamp;
            //string signature = CreateHMACSignature("NhqPtmdSJYdKjVHjA7PZj4Mge3R5YNiP1e3UZjInClVN65XAbvqqM6A7H5fATj0j", str);
            string signature = CreateHMACSignature(secretKey, str);
            cout(signature);
            //string cmd = "curl - H \"X-MBX-APIKEY: vmPUZE6mv9SD5VNHk4HlWFsOr6aKE2zvsw0MuIgwCIPy6utIco14y7Ju91duEh8A\" - X POST 'https://api.binance.com/api/v3/order?symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.1&recvWindow=5000&timestamp=1499827319559&signature=" + signature + "'";
            //apiKey = "vmPUZE6mv9SD5VNHk4HlWFsOr6aKE2zvsw0MuIgwCIPy6utIco14y7Ju91duEh8A";
            string cmd = "curl -H \"X-MBX-APIKEY: " + apiKey + "\" -X POST 'https://api.binance.com/api/v3/order?" + str + "&signature=" + signature + "'";
            cout(cmd);
            string result = Bash(cmd);
            System.Console.WriteLine("\n{0}", result);
            return;*/

            //Initialise the general client client with config
            client = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = apiKey,
                SecretKey = secretKey,
                Logger = logger,
            });

            cout("Interacting with Binance...");

            bool DEBUG_ALL = true;

            /*
             *  Code Examples - Make sure you adjust value of DEBUG_ALL
             */
            if (DEBUG_ALL)
            {
                int i;

                // ---TEST CLIENT CONNECTION---
                await client.TestConnectivity();


                // ---ACCOUNT INFO---
                var accountInfo = await client.GetAccountInformation();
                var balances = accountInfo.Balances;
                balances.Where(s => s.Free!=0.0M || s.Locked!=0.0M).ToList().ForEach(cout);
                var canDeposit = accountInfo.CanDeposit;
                var canTrade = accountInfo.CanTrade;
                var canWithdraw = accountInfo.CanWithdraw;
                var buyerComm = accountInfo.BuyerCommission;
                var sellerComm = accountInfo.SellerCommission;
                var makerComm = accountInfo.MakerCommission;
                var takerComm = accountInfo.TakerCommission;
                cout("commissions  buyer:{0} seller:{1} maker:{2} taker:{3}\npermissions  deposit:{4} trade:{5} withdraw:{6}", buyerComm, sellerComm, makerComm, takerComm, canDeposit, canTrade, canWithdraw);

                // SYMBOL TO TRADE
                string symbol = TradingPairSymbols.ETHPairs.TRX_ETH;




                return;



                /*// Create an order with varying options
                //var createOrder = await client.CreateOrder(new CreateOrderRequest()
                var createOrder = await client.CreateTestOrder(new CreateOrderRequest()                                        
                {
                    //IcebergQuantity = 200,
                    Price = 0.00011470,
                    Quantity = 200,
                    Side = OrderSide.Sell,
                    Symbol = "TRXETH",
                    Type = OrderType.Limit,
                    TimeInForce = TimeInForce.GTC
                });*/

                /*// Get All Orders
                cout("Getting all {0} orders...", symbol);
                var allOrdersRequest = new AllOrdersRequest()
                {
                    //Symbol = TradingPairSymbols.BTCPairs.ETH_BTC,
                    Symbol = symbol,
                    Limit = 50,
                };
                var allOrders = await client.GetAllOrders(allOrdersRequest);                // Get All Orders
                i = 0;
                foreach (var o in allOrders)
                {
                    cout("{0}) {1}", ++i, OrderStr(o));
                }*/

                // Get current open orders for the specified symbol
                cout("Getting open {0} orders...", symbol);
                var currentOpenOrders = await client.GetCurrentOpenOrders(new CurrentOpenOrdersRequest()
                {
                    Symbol = symbol,
                });
                i = 0;
                foreach (var o in currentOpenOrders)
                {
                    cout("{0}) {1}", ++i, o.ToString());
                }

                if (currentOpenOrders.Count > 0)
                {
                    long lastOrderId = currentOpenOrders.FindLast(m => m.Symbol != null).OrderId;
                    // TODO: The API docs seem to indicate these client order IDs can be strings like "MyOrder1" or "CancelMyOrder1" (?)
                    // Cancel an order
                    cout("Cancelling order...");
                    var co = new CancelOrderRequest()
                    {
                        //NewClientOrderId = "123456",
                        //OrderId = 12136215,
                        OrderId = lastOrderId,
                        //OriginalClientOrderId = "23525",
                        Symbol = symbol,
                    };
                    var cancelOrder = await client.CancelOrder(co);
                }

                return;

                // Get the order book, and use the cache
                cout("Getting {0} order book...", symbol);
                var orderBook = await client.GetOrderBook(symbol, true);
                orderBook.Asks.Reverse();
                foreach (var o in orderBook.Asks)
                {
                    //cout(OrderStr(o));
                }
                cout("---------");
                foreach (var o in orderBook.Bids)
                {
                    //cout(OrderStr(o));
                }

                /*// Cancel an order
                var cancelOrder = await client.CancelOrder(new CancelOrderRequest()
                {
                    NewClientOrderId = 123456,
                    OrderId = 523531,
                    OriginalClientOrderId = 23525,
                    Symbol = "ETHBTC",
                });*/


                // Get account information
                cout("Getting {0} account information...", symbol);
                var accountInformation = await client.GetAccountInformation();  //3500);
                cout("Account Information:");
                cout("CanTrade:{0} CanDeposit:{1} CanWithdraw:{2}", accountInformation.CanTrade, accountInformation.CanDeposit, accountInformation.CanWithdraw);
                System.Console.WriteLine("Commissions     buyer:{0}  seller:{1}  maker:{2}  taker:{3}", accountInformation.BuyerCommission, accountInformation.SellerCommission, accountInformation.MakerCommission, accountInformation.TakerCommission);
                System.Console.WriteLine("Balances:");
                foreach (var balance in accountInformation.Balances)
                {
                    if (balance.Free != 0 || balance.Locked != 0)
                    {
                        System.Console.WriteLine("{0}  free:{1}  locked:{2}", balance.Asset, balance.Free, balance.Locked);
                    }
                }
                System.Console.WriteLine("");

                return;

                // Get account trades
                var accountTrades = await client.GetAccountTrades(new AllTradesRequest()
                {
                    FromId = 352262,
                    Symbol = symbol,
                });

                // Get a list of Compressed aggregate trades with varying options
                var aggTrades = await client.GetCompressedAggregateTrades(new GetCompressedAggregateTradesRequest()
                {
                    StartTime = DateTime.UtcNow.AddDays(-1),
                    Symbol = symbol,
                });

                /*// Get current open orders for the specified symbol
                var currentOpenOrders = await client.GetCurrentOpenOrders(new CurrentOpenOrdersRequest()
                {
                    Symbol = symbol,
                });*/

                // Get daily ticker
                var dailyTicker = await client.GetDailyTicker(symbol);

                // Get Symbol Order Book Ticker
                var symbolOrderBookTicker = await client.GetSymbolOrderBookTicker();

                // Get Symbol Order Price Ticker
                var symbolOrderPriceTicker = await client.GetSymbolsPriceTicker();

                // Query a specific order on Binance
                var orderQuery = await client.QueryOrder(new QueryOrderRequest()
                {
                    OrderId = 5425425,
                    Symbol = symbol,
                });

                // Firing off a request and catching all the different exception types.
                try
                {
                    accountTrades = await client.GetAccountTrades(new AllTradesRequest()
                    {
                        FromId = 352262,
                        Symbol = "ETHBTC",
                    });
                }
                catch (BinanceBadRequestException badRequestException)
                {

                }
                catch (BinanceServerException serverException)
                {

                }
                catch (BinanceTimeoutException timeoutException)
                {

                }
                catch (BinanceException unknownException)
                {
                    
                }
            }

            // Start User Data Stream, ping and close
            var userData = await client.StartUserDataStream();
            await client.KeepAliveUserDataStream(userData.ListenKey);
            await client.CloseUserDataStream(userData.ListenKey);

            return;

            // Manual WebSocket usage
            var manualBinanceWebSocket = new InstanceBinanceWebSocketClient(client);
            var socketId = manualBinanceWebSocket.ConnectToDepthWebSocket("ETHBTC", b =>
            {
                System.Console.Clear();
                System.Console.WriteLine($"{JsonConvert.SerializeObject(b.BidDepthDeltas, Formatting.Indented)}");
                System.Console.SetWindowPosition(0, 0);
            });


            #region Advanced Examples           
            // This builds a local Kline cache, with an initial call to the API and then continues to fill
            // the cache with data from the WebSocket connection. It is quite an advanced example as it provides 
            // additional options such as an Exit Func<T> or timeout, and checks in place for cache instances. 
            // You could provide additional logic here such as populating a database, ping off more messages, or simply
            // timing out a fill for the cache.
            var dict = new Dictionary<string, KlineCacheObject>();
            //await BuildAndUpdateLocalKlineCache(client, "BNBBTC", KlineInterval.OneMinute,
            //    new GetKlinesCandlesticksRequest()
            //    {
            //        StartTime = DateTime.UtcNow.AddHours(-1),
            //        EndTime = DateTime.UtcNow,
            //        Interval = KlineInterval.OneMinute,
            //        Symbol = "BNBBTC"
            //    }, new WebSocketConnectionFunc(15000), dict);

            // This builds a local depth cache from an initial call to the API and then continues to fill 
            // the cache with data from the WebSocket
            var localDepthCache = await BuildLocalDepthCache(client);
            // Build the Buy Sell volume from the results
            var volume = ResultTransformations.CalculateTradeVolumeFromDepth("BNBBTC", localDepthCache);

            #endregion
            System.Console.WriteLine("Complete.");
            Thread.Sleep(6000);
            manualBinanceWebSocket.CloseWebSocketInstance(socketId);
            System.Console.ReadLine();
        }

        /// <summary>
        /// Build local Depth cache from WebSocket and API Call example.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task<Dictionary<string, DepthCacheObject>> BuildLocalDepthCache(IBinanceClient client)
        {
            // Code example of building out a Dictionary local cache for a symbol using deltas from the WebSocket
            var localDepthCache = new Dictionary<string, DepthCacheObject> {{ "BNBBTC", new DepthCacheObject()
            {
                Asks = new Dictionary<decimal, decimal>(),
                Bids = new Dictionary<decimal, decimal>(),
            }}};
            var bnbBtcDepthCache = localDepthCache["BNBBTC"];

            // Get Order Book, and use Cache
            var depthResults = await client.GetOrderBook("BNBBTC", true, 100);
            //Populate our depth cache
            depthResults.Asks.ForEach(a =>
            {
                if (a.Quantity != 0.00000000M)
                {
                    bnbBtcDepthCache.Asks.Add(a.Price, a.Quantity);
                }
            });
            depthResults.Bids.ForEach(a =>
            {
                if (a.Quantity != 0.00000000M)
                {
                    bnbBtcDepthCache.Bids.Add(a.Price, a.Quantity);
                }
            });

            // Store the last update from our result set;
            long lastUpdateId = depthResults.LastUpdateId;
            using (var binanceWebSocketClient = new DisposableBinanceWebSocketClient(client))
            {
                binanceWebSocketClient.ConnectToDepthWebSocket("BNBBTC", data =>
                {
                    if (lastUpdateId < data.UpdateId)
                    {
                        data.BidDepthDeltas.ForEach((bd) =>
                        {
                            CorrectlyUpdateDepthCache(bd, bnbBtcDepthCache.Bids);
                        });
                        data.AskDepthDeltas.ForEach((ad) =>
                        {
                            CorrectlyUpdateDepthCache(ad, bnbBtcDepthCache.Asks);
                        });
                    }
                    lastUpdateId = data.UpdateId;
                    System.Console.Clear();
                    System.Console.WriteLine($"{JsonConvert.SerializeObject(bnbBtcDepthCache, Formatting.Indented)}");
                    System.Console.SetWindowPosition(0, 0);
                });

                Thread.Sleep(8000);
            }
            return localDepthCache;
        }

        /// <summary>
        /// Advanced approach to building local Kline Cache from WebSocket and API Call example (refactored)
        /// </summary>
        /// <param name="binanceClient">The BinanceClient instance</param>
        /// <param name="symbol">The Symbol to request</param>
        /// <param name="interval">The interval for Klines</param>
        /// <param name="klinesCandlesticksRequest">The initial request for Klines</param>
        /// <param name="webSocketConnectionFunc">The function to determine exiting the websocket (can be timeout or Func based on external params)</param>
        /// <param name="cacheObject">The cache object. Must always be provided, and can exist with data.</param>
        /// <returns></returns>
        public static async Task BuildAndUpdateLocalKlineCache(IBinanceClient binanceClient,
            string symbol,
            KlineInterval interval,
            GetKlinesCandlesticksRequest klinesCandlesticksRequest,
            WebSocketConnectionFunc webSocketConnectionFunc,
            Dictionary<string, KlineCacheObject> cacheObject)
        {
            Guard.AgainstNullOrEmpty(symbol);
            Guard.AgainstNull(webSocketConnectionFunc);
            Guard.AgainstNull(klinesCandlesticksRequest);
            Guard.AgainstNull(cacheObject);

            long epochTicks = new DateTime(1970, 1, 1).Ticks;

            if (cacheObject.ContainsKey(symbol))
            {
                if (cacheObject[symbol].KlineInterDictionary.ContainsKey(interval))
                {
                    throw new Exception(
                        "Symbol and Interval pairing already provided, please use a different interval/symbol or pair.");
                }
                cacheObject[symbol].KlineInterDictionary.Add(interval, new KlineIntervalCacheObject());
            }
            else
            {
                var klineCacheObject = new KlineCacheObject
                {
                    KlineInterDictionary = new Dictionary<KlineInterval, KlineIntervalCacheObject>()
                };
                cacheObject.Add(symbol, klineCacheObject);
                cacheObject[symbol].KlineInterDictionary.Add(interval, new KlineIntervalCacheObject());
            }
            
            // Get Kline Results, and use Cache
            var startTimeKeyTime = (klinesCandlesticksRequest.StartTime.Ticks - epochTicks) / TimeSpan.TicksPerSecond;
            var klineResults = await binanceClient.GetKlinesCandlesticks(klinesCandlesticksRequest);

            var oneMinKlineCache = cacheObject[symbol].KlineInterDictionary[interval];
            oneMinKlineCache.TimeKlineDictionary = new Dictionary<long, KlineCandleStick>();
            var instanceKlineCache = oneMinKlineCache.TimeKlineDictionary;
            //Populate our kline cache with initial results
            klineResults.ForEach(k =>
            {
                instanceKlineCache.Add(((k.OpenTime.Ticks - epochTicks) / TimeSpan.TicksPerSecond), new KlineCandleStick()
                {
                    Close = k.Close,
                    High = k.High,
                    Low = k.Low,
                    Open = k.Open,
                    Volume = k.Volume,
                });
            });

            // Store the last update from our result set;
            using (var binanceWebSocketClient = new DisposableBinanceWebSocketClient(binanceClient))
            {
                binanceWebSocketClient.ConnectToKlineWebSocket(symbol, interval, data =>
                {
                    var keyTime = (data.Kline.StartTime.Ticks - epochTicks) / TimeSpan.TicksPerSecond;
                    var klineObj = new KlineCandleStick()
                    {
                        Close = data.Kline.Close,
                        High = data.Kline.High,
                        Low = data.Kline.Low,
                        Open = data.Kline.Open,
                        Volume = data.Kline.Volume,
                    };
                    if (!data.Kline.IsBarFinal)
                    {
                        if (keyTime < startTimeKeyTime)
                        {
                            return;
                        }

                        TryAddUpdateKlineCache(instanceKlineCache, keyTime, klineObj);
                    }
                    else
                    {
                        TryAddUpdateKlineCache(instanceKlineCache, keyTime, klineObj);
                    }
                    System.Console.Clear();
                    System.Console.WriteLine($"{JsonConvert.SerializeObject(instanceKlineCache, Formatting.Indented)}");
                    System.Console.SetWindowPosition(0, 0);
                });
                if (webSocketConnectionFunc.IsTimout)
                {
                    Thread.Sleep(webSocketConnectionFunc.Timeout);
                }
                else
                {
                    while (true)
                    {
                        if (!webSocketConnectionFunc.ExitFunction())
                        {
                            // Throttle Application
                            Thread.Sleep(100);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }


        private static void TryAddUpdateKlineCache(Dictionary<long, KlineCandleStick> primary, long keyTime, KlineCandleStick klineObj)
        {
            if (primary.ContainsKey(keyTime))
            {
                primary[keyTime] = klineObj;
            }
            else
            {
                primary.Add(keyTime, klineObj);
            }
        }

        private static void CorrectlyUpdateDepthCache(TradeResponse bd,  Dictionary<decimal, decimal> depthCache)
        {
            const decimal defaultIgnoreValue = 0.00000000M;

            if (depthCache.ContainsKey(bd.Price))
            {
                if (bd.Quantity == defaultIgnoreValue)
                {
                    depthCache.Remove(bd.Price);
                }
                else
                {
                    depthCache[bd.Price] = bd.Quantity;
                }
            }
            else
            {
                if (bd.Quantity != defaultIgnoreValue)
                {
                    depthCache[bd.Price] = bd.Quantity;
                }
            }
        }
    }
}
