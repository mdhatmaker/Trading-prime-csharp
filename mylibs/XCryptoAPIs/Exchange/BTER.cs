using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools;
using static Tools.G;

//using JSON;

namespace CryptoAPIs.Exchange
{
    // https://bter.com/api

    public class BTER : BaseExchange
    {
        public override string BaseUrl { get { return "https://data.bter.com"; } }
        public override string ExchangeName { get { return "BTER"; } }

        // SINGLETON
        public static BTER Instance { get { return m_instance; } }
        private static readonly BTER m_instance = new BTER();
        private BTER() { }

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    m_symbolList = GET<List<string>>("http://data.bter.com/api/1/pairs");
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            return GET<BTERTicker>("http://data.bter.com/api/1/ticker/{0}", symbol);
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
            //http://data.bter.com/api/1/depth/btc_cny
            return GET<BTERDepth>("http://data.bter.com/api/1/depth/{0}", symbol);
        }

    } // end of class

    //======================================================================================================================================

    public class BTERTicker : ZTicker
    {
        public string result { get; set; }
        public decimal last { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal avg { get; set; }
        public decimal sell { get; set; }
        public decimal buy { get; set; }
        public decimal vol_btc { get; set; }
        public decimal vol_cny { get; set; }
        public string rate_change_percentage { get; set; }

        public override decimal Bid { get { return buy; } }
        public override decimal Ask { get { return sell; } }
        public override decimal Last { get { return last; } }
        public override decimal High { get { return high; } }
        public override decimal Low { get { return low; } }
        public override decimal Volume { get { return this.vol_btc; } }
        public override string Timestamp { get { return DateTime.Now.ToString(); } }

    } // end of class BTERTicker

    public class BTERDepthEntry : ZCryptoOrderBookEntry
    {
        //public override string Price { get; private set; }
        //public override string Amount { get; private set; }

    }

    public class BTERDepth : ZCryptoOrderBook
    {
        public string result { get; set; }
        public List<List<string>> asks { get; set; }
        public List<List<string>> bids { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }
    }


} // end of namespace 
