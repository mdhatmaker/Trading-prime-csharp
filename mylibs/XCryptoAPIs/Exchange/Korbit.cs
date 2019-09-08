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
    
    // https://apidocs.korbit.co.kr

    public class Korbit : BaseExchange
    {
        public override string BaseUrl { get { return "https://api.korbit.co.kr/v1"; } }
        public override string ExchangeName { get { return "KORBIT"; } }

        // SINGLETON
        public static Korbit Instance { get { return m_instance; } }
        private static readonly Korbit m_instance = new Korbit();
        private Korbit() { }

        //private static ClientWebSocket m_socket;

        public int Level = 1;

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    m_symbolList = new List<string>() { "btc_krw", "etc_krw", "eth_krw", "xrp_krw", "bch_krw" };
                    m_symbolList.Sort();

                    SupportedSymbols = new Dictionary<string, ZCurrencyPair>();
                    foreach (var s in m_symbolList)
                    {
                        var pair = ZCurrencyPair.FromSymbol(s, CryptoExch.KORBIT);
                        SupportedSymbols[pair.Symbol] = pair;
                    }
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
            var symbols = SymbolList;
            foreach (var s in symbols)
            {
                //result[s] = GetTicker(s);
            }
            return result;
        }

        /*
         level
            1 Only the best bid and ask
            2 Top 50 bids and asks (aggregated)
            3 Full order book (non aggregated)
            Levels 1 and 2 are aggregated and return the number of orders at each level.
            Level 3 is non-aggregated and returns the entire order book.
        */
        public override ZCryptoOrderBook GetOrderBook(string productId)
        {
            string url = BaseUrl + "/products/{0}/book";
            var book = GET<GdaxOrderBook>(url, productId);
            return book as ZCryptoOrderBook;
        }

       

      
    } // end of class Korbit

    //======================================================================================================================================

}
