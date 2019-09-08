using Newtonsoft.Json;
using RestSharp;
using Rokolab.BitstampClient.Logging;
using Rokolab.BitstampClient.Models;
using System;
using System.Collections.Generic;

namespace Rokolab.BitstampClient
{
    public class BitstampClient : IBitstampClient
    {
        private readonly IRequestAuthenticator _requestAuthenticator;
        private const string _tickerRoute = "https://www.bitstamp.net/api/ticker/";
        private const string _balanceRoute = "https://www.bitstamp.net/api/v2/balance/";
        private const string _transactionsRoute = "https://www.bitstamp.net/api/v2/user_transactions/btcusd/";
        private const string _cancelAllOrdersRoute = "https://www.bitstamp.net/api/cancel_all_orders/";
        private const string _cancelOrderRoute = "https://www.bitstamp.net/api/v2/cancel_order/";
        private const string _openOrdersRoute = "https://www.bitstamp.net/api/v2/open_orders/btcusd/";
        private const string _orderStatusRoute = "https://www.bitstamp.net/api/order_status/";
        private const string _buyRoute = "https://www.bitstamp.net/api/v2/buy/btcusd/";
        private const string _sellRoute = "	https://www.bitstamp.net/api/v2/sell/btcusd/";
        private object _lock = new object();
        private DateTime _lastApiCallTimestamp;
        private readonly ILog _log;

        public BitstampClient(IRequestAuthenticator requestAuthenticator, ILogFactory logFactory)
        {
            _log = logFactory.CreateLog(nameof(BitstampClient));
            _requestAuthenticator = requestAuthenticator;
            _lastApiCallTimestamp = DateTime.Now;
        }

        public TickerResponse GetTicker()
        {
            lock (_lock)
            {
                try
                {
                    var request = GetAuthenticatedRequest(Method.POST);
                    var response = new RestClient(_tickerRoute).Execute(request);
                    _lastApiCallTimestamp = DateTime.Now;
                    return JsonConvert.DeserializeObject<TickerResponse>(response.Content);
                }
                catch (Exception ex)
                {
                    _log.TraceException("BitstampClient@GetTicker", ex);
                    return null;
                }
            }
        }

        public BalanceResponse GetBalance()
        {
            lock (_lock)
            {
                try
                {
                    var request = GetAuthenticatedRequest(Method.POST);
                    var response = new RestClient(_balanceRoute).Execute(request);
                    _lastApiCallTimestamp = DateTime.Now;
                    return JsonConvert.DeserializeObject<BalanceResponse>(response.Content);
                }
                catch (Exception ex)
                {
                    _log.TraceException("BitstampClient@GetBalance", ex);
                    return null;
                }
            }
        }

        public OrderStatusResponse GetOrderStatus(string orderId)
        {
            lock (_lock)
            {
                try
                {
                    var request = GetAuthenticatedRequest(Method.POST);
                    request.AddParameter("id", orderId);

                    var response = new RestClient(_orderStatusRoute).Execute(request);
                    _lastApiCallTimestamp = DateTime.Now;

                    return JsonConvert.DeserializeObject<OrderStatusResponse>(response.Content);
                }
                catch (Exception ex)
                {
                    _log.TraceException("BitstampClient@GetOrderStatus", ex);
                    return null;
                }
            }
        }

        public List<OpenOrderResponse> GetOpenOrders()
        {
            lock (_lock)
            {
                try
                {
                    var request = GetAuthenticatedRequest(Method.POST);
                    var response = new RestClient(_openOrdersRoute).Execute(request);
                    _lastApiCallTimestamp = DateTime.Now;
                    return JsonConvert.DeserializeObject<List<OpenOrderResponse>>(response.Content);
                }
                catch (Exception ex)
                {
                    _log.TraceException("BitstampClient@GetOpenOrders", ex);
                    return null;
                }
            }
        }

        public List<TransactionResponse> GetTransactions(int offset, int limit)
        {
            lock (_lock)
            {
                try
                {
                    var request = GetAuthenticatedRequest(Method.POST);
                    request.AddParameter("offset", offset);
                    request.AddParameter("limit", limit);

                    var response = new RestClient(_transactionsRoute).Execute(request);
                    _lastApiCallTimestamp = DateTime.Now;
                    return JsonConvert.DeserializeObject<List<TransactionResponse>>(response.Content);
                }
                catch (Exception ex)
                {
                    _log.TraceException("BitstampClient@GetTransactions", ex);
                    return null;
                }
            }
        }

        public bool CancelOrder(string orderId)
        {
            lock (_lock)
            {
                try
                {
                    var request = GetAuthenticatedRequest(Method.POST);
                    request.AddParameter("id", orderId);

                    var response = new RestClient(_cancelOrderRoute).Execute(request);
                    _lastApiCallTimestamp = DateTime.Now;

                    return (response.Content == "true") ? true : false;
                }
                catch (Exception ex)
                {
                    _log.TraceException("BitstampClient@CancelOrder", ex);
                    return false;
                }
            }
        }

        public bool CancelAllOrders()
        {
            lock (_lock)
            {
                try
                {
                    var request = GetAuthenticatedRequest(Method.POST);

                    var response = new RestClient(_cancelAllOrdersRoute).Execute(request);
                    _lastApiCallTimestamp = DateTime.Now;

                    return (response.Content == "true") ? true : false;
                }
                catch (Exception ex)
                {
                    _log.TraceException("BitstampClient@CancelAllOrders", ex);
                    return false;
                }
            }
        }

        public BuySellResponse Buy(double amount, double price)
        {
            lock (_lock)
            {
                try
                {
                    var request = GetAuthenticatedRequest(Method.POST);
                    request.AddParameter("amount", amount.ToString().Replace(",", "."));
                    request.AddParameter("price", price.ToString().Replace(",", "."));

                    var response = new RestClient(_buyRoute).Execute(request);
                    _lastApiCallTimestamp = DateTime.Now;
                    return JsonConvert.DeserializeObject<BuySellResponse>(response.Content);
                }
                catch (Exception ex)
                {
                    _log.TraceException("BitstampClient@Buy", ex);
                    return null;
                }
            }
        }

        public BuySellResponse Sell(double amount, double price)
        {
            lock (_lock)
            {
                try
                {
                    var request = GetAuthenticatedRequest(Method.POST);
                    request.AddParameter("amount", amount.ToString().Replace(",", "."));
                    request.AddParameter("price", price.ToString().Replace(",", "."));

                    var response = new RestClient(_sellRoute).Execute(request);
                    _lastApiCallTimestamp = DateTime.Now;
                    return JsonConvert.DeserializeObject<BuySellResponse>(response.Content);
                }
                catch (Exception ex)
                {
                    _log.TraceException("BitstampClient@Sell", ex);
                    return null;
                }
            }
        }

        private RestRequest GetAuthenticatedRequest(Method method)
        {
            var request = new RestRequest(method);
            _requestAuthenticator.Authenticate(request);
            return request;
        }
    }
}