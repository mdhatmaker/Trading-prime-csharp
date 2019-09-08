using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrakenClient;
using System.Collections;
using System.Globalization;
using System.Threading;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using System.Diagnostics;


namespace KrakenClientConsole
{
    public class Program
    {

        public static  KrakenClient.KrakenClient client = new KrakenClient.KrakenClient();
        public static Broker broker = new Broker();

        // Get DateTime from Unix timestamp
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        // Get Unix timestamp from DateTime
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }

        public static long Since(DateTime dt)
        {
            return (long)DateTimeToUnixTimestamp(dt);
        }

        public static long BackHours(int hours=1, int minutes=0)
        {
            var dt = DateTime.Now - new TimeSpan(hours, minutes, 0);
            return (long)DateTimeToUnixTimestamp(dt);
        }

        public static void print(string text)
        {
            Console.WriteLine(text);
        }

        // Parse a KrakenApi-returned JSON object where the "result" is a dictionary of other dictionaries
        public static IDictionary GetDictionary(JsonObject json)
        {
            //var names = json.Names;
            IList error = json["error"] as IList;
            print("Error: " + error);
            var result = json["result"] as IDictionary;
            print("Result: ");
            foreach (var key in result.Keys)
            {
                var dict = result[key] as IDictionary;
                print(dict);
            }
            return null;
        }

        public static void print(IDictionary dict)
        {
            foreach (var k in dict.Keys)
            {
                Console.WriteLine("  {0} = {1}", k, dict[k]);
            }
            Console.WriteLine();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("calling kraken api...\n\n");

            //var time = client.GetServerTime();
            //var assets = client.GetActiveAssets();
            //var assetPairs = client.GetAssetPairs(new List<string> { "XXBTZEUR" });

            var ticker = client.GetTicker(new List<string> { "XXBTZEUR" });
            print("ticker: " + ticker.ToString() + "\n\n");
            GetDictionary(ticker);

            var tinfos = TickerInfo.Create(ticker);

            var depth = client.GetOrderBook("XXBTZUSD", 1);
            /*var trades = client.GetRecentTrades("XXBTZEUR", BackHours(2));
            var spreads = client.GetRecentSpreadData("XXBTZEUR", BackHours(5));
            var balance = client.GetBalance();
            var tradeBalance = client.GetTradeBalance("currency", string.Empty);
            var openOrders = client.GetOpenOrders();
            var closedOrders = client.GetClosedOrders();*/

            var openPositions = client.GetOpenPositions();
            var ledgers = client.GetLedgers();

            #region Simple requests

            //var time = client.GetServerTime();
            //Console.WriteLine("time: " + time.ToString() + "\n\n");

            //var assets = client.GetActiveAssets();
            //Console.WriteLine("assets: " + assets.ToString() + "\n\n");

            //var assetPairs = client.GetAssetPairs(new List<string> { "XXBTZEUR" });
            //Console.WriteLine("asset pairs: " + assetPairs.ToString() + "\n\n");

            //var ticker = client.GetTicker(new List<string> { "XXBTZEUR" });
            //Console.WriteLine("ticker: " + ticker.ToString() + "\n\n");

            //var depth = client.GetOrderBook("XXBTZUSD", 1);
            //Console.WriteLine("depth: " + depth.ToString() + "\n\n");

            //var trades = client.GetRecentTrades("XXBTZEUR", 137589964200000000);
            //Console.WriteLine("trades: " + trades.ToString() + "\n\n");

            //var spreads = client.GetRecentSpreadData("XXBTZEUR", 137589964200000000);
            //Console.WriteLine("spreads: " + spreads.ToString() + "\n\n");

            //var balance = client.GetBalance();
            //Console.WriteLine("balance: " + balance.ToString() + "\n\n");

            //var tradeBalance = client.GetTradeBalance("currency", string.Empty);
            //Console.WriteLine("trade balance: " + tradeBalance.ToString() + "\n\n");

            //var openOrders = client.GetOpenOrders();
            //Console.WriteLine("open orders: " + openOrders.ToString() + "\n\n");

            //var closedOrders = client.GetClosedOrders();
            //Console.WriteLine("closed orders: " + closedOrders.ToString() + "\n\n");

            //var queryOrders = client.QueryOrders(string.Empty);
            //Console.WriteLine("query orders: " + queryOrders.ToString() + "\n\n");

            //var tradesHistory = client.GetTradesHistory(string.Empty);
            //Console.WriteLine("trades history: " + tradesHistory.ToString() + "\n\n");

            //var queryTrades = client.QueryTrades();
            //Console.WriteLine("query trades: " + queryTrades.ToString() + "\n\n");

            //var openPositions = client.GetOpenPositions();
            //Console.WriteLine("open positions: " + openPositions.ToString() + "\n\n");

            //var ledgers = client.GetLedgers();
            //Console.WriteLine("ledgers: " + ledgers.ToString() + "\n\n");

            //var queryLedgers = client.QueryLedgers();
            //Console.WriteLine("query ledgers: " + queryLedgers.ToString() + "\n\n");

            //var tradeVolume = client.GetTradeVolume();
            //Console.WriteLine("trade volume: " + tradeVolume.ToString() + "\n\n");
            
            #endregion
           
            #region Simple trading requests

            //var closeDictionary = new Dictionary<string,string>();
            //closeDictionary.Add("ordertype","stop-loss-profit");
            //closeDictionary.Add("price","#5%");
            //closeDictionary.Add("price2","#10");

            //var addOrderRes = client.AddOrder("XXBTZEUR",
            //    "buy",
            //    "limit",
            //    (decimal)2.12345678,
            //    (decimal)101.9901,
            //    null,
            //    @"1:1",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    true,                
            //    closeDictionary);

            //Console.WriteLine("add order result: " + addOrderRes.ToString());

            //var cancelOrder = client.CancelOrder("");
            //Console.WriteLine("cancel order : " + cancelOrder.ToString());
            
            #endregion

            #region Using the broker helper

            //KrakenOrder openingOrder = broker.CreateOpeningOrder2(OrderType.buy, KrakenOrderType.stop_loss, 420.1M, 10,415M,viqc:true,validateOnly: false);
            //PlaceOrder(ref openingOrder, true);
            //CancelOrder(ref openingOrder);

            /*Stopwatch stopwatch = new Stopwatch();
            KrakenOrder order = new KrakenOrder();
            order.TxId = "OYNRKT-RQB5J-OM4DQU";
            for (int i = 1; i <= 10; i++)
            {

                stopwatch.Start();
                var res = broker.RefreshOrder(ref order);
                stopwatch.Stop();
                Console.WriteLine(stopwatch.Elapsed.ToString());
                stopwatch.Start();
            }*/
            #endregion    

            Console.ReadKey();
        }



        public static void PlaceOrder(ref KrakenOrder order, bool wait)
        {
            try
            {

                Console.WriteLine("Placing order...");

                var placeOrderResult = broker.PlaceOrder(ref order,wait);

                switch (placeOrderResult.ResultType)
                {
                    case PlaceOrderResultType.error:
                        Console.WriteLine("An error occured while placing the order");
                        foreach (var item in placeOrderResult.Errors)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    case PlaceOrderResultType.success:
                        Console.WriteLine(string.Format("Succesfully placed order {0}", order.TxId));
                        break;
                    case PlaceOrderResultType.partial:
                        Console.WriteLine(string.Format("Partially filled order {0}. {1} of {2}", order.TxId, order.VolumeExecuted, order.Volume));
                        break;
                    case PlaceOrderResultType.txid_null:
                        Console.WriteLine(string.Format("Order was not placed. Unknown reason"));
                        break;
                    case PlaceOrderResultType.canceled_not_partial:
                        Console.WriteLine(string.Format("The order was cancelled. Reason: {0}", order.Reason));
                        break;
                    case PlaceOrderResultType.exception:
                        Console.WriteLine(string.Format("Something went wrong. {0}", placeOrderResult.Exception.Message));
                        break;
                    default:
                        Console.WriteLine(string.Format("unknown PlaceOrderResultType {0}", placeOrderResult.ResultType));
                        break;

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong " + ex.Message);
                throw;
            }
        }

        public static void ClosePositionAndWaitForConfirmation(ref KrakenOrder openingOrder, decimal limitPrice)
        {

            try
            {
                Console.WriteLine("Closing position...");

                var closingOrder = broker.CreateClosingOrder(openingOrder, limitPrice, false);

                var closePositionResult = broker.PlaceOrder(ref closingOrder,true);

                switch (closePositionResult.ResultType)
                {
                    case PlaceOrderResultType.error:
                        Console.WriteLine("An error occured while placing the order");
                        foreach (var item in closePositionResult.Errors)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    case PlaceOrderResultType.success:
                        Console.WriteLine(string.Format("Succesfully placed order {0}", closingOrder.TxId));
                        break;
                    case PlaceOrderResultType.partial:
                        Console.WriteLine(string.Format("Partially filled order {0}. {1} of {2}", closingOrder.TxId, closingOrder.VolumeExecuted, closingOrder.Volume));
                        break;
                    case PlaceOrderResultType.txid_null:
                        Console.WriteLine(string.Format("Order was not placed. Unknown reason"));
                        break;
                    case PlaceOrderResultType.canceled_not_partial:
                        Console.WriteLine(string.Format("The order was canceled. Reason: {0}", closingOrder.Reason));
                        break;
                    case PlaceOrderResultType.exception:
                        Console.WriteLine(string.Format("Something went wrong. {0}", closingOrder.Reason));
                        break;
                    default:
                        Console.WriteLine(string.Format("unknown PlaceOrderResultType {0}", closePositionResult.ResultType));
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Something went wrong. {0}", ex.Message));
                throw;
            }
        }

        public static void CancelOrder(ref KrakenOrder order)
        {
            try
            {

                Console.WriteLine("Cancelling order...");

                var cancelOrderResult = broker.CancelOrder(ref order);

                switch (cancelOrderResult.ResultType)
                {
                    case CancelOrderResultType.error:
                        Console.WriteLine("An error occured while cancelling the order");
                        foreach (var item in cancelOrderResult.Errors)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    case CancelOrderResultType.success:
                        Console.WriteLine(string.Format("Succesfully cancelled order {0}", order.TxId));
                        break;
                    case CancelOrderResultType.exception:
                        Console.WriteLine(string.Format("Something went wrong. {0}", order.Reason));
                        break;
                    default:
                        Console.WriteLine(string.Format("unknown CancelOrderResultType {0}", cancelOrderResult.ResultType));
                        break;

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong " + ex.Message);
                throw;
            }
        }
    }
}
