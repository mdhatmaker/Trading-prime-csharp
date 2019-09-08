using System;
using System.Collections.Generic;
using System.Linq;

namespace XCT.BaseLib.API.Bithumb.Public
{
    /// <summary>
    /// https://api.bithumb.com/public/orderbook/{currency}
    /// bithumb 거래소 판/구매 등록 대기 또는 거래 중 내역 정보
    /// * {currency} = BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC), ALL(전체)
    /// </summary>
    public class PublicOrderBookData
    {
        /// <summary>
        /// Currency 수량
        /// </summary>
        public decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 1Currency당 거래금액
        /// </summary>
        public decimal price
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OrderBookData
    {
        /// <summary>
        /// 현재 시간 Timestamp
        /// </summary>
        public long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 화폐단위
        /// </summary>
        public string order_currency
        {
            get;
            set;
        }

        /// <summary>
        /// 결제 화폐단위
        /// </summary>
        public string payment_currency
        {
            get;
            set;
        }

        /// <summary>
        /// 구매요청
        /// </summary>
        public List<PublicOrderBookData> bids
        {
            get;
            set;
        }

        /// <summary>
        /// 판매요청
        /// </summary>
        public List<PublicOrderBookData> asks
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicOrderBook : ApiResult<OrderBookData>
    {
        public PublicOrderBook()
        {
            this.data = new OrderBookData();
        }
    }
}