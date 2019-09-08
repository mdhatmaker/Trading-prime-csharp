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
    // https://api.vaultoro.com/

    public class Vaultoro : BaseExchange
    {
        public override string BaseUrl => "https://api.vaultoro.com";
        public override string Name => "VAULTORO";
        public override CryptoExch Exch => CryptoExch.VAULTORO;

        // SINGLETON
        public static Vaultoro Instance { get { return m_instance; } }
        private static Vaultoro m_instance = new Vaultoro();
        private Vaultoro() { }

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    if (CurrencyPairs.Count > 0)
                    {
                        UpdateSymbolsFromCurrencyPairs();
                        return m_symbolList;
                    }

                    var response = GetMarkets();
                    m_symbolList = new List<string>();
                    m_symbolList.Add(response.data.MarketName);
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            //var tickers = GetTickers(new List<string>() { symbol });
            var tickers = GET<Dictionary<string, Ticker>>(BaseUrl + "/ticker/{0}", symbol);
            return tickers.Values.First();
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
            var res = GET<VaultoroOrderBook>(BaseUrl + "/bidandask");    //, symbol);
            //m_errors = res.error;
            return res; //.Values.First();
        }

        //--------------------------------------------------------------------------------------------------------------

        public MarketsResponse GetMarkets()
        {
            var res = GET<MarketsResponse>(BaseUrl + "/markets");
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

        public class MarketsResponse
        {
            public string status { get; set; }
            public Market data { get; set; }
            //public Dictionary<string, CurrencyPair> pairs { get; set; }

            public MarketsResponse()
            {
                data = new Market();
            }
        } // end of class MarketsResponse

        public class Market
        {
            public string MarketCurrency { get; set; }
            public string BaseCurrency { get; set; }
            public string MarketCurrencyLong { get; set; }
            public string BaseCurrencyLong { get; set; }
            public decimal MinTradeSize { get; set; }
            public string MarketName { get; set; }
            public bool IsActive { get; set; }
            public decimal MinUnitQty { get; set; }
            public decimal MinPrice { get; set; }
            public decimal LastPrice { get; set; }
            [JsonProperty(PropertyName = "24hLow")]         // necessary for property names that are invalid in C#
            public decimal _24hLow { get; set; }
            [JsonProperty(PropertyName = "24hHigh")]        // necessary for property names that are invalid in C#
            public decimal _24hHigh { get; set; }
            [JsonProperty(PropertyName = "24hVolume")]      // necessary for property names that are invalid in C#
            public decimal _24hVolume { get; set; }
        } // end of class Market

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
        public class VaultoroOrderBook : ZCryptoOrderBook
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
        } // end of class VaultoroOrderBook

        //==============================================================================================================


    } // end of class Vaultoro


} // end of namespace
