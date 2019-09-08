using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
using CryptoAPIs.Exchange.Clients.BitFinex;

namespace CryptoAPIs.Exchange
{
    public partial class Bitfinex : BaseExchange, IExchangeWebSocket, IOrderExchange
    {
        public bool EnableLiveOrders { get; set; }

        public override string BaseUrl { get { return "https://api.bitfinex.com/v1"; } }
        public override string Name { get { return "BITFINEX"; } }
        public override CryptoExch Exch => CryptoExch.BITFINEX;

        BitfinexApiV1 m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static Bitfinex m_instance;
        public static Bitfinex Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static Bitfinex Create(string apikey = "", string apisecret = "")
        {
            if (m_instance != null)
                return m_instance;
            else
                return new Bitfinex(apikey, apisecret);
        }
        private Bitfinex(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            m_api = new BitfinexApiV1(ApiKey, ApiSecret);
            m_instance = this;
        }


        private static readonly HttpClient m_httpClient = new HttpClient();

        public int LimitBids = 50;
        public int LimitAsks = 50;

        //public static string BaseURL { get { return "https://api.bitfinex.com/v1"; } }
        //public static string BaseURL2 { get { return "https://api.bitfinex.com/v2"; } }

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

                    m_symbolList = GET<List<string>>(string.Format("{0}/symbols", BaseUrl));
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
                result[s] = GetTicker(s);
                dout("(sleeping Bitfinex::GetAllTickers)  {0}", DateTime.Now.ToString("HH:mm:ss"));
                Thread.Sleep(20);
            }
            return result;
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            var request = GET<BitfinexOrderBook>(string.Format("{0}/book/{1}?limit_bids={2}&limit_asks={3}", BaseUrl, symbol, LimitBids, LimitAsks));
            return request as ZCryptoOrderBook;
        }

        public List<BitfinexSymbolDetail> GetSymbolDetails()
        {
            var request = GET<List<BitfinexSymbolDetail>>("{0}/symbols_details", BaseUrl);
            return request;
        }

        public override ZTicker GetTicker(string symbol)
        {
            var request = GET<BitfinexTicker>(string.Format("{0}/pubticker/{1}", BaseUrl, symbol));
            return request;
        }

        public List<ZAccountBalance> GetBalances()
        {
            var res = m_api.GetBalances();
            var depositBalance = res.deposit;
            var exchangeBalance = res.exchange;
            var tradingBalance = res.trading;
            var balances = new List<ZAccountBalance>();
            balances.Add(new ZAccountBalance("BTC", res.totalAvailableBTC, res.totalBTC));
            balances.Add(new ZAccountBalance("USD", res.totalAvailableUSD, res.totalUSD));
            return balances;
        }

        /*public List<List<string>> GetOrderBook(string symbol)
        {
            return new OrderBook(GetBitfinexOrderBook(symbol));
        }*/

        public async Task<string> Authenticate()
        {
            string apiKey = this.ApiKey;
            string apiSecret = this.ApiSecret;
            const string baseUrl = "https://api.bitfinex.com";
            const string url = "/v1/account_infos";

            string nonce = GetNonceStr();

            string completeURL = baseUrl + url;
            /*string body = string.Format(@"{
              request: {0},
              {1}
            }", completeURL, nonce);*/
            string body = string.Format("{{ request: {0}, {1} }}", completeURL, nonce);

            byte[] payload = body.ToBase64Bytes();

            string signature = GetHash(body, HashType.SHA384, apiSecret);
            string options = string.Format(@"{{ url: {0}, headers: {{ 'X-BFX-APIKEY': {1}, 'X-BFX-PAYLOAD': {2}, 'X-BFX-SIGNATURE': {3} }}, body: {4} }}", completeURL, apiKey, payload, signature, body);

            /*var values = new Dictionary<string, string>
            {
               { "thing1", "hello" },
               { "thing2", "world" }
            };
            var content = new FormUrlEncodedContent(values);*/

            var response = await m_httpClient.PostAsync(completeURL, new StringContent(options));                   // POST
            //var responseString = await client.GetStringAsync("http://www.example.com/recepticle.aspx");             // GET

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;

            /*SHA384 shaM = new SHA384Managed();
            byte[] signaure = shaM.ComputeHash(payload);*/

            // ORIGINAL JAVASCRIPT SIGNATURE CRYPTOGRAPHY CODE:
            /*string signature = crypto
              .createHmac('sha384', apiSecret)
              .update(payload)
              .digest('hex')*/

            /*using (HMACSHA384 hmac = new HMACSHA384(apiSecret.ToBase64Bytes()))
            {
            }*/

            /*Request. return request.post(
             options,
             function(error, response, body) {
                console.log('response:', JSON.stringify(body, 0, 2))*/
        }

        private bool GetOrderSymbol(string pair, out OrderSymbol orderSymbol)
        {
            return Enum.TryParse<OrderSymbol>(pair, true, out orderSymbol);
        }


        public override void SubscribeOrderBookUpdates(ZCurrencyPair pair, bool subscribe)
        {
            StartWebSocket();
            SubscribeWebSocket();
        }

        public override void SubscribeOrderUpdates(ZCurrencyPair pair, bool subscribe)
        {
            if (subscribe)
                m_subscribedPairs.Add(pair);
            else
                m_subscribedPairs.Remove(pair);
        }

        public override void SubscribeTickerUpdates(ZCurrencyPair pair, bool subscribe)
        {
            if (subscribe)
                SubscribeChannel("ticker", pair.BitfinexSymbol);
            else
                UnsubscribeChannel("ticker", pair.BitfinexSymbol);
        }

        public override void SubscribeTradeUpdates(ZCurrencyPair pair, bool subscribe)
        {
            if (subscribe)
                SubscribeChannel("trades", pair.BitfinexSymbol);
            else
                UnsubscribeChannel("trades", pair.BitfinexSymbol);
        }

        #region ---------- IOrderExchange IMPLEMENTATION -----------------------------------------------------------------------
        public OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty)
        {
            if (!EnableLiveOrders)
            {
                cout("\nBITFINEX::SubmitLimitOrder=> '{0}' {1} {2} {3}\n", pair, side, price, qty);
                return null;
            }
            var orderSide = (side == OrderSide.Buy) ? Clients.BitFinex.OrderSide.Buy : Clients.BitFinex.OrderSide.Sell;
            //OrderType.MarginMarket, OrderType.MarginStop, OrderType.MarginTrailingStop
            //var res = m_api.ExecuteBuyOrderBTC(qty, price, Clients.BitFinex.OrderExchange.Bitfinex, Clients.BitFinex.OrderType.MarginLimit);
            OrderSymbol orderSymbol;
            if (!GetOrderSymbol(pair, out orderSymbol))
            {
                ErrorMessage("Bitfinex::SubmitLimitOrder=> Cannot convert '{0}' to Enum OrderSymbol", pair);
                return null;
            }
            var res = m_api.ExecuteOrder(orderSymbol, qty, price, OrderExchange.Bitfinex, orderSide, Clients.BitFinex.OrderType.MarginLimit);
            //var res = AsyncHelpers.RunSync<NewOrderResponse>(() => m_api.ExecuteOrder(orderSymbol, qty, price, OrderExchange.Bitfinex, orderSide, Clients.BitFinex.OrderType.MarginLimit));
            return (res == null ? null : new OrderNew(pair, res));
        }

        public OrderCxl CancelOrder(string pair, string orderId)
        {
            if (!EnableLiveOrders)
            {
                cout("\nBITFINEX::CancelOrder=> {0} [{1}]\n", pair, orderId);
                return null;
            }
            var res = m_api.CancelOrder(int.Parse(orderId));
            return (res == null ? null : new OrderCxl(pair, res));
        }

        public IEnumerable<ZOrder> GetWorkingOrders(string pair)
        {
            var result = new List<ZOrder>();
            var res = m_api.GetActiveOrders();
            foreach (var o in res.orders)
            {
                result.Add(new ZOrder(o));
            }
            return result;
        }

        IEnumerable<ZTrade> IOrderExchange.GetTrades(string pair)
        {
            var result = new List<ZTrade>();
            var res = m_api.GetActivePositions();
            /*foreach (var ap in res)
            {
                result.Add(new Trade(pair, ap));
            }*/
            return result;
        }

        public IDictionary<string, ZAccountBalance> GetAccountBalances()
        {
            var result = new Dictionary<string, ZAccountBalance>();
            var res = m_api.GetBalances();
            //res.deposit
            //res.exchange
            //res.trading
            if (res != null)
            {
                result.Add("BTC", new ZAccountBalance("BTC", res.totalAvailableBTC, res.totalBTC - res.totalAvailableBTC));
                result.Add("USD", new ZAccountBalance("USD", res.totalAvailableUSD, res.totalUSD - res.totalAvailableUSD));
            }
            return result;
        }
        #endregion -------------------------------------------------------------------------------------------------------------

    } // end of class Bitfinex

    //======================================================================================================================================

    public class BitfinexTicker : ZTicker
    {
        public string mid { get; set; }
        public string bid { get; set; }
        public string ask { get; set; }
        public string last_price { get; set; }
        public string low { get; set; }
        public string high { get; set; }
        public string volume { get; set; }
        public string timestamp { get; set; }

        public override decimal Bid { get { return decimal.Parse(this.bid ?? "0"); } }
        public override decimal Ask { get { return decimal.Parse(this.ask ?? "0"); } }
        public override decimal Last { get { return decimal.Parse(this.last_price ?? "0"); } }
        public override decimal High { get { return decimal.Parse(this.high ?? "0"); } }
        public override decimal Low { get { return decimal.Parse(this.low ?? "0"); } }
        public override decimal Volume { get { return decimal.Parse(this.volume ?? "0"); } }
        public override string Timestamp { get { return this.timestamp ?? ""; } }
    } // end of class BitfinexTicker

    public class BitfinexSymbolDetail
    {
        public string pair { get; set; }
        public int price_precision { get; set; }
        public string initial_margin { get; set; }
        public string minimum_margin { get; set; }
        public string maximum_order_size { get; set; }
        public string minimum_order_size { get; set; }
        public string expiration { get; set; }
    } // end of class BitfinexSymbolDetail

    public class BitfinexOrderBookEntry : ZCryptoOrderBookEntry
    {
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public string timestamp { get; set; }

        public override decimal Price { get { return price; } }
        public override decimal Amount { get { return amount; } }
        public override string Timestamp { get { return ""; } }


        /*public float Price { get { return float.Parse(price); } }
        public float Amount { get { return float.Parse(amount); } }
        public DateTime Timestamp { get { return GDate.UnixTimeStampToDateTime(double.Parse(timestamp)); } }*/
    } // end of class BitfinexOrderBookEntry

    public class BitfinexOrderBook : ZCryptoOrderBook
    {
        public List<BitfinexOrderBookEntry> bids { get; set; }
        public List<BitfinexOrderBookEntry> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("BIDS\n");
            int ix = 0;
            foreach (var b in bids)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}  {3}\n", ++ix, b.Price, b.amount, ""));   //b.timestamp));
            }
            sb.Append("ASKS\n");
            ix = 0;
            foreach (var a in asks)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}  {3}\n", ++ix, a.Price, a.amount, ""));   //a.timestamp));
            }
            return sb.ToString();
        }
    } // end of class BitfinexOrderBook


} // end of namespace
