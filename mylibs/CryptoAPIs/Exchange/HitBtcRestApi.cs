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
using CryptoApis.Models;
using Binance.Net;
using Binance.Net.Objects;
using CryptoTools.Net;
using static CryptoTools.Global;

namespace CryptoApis.Exchange.HitBtc
{
    // https://api.hitbtc.com/
    // https://github.com/hitbtc-com/hitbtc-api

    public class HitBtcRestApi : ICryptoRestApi
    {
        private HMACSHA256 m_hmac;

        private const string URL = "https://api.hitbtc.com/";
        private string m_apiKey;
        private string m_apiSecret;
        private byte[] m_secretKey;
        private HttpClient m_invokeClient;

        private Uri m_baseUri;
        private HttpClient m_client = new HttpClient();
        private long m_msTimeAdjust = 0;                    // time adjustment in milliseconds


        private CryptoTools.SymbolManager m_symbolManager;

        //private BinanceClient m_client;

        public HitBtcRestApi(string apiKey, string apiSecret)
        {
            //var options = new BinanceClientOptions();
            //options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(apiKey, apiSecret);
            //m_client = new BinanceClient(options);

            m_apiKey = apiKey;
            m_apiSecret = apiSecret;
            m_secretKey = Encoding.ASCII.GetBytes(apiSecret);
            m_hmac = new HMACSHA256(m_secretKey);

            m_baseUri = new Uri(URL);
            m_client.BaseAddress = m_baseUri;

            InitializeInvokeHttpClient();
        }

        public void Test()
        {
            // API test code goes here
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
            var _userAgent = "tradetronic";
            m_invokeClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);
            // You get the following exception when trying to set the "Content-Type" header like this:
            // cl.DefaultRequestHeaders.Add("Content-Type", _contentType);
            // "Misused header name. Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects."
        }

        public HitBtcSymbolList GetSymbols()
        {
            string method = "/api/2/public/symbol";
            var symbols = Get<HitBtcSymbolList>(method);
            return symbols;
        }

        public HitBtcTickerList GetAllTickers()
        {
            string method = "/api/2/public/ticker";
            var tickers = Get<HitBtcTickerList>(method);
            return tickers;
        }

        public HitBtcTicker GetOneTicker(string symbol)
        {
            string method = string.Format("/api/2/public/ticker/{0}", symbol);
            var ticker = Get<HitBtcTicker>(method);
            return ticker;
        }

        public HitBtcCandleList GetCandles(string symbol, string period = "M1", int limit=1000)
        {
            string method = string.Format("/api/2/public/candles/{0}?period={1}&limit={2}", symbol, period, limit);
            var candles = Get<HitBtcCandleList>(method);
            return candles;
        }

        #region ---------- ICryptoApi ---------------------------------------------------------------
        public string Exchange { get { return "HITBTC"; } }

        public List<string> GetAllSymbols()
        {
            var result = new List<string>();
            var symbols = GetSymbols();            
            foreach (var s in symbols)
            {
                result.Add(s.id);
            }
            return result;
        }

        public string GetSymbol(string symbolId)
        {
            if (m_symbolManager == null) m_symbolManager = new CryptoTools.SymbolManager();

            string symbol = m_symbolManager.GetSymbol(Exchange, symbolId);
            if (symbol != null)
                return symbol;
            else
            {
                Console.WriteLine("ERROR: Symbol ID not found.");
                return null;
            }
        }

        public async Task<XTicker> GetTicker(string symbolId)
        {
            string symbol = GetSymbol(symbolId);
            var ticker = GetOneTicker(symbol);
            return new XTicker(ticker);
        }

        public async Task<XBalanceMap> GetBalances()
        {
            //var res = await m_client.GetAccountInfoAsync();
            //return new XBalanceMap(res.Data);
            return null;
        }
        #endregion ----------------------------------------------------------------------------------



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


    } // end of class HitBtcRestApi
} // end of namespace

