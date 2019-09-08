using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools;
using static Tools.G;
using CryptoAPIs.Exchange.Clients.ItBit;

namespace CryptoAPIs.Exchange
{

    public class ItBit : BaseExchange, IOrderExchange
    {
        public bool EnableLiveOrders { get; set; }

        public override string BaseUrl { get { return "https://api.itbit.com/v1"; } }
        public override string Name { get { return "ITBIT"; } }
        public override CryptoExch Exch => CryptoExch.ITBIT;

        ItbitClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static ItBit m_instance;
        public static ItBit Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static ItBit Create(string apikey = "", string apisecret = "")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new ItBit(apikey, apisecret);
        }
        private ItBit(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            m_api = new ItbitClient(ApiKey, ApiSecret);
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

                    m_symbolList = new List<string>() { "XBTUSD", "XBTSGD", "XBTEUR" };
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;
            }
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var symbols = SymbolList;
            foreach (var s in symbols)
            {
                var ticker = GetTicker(s);
                if (ticker != null)
                    result[s] = ticker;
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            var book = GET<ItBitOrderBook>(string.Format("https://api.itbit.com/v1/markets/{0}/order_book", symbol));
            return book as ZCryptoOrderBook;
            //return GET<ItBitOrderBook>(string.Format("https://api.itbit.com/v1/markets/{0}/order_book", symbol));
        }

        public TickerSymbol GetTickerEnum(string symbol)
        {            
            return (TickerSymbol)Enum.Parse(typeof(TickerSymbol), symbol);
        }

        public override ZTicker GetTicker(string symbol)
        {
            // XBTUSD, XBTSGD, XBTEUR
            //return GET<ItBitTicker>(string.Format("https://api.itbit.com/v1/markets/{0}/ticker", symbol));
            //var res = m_api.GetTickerAsync(GetTickerEnum(symbol)).Result;
            try
            {
                var res = AsyncHelpers.RunSync<Ticker>(() => m_api.GetTickerAsync(GetTickerEnum(symbol)));
                return new ItBitTicker(res);
            }
            catch (Exception ex)
            {
                ErrorMessage("ItBit::GetTicker=> {0}", ex.Message);
                return null;
            }
        }

        public Dictionary<string, ZTicker> GetTickers(List<string> symbols = null)
        {
            if (symbols == null) symbols = SymbolList;         // passing null gets ALL tickers
            var tickers = GetTickers(symbols);
            return tickers;
        }

        /*public static OrderBook GetOrderBook(string symbol)
        {
            var ob = GetItOrderBook(symbol);
            return new OrderBook(ob.bids, ob.asks);
        }*/

        public List<ZTrade> GetRecentTrades(string symbol, int? sinceMatchNumber)
        {
            // XBTUSD, XBTSGD, XBTEUR
            if (sinceMatchNumber == null)
            {
                var res = GET<RecentTradesResponse>("https://api.itbit.com/v1/markets/{0}/trades", symbol);
                return res.recentTrades;
            }
            else
            {
                var res = GET<RecentTradesResponse>("https://api.itbit.com/v1/markets/{0}/trades?since={1}", symbol, sinceMatchNumber.Value);
                return res.recentTrades;
            }
        }

        public void GetWallets()
        {
            Guid userId = Guid.Empty;
            Page page = Page.Create(number: 1, size: 500);
            var wallets = m_api.GetAllWalletsAsync(userId, page).Result;
            foreach (var w in wallets)
            {
                CurrencyCode curcode = CurrencyCode.USD;    // EUR, SGD, XBT
                var b = m_api.GetWalletBalanceAsync(w.Id, curcode).Result;
                var balance = new ZAccountBalance(w.Name, b.AvailableBalance, b.TotalBalance - b.AvailableBalance);
            }
        }

        private bool GetTickerSymbol(string pair, out TickerSymbol tickerSymbol, out CurrencyCode currencyCode)
        {
            bool okay1 = Enum.TryParse<TickerSymbol>(pair, true, out tickerSymbol);
            bool okay2 = Enum.TryParse<CurrencyCode>(pair.Substring(3), true, out currencyCode);
            return okay1 && okay2;
        }


        #region ---------- IOrderExchange IMPLEMENTATION -----------------------------------------------------------------------
        public OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty)
        {
            if (!EnableLiveOrders)
            {
                cout("\nITBIT::SubmitLimitOrder=> '{0}' {1} {2} {3}\n", pair, side, price, qty);
                return null;
            }
            //var ts = TickerSymbol.XBTEUR;    // XBTSGD, XBTUSD
            //var curcode = CurrencyCode.EUR; // SGD, USD, XBT
            TickerSymbol tickerSymbol;
            CurrencyCode currencyCode;
            if (!GetTickerSymbol(pair, out tickerSymbol, out currencyCode))
            {
                ErrorMessage("ItBit::SubmitLimitOrder=> Cannot convert '{0}' to Enums TickerSymbol and CurrencyCode", pair);
                return null;
            }
            NewOrder order;
            if (side == OrderSide.Buy)
                order = NewOrder.Buy(tickerSymbol, currencyCode, qty, price);
            else
                order = NewOrder.Sell(tickerSymbol, currencyCode, qty, price);
            Guid wid = Guid.Empty;
            //var res = m_api.NewOrderAsync(wid, order).Result;
            var res = AsyncHelpers.RunSync<Order>(() => m_api.NewOrderAsync(wid, order));
            return new OrderNew(pair, res);
        }

        public OrderCxl CancelOrder(string pair, string orderId)
        {
            if (!EnableLiveOrders)
            {
                cout("\nITBIT::CancelOrder=> {0} [{1}]\n", pair, orderId);
                return null;
            }
            Guid wid = Guid.Empty;
            var oid = Guid.Parse(orderId);
            var res = m_api.CancelOrderAsync(wid, oid).Result;
            return new OrderCxl(pair, res);
        }

        public IEnumerable<ZOrder> GetWorkingOrders(string pair)
        {
            var result = new List<ZOrder>();
            Guid wid = Guid.Empty;
            var res = m_api.GetOrdersAsync(wid).Result;
            foreach (var o in res)
            {
                result.Add(new ZOrder(o));
            }
            return result;
        }

        public IEnumerable<ZTrade> GetTrades(string pair)
        {
            var result = new List<ZTrade>();
            var ts = TickerSymbol.XBTEUR;
            int? since = default(int?);
            var res = m_api.GetRecentTradesAsync(ts, since).Result;
            foreach (var t in res.Trades)
            {
                result.Add(new ZTrade(pair, t));
            }
            return result;
        }

        public IDictionary<string, ZAccountBalance> GetAccountBalances()
        {
            var result = new Dictionary<string, ZAccountBalance>();
            /*var res = m_api.GetBalances();
            foreach (var b in res)
            {
                result.Add(b.Currency, new AccountBalance(b.Currency, b.Available, b.Balance - b.Available));
            }*/
            return result;
        }
        #endregion -------------------------------------------------------------------------------------------------------------


        //-----------------------------------------------------------------------------------------------------------------------------------
        public class RecentTradesResponse
        {
            public List<ZTrade> recentTrades { get; set; }
        }

        public class ItBitTrade
        {
            public string timestamp { get; set; }       // "2018-02-12T06:23:05.4200000Z
            public int matchNumber { get; set; }        // 2680738
            public decimal price { get; set; }          // "8455.0400000"
            public decimal amount { get; set; }         // "0.00310000"
        }

    } // end of class

    //======================================================================================================================================

    public class ItBitTicker : ZTicker
    {
        Ticker ticker;

        public ItBitTicker(Ticker ticker)
        {
            this.ticker = ticker;
        }

        public override decimal Bid { get { return ticker.Bid; } }
        public override decimal Ask { get { return ticker.Ask; } }
        public override decimal Last { get { return ticker.LastPrice; } }
        public override decimal High { get { return ticker.HighestPrice24Hs; } }
        public override decimal Low { get { return ticker.LowestPrice24Hs; } }
        public override decimal Volume { get { return ticker.Volume24Hs; } }
        public override string Timestamp { get { return ticker.ServerTimeUtc.ToString(); } } 
    } // end of class ItBitTicker

    public class ItBitOrderBookEntry : ZCryptoOrderBookEntry
    {
        public override decimal Price { get { return decimal.MinValue; } }
        public override decimal Amount { get { return decimal.MinValue; } }
    }

    public class ItBitOrderBook : ZCryptoOrderBook
    {
        // List entries are [price, amount]
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }
    }

} // end of namespace 
