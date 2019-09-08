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
    // https://wex.nz/api/3/docs

    public class Wex : BaseExchange
    {
        public override string BaseUrl { get { return "https://wex.nz/api/3"; } }
        public override string ExchangeName { get { return "WEX"; } }

        // SINGLETON
        public static Wex Instance { get { return m_instance; } }
        private static Wex m_instance = new Wex();
        private Wex() { }

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    var response = GetPairs();
                    m_symbolList = new List<string>();
                    foreach (string symbol in response.pairs.Keys)
                    {
                        m_symbolList.Add(symbol);
                    }
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            var res = GET<Dictionary<string, Ticker>>(BaseUrl + "/ticker/{0}", symbol);
            return res.Values.First();
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var symbols = SymbolList;
            Dictionary<string, Ticker> tickers = GetTickers(symbols);
            foreach (var k in tickers.Keys)
            {
                result[k] = tickers[k];
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)     //, int limit=150)
        {
            var res = GET<Dictionary<string, WexOrderBook>>(BaseUrl + "/depth/{0}", symbol);
            //var res = GET<Dictionary<string, OrderBook>>(BaseUrl + "/depth/{0}?limit={1}", symbol, limit);
            //m_errors = res.error;
            return res.Values.First();
        }

        //--------------------------------------------------------------------------------------------------------------

        public CurrencyPairsResponse GetPairs()
        {
            var res = GET<CurrencyPairsResponse>(BaseUrl + "/info");
            return res;
        }

        public Dictionary<string, Ticker> GetTickers(List<string> assetPairs)
        {
            if (assetPairs == null)
            {
                //m_errors = new List<string>();
                return new Dictionary<string, Ticker>();
            }
            string pairs = string.Join("-", assetPairs);
            var res = GET<Dictionary<string, Ticker>>(BaseUrl +  "/ticker/{0}", pairs);
            /*if (res.result == null)
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
            }*/
            //m_errors = res.error;
            return res;
        }

        //==============================================================================================================

        /*public class Result<T>
        {
            public T result { get; set; }
            public string errorCode { get; set; }
            public string errorMsg { get; set; }
        } // end of class Result*/

        public class CurrencyPairsResponse
        {
            public long server_time { get; set; }
            public Dictionary<string, CurrencyPair> pairs { get; set; }

            public CurrencyPairsResponse()
            {
                pairs = new Dictionary<string, CurrencyPair>();
            }
        } // end of class CurrencyPairsResponse

        public class CurrencyPair
        {
            public int decimal_places { get; set; }
            public decimal min_price { get; set; }
            public decimal max_price { get; set; }
            public decimal min_amount { get; set; }
            public int hidden { get; set; }
            public decimal fee { get; set; }
        } // end of class CurrencyPair

        // Ticker
        public class Ticker : ZTicker
        {
            public decimal high { get; set; }
            public decimal low { get; set; }
            public decimal avg { get; set; }
            public double vol { get; set; }
            public double vol_cur { get; set; }
            public decimal last { get; set; }
            public decimal buy { get; set; }
            public decimal sell { get; set; }
            public long updated { get; set; }

            public override decimal Bid { get { return buy; } }
            public override decimal Ask { get { return sell; } }
            public override decimal Last { get { return last; } }
            public override decimal High { get { return high; } }
            public override decimal Low { get { return low; } }
            public override decimal Volume { get { return (decimal)vol; } }
            public override string Timestamp { get { return updated.ToString(); } }
        } // end of class Ticker

        // Order Book
        public class WexOrderBook : ZCryptoOrderBook
        {
            public override List<ZCryptoOrderBookEntry> Bids
            {
                get
                {
                    var li = new List<ZCryptoOrderBookEntry>();
                    foreach (var ja in bids)
                    {
                        decimal price = ja.Value<decimal>(0);
                        decimal amount = ja.Value<decimal>(1);
                        long timestamp = 0; //ja.Value<long>(2);                        
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
                        decimal price = ja.Value<decimal>(0);
                        decimal amount = ja.Value<decimal>(1);
                        long timestamp = 0; //ja.Value<long>(2);
                        li.Add(new ZCryptoOrderBookEntry(price, amount, timestamp));
                    }
                    return li;
                }
            }

            public List<JArray> bids { get; set; }
            public List<JArray> asks { get; set; }
        } // end of class OrderBook


    } // end of class Wex

    //======================================================================================================================================



} // end of namespace
