using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
using Newtonsoft.Json;

namespace CryptoAPIs.Exchange
{
    // https://www.bit-z.com/api.html

    public class BitZ : BaseExchange
    {
        public override string BaseUrl { get { return "https://www.bit-z.com/api_v1"; } }
        public override string ExchangeName { get { return "BITZ"; } }

        // SINGLETON
        public static BitZ Instance { get { return m_instance; } }
        private static readonly BitZ m_instance = new BitZ();
        private BitZ() { }

        private static readonly HttpClient m_httpClient = new HttpClient();

        private Dictionary<string, int> m_channelIds = new Dictionary<string, int>();

        class BitZResponse<T>
        {
            public int code { get; set; }       // status code (ex: 0)
            public string msg { get; set; }     // message (ex: "Success")
            public T data { get; set; }
        }

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    var tickers = GetAllTickers().Result;
                    m_symbolList = new List<string>();
                    foreach (var s in tickers.Keys)
                    {
                        m_symbolList.Add(s);
                    }
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
            /*var tickers = GetAllTickers();
            foreach (var s in tickers.Keys)
            {
                result[s] = tickers[s] as ZTicker;
            }*/
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            var request = GET<BitZResponse<BitZOrderBook>>(string.Format("{0}/depth?coin={1}", BaseUrl, symbol));
            return request.data as ZCryptoOrderBook;
        }

        /*public Dictionary<string, BitZTicker> GetAllTickers()
        {
            var request = GET<BitZResponse<Dictionary<string, BitZTicker>>>("{0}/tickerall", BaseUrl);
            return request.data;
        }*/

        /*public List<BleutradeSymbolDetail> GetAllTickers()
        {
            var request = GET<BitZResponse<Dictionary<string, BitZSymbolDetail>>>("{0}/tickerall", BaseUrl);
            return request.data;
        }

        public List<BleutradeCurrencyDetail> GetCurrencyDetails()
        {
            var request = GET<BleutradeResponse<List<BleutradeCurrencyDetail>>>("{0}/getcurrencies", BaseUrl);
            return request.result;
        }

        // where symbol like "ETH_BTC" or "ETH_BTC,HTML5_DOGE,DOGE_LTC"
        public BleutradeTicker GetTicker(string symbol)
        {
            var request = GET<BleutradeTicker>(string.Format("{0}/getticker?market={1}", BaseUrl, symbol));
            request.Symbol = symbol;
            return request;
        }*/

        /*public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            var request = GET<OKExOrderBook>(string.Format("{0}/api/v1/depth.do?symbol={1}", BaseUrl, symbol));
            request.Symbol = symbol;
            return request;
        }*/

        /*public List<List<string>> GetOrderBook(string symbol)
        {
            return new OrderBook(GetBitfinexOrderBook(symbol));
        }*/

    } // end of class BitZ

    //======================================================================================================================================

    public class BitZTickerX
    {
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Last { get; set; }
    }

    public class BitZTicker : ZTicker
    {
        public long date { get; set; }                  // timestamp
        public decimal last { get; set; }               // latest price
        public decimal buy { get; set; }                // buy one price
        public decimal sell { get; set; }               // selling univalent
        public decimal high { get; set; }               // highest price
        public decimal low { get; set; }                // minimum price
        public decimal vol { get; set; }                // 24 hour volume

        public override decimal Bid { get { return buy; } }
        public override decimal Ask { get { return sell; } }
        public override decimal Last { get { return last; } }
        public override decimal High { get { return high; } }
        public override decimal Low { get { return low; } }
        public override decimal Volume { get { return vol; } }
        public override string Timestamp { get { return DateTime.Now.ToString(); } }
    } // end of class BitZTicker

    public class BitZCurrencyDetail
    {
        public string Currency { get; set; }            // "BTC"
        public string CurrencyLong { get; set; }        // "Bitcoin"
        public int MinConfirmation { get; set; }        // 2
        public decimal TxFee { get; set; }              // 0.00080000
        public bool IsActive { get; set; }              // true
        public string CoinType { get; set; }            // "BITCOIN"
        public bool MaintenanceMode { get; set; }       // false
    } // end of class BitZCurrencyDetail

    public class BitZSymbolDetail
    {
        
    } // end of class BizZSymbolDetail

    public class BitZOrderBookEntry
    {
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
    }

    public class BitZOrderBook : ZCryptoOrderBook
    {
        public long date { get; set; }
        public List<List<decimal>> bids { get; set; }
        public List<List<decimal>> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("BIDS\n");
            int ix = 0;
            foreach (var b in bids)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, b[0], b[1]));
            }
            sb.Append("ASKS\n");
            ix = 0;
            //revasks = as
            foreach (var a in asks)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, a[0], a[1]));
            }
            return sb.ToString();
        }
    } // end of class BitZOrderBook


} // end of namespace
