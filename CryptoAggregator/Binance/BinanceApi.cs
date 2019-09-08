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
using CryptoTools.Net;
using static CryptoTools.Global;

namespace Aggregator
{
    public class BinanceApi
    {
        public IDictionary<string, BinanceAccountBalance> Balances { get { return m_balances; } }

        private HMACSHA256 m_hmac;

        private const string URL = "https://api.binance.com";
        private string m_apiKey;
        private string m_apiSecret;
        private byte[] m_secretKey;
        private HttpClient m_invokeClient;
        private string m_listenKey;

        System.Timers.Timer m_udsKeepAliveTimer;    // Binance must receive "keep-alive" or user data stream will close after 60 minutes

        private Uri m_baseUri;

        private long m_msTimeAdjust = 0;                    // time adjustment in milliseconds

        private HttpClient m_client = new HttpClient();

        private IDictionary<string, BinanceAccountBalance> m_balances = new SortedDictionary<string, BinanceAccountBalance>();

        public BinanceApi(string apiKey, string apiSecret)
        {
            m_apiKey = apiKey;
            m_apiSecret = apiSecret;
            m_secretKey = Encoding.ASCII.GetBytes(apiSecret);
            m_hmac = new HMACSHA256(m_secretKey);

            m_baseUri = new Uri(URL);
            m_client.BaseAddress = m_baseUri;

            m_listenKey = null;

            InitializeInvokeHttpClient();

            // Check the Binance server time and create a "time adjustment" relative to this machine's local time
            var dt = GetServerTime();
            var dtNow = DateTime.Now;
            Console.WriteLine("server time: {0}", dt.ToLongTimeString());
            Console.WriteLine(" local time: {0}", dtNow.ToLongTimeString());
            m_msTimeAdjust = dt.Subtract(dtNow).Milliseconds;
            Console.WriteLine("time adjust: {0} ms\n", m_msTimeAdjust);

            var ai = GetAccountInfo();
            m_balances = ai.GetNonZeroBalances();
        }

        public void Test()
        {
            foreach (var k in m_balances.Keys)
            {
                Console.WriteLine(m_balances[k]);
            }

            var o = Buy("LTCBTC", 1.0M, 0.01M);
            //Sell("LTCBTC", 1.0M, 0.2M);

            //StartUserDataStream();

            var orders = GetOpenOrders("LTCBTC");

            CancelOrder("LTCBTC", o.orderId);

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
            var _userAgent = "tradetronic";
            m_invokeClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);
            // You get the following exception when trying to set the "Content-Type" header like this:
            // cl.DefaultRequestHeaders.Add("Content-Type", _contentType);
            // "Misused header name. Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects."
        }

        #region ---------- ORDERS -------------------------------------------------------------------------------------
        // BUY LIMIT order
        public BinanceOrder Buy(string symbol, decimal qty, decimal price)
        {
            return SubmitOrder(symbol.ToUpper(), "BUY", qty, price);
        }

        // SELL LIMIT order
        public BinanceOrder Sell(string symbol, decimal qty, decimal price)
        {
            return SubmitOrder(symbol.ToUpper(), "SELL", qty, price);
        }

        // LIFT OFFER (BUY limit order)
        public BinanceOrder BuyAsk(string symbol, decimal qty)
        {
            var ticker = GetOrderBookTicker(symbol);
            return SubmitOrder(symbol.ToUpper(), "BUY", qty, ticker.askPrice);
        }

        // HIT BID (SELL limit order)
        public BinanceOrder SellBid(string symbol, decimal qty)
        {
            var ticker = GetOrderBookTicker(symbol);
            return SubmitOrder(symbol.ToUpper(), "SELL", qty, ticker.bidPrice);
        }

        // JOIN BID (BUY limit order)
        public BinanceOrder BuyBid(string symbol, decimal qty)
        {
            var ticker = GetOrderBookTicker(symbol);
            return SubmitOrder(symbol.ToUpper(), "BUY", qty, ticker.bidPrice);
        }

        // JOIN OFFER (SELL limit order)
        public BinanceOrder SellAsk(string symbol, decimal qty)
        {
            var ticker = GetOrderBookTicker(symbol);
            return SubmitOrder(symbol.ToUpper(), "SELL", qty, ticker.askPrice);
        }

        // BUY MID-MARKET (BUY limit order)
        public BinanceOrder BuyMid(string symbol, decimal qty)
        {
            var ticker = GetOrderBookTicker(symbol);
            decimal price = ticker.MidPrice;     // TODO: ensure this price falls on a valid boundary
            return SubmitOrder(symbol.ToUpper(), "BUY", qty, price);
        }

        // SELL MID-MARKET (SELL limit order)
        public BinanceOrder SellMid(string symbol, decimal qty)
        {
            var ticker = GetOrderBookTicker(symbol);
            decimal price = ticker.MidPrice;     // TODO: ensure this price falls on a valid boundary
            return SubmitOrder(symbol.ToUpper(), "SELL", qty, price);
        }

        // Submit limit order (where side is "BUY" or "SELL")
        public BinanceOrder SubmitOrder(string symbol, string side, decimal qty, decimal price)
        {
            //string q = "symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.01&recvWindow=5000";
            string sNewOrder = GetNewOrderString(symbol, side, qty, price);
            var o = PostEncrypt<BinanceOrder>("/api/v3/order", sNewOrder);

            if (!o.Result.IsNull)
            {
                // order is valid
            }

            return o.Result;
        }

        // Cancel an active order
        public void CancelOrder(string symbol, long orderId)
        {
            string queryString = string.Format("symbol={0}&orderId={1}&recvWindow=5000", symbol, orderId);
            queryString = GetSignature(queryString);
            string requestUri = "/api/v3/order?" + queryString;
            string resultContent = m_invokeClient.Invoke("DELETE", requestUri, "");
            Console.WriteLine(resultContent);
        }

        // where symbol like "LTCBTC"
        public BinanceOrderList GetOpenOrders(string symbol)
        {
            string queryString = string.Format("symbol={0}&recvWindow=5000", symbol);
            var oo = GetEncrypt<BinanceOrderList>("/api/v3/openOrders", queryString);

            if (!oo.IsNull)
            {
                // open orders is valid
            }
            
            return oo;
        }

        // where symbol like "LTCBTC" and side like "BUY"/"SELL" and quantity like "1.0" and price like "0.01"
        private string GetNewOrderString(string symbol, string side, decimal qty, decimal price)
        {
            return string.Format("symbol={0}&side={1}&type=LIMIT&timeInForce=GTC&quantity={2}&price={3}&recvWindow=5000", symbol, side, qty, price);
        }

        /*// where symbol like "LTCBTC" and quantity like "1.0" and price like "0.01"
        private string GetBuyString(string symbol, decimal qty, decimal price)
        {
            return string.Format("symbol={0}&side=BUY&type=LIMIT&timeInForce=GTC&quantity={1}&price={2}&recvWindow=5000", symbol, qty, price);
        }

        // where symbol like "LTCBTC" and quantity like "1.0" and price like "0.01"
        private string GetSellString(string symbol, decimal qty, decimal price)
        {
            return string.Format("symbol={0}&side=SELL&type=LIMIT&timeInForce=GTC&quantity={1}&price={2}&recvWindow=5000", symbol, qty, price);
        }*/
        #endregion ----------------------------------------------------------------------------------------------------

        // Refresh the member variable containing account balances
        public void UpdateAccountBalances()
        {
            var ai = GetAccountInfo();
            m_balances = ai.GetNonZeroBalances();
        }

        // Get account balances
        public BinanceAccountInfo GetAccountInfo()
        {
            string queryString = "recvWindow=5000";
            var ai = GetEncrypt<BinanceAccountInfo>("/api/v3/account", queryString);

            if (!ai.IsNull)
            {
                // account info is valid
            }

            return ai;
        }

        // Get BinanceOrderBookTicker for ONE symbol
        // where symbol like "LTCBTC"
        public BinanceOrderBookTicker GetOrderBookTicker(string symbol)
        {
            var ticker = m_invokeClient.Get<BinanceOrderBookTicker>(string.Format("/api/v3/ticker/bookTicker?symbol={0}", symbol.ToUpper()));

            if (!ticker.IsNull)
            {
                // ticker is valid
            }

            return ticker;
        }

        // Get BinanceOrderBookTicker for ALL symbols
        public IDictionary<string, BinanceOrderBookTicker> GetOrderBookTickers()
        {
            var result = new SortedDictionary<string, BinanceOrderBookTicker>();
            var tickers = m_invokeClient.Get<BinanceOrderBookTickerList>("/api/v3/ticker/bookTicker");

            if (!tickers.IsNull)
            {
                // tickers are valid
                foreach (var t in tickers)
                {
                    result[t.symbol] = t;
                }
            }

            return result;
        }

        // Return the Binance server time (as LOCAL time)
        public DateTime GetServerTime()
        {
            var btime = m_invokeClient. Get<BinanceServerTime>("/api/v1/time");
            long x = btime.serverTime;
            return FromTimestampMilliseconds(x);
        }

        /*// Given a quantity in BTC, return a corresponding quantity in the given
        // where 
        public decimal FromBtcQuantity(string asset, decimal btcQty)
        {

        }*/

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

        /*// GET (no encryption)
        // where method like "/api/v3/ticker/bookTicker"
        // where T like BinanceBookTicker
        public T Get<T>(string method) where T : NullableObject
        {
            //Console.WriteLine(method);
            string resultContent = m_invokeClient.Invoke("GET", method, "");
            //Console.WriteLine(resultContent);

            var obj = JsonConvert.DeserializeObject<T>(resultContent);

            if (obj.IsNull)     // an error occurred during GET
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;
        }*/

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


        #region ---------- USER DATA STREAM ---------------------------------------------------------------------------
        public void StartUserDataStream()
        {
            var uds = Post<BinanceStartUserDataStream>("/api/v1/userDataStream");

            if (!uds.IsNull)    // user data stream is valid
            {
                m_listenKey = uds.listenKey;
                LaunchUserDataStreamThread(m_listenKey);

                // TODO: Create 15-minute timer to send Keep-Alive for user data stream
                m_udsKeepAliveTimer = new System.Timers.Timer(900000);
                m_udsKeepAliveTimer.Elapsed += M_udsKeepAliveTimer_Elapsed;
                m_udsKeepAliveTimer.AutoReset = true;
                m_udsKeepAliveTimer.Start();
            }
            else
            {
                m_listenKey = null;
            }
        }

        private void M_udsKeepAliveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            KeepAliveUserDataStream();
        }

        // Send Ping/Keep-Alive to user data stream (stream will close after 60-minutes if no ping/keep-alive is received)
        public void KeepAliveUserDataStream()
        {
            // PUT /api/v1/userDataStream?listenKey=pqia91ma19a5s61cv6a81va65sdf19v8a65a1a5s61cv6a81va65sdf19v8a65a1
            string queryString = string.Format("listenKey={0}", m_listenKey);
            var res = Put<BinanceEmpty>("/api/v1/userDataStream?" + queryString);
        }

        // Close out a user data stream
        public void StopUserDataStream()
        {
            // DELETE /api/v1/userDataStream?listenKey=pqia91ma19a5s61cv6a81va65sdf19v8a65a1a5s61cv6a81va65sdf19v8a65a1
            // TODO: Delete the 15-minute timer Keep-Alive timer for this user data stream            
            m_listenKey = null;
        }

        public static void LaunchUserDataStreamThread(string listenKey)
        {
            const int BYTE_SIZE = 32768;

            // Create a string like the following (for use when connecting to websocket):
            // "/ws/<listenKey>"
            // (websocket endpoint is "wss://stream.binance.com:9443")

            var socket = new ClientWebSocket();
            Task task = socket.ConnectAsync(new Uri("wss://stream.binance.com:9443/ws/" + listenKey), CancellationToken.None);
            task.Wait();

            Thread readThread = new Thread(
                delegate (object obj)
                {
                    byte[] recBytes = new byte[BYTE_SIZE];
                    while (true)
                    {
                        ArraySegment<byte> t = new ArraySegment<byte>(recBytes);
                        Task<WebSocketReceiveResult> receiveAsync = socket.ReceiveAsync(t, CancellationToken.None);
                        receiveAsync.Wait();
                        string jsonString = Encoding.UTF8.GetString(recBytes);
                        //Console.WriteLine("jsonString = {0}", jsonString);

                        JObject jo = JsonConvert.DeserializeObject<JObject>(jsonString);
                        //string stream = jo["stream"].Value<string>();
                        //jo = jo["data"].Value<JObject>();
                        string type = jo["e"].Value<string>();
                        //Console.WriteLine("stream: {0}   eventType: {1}", stream, type);

                        if (type == "outboundAccountInfo") // "outboundAccountInfo" : Account state is updated
                        {
                            //Console.WriteLine("outboundAccountInfo:");
                            var oai = JsonConvert.DeserializeObject<BinanceOutboundAccountInfo>(jsonString);
                            //Console.WriteLine("{0}", oai);
                        }
                        else if (type == "executionReport")     // "executionReport" : Orders are updated
                        {
                            //Console.WriteLine("executionReport:");
                            var er = JsonConvert.DeserializeObject<BinanceExecutionReport>(jsonString);
                            //Console.WriteLine("{0}", er);
                        }
                        else
                        {
                            Console.WriteLine("unknown event type: '{0}'", type);
                        }

                        Array.Clear(recBytes, 0, BYTE_SIZE);
                    }
                });
            readThread.Start();

            //Console.ReadLine();
        }
        #endregion ----------------------------------------------------------------------------------------------------


        /*#region ---------- ICryptoApi ---------------------------------------------------------------
        public static BinanceApi Create(string encryptedCredentialsFile, string password)
        {
            var credentials = CryptoTools.Cryptography.Credentials.LoadFromFile(encryptedCredentialsFile, password);
            var cred = credentials["BINANCE"];
            return new BinanceApi(cred.Key, cred.Secret);
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
            m_client.
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

    } // end of class BinanceApi
} // end of namespace



// GET https://api.binance.com/api/v1/exchangeInfo              // exchange information for each symbol
// ---- HMAC SHA256 ----
// GET https://api.binance.com/api/v3/account                   // account information
// POST https://api.binance.com/api/v1/userDataStream           // start user data stream (stream will close after 60 minutes unless a keep-alive is sent)
// PUT https://api.binance.com/api/v1/userDataStream            // keep-alive (send every 30 minutes to prevent timeout
// POST https://api.binance.com/api/v3/order/test               // test new order
// POST https://api.binance.com/api/v3/order                    // new order
// GET https://api.binance.com/api/v3/order                     // query order
// GET https://api.binance.com/api/v3/openOrders                // current open orders (be careful when accessing with no symbol)
// GET https://api.binance.com/api/v3/allOrders                 // all orders
// DELETE https://api.binance.com/api/v3/order                  // cancel order
