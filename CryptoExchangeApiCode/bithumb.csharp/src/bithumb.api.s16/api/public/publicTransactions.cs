using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace XCT.BaseLib.API.Bithumb.Public
{
    /// <summary>
    /// https://api.bithumb.com/public/recent_transactions/{currency}
    /// bithumb 거래소 거래 체결 완료 내역
    /// * {currency} = BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC)
    /// </summary>
    public class PublicTransactionData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction_date"></param>
        /// <param name="type"></param>
        /// <param name="units_traded"></param>
        /// <param name="price"></param>
        /// <param name="total"></param>
        [JsonConstructor]
        public PublicTransactionData(string transaction_date, string type, string units_traded, string price, string total)
        {
            if (CUnixTime.IsDateTimeFormat(transaction_date) == true)
            {
                var _tdate = CUnixTime.ConvertToUtcTime(transaction_date + "+09:00");
                this.transaction_date = CUnixTime.ConvertToUnixTimeMilli(_tdate);
            }
            else
                this.transaction_date = Convert.ToInt64(transaction_date);

            this.type = type;
            this.units_traded = Convert.ToDecimal(units_traded);
            this.price = decimal.Parse(price, NumberStyles.Float);
            this.total = decimal.Parse(total, NumberStyles.Float);
        }

        /// <summary>
        /// 거래 채결 시간
        /// </summary>
        public long transaction_date
        {
            get;
            set;
        }

        /// <summary>
        /// 판/구매 (ask, bid)
        /// </summary>
        public string type
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 Currency 수량
        /// </summary>
        public decimal units_traded
        {
            get;
            set;
        }

        /// <summary>
        /// 1Currency당 거래금액 (BTC, ETH, DASH, LTC, ETC, XRP)
        /// </summary>
        public decimal price
        {
            get;
            set;
        }


        /// <summary>
        /// 총 거래금액
        /// </summary>
        public decimal total
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicTransactions : ApiResult<List<PublicTransactionData>>
    {
        public PublicTransactions()
        {
            this.data = new List<PublicTransactionData>();
        }
    }
}