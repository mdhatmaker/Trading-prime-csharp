using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using static Tools.G;
using CryptoAPIs.Exchange.Clients.Bittrex;

namespace CryptoAPIs.Exchange
{
    // https://bittrex.com/Home/Api

    public class Bittrex : BaseExchange, IOrderExchange
    {
        public bool EnableLiveOrders { get; set; }

        public override string BaseUrl { get { return "https://bittrex.com/api/v1.1/public"; } }
        public override string Name { get { return "BITTREX"; } }
        public override CryptoExch Exch => CryptoExch.BITTREX;

        BittrexClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static Bittrex m_instance;
        public static Bittrex Create(ApiCredentials creds, string baseUrl = "https://bittrex.com/api")
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret, baseUrl);
        }
        public static Bittrex Create(string apikey = "", string apisecret = "", string baseUrl = "https://bittrex.com/api")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new Bittrex(apikey, apisecret, baseUrl);
        }
        private Bittrex(string apikey, string apisecret, string baseUrl)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            m_api = new BittrexClient(baseUrl, ApiKey, ApiSecret);
            m_instance = this;
        }


        public bool Success { get { return m_success; } }
        public string Message { get { return m_message; } }
        private bool m_success = false;
        private string m_message = "";


        // "BTC-LTC", "BTC-DOGE", etc...
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

                    m_symbolList = new List<string>();
                    var markets = m_api.GetMarkets();
                    //ApiResult<MarketSummary[]> summaries = await m_api.GetMarketSummaries();
                    foreach (var m in markets)
                    {
                        m_symbolList.Add(m.MarketName);
                    }
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;
            }
        }

        // TODO: Need to fix the BittrexTicker to implement ICryptoTicker
        // TODO: Change ALL instances of GetAllTickers to use SortedDictionary rather than Dictionary
        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            var summaryList = m_api.GetMarketSummaries();
            foreach (var summary in summaryList)
            {
                string s = summary.MarketName;
                result[s] = new BittrexTicker(summary);
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string market)
        {
            string type = "both";   // type can be "buy", "sell" or "both" (the default)
            string json = GetJSON(string.Format("https://bittrex.com/api/v1.1/public/getorderbook?market={0}&type={1}", market, type));
            var res = DeserializeJson<BittrexResult<BittrexOrderBook>>(json);
            m_success = res.success;
            m_message = res.message;
            res.result.SetProperty(market);
            return res.result as ZCryptoOrderBook;
        }

        /*public Dictionary<string, ZTicker> GetTickers(List<string> symbols = null)
        {
            if (symbols == null) symbols = SymbolList;         // passing null gets ALL tickers
            var tickers = GetTickers(symbols);
            return tickers;
        }*/

        public Market[] GetMarkets()
        {
            var res = m_api.GetMarkets();
            return res;
        }

        public CryptoCurrency[] GetCurrencies()
        {
            var res = m_api.GetCurrencies();
            return res;
        }

        public override ZTicker GetTicker(string market)
        {
            var res = m_api.GetTicker(market);
            return new BittrexTicker(res);
        }

        public MarketSummary[] GetMarketSummaries()
        {
            var res = m_api.GetMarketSummaries();
            return res;
        }

        public MarketSummary GetMarketSummary(string market)
        {
            var res = m_api.GetMarketSummary(market);
            return res;
        }

        public BookOrder[] GetOrderBook(string market, BookOrderType orderType, int depth=20)
        {
            var res = m_api.GetOrderBook(market, orderType, depth);
            return res;
        }

        public HistoricTrade[] GetMarketHistory(string market)
        {
            var res = m_api.GetMarketHistory(market);
            return res;
        }

        public OrderResult BuyLimit(string market, decimal quantity, decimal rate)
        {
            var res = m_api.BuyLimit(market, quantity, rate);
            return res;
        }

        public OrderResult SellLimit(string market, decimal quantity, decimal rate)
        {
            var res = m_api.SellLimit(market, quantity, rate);
            return res;
        }

        public OrderResult CancelOrder(Guid uuid)
        {
            var res = m_api.CancelOrder(uuid);
            return res;
        }

        public Exchange.Clients.Bittrex.OpenOrder[] GetOpenOrders(string market=null)
        {
            var res = m_api.GetOpenOrders(market);
            return res;
        }

        public Clients.Bittrex.AccountBalance[] GetBalances()
        {
            var res = m_api.GetBalances();
            return res;
        }

        public ZAccountBalance GetBalance(string currency)
        {
            var res = m_api.GetBalance(currency);
            return new ZAccountBalance(res.Currency, res.Available, res.Pending);
        }

        public DepositAddress GetDepositAddress(string currency)
        {
            var res = m_api.GetDepositAddress(currency);
            return res;
        }

        public HistoricAccountOrder[] GetOrderHistory(string market=null)
        {
            var res = m_api.GetOrderHistory(market);
            return res;
        }

        public Exchange.Clients.Bittrex.Order GetOrder(Guid uuid)
        {
            var res = m_api.GetOrder(uuid);
            return res;
        }


        #region ---------- IOrderExchange IMPLEMENTATION -----------------------------------------------------------------------
        public OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty)
        {
            if (!EnableLiveOrders)
            {
                cout("\nBITTREX::SubmitLimitOrder=> '{0}' {1} {2} {3}\n", pair, side, price, qty);
                return null;
            }
            OrderResult res;
            if (side == OrderSide.Buy)
                res = m_api.BuyLimit(pair, qty, price);
            else
                res = m_api.SellLimit(pair, qty, price);
            return new OrderNew(pair, res);
        }

        public OrderCxl CancelOrder(string pair, string orderId)
        {
            if (!EnableLiveOrders)
            {
                cout("\nBITTREX::CancelOrder=> {0} [{1}]\n", pair, orderId);
                return null;
            }
            var res = m_api.CancelOrder(Guid.Parse(orderId));
            return new OrderCxl(pair, res);
        }

        // Pass pair=null to get working orders for ALL pairs
        public IEnumerable<ZOrder> GetWorkingOrders(string pair)
        {
            var result = new List<ZOrder>();
            var res = m_api.GetOpenOrders(pair);
            foreach (var o in res ?? new OpenOrder[] { })
            {
                result.Add(new ZOrder(o));
            }
            return result;
        }

        public IEnumerable<ZTrade> GetTrades(string pair)
        {
            var result = new List<ZTrade>();
            //var res = m_api.GetOpenOrders(pair);
            //var res = m_api.GetOrderHistory(pair);
            var res = m_api.GetMarketHistory(pair);
            foreach (var t in res)
            {
                result.Add(new ZTrade(pair, t));
            }
            return result;
        }

        public IDictionary<string, ZAccountBalance> GetAccountBalances()
        {
            var result = new Dictionary<string, ZAccountBalance>();
            var res = m_api.GetBalances();
            foreach (var b in res ?? new AccountBalance[] { })
            {
                result.Add(b.Currency, new ZAccountBalance(b.Currency, b.Available, b.Balance - b.Available));
            }
            return result;
        }
        #endregion -------------------------------------------------------------------------------------------------------------

    } // end of CLASS Bittrex

    //======================================================================================================================================

    public class BittrexResult<T>
    {
        public bool success;
        public string message;
        public T result { get; set; }
    } // end of class BittrexResult<T>


    public class BittrexTicker : ZTicker
    {
        public string marketName { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal volume { get; set; }
        public decimal last { get; set; }
        //public float baseVolume { get; set; }
        public string timestamp { get; set; }
        public decimal bid { get; set; }
        public decimal ask { get; set; }

        public override decimal Bid { get { return (decimal)bid; } }
        public override decimal Ask { get { return (decimal)ask; } }
        public override decimal Last { get { return (decimal)last; } }
        public override decimal High { get { return (decimal)high; } }
        public override decimal Low { get { return (decimal)low; } }
        public override decimal Volume { get { return (decimal)volume; } }
        public override string Timestamp { get { return timestamp; } }

        public BittrexTicker(Ticker t)
        {
            if (t != null)
            {
                bid = t.Bid;
                ask = t.Ask;
                last = t.Last;
            }
        }

        public BittrexTicker(MarketSummary summary)
        {
            this.marketName = summary.MarketName;
            this.bid = summary.Bid;
            this.ask = summary.Ask;
            this.last = summary.Last;
            this.high = summary.High;
            this.low = summary.Low;
            this.volume = summary.Volume;
            this.timestamp = summary.TimeStamp.ToCompactDateTime();            
        }

        /*public void SetProperty(string marketName)
        {
            m_marketName = marketName;
        }

        // Find available markets by examining BittrexMarket.MarketName property
        public static BittrexTicker GetObject(string market, out bool success, out string message)
        {
            string json = GetJSON(string.Format("https://bittrex.com/api/v1.1/public/getticker?market={0}", market));
            var res = DeserializeJson<BittrexResult<BittrexTicker>>(json);
            success = res.success;
            message = res.message;
            res.result.SetProperty(market);
            return res.result;
        }*/

    } // end of CLASS BittrexTicker

    public class BittrexOrderBookEntry : ZCryptoOrderBookEntry
    {
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }

        public override decimal Price { get { return Rate; } }
        public override decimal Amount { get { return Quantity; } }
    } // end of class BittrexOrderBookEntry

    public class BittrexOrderBook : ZCryptoOrderBook
    {
        public string MarketName { get { return m_marketName; } }
        public List<BittrexOrderBookEntry> buy { get; set; }
        public List<BittrexOrderBookEntry> sell { get; set; }

        private string m_marketName;

        public override List<ZCryptoOrderBookEntry> Bids { get { return buy.Cast<ZCryptoOrderBookEntry>().ToList(); } }
        public override List<ZCryptoOrderBookEntry> Asks { get { return sell.Cast<ZCryptoOrderBookEntry>().ToList(); } }

        public void SetProperty(string marketName)
        {
            m_marketName = marketName;
        }
    } // end of class BittrexOrderBook

} // end of namespace

    

    /*public class BittrexMarketHistoryTrade
    {
        public int Id { get; set; }
        public string TimeStamp { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public float Total { get; set; }
        public string FillType { get; set; }
        public string OrderType { get; set; }

        public override string ToString()
        {
            return "BittrexMarketHistoryTrade::" + Str(this);
        }

        public static List<BittrexMarketHistoryTrade> GetList(string market, out bool success, out string message)
        {
            string json = GetJSON(string.Format("https://bittrex.com/api/v1.1/public/getmarkethistory?market={0}", market));
            var res = DeserializeJson<BittrexResult<List<BittrexMarketHistoryTrade>>>(json);
            success = res.success;
            message = res.message;
            return res.result;
        }
    } // end of CLASS BittrexMarketHistoryTrade

    public class BittrexMarketApi
    {
        public string uuid { get; set; }

        public override string ToString()
        {
            return "BittrexMarketMarketApi:: uuid=" + uuid;
        }

        public static BittrexMarketApi BuyLimit(string market, float quantity, float rate, out bool success, out string message)
        {
            string url = string.Format("https://bittrex.com/api/v1.1/market/buylimit/apikey={0}&market={1}&quantity={2}&rate={3}", Bittrex.API_KEY, market, quantity, rate);
            return GetObject(url, out success, out message);
        }

        public static BittrexMarketApi SellLimit(string market, float quantity, float rate, out bool success, out string message)
        {
            string url = string.Format("https://bittrex.com/api/v1.1/market/selllimit/apikey={0}&market={1}&quantity={2}&rate={3}", Bittrex.API_KEY, market, quantity, rate);
            return GetObject(url, out success, out message);
        }

        public static BittrexMarketApi Cancel(string orderUUID, out bool success, out string message)
        {
            string url = string.Format("https://bittrex.com/api/v1.1/market/cancel/apikey={0}&uuid={1}", Bittrex.API_KEY, orderUUID);
            return GetObject(url, out success, out message);
        }

        public static BittrexMarketApi GetObject(string url, out bool success, out string message)
        {
            string json = GetJSON(url);
            var res = DeserializeJson<BittrexResult<BittrexMarketApi>>(json);
            success = res.success;
            message = res.message;
            return res.result;
        }    
    } // end of class BittrexMarketApi


    public class BittrexTraderMarket : IDataColumns<float>
    {
        static public string[] Columns = { "Buy", "Price", "Sell", "Last","Volume" };

        private string[] cellValues = new string[Columns.Length];

        public string Key { get { return symbol; } }

        //[JsonProperty(PropertyName = "15m")]        // necessary for property names that begin with a digit (or other invalid starting character)
        public string symbol { get; set; }



        public override string ToString()
        {
            return "BittrexTraderMarket:: " + Str(this);
        }

        //public string[] GetCells()
        //{
        //    cellValues[0] = symbol;
        //    cellValues[1] = last;
        //    cellValues[2] = bid;
        //    cellValues[3] = ask;
        //    cellValues[4] = vwap;
        //    cellValues[5] = volume;
        //    cellValues[6] = open;
        //    cellValues[7] = high;
        //    cellValues[8] = low;
        //    cellValues[9] = timestamp;
        //    return cellValues;
        //}

        public Dictionary<float, string> GetColumns()
        {
            throw new NotImplementedException();
        }

        public Dictionary<float, int> GetKeyColumns()
        {
            throw new NotImplementedException();
        }
    
    } // end of CLASS BittrexTraderMarket

    public class BittrexMarket
    {
        public string MarketCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrencyLong { get; set; }
        public string BaseCurrencyLong { get; set; }
        public float MinTradeSize { get; set; }
        public string MarketName { get; set; }
        public bool IsActive { get; set; }
        public string Created { get; set; }

        public override string ToString()
        {
            return "BittrexMarket::" + Str(this);
        }

        public static List<BittrexMarket> GetList(out bool success, out string message)
        {
            string json = GetJSON("https://bittrex.com/api/v1.1/public/getmarkets");
            var res = DeserializeJson<BittrexResult<List<BittrexMarket>>>(json);
            success = res.success;
            message = res.message;
            return res.result;
        }
    } // end of CLASS BittrexMarket


    public class BittrexCurrency
    {
        public string Currency { get; set; }
        public string CurrencyLong { get; set; }
        public int MinConfirmation { get; set; }
        public float TxFee { get; set; }
        public bool IsActive { get; set; }
        public string CoinType { get; set; }
        public string BaseAddress { get; set; }

        public override string ToString()
        {
            return "BittrexCurrency::" + Str(this);
        }

        public static List<BittrexCurrency> GetList(out bool success, out string message)
        {
            string json = GetJSON("https://bittrex.com/api/v1.1/public/getcurrencies");
            var res = DeserializeJson<BittrexResult<List<BittrexCurrency>>>(json);
            success = res.success;
            message = res.message;
            return res.result;
        }
    } // end of CLASS BittrexCurrency

    public class BittrexMarketSummary
    {
        public string MarketName { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Volume { get; set; }
        public float Last { get; set; }
        public float BaseVolume { get; set; }
        public string TimeStamp { get; set; }
        public float Bid { get; set; }
        public float Ask { get; set; }
        public int OpenBuyOrders { get; set; }
        public int OpenSellOrders { get; set; }
        public float PrevDay { get; set; }
        public string Created { get; set; }
        public string DisplayMarketName { get; set; }
    } // end of CLASS BittrexMarketSummary*/

