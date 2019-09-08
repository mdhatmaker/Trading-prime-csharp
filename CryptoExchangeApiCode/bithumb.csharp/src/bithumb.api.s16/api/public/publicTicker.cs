namespace XCT.BaseLib.API.Bithumb.Public
{
    /// <summary>
    /// https://api.bithumb.com/public/ticker/{currency}
    /// bithumb 거래소 마지막 거래 정보
    /// * {currency} = BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC), ALL(전체)
    /// </summary>
    public class PublicTickerData
    {
        /// <summary>
        /// 최근 24시간 내 시작 거래금액
        /// </summary>
        public decimal opening_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 마지막 거래금액
        /// </summary>
        public decimal closing_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 최저 거래금액
        /// </summary>
        public decimal min_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 최고 거래금액
        /// </summary>
        public decimal max_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 평균 거래금액
        /// </summary>
        public decimal average_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 BTC 거래량
        /// </summary>
        public decimal units_traded
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 1일간 BTC 거래량
        /// </summary>
        public decimal volume_1day
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 7일간 BTC 거래량
        /// </summary>
        public decimal volume_7day
        {
            get;
            set;
        }


        /// <summary>
        /// 거래 대기건 최고 구매가
        /// </summary>
        public decimal buy_price
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 대기건 최소 판매가
        /// </summary>
        public decimal sell_price
        {
            get;
            set;
        }

        /// <summary>
        /// 현재 시간 Timestamp
        /// </summary>
        public long date
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicTicker : ApiResult<PublicTickerData>
    {
        public PublicTicker()
        {
            this.data = new PublicTickerData();
        }
    }

    public class PublicTickerAll
    {
        public PublicTickerAll()
        {
            BTC = new PublicTickerData();
            ETH = new PublicTickerData();
            DASH = new PublicTickerData();
            LTC = new PublicTickerData();
            ETC = new PublicTickerData();
            XRP = new PublicTickerData();
        }

        /// <summary>
        /// BitCoin
        /// </summary>
        public PublicTickerData BTC
        {
            get;
            set;
        }

        /// <summary>
        /// Ethereum
        /// </summary>
        public PublicTickerData ETH
        {
            get;
            set;
        }

        /// <summary>
        /// DashCoin
        /// </summary>
        public PublicTickerData DASH
        {
            get;
            set;
        }

        /// <summary>
        /// LiteCoin
        /// </summary>
        public PublicTickerData LTC
        {
            get;
            set;
        }

        /// <summary>
        /// Ethereum Classic
        /// </summary>
        public PublicTickerData ETC
        {
            get;
            set;
        }

        /// <summary>
        /// Ripple
        /// </summary>
        public PublicTickerData XRP
        {
            get;
            set;
        }

        /// <summary>
        /// 현재 시간 Timestamp
        /// </summary>
        public long date
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicTickers : ApiResult<PublicTickerAll>
    {
        public PublicTickers()
        {
            this.data = new PublicTickerAll();
        }
    }
}