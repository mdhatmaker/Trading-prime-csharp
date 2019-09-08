using System;
using System.Collections.Generic;

namespace CryptoAPIs
{
    // This class contains a dictionary mapping currency (string) to Min and Max values (decimal)
    // We can use this to set limits on the Balances we want to maintain in each currency
    public class BalanceMinMaxMap
    {
        private Dictionary<string, BalanceMinMax> m_limits = new Dictionary<string, BalanceMinMax>();

        public void Add(string currency, decimal min, decimal max)
        {
            m_limits[currency] = new BalanceMinMax(currency, min, max);
        }

        public BalanceMinMax this[string s]
        {
            get { return m_limits[s]; }
        }

        // Given a dictionary of starting balances, create min/max values based on given percentages (upPct/downPct)
        public static BalanceMinMaxMap CreatePercentageUpDown(IDictionary<string, ZAccountBalance> balances, decimal upPct, decimal downPct)
        {
            var limits = new BalanceMinMaxMap();
            foreach (var kv in balances)
            {
                var free = balances[kv.Key].Free;
                limits.Add(kv.Key, free - (downPct / 100 * free), free + (upPct / 100 * free));
            }
            return limits;
        }
    } // end of class BalanceMinMaxMap

    // A single entry in the BalanceMinMaxMap dictionary
    public class BalanceMinMax
    {
        public string Currency;
        public decimal Min;
        public decimal Max;

        public BalanceMinMax(string currency, decimal min, decimal max)
        {
            Currency = currency;
            Min = min;
            Max = max;
        }
    } // end of class BalanceMinMax

} // end of namespace
