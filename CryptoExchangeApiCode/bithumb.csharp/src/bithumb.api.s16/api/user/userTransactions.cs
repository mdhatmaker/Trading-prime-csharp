using System.Collections.Generic;

namespace XCT.BaseLib.API.Bithumb.User
{
    /// <summary>
    /// 회원 거래 내역
    /// </summary>
    public class UserTransactionData
    {
        /// <summary>
        /// 검색 구분 (0 : 전체, 1 : 구매완료, 2 : 판매완료, 3 : 출금중, 4 : 입금, 5 : 출금, 9 : KRW입금중)
        /// </summary>
        public int search
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 일시 Timestamp
        /// </summary>
        public long transfer_date
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 Currency 수량 (BTC, ETH, DASH, LTC, ETC, XRP)
        /// </summary>
        public string units
        {
            get;
            set;
        }

        /// <summary>
        /// 거래금액
        /// </summary>
        public decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 1Currency당 거래금액 (btc, eth, dash, ltc, etc, xrp)
        /// </summary>
        public decimal btc1krw
        {
            get;
            set;
        }

        public decimal eth1krw
        {
            get;
            set;
        }

        public decimal dash1krw
        {
            get;
            set;
        }

        public decimal ltc1krw
        {
            get;
            set;
        }

        public decimal etc1krw
        {
            get;
            set;
        }

        public decimal xrp1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 거래수수료
        /// </summary>
        public string fee
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 후 Currency 잔액 (btc, eth, dash, ltc, etc, xrp)
        /// </summary>
        public decimal btc_remain
        {
            get;
            set;
        }

        public decimal eth_remain
        {
            get;
            set;
        }

        public decimal dash_remain
        {
            get;
            set;
        }

        public decimal ltc_remain
        {
            get;
            set;
        }

        public decimal etc_remain
        {
            get;
            set;
        }

        public decimal xrp_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 후 KRW 잔액
        /// </summary>
        public decimal krw_remain
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 회원 거래 내역
    /// </summary>
    public class UserTransactions : ApiResult<List<UserTransactionData>>
    {
        public UserTransactions()
        {
            this.data = new List<UserTransactionData>();
        }
    }
}