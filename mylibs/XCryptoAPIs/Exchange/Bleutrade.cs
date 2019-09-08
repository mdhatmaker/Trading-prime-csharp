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
    public class Bleutrade : BaseExchange
    {
        public override string BaseUrl { get { return "https://bleutrade.com/api/v2/public"; } }
        public override string ExchangeName { get { return "BLEUTRADE"; } }

        //BleuTradeClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static Bleutrade m_instance;
        public static Bleutrade Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static Bleutrade Create(string apikey = null, string apisecret = null)
        {
            if (m_instance != null)
                return m_instance;
            else
                return new Bleutrade(apikey, apisecret);
        }
        private Bleutrade(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            //var apiclient = new Exchange.Clients.Binance.ApiClient(ApiKey, ApiSecret, "https://www.binance.com", "wss://stream.binance.com:9443/ws/", true);
            //m_api = new Exchange.Clients.Binance.BinanceClient(apiclient, loadTradingRules: false);
            m_instance = this;
        }

        private static readonly HttpClient m_httpClient = new HttpClient();

        private Dictionary<string, int> m_channelIds = new Dictionary<string, int>();

        class BleutradeResponse<T>
        {
            public bool success { get; set; }
            public string message { get; set; }
            public T result { get; set; }
        }

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    var details = GetSymbolDetails();
                    m_symbolList = new List<string>();
                    foreach (var d in details)
                    {
                        m_symbolList.Add(d.MarketName);
                    }
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        // where symbol like "ETH_BTC" or "ETH_BTC,HTML5_DOGE,DOGE_LTC"
        public override ZTicker GetTicker(string symbol)
        {
            var request = GET<BleutradeTicker>(string.Format("{0}/getticker?market={1}", BaseUrl, symbol));
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
            // where depthType like "BUY|SELL|ALL" and depthLimit like 20 (default)
            string depthType = "ALL";
            int depthLimit = 100;
            var request = GET<BleutradeResponse<BleutradeOrderBook>>(string.Format("{0}/getorderbook?market={1}&type={2}&depth={3}", BaseUrl, symbol, depthType, depthLimit));
            return request.result as ZCryptoOrderBook;
        }

        public List<BleutradeSymbolDetail> GetSymbolDetails()
        {
            var request = GET<BleutradeResponse<List<BleutradeSymbolDetail>>>("{0}/getmarkets", BaseUrl);
            return request.result;
        }

        public List<BleutradeCurrencyDetail> GetCurrencyDetails()
        {
            var request = GET<BleutradeResponse<List<BleutradeCurrencyDetail>>>("{0}/getcurrencies", BaseUrl);
            return request.result;
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

    } // end of class Bleutrade

    //======================================================================================================================================

    public class BleutradeTickerX
    {
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Last { get; set; }
    }

    public class BleutradeTicker : ZTicker
    {
        public string date { get; set; }
        public BleutradeTickerX ticker { get; set; }

        public override decimal Bid { get { return ticker != null ? ticker.Bid : 0M; } }
        public override decimal Ask { get { return ticker != null ? ticker.Ask : 0M; } }
        public override decimal Last { get { return ticker != null ? ticker.Last : 0M; } }
        public override decimal High { get { return decimal.Parse("0"); } }
        public override decimal Low { get { return decimal.Parse("0"); } }
        public override decimal Volume { get { return decimal.Parse("0"); } }
        public override string Timestamp { get { return DateTime.Now.ToUnixTimestamp().ToString(); } }  
    } // end of class BleutradeTicker

    public class BleutradeCurrencyDetail
    {
        public string Currency { get; set; }            // "BTC"
        public string CurrencyLong { get; set; }        // "Bitcoin"
        public int MinConfirmation { get; set; }        // 2
        public decimal TxFee { get; set; }              // 0.00080000
        public bool IsActive { get; set; }              // true
        public string CoinType { get; set; }            // "BITCOIN"
        public bool MaintenanceMode { get; set; }       // false
    } // end of class BleutradeCurrencyDetail

    public class BleutradeSymbolDetail
    {
        public string MarketCurrency { get; set; }      // "SLR"
        public string BaseCurrency { get; set; }        // "ETH"
        public string MarketCurrencyLong { get; set; }  // "SolarCoin"
        public string BaseCurrencyLong { get; set; }    // "Ethereum"
        public decimal MinTradeSize { get; set; }       // "0.00001000"
        public string MarketName { get; set; }          // "SLR_ETH"
        public bool IsActive { get; set; }              // "true"
    } // end of class BleutradeSymbolDetail

    public class BleutradeOrderBookEntry
    {
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
    }

    public class BleutradeOrderBook : ZCryptoOrderBook
    {
        public List<BleutradeOrderBookEntry> buy { get; set; }
        public List<BleutradeOrderBookEntry> sell { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("BIDS\n");
            int ix = 0;
            foreach (var b in buy)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, b.Rate, b.Quantity));
            }
            sb.Append("ASKS\n");
            ix = 0;
            //revasks = as
            foreach (var a in sell)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, a.Rate, a.Quantity));
            }
            return sb.ToString();
        }
    } // end of class BleutradeOrderBook


} // end of namespace
