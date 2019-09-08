using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tools;
using static Tools.G;
using static Tools.GDate;

namespace CryptoAPIs.Exchange
{
    // https://x-crypto.com/_cc_api.php

    public class XCrypto : BaseExchange
    {
        public override string BaseUrl { get { return "https://x-crypto.com/api"; } }
        public override string ExchangeName { get { return "XCRYPTO"; } }

        // SINGLETON
        public static XCrypto Instance { get { return m_instance; } }
        private static readonly XCrypto m_instance = new XCrypto();
        private XCrypto() { }


        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    m_symbolList = new List<string>() { "btc_usd", "ltc_usd", "btc_eur", "ltc_eur", "btc_rub", "ltc_rub", "usd_eur", "usd_rub" };
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string pair = "btc_usd")
        {
            var substrings = pair.Split('_');
            string json = GetJSON(string.Format("https://x-crypto.com/api/ticker/{0}/{1}", substrings[0], substrings[1]));
            return DeserializeJson<XCryptoTicker>(json);
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var symbols = SymbolList;
            foreach (var s in symbols)
            {
                result[s] = GetTicker(s);
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            return null;
        }

    } // end of CLASS

    //======================================================================================================================================

    public class XCryptoTicker : ZTicker
    {
        //[JsonProperty(PropertyName = "15m")]        // necessary for property names that begin with a digit (or other invalid starting character)
        public float high { get; set; }
        public float low { get; set; }
        public float avg { get; set; }
        [JsonProperty(PropertyName = "vol")]
        public float volume { get; set; }
        public float last { get; set; }
        public float buy { get; set; }
        public float sell { get; set; }
        public float since_last { get; set; }
        public float since_24_hour { get; set; }
        public long timestamp { get; set; }

        public override decimal Bid { get { return (decimal)buy; } }
        public override decimal Ask { get { return (decimal)sell; } }
        public override decimal Last { get { return (decimal)last; } }
        public override decimal High { get { return (decimal)high; } }
        public override decimal Low { get { return (decimal)low; } }
        public override decimal Volume { get { return (decimal)volume; } }
        public override string Timestamp { get { return timestamp.ToString(); } }

        /*public override string ToString()
        {
            return string.Format("[{0}] last:{1} buy:{2} sell:{3} vol:{4} high:{5} low:{6} since_last:{7} since_24hr:{8}", UnixTimeStampToDateTime(timestamp), last, buy, sell, volume, high, low, since_last, since_24_hour);
        }*/

        public static XCryptoTicker GetObject(string currency1="btc", string currency2="usd")
        {
            string json = GetJSON(string.Format("https://x-crypto.com/api/ticker/{0}/{1}", currency1, currency2));
            return DeserializeJson<XCryptoTicker>(json);
        }
    } // end of CLASS XCryptoTicker

    public class XCryptoOrderBook
    {
        public long timestamp { get; set; }
        public string pair { get; set; }
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }

        public override string ToString()
        {
            return "XCryptoOrderBook::" + Str(this);
        }

        public static XCryptoOrderBook GetObject(string currency1 = "btc", string currency2 = "usd", int maxOrdersInList = 5)
        {
            string json = GetJSON(string.Format("https://x-crypto.com/api/orderbook/{0}/{1}/{2}", currency1, currency2, maxOrdersInList));
            cout(json);
            return DeserializeJson<XCryptoOrderBook>(json);
        }        
    } // end of CLASS XCryptoOrderBook

    public class XCryptoTrade
    {
        public string type { get; set; }
        public float rate { get; set; }
        public float amount { get; set; }
        public int tid { get; set; }
        public long timestamp { get; set; }

        public override string ToString()
        {
            return "XCryptoTrade::" + Str(this);
        }
    } // end of class XCryptoTrade

    public class XCryptoTrades
    {
        public long timestamp { get; set; }
        public string pair { get; set; }
        public List<XCryptoTrade> trades { get; set; }

        public override string ToString()
        {
            return "XCryptoTrades::" + Str(this);
        }

        public static XCryptoTrades GetObject(string currency1 = "btc", string currency2 = "usd", int maxTradesInList = 5)
        {
            // or GET https://x-crypto.com/api/trades/<currency1>/<currency2>?since=<transaction-id>
            string json = GetJSON(string.Format("https://x-crypto.com/api/trades/{0}/{1}/{2}", currency1, currency2, maxTradesInList));
            cout(json);
            return DeserializeJson<XCryptoTrades>(json);
        }
    } // end of CLASS XCryptoTrades

} // end of NAMESPACE
