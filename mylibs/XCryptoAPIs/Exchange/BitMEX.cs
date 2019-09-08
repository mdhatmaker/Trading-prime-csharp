using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    // https://www.bitmex.com/app/apiOverview

    public class BitMEX : BaseExchange, IExchangeWebSocket
    {
        public override string BaseUrl { get { return "https://testnet.bitmex.com/api/v1"; } }
        public override string ExchangeName { get { return "BITMEX"; } }

        // SINGLETON
        public static BitMEX Instance { get { return m_instance; } }
        private static readonly BitMEX m_instance = new BitMEX();
        private BitMEX() { }

        // https://testnet.bitmex.com/api/v1/quote?symbol=XBTUSD

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    var stats = GET<List<BitMEXStat>>("https://testnet.bitmex.com/api/v1/stats");
                    m_symbolList = new List<string>();
                    foreach (var s in stats)
                    {
                        m_symbolList.Add(s.rootSymbol + s.currency.ToUpper());
                    }
                    m_symbolList.Sort();
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            throw new NotImplementedException();
        }

        public override async Task<Dictionary<string, ZTicker>> GetAllTickers()
        {
            throw new NotImplementedException();
        }

        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            throw new NotImplementedException();
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
                if (msg["table"] != null && msg["table"].ToString() == "orderBook10")
                {
                    //cout("BitMEX: orderBook10");
                    //cout(msg["table"]);
                    JArray array = msg["data"] as JArray;
                    //cout(array.Count);
                    var a1 = array[0];
                    var bids = a1["bids"] as JArray;
                    m_orderBook.Clear();
                    //cout("\n--------BIDS---------");
                    int bidCount = bids.Count;
                    for (int i = 0; i < bidCount; ++i)
                    {
                        var b = bids[i] as JArray;
                        string price = b[0].Value<string>();
                        int quantity = b[1].Value<int>();
                        m_orderBook.UpdateBid(price, quantity);
                        //cout("[{0}]  {1} {2}", i, price, quantity);
                    }
                    var asks = a1["asks"] as JArray;
                    int askCount = asks.Count;
                    //cout("--------ASKS---------");
                    for (int i = 0; i < askCount; ++i)
                    {
                        var a = asks[i] as JArray;
                        string price = a[0].Value<string>();
                        int quantity = a[1].Value<int>();
                        m_orderBook.UpdateAsk(price, quantity);
                        //cout("[{0}]  {1} {2}", i, price, quantity);
                    }
                    UpdateOrderBook();
                }
                else if (msg["info"] != null)
                {
                    cout(msg["info"]);
                }
                else if (msg["subscribe"] != null)
                {
                    cout(msg["subscribe"]);
                }
                else
                {
                    cout("Unknown BitMEX message type: ", Str(msg));
                    cout(e.Text);
                }
            }
            catch (Exception ex)
            {
                cout("ERROR: Exception processing BitMEX message: {0}", ex.Message);
                cout(e.Text);
            }
            return;
        }

        public void StartWebSocket(string[] args = null)
        {
            m_socket = new ZWebSocket("wss://www.bitmex.com/realtime", this.WebSocketMessageHandler);
        }

        // TODO: handle different args
        public void SubscribeWebSocket(string[] args = null)
        { 
            string json = @"{""op"": ""subscribe"", ""args"": [""orderBook10:XBTUSD""]}";
            m_socket.SendMessage(json);
        }
        #endregion -----------------------------------------------------------------------------------------------------


    } // end of class BitMEX

    public class BitMEXStat
    {
        public string rootSymbol { get; set; }          // "QTUM"
        public string currency { get; set; }            // "XBt"
        public long volume24h { get; set; }             // 25427788
        public long turnover24h { get; set; }           // 289548843529
        public long openInterest { get; set; }          // 378450
        public long openValue { get; set; }             // 4749366466
    } // end of class BitMEXStat

} // end of namespace
