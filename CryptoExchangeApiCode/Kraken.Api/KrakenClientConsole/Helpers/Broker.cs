using Jayrock.Json;
using KrakenClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KrakenClientConsole
{
    public class Broker
    {

        KrakenClient.KrakenClient client;

        public Broker()
        {
            client = new KrakenClient.KrakenClient();
        }

        public KrakenOrder CreateOpeningOrder(OrderType type, KrakenOrderType orderType, decimal enteringPrice, decimal volume, string pair = "XXBTZEUR", bool viqc = false, bool validateOnly = false)
        {
            KrakenOrder order = new KrakenOrder();

            order.Pair = pair;
            order.Type = type.ToString();
            order.OrderType = orderType.ToString().Replace("_", "-");
            order.Price = enteringPrice;
            order.Volume = volume;
            if (viqc)
                order.OFlags = OFlag.viqc.ToString();
            order.Validate = validateOnly;

            return order;
        }

        public KrakenOrder CreateClosingOrder(KrakenOrder orderToClose, decimal limitPrice, bool validateOnly = false)
        {
            KrakenOrder order = new KrakenOrder();
            
            order.Pair = orderToClose.Pair;
            order.Type = (orderToClose.Type == "buy") ? "sell" : "buy";
            order.OrderType = (KrakenOrderType.stop_loss).ToString().Replace("_", "-");
            order.Price = limitPrice;
            order.Volume = orderToClose.Volume;
            order.OFlags = orderToClose.OFlags;
            order.Validate = validateOnly;

            return order;
            
        }

        public KrakenOrder CreateOpeningOrder2(OrderType type, KrakenOrderType orderType, decimal enteringPrice, decimal volume, decimal stopPrice, string pair = "XXBTZEUR", bool viqc = false, bool validateOnly = false)
        {
            KrakenOrder order = new KrakenOrder();

            order.Pair = pair;
            order.Type = type.ToString();
            order.OrderType = orderType.ToString().Replace("_", "-");
            order.Price = enteringPrice;
            order.Volume = volume;
            if (viqc)
                order.OFlags = OFlag.viqc.ToString();
            order.Validate = validateOnly;
            var closeDictionary = new Dictionary<string,string>();
            closeDictionary.Add("ordertype","stop-loss");
            closeDictionary.Add("price",stopPrice.ToString());
            closeDictionary.Add("price2", null);
            order.Close = closeDictionary;
          
            return order;
        }

        /// <summary>
        /// Submit an order to Kraken. The order passed by reference will be updated with info set by Kraken.
        /// </summary>
        /// <param name="order">Order to submit.</param>
        /// <param name="wait">If set to true, the function will wait until the order is closed or canceled.</param>
        /// <returns>PlaceOrderResult containing info about eventual success or failure of the request</returns>
        public PlaceOrderResult PlaceOrder(ref KrakenOrder order,bool wait)
        {
            PlaceOrderResult placeOrderResult = new PlaceOrderResult();

            try
            {
                JsonObject res = client.AddOrder(order);

                JsonArray error = (JsonArray)res["error"];
                if (error.Count() > 0)
                {
                    placeOrderResult.ResultType = PlaceOrderResultType.error;
                    List<string> errorList = new List<string>();
                    foreach (var item in error)
                    {
                        errorList.Add(item.ToString());
                    }
                    placeOrderResult.Errors = errorList;
                    return placeOrderResult;
                }
                else
                {
                    JsonObject result = (JsonObject)res["result"];
                    JsonObject descr = (JsonObject)result["descr"];
                    JsonArray txid = (JsonArray)result["txid"];

                    if (txid == null)
                    {
                        placeOrderResult.ResultType = PlaceOrderResultType.txid_null;
                        return placeOrderResult;
                    }
                    else
                    {
                        string transactionIds = "";

                        foreach (var item in txid)
                        {
                            transactionIds += item.ToString() + ",";
                        }
                        transactionIds = transactionIds.TrimEnd(',');

                        order.TxId = transactionIds;

                        if (wait)
                        {
                            #region Repeatedly check order status by calling RefreshOrder
                            bool keepSpinning = true;
                            while (keepSpinning)
                            {
                                RefreshOrderResult refreshOrderResult = RefreshOrder(ref order);
                                switch (refreshOrderResult.ResultType)
                                {
                                    case RefreshOrderResultType.success:
                                        switch (order.Status)
                                        {
                                            case "closed":
                                                placeOrderResult.ResultType = PlaceOrderResultType.success;
                                                return placeOrderResult;
                                            case "pending":
                                                break;
                                            case "open":
                                                break;
                                            case "canceled":
                                                if (order.VolumeExecuted > 0)
                                                {
                                                    placeOrderResult.ResultType = PlaceOrderResultType.partial;
                                                    return placeOrderResult;
                                                }
                                                else
                                                {
                                                    placeOrderResult.ResultType = PlaceOrderResultType.canceled_not_partial;
                                                    return placeOrderResult;
                                                }
                                            default:
                                                throw new Exception(string.Format("Unknown type of order status: {0}", order.Status));
                                        }
                                        break;
                                    case RefreshOrderResultType.error:
                                        throw new Exception(string.Format("An error occured while trying to refresh the order.\nError List: {0}", refreshOrderResult.Errors.ToString()));

                                    case RefreshOrderResultType.order_not_found:
                                        throw new Exception("An error occured while trying to refresh the order.\nOrder not found");

                                    case RefreshOrderResultType.exception:
                                        throw new Exception("An unexpected exception occured while trying to refresh the order.", refreshOrderResult.Exception);

                                    default:
                                        keepSpinning = false;
                                        break;
                                }
                                Thread.Sleep(5000);
                            }
                            #endregion
                        }

                        placeOrderResult.ResultType = PlaceOrderResultType.success;
                        return placeOrderResult;
                    }
                }
            }
            catch (Exception ex)
            {
                placeOrderResult.ResultType = PlaceOrderResultType.exception;
                placeOrderResult.Exception = ex;
                return placeOrderResult;
            }

        }

        /// <summary>
        /// Cancels the order
        /// </summary>
        /// <param name="order">Order to cancel</param>
        /// <returns>CancelOrderResult containing info about eventual success or failure of the request</returns>
        public CancelOrderResult CancelOrder(ref KrakenOrder order)
        {
            CancelOrderResult cancelOrderResult = new CancelOrderResult();
            try
            {
                JsonObject res = client.CancelOrder(order.TxId);
                JsonArray error = (JsonArray)res["error"];
                if (error.Count() > 0)
                {
                    cancelOrderResult.ResultType = CancelOrderResultType.error;
                    List<string> errorList = new List<string>();
                    foreach (var item in error)
                    {
                        errorList.Add(item.ToString());
                    }
                    cancelOrderResult.Errors = errorList;
                    return cancelOrderResult;
                }
                else
                {
                    JsonObject result = (JsonObject)res["result"];
                    var count = int.Parse(result["count"].ToString());
                    var pending = (string)result["pending"];

                    RefreshOrder(ref order);

                    cancelOrderResult.ResultType = CancelOrderResultType.success;
                    cancelOrderResult.OrdersCanceled = count;
                    cancelOrderResult.OrdersPending = pending;

                    return cancelOrderResult;
                }
            }
            catch (Exception ex)
            {
                cancelOrderResult.ResultType = CancelOrderResultType.exception;
                cancelOrderResult.Exception = ex;
                return cancelOrderResult;
            }
        }

        /// <summary>
        /// Call Kraken to update info about order execution.
        /// </summary>
        /// <param name="order">Order to update</param>
        /// <returns>RefreshOrderResult containing info about eventual success or failure of the request</returns>
        public RefreshOrderResult RefreshOrder(ref KrakenOrder order)
        {
            RefreshOrderResult refreshOrderResult = new RefreshOrderResult();
            
            try
            {
                JsonObject res = client.QueryOrders(order.TxId);

                JsonArray error = (JsonArray)res["error"];
                if (error.Count() > 0)
                {
                    refreshOrderResult.ResultType = RefreshOrderResultType.error;
                    List<string> errorList = new List<string>();
                    foreach (var item in error)
                    {
                        errorList.Add(item.ToString());
                    }
                    refreshOrderResult.Errors = errorList;
                    return refreshOrderResult;
                }
                else
                {
                    JsonObject result = (JsonObject)res["result"];
                    JsonObject orderDetails = (JsonObject)result[order.TxId];

                    if (orderDetails == null)
                    {
                        refreshOrderResult.ResultType = RefreshOrderResultType.order_not_found;
                        return refreshOrderResult;
                    }
                    else
                    {
                        string status = (orderDetails["status"] != null) ? orderDetails["status"].ToString() : null;
                        string reason = (orderDetails["reason"] != null) ? orderDetails["reason"].ToString() : null;
                        string openTime = (orderDetails["opentm"] != null) ? orderDetails["opentm"].ToString() : null;
                        string closeTime = (orderDetails["closetm"] != null) ? orderDetails["closetm"].ToString() : null;
                        string vol_exec = (orderDetails["vol_exec"] != null) ? orderDetails["vol_exec"].ToString() : null;
                        string cost = (orderDetails["cost"] != null) ? orderDetails["cost"].ToString() : null;
                        string fee = (orderDetails["fee"] != null) ? orderDetails["fee"].ToString() : null;
                        string price = (orderDetails["price"] != null) ? orderDetails["price"].ToString() : null;
                        string misc = (orderDetails["misc"] != null) ? orderDetails["misc"].ToString() : null;
                        string oflags = (orderDetails["oflags"] != null) ? orderDetails["oflags"].ToString() : null;
                        JsonArray tradesArray = (JsonArray)orderDetails["trades"];
                        string trades = null;
                        if (tradesArray != null)
                        {

                            foreach (var item in tradesArray)
                            {
                                trades += item.ToString() + ",";
                            }
                            trades = trades.TrimEnd(',');
                        }

                        order.Status = status;
                        order.Reason = reason;
                        order.OpenTime = openTime;
                        order.CloseTime = closeTime;
                        order.VolumeExecuted = double.Parse(vol_exec);
                        order.Cost = decimal.Parse(cost);
                        order.Fee = decimal.Parse(fee);
                        order.AveragePrice = decimal.Parse(price);
                        order.Info = misc;
                        order.OFlags = oflags;
                        order.Trades = trades;

                        refreshOrderResult.ResultType = RefreshOrderResultType.success;
                        return refreshOrderResult;
                    }
                }
            }
            catch (Exception ex)
            {
                refreshOrderResult.ResultType = RefreshOrderResultType.exception;
                refreshOrderResult.Exception = ex;
                return refreshOrderResult;
            }
        }

        public GetOrderResult GetOpenOrders()
        {
            GetOrderResult getOrderResult = new GetOrderResult();

            try
            {
                JsonObject res = client.GetOpenOrders();

                JsonArray error = (JsonArray)res["error"];
                if (error.Count() > 0)
                {
                    getOrderResult.ResultType = GetOrderResultType.error;
                    List<string> errorList = new List<string>();
                    foreach (var item in error)
                    {
                        errorList.Add(item.ToString());
                    }
                    getOrderResult.Errors = errorList;
                    return getOrderResult;
                }
                else
                {
                    JsonObject result = (JsonObject)res["result"];
                    JsonObject openOrders = (JsonObject)result["open"];
                    var orderIds = openOrders.Names;
                    List<KrakenOrder> orderList = new List<KrakenOrder>();

                    foreach (var id in orderIds)
                    {

                        JsonObject orderDetails = (JsonObject)openOrders[id.ToString()];

                        if (orderDetails == null)
                        {
                            getOrderResult.ResultType = GetOrderResultType.error;
                            return getOrderResult;
                        }
                        else
                        {
                            //string pair =  orderDetails["pair"].ToString();
                            string txid = id.ToString();
                            JsonObject descr = (JsonObject)orderDetails["descr"];
                            string pair = descr["pair"].ToString();
                            string type = descr["type"].ToString();
                            string ordertype = descr["ordertype"].ToString();
                            string price = descr["price"].ToString();
                            string price2 = descr["price2"].ToString();
                            string leverage = descr["leverage"].ToString();

                            string status = (orderDetails["status"] != null) ? orderDetails["status"].ToString() : null;
                            string reason = (orderDetails["reason"] != null) ? orderDetails["reason"].ToString() : null;
                            string openTime = (orderDetails["opentm"] != null) ? orderDetails["opentm"].ToString() : null;
                            string closeTime = (orderDetails["closetm"] != null) ? orderDetails["closetm"].ToString() : null;
                            string vol_exec = (orderDetails["vol_exec"] != null) ? orderDetails["vol_exec"].ToString() : null;
                            string cost = (orderDetails["cost"] != null) ? orderDetails["cost"].ToString() : null;
                            string fee = (orderDetails["fee"] != null) ? orderDetails["fee"].ToString() : null;
                            string averagePrice = (orderDetails["price"] != null) ? orderDetails["price"].ToString() : null;
                            string misc = (orderDetails["misc"] != null) ? orderDetails["misc"].ToString() : null;
                            string oflags = (orderDetails["oflags"] != null) ? orderDetails["oflags"].ToString() : null;
                            JsonArray tradesArray = (JsonArray)orderDetails["trades"];
                            string trades = null;
                            if (tradesArray != null)
                            {

                                foreach (var trade in tradesArray)
                                {
                                    trades += trade.ToString() + ",";
                                }
                                trades = trades.TrimEnd(',');
                            }

                            KrakenOrder order = new KrakenOrder();

                            order.Status = status;
                            order.Reason = reason;
                            order.OpenTime = openTime;
                            order.CloseTime = closeTime;
                            order.VolumeExecuted = double.Parse(vol_exec);
                            order.Cost = decimal.Parse(cost);
                            order.Fee = decimal.Parse(fee);
                            order.AveragePrice = decimal.Parse(averagePrice);
                            order.Info = misc;
                            order.OFlags = oflags;
                            order.Trades = trades;

                            orderList.Add(order);
                        }
                    }
                    getOrderResult.Order = orderList.FirstOrDefault(); ;
                    getOrderResult.ResultType = GetOrderResultType.success;
                    return getOrderResult;
                }
            }
            catch (Exception ex)
            {
                getOrderResult.ResultType = GetOrderResultType.exception;
                getOrderResult.Exception = ex;
                return getOrderResult;
            }
        }

    }

    

}
