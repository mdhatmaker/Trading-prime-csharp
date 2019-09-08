using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    // https://github.com/huobiapi/API_Docs_en/wiki
    // https://github.com/huobiapi/API_Docs_en/wiki/REST_Reference
    // https://api.huobi.com/apiv2.php
    // https://www.huobi.com/help/index.php?a=api_help_v2#con_1
    // https://github.com/huobiapi/API_Docs/wiki
    // https://github.com/huobiapi/API_Docs_en/wiki/Huobi.pro-API

    public class Huobi : BaseExchange, IExchangeWebSocket
    {
        public override string BaseUrl { get { return "https://api.huobi.pro"; } }
        public override string ExchangeName { get { return "HUOBI"; } }

        // SINGLETON
        public static Huobi Instance { get { return m_instance; } }
        private static readonly Huobi m_instance = new Huobi();
        private Huobi() { }


        class HuobiResponse<T>
        {
            public string status { get; set; }
            public T data { get; set; }
        }

        public override List<string> SymbolList
        {
            get
            {
                if (m_symbolList == null)
                {
                    var details = GetSymbolDetails();
                    m_symbolList = new List<string>();
                    foreach (var d in details)
                    {
                        m_symbolList.Add(d.Symbol);
                    }
                    m_symbolList.Sort();

                    SupportedSymbols = new Dictionary<string, ZCurrencyPair>();
                    foreach (var s in m_symbolList)
                    {
                        var pair = ZCurrencyPair.FromSymbol(s, CryptoExch.HUOBI);
                        SupportedSymbols[pair.Symbol] = pair;
                    }
                }
                return m_symbolList;
            }
        }

        public override ZTicker GetTicker(string symbol)
        {
            return GET<HuobiTicker>("https://be.huobi.com/market/detail/merged?symbol={0}", symbol);
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

        // type is step0,step1,step2,step3,step4,step5
        public override ZCryptoOrderBook GetOrderBook(string symbol)
        {
            string type = "step0";
            var book = GET<HuobiOrderBookResponse>("{0}/market/depth?symbol={1}&type={2}", BaseUrl, symbol, type);
            return book.tick as ZCryptoOrderBook;
        }

        public List<HuobiSymbolDetail> GetSymbolDetails()
        {
            var request = GET<HuobiResponse<List<HuobiSymbolDetail>>>(string.Format("{0}/v1/common/symbols", BaseUrl));
            return request.data;
        }

        // Where symbol like "btcusdt"
        // Where period like "1min", "5min", "15min", "30min", "60min", "1day", "1mon", "1week", "1year"
        public void GetKline(string symbol, string period)
        {
            string url = string.Format("https://api.huobi.pro/market/history/kline?symbol={0}&period={1}", symbol, period);

        }

        #region --------------------------------------------------------------------------------------------------------
        public void WebSocketMessageHandler(MessageArgs e)
        {
            cout(e.Text);
        }

        // TODO: Looks like there are different WebSocket addresses for different products
        public void StartWebSocket(string[] args)
        {
            // ETH/CNY、ETC/CNY   wss://be.huobi.com/ws
            // BTC/CNY、LTC/CNY   wss://api.huobi.com/ws
            // ETH/BTC、LTC/BTC、ETC/BTC, BCC/BTC   wss://api.huobi.pro/ws
            m_socket = new ZWebSocket("wss://be.huobi.com/ws", this.WebSocketMessageHandler);
        }

        // TODO: handle different args
        public void SubscribeWebSocket(string[] args)
        {
            string json = @"{
                ""sub"": ""market.btccny.depth.step0"",
                ""id"": ""id1""
            }";
            m_socket.SendMessage(json);
        }
        #endregion -----------------------------------------------------------------------------------------------------


    } // end of class Huobi

    //======================================================================================================================================

    public class HuobiOrderBookResponse
    {
        public string status { get; set; }          // "ok"
        public string ch { get; set; }              // "market.btcusdt.depth.step0"
        public long ts { get; set; }                // 1517797496310
        public HuobiOrderBook tick { get; set; }
    }

    public class HuobiOrderBook : ZCryptoOrderBook
    {
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
            foreach (var a in asks)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}\n", ++ix, a[0], a[1]));
            }
            return sb.ToString();
        }
    }

    // https://be.huobi.com/market/detail/merged?symbol={0}

    // {"status":"ok","ch":"market.ethcny.detail.merged","ts":1509129343543,"tick":{"amount":3548.6170,
    // "open":1807.8100,"close":1813.0000,"high":1825.0000,"ts":1509128869000,"id":1509128869,"count":2120,"low":1781.4000,
    // "ask":[1812.0000,0.1110],"vol":6387413.10992000,"bid":[1805.2000,2.4000]}}

    public class HuobiTicker : ZTicker
    {
        public string status { get; set; }
        public string ch { get; set; }
        public long ts { get; set; }
        public HuobiTickerData tick { get; set; }

        public override decimal Bid { get { return (decimal)tick.bid[0]; } }
        public override decimal Ask { get { return (decimal)tick.ask[0]; } }
        public override decimal Last { get { return (decimal)tick.close; } }
        public override decimal High { get { return (decimal)tick.high; } }
        public override decimal Low { get { return (decimal)tick.low; } }
        public override decimal Volume { get { return (decimal)tick.vol; } }
        public override string Timestamp { get { return ts.ToString(); } }
    }

    public class HuobiTickerData
    {
        public float amount { get; set; }
        public float open { get; set; }
        public float close { get; set; }
        public float high { get; set; }
        public long ts { get; set; }
        public long id { get; set; }
        public int count { get; set; }
        public float low { get; set; }
        public List<float> ask { get; set; }
        public float vol { get; set; }
        public List<float> bid { get; set; }
    }

    public class HuobiSymbolDetail
    {
        [JsonProperty("base-currency")]
        public string baseCurrency { get; set; }        // "omg"
        [JsonProperty("quote-currency")]
        public string quoteCurrency { get; set; }       // "usdt"
        [JsonProperty("price-precision")]
        public int pricePrecision { get; set; }         // 2
        [JsonProperty("amount-precision")]
        public int amountPrecision { get; set; }        // 4
        [JsonProperty("symbol-partition")]
        public string symbolPartition { get; set; }     // "main"

        public string Symbol { get { return baseCurrency + quoteCurrency; }}
    }


} // end of namespace
