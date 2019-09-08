using System.Collections.Generic;

namespace XCT.BaseLib.API.Bithumb.Trade
{
    /// <summary>
    /// 시장가 구매
    /// </summary>
    public class TradeMarketData
    {
        /// <summary>
        /// 체결 번호
        /// </summary>
        public long cont_id
        {
            get;
            set;
        }

        /// <summary>
        /// 총 구매/판매 수량(수수료 포함)
        /// </summary>
        public decimal units
        {
            get;
            set;
        }

        /// <summary>
        /// 1Currency당 체결가 (BTC, ETH, DASH, LTC, ETC, XRP)
        /// </summary>
        public decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 구매/판매 KRW
        /// </summary>
        public decimal total
        {
            get;
            set;
        }

        /// <summary>
        /// 구매/판매 수수료
        /// </summary>
        public decimal fee
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 시장가 구매
    /// </summary>
    public class TradeMarket : ApiResult<List<TradeMarketData>>
    {
        /// <summary>
        /// 주문 번호
        /// </summary>
        public string order_id
        {
            get;
            set;
        }
    }
}