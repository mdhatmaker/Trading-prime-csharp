using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    // https://market.bitsquare.io/api/

    public class Bitsquare : BaseExchange
    {
        public override string BaseUrl { get { return "https://market.bisq.io/api"; } }
        public override string ExchangeName { get { return "BITSQUARE"; } }

        // SINGLETON
        public static Bitsquare Instance { get { return m_instance; } }
        private static readonly Bitsquare m_instance = new Bitsquare();
        private Bitsquare() { }


        // "btc_usd", "btc_eur", etc...
        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    var markets = GetMarkets();
                    m_symbolList = markets.Keys.ToList();
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

        public override ZTicker GetTicker(string marketSymbol)
        {
            var list = GetJsonList<BitsquareTicker>(string.Format("https://market.bisq.io/api/ticker?market={0}&format=json", marketSymbol));
            return list[0];
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var allTickers = GetAllBitsquareTickers();
            foreach (string s in allTickers.Keys)
            {
                if (allTickers[s] != null)          // ignore tickers with null value (the GetAllTickers call returns many more symbols than are apparently active)
                {
                    result[s] = allTickers[s];
                }
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string marketSymbol)
        {
            var dict = GetJsonDictionary<BitsquareOrderBook>(string.Format("https://market.bisq.io/api/offers?market={0}&format=json", marketSymbol));
            return dict[marketSymbol] as ZCryptoOrderBook;
        }

        public Dictionary<string, BitsquareMarket> GetMarkets()
        {
            return GetJsonDictionary<BitsquareMarket>("https://market.bisq.io/api/markets");
        }

        public Dictionary<string, BitsquareTicker> GetAllBitsquareTickers()
        {
            var res = GET<Dictionary<string, BitsquareTicker>>("https://market.bisq.io/api/ticker");
            return res;
        }

        public List<BitsquareHLOC> GetHLOC(string marketSymbol)
        {
            var list = GetJsonList<BitsquareHLOC>(string.Format("https://market.bisq.io/api/hloc?market={0}&format=json", marketSymbol));
            return list;
        }

        public BitsquareDepth GetDepth(string marketSymbol)
        {
            var dict = GetJsonDictionary<BitsquareDepth>(string.Format("https://market.bisq.io/api/depth?market={0}&format=json", marketSymbol));
            return dict[marketSymbol];
        }

        

        /*public static BitsquareDepth GetBuys(string marketSymbol)
        {
            var offers = GetJsonDictionary<BitsquareDepth>(string.Format("https://market.bisq.io/api/offers?market={0}&direction=BUY", marketSymbol));
            return offers[marketSymbol];
        }

        public static BitsquareDepth GetSells(string marketSymbol)
        {
            var offers = GetJsonDictionary<BitsquareDepth>(string.Format("https://market.bisq.io/api/offers?market={0}&direction=SELL", marketSymbol));
            return offers[marketSymbol];
        }*/



    } // end of class Bitsquare

    //======================================================================================================================================

    public class BitsquareMarket
    {
        public string pair { get; set; }
        public string lname { get; set; }
        public string rname { get; set; }
        public string lsymbol { get; set; }
        public string rsymbol { get; set; }
        public int lprecision { get; set; }
        public int rprecision { get; set; }
        public string ltype { get; set; }
        public string rtype { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return "BitsquareMarket::" + Str(this);
        }
    } // end of class BitsquareMarket

    public class BitsquareTicker : ZTicker
    {
        public string last { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string volume_left { get; set; }
        public string volume_right { get; set; }
        public string buy { get; set; }
        public string sell { get; set; }

        public override decimal Bid { get { return decimal.Parse(buy ?? "0"); } }
        public override decimal Ask { get { return decimal.Parse(sell ?? "0"); } }
        public override decimal Last { get { return decimal.Parse(last ?? "0"); } }
        public override decimal High { get { return decimal.Parse(high ?? "0"); } }
        public override decimal Low { get { return decimal.Parse(low ?? "0"); } }
        public override decimal Volume { get { return decimal.Parse(volume_left ?? "0"); } }
        public override string Timestamp { get { return DateTime.Now.ToUnixTimestamp().ToString(); } }

        public override string ToString()
        {
            return "BitsquareTicker::" + Str(this);
        }
    } // end of class BitsquareTicker

    public class BitsquareHLOC
    {
        public long period_start { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string close { get; set; }
        public string volume_left { get; set; }
        public string volume_right { get; set; }
        public string avg { get; set; }

        public override string ToString()
        {
            return "BitsquareHLOC::" + Str(this);
        }
    } // end of class BitsquareHLOC

    public class BitsquareTrade
    {
        public string direction { get; set; }
        public string price { get; set; }
        public string amount { get; set; }
        public string trade_id { get; set; }
        public long trade_date { get; set; }

        public override string ToString()
        {
            return "BitsquareTrade::" + Str(this);
        }
    } // end of class BitsquareTrade

    public class BitsquareOrderBookEntry : ZCryptoOrderBookEntry
    {
        public string offer_id { get; set; }
        public long offer_date { get; set; }
        public string direction { get; set; }
        public string min_amount { get; set; }
        public decimal amount { get; set; }
        public decimal price { get; set; }
        public string volume { get; set; }
        public string payment_method { get; set; }
        public string offer_fee_txid { get; set; }

        public override decimal Price { get { return price; } }
        public override decimal Amount { get { return amount; } }
        public override string Timestamp { get { return offer_date.ToString(); } }
    } // end of class BitsquareOrderBookEntry

    public class BitsquareOrderBook : ZCryptoOrderBook
    {
        public List<BitsquareOrderBookEntry> buys { get; set; }
        public List<BitsquareOrderBookEntry> sells { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get { return buys.Cast<ZCryptoOrderBookEntry>().ToList(); } }
        public override List<ZCryptoOrderBookEntry> Asks { get { return sells.Cast<ZCryptoOrderBookEntry>().ToList(); } }
        //public int BuyCount { get { return buys == null ? 0 : buys.Count; } }
        //public int SellCount { get { return sells == null ? 0 : sells.Count; } }
    } // end of class BitsquareOrderBook

    public class BitsquareDepth
    {
        public List<string> buys { get; set; }
        public List<string> sells { get; set; }

        public int BuyCount { get { return buys == null ? 0 : buys.Count; } }
        public int SellCount { get { return sells == null ? 0 : sells.Count; } }

        public override string ToString()
        {
            return "BitsquareDepth::" + Str(this);
        }
    } // end of class BitsquareDepth


} // end of NAMESPACE
