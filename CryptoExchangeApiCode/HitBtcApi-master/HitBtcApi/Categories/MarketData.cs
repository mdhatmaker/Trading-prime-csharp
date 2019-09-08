using System;
using HitBtcApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace HitBtcApi.Categories
{
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
    }
}