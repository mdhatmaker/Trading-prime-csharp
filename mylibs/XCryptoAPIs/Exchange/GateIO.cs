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
    public class GateIO : BaseExchange
    {
        public override string BaseUrl { get { return "http://data.gate.io/api2/1"; } }
        public override string ExchangeName { get { return "GATEIO"; } }

        // SINGLETON
        public static GateIO Instance { get { return m_instance; } }
        private static readonly GateIO m_instance = new GateIO();
        private GateIO() { }

        private static readonly HttpClient m_httpClient = new HttpClient();

        private Dictionary<string, int> m_channelIds = new Dictionary<string, int>();


        public override List<string> SymbolList
        {
            get
            {

                if (m_symbolList == null)
                {
                    m_symbolList = GET<List<string>>("{0}/pairs", BaseUrl);
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            var request = GET<GateIOTicker>(string.Format("{0}/api/v1/ticker.do?symbol={1}", BaseUrl, symbol));
            request.Symbol = symbol;
            return request;
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
            // where group like (number) and limit like (number)
            var request = GET<GateIOOrderBook>(string.Format("{0}/open/orders?symbol={1}", BaseUrl, symbol));
            return request as ZCryptoOrderBook;
        }

        public List<GateIOSymbolDetail> GetSymbolDetails()
        {
            var request = GET<List<GateIOSymbolDetail>>("{0}/market/open/symbols", BaseUrl);
            return request;
        }

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



    } // end of class Bitfinex

    //======================================================================================================================================

    public class GateIOTickerX
    {
        public string last { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string vol { get; set; }
        public string buy { get; set; }
        public string sell { get; set; }
    }

    public class GateIOTicker : ZTicker
    {
        public string date { get; set; }
        public OkExTickerX ticker { get; set; }

        public override decimal Bid { get { return decimal.Parse(ticker.buy); } }
        public override decimal Ask { get { return decimal.Parse(ticker.sell); } }
        public override decimal Last { get { return decimal.Parse(ticker.last); } }
        public override decimal High { get { return decimal.Parse(ticker.high); } }
        public override decimal Low { get { return decimal.Parse(ticker.low); } }
        public override decimal Volume { get { return decimal.Parse(ticker.vol); } }
        public override string Timestamp { get { return DateTime.Now.ToString(); } }
    } // end of class GateIOTicker

    public class GateIOSymbolDetail
    {
        public string coinType { get; set; }                // "KCS"
        public bool trading { get; set; }                   // true
        public string symbol { get; set; }                  // "KCS-BCH"
        public decimal lastDealPrice { get; set; }          // 0.005997
        public decimal buy { get; set; }                    // 0.0057141
        public decimal sell { get; set; }                   // 0.00599697
        public decimal change { get; set; }                 // 0.00019731
        public string coinTypePair { get; set; }            // "BCH"
        public int sort { get; set; }                       // 0
        public decimal feeRate { get; set; }                // 0.001
        public decimal volValue { get; set; }               // 131.92643462
        public decimal high { get; set; }                   // 0.006
        public long datetime { get; set; }                  // 1517634928000
        public decimal vol { get; set; }                    // 24369.0449
        public decimal low { get; set; }                    // 0.00513137
        public decimal changeRate { get; set; }             // 0.034
    } // end of class KucoinSymbolDetail

    // arr[0]->Price    arr[1]->Amount  arr[2]->Volume
    public class GateIOOrderBook : ZCryptoOrderBook
    {
        //public List<KucoinOrderBookEntry> asks { get; set; }
        //public List<KucoinOrderBookEntry> bids { get; set; }
        public List<List<decimal>> SELL { get; set; }
        public List<List<decimal>> BUY { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("BIDS\n");
            int ix = 0;
            foreach (var b in BUY)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, b[0], b[1], b[2]));
            }
            sb.Append("ASKS\n");
            ix = 0;
            //revasks = as
            foreach (var a in SELL)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, a[0], a[1], a[2]));
            }
            return sb.ToString();
        }
    } // end of class GateIOOrderBook


} // end of namespace
