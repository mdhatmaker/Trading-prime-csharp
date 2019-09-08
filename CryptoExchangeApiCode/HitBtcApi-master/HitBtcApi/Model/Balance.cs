using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi.Model
{
    public class Balance
    {
        /// <summary>
        /// Currency symbol, e.g. BTC
        /// </summary>
        public string currency_code { get; set; }

        /// <summary>
        /// Funds amount
        /// </summary>
        public double balance { get; set; }
    }

    public class MultiCurrencyBalance
    {
        public List<Balance> balance { get; set; }
    }
}
