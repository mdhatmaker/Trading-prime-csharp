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
    // https://github.com/chbtc/API/wiki/rest_api_行情

    public class CHBTC : BaseExchange
    {
        public override string BaseUrl { get { return "http://api.chbtc.com/data/v1"; } }
        public override string ExchangeName { get { return "CHBTC"; } }

        // SINGLETON
        public static CHBTC Instance { get { return m_instance; } }
        private static readonly CHBTC m_instance = new CHBTC();
        private CHBTC() { }

        private static readonly HttpClient m_httpClient = new HttpClient();

        private Dictionary<string, int> m_channelIds = new Dictionary<string, int>();

        /*class CHBTCResponse<T>
        {
            public int code { get; set; }       // status code (ex: 0)
            public string msg { get; set; }     // message (ex: "Success")
            public T data { get; set; }
        }*/

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    m_symbolList = new List<string> { "btc_cny", "ltc_cny", "eth_cny", "etc_cny", "bts_cny", "eos_cny", "bcc_cny", "qtum_cny" };
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            var request = GET<CHBTCTicker>(string.Format("{0}/ticker?currency={1}", BaseUrl, symbol));
            return request;
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            foreach (var s in SymbolList)
            {
                result[s] = GetTicker(s);
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            // where size 1-50
            int size = 3;
            // where merge [btc_cny:1,0.1],[ltc_cny:0.5,0.3,0.1],[eth_cny:0.5,0.3,0.1],[etc_cny:0.3,0.1],[bts_cny:1,0.1,0.01],[eos_cny:1,0.1,0.01],[bcc_cny:1,0.1,0.01],[qtum_cny:1,0.1,0.01]
            decimal merge = 0.1m;
            var request = GET<CHBTCOrderBook>(string.Format("{0}/depth?currency={1}&size={2}&merge={3}", BaseUrl, symbol, size, merge));
            return request as ZCryptoOrderBook;
        }

        public CHBTCKlines GetKlines(string symbol)
        {
            var request = GET<CHBTCKlines>(string.Format("{0}/kline?currency={1}", BaseUrl, symbol));
            return request;
        }

    } // end of class CHBTC

    //======================================================================================================================================

    public class CHBTCTickerX
    {
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal buy { get; set; }
        public decimal sell { get; set; }
        public decimal last { get; set; }
        public decimal vol { get; set; }
    }

    public class CHBTCTicker : ZTicker
    {
        public CHBTCTickerX ticker { get; set; }

        public override decimal Bid { get { return ticker.buy; } }
        public override decimal Ask { get { return ticker.sell; } }
        public override decimal Last { get { return ticker.last; } }
        public override decimal High { get { return ticker.high; } }
        public override decimal Low { get { return ticker.low; } }
        public override decimal Volume { get { return ticker.vol; } }
        public override string Timestamp { get { return DateTime.Now.ToString(); } }
    } // end of class CHBTCTicker

    public class CHBTCTrade //: ZTrade
    {
        public long date { get; set; }              // 1472711925
        public decimal amount { get; set; }         // 0.541
        public decimal price { get; set; }          // 81.87
        public long tid { get; set; }               // 16497097
        public string trade_type { get; set; }      // "ask"
        public string type { get; set; }            // "sell"

        /*// ICryptoTrade interface implementation
        public override string Price { get { return price.ToString(); } }
        public override string Amount { get { return amount.ToString(); } }
        public override string TradeId { get { return tid.ToString(); } }
        public override string TradeType { get { return trade_type.ToString(); } }
        public override string Type { get { return type.ToString(); } }
        public override string Timestamp { get { return date.ToString(); } }*/
    }

    public class CHBTCKline : ZKline
    {
        public List<decimal> dataX { get; set; }

        public override long Timestamp { get { return (long)dataX[0]; }}
        public override decimal Open { get { return dataX[1]; }}
        public override decimal High { get { return dataX[2]; }}
        public override decimal Low { get { return dataX[3]; }}
        public override decimal Close { get { return dataX[4]; }}
        public override decimal Volume { get { return dataX[5]; }}
    }

    public class CHBTCKlines
    {
        public List<CHBTCKline> data { get; set; }
        public string moneyType { get; set; }           // "cny"
        public string symbol { get; set; }              // "btc"
    }

    public class CHBTCOrderBook : ZCryptoOrderBook
    {
        public long timestamp { get; set; }
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
    } // end of class CHBTCOrderBook


} // end of namespace
