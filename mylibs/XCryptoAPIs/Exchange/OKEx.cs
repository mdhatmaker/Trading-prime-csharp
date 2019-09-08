using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;
using Newtonsoft.Json;

namespace CryptoAPIs.Exchange
{
    public class OKEx : BaseExchange, IExchangeWebSocket
    {
        public override string BaseUrl { get { return "https://www.okex.com"; } }
        public override string ExchangeName { get { return "OKEX"; } }

        // SINGLETON
        public static OKEx Instance { get { return m_instance; } }
        private static readonly OKEx m_instance = new OKEx();
        private OKEx() { }

        private static readonly HttpClient m_httpClient = new HttpClient();

        private Dictionary<string, int> m_channelIds = new Dictionary<string, int>();


        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    m_symbolList = new List<string>() {
                    "ltc_btc", "eth_btc", "etc_btc", "bch_btc", "btc_usdt", "eth_usdt", "ltc_usdt", "etc_usdt", "bch_usdt", "etc_eth",
                    "bt1_btc", "bt2_btc", "btg_btc", "qtum_btc", "hsr_btc", "neo_btc", "gas_btc", "qtum_usdt", "hsr_usdt", "neo_usdt", "gas_usdt"
                };
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            var request = GET<OKExTicker>(string.Format("{0}/api/v1/ticker.do?symbol={1}", BaseUrl, symbol));
            request.Symbol = symbol;
            return request;
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

        public override ZCryptoOrderBook GetOrderBook(string symbol)    //="ltc_btc")
        {
            var request = GET<OKExOrderBook>(string.Format("{0}/api/v1/depth.do?symbol={1}", BaseUrl, symbol));
            //request = GET<OKExOrderBook>(string.Format("https://www.okex.com/api/v1/depth.do?symbol=qtum_btc"));
            return request as ZCryptoOrderBook;
        }

        public List<OKExSymbolDetail> GetSymbolDetails()
        {
            var request = GET<List<OKExSymbolDetail>>("{0}/symbols_details", BaseUrl);
            return request;
        }

        /*public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            var request = GET<OKExOrderBook>(string.Format("{0}/api/v1/depth.do?symbol={1}", BaseUrl, symbol));
            request.Symbol = symbol;
            return request;
        }*/

        /*public List<List<string>> GetOrderBook(string symbol)
        {
            return new OrderBook(GetBitfinexOrderBook(symbol));
        }*/


        #region --------------------------------------------------------------------------------------------------------
        public void WebSocketMessageHandler(MessageArgs e)
        {
            //cout(e.Text);
            if (e.Text.StartsWith("["))
            {
                JArray array = JArray.Parse(e.Text);
                int chanId = array[0].Value<int>();
                if (array[1] is JArray)
                {
                    JArray book = array[1].Value<JArray>();
                    if (book[0] is JArray)
                    {
                        //m_orderBook.Clear();
                        for (int i = 0; i < book.Count; ++i)
                        {
                            var bookEntry = book[i].Value<JArray>();
                            float price = bookEntry[0].Value<float>();
                            int ordersAtLevel = bookEntry[1].Value<int>();
                            float amount = bookEntry[2].Value<float>();
                            //cout("{0} {1} {2}", price, ordersAtLevel, amount);
                            if (ordersAtLevel == 0)
                            {
                                m_orderBook.UpdateBid(price.ToString(), 0);
                                m_orderBook.UpdateAsk(price.ToString(), 0);
                            }
                            else
                            {
                                if (amount >= 0)     // bid
                                {
                                    m_orderBook.UpdateBid(price.ToString(), amount.ToString());
                                }
                                if (amount <= 0)    // ask
                                {
                                    m_orderBook.UpdateAsk(price.ToString(), (-amount).ToString());
                                }
                            }
                        }
                        UpdateOrderBook();
                    }
                    else        // only contains a SINGLE book entry (a single list containing 3 values)
                    {
                        float price = book[0].Value<float>();
                        int ordersAtLevel = book[1].Value<int>();
                        float amount = book[2].Value<float>();
                        //cout("{0} {1} {2}", price, ordersAtLevel, amount);
                        if (ordersAtLevel == 0)
                        {
                            m_orderBook.UpdateBid(price.ToString(), 0);
                            m_orderBook.UpdateAsk(price.ToString(), 0);
                        }
                        else
                        {
                            if (amount >= 0)     // bid
                            {
                                m_orderBook.UpdateBid(price.ToString(), amount.ToString());
                            }
                            if (amount <= 0)    // ask
                            {
                                m_orderBook.UpdateAsk(price.ToString(), (-amount).ToString());
                            }
                        }
                        UpdateOrderBook();
                    }
                }
                else
                {
                    if (array[1].Type == JTokenType.String && array[1].Value<string>() == "hb")
                    {
                        cout("HEARTBEAT!");
                    }
                    else
                    {
                        cout("OKEx: Unknown message: {0}", array[1]);
                    }
                }
            }
            else
            {
                JObject msg = JObject.Parse(e.Text);
                //dynamic dict = JContainer.Parse(e.Text);
                //dynamic list = JArray.Parse(e.Text);
                if (msg["event"] != null)
                {
                    string msgType = msg["event"].Value<string>();
                    if (msgType == "info")
                    {
                        cout("OKEx: info");
                    }
                    else if (msgType == "subscribed")
                    {
                        cout("OKEx: subscribed");
                        string symbol = msg["symbol"].Value<string>();
                        int chanId = msg["chanId"].Value<int>();
                        m_channelIds[symbol] = chanId;
                        cout("symbol={0}    chanId={1}", symbol, chanId);
                    }
                }
                /*else
                {
                    //JArray arr = msg.Values();
                }*/
                else
                {
                    cout("Unknown message type: ", Str(msg));
                }
            }

            return;
        }

        public void StartWebSocket(string[] args = null)
        {
            m_socket = new ZWebSocket("wss://api.bitfinex.com/ws/2", this.WebSocketMessageHandler);
        }

        // TODO: handle different args
        public void SubscribeWebSocket(string[] args = null)
        {
            string pair = "BTCUSD";
            string json = @"{ ""event"":""subscribe"", ""channel"":""book"", ""pair"":""" + pair + @""", ""prec"":""P0"" }";
            m_socket.SendMessage(json);
        }
        #endregion -----------------------------------------------------------------------------------------------------


    } // end of class Bitfinex

    //======================================================================================================================================

    public class OkExTickerX
    {
        public string last { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string vol { get; set; }
        public string buy { get; set; }
        public string sell { get; set; }
    }

    public class OKExTicker : ZTicker
    {
        public string date { get; set; }
        public OkExTickerX ticker { get; set; }

        public override decimal Bid { get { return decimal.Parse(ticker.buy); } }
        public override decimal Ask { get { return decimal.Parse(ticker.sell); } }
        public override decimal Last { get { return decimal.Parse(ticker.last); } }
        public override decimal High { get { return decimal.Parse(ticker.high); } }
        public override decimal Low { get { return decimal.Parse(ticker.low); } }
        public override decimal Volume { get { return decimal.Parse(ticker.vol); } }
        public override string Timestamp { get { return date; } }
    } // end of class OKExTicker

    public class OKExSymbolDetail
    {
        public string pair { get; set; }
        public int price_precision { get; set; }
        public string initial_margin { get; set; }
        public string minimum_margin { get; set; }
        public string maximum_order_size { get; set; }
        public string minimum_order_size { get; set; }
        public string expiration { get; set; }
    } // end of class OKExSymbolDetail

    /*[JsonArray]
    public class OKExOrderBookEntry : CryptoOrderBookEntry
    {
        [JsonProperty(Order = 1)]
        //[DataMember(Order = 1)]
        public decimal price { get; set; }
        [JsonProperty(Order = 2)]
        //[DataMember(Order = 2)]
        public decimal amount { get; set; }
        //public string timestamp { get; set; }

        public override string Price { get { return price.ToString(); } }
        public override string Amount { get { return amount.ToString(); } }
        //public override string Timestamp { get { return ""; } }

        //public float Price { get { return float.Parse(price); } }
        //public float Amount { get { return float.Parse(amount); } }
        //public DateTime Timestamp { get { return GDate.UnixTimeStampToDateTime(double.Parse(timestamp)); } }
    } // end of class OKExOrderBookEntry*/


    public class OKExOrderBook : ZCryptoOrderBook
    {
        //public List<OKExOrderBookEntry> asks { get; set; }
        //public List<OKExOrderBookEntry> bids { get; set; }
        public List<List<decimal>> bids { get; set; }
        public List<List<decimal>> asks { get; set; }

        public override List<ZCryptoOrderBookEntry> Bids { get; }
        public override List<ZCryptoOrderBookEntry> Asks { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("BIDS\n");
            int ix = 0;
            foreach (var b in bids)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, b[0], b[1]));
            }
            sb.Append("ASKS\n");
            ix = 0;
            //revasks = as
            foreach (var a in asks)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, a[0], a[1]));
            }
            return sb.ToString();
        }
    } // end of class OKExOrderBook


} // end of namespace
