using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    public class Coinone : BaseExchange
    {
        // http://doc.coinone.co.kr/

        public override string BaseUrl { get { return "https://api.coinone.co.kr/"; } }
        public override string ExchangeName { get { return "COINONE"; } }

        // SINGLETON
        public static Coinone Instance { get { return m_instance; } }
        private static Coinone m_instance = new Coinone();
        private Coinone() { }

        class CoinoneResult<T>
        {
            public T result { get; set; }
            public string errorCode { get; set; }
            public string errorMsg { get; set; }
        } // end of class CoinoneResult

        public override List<string> SymbolList
        {
            get
            {

                if (m_symbolList == null)
                {
                    m_symbolList = new List<string>() { "bch", "qtum", "iota", "ltc", "etc", "btg", "btc", "eth", "xrp" };
                    //GET<Dictionary<string, CoinoneTicker>>("https://api.coinone.co.kr/ticker/?format=json&currency=all");
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            throw new NotImplementedException();
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var symbols = SymbolList;
            /*Dictionary<string, CoinoneTicker> tickers = GetTickers(symbols, out List<string> errors);
            foreach (var k in tickers.Keys)
            {
                result[k] = tickers[k];
            }*/
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            var res = GET<CoinoneResult<Dictionary<string, CoinoneOrderBook>>>(BaseUrl + "/Depth?pair={0}", symbol);
            //m_errors = res.error;
            return res.result.Values.First();
        }

        /*// The assetPairs List should contain pairs such as "BCHUSD", "DASHEUR", etc. Use KrakenAssetPair class to get available.
        public Dictionary<string, CoinoneTicker> GetTickers(List<string> assetPairs, out List<string> errors)
        {
            if (assetPairs == null)
            {
                errors = new List<string>();
                return new Dictionary<string, CoinoneTicker>();
            }
            string pairs = string.Join(",", assetPairs);
            var res = GET<KrakenResult<Dictionary<string, CoinoneTicker>>>(BaseUrl +  "/Ticker?pair={0}", pairs);
            if (res.result == null)
            {
                ErrorMessage(ExchangeName + ": GetTickers has a NULL result!");
            }
            else
            {
                foreach (var k in res.result.Keys)
                {
                    var ticker = res.result[k];
                    ticker.UpdateProperties();
                }
            }
            errors = res.error;
            return res.result;
        }*/

    } // end of class Coinone

    //======================================================================================================================================



    // Order Book
    public class CoinoneOrderBook : ZCryptoOrderBook
    {
        public override List<ZCryptoOrderBookEntry> Bids
        {
            get
            {
                var li = new List<ZCryptoOrderBookEntry>();
                foreach (var ja in bids)
                {
                    string price = ja.Value<string>(0);
                    string amount = ja.Value<string>(1);
                    long timestamp = ja.Value<long>(2);
                    li.Add(new ZCryptoOrderBookEntry(price, amount, timestamp));
                }
                return li;
            }
        }
        public override List<ZCryptoOrderBookEntry> Asks
        {
            get
            {
                var li = new List<ZCryptoOrderBookEntry>();
                foreach (var ja in asks)
                {
                    string price = ja.Value<string>(0);
                    string amount = ja.Value<string>(1);
                    long timestamp = ja.Value<long>(2);
                    li.Add(new ZCryptoOrderBookEntry(price, amount, timestamp));
                }
                return li;
            }
        }

        public List<JArray> bids { get; set; }
        public List<JArray> asks { get; set; }
    } // end of class CoineoneOrderBook

    // Ticker
    public class CoinoneTicker : ZTicker
    {
        public decimal volume { get; set; }             // "4599.9232"
        public int last { get; set; }                   // "960500"
        public int yesterday_last { get; set; }         // "1089500"
        public int yesterday_low { get; set; }          // "1078500"
        public int high { get; set; }                   // "1160000"
        public string currency { get; set; }            // "bch"
        public int low { get; set; }                    // "852000"
        public int yesterday_first { get; set; }        // "1250000"
        public decimal yesterday_volume { get; set; }   // "2113.7707"
        public int yesterday_high { get; set; }         // "1295000"
        public int first { get; set; }                  // "1152500"

        public override decimal Bid { get { return decimal.Parse("0M"); } }
        public override decimal Ask { get { return decimal.Parse("0M"); } }
        public override decimal Last { get { return last; } }
        public override decimal High { get { return high; } }
        public override decimal Low { get { return low; } }
        public override decimal Volume { get { return this.volume; } }
        public override string Timestamp { get { return DateTime.Now.ToUnixTimestamp().ToString(); } }  
    }

    /*public class CoinoneTicker : ZTicker
    {
        public List<string> a { get; set; }
        public List<string> b { get; set; }
        public List<string> c { get; set; }
        public List<string> v { get; set; }
        public List<string> p { get; set; }
        public List<int> t { get; set; }
        public List<string> l { get; set; }
        public List<string> h { get; set; }
        public float o { get; set; }

        private PriceArray m_ask;
        private PriceArray m_bid;
        private float m_lastTradePrice;
        private float m_lastTradeVolume;
        private float m_volumeToday;
        private float m_volume24h;
        private int m_tradesToday;
        private int m_trades24h;
        private float m_vwapToday;
        private float m_vwap24h;
        private float m_lowToday;
        private float m_low24h;
        private float m_highToday;
        private float m_high24h;
        private DateTime m_timestamp;

        // ICryptoTicker interface implementation
        public override string Bid { get { return m_bid.Price.ToString(); } }
        public override string Ask { get { return m_ask.Price.ToString(); } }
        public override string Last { get { return m_lastTradePrice.ToString(); } }
        public override string High { get { return m_high24h.ToString(); } }
        public override string Low { get { return m_low24h.ToString(); } }
        public override string Volume { get { return this.m_volume24h.ToString(); } }
        public override string Timestamp { get { return m_timestamp.ToString(); } }

        public void UpdateProperties()
        {
            m_ask = new PriceArray(this.a);
            m_bid = new PriceArray(this.b);
            m_lastTradePrice = float.Parse(this.c[0]);
            m_lastTradeVolume = float.Parse(this.c[1]);
            m_volumeToday = float.Parse(this.v[0]);
            m_volume24h = float.Parse(this.v[1]);
            m_vwapToday = float.Parse(this.p[0]);
            m_vwap24h = float.Parse(this.p[1]);
            m_tradesToday = t[0];
            m_trades24h = t[1];
            m_lowToday = float.Parse(this.l[0]);
            m_low24h = float.Parse(this.l[1]);
            m_highToday = float.Parse(this.h[0]);
            m_high24h = float.Parse(this.h[1]);
            m_timestamp = DateTime.Now;
        }

    } // end of class CoineoneTicker*/


} // end of namespace
