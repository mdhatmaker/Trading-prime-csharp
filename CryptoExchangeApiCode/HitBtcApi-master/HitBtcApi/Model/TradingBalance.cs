using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi.Model
{
    public class TradingBalance
    {
        /// <summary>
        /// Currency symbol, e.g. BTC
        /// </summary>
        public string currency_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double cash { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double reserved { get; set; }
    }

    public class TradingBalanceList
    {
        public List<TradingBalance> balance { get; set; }
    }
}
