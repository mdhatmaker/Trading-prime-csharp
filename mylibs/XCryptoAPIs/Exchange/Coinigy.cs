using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    // Coinigy Exchange Codes: https://www.coinigy.com/bitcoin-exchanges/

    public class Coinigy : BaseExchange
    {
        public override string BaseUrl { get { return "https://api.coinigy.com/api/v1"; } }
        public override string ExchangeName { get { return "COINIGY"; } }
        public override CryptoExch Exch => CryptoExch.COINIGY;

        public string WebsocketUri = "wss://sc-02.coinigy.com/socketcluster/";

        //CoinigyClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static Coinigy m_instance;
        public static Coinigy Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static Coinigy Create(string apikey = "", string apisecret = "")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new Coinigy(apikey, apisecret);
        }
        private Coinigy(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            //m_api = new CoinigyClient(ApiKey, ApiSecret);
            m_instance = this;
        }

        public string ExchangeCode { get; set; }    // set the ExchangeCode (ex: "BINA", "FLYR", "CCEX", ...)

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    var markets = ListMarkets(ExchangeCode);
                    // TODO: maybe store all the market detail for later reference?
                    m_symbolList = new List<string>();
                    foreach (var m in markets)
                    {
                        m_symbolList.Add(m.mkt_name);
                    }
                    m_symbolList.Sort();

                    /*SupportedSymbols = new Dictionary<string, ZCurrencyPair>();
                    foreach (var s in m_symbolList)
                    {
                        var pair = ZCurrencyPair.FromSymbol(s, CryptoExch.GDAX);
                        SupportedSymbols[pair.Symbol] = pair;
                    }*/
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            var res = GetCoinigyTicker(ExchangeCode, symbol);
            return res;
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var symbols = SymbolList;
            foreach (var s in symbols)
            {
                result[s] = GetCoinigyTicker(ExchangeCode, s);
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ListExchanges()
        {
            var result = new List<string>();
            string url = BaseUrl + "/exchanges";
            //string postData = string.Format("{ \"exchange_code\": \"{0}\" }", exchange_code);
            string postData = "";
            string json = JsonWebRequest(url, postData);
            //var markets = GET<CoinigyMarketResponse>(url);
            //return markets.data;
            //return null;
            return result;
        }

        // Where exchange_code like "BINA", "GDAX", "ITBT", ...
        public IEnumerable<CoinigyMarket> ListMarkets(string exchange_code)
        {
            var result = new List<CoinigyMarket>();
            string url = BaseUrl + "/markets";
            string postData = string.Format("{ \"exchange_code\": \"{0}\" }", exchange_code);
            string json = JsonWebRequest(url, postData);
            //var markets = GET<CoinigyMarketResponse>(url);
            //return markets.data;
            //return null;
            return result;
        }

        // Where exchange_code like "GDAX" and exchange_market like "BTC/USD"
        public CoinigyTicker GetCoinigyTicker(string exchange_code, string exchange_market)
        {
            string url = BaseUrl + "/ticker";
            var ticker = GET<CoinigyTickerResponse>(url);
            return ticker.data[0];
        }

        public IEnumerable<string> ListMarkets()
        {
            var result = new List<string>();
            return result;
        }
    } // end of class Coinigy

    public class CoinigyMarketResponse
    {
        public List<CoinigyMarket> data { get; set; }
        public List<string> notifications { get; set; }

        public CoinigyMarketResponse()
        {
            this.data = new List<CoinigyMarket>();
        }
    } // end of class CoinigyMarketResponse

    public class CoinigyMarket
    {
        public string exch_id { get; set; }
        public string exch_name { get; set; }
        public string exch_code { get; set; }
        public string mkt_id { get; set; }
        public string mkt_name { get; set; }
        public string exchmkt_id { get; set; }
    } // end of class CoinigyMarket

    public class CoinigyTickerResponse
    {
        public List<CoinigyTicker> data { get; set; }
        public List<string> notificiations { get; set; }

        public CoinigyTickerResponse()
        {
            this.data = new List<CoinigyTicker>();
        }
    }

    public class CoinigyTicker : ZTicker
    {
        public string exchange { get; set; }
        public string market { get; set; }
        public string last_trade { get; set; }
        public string high_trade { get; set; }
        public string low_trade { get; set; }
        public string current_volume { get; set; }
        public string timestamp { get; set; }
        public string ask { get; set; }
        public string bid { get; set; }

        public override decimal Bid { get { return decimal.Parse(this.bid); } }
        public override decimal Ask { get { return decimal.Parse(this.ask); } }
        public override decimal Last { get { return decimal.Parse(this.last_trade); } }
        public override decimal High { get { return decimal.Parse(this.high_trade); } }
        public override decimal Low { get { return decimal.Parse(this.low_trade); } }
        public override decimal Volume { get { return decimal.Parse(this.current_volume); } }
        public override string Timestamp { get { return this.timestamp; } } 
    } // end of class CoinigyTicker


} // end of namespace
