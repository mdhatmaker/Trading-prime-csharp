using System;
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
using CryptoApis.SharedModels;
using Bittrex.Net;
using Bittrex.Net.Objects;
using CryptoTools.Net;
using static CryptoTools.Global;

namespace Aggregator
{
    public class BittrexApi
    {
        private HMACSHA512 m_hmac;

        private const string URL = "https://bittrex.com/api/v1.1";
        private string m_apiKey;
        private string m_apiSecret;
        private byte[] m_secretKey;
        private HttpClient m_invokeClient;

        private Uri m_baseUri;

        private long m_msTimeAdjust = 0;                    // time adjustment in milliseconds

        private HttpClient m_client = new HttpClient();

        private BittrexClient m_bittrexClient;

        public BittrexApi(string apiKey, string apiSecret)
        {
            m_apiKey = apiKey;
            m_apiSecret = apiSecret;
            m_secretKey = Encoding.ASCII.GetBytes(apiSecret);
            m_hmac = new HMACSHA512(m_secretKey);

            m_baseUri = new Uri(URL);
            m_client.BaseAddress = m_baseUri;

            InitializeInvokeHttpClient();

            var options = new BittrexClientOptions();
            options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(m_apiKey, m_apiSecret);
            m_bittrexClient = new BittrexClient(options);

            var t = GetTicker("BTC-ETH");

            /*// Check the Binance server time and create a "time adjustment" relative to this machine's local time
            var dt = GetServerTime();
            var dtNow = DateTime.Now;
            Console.WriteLine("server time: {0}", dt.ToLongTimeString());
            Console.WriteLine(" local time: {0}", dtNow.ToLongTimeString());
            m_msTimeAdjust = dt.Subtract(dtNow).Milliseconds;
            Console.WriteLine("time adjust: {0} ms\n", m_msTimeAdjust);*/

            //var ai = GetAccountInfo();
            //m_balances = ai.GetNonZeroBalances();
        }

        private void InitializeInvokeHttpClient()
        {
            m_invokeClient = new HttpClient();
            //cl.BaseAddress = new Uri(uri);
            m_invokeClient.BaseAddress = m_baseUri;
            int _timeoutSec = 90;
            m_invokeClient.Timeout = new TimeSpan(0, 0, _timeoutSec);
            string _contentType = "application/json";
            m_invokeClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(_contentType));
            //var _credentialBase64 = "RWRnYXJTY2huaXR0ZW5maXR0aWNoOlJvY2taeno=";
            //cl.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", _credentialBase64));
            //m_invokeClient.DefaultRequestHeaders.Add("apikey", m_apiKey);
            m_invokeClient.DefaultRequestHeaders.Add("Accept-Language", "en;q=1.0");
            //m_invokeClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip;q=1.0, compress;q=0.5");
            // You can actually also set the User-Agent via a built-in property
            var _userAgent = "tradetronic";
            m_invokeClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);
            // You get the following exception when trying to set the "Content-Type" header like this:
            // cl.DefaultRequestHeaders.Add("Content-Type", _contentType);
            // "Misused header name. Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects."
        }

        public List<BittrexMarket> GetMarkets()
        {
            // https://bittrex.com/api/v1.1/public/getmarkets
            var markets = m_invokeClient.Get<BittrexGetMarketsResponse>("https://bittrex.com/api/v1.1/public/getmarkets");
            //var markets = m_invokeClient.Get<BittrexGetMarketsResponse>("/public/getmarkets");
            if (!markets.IsNull)
            {
                // markets is valid
                return markets.result;
            }

            return null;
        }

        // where symbol like "BTC-ETH"
        public BittrexPrice GetTicker(string symbol)
        {
            var res = m_bittrexClient.GetTicker(symbol.ToUpper());
            if (res.Success)
                return res.Data;
            else
            {
                Console.WriteLine("ERROR: {0} '{1}'", res.Error.Code, res.Error.Message);
                return null;
            }
        }

        public List<BittrexAccountBalance> GetBalances()
        {
            /*// https://bittrex.com/api/v1.1/account/getbalances?apikey=API_KEY&nonce=NONCE
            string req = GetSignature("/account/getbalances");
            var balances = m_invokeClient.Get<BittrexAccountBalanceResponse>(req);

            if (!balances.IsNull)
            {
                // markets is valid
                return balances.result;
            }

            return null;*/


            var res = m_bittrexClient.GetBalances();

            return null;
        }

        #region ---------- ORDERS -------------------------------------------------------------------------------------
        // BUY LIMIT order
        public BittrexOrder Buy(string symbol, decimal qty, decimal price)
        {
            return SubmitOrder(symbol.ToUpper(), "BUY", qty, price);
        }

        // SELL LIMIT order
        public BittrexOrder Sell(string symbol, decimal qty, decimal price)
        {
            return SubmitOrder(symbol.ToUpper(), "SELL", qty, price);
        }

        // Submit limit order (where side is "BUY" or "SELL")
        public BittrexOrder SubmitOrder(string symbol, string side, decimal qty, decimal price)
        {
            /*string sNewOrder = GetNewOrderString(symbol, side, qty, price);
            var o = PostEncrypt<BittrexOrder>("/api/v3/order", sNewOrder);

            if (!o.Result.IsNull)
            {
                // order is valid
            }

            return o.Result;*/
            return null;
        }
        #endregion ----------------------------------------------------------------------------------------------------

        // Get BinanceOrderBookTicker for ALL symbols
        public IDictionary<string, OrderBookTicker> GetOrderBookTickers()
        {
            var result = new SortedDictionary<string, OrderBookTicker>();
            /*var tickers = m_invokeClient.Get<OrderBookTickerList>("/api/v3/ticker/bookTicker");

            if (!tickers.IsNull)
            {
                // tickers are valid
                foreach (var t in tickers)
                {
                    result[t.symbol] = t;
                }
            }*/

            return result;
        }

        public void GetOrderBook()
        {
           // https://bittrex.com/api/v1.1/public/getorderbook?market=BTC-LTC&type=both
        }

        /*#region ---------- ICryptoApi ---------------------------------------------------------------
        public static BittrexApi Create(string encryptedCredentialsFile, string password)
        {
            var credentials = CryptoTools.Cryptography.Credentials.LoadFromFile(encryptedCredentialsFile, password);
            var cred = credentials["BITTREX"];
            return new BittrexApi(cred.Key, cred.Secret);
        }

        public string GetSymbol(string symbolId)
        {
            if (symbolId == "btcusd") return "XBTUSD";
            else if (symbolId == "ethusd") return "ETHUSD";
            else if (symbolId == "ethbtc") return "ETHBTC";
            else
            {
                Console.WriteLine("ERROR: Symbol ID not found.");
                return null;
            }
        }

        public async Task<XTicker> GetTicker(string symbolId)
        {
            string symbol = GetSymbol(symbolId);
            var res = await m_client.GetTickerInformation(symbol);
            return new XTicker(res.Result.Values.ToList()[0]);
        }

        public async Task<XBalances> GetBalances()
        {
            var res = await m_client.GetAccountBalance();
            return new XBalances(res.Result);
        }
        #endregion ----------------------------------------------------------------------------------
        */

        #region ---------- HMAC SIGNATURE -----------------------------------------------------------------------------
        // HMAC SHA512 signature (with timestamp nonce added)
        // where queryString like "symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.1&recvWindow=5000"
        public string GetSignature(string queryString)
        {
            string result = null;

            // add apikey and nonce to queryString like "apikey=f23439b&nonce=1499827319559"
            long timestamp = ToTimestampMilliseconds(DateTime.Now) + m_msTimeAdjust;
            result = string.Format("{0}{1}&apikey={2}&nonce={3}", URL, queryString, m_apiKey, timestamp);

            /*byte[] buffer = Encoding.ASCII.GetBytes(qs);
            byte[] hashValue = m_hmac.ComputeHash(buffer);*/

            // add signature to queryString like "&signature=c8db56825ae71d6d79447849e617115f4a920fa2acdcab2b053c4b2838bd6b71"
            string signature = GetHashValueSHA512(result);

            m_client.DefaultRequestHeaders.Add("apisign", signature);

            return result;
        }

        // HMAC SHA512 signature
        public string GetHashValueSHA512(string text)
        {
            string result = null;

            byte[] buffer = Encoding.ASCII.GetBytes(text);
            byte[] hashValue = m_hmac.ComputeHash(buffer);
            result = GetByteString(hashValue);

            return result;
        }
        #endregion ----------------------------------------------------------------------------------------------------


    } // end of class BittrexApi
} // end of namespace


// https://github.com/Coinio/Bittrex.Api.Client
// https://github.com/JKorf/Bittrex.Net
