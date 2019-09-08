using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using static CryptoTools.Global;

namespace CryptoApis.ExchangeX.CoinMarketCap
{
    // A List of CoinMarketCapTicker objects (that implements NullableObject)
    public class CoinMarketCapTickerList : List<CoinMarketCapTicker>, CryptoTools.Net.NullableObject
    {
        public bool IsNull => (this.Count == 0);

        public void RemoveZeroMarketCap()
        {
            this.RemoveAll(t => t.market_cap_usd == 0);
        }
    }

    public class CoinMarketCapTicker : CryptoTools.Net.NullableObject
    {        
        public string id { get; set; }                  // "bitcoin"
        public string name { get; set; }                // "Bitcoin"
        public string symbol { get; set; }              // "BTC"
        public int rank { get; set; }                   // "1", "2", "3", ...
        public decimal price_usd { get; set; }          // "9343.13"
        public decimal price_btc { get; set; }          // "1.0"
        [JsonProperty(PropertyName = "24h_volume_usd")]
        public decimal _24h_volume_usd { get; set; }    // "8278080000.0"
        public decimal market_cap_usd { get; set; }     // "158904217788"
        public decimal available_supply { get; set; }   // "17007600.0"
        public decimal total_supply { get; set; }       // "17007600.0"
        public decimal max_supply { get; set; }         // "21000000.0"
        public decimal percent_change_1h { get; set; }  // "0.21"
        public decimal percent_change_24h { get; set; } // "0.16"
        public decimal percent_change_7d { get; set; }  // "4.84"
        public long last_updated { get; set; }          // "1525112971"

        public bool IsNull => (id == null);

		public static string CsvHeaders => "DateTime,Symbol,Id,Name,PriceUSD,PriceBTC,Volume24h,MarketCapUSD,AvailableSupply,TotalSupply,MaxSupply,PercentChg1h,PercentChg24h,PercentChg7d";

        public string ToCsv()
		{
			string updateTime = FromTimestampSeconds((int)last_updated).ToString("yyyy-MM-dd HH:mm:ss");
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8:0},{9},{10},{11},{12},{13}", updateTime, symbol, id, name, price_usd, price_btc, _24h_volume_usd, market_cap_usd, available_supply, total_supply, max_supply, percent_change_1h, percent_change_24h, percent_change_7d);
		}

        public override string ToString()
        {
            string updateTime = FromTimestampSeconds((int)last_updated).ToString("yyyy-MM-dd HH:mm:ss");
            return string.Format("{0} {1,4} {2,-5} [{3}] '{4}' ${5} B{6} vol:{7} cap:${8:0,000} supply:[{9:0} / {10:0} / {11:0}] %chg:[{12} {13} {14}]", updateTime, rank, symbol, id, name, price_usd, price_btc, _24h_volume_usd, market_cap_usd, available_supply, total_supply, max_supply, percent_change_1h, percent_change_24h, percent_change_7d);
        }

    } // end of CoinMarketCapTicker
    

    public static class CoinMarketCapExtensions
	{
		public static void Print(this CoinMarketCapTicker t)
		{
            Console.WriteLine("{0,4} {1,-6} {2,-25} {3,7:0.00}% {4,7:0.00}% {5,7:0.00}%  {6,12:0.00000000}btc {7,12:0.00000000}usd   mkt_cap:${8,14:#,##0}  24h_vol:${9,13:#,##0}", t.rank, t.symbol, t.name, t.percent_change_1h, t.percent_change_24h, t.percent_change_7d, t.price_btc, t.price_usd, t.market_cap_usd, t._24h_volume_usd);
		}
	} // end of class CoinMarketCapExtensions


} // end of namespace
