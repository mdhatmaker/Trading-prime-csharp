using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace CryptoAPIs.Exchange.Clients.HitBTC
{
    public class HitBtcApi
    {
        private const string url = "http://api.hitbtc.com";

        public MarketData MarketData { get; set; }
        public Payment Payment { get; set; }
        public Trading Trading { get; set; }

        public HitBtcApi()
        {
            MarketData = new MarketData(this);
            Payment = new Payment(this);
            Trading = new Trading(this);
        }

        /// <summary>
        /// Method that allow to execute a request to api
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requireAuthentication"></param>
        /// <returns></returns>
        //public ApiResponse Execute(RestRequest request, bool requireAuthentication = true)
        //{
        //    if (requireAuthentication && !IsAuthorized)
        //        throw new Exception("AccessTokenInvalid");

        //    var client = new RestClient(url);

        //    if (requireAuthentication)
        //    {
        //        request.AddParameter("nonce", GetNonce());
        //        request.AddParameter("apikey", _apiKey);
        //        string sign = CalculateSignature(client.BuildUri(request).PathAndQuery, _secretKey);
        //        request.AddHeader("X-Signature", sign);
        //    }

        //    var response = client.Execute(request);

        //    if (response.ErrorException != null)
        //    {
        //        const string message = "Error retrieving response.  Check inner details for more info.";
        //        var exception = new ApplicationException(message, response.ErrorException);
        //        throw exception;
        //    }

        //    return new ApiResponse { content = response.Content };
        //}

        public async Task<ApiResponse> Execute(RestRequest request, bool requireAuthentication = true)
        {
            if (requireAuthentication && !IsAuthorized)
                throw new Exception("AccessTokenInvalid");

            var client = new RestClient(url);

            if (requireAuthentication)
            {
                request.AddParameter("nonce", GetNonce());
                request.AddParameter("apikey", _apiKey);
                string sign = CalculateSignature(client.BuildUri(request).PathAndQuery, _secretKey);
                request.AddHeader("X-Signature", sign);
            }

            var response = await client.GetResponseAsync(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            return new ApiResponse { content = response.Content };
        }

        #region Authentication

        /// <summary>
        /// Flag shows that user is authorized
        /// </summary>
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// Method for authorization 
        /// </summary>
        /// <param name="apiKey">API key from the Settings page.</param>
        /// <param name="secretKey">Secret key from the Settings page.</param>
        public void Authorize(string apiKey, string secretKey)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
            IsAuthorized = true;
        }

        private string _apiKey;
        private string _secretKey;

        private static long GetNonce()
        {
            // use millisecond timestamp or whatever you want
            return DateTime.Now.Ticks * 10 / TimeSpan.TicksPerMillisecond;
        }

        private static string CalculateSignature(string text, string secretKey)
        {
            using (var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
            {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(text));

                // minimalistic hex-encoding and lower case
                return string.Concat(hmacsha512.Hash.Select(b => b.ToString("x2")).ToArray());
            }
        }

        #endregion
    }

    public static class RestClientExtensions
    {
        private static Task<T> SelectAsync<T>(this RestClient client, IRestRequest request,
            Func<IRestResponse, T> selector)
        {
            var tcs = new TaskCompletionSource<T>();
            var loginResponse = client.ExecuteAsync(request, r =>
            {
                if (r.ErrorException == null)
                {
                    tcs.SetResult(selector(r));
                }
                else
                {
                    tcs.SetException(r.ErrorException);
                }
            });
            return tcs.Task;
        }

        public static Task<string> GetContentAsync(this RestClient client, IRestRequest request)
        {
            return client.SelectAsync(request, r => r.Content);
        }

        public static Task<IRestResponse> GetResponseAsync(this RestClient client, IRestRequest request)
        {
            return client.SelectAsync(request, r => r);
        }
    } // end of class HitBtcApi


    /// <summary>
    /// Market data RESTful API
    /// </summary>
    public class MarketData
    {
        private readonly HitBtcApi _api;

        public MarketData(HitBtcApi api)
        {
            _api = api;
        }

        /// <summary>
        /// returns the server time in UNIX timestamp format
        /// /api/1/public/time
        /// </summary>
        /// <returns></returns>
        public async Task<Timestamp> GetTimestamp()
        {
            return await _api.Execute(new RestRequest("/api/1/public/time"), false);
        }

        /// <summary>
        /// Simbols returns the actual list of currency symbols traded on HitBTC exchange with their characteristics
        /// /api/1/public/symbols
        /// </summary>
        /// <returns></returns>
        public async Task<Symbols> GetSymbols()
        {
            return await _api.Execute(new RestRequest("/api/1/public/symbols"), false);
        }

        /// <summary>
        /// returns the actual data on exchange rates of the specified cryptocurrency.
        /// /api/1/public/:symbol/ticker
        /// </summary>
        /// <param name="symbol">is a currency symbol traded on HitBTC exchange</param>
        /// <returns></returns>
        public async Task<Ticker> GetSymbolTicker(Symbol symbol)
        {
            var request = new RestRequest();
            request.Resource = "/api/1/public/{symbol}/ticker";
            request.AddParameter("symbol", symbol.symbol, ParameterType.UrlSegment);

            return await _api.Execute(request, false);
        }

        /// <summary>
        /// returns the actual data on exchange rates for all traded cryptocurrencies - all tickers.
        /// /api/1/public/ticker
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, Ticker>> GetPublickTickers()
        {
            return await _api.Execute(new RestRequest("/api/1/public/ticker"), false);
        }

        /// <summary>
        /// returns a list of open orders for specified currency symbol: their prices and sizes.
        /// /api/1/public/:symbol/orderbook
        /// </summary>
        /// <param name="symbol">is a currency symbol traded on HitBTC exchange </param>
        /// <returns></returns>
        public async Task<Orderbook> GetOrderbook(Symbol symbol)
        {
            var request = new RestRequest("api/1/public/{symbol}/orderbook");
            request.AddParameter("symbol", symbol.symbol, ParameterType.UrlSegment);
            return await _api.Execute(request, false);
        }

        /// <summary>
        /// returns recent trades for the specified currency symbol.
        /// /api/1/public/:symbol/trades/recent
        /// </summary>
        /// <param name="symbol">is a currency symbol traded on HitBTC exchange</param>
        /// <param name="max_results">Maximum quantity of returned results, at most 1000</param>
        /// <returns></returns>
        public async Task<SpecifiedTrades> GetRecentTrades(Symbol symbol, int max_results = 1000)
        {
            var request = new RestRequest("/api/1/public/{symbol}/trades/recent");
            request.Parameters.Add(new Parameter { Name = "symbol", Value = symbol.symbol, Type = ParameterType.UrlSegment });
            request.Parameters.Add(new Parameter { Name = "max_results", Value = max_results, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "format_item", Value = "object", Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "side", Value = "true", Type = ParameterType.GetOrPost });

            return await _api.Execute(request);
        }
    } // end of class MarketData

    /// <summary>
    /// Payment RESTful API
    /// </summary>
    public class Payment
    {
        HitBtcApi _api;

        public Payment(HitBtcApi api)
        {
            _api = api;
        }

        /// <summary>
        /// returns multi-currency balance of the main account.
        /// /api/1/payment/balance
        /// </summary>
        /// <returns></returns>
        public async Task<MultiCurrencyBalance> GetMultiCurrencyBalance()
        {
            return await _api.Execute(new RestRequest("/api/1/payment/balance"));
        }

        /// <summary>
        /// returns payment transaction and its status.
        /// /api/1/payment/transactions/:id
        /// </summary>
        /// <param name="id">Transaction Id, Required</param>
        /// <returns></returns>
        public async Task<Transaction> GetTransactions(string id)
        {
            var request = new RestRequest("/api/1/payment/transactions/{id}");
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return await _api.Execute(request);
        }

        /// <summary>
        /// returns a list of payment transactions and their statuses(array of transactions).
        /// /api/1/payment/transactions
        /// </summary>
        /// <param name="offset">Start index for the query, default = 0</param>
        /// <param name="limit">Maximum results for the query, Required</param>
        /// <param name="dir">Transactions are sorted ascending (ask) or descending (desc) (default)</param>
        /// <returns></returns>
        public async Task<TransactionList> GetTransactions(int limit = 1000, string dir = "desc", int offset = 0)
        {
            var request = new RestRequest("/api/1/payment/transactions");
            request.Parameters.Add(new Parameter { Name = "limit", Value = limit, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "dir", Value = dir, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "offset", Value = offset, Type = ParameterType.GetOrPost });
            return await _api.Execute(request);
        }

        /// <summary>
        /// withdraws money and creates an outgoing crypotocurrency transaction; 
        /// returns a transaction ID on the exchange or an error.
        /// /api/1/payment/payout
        /// </summary>
        /// <param name="amount">Funds amount to withdraw, Required</param>
        /// <param name="currency_code">Currency symbol, e.g.BTC, Required</param>
        /// <param name="address">BTC/LTC address to withdraw to, Required</param>
        /// <param name="id">payment id for cryptonote</param>
        /// <returns></returns>
        public async Task<Transaction> GetPyout(decimal amount, string currency_code, string address, string extra_id)
        {
            var request = new RestRequest("/api/1/payment/transactions", Method.POST);
            request.Parameters.Add(new Parameter { Name = "amount", Value = amount, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "currency_code", Value = currency_code, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "address", Value = address, Type = ParameterType.GetOrPost });
            if (!string.IsNullOrEmpty(extra_id))
                request.Parameters.Add(new Parameter { Name = "extra_id", Value = extra_id, Type = ParameterType.GetOrPost });

            return await _api.Execute(request);
        }

        /// <summary>
        /// returns the last created incoming cryptocurrency address that can be used to deposit cryptocurrency to your account. 
        /// /api/1/payment/address/ (GET)
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public async Task<Address> GetAddress(string currency)
        {
            var request = new RestRequest("/api/1/payment/address/{currency}");
            request.AddParameter("currency", currency, ParameterType.UrlSegment);
            return await _api.Execute(request);
        }

        /// <summary>
        /// returns the last created incoming cryptocurrency address that can be used to deposit cryptocurrency to your account. 
        /// /api/1/payment/address/ (GET)
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public async Task<Address> CreateAddress(string currency)
        {
            var request = new RestRequest("/api/1/payment/address/{currency}", Method.POST);
            request.AddParameter("currency", currency, ParameterType.UrlSegment);
            return await _api.Execute(request);
        }

        /// <summary>
        /// transfers funds between main and trading accounts; returns a transaction ID or an error.
        /// /api/1/payment/transfer_to_trading
        /// </summary>
        /// <param name="amount">Funds amount to transfer, Required</param>
        /// <param name="currency_code">Currency symbol, e.g. BTC, Required</param>
        /// <returns></returns>
        public async Task<Transaction> TransferToTrading(decimal amount, string currency_code)
        {
            var request = new RestRequest("/api/1/payment/transfer_to_trading", Method.POST);
            request.Parameters.Add(new Parameter { Name = "amount", Value = amount, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "currency_code", Value = currency_code, Type = ParameterType.GetOrPost });
            return await _api.Execute(request);
        }

        /// <summary>
        /// transfers funds between main and trading accounts; returns a transaction ID or an error.
        /// /api/1/payment/transfer_to_main
        /// </summary>
        /// <param name="amount">Funds amount to transfer, Required</param>
        /// <param name="currency_code">Currency symbol, e.g. BTC, Required</param>
        /// <returns></returns>
        public async Task<Transaction> TransferToMain(decimal amount, string currency_code)
        {
            var request = new RestRequest("/api/1/payment/transfer_to_main", Method.POST);
            request.Parameters.Add(new Parameter { Name = "amount", Value = amount, Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Name = "currency_code", Value = currency_code, Type = ParameterType.GetOrPost });
            return await _api.Execute(request);
        }
    } // end of class Payment

    /// <summary>
    /// Trading RESTful API
    /// </summary>
    public class Trading
    {
        private HitBtcApi _api;

        public Trading(HitBtcApi api)
        {
            _api = api;
        }

        /// <summary>
        /// returns trading balance.
        /// /api/1/trading/balance
        /// </summary>
        /// <returns></returns>
        public async Task<TradingBalanceList> GeTradingBalance()
        {
            return await _api.Execute(new RestRequest("/api/1/trading/balance"));
        }

        /// <summary>
        /// returns all orders in status new or partiallyFilled.
        /// /api/1/trading/orders/active
        /// </summary>
        /// <param name="symbols">Comma-separated list of symbols. Default - all symbols</param>
        /// <param name="clientOrderId">Unique order ID</param>
        /// <returns></returns>
        public async Task<Orders> GetActiveOrders(string symbols = null, string clientOrderId = null)
        {
            var request = new RestRequest("/api/1/trading/orders/active");

            if (!string.IsNullOrEmpty(symbols))
                request.Parameters.Add(new Parameter { Value = symbols, Name = "symbols", Type = ParameterType.GetOrPost });
            if (!string.IsNullOrEmpty(clientOrderId))
                request.Parameters.Add(new Parameter { Value = clientOrderId, Name = "clientOrderId", Type = ParameterType.GetOrPost });

            return await _api.Execute(request);
        }

        /// <summary>
        /// returns an array of user’s recent orders (order objects) for last 24 hours, sorted by order update time.
        /// /api/1/trading/orders/recent
        /// </summary>
        /// <param name="start_index">Zero-based index, 0 by default</param>
        /// <param name="max_results">Maximum quantity of returned items, at most 1000, Required</param>
        /// <param name="sort">Orders are sorted ascending (default) or descending</param>
        /// <param name="symbols">Comma-separated list of currency symbols</param>
        /// <param name="statuses">Comma-separated list of order statuses: 
        /// new, partiallyFilled, filled, canceled, expired,rejected</param>
        /// <returns></returns>
        public async Task<Orders> GetRecentOrders(int start_index = 0, int max_results = 1000, string sort = "asc", string symbols = null, string statuses = null)
        {
            var request = new RestRequest("/api/1/trading/orders/recent");

            request.Parameters.Add(new Parameter { Value = start_index, Name = "start_index", Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Value = max_results, Name = "max_results", ContentType = "integer", Type = ParameterType.GetOrPost });
            if (!string.IsNullOrEmpty(sort))
                request.Parameters.Add(new Parameter { Value = sort, Name = "sort", Type = ParameterType.GetOrPost });
            if (!string.IsNullOrEmpty(symbols))
                request.Parameters.Add(new Parameter { Value = symbols, Name = "symbols", Type = ParameterType.GetOrPost });
            if (!string.IsNullOrEmpty(statuses))
                request.Parameters.Add(new Parameter { Value = statuses, Name = "statuses", Type = ParameterType.GetOrPost });

            return await _api.Execute(request);
        }

        /// <summary>
        /// returns the trading history - an array of user’s trades(trade objects).
        /// /api/1/trading/trades
        /// </summary>
        /// <param name="by">Selects if filtering and sorting is performed by trade_id or by timestamp, Required
        /// possible values: "trade_id" or "ts"</param>
        /// <param name="start_index">Zero-based index. Default value is 0, Required</param>
        /// <param name="max_results">  Maximum quantity of returned results, at most 1000, Required</param>
        /// <param name="symbols">Comma-separated list of currency symbols</param>
        /// <param name="sort">Trades are sorted ascending (default) or descending</param>
        /// <param name="from">Returns trades with trade_id > specified trade_id (if by=trade_id) or returns trades with timestamp >= specified timestamp(ifby=ts`)</param>
        /// <param name="till">Returns trades with trade_id < specified trade_id (if by=trade_id) or returns trades with timestamp < specified timestamp (if by=ts)</param>
        /// <returns></returns>
        public async Task<Trades> GetTrades(string by = "trade_id", int start_index = 0, int max_results = 1000, string symbols = null, string sort = "asc",
            string from = null, string till = null)
        {
            var request = new RestRequest("/api/1/trading/trades");

            request.Parameters.Add(new Parameter { Value = by, Name = "by", Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Value = start_index, Name = "start_index", Type = ParameterType.GetOrPost });
            request.Parameters.Add(new Parameter { Value = max_results, Name = "max_results", Type = ParameterType.GetOrPost });
            if (!string.IsNullOrEmpty(symbols))
                request.Parameters.Add(new Parameter { Value = symbols, Name = "symbols", Type = ParameterType.GetOrPost });
            if (!string.IsNullOrEmpty(sort))
                request.Parameters.Add(new Parameter { Value = sort, Name = "sort", Type = ParameterType.GetOrPost });
            if (!string.IsNullOrEmpty(from))
                request.Parameters.Add(new Parameter { Value = from, Name = "from", Type = ParameterType.GetOrPost });
            if (!string.IsNullOrEmpty(till))
                request.Parameters.Add(new Parameter { Value = till, Name = "till", Type = ParameterType.GetOrPost });

            var str = request.ToString();
            return await _api.Execute(request);
        }
    } // end of class Trading



} // end of namespace
