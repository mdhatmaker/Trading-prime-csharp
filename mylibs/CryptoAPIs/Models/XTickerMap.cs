using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CryptoApis.Models
{
    public class XTickerMap
    {
        public IEnumerable<string> Keys { get { return m_tickers.Keys.OrderBy(k => k); } }
        public XTicker this[string symbol] { get { return m_tickers[symbol]; } }

        private Dictionary<string, XTicker> m_tickers = new Dictionary<string, XTicker>();

        // Kraken
        public XTickerMap(Dictionary<string, KrakenCore.Models.TickerInfo> tickers)
        {
            foreach (var kv in tickers)
            {
                m_tickers[kv.Key.ToLower()] = new XTicker(kv.Value);
            }
        }

        // Binance
        public XTickerMap(IEnumerable<Binance.Net.Objects.Spot.MarketData.BinanceBookPrice> prices)
        {
            foreach (var p in prices)
            {
                m_tickers[p.Symbol] = new XTicker(p);
            }
        }



        public void Print(string title = "")
        {
            Console.WriteLine("\n--- {0} ---", title);
            var keys = this.Keys.ToList();
            //keys.Sort();
            foreach (var symbol in keys)
            {
                var t = this[symbol];
                Console.WriteLine(t.ToString());
            }
        }


    } // end of class XTickerMap
} // end of namespace
