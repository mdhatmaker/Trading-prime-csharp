using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    // https://blinktrade.com/docs/

    public class BlinkTrade : BaseExchange, IExchangeWebSocket
    {
        public override string BaseUrl { get { return "https://api_testnet.blinktrade.com/api/v1"; } }              // testing
        public string WebsocketUri { get { return "wss://api.testnet.blinktrade.com/trade/"; } }    // testing
        //public override string BaseUrl { get { return "https://api.blinktrade.com/api/vi"; } }                      // production
        //public string WebsocketUri { get { return "wss://ws.blinktrade.com/trade/"; } }             // production
        public override string ExchangeName { get { return "BLINKTRADE"; } }

        //BlinkTradeClient m_api;

        // SINGLETON (ACTUALLY MORE OF A FACTORY PATTERN)
        private static BlinkTrade m_instance;
        public static BlinkTrade Create(ApiCredentials creds)
        {
            if (creds == null)
                return Create();
            else
                return Create(creds.ApiKey, creds.ApiSecret);
        }
        public static BlinkTrade Create(string apikey = null, string apisecret = null)
        {
            if (m_instance != null)
                return m_instance;
            else
                return new BlinkTrade(apikey, apisecret);
        }
        private BlinkTrade(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
            //var apiclient = new Exchange.Clients.Binance.ApiClient(ApiKey, ApiSecret, "https://www.binance.com", "wss://stream.binance.com:9443/ws/", true);
            //m_api = new Exchange.Clients.Binance.BinanceClient(apiclient, loadTradingRules: false);
            m_instance = this;
        }

        public int Depth = 20;

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    // Currency codes and related brokers:
                    // VEF  Venezuelan Bolivares (SurBitcoin)
                    // VND  Vietnamise Dongs (VBTC)
                    // BRL  Brazil Reals (FoxBit)
                    // PKR  Pakistani Ruppe (UrduBit)
                    // CLP  Chilean Pesos (ChileBit.NET)
                    m_symbolList = new List<string> { "BTCVEF", "BTCVND", "BTCBRL", "BTCPKR", "BTCCLP" };
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            return GET<BlinkTradeTicker>(BaseUrl + "/ticker/{0}", symbol);
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
            var book = GET<BlinkTradeOrderBook>(BaseUrl + "/depth/{0}", symbol, this.Depth);
            return book as ZCryptoOrderBook;
        }

        #region --------------------------------------------------------------------------------------------------------
        public void WebSocketMessageHandler(MessageArgs e)
        {
            //cout(e.Text);
            try
            {
                JObject msg = JObject.Parse(e.Text);
                //dynamic dict = JContainer.Parse(e.Text);
                //dynamic list = JArray.Parse(e.Text);
                if (msg["e"] != null)
                {
                    string msgType = msg["e"].ToString();
                    if (msgType == "md")
                    {
                        var data = msg["data"] as JObject;
                        //cout(array.Count);
                        string pair = data["pair"].Value<string>();
                        cout(pair);
                        var bids = data["buy"] as JArray;
                        var asks = data["sell"] as JArray;
                        long buy_total = data["buy_total"].Value<long>();
                        long sell_total = data["sell_total"].Value<long>();
                        //print_bids_asks(bids, asks);
                        for (int i = 0; i < bids.Count; ++i)
                        {
                            var b = bids[i] as JArray;
                            float price = b[0].Value<float>();
                            long quantity = b[1].Value<long>();
                            m_orderBook.UpdateBid(price.ToString(), quantity.ToString());
                        }
                        for (int i = 0; i < asks.Count; ++i)
                        {
                            var a = asks[i] as JArray;
                            float price = a[0].Value<float>();
                            long quantity = a[1].Value<long>();
                            m_orderBook.UpdateAsk(price.ToString(), quantity.ToString());
                        }
                    }
                    else if (msgType == "md_groupped")
                    {
                        var data = msg["data"] as JObject;
                        //cout(array.Count);
                        string pair = data["pair"].Value<string>();
                        cout(pair);
                        long id = data["id"].Value<long>();
                        var bids = data["buy"] as JObject;
                        var asks = data["sell"] as JObject;
                        //long buy_total = data["buy_total"].Value<long>();
                        //long sell_total = data["sell_total"].Value<long>();
                        //print_bids_asks2(bids, asks);
                        foreach (var prop in bids.Properties())
                        {
                            string name = prop.Name;
                            var value = prop.Value;
                            m_orderBook.UpdateBid(name, value.ToString());
                        }
                        foreach (var prop in asks.Properties())
                        {
                            string name = prop.Name;
                            var value = prop.Value;
                            m_orderBook.UpdateAsk(name, value.ToString());
                        }
                    }
                    else if (msgType == "history-update")
                    {
                        cout("history-update");
                    }
                    else if (msgType == "ping")
                    {
                        m_socket.SendMessage(@"{""e"": ""pong""}");
                    }
                    else if (msgType == "auth")
                    {
                        m_socket.SendMessage(@"{ ""e"": ""subscribe"", ""rooms"": [""pair-BTC-USD""] }");
                    }
                }
                else
                {
                    cout("Unknown message type: ", Str(msg));
                }
            }
            catch (Exception ex)
            {
                cout("ERROR: Exception processing message: {0}", ex.Message);
                cout(e.Text);
            }
            return;
        }

        public void StartWebSocket(string[] args = null)
        {
            m_socket = new ZWebSocket("wss://ws.cex.io/ws/", this.WebSocketMessageHandler);
        }

        // TODO: handle different args
        public void SubscribeWebSocket(string[] args = null)
        {
            string authRequest = Cex.Instance.CreateAuthRequest();
            //string json = @"{""op"": ""subscribe"", ""args"": [""orderBook10:XBTUSD""]}";
            m_socket.SendMessage(authRequest);
        }
        #endregion -----------------------------------------------------------------------------------------------------



    } // end of class

    //======================================================================================================================================

    public class BlinkTradeTicker : ZTicker
    {
        public string bid { get; set; }         // highest buy order
        public string ask { get; set; }         // lowest sell order
        public string low { get; set; }         // last 24 hours price low
        public string high { get; set; }        // last 24 hours price high
        public string last { get; set; }        // last BTC price
        public string volume { get; set; }      // last 24 hours volume
        public string volume30d { get; set; }   // last 30 days volume
        public string timestamp { get; set; }

        public override decimal Bid { get { return decimal.Parse(bid ?? "0"); } }
        public override decimal Ask { get { return decimal.Parse(ask ?? "0"); } }
        public override decimal Last { get { return decimal.Parse(last ?? "0"); } }
        public override decimal High { get { return decimal.Parse(high ?? "0"); } }
        public override decimal Low { get { return decimal.Parse(low ?? "0"); } }
        public override decimal Volume { get { return decimal.Parse(volume ?? "0"); } }
        public override string Timestamp { get { return this.timestamp ?? ""; } }
    }

    public class BlinkTradeOrderBookEntry : ZCryptoOrderBookEntry
    {
        public long timestamp { get; set; }
        public List<PriceAmount> bids { get; set; }
        public List<PriceAmount> asks { get; set; }
        public string pair { get; set; }
        public int id { get; set; }
        public string sell_total { get; set; }
        public string buy_total { get; set; }

        public override decimal Price { get { return decimal.MinValue; } }
        public override decimal Amount { get { return decimal.MinValue; } }
    }

    public class BlinkTradeOrderBook : ZCryptoOrderBook
    {
        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }
    }

} // end of namespace
