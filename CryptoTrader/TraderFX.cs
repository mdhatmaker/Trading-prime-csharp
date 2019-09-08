using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using CryptoApis;
using CryptoTools;
using CryptoTools.Messaging;
using ExchangeSharp;
using IQFeed;

namespace CryptoTrader
{
    public class TraderFX
    {
        ExchangeSharpApi m_api;
        //private List<IDisposable> m_sockets;
        private ProwlPub m_prowl;
        private Credentials m_creds;
        private bool m_notify = false;

        private PriceFeed m_priceFeed;
        //private ConcurrentBag<ExchangeOrderResult> m_orders;
        //private CandlestickMaker m_maker;
        private OrderManager m_om;
        //private List<TradeSymbolRawCsvRecord> m_tradeSymbols;
        //private List<TradeSymbolRawCsvRecord> m_activeTradeSymbols;

        public TraderFX()
        {
            m_api = new ExchangeSharpApi(ExchangeSet.All);
            m_api.LoadCredentials(Credentials.CredentialsFile, Credentials.CredentialsPassword);
            m_creds = Credentials.LoadEncryptedCsv(Credentials.CredentialsFile, Credentials.CredentialsPassword);
            m_prowl = new ProwlPub(m_creds["PROWL"].Key, "Scalper");
            m_om = new OrderManager(m_creds);
            //m_maker = new CandlestickMaker();
            //m_orders = new ConcurrentBag<ExchangeOrderResult>();
            m_priceFeed = PriceFeed.Instance;
        }

        public void Test()
        {
            m_priceFeed.SubscribePrices("@M6E#");
            m_priceFeed.UpdatePrices += M_priceFeed_UpdatePrices;

            var denoms = new HashSet<string>();
            foreach (var eid in m_api.ExchangeIds)
            {
                Console.WriteLine(eid);
                try
                {
                    var symbols = m_api[eid].GetSymbols();
                    foreach (var s in symbols)
                    {
                        try
                        {
                            var gs = GlobalSymbol(eid, s);
                            if (gs.EndsWith("-BTC")) Console.WriteLine("{0,12}  {1,10}  {2,10}", eid, s, gs);
                            //Console.WriteLine("{0,12}  {1,10}  {2,10}", eid, s, gs);
                            var split = gs.Split('-');
                            denoms.Add(split[0]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERROR: {0}", ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0} ERROR: {1}", eid, ex.Message);
                }
                Console.WriteLine(new string('-', 80));
            }
            Console.WriteLine("\nUnique denominations:");
            foreach (var d in denoms.OrderBy(x => x))
            {
                Console.WriteLine(d);
            }
        }

        private void M_priceFeed_UpdatePrices(Tools.PriceUpdateIQ update)
        {
            Console.WriteLine("IQFeed Update:  {0}  {1}:{2}-{3}:{4}", update.Symbol, update.BidSize, update.Bid, update.Ask, update.AskSize);
        }

        public string GlobalSymbol(string exchangeId, string symbol)
        {
            if (exchangeId == "BITFINEX")
            {
                //var sp = symbol.Split("-");
                var len = symbol.Length;
                symbol = symbol.Substring(len - 3) + "-" + symbol.Substring(0, len - 3);
            }
            else if (exchangeId == "BITSTAMP")
            {
                var len = symbol.Length;
                symbol = symbol.Substring(len - 3) + "-" + symbol.Substring(0, len - 3);
            }

            var gsymbol = m_api[exchangeId].ExchangeSymbolToGlobalSymbol(symbol);
            var split = gsymbol.Split("-");
            var flipExchanges = new string[] { "YOBIT", "TUX", "LIVECOIN", "HITBTC", "GDAX", "CRYPTOPIA", "BITHUMB", "ABUCOINS" };
            if (flipExchanges.Contains(exchangeId))
            {
                return split[1] + "-" + split[0];
            }
            else
            {
                return gsymbol;
            }
        }
    } // end of class TraderFX
} // end of namespace
