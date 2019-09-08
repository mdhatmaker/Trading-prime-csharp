using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    // http://api.bithumb.com/

    public class Bithumb : BaseExchange
    {
        public override string BaseUrl { get { return "https://api.bithumb.com"; } }
        public override string ExchangeName { get { return "BITHUMB"; } }

        // SINGLETON
        public static Bithumb Instance { get { return m_instance; } }
        private static readonly Bithumb m_instance = new Bithumb();
        private Bithumb() { }

        //private System.Timers.Timer m_tickerTimer;

        private static List<string> currency_pairs = new List<string>()
        {
            "BTC/KRW", "BCH/KRW", "XRP/KRW", "ETH/KRW",
            "QTUM/KRW", "LTC/KRW", "DASH/KRW", "ETC/KRW",
            "XMR/KRW", "ZEC/KRW"
        };

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    m_symbolList = currency_pairs;
                    m_symbolList.Sort();

                    SupportedSymbols = new Dictionary<string, ZCurrencyPair>();
                    foreach (var s in m_symbolList)
                    {
                        var pair = ZCurrencyPair.FromSymbol(s, CryptoExch.BITHUMB);
                        SupportedSymbols[pair.Symbol] = pair;
                    }
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            // https://api.bithumb.com/public/ticker/btc/krw
            string[] substrings = symbol.Split('/');
            //string url = string.Format($"https://api.bithumb.com/public/ticker/{0}/{1}", substrings[0], substrings[1]);
            //string url = $"https://api.bithumb.com/public/ticker/" + substrings[0] + $"/" + substrings[1];
            string url = $"https://api.bithumb.com/public/ticker/" + symbol;
            return GET<BithumbTicker>(url);
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
            var book = GET<BithumbOrderBook>("https://api.bithumb.com/public/orderbook");
            return book as ZCryptoOrderBook;
        }

        /*public Dictionary<string, ZTicker> GetTickers(List<string> symbols = null)
        {
            var result = new Dictionary<string, ZTicker>();
            if (symbols == null) symbols = GetSymbolList();         // passing null gets ALL tickers
            //List<string> errors;
            Dictionary<string, KrakenTicker> tickers = GetTickers(symbols, out List<string> errors);
            foreach (var k in tickers.Keys)
            {
                result[k] = tickers[k];
            }
            return result;
        }*/


        public BithumbTransactions GetBithumbRecentTransactions()
        {
            return GET<BithumbTransactions>("https://api.bithumb.com/public/recent_transactions");
        }

    } // end of class Bithumb

    //======================================================================================================================================

    public class BithumbTicker : ZTicker
    {
        public string status { get; set; }
        public BithumbTickerData data { get; set;}

        public override decimal Bid { get { return this.data.buy_price; } }
        public override decimal Ask { get { return this.data.sell_price; } }
        public override decimal Last { get { return this.data.closing_price; } }
        public override decimal High { get { return this.data.max_price; } }
        public override decimal Low { get { return this.data.min_price; } }
        public override decimal Volume { get { return this.data.units_traded; } }
        public override string Timestamp { get { return this.data.date.ToString(); } }  

        public BithumbTicker()
        {
            this.data = new BithumbTickerData();
        }
    } // end of class BithumbTicker

    public class BithumbTickerData
    {
        /*public string opening_price { get; set; }
        public string closing_price { get; set; }
        public string min_price { get; set; }
        public string max_price { get; set; }
        public string average_price { get; set; }
        public string units_traded { get; set; }
        public string volume_1day { get; set; }
        public string volume_7day { get; set; }
        public string buy_price { get; set; }
        public string sell_price { get; set; }
        public string date { get; set; }*/

        public decimal opening_price { get; set; }
        public decimal closing_price { get; set; }
        public decimal min_price { get; set; }
        public decimal max_price { get; set; }
        public decimal average_price { get; set; }
        public decimal units_traded { get; set; }
        public decimal volume_1day { get; set; }
        public decimal volume_7day { get; set; }
        public decimal buy_price { get; set; }
        public decimal sell_price { get; set; }
        public long date { get; set; }

    } // end of class BithumbTickerData

    public class BithumbOrderBook : ZCryptoOrderBook
    {
        public string status { get; set; }
        public BithumbOrderBookData data { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get { return data.bids.Cast<ZCryptoOrderBookEntry>().ToList(); } }
        public override List<ZCryptoOrderBookEntry> Asks { get { return data.asks.Cast<ZCryptoOrderBookEntry>().ToList(); } }
    } // end of class BithumbOrderbook

    public class BithumbTransactions
    {
        public string status { get; set; }
        public List<BithumbTransaction> data { get; set; }
    }

    public class BithumbOrderBookEntry : ZCryptoOrderBookEntry
    {
        public decimal quantity { get; set; }
        public decimal price { get; set; }

        public override decimal Price { get { return price; } }
        public override decimal Amount { get { return quantity; } }

        public override string ToString()
        {
            return string.Format("qty:{0}  price:{1}", quantity, price);
        }
    } // end of class BithumbOrderBookEntry

    public class BithumbOrderBookData
    {
        public string timestamp { get; set; }
        public string payment_currency { get; set; }
        public string order_currency { get; set; }
        public List<BithumbOrderBookEntry> bids { get; set; }
        public List<BithumbOrderBookEntry> asks { get; set; }

        public override string ToString()
        {
            return string.Format("timestamp:{0}  payment_currency:{1} order_currency:{2}\nbids:{3}\nasks:{4}", timestamp, payment_currency, order_currency, string.Join("; ", bids), string.Join("; ", asks));
        }
    } // end of class BithumbOrderBookData

    public class BithumbTransaction
    {
        public string transaction_date { get; set; }    //"2017-10-26 3:46:52"
        public string type { get; set; }                //"bid"
        public string units_traded { get; set; }        //"0.1801399"
        public string price { get; set; }               //"6433000"
        public string total { get; set; }               //"1158840"

        public override string ToString()
        {
            return string.Format("{0}  [{1}]  units_traded:{2,12:0.00000000}   price:{3,10}   total:{4,10:n0}", transaction_date, type, float.Parse(units_traded), price, int.Parse(total));
        }
    } // end of class BithumbTransaction

    


} // end of namespace
