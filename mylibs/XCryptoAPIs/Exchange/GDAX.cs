using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
using CryptoAPIs.Exchange.Clients.GDAX;
using System.Runtime.Serialization;

namespace CryptoAPIs.Exchange
{
    // https://docs.gdax.com/

    public enum GdaxOHLCInterval { _1m = 60, _5m = 300, _15m = 900, _1h = 3600, _6h = 21600, _1d = 86400 }

    public partial class GDAX : BaseExchange, IExchangeWebSocket, IOrderExchange
    {
        public bool EnableLiveOrders { get; set; }

        public int GetOrderBookLevelParam = 1;

        public override string BaseUrl { get { return "https://api.gdax.com"; } }
        public override string Name { get { return "GDAX"; } }
        public override CryptoExch Exch => CryptoExch.GDAX;

        GdaxClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static GDAX m_instance;
        public static GDAX Create(ApiCredentials creds, string passphrase = null)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret, passphrase);
        }
        public static GDAX Create(string apikey = "", string apisecret = "", string passphrase = "")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new GDAX(apikey, apisecret, passphrase);
        }
        private GDAX(string apikey, string apisecret, string passphrase)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            var auth = new Authenticator(ApiKey, ApiSecret, passphrase);
            m_api = new GdaxClient(auth, sandBox: false);
            m_instance = this;
        }

        public static Dictionary<string, string> ChartIntervals = new Dictionary<string, string>() { { "1m", "60" }, { "5m", "300" }, { "15m", "900" }, { "1h", "3600" }, { "6h", "21600" }, { "1d", "86600" } };

        private Dictionary<string, Clients.GDAX.ProductType> m_productPairs = new Dictionary<string, ProductType>(); 

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

                    List<string> result = new List<string>();
                    var products = GetProducts();
                    foreach (var p in products)
                    {
                        //result.Add(p.Id);
                        result.Add(p.id);
                    }
                    m_symbolList = result;
                    m_symbolList.Sort();

                    UpdateCurrencyPairsFromSymbols();
                }
                return m_symbolList;
            }
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            var result = new Dictionary<string, ZTicker>();
            foreach (var s in SymbolList)
            {
                result[s] = GetTicker(s);
                dout("(sleeping Bitfinex::GetAllTickers)  {0}", DateTime.Now.ToString("HH:mm:ss"));
                Thread.Sleep(175);              // THROTTLE: maximum of 10 requests per second in bursts (private...6 per second for public)
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

        public override ZTicker GetTicker(string symbol)
        {
            //string url = BaseUrl + "/products/{0}/ticker";
            //var ticker = GET<GdaxTicker>(url, productId);
            //var res = m_api.ProductsService.GetProductTickerAsync(GetProductPair(symbol)).Result;
            var res = AsyncHelpers.RunSync<ProductTicker>(() => m_api.ProductsService.GetProductTickerAsync(GetProductPair(symbol)));
            return new GdaxTicker(res);
        }

        public List<GdaxTrade> GetTrades(string symbol)
        {
            string url = BaseUrl + "/products/{0}/trades";
            var trades = GET<List<GdaxTrade>>(url, symbol);
            return trades;
        }

        // Where start/end are start and end times for the data
        // AND granularity is desired timeslice in seconds: {60, 300, 900, 3600, 21600, 86400}
        public ZCandlestickMap GetCandlesticks(string productId, string intervalSeconds, DateTime? start = null, DateTime? end = null)
        {
            var result = new ZCandlestickMap(CryptoExch.GDAX, productId, intervalSeconds);
            var productPair = GetProductPair(productId);
            int seconds = int.Parse(ChartIntervals[intervalSeconds]);
            var dtEnd = (end == null ? DateTime.Now : end.Value);
            var dtStart = (start == null ? dtEnd.Subtract(TimeSpan.FromSeconds(seconds * 100)) : start.Value);
            // returns list of list: [[ time, low, high, open, close, volume ], [ 1415398768, 0.32, 4.2, 0.35, 4.2, 12.3 ]]
            var sticks = AsyncHelpers.RunSync<IEnumerable<object[]>>(() => m_api.ProductsService.GetHistoricRatesAsync(productPair, dtStart, dtEnd, seconds));
            //var sticks = await m_api.ProductsService.GetHistoricRatesAsync(productPair, dtStart, dtEnd, int.Parse(ChartIntervals[intervalSeconds]));
            //string url = BaseUrl + "/products/{0}/candles";
            //var sticks = GET<List<List<float>>>(url, productId);
            foreach (var li in sticks)
            {
                var cs = new GdaxCandlestick(li);
                result.Add(cs.time, cs);
            }
            return result;
        }

        public ProductStats GetProductStats(string symbol)
        {
            var res = m_api.ProductsService.GetProductStatsAsync(GetProductPair(symbol)).Result;
            return res;
        }

        public IEnumerable<GdaxProduct> GetProducts()
        {
            string url = BaseUrl + "/products";
            var headers = new Dictionary<string, string>() { { "User-Agent", "GDAXClient" } };
            var products = XGET<List<GdaxProduct>>(headers, url);
            return products;
            //var res = m_api.ProductsService.GetAllProductsAsync().Result;
            //return res;
        }

        public List<Account> GetAccounts()
        {
            var res = m_api.AccountsService.GetAllAccountsAsync().Result;
            return res.ToList();
        }

        // We'll cache this translation of pair (string) to GDAX ProductPair object for faster performance
        public Clients.GDAX.ProductType GetProductPair(string pair)
        {
            Clients.GDAX.ProductType productPair;
            if (m_productPairs.TryGetValue(pair, out productPair)) return productPair;

            var zcp = ZCurrencyPair.FromSymbol(pair, CryptoExch.GDAX);
            string strEnum = zcp.ExchangeSpecificLeft + zcp.ExchangeSpecificRight;
            //var li = ZCurrencyPair.GetLeftRight(pair, CryptoExch.GDAX);
            //string strEnum = li[0] + li[1];
            //string strEnum = "";
            productPair = (Clients.GDAX.ProductType)Enum.Parse(typeof(Clients.GDAX.ProductType), strEnum, ignoreCase: true);
            m_productPairs[pair] = productPair;
            return productPair;
        }

        public override void SubscribeOrderBookUpdates(ZCurrencyPair pair, bool subscribe)
        {
            // GDAX: WORKS!!!
            StartWebSocket(null);
            string symbol = pair.GDAXSymbol;
            // TODO: args should use this GDAXSymbol property
            string[] args = { @"""type"":""subscribe""", @"""product_ids"":[""btc-usd""]", @"""channels"":[""level2""]" };
            SubscribeWebSocket(args);
        }

        #region ---------- IOrderExchange IMPLEMENTATION -----------------------------------------------------------------------
        public OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty)
        {
            if (!EnableLiveOrders)
            {
                cout("\nGDAX::SubmitLimitOrder=> '{0}' {1} {2} {3}\n", pair, side, price, qty);
                return null;
            }
            var orderSide = (side == OrderSide.Buy) ? Clients.GDAX.OrderSide.Buy : Clients.GDAX.OrderSide.Sell;
            var gdaxProductPair = GetProductPair(pair);
            // For placing GDAX orders, the "postOnly" parameter disallows 'taker' orders (allows only 'maker' orders)
            //var res = m_api.OrdersService.PlaceLimitOrderAsync(orderSide, gdaxProductPair, qty, price, TimeInForce.Gtc, postOnly: false).Result;
            var res = AsyncHelpers.RunSync<OrderResponse>(() => m_api.OrdersService.PlaceLimitOrderAsync(orderSide, gdaxProductPair, qty, price, TimeInForce.Gtc, postOnly: false));
            return (res == null ? null : new OrderNew(pair, res));
        }

        public OrderCxl CancelOrder(string pair, string orderId)
        {
            if (!EnableLiveOrders)
            {
                cout("\nGDAX::CancelOrder=> {0} [{1}]\n", pair, orderId);
                return null;
            }
            var res = m_api.OrdersService.CancelOrderByIdAsync(orderId).Result;
            return (res == null ? null : new OrderCxl(pair, res));
        }

        public IEnumerable<ZOrder> GetWorkingOrders(string pair)
        {
            var result = new List<ZOrder>();
            //var res = m_api.OrdersService.GetOrderByIdAsync(string id)
            var res = m_api.OrdersService.GetAllOrdersAsync(limit: 100).Result;
            foreach (var li in res)
            {
                foreach (var or in li)
                {
                    if (or.Status != "done" && or.Status != "settled")  // "open", "pending", "active", "done", "settled"
                        result.Add(new ZOrder(or));
                }
            }
            return result;
        }

        // Pass pair=null to get trades for ALL pairs
        IEnumerable<ZTrade> IOrderExchange.GetTrades(string pair)
        {
            var result = new List<ZTrade>();
            IList<IList<FillResponse>> res;
            //var res = m_api.FillsService.GetFillsByOrderIdAsync(orderId:, limit: 100).Result;
            if (pair == null)
                res = m_api.FillsService.GetAllFillsAsync(limit: 100).Result;
            else
                res = m_api.FillsService.GetFillsByProductIdAsync(GetProductPair(pair), limit: 100).Result;

            foreach (var li in res)
            {
                foreach (var fr in li)
                {
                    result.Add(new ZTrade(pair, fr));
                }
            }
            return result;
        }

        public IDictionary<string, ZAccountBalance> GetAccountBalances()
        {
            var result = new Dictionary<string, ZAccountBalance>();
            var res = m_api.AccountsService.GetAllAccountsAsync().Result;
            foreach (var acct in res)
            {
                var bal = new ZAccountBalance(acct.Currency, acct.Available, acct.Balance - acct.Available);
                result.Add(acct.Currency, bal);
            }
            return result;
        }
        #endregion -------------------------------------------------------------------------------------------------------------

        /*Deposits and Withdrawals
        ACH Deposit: Free  ACH Withdrawal: Free
        SEPA Deposit: Free  SEPA Withdrawal: €0.15 EUR
        USD Wire Deposit: $10 USD  USD Wire Withdrawal: $25 USD
          Account Fees
        There are no fees for having a GDAX account or for holding funds in your GDAX account.*/
                
        // https://www.gdax.com/fees/BTC-USD
        // NOTE: Taker trades are charged tier one fees at the time of trade but a rebate for the
        // previous 24 hours of trading fees will be credited to your account each day at 12:00am UTC.
        // TODO: These fees are DIFFERENT for ETH (and other currencies)
        public float GetTakerFee(string symbol) {
            float exchange30DayVolume = 450885.18f;
            float my30DayVolume = 0.0f;
            if (my30DayVolume > .20 * exchange30DayVolume)
                return .10f;
            else if (my30DayVolume > .10 * exchange30DayVolume)
                return .15f;
            else if (my30DayVolume > .05 * exchange30DayVolume)
                return .19f;
            else if (my30DayVolume > .025 * exchange30DayVolume)
                return .22f;
            else if (my30DayVolume > .01 * exchange30DayVolume)
                return .24f;
            else
                return .25f;
        } 


    } // end of class GDAX

    //======================================================================================================================================

    public class GdaxProduct
    {
        public string id { get; set; }                  // "BCH-BTC"
        public string base_currency { get; set; }       // "BCH"
        public string quote_currency { get; set; }      // "BTC"
        public decimal base_min_size { get; set; }      // "0.01"
        public decimal base_max_size { get; set; }      // "200"
        public decimal quote_increment { get; set; }    // "0.00001"
        public string display_name { get; set; }        // "BCH/BTC"
        public string status { get; set; }              // "online"
        public bool margin_enabled { get; set; }        // false
        public string status_message { get; set; }      // null
        public decimal min_market_funds { get; set; }   // "0.001"
        public decimal max_market_funds { get; set; }   // "30"
        public bool post_only { get; set; }             // false
        public bool limit_only { get; set; }            // false
        public bool cancel_only { get; set; }           // false
    } // end of class GdaxProduct

    public class GdaxOrderBook : ZCryptoOrderBook
    {
        public string sequence { get; set; }
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }

        //[price, size, order_id],
        //[ "295.97","5.72036512","da863862-25f4-4868-ac41-005d11ab0a5f" ],

    } // end of class GdaxOrderBook

    public class GdaxTicker : ZTicker
    {
        public long trade_id { get; set; }
        public decimal price { get; set; }
        public decimal size { get; set; }
        public decimal bid { get; set; }
        public decimal ask { get; set; }
        public decimal volume { get; set; }
        public string time { get; set; }

        public override decimal Bid { get { return bid; } }
        public override decimal Ask { get { return ask; } }
        public override decimal Last { get { return price; } }
        public override decimal High { get { return decimal.Parse("0");; } }
        public override decimal Low { get { return decimal.Parse("0"); } }
        public override decimal Volume { get { return volume; } }
        public override string Timestamp { get { return time; } }

        public GdaxTicker(ProductTicker pt)
        {
            trade_id = pt.Trade_id;
            price = pt.Price;
            size = pt.Size;
            bid = pt.Bid;
            ask = pt.Ask;
            volume = pt.Volume;
            time = pt.Time.ToDateTimeString();
        }
    } // end of class GdaxTicker

    /*
     The trade side indicates the maker order side. The maker order is the order that was open on
     the order book. buy side indicates a down-tick because the maker was a buy order and their order
     was removed. Conversely, sell side indicates an up-tick.
     */
    public class GdaxTrade
    {
        public string time { get; set; }
        public long trade_id { get; set; }
        public string price { get; set; }
        public string size { get; set; }
        public string side { get; set; }        // "buy", "sell"
    } // end of class GdaxTrade

    public class GdaxCandlestick : ZCandlestick
    {
        /*[DataMember(Name = "time")]
        public long Time { get; set; }
        public override decimal open { get; set; }
        public override decimal high { get; set; }
        public override decimal low { get; set; }
        public decimal close { get; set; }
        public decimal volume { get; set; }*/

        //[
        // [ time, low, high, open, close, volume ],
        // [ 1415398768, 0.32, 4.2, 0.35, 4.2, 12.3 ],
        // ...
        //]

        public GdaxCandlestick(object[] values)
        {
            t = ((long)values[0]).ToDateTime();
            l = decimal.Parse(values[1].ToString());
            h = decimal.Parse(values[2].ToString()); ;
            o = decimal.Parse(values[3].ToString()); ;
            c = decimal.Parse(values[4].ToString()); ;
            v = decimal.Parse(values[5].ToString()); ;
        }

        public GdaxCandlestick(List<decimal> values)
        {
            t = ((long)values[0]).ToDateTime();
            l = values[1];
            h = values[2];
            o = values[3];
            c = values[4];
            v = values[5];
        }

        // {"type":"open","side":"sell","price":"5875.84000000",
        // "order_id":"e9e7c41f-43d7-4a98-bc7d-c62b0b7538d7","remaining_size":"1.00000000",
        // "product_id":"BTC-USD","sequence":4276337186,"time":"2017-10-27T05:20:29.147000Z"}

        public override string ToString()
        {
            return string.Format("[GdaxCandlestick: time={0}, open={1}, high={2}, low={3}, close={4}, volume={5}]", time, open, high, low, close, volume);
        }
    } // end of class GdaxCandlesticks

} // end of namespace
