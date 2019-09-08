using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using static Tools.G;

namespace CryptoAPIs.ExchangeX
{
    public class CoinTracking
    {
        // https://cointracking.info/api/api.php
        // API Key Status: active
        // Your API Key: 8d78a70904a938b891c705322f1b5b0f
        // Your API Secret: 5e6b214aa63a9e51063ed5dc0b00f28101b5ed9d8e5a3e4b

        public static void Test()
        {

        }

        public static List<CoinTrackingTicker> GetTickers(int count=0)
        {
            string url = string.Format("https://api.coinmarketcap.com/v1/ticker/?convert=USD&limit={0}", count);
            string json = GetJSON(url);
            var li = DeserializeJson<List<CoinTrackingTicker>>(json);
            return li;
        }


    } // end of class

    //----------------------------------------------------------------------------------------------------------------------------------------------------------

    public class CoinTrackingTicker : IDataRow
    {
        static public string[] Columns = { "symbol", "name", "id", "rank", "market_cap_usd", "total_supply", "available_supply", "24h_volume_usd", "percent_change_1h", "percent_change_24h", "percent_change_7d", "price_usd", "price_btc", "last_updated" };

        public string Key { get { return this.id; } set {; } }

        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string rank { get; set; }
        public string price_usd { get; set; }
        public string price_btc { get; set; }
        [JsonProperty(PropertyName = "24h_volume_usd")]
        public string _24h_volume_usd { get; set; }
        public string market_cap_usd { get; set; }
        public string available_supply { get; set; }
        public string total_supply { get; set; }
        public string percent_change_1h { get; set; }
        public string percent_change_24h { get; set; }
        public string percent_change_7d { get; set; }
        public string last_updated { get; set; }

        private string[] cellValues = new string[14];

        public override string ToString()
        {
            return "CoinTrackingTicker: " + Str(this);
            //return string.Format("15m: {0}   last: {1}   buy: {2}   sell: {3}   currency: {4}", _15m, last, buy, sell, currency_symbol);
        }

        public string[] GetCells()
        {
            cellValues[0] = id;
            cellValues[1] = name;
            cellValues[2] = symbol;
            cellValues[3] = rank;
            cellValues[4] = price_usd;
            cellValues[5] = price_btc;
            cellValues[6] = _24h_volume_usd;
            cellValues[7] = market_cap_usd;
            cellValues[8] = available_supply;
            cellValues[9] = total_supply;
            cellValues[10] = percent_change_1h;
            cellValues[11] = percent_change_24h;
            cellValues[12] = percent_change_7d;
            cellValues[13] = last_updated;
            return cellValues;
        }

        //Example: https://cointracking.info/api/api.php
        public static List<CoinTrackingTicker> GetList()
        {
            //string url = "https://api.coinmarketcap.com/v1/ticker/?convert=USD";
            string url = string.Format("https://api.coinmarketcap.com/v1/ticker/?convert=USD&limit={0}", 20);
            string json = GetJSON(url);
            //cout(json);
            var li = DeserializeJson<List<CoinTrackingTicker>>(json);
            return li;
        }
    } // end of CLASS CoinTrackingTicker

    /*public class BlockchainInfoStats
    {
        //[JsonProperty(PropertyName = "15m")]        // necessary for property names that begin with a digit (or other invalid starting character)
        public float market_price_usd { get; set; }
        public float hash_rate { get; set; }
        public long total_fees_btc { get; set; }
        public long n_btc_mined { get; set; }
        public int n_tx { get; set; }
        public int n_blocks_mined { get; set; }
        public float minutes_between_blocks { get; set; }
        public long totalbc { get; set; }
        public int n_blocks_total { get; set; }
        public float estimated_transaction_volume_usd { get; set; }
        public long blocks_size { get; set; }
        public float miners_revenue_usd { get; set; }
        public int nextretarget { get; set; }
        public long difficulty { get; set; }
        public long estimated_btc_sent { get; set; }
        public int miners_revenue_btc { get; set; }
        public long total_btc_sent { get; set; }
        public float trade_volume_btc { get; set; }
        public float trade_volume_usd { get; set; }
        public long timestamp { get; set; }

        public override string ToString()
        {
            return string.Format("market_price_usd: {0}", market_price_usd);
        }

        public static BlockchainInfoStats GetObject()
        {
            string json = GetJSON("https://api.blockchain.info/stats");
            //cout(json);
            return DeserializeJson<BlockchainInfoStats>(json);
        }

    } // end of CLASS BlockChainInfoStats*/

} // end of NAMESPACE
