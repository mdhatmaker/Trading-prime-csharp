using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    // https://www.okcoin.cn/about/rest_api.do
    // https://github.com/OKCoin/rest

    public class OkCoin : BaseExchange
    {
        public override string BaseUrl { get { return "https://www.okcoin.cn/api/v1"; } }
        public override string ExchangeName { get { return "OKCOIN"; } }

        // SINGLETON
        public static OkCoin Instance { get { return m_instance; } }
        private static readonly OkCoin m_instance = new OkCoin();
        private OkCoin() { }


        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    m_symbolList = new List<string>() { "btc_cny", "ltc_cny", "eth_cny", "etc_cny", "bcc_cny" };
                    m_symbolList.Sort();

                    SupportedSymbols = new Dictionary<string, ZCurrencyPair>();
                    foreach (var s in m_symbolList)
                    {
                        var pair = ZCurrencyPair.FromSymbol(s, CryptoExch.GDAX);
                        SupportedSymbols[pair.Symbol] = pair;
                    }
                }
                return m_symbolList;
            }
        }

        // symbol can be btc_cny, ltc_cny, eth_cny, etc_cny, bcc_cny
        public override ZTicker GetTicker(string symbol)
        {
            string url = BaseUrl + "/ticker.do?symbol={0}";
            var ticker = GET<OkCoinTicker>(url, symbol);
            return ticker;
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
            string url = BaseUrl + "/depth.do?symbol={0}";
            var depth = GET<OkCoinMarketDepth>(url, symbol);
            return depth as ZCryptoOrderBook;
        }

        /*public Dictionary<string, OkCoinTicker> GetTickers(List<string> symbols)
        {
            Dictionary<string, OkCoinTicker> tickers = new Dictionary<string, OkCoinTicker>();
            foreach (var s in symbols)
            {
                tickers.Add(s, GetTicker(s));
            }
            return tickers;
        }*/

        public List<OkCoinTrade> GetTrades(string symbol)
        {
            string url = BaseUrl + "/trades.do?symbol={0}";
            var trades = GET<List<OkCoinTrade>>(url, symbol);
            return trades;
        }

        /*
        type
            1min : 1 minute candlestick data
            3min : 3 minutes candlestick data
            5min : 5 minutes candlestick data
            15min : 15 minutes candlestick data
            30min : 30 minutes candlestick data
            1day : 1 day candlestick data
            3day : 3 days candlestick data
            1week : 1 week candlestick data
            1hour : 1 hour candlestick data
            2hour : 2 hours candlestick data
            4hour : 4 hours candlestick data
            6hour : 6 hours candlestick data
            12hour : 12 hours candlestick data
        size
            specify data size to be acquired
        since
            timestamp(eg:1417536000000). data after the timestamp will be returned
        */
        public Dictionary<long, OkCoinCandlestick> GetCandlestickData(string symbol, string type)
        {
            Dictionary<long, OkCoinCandlestick> result = new Dictionary<long, OkCoinCandlestick>();
            string url = BaseUrl + "/kline.do?symbol={0}&type={1}";
            var sticks = GET<List<List<float>>>(url, symbol, type);
            foreach (var v in sticks)
            {
                var stick = new OkCoinCandlestick(v);
                result[stick.timestamp] = stick;
            }
            return result;
        }

    } // end of class OkCoin

    //======================================================================================================================================

    public class OkCoinTicker : ZTicker
    {
        public string date { get; set; }
        public OkCoinTickerData ticker { get; set; }

        public override decimal Bid { get { return decimal.Parse(ticker.buy ?? "0"); } }
        public override decimal Ask { get { return decimal.Parse(ticker.sell ?? "0"); } }
        public override decimal Last { get { return decimal.Parse(ticker.last ?? "0"); } }
        public override decimal High { get { return decimal.Parse(ticker.high ?? "0"); } }
        public override decimal Low { get { return decimal.Parse(ticker.low ?? "0"); } }
        public override decimal Volume { get { return decimal.Parse(ticker.vol ?? "0"); } }
        public override string Timestamp { get { return date; } }

        public OkCoinTicker()
        {
            ticker = new OkCoinTickerData();
        }
        /*public override string ToString()
        {
            return string.Format("[OkCoinTicker: {0}  {1}]", date, ticker);
        }*/
    } // end of class OkCoinTicker

    public class OkCoinTickerData
    {
        public string buy { get; set; }
        public string high { get; set; }
        public string last { get; set; }
        public string low { get; set; }
        public string sell { get; set; }
        public string vol { get; set; }

        /*public override string ToString()
        {
            return string.Format("buy={0}, high={1}, last={2}, low={3}, sell={4}, vol={5}", buy, high, last, low, sell, vol);
        }*/
    } // end of class OkCoinTickerData

    public class OkCoinMarketDepth : ZCryptoOrderBook
    {
        public List<List<float>> bids { get; set; }
        public List<List<float>> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }
    } // end of class OkCoinMarketDepth

    public class OkCoinTrade
    {
        public string date { get; set; }
        public string date_ms { get; set; }
        public float price { get; set; }
        public float amount { get; set; }
        public string tid { get; set; }
        public string type { get; set; }    // "sell" or "buy"
    } // end of class OkCoinTrade

    public class OkCoinCandlestick
    {
        public long timestamp { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public float volume { get; set; }

        public OkCoinCandlestick(List<float> values)
        {
            timestamp = (long)values[0];
            open = values[1];
            high = values[2];
            low = values[3];
            close = values[4];
            volume = values[5];
        }
    } // end of class OkCoinCandlestick

} // end of namespace
