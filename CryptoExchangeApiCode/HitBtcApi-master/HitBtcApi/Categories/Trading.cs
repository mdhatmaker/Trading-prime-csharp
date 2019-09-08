using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;
using HitBtcApi.Model;
using RestSharp;

namespace HitBtcApi.Categories
{
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
        /// <param name="max_results"> 	Maximum quantity of returned results, at most 1000, Required</param>
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
    }
}
