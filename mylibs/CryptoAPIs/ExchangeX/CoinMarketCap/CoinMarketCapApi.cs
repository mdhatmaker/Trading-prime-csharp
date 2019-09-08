using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoTools.Net;
using static CryptoTools.Global;
using HtmlAgilityPack;
using System.Net;

namespace CryptoApis.ExchangeX.CoinMarketCap
{
    // https://coinmarketcap.com/api/

    public class CoinMarketCapApi
    {
		private static Dictionary<string, List<CmcMarketsRow>> m_markets = new Dictionary<string, List<CmcMarketsRow>>();

        private HMACSHA256 m_hmac;

        private const string URL = "https://api.coinmarketcap.com";
        private string m_apiKey;
        private string m_apiSecret;
        private byte[] m_secretKey;
        private HttpClient m_invokeClient;

        private Uri m_baseUri;

        private long m_msTimeAdjust = 0;                    // time adjustment in milliseconds

        private HttpClient m_client = new HttpClient();

        public CoinMarketCapApi()   //string apiKey, string apiSecret)
        {
            //m_apiKey = apiKey;
            //m_apiSecret = apiSecret;
            //m_secretKey = Encoding.ASCII.GetBytes(apiSecret);
            //m_hmac = new HMACSHA256(m_secretKey);

            m_baseUri = new Uri(URL);
            m_client.BaseAddress = m_baseUri;

            InitializeInvokeHttpClient();
        }

        public void Test()
        {
            //var li = GetRankings(50);
            var li = GetRankings();
            li.ForEach(t => Console.WriteLine(t));
            //li.Where(t => t.market_cap_usd > 0).ToList().ForEach(t => Console.WriteLine(t));

            Console.WriteLine("\n*** DONE ***\n");
        }

        private void InitializeInvokeHttpClient()
        {
            m_invokeClient = new HttpClient();
            //cl.BaseAddress = new Uri(uri);
            m_invokeClient.BaseAddress = m_baseUri;
            int _timeoutSec = 90;
            m_invokeClient.Timeout = new TimeSpan(0, 0, _timeoutSec);
            string _contentType = "application/json";
            m_invokeClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
            //var _credentialBase64 = "RWRnYXJTY2huaXR0ZW5maXR0aWNoOlJvY2taeno=";
            //cl.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", _credentialBase64));
            m_invokeClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", m_apiKey);
            // You can actually also set the User-Agent via a built-in property
            var _userAgent = "api_request";
            m_invokeClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);
            // You get the following exception when trying to set the "Content-Type" header like this:
            // cl.DefaultRequestHeaders.Add("Content-Type", _contentType);
            // "Misused header name. Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects."
        }


        // coincodex.com (apparently) uses the same id provided by CoinMarketCap and can retrieve the exchanges for a specific coin as follows
        // https://coincodex.com/crypto/{id}/exchanges/
        // ex: https://coincodex.com/crypto/funfair/exchanges/

        // Get rankings by market cap (default limit is ALL results)
        public CoinMarketCapTickerList GetRankings(int limit = 0, bool includeZeroMarketCap = false)
        {
            string method = string.Format("/v1/ticker/?limit={0}", limit);
            var tickers = Get<CoinMarketCapTickerList>(method);
            //tickers.ForEach(t => Console.WriteLine(t));
            if (includeZeroMarketCap == false)
                tickers.RemoveZeroMarketCap();
            return tickers;
        }

        // where id like "DCR", "ETH", "BTC", ...
		public CoinMarketCapTicker GetRankingTicker(string id)
		{
			var rankings = GetRankings(includeZeroMarketCap: true);
			return rankings.Where(r => r.symbol == id).FirstOrDefault();
		}

        // where id like "bitcoin"
        public CoinMarketCapTicker GetTicker(string id)
        {
            string method = string.Format("/v1/ticker/{0}", id);
            var tickers = Get<CoinMarketCapTickerList>(method);
            return tickers[0];
        }

        // Get summary data regarding global crypto markets
        public CoinMarketCapGlobalData GetGlobalData()
        {
            string method = "/v1/global";
            var globdata = Get<CoinMarketCapGlobalData>(method);
            return globdata;
        }

		public IEnumerable<CoinMarketCapTicker> GetBiggestGainers1h(int ntake = 250, bool display = false)
        {
            var cmc = new CoinMarketCapApi();
            var rankings = cmc.GetRankings();

            if (display) Console.WriteLine("\n---Biggest Gainers 1h---");
            var gainers1h = rankings.OrderByDescending(t => t.percent_change_1h).Take(ntake);
            foreach (var t in gainers1h)
                if (display) t.Print();
            return gainers1h;
        }

        public IEnumerable<CoinMarketCapTicker> GetBiggestGainers24h(int ntake = 250, bool display = false)
        {
            var cmc = new CoinMarketCapApi();
            var rankings = cmc.GetRankings();

            if (display) Console.WriteLine("\n---Biggest Gainers 24h---");
            var gainers24h = rankings.OrderByDescending(t => t.percent_change_24h).Take(ntake);
            foreach (var t in gainers24h)
                if (display) t.Print();
            return gainers24h;
        }

		// Given a coin name
        // Return the list of Markets where this coin trades (i.e. "GCN/BTC" on "Cryptopia")
        // where name like "GCN Coin"
        public static List<CmcMarketsRow> GetMarkets(string name, bool forceUpdate = false)
        {
            // Format name to lowercase and replace space with dash ("-")
            name = name.ToLower().Replace(' ', '-');

            if (m_markets.ContainsKey(name) && !forceUpdate) return m_markets[name];

            var result = new List<CmcMarketsRow>();

            string url = string.Format("https://coinmarketcap.com/currencies/{0}/#markets", name);

            try
            {
                using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                {
                    //client.DownloadFile("http://yoursite.com/page.html", @"C:\localfile.html");
                    // Or you can get the file content without saving it:
                    string htmlCode = client.DownloadString(url);
                    //...

                    //Console.WriteLine(htmlCode);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlCode);
                    var myTable = doc.DocumentNode
                     .Descendants("table")
                     .Where(t => t.Attributes["id"].Value == "markets-table")
                     .FirstOrDefault();

                    if (myTable != null)
                    {
                        // further parsing here
                        //List<HtmlNode> rows = doc.DocumentNode.SelectNodes("//tr").Skip(10).ToList();
                        var rows = myTable.ChildNodes["tbody"].ChildNodes;
                        foreach (var r in rows)
                        {
                            if (r.Name == "tr")
                            {
                                //Console.WriteLine(r.InnerText);
                                var cr = new CmcMarketsRow(r.InnerText);
                                //Console.WriteLine(cr);
                                result.Add(cr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("'{0}' : {1}", name, ex.Message);
            }

            m_markets[name] = result;

            return result;
        }

        // Given a CoinMarketCap exchange name
        // Return the exchange id used to identify exchanges in ExchangeSharpRestApi
		public static string GetExchangeId(string exchangeName)
		{
			var exchanges = new Dictionary<string, string>() {
				{ "Cryptopia", "CRYPTOPIA" },
				{ "YoBit", "YOBIT" },
				{ "Livecoin", "LIVECOIN" },
				{ "HitBTC", "HITBTC" },
				{ "Poloniex", "POLONIEX" },
				{ "Bleutrade", "BLUETRADE" },
				{ "Kucoin", "KUCOIN" },
				{ "Bittrex", "BITTREX" },
				{ "Huobi", "HUOBI" },
				{ "Binance", "BINANCE" },
				{ "Bithumb", "BITHUMB" },
				{ "Kraken", "KRAKEN" },
				{ "Bitfinex", "BITFINEX" },
				{ "Tux Exchange", "TUX" },
				{ "Nanex", "NANEX" },
				{ "EtherDelta (ForkDelta)", "ETHERDELTA" },
				{ "Waves Decentralized Exchange", "WAVES" }};

			return exchanges[exchangeName];
		}

        #region ---------- GET/POST -----------------------------------------------------------------------------------
        // GET (with HMAC SHA256 encryption)
        // where method like "/api/v3/account" and query like "recvWindow=5000"
        // where T like BinanceAccountInfo
        public T GetEncrypt<T>(string method, string query) where T : NullableObject
        {
            string queryString = GetSignature(query);
            //Console.WriteLine(queryString);
                        
            string requestUri = method + "?" + queryString;
            string resultContent = m_invokeClient.Invoke("GET", requestUri, "");
            //Console.WriteLine(resultContent);

            var obj = JsonConvert.DeserializeObject<T>(resultContent);

            if (obj.IsNull)     // an error occurred during POST
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;         //return default(T);
        }

        // POST (with HMAC SHA256 encryption)
        // where method like "/api/v3/order" and query like "symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.01&recvWindow=5000"
        // where T like BinanceNewOrder
        public async Task<T> PostEncrypt<T>(string method, string query) where T : NullableObject
        {
            string queryString = GetSignature(query);
            //Console.WriteLine(queryString);
            
            HttpContent content = new StringContent(queryString);
            content.Headers.Add("X-MBX-APIKEY", m_apiKey);
            var result = await m_client.PostAsync(method, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(resultContent);

            var obj = JsonConvert.DeserializeObject<T>(resultContent);

            if (obj.IsNull)     // an error occurred during POST
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;
        }

        // POST (no encryption)
        // where method like "/api/v3/order" and query like "symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.01&recvWindow=5000"
        // where T like BinanceNewOrder
        public async Task<T> Post<T>(string method, string queryString) where T : NullableObject
        {
            //Console.WriteLine(queryString);
            HttpContent content = new StringContent(queryString);
            var result = await m_client.PostAsync(method, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(resultContent);

            var obj = JsonConvert.DeserializeObject<T>(resultContent);

            if (obj.IsNull)     // an error occurred during POST
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;
        }

        // POST (no encryption)
        // where method like "/api/v1/userDataStream"
        // where T like BinanceStartUserStream
        public T Post<T>(string method) where T : NullableObject
        {
            //Console.WriteLine(method);
            string resultContent = m_invokeClient.Invoke("POST", method, "");
            //Console.WriteLine(resultContent);

            var obj = JsonConvert.DeserializeObject<T>(resultContent);

            if (obj.IsNull)     // an error occurred during POST
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;
        }

        // GET (no encryption)
        // where method like "/api/v3/ticker/bookTicker"
        // where T like BinanceBookTicker
        public T Get<T>(string method) where T : NullableObject
        {
            //Console.WriteLine(method);
            string resultContent = m_invokeClient.Invoke("GET", method, "");
            //Console.WriteLine(resultContent);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var obj = JsonConvert.DeserializeObject<T>(resultContent, settings);

            if (obj.IsNull)     // an error occurred during GET
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;
        }

        // PUT (no encryption)
        // where method like "/api/v1/userDataStream?listenKey=pqia91ma19a5s61cv6a81va65sdf19v8a65a1a5s61cv6a81va65sdf19v8a65a1"
        // where T like BinanceBookTicker
        public T Put<T>(string method) where T : NullableObject
        {
            //Console.WriteLine(method);
            string resultContent = m_invokeClient.Invoke("PUT", method, "");
            //Console.WriteLine(resultContent);

            var obj = JsonConvert.DeserializeObject<T>(resultContent);

            if (obj.IsNull)     // an error occurred during POST
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;
        }
        #endregion ----------------------------------------------------------------------------------------------------


        #region ---------- HMAC SIGNATURE -----------------------------------------------------------------------------
        // HMAC SHA256 signature (with timestamp added)
        // where queryString like "symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.1&recvWindow=5000"
        public string GetSignature(string queryString)
        {
            string result = null;

            // add timestamp to queryString like "&timestamp=1499827319559"
            long timestamp = ToTimestampMilliseconds(DateTime.Now) + m_msTimeAdjust;
            string qs = string.Format("{0}&timestamp={1}", queryString, timestamp);

            /*byte[] buffer = Encoding.ASCII.GetBytes(qs);
            byte[] hashValue = m_hmac.ComputeHash(buffer);*/

            // add signature to queryString like "&signature=c8db56825ae71d6d79447849e617115f4a920fa2acdcab2b053c4b2838bd6b71"
            result = string.Format("{0}&signature={1}", qs, GetHashValueSHA256(qs));

            return result;
        }

        // HMAC SHA256 signature
        public string GetHashValueSHA256(string text)
        {
            string result = null;

            byte[] buffer = Encoding.ASCII.GetBytes(text);
            byte[] hashValue = m_hmac.ComputeHash(buffer);
            result = GetByteString(hashValue);

            return result;
        }
        #endregion ----------------------------------------------------------------------------------------------------



    } // end of class CoinMarketCapApi
} // end of namespace

