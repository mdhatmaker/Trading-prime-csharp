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
    public class Cex : BaseExchange, IExchangeWebSocket
    {
        public override string BaseUrl { get { return "https://cex.io/api"; } }
        public override string ExchangeName { get { return "CEX"; } }

        // SINGLETON
        public static Cex Instance { get { return m_instance; } }
        private static readonly Cex m_instance = new Cex();
        private Cex() { }

        public int Depth = 20;

        private static readonly Encoding U8 = Encoding.UTF8;

        public void Test()
        {
            var res = LoadFile(@"/Users/michael/Projects/JSON/cex_test.json");
            var w = DeserializeJSON<CexOHLCV>(res);

            var x = GetTicker("BTC/USD");
            cout(x);

            var y = GetHistorical("20160228", "BTC", "USD");
            cout(y);

            var z = GetOrderBook("BTC/USD");
            cout(z);

        }

        private static string[] currency_pairs =
        {
            "BTC/USD", "ETH/USD", "BCH/USD", "DASH/USD", "ZEC/USD",
            "BTC/EUR", "ETH/EUR", "BCH/EUR", "DASH/EUR", "ZEC/EUR",
            "BTC/GBP", "ETH/GBP", "BCH/GBP", "DASH/GBP", "ZEC/GBP",
            "BTC/RUB",
            "ETH/BTC", "BCH/BTC", "DASH/BTC", "ZEC/BTC", "GHS/BTC"
        };

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    m_symbolList = currency_pairs.ToList();
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string pair)
        {
            //string[] substrings = pair.Split('/');
            //return GET<CexTicker>("https://cex.io/api/ticker/{0}/{1}", substrings[0], substrings[1]);
            return GET<CexTicker>("https://cex.io/api/ticker/{0}", pair);
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

        public override ZCryptoOrderBook GetOrderBook(string pair)
        {
            string[] substrings = pair.Split('/');
            var book = GET<CexOrderBook>("https://cex.io/api/order_book/{0}/{1}/?depth={2}", substrings[0], substrings[1], this.Depth);
            return book as ZCryptoOrderBook;
        }

        // Historical 1m OHLCV Chart
        public CexOHLCV GetHistorical(string dateYYYYMMDD, string symbol1, string symbol2)
        {
            return GET<CexOHLCV>("https://cex.io/api/ohlcv/hd/{0}/{1}/{2}", dateYYYYMMDD, symbol1, symbol2);
        }

        //https://cex.io/api/currency_limits

        public string CreateAuthRequest()
        {
            string timestamp = Math.Floor(GDate.DateTimeToUnixTimestamp(DateTime.Now)).ToString();

            //HMACSHA512 hmac = new HMACSHA512(U8.GetBytes(apiSecret));
            HMACSHA256 hmac = new HMACSHA256(U8.GetBytes(ApiSecret));
            byte[] hashmessage = hmac.ComputeHash(U8.GetBytes(timestamp + ApiKey));
            string sign = ToHexString(hashmessage);

            string authRequest = @"{
                ""e"": ""auth"",
                ""auth"": {
                    ""key"": """ + ApiKey + @""",
                    ""signature"": """ + sign + @""",
                    ""timestamp"": " + timestamp + @"
                }
            }";
            return authRequest;
        }

        private void print_bids_asks(JArray bids, JArray asks)
        {
            cout("\n--------BIDS---------");
            int bidCount = bids.Count;
            for (int i = 0; i < bidCount; ++i)
            {
                var b = bids[i] as JArray;
                float price = b[0].Value<float>();
                long quantity = b[1].Value<long>();
                cout("[{0}]  {1} {2}", i, price, quantity);
            }
            cout("--------ASKS---------");
            int askCount = asks.Count;
            for (int i = 0; i < askCount; ++i)
            {
                var a = asks[i] as JArray;
                float price = a[0].Value<float>();
                long quantity = a[1].Value<long>();
                cout("[{0}]  {1} {2}", i, price, quantity);
            }
        }

        private void print_bids_asks2(JObject bids, JObject asks)
        {
            cout("\n--------BIDS---------");
            foreach (var prop in bids.Properties())
            {
                string name = prop.Name;
                var value = prop.Value;
                m_orderBook.UpdateBid(name, value.ToString());
                cout("{0} {1}", name, value);
            }
            cout("--------ASKS---------");
            int askCount = asks.Count;
            foreach (var prop in asks.Properties())
            {
                string name = prop.Name;
                var value = prop.Value;
                m_orderBook.UpdateAsk(name, value.ToString());
                cout("{0} {1}", name, value);
            }
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

    public class CexTicker : ZTicker
    {
        public string bid { get; set; }         // highest buy order
        public string ask { get; set; }         // lowest sell order
        public string low { get; set; }         // last 24 hours price low
        public string high { get; set; }        // last 24 hours price high
        public string last { get; set; }        // last BTC price
        public string volume { get; set; }      // last 24 hours volume
        public string volume30d { get; set; }   // last 30 days volume
        public string timestamp { get; set; }

        public override decimal Bid { get { return decimal.Parse(bid); } }
        public override decimal Ask { get { return decimal.Parse(ask); } }
        public override decimal Last { get { return decimal.Parse(last); } }
        public override decimal High { get { return decimal.Parse(high); } }
        public override decimal Low { get { return decimal.Parse(low); } }
        public override decimal Volume { get { return decimal.Parse(volume); } }
        public override string Timestamp { get { return timestamp; } }
    }

    public class CexOHLCVDataPoint
    {
        public long timestamp { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public float volume { get; set; }
    }

    public class CexOHLCV
    {
        public long time { get; set; }
        public List<List<float>> data1m { get; set; }
    }

    public class PriceAmount
    {
        public float price { get; set; }
        public float amount { get; set; }
    }

    public class CexOrderBookEntry : ZCryptoOrderBookEntry
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

    public class CexOrderBook : ZCryptoOrderBook
    {
        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }
    }

} // end of namespace
