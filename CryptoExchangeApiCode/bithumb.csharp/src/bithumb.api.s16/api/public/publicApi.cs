using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XCT.BaseLib.API.Bithumb.Public
{
    /// <summary>
    /// https://api.bithumb.com/
    /// </summary>
    public class BPublicApi
    {
        private string __connect_key;
        private string __secret_key;

        /// <summary>
        /// 
        /// </summary>
        public BPublicApi()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public BPublicApi(string connect_key, string secret_key)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
        }

        private BithumbClient __public_client = null;

        private BithumbClient PublicClient
        {
            get
            {
                if (__public_client == null)
                    __public_client = new BithumbClient(__connect_key, __secret_key);
                return __public_client;
            }
        }

        /// <summary>
        /// bithumb 거래소 마지막 거래 정보
        /// </summary>
        /// <param name="currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC), ALL(전체)</param>
        /// <returns></returns>
        public async Task<PublicTicker> Ticker(string currency)
        {
            return await PublicClient.CallApiGetAsync<PublicTicker>($"/public/ticker/{currency}");
        }

        /// <summary>
        /// bithumb 거래소 마지막 거래 정보
        /// </summary>
        /// <returns></returns>
        public async Task<PublicTickers> TickerAll()
        {
            return await PublicClient.CallApiGetAsync<PublicTickers>($"/public/ticker/ALL");
        }

        /// <summary>
        /// bithumb 거래소 판/구매 등록 대기 또는 거래 중 내역 정보
        /// </summary>
        /// <param name="currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC), ALL(전체)</param>
        /// <param name="group_orders">Value : 0 또는 1 (Default : 1)</param>
        /// <param name="count">Value : 1 ~ 50 (Default : 20)</param>
        /// <returns></returns>
        public async Task<PublicOrderBook> OrderBook(string currency, int group_orders = 1, int count = 20)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("group_orders", group_orders);
                _params.Add("count", count);
            }

            return await PublicClient.CallApiGetAsync<PublicOrderBook>($"/public/orderbook/{currency}", _params);
        }

        /// <summary>
        /// bithumb 거래소 거래 체결 완료 내역
        /// </summary>
        /// <param name="currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC)</param>
        /// <param name="offset">Value : 0 ~ (Default : 0)</param>
        /// <param name="count">Value : 1 ~ 100 (Default : 20)</param>
        /// <returns></returns>
        public async Task<PublicTransactions> RecentTransactions(string currency, int offset = 0, int count = 50)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("offset", offset);
                _params.Add("count", count);
            }

            return await PublicClient.CallApiGetAsync<PublicTransactions>($"/public/recent_transactions/{currency}", _params);
        }
    }
}