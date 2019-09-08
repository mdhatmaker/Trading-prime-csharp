using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    // https://www.btcc.com/apidocs/btcc-api-documentations

    public class BTCC : BaseExchange
    {
        public override string BaseUrl { get { return "https://spotusd-data.btcc.com/data/pro"; } }
        public override string ExchangeName { get { return "BTCC"; } }

        // SINGLETON
        public static BTCC Instance { get { return m_instance; } }
        private static readonly BTCC m_instance = new BTCC();
        private BTCC() { }

        public int Limit = 100;

        private static string[] currency_pairs =
        {
            "BTCUSD",
        };

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    m_symbolList = new List<string>(currency_pairs);
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
            var symbols = SymbolList;
            foreach (var s in symbols)
            {
                result[s] = GetTicker(s);
            }
            return result;
        }

        // symbol like BTCUSD
        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            string url = BaseUrl + "/orderbook?symbol={0}&limit={1}";
            var book = GET<BTCCOrderBook>(url, symbol, this.Limit);
            return book as ZCryptoOrderBook;
        }

        public BTCCTicker GetTicker(string symbol, int limit = 100)
        {
            string url = BaseUrl + "/ticker?symbol={0}";
            var ticker = GET<BTCCTicker>(url, symbol);
            return ticker;
        }

       public List<BTCCTrade> GetTradeHistory(string symbol, int limit=100)
        {
            string url = BaseUrl + "/historydata?symbol={0}&limit={1}";
            var trades = GET<List<BTCCTrade>>(url, symbol, limit);
            return trades;
        }

        /*public static List<string> GetSymbolList()
        {
            List<string> result = new List<string>();
            var products = GetProducts();
            foreach (var p in products)
            {
                result.Add(p.id);
            }
            return result;
        }*/

    } // end of class BTCC

    //======================================================================================================================================

    public class BTCCOrderBook : ZCryptoOrderBook
    {
        public List<List<float>> asks { get; set; }
        public List<List<float>> bids { get; set; }
        public long date { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }
    } // end of class BTCCOrderBook

    public class BTCCTicker : ZTicker
    {
        public BTCCTickerData ticker { get; set; }

        public override decimal Bid { get { return decimal.Parse("0"); } }
        public override decimal Ask { get { return decimal.Parse("0"); } }
        public override decimal Last { get { return (decimal)ticker.Last; } }
        public override decimal High { get { return (decimal)ticker.High; } }
        public override decimal Low { get { return (decimal)ticker.Low; } }
        public override decimal Volume { get { return (decimal)ticker.Volume; } }
        public override string Timestamp { get { return ticker.Timestamp.ToString(); } }  

        public BTCCTicker()
        {
            ticker = new BTCCTickerData();
        }

        /*public override string ToString()
        {
            return string.Format("[BTCCTicker: {0}]", ticker);
        }*/
    } // end of class BTCCTicker

    public class BTCCTickerData
    {
        public float BidPrice { get; set; }
        public float AskPrice { get; set; }
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Last { get; set; }
        public float LastQuantity { get; set; }
        public float PrevCls { get; set; }
        public float Volume { get; set; }
        public float Volume24H { get; set; }
        public long Timestamp { get; set; }
        public float ExecutionLimitDown { get; set; }
        public float ExecutionLimitUp { get; set; }

        /*public override string ToString()
        {
            return string.Format("BidPrice={0}, AskPrice={1}, Open={2}, High={3}, Low={4}, Last={5}, LastQuantity={6}, PrevCls={7}, Volume={8}, Volume24H={9}, Timestamp={10}, ExecutionLimitDown={11}, ExecutionLimitUp={12}", BidPrice, AskPrice, Open, High, Low, Last, LastQuantity, PrevCls, Volume, Volume24H, Timestamp, ExecutionLimitDown, ExecutionLimitUp);
        }*/
    } // end of class BTCCTickerData

    public class BTCCTrade
    {
        public string Id { get; set; }
        public long Timestamp { get; set; }
        public float Price { get; set; }
        public float Quantity { get; set; }
        public string Side { get; set; }

        public override string ToString()
        {
            return string.Format("[BTCCTrade: Id={0}, Timestamp={1}, Price={2}, Quantity={3}, Side={4}]", Id, Timestamp, Price, Quantity, Side);
        }
    } // end of class BTCCTrade
} // end of namespace
