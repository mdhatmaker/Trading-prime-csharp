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
    public class BitcoinChartsWeightedPrices : IDataRow
    {        
        public static string BaseUrl = "http://api.bitcoincharts.com";

        static public string[] Columns = { "symbol", "24h", "7d", "30d", "timestamp" };

        public string Key { get { return m_symbol; } set { ; } }

        [JsonProperty(PropertyName = "24h")]
        public string _24h { get; set; }
        [JsonProperty(PropertyName = "7d")]
        public string _7d { get; set; }
        [JsonProperty(PropertyName = "30d")]
        public string _30d { get; set; }

        public static string timestamp { get; set; }

        private string[] cellValues = new string[5];
        private string m_symbol;

        public override string ToString()
        {
            return "BitcoinChartsWeightedPrices: " + Str(this);
            //return string.Format("15m: {0}   last: {1}   buy: {2}   sell: {3}   currency: {4}", _15m, last, buy, sell, currency_symbol);
        }

        public string[] GetCells()
        {
            //cellValues[0] = id;
            cellValues[1] = _24h;
            cellValues[2] = _7d;
            cellValues[3] = _30d;
            cellValues[4] = timestamp;
            return cellValues;
        }

        public static Dictionary<string, BitcoinChartsWeightedPrices> GetDictionary()
        {
            var dict = new Dictionary<string, BitcoinChartsWeightedPrices>();
            string url = "http://api.bitcoincharts.com/v1/weighted_prices.json";
            string json = GetJSON(url);
            //cout(json);
            int i1 = json.IndexOf("\"timestamp\"");
            if (i1 >= 0)
            {
                int i2 = json.IndexOf(',', i1 + 1);
                string sub = json.Substring(i1 - 1, i2 - i1 + 1);
                json = json.Substring(0, i1 - 2) + json.Substring(i2);
                string[] split = sub.Split(':');
                timestamp = split[1].Trim();
                dict = DeserializeJson<Dictionary<string, BitcoinChartsWeightedPrices>>(json);
                foreach (var k in dict.Keys)
                {
                    dict[k].m_symbol = k;
                }
            }
            else
            {
                dout("BitcoinCharts::GetDictionary(): No timestamp found in JSON '{0}'", json);
            }
            return dict;
        }

        /*public static List<BitcoinChartsWeightedPrices> GetList()
        {
            string url = "http://api.bitcoincharts.com/v1/weighted_prices.json";
            string json = GetJSON(url);
            //cout(json);
            var li = DeserializeJson<List<BitcoinChartsWeightedPrices>>(json);
            return li;
        }*/

    } // end of CLASS BitcoinChartWeightedPrices

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
