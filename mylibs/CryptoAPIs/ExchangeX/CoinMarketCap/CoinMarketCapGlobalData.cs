using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using static CryptoTools.Global;

namespace CryptoApis.ExchangeX.CoinMarketCap
{
    public class CoinMarketCapGlobalData : CryptoTools.Net.NullableObject
    {
        public decimal total_market_cap_usd { get; set; }
        public decimal total_24h_volume_usd { get; set; }
        public decimal bitcoin_percentage_of_market_cap { get; set; }
        public int active_currencies { get; set; }
        public int active_assets { get; set; }
        public int active_markets { get; set; }
        public long last_updated { get; set; }

        public bool IsNull => false;

        public override string ToString()
        {
            string updateTime = FromTimestampSeconds((int)last_updated).ToString("yyyy-MM-dd HH:mm:ss");
            return string.Format("{0} GLOBAL_TOTALS cap:${1:#,###} vol:{2:#,###}  btc%:{3}  currencies:{4:#,###} assets:{5:#,###} markets:{6:#,###}", updateTime, total_market_cap_usd, total_24h_volume_usd, bitcoin_percentage_of_market_cap, active_currencies, active_assets, active_markets);
        }
    } // end of CoinMarketCapGlobalData

} // end of namespace
