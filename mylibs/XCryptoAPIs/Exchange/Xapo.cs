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
//using CryptoAPIs.Exchange.Clients.Xapo;

namespace CryptoAPIs.Exchange
{
    // https://xapo.docs.apiary.io/#

    public partial class Xapo : BaseExchange, IOrderExchange    //, IExchangeWebSocket
    {
        public bool EnableLiveOrders { get; set; }

        public override string BaseUrl { get { return "https://v2.api.xapo.com"; } }
        public override string ExchangeName { get { return "XAPO"; } }

        //XapoClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static Xapo m_instance;
        public static Xapo Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static Xapo Create(string apikey = "", string apisecret = "")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new Xapo(apikey, apisecret);
        }
        private Xapo(string apikey, string apisecret)
        {
            m_token = string.Format("{0} {1}", "Token", apikey);    // specific to B2C2 — just a convenience to reference "Token xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            ApiKey = apikey;
            ApiSecret = apisecret;
            //var auth = new Authenticator(ApiKey, ApiSecret);
            //m_api = new GdaxClient(auth, sandBox: false);
            m_instance = this;
        }

        private string m_token;

        #region ---------- EXCHANGE BASE CLASS OVERRIDES ------------------------------------------------------------------------
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
                    /*var products = GetProducts();
                    foreach (var p in products)
                    {
                        //result.Add(p.Id);
                        result.Add(p.id);
                    }*/
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
                Thread.Sleep(340);              // THROTTLE: maximum of 3 requests per second
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string productId)
        {
            string url = BaseUrl + "/products/{0}/book";
            var book = GET<BcOrderBook>(url, productId);
            return book as ZCryptoOrderBook;
        }

        public override ZTicker GetTicker(string symbol)
        {
            //string url = BaseUrl + "/products/{0}/ticker";
            //var ticker = GET<GdaxTicker>(url, productId);
            //var res = m_api.ProductsService.GetProductTickerAsync(symbol).Result;
            //return new BcTicker(res);
            return null;
        }
        #endregion --------------------------------------------------------------------------------------------------------------


        #region ---------- EXCHANGE-SPECIFIC API METHODS ------------------------------------------------------------------------
        public IList<BcMarginRequirement> GetMarginRequirements(string currencySymbol)
        {
            var result = new List<BcMarginRequirement>();
            //var mr = BcRequest<BcMarginRequirement>("/cfd/margin_requirements", $"currency={currencySymbol}");
            //result.Add(mr);
            return result;
        }


        #endregion --------------------------------------------------------------------------------------------------------------


        #region ---------- IOrderExchange IMPLEMENTATION ------------------------------------------------------------------------
        public OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty)
        {
            if (!EnableLiveOrders)
            {
                cout("\nXapo::SubmitLimitOrder=> '{0}' {1} {2} {3}\n", pair, side, price, qty);
                return null;
            }
            /*var orderSide = (side == OrderSide.Buy) ? Clients.GDAX.OrderSide.Buy : Clients.GDAX.OrderSide.Sell;
            var res = m_api.OrdersService.PlaceLimitOrderAsync(orderSide, gdaxProductPair, qty, price, TimeInForce.Gtc, postOnly: false).Result;
            return (res == null ? null : new OrderNew(pair, res));*/
            return null;
        }

        public OrderCxl CancelOrder(string pair, string orderId)
        {
            if (!EnableLiveOrders)
            {
                cout("\nXapo::CancelOrder=> {0} [{1}]\n", pair, orderId);
                return null;
            }
            //var res = m_api.OrdersService.CancelOrderByIdAsync(orderId).Result;            
            //return (res == null ? null : new OrderCxl(pair, res));
            return null;
        }

        public IEnumerable<ZOrder> GetWorkingOrders(string pair)
        {
            var result = new List<ZOrder>();
            /*var res = m_api.OrdersService.GetAllOrdersAsync(limit: 100).Result;            
            foreach (var li in res)
            {
                foreach (var or in li)
                {
                    if (or.Status != "done" && or.Status != "settled")  // "open", "pending", "active", "done", "settled"
                        result.Add(new ZOrder(or));
                }
            }*/
            return result;
        }

        // Pass pair=null to get trades for ALL pairs
        IEnumerable<ZTrade> IOrderExchange.GetTrades(string pair)
        {
            var result = new List<ZTrade>();
            /*IList<IList<FillResponse>> res;
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
            }*/
            return result;
        }

        public IDictionary<string, ZAccountBalance> GetAccountBalances()
        {
            var result = new Dictionary<string, ZAccountBalance>();
            /*string url = BaseUrl + "/balance/";
            var headers = new Dictionary<string, string>() { { "Authorization", m_token } };
            var balances = XGET<BcBalances>(headers, url);*/
            /*var balances = BcRequest<BcBalances>("/balance/");
            var map = balances.GetMap();
            foreach (var symbol in map.Keys)
            {
                result.Add(symbol, new ZAccountBalance(symbol, map[symbol]));
            }*/
            return result;
        }
        #endregion --------------------------------------------------------------------------------------------------------------


    } // end of class Xapo

    //======================================================================================================================================

    #region ---------- MODEL CLASS FOR EXCHANGE BASE CLASS OVERRIDE METHODS -----------------------------------------------------
    public class XapoProduct
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
    } // end of class XapoProduct

    public class XapoOrderBook : ZCryptoOrderBook
    {
        public string sequence { get; set; }
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }

        //[price, size, order_id],
        //[ "295.97","5.72036512","da863862-25f4-4868-ac41-005d11ab0a5f" ],

    } // end of class XapoOrderBook

    public class XapoTicker : ZTicker
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

        public XapoTicker(ProductTicker pt)
        {
            trade_id = pt.Trade_id;
            price = pt.Price;
            size = pt.Size;
            bid = pt.Bid;
            ask = pt.Ask;
            volume = pt.Volume;
            time = pt.Time.ToDateTimeString();
        }
    } // end of class XapoTicker
    #endregion ------------------------------------------------------------------------------------------------------------------


    #region ---------- EXCHANGE-SPECIFIC MODEL CLASSES --------------------------------------------------------------------------
    public class XapoTrade
    {
        public string time { get; set; }
        public long trade_id { get; set; }
        public string price { get; set; }
        public string size { get; set; }
        public string side { get; set; }        // "buy", "sell"
    } // end of class BcTrade

    public class XapoBalances
    {
        public decimal USD { get; set; }
        public decimal BTC { get; set; }
        public decimal JPY { get; set; }
        public decimal GBP { get; set; }
        public decimal ETH { get; set; }
        public decimal EUR { get; set; }
        public decimal CAD { get; set; }

        public IDictionary<string, decimal> GetMap()
        {
            var result = new SortedDictionary<string, decimal>();
            result.Add("USD", USD);
            result.Add("BTC", BTC);
            result.Add("JPY", JPY);
            result.Add("GBP", GBP);
            result.Add("ETH", ETH);
            result.Add("EUR", EUR);
            result.Add("CAD", CAD);
            return result;
        }
    } // end of class XapoBalances


    #endregion ------------------------------------------------------------------------------------------------------------------

} // end of namespace
