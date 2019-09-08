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

namespace CryptoApis.Exchange
{

    public abstract class RestApiBase : ICryptoRestApi
    {
        protected string m_url;                             // = "https://api.hitbtc.com/";
        protected string m_apiKey;
        protected string m_apiSecret;
        private byte[] m_secretKey;
        private HttpClient m_invokeClient;

        private HMACSHA256 m_hmacSHA256;
        private HMACMD5 m_hmacMD5;
        private Uri m_baseUri;
        private HttpClient m_client = new HttpClient();
        private long m_msTimeAdjust = 0;                    // time adjustment in milliseconds

        protected CryptoTools.SymbolManager m_symbolManager;


        public RestApiBase(string url, string apiKey, string apiSecret)
        {
            m_url = url;
            m_apiKey = apiKey;
            m_apiSecret = apiSecret;
            m_secretKey = Encoding.ASCII.GetBytes(apiSecret);
            m_hmacSHA256 = new HMACSHA256(m_secretKey);
            m_hmacMD5 = new HMACMD5(m_secretKey);

            m_baseUri = new Uri(m_url);
            m_client.BaseAddress = m_baseUri;

            InitializeInvokeHttpClient();
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
            var _userAgent = "trader";
            m_invokeClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);
            // You get the following exception when trying to set the "Content-Type" header like this:
            // cl.DefaultRequestHeaders.Add("Content-Type", _contentType);
            // "Misused header name. Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects."
        }


        #region ---------- ICryptoApi ---------------------------------------------------------------
        public abstract string Exchange { get; }

        public abstract List<string> GetAllSymbols();

        public abstract string GetSymbol(string symbolId);

        public abstract Task<XTicker> GetTicker(string symbolId);

        public abstract Task<XBalanceMap> GetBalances();
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
            HttpContent content = new StringContent(queryString, Encoding.UTF8, "application/json");
            //m_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var result = await m_client.PostAsync(method, content);
            var result = await m_invokeClient.PostAsync(method, content);
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
            byte[] hashValue = m_hmacSHA256.ComputeHash(buffer);
            result = GetByteString(hashValue);

            return result;
        }

        // HMAC MD5 signature
        public string GetHashValueMD5(string text, string overrideSecret = null)
        {
            string result = null;

            byte[] buffer = Encoding.ASCII.GetBytes(text);
            byte[] hashValue;
            if (overrideSecret == null)
                hashValue = m_hmacMD5.ComputeHash(buffer);
            else
            {
                var secret = Encoding.ASCII.GetBytes(overrideSecret);
                var hmac = new HMACMD5(secret);
                hashValue = hmac.ComputeHash(buffer);
            }
            result = GetByteString(hashValue);

            return result;
        }
        #endregion ----------------------------------------------------------------------------------------------------


    } // end of class RestApiBase
} // end of namespace

