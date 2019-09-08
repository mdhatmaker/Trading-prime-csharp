using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tools;
using static Tools.G;
using Newtonsoft.Json.Linq;
using CryptoAPIs.Exchange.Clients.HitBTC;

namespace CryptoAPIs.Exchange
{
    public partial class HitBTC : BaseExchange, IExchangeWebSocket
    {
        public override string BaseUrl { get { return "https://api.hitbtc.com/api/2"; } }
        public override string Name { get { return "HITBTC"; } }
        public override CryptoExch Exch => CryptoExch.HITBTC;

        HitBtcApi m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static HitBTC m_instance;
        public static HitBTC Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static HitBTC Create(string apikey = "", string apisecret = "")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new HitBTC(apikey, apisecret);
        }
        private HitBTC(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            //var auth = new Authenticator(ApiKey, ApiSecret);
            m_api = new HitBtcApi();
            m_instance = this;
        }


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

                    var markets = GET<List<Market>>("https://api.hitbtc.com/api/2/public/symbol");
                    m_symbolList = new List<string>();
                    foreach (var m in markets)
                    {
                        m_symbolList.Add(m.id);
                    }
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            return GetAllTickers().Result[symbol];
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var tickers = GetHitBTCTickers();
            foreach (var t in tickers)
            {
                result[t.symbol] = t;
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            var request = GET<ExchangeOrderBook>(string.Format("{0}/public/orderbook/{1}", BaseUrl, symbol));
            return request as ZCryptoOrderBook;
        }

        // Return the actual list of available currencies, tokens, ICO, etc.
        public Dictionary<string, Currency> GetCurrencies()
        {
            var currencyList = GET<List<Currency>>("https://api.hitbtc.com/api/2/public/currency");
            var result = new Dictionary<string, Currency>();
            foreach (var c in currencyList)
            {
                result[c.id] = c;
            }
            return result;
        }

        public List<Ticker> GetHitBTCTickers()
        {
            return GET<List<Ticker>>("https://api.hitbtc.com/api/2/public/ticker");            
        }


        public override void SubscribeOrderBookUpdates(ZCurrencyPair pair, bool subscribe)
        {
            StartWebSocket();
            SubscribeWebSocket();
        }

        public override void SubscribeOrderUpdates(ZCurrencyPair pair, bool subscribe)
        {
            if (subscribe)
                m_subscribedPairs.Add(pair);
            else
                m_subscribedPairs.Remove(pair);
        }

        /*public override void SubscribeTickerUpdates(ZCurrencyPair pair, bool subscribe)
        {
            SubscribeChannel("ticker", pair.BinanceSymbol);
        }*/

        //---------------------------------------------------------------------------------------------------------------------------------

        public class Market
        {
            //"id":"BTCUSD","baseCurrency":"BTC","quoteCurrency":"USD","quantityIncrement":"0.01","tickSize":"0.01",
            //"takeLiquidityRate":"0.001","provideLiquidityRate":"-0.0001","feeCurrency":"USD"
            public string id { get; set; }
            public string baseCurrency { get; set; }
            public string quoteCurrency { get; set; }
            public decimal quantityIncrement { get; set; }
            public decimal tickSize { get; set; }
            public decimal takeLiquidityRate { get; set; }
            public decimal provideLiquidityRate { get; set; }
            public string feeCurrency { get; set; }
        }

        public class Ticker : ZTicker
        {
            public string ask { get; set; }
            public string bid { get; set; }
            public string last { get; set; }
            public string open { get; set; }
            public string low { get; set; }
            public string high { get; set; }
            public string volume { get; set; }         // total trading amount within 24 hours in base currency
            public string volumeQuote { get; set; }    // total trading amount within 24 hours in quote currency
            public string timestamp { get; set; }
            public string symbol { get; set; }

            public override decimal Bid { get { return decimal.Parse(bid ?? "0"); } }
            public override decimal Ask { get { return decimal.Parse(ask ?? "0"); } }
            public override decimal Last { get { return decimal.Parse(last ?? "0"); } }
            public override decimal High { get { if (string.IsNullOrEmpty(high)) return 0; else return decimal.Parse(high ?? "0"); } }
            public override decimal Low { get { if (string.IsNullOrEmpty(low)) return 0; else return decimal.Parse(low ?? "0"); } }
            public override decimal Volume { get { return decimal.Parse(volume ?? "0"); } }
            public override string Timestamp { get { return timestamp; } }

            /*public decimal ask { get; set; }
            public decimal bid { get; set; }
            public decimal last { get; set; }
            public decimal open { get; set; }
            public decimal low { get; set; }
            public decimal high { get; set; }
            public decimal volume { get; set; }         // total trading amount within 24 hours in base currency
            public decimal volumeQuote { get; set; }    // total trading amount within 24 hours in quote currency
            public DateTime timestamp { get; set; }
            public string symbol { get; set; }

            // ZTicker abstract class implementation
            public override string Bid { get { return bid.ToString(); } }
            public override string Ask { get { return ask.ToString(); } }
            public override string Last { get { return last.ToString(); } }
            public override string High { get { return high.ToString(); } }
            public override string Low { get { return low.ToString(); } }
            public override string Volume { get { return volume.ToString(); } }
            public override string Timestamp { get { return timestamp.ToString(); } }*/
        }

        public class OrderBookEntry
        {
            public decimal price { get; set; }
            public decimal size { get; set; }
        }

        public class ExchangeOrderBook : ZCryptoOrderBook
        {
            public List<OrderBookEntry> bid { get; set; }
            public List<OrderBookEntry> ask { get; set; }

            public override List<ZCryptoOrderBookEntry> Bids { get; }
            public override List<ZCryptoOrderBookEntry> Asks { get; }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("BIDS\n");
                int ix = 0;
                foreach (var b in bid)
                {
                    sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, b.price, b.size));
                }
                sb.Append("ASKS\n");
                ix = 0;
                //revasks = as
                foreach (var a in ask)
                {
                    sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, a.price, a.size));
                }
                return sb.ToString();
            }
        } // end of class ExchangeOrderBook

        public class Currency
        {
            public string id { get; set; }                      // Currency identifier. In the future, the description will simply use the currency
            public string fullName { get; set; }                // Currency full name
            public bool crypto { get; set; }                    // Is currency belongs to blockchain (false for ICO and fiat, like EUR)
            public bool payinEnabled { get; set; }              // Is allowed for deposit (false for ICO)
            public bool payinPaymentId { get; set; }            // Is required to provide additional information other than the address for deposit
            public bool payinConfirmations { get; set; }        // Blocks confirmations count for deposit
            public bool payoutEnabled { get; set; }             // is allowed for withdraw (false for ICO)
            public bool payoutIsPaymentId { get; set; }         // Is allowed to provide additional information for withdraw
            public bool transferEnabled { get; set; }           // Is allowed to transfer between trading and account (may be disabled on maintain)
        }


    } // end of class HitBTC

} // end of namespace
