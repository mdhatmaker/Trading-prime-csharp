using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using static Tools.G;
using System.IO;

namespace CryptoAPIs.ExchangeX
{
    // https://coinmarketcap.com/api/

    public class CoinMarketCap
    {
        // SINGLETON
        public static CoinMarketCap Instance { get { return m_instance; } }
        private static readonly CoinMarketCap m_instance = new CoinMarketCap();
        private CoinMarketCap() { }

        public List<CoinMarketCapTicker> GetTickers(int count=0)
        {
            string url = string.Format("https://api.coinmarketcap.com/v1/ticker/?convert=USD&limit={0}", count);
            string json = GetJSON(url);
            var li = DeserializeJson<List<CoinMarketCapTicker>>(json);
            return li;
        }

        public static void WriteToFile(string filename = null)
        {
            if (filename == null) filename = string.Format("CoinMarketCap.{0}.DF.csv", DateTime.Now.ToCompactDateTime());

            var tickers = CoinMarketCap.Instance.GetTickers();

            string pathname = Folders.crypto_path(filename);
            using (var f = new StreamWriter(pathname))
            {
                f.WriteLine(CoinMarketCapTicker.ColumnNames);
                foreach (var t in tickers)
                {
                    //var price = decimal.Parse(t.price_usd ?? "0");
                    //var marketCap = decimal.Parse(t.market_cap_usd ?? "0");
                    //cout("{0} {1} {2} ${3:N}   market_cap:${4:N0}", t.rank, t.symbol, t.name, price, marketCap);
                    if (t.last_updated != null)
                        f.WriteLine(t.ToString());
                }
                cout("CoinMarketCap output to file: '{0}'", pathname);
            }
        }

    } // end of class

    //----------------------------------------------------------------------------------------------------------------------------------------------------------

    public class CoinMarketCapTicker : IDataRow
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

        public static string ColumnNames = "DateTime,Rank,Symbol,Name,PriceBTC,PriceUSD,MarketCapUSD,AvailableSupply,TotalSupply,PercentChange1h,PercentChange24h,PercentChange7d,Volume24hUSD";

        public override string ToString()
        {
            string time;
            int result;
            if (int.TryParse(last_updated, out result))
                time = result.ToDateTimeString();
            else
                time = "";
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", time, rank, symbol, name, price_btc, price_usd, market_cap_usd, available_supply, total_supply, percent_change_1h, percent_change_24h, percent_change_7d, _24h_volume_usd);
            //return "CoinMarketCapTicker: " + Str(this);
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

        //Example: https://api.coinmarketcap.com/v1/ticker/
        //Example: https://api.coinmarketcap.com/v1/ticker/?limit=10
        //Example: https://api.coinmarketcap.com/v1/ticker/?convert=EUR&limit=10
        public static List<CoinMarketCapTicker> GetList()
        {
            //string url = "https://api.coinmarketcap.com/v1/ticker/?convert=USD";
            string url = string.Format("https://api.coinmarketcap.com/v1/ticker/?convert=USD&limit={0}", 20);
            string json = GetJSON(url);
            //cout(json);
            var li = DeserializeJson<List<CoinMarketCapTicker>>(json);
            return li;
        }

    } // end of CLASS CoinMarketCapTicker


} // end of NAMESPACE
