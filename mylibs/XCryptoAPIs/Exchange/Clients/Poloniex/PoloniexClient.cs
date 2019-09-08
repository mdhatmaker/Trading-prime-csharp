using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Tools.Logging;
using Tools;

namespace CryptoAPIs.Exchange.Clients.Poloniex
{
    /// <summary>
    /// https://poloniex.com/
    /// </summary>
    public class PoloniexClient
    {
        private string __connect_key;
        private string __secret_key;
        private string __end_point;
        private string __end_pointT;
        private string __end_pointU;

        /// <summary>
        /// 
        /// </summary>
        public PoloniexClient(string apikey, string apisecret, string end_point = "public", string tradingEndPoint = "tradingApi", string userEndPoint = "tradingApi")
        {
            __connect_key = apikey;
            __secret_key = apisecret;
            __end_point = end_point;
            __end_pointT = tradingEndPoint;
            __end_pointU = userEndPoint;
        }

        private PoloniexApi __api = null;

        private PoloniexApi Api
        {
            get
            {
                if (__api == null)
                    __api = new PoloniexApi(__connect_key, __secret_key);
                return __api;
            }
        }


        /// <summary>
        /// poloniex 거래소 마지막 거래 정보
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, PublicTicker>> GetTicker()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTicker");
            }

            return await Api.CallApiGetAsync<Dictionary<string, PublicTicker>>(__end_point, _params);
        }

        /// <summary>
        /// poloniex 거래소 판/구매 등록 대기 또는 거래 중 내역 정보
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public async Task<PublicOrderBook> GetOrderBook(CurrencyPair currency_pair, uint depth)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnOrderBook");
                _params.Add("currencyPair", currency_pair);
                _params.Add("depth", depth);
            }

            return await Api.CallApiGetAsync<PublicOrderBook>(__end_point, _params);
        }

        /// <summary>
        /// poloniex 거래소 거래 체결 완료 내역
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <returns></returns>
        public async Task<List<PublicTrade>> GetTradeHistory(CurrencyPair currency_pair)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTradeHistory");
                _params.Add("currencyPair", currency_pair);
            }

            return await Api.CallApiGetAsync<List<PublicTrade>>(__end_point, _params);
        }

        /// <summary>
        /// poloniex 거래소 거래 체결 완료 내역
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<List<PublicTrade>> GetTradeHistory(CurrencyPair currency_pair, DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTradeHistory");
                _params.Add("currencyPair", currency_pair);
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
            }

            return await Api.CallApiGetAsync<List<PublicTrade>>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="period"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<List<PublicChart>> GetChartData(CurrencyPair currency_pair, ChartPeriod period, DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnChartData");
                _params.Add("currencyPair", currency_pair);
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
                _params.Add("period", (int)period);
            }

            return await Api.CallApiGetAsync<List<PublicChart>>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <returns></returns>
        public async Task<List<TradeOrder>> OpenOrders(CurrencyPair currency_pair)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnOpenOrders");
                _params.Add("currencyPair", currency_pair);
            }

            return await Api.CallApiPostAsync<List<TradeOrder>>(__end_pointT, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<List<TradeOrder>> GetTrades(CurrencyPair currency_pair, DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTradeHistory");
                _params.Add("currencyPair", currency_pair);
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
            }

            return await Api.CallApiPostAsync<List<TradeOrder>>(__end_pointT, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currencyPair"></param>
        /// <param name="type"></param>
        /// <param name="pricePerCoin"></param>
        /// <param name="amountQuote"></param>
        /// <returns></returns>
        public async Task<ulong> PlaceOrder(CurrencyPair currency_pair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", type.ToStringNormalized());
                _params.Add("currencyPair", currency_pair);
                _params.Add("rate", pricePerCoin.ToStringNormalized());
                _params.Add("amount", amountQuote.ToStringNormalized());
            }

            var _data = await Api.CallApiPostAsync<JObject>(__end_pointT, _params);
            var err = _data.Value<string>("error");
            if (!string.IsNullOrEmpty(err))
            {
                Console.WriteLine("\n***PoloniexClient::PlaceOrder=> {0}\n", err);
                return default(ulong); 
            }
            else
                return _data.Value<ulong>("orderNumber");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteOrder(CurrencyPair currency_pair, ulong order_id)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "cancelOrder");
                _params.Add("currencyPair", currency_pair);
                _params.Add("orderNumber", order_id);
            }

            var _data = await Api.CallApiPostAsync<JObject>(__end_pointT, _params);
            var err = _data.Value<string>("error");
            if (!string.IsNullOrEmpty(err))
            {
                Console.WriteLine("\n***PoloniexClient::DeleteOrder=> {0}\n", err);
                return false;
            }
            else
                return _data.Value<byte>("success") == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, UserBalance>> GetBalances()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnCompleteBalances");
            }

            return await Api.CallApiPostAsync<Dictionary<string, UserBalance>>(__end_pointU, _params);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetDepositAddresses()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnDepositAddresses");
            }

            return await Api.CallApiPostAsync<Dictionary<string, string>>(__end_pointU, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<IDepositWithdrawal> GetDepositsAndWithdrawals(DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnDepositsWithdrawals");
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
            }

            return await Api.CallApiPostAsync<DepositWithdrawal>(__end_pointU, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name"></param>
        /// <returns></returns>
        public async Task<UserDepositAddress> GenerateNewDepositAddress(string currency_name)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "generateNewAddress");
                _params.Add("currency", currency_name);
            }

            return await Api.CallApiPostAsync<UserDepositAddress>(__end_pointU, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name"></param>
        /// <param name="amount"></param>
        /// <param name="address"></param>
        /// <param name="payment_id"></param>
        /// <returns></returns>
        public async Task<UserDepositAddress> Withdrawal(string currency_name, decimal amount, string address, string payment_id = null)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "withdraw");
                _params.Add("currency", currency_name);
                _params.Add("amount", amount);
                _params.Add("address", address);

                if (payment_id != null)
                    _params.Add("paymentId", payment_id);
            }

            return await Api.CallApiPostAsync<UserDepositAddress>(__end_pointU, _params);
        }
    } // end of class PoloniexClient







    /// <summary>
    /// 
    /// </summary>
    public class PoloniexApi : XApiClient
    {
        private const string __api_url = "https://poloniex.com";

        /// <summary>
        /// 
        /// </summary>
        public PoloniexApi()
            : base(__api_url, "", "")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connect_key"></param>
        /// <param name="secret_key"></param>
        public PoloniexApi(string connect_key, string secret_key)
            : base(__api_url, connect_key, secret_key)
        {
        }


        private HMACSHA512 __encryptor = null;
        public HMACSHA512 Encryptor
        {
            get
            {
                if (__encryptor == null)
                    __encryptor = new HMACSHA512(Encoding.UTF8.GetBytes(__secret_key));

                return __encryptor;
            }
        }

        private BigInteger CurrentHttpPostNonce
        {
            get;
            set;
        }

        private string GetCurrentHttpPostNonce()
        {
            var _ne_nonce = new BigInteger(
                                    Math.Round(
                                        DateTime.UtcNow.Subtract(
                                            //CUnixTime.DateTimeUnixEpochStart
                                            GDate.DateTimeUnixEpochStart
                                        )
                                        .TotalMilliseconds * 1000,
                                        MidpointRounding.AwayFromZero
                                    )
                                );

            if (_ne_nonce > CurrentHttpPostNonce)
            {
                CurrentHttpPostNonce = _ne_nonce;
            }
            else
            {
                CurrentHttpPostNonce += 1;
            }

            return CurrentHttpPostNonce.ToString(CultureInfo.InvariantCulture);
        }

        private string HttpPostString(List<Parameter> dictionary)
        {
            var _result = "";

            foreach (var _entry in dictionary)
            {
                var _value = _entry.Value as string;
                if (_value == null)
                    _result += "&" + _entry.Name + "=" + _entry.Value;
                else
                    _result += "&" + _entry.Name + "=" + _value.Replace(' ', '+');
            }

            return _result.Substring(1);
        }

        private string ConvertHexString(byte[] value)
        {
            var _result = "";

            for (var i = 0; i < value.Length; i++)
                _result += value[i].ToString("x2", CultureInfo.InvariantCulture);

            return _result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public new async Task<T> CallApiPostAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _request = CreateJsonRequest(endpoint, Method.POST);
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("nonce", GetCurrentHttpPostNonce());

                    if (args != null)
                    {
                        foreach (var a in args)
                            _params.Add(a.Key, a.Value);
                    }
                }

                foreach (var _p in _params)
                    _request.AddParameter(_p.Key, _p.Value);

                var _post_data = HttpPostString(_request.Parameters);
                var _post_bytes = Encoding.UTF8.GetBytes(_post_data);
                var _post_hash = Encryptor.ComputeHash(_post_bytes);

                var _signature = ConvertHexString(_post_hash);
                {
                    _request.AddHeader("Key", __connect_key);
                    _request.AddHeader("Sign", _signature);
                }
            }

            var _client = CreateJsonClient(__api_url);
            {
                var _tcs = new TaskCompletionSource<IRestResponse>();
                var _handle = _client.ExecuteAsync(_request, response =>
                {
                    _tcs.SetResult(response);
                });

                var _response = await _tcs.Task;
                return JsonConvert.DeserializeObject<T>(_response.Content);
            }
        }
    } // end of class PoloniexApi


    /// <summary>
    /// 
    /// </summary>
    public class XApiClient : IDisposable
    {
        //protected static CLogger __clogger = new CLogger();
        protected static ILogFactory __logFactory = new NLogFactory();
        protected static ILog _log = __logFactory.CreateLog(nameof(PoloniexApi));
        private string __api_url = "";

        protected string __connect_key;
        protected string __secret_key;

        protected const string __content_type = "application/json";
        protected const string __user_agent = "btc-trading/5.2.2017.01";

        /// <summary>
        /// 
        /// </summary>
        public XApiClient(string api_url, string connect_key, string secret_key)
        {
            __api_url = api_url;
            __connect_key = connect_key;
            __secret_key = secret_key;
        }

        private static char[] __to_digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        public byte[] EncodeHex(byte[] data)
        {
            int l = data.Length;
            byte[] _result = new byte[l << 1];

            // two characters form the hex value.
            for (int i = 0, j = 0; i < l; i++)
            {
                _result[j++] = (byte)__to_digits[(0xF0 & data[i]) >> 4];
                _result[j++] = (byte)__to_digits[0x0F & data[i]];
            }

            return _result;
        }

        public string EncodeURIComponent(Dictionary<string, object> rgData)
        {
            string _result = String.Join("&", rgData.Select((x) => String.Format("{0}={1}", x.Key, x.Value)));

            _result = System.Net.WebUtility.UrlEncode(_result)
                        .Replace("+", "%20").Replace("%21", "!")
                        .Replace("%27", "'").Replace("%28", "(")
                        .Replace("%29", ")").Replace("%26", "&")
                        .Replace("%3D", "=").Replace("%7E", "~");

            return _result;
        }

        public IRestClient CreateJsonClient(string baseurl)
        {
            var _client = new RestClient(baseurl);
            {
                _client.RemoveHandler(__content_type);
                _client.AddHandler(__content_type, new RestSharpJsonNetDeserializer());
                _client.Timeout = 5 * 1000;
                _client.ReadWriteTimeout = 32 * 1000;
                _client.UserAgent = __user_agent;
            }

            return _client;
        }

        public IRestRequest CreateJsonRequest(string resource, Method method = Method.GET)
        {
            var _request = new RestRequest(resource, method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            return _request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<T> CallApiPostAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _request = CreateJsonRequest(endpoint, Method.POST);
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("endpoint", endpoint);
                    if (args != null)
                    {
                        foreach (var a in args)
                            _params.Add(a.Key, a.Value);
                    }
                }

                foreach (var a in _params)
                    _request.AddParameter(a.Key, a.Value);
            }

            var _client = CreateJsonClient(__api_url);
            {
                var _tcs = new TaskCompletionSource<IRestResponse>();
                var _handle = _client.ExecuteAsync(_request, response =>
                {
                    _tcs.SetResult(response);
                });

                var _response = await _tcs.Task;
                return JsonConvert.DeserializeObject<T>(_response.Content);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<T> CallApiGetAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _request = CreateJsonRequest(endpoint, Method.GET);

            if (args != null)
            {
                foreach (var a in args)
                    _request.AddParameter(a.Key, a.Value);
            }

            var _client = CreateJsonClient(__api_url);
            {
                var _tcs = new TaskCompletionSource<IRestResponse>();
                var _handle = _client.ExecuteAsync(_request, response =>
                {
                    _tcs.SetResult(response);
                });

                var _response = await _tcs.Task;
                return JsonConvert.DeserializeObject<T>(_response.Content);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
    } // end of class XApiClient




    /*/// <summary>
    /// https://poloniex.com/
    /// </summary>
    public class PPublicApi
    {
        private string __end_point;

        /// <summary>
        /// 
        /// </summary>
        public PPublicApi(string end_point = "public")
        {
            __end_point = end_point;
        }

        private PoloniexClient __public_client = null;

        private PoloniexClient PublicClient
        {
            get
            {
                if (__public_client == null)
                    __public_client = new PoloniexClient();
                return __public_client;
            }
        }

        /// <summary>
        /// poloniex 거래소 마지막 거래 정보
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, PublicTicker>> GetTicker()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTicker");
            }

            return await PublicClient.CallApiGetAsync<Dictionary<string, PublicTicker>>(__end_point, _params);
        }

        /// <summary>
        /// poloniex 거래소 판/구매 등록 대기 또는 거래 중 내역 정보
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public async Task<PublicOrderBook> GetOrderBook(CurrencyPair currency_pair, uint depth)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnOrderBook");
                _params.Add("currencyPair", currency_pair);
                _params.Add("depth", depth);
            }

            return await PublicClient.CallApiGetAsync<PublicOrderBook>(__end_point, _params);
        }

        /// <summary>
        /// poloniex 거래소 거래 체결 완료 내역
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <returns></returns>
        public async Task<List<PublicTrade>> GetTradeHistory(CurrencyPair currency_pair)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTradeHistory");
                _params.Add("currencyPair", currency_pair);
            }

            return await PublicClient.CallApiGetAsync<List<PublicTrade>>(__end_point, _params);
        }

        /// <summary>
        /// poloniex 거래소 거래 체결 완료 내역
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<List<PublicTrade>> GetTradeHistory(CurrencyPair currency_pair, DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTradeHistory");
                _params.Add("currencyPair", currency_pair);
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
            }

            return await PublicClient.CallApiGetAsync<List<PublicTrade>>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="period"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<List<PublicChart>> GetChartData(CurrencyPair currency_pair, ChartPeriod period, DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnChartData");
                _params.Add("currencyPair", currency_pair);
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
                _params.Add("period", (int)period);
            }

            return await PublicClient.CallApiGetAsync<List<PublicChart>>(__end_point, _params);
        }
    } // end of class PPublicApi

    /// <summary>
    /// https://poloniex.com/
    /// </summary>
    public class PTradeApi
    {
        private string __connect_key;
        private string __secret_key;
        private string __end_point;

        /// <summary>
        /// 
        /// </summary>
        public PTradeApi(string connect_key, string secret_key, string end_point = "tradingApi")
        {
            __connect_key = connect_key;
            __secret_key = secret_key;

            __end_point = end_point;
        }

        private PoloniexClient __trade_client = null;

        private PoloniexClient TradeClient
        {
            get
            {
                if (__trade_client == null)
                    __trade_client = new PoloniexClient(__connect_key, __secret_key);
                return __trade_client;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <returns></returns>
        public async Task<List<TradeOrder>> OpenOrders(CurrencyPair currency_pair)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnOpenOrders");
                _params.Add("currencyPair", currency_pair);
            }

            return await TradeClient.CallApiPostAsync<List<TradeOrder>>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<List<TradeOrder>> GetTrades(CurrencyPair currency_pair, DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTradeHistory");
                _params.Add("currencyPair", currency_pair);
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
            }

            return await TradeClient.CallApiPostAsync<List<TradeOrder>>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currencyPair"></param>
        /// <param name="type"></param>
        /// <param name="pricePerCoin"></param>
        /// <param name="amountQuote"></param>
        /// <returns></returns>
        public async Task<ulong> PlaceOrder(CurrencyPair currency_pair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", type.ToStringNormalized());
                _params.Add("currencyPair", currency_pair);
                _params.Add("rate", pricePerCoin.ToStringNormalized());
                _params.Add("amount", amountQuote.ToStringNormalized());
            }

            var _data = await TradeClient.CallApiPostAsync<JObject>(__end_point, _params);
            return _data.Value<ulong>("orderNumber");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteOrder(CurrencyPair currency_pair, ulong order_id)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "cancelOrder");
                _params.Add("currencyPair", currency_pair);
                _params.Add("orderNumber", order_id);
            }

            var _data = await TradeClient.CallApiPostAsync<JObject>(__end_point, _params);
            return _data.Value<byte>("success") == 1;
        }
    } // end of class PTradeApi

    /// <summary>
    /// https://poloniex.com/
    /// </summary>
    public class PUserApi
    {
        private string __connect_key;
        private string __secret_key;
        private string __end_point;

        /// <summary>
        /// 
        /// </summary>
        public PUserApi(string connect_key, string secret_key, string end_point = "tradingApi")
        {
            __connect_key = connect_key;
            __secret_key = secret_key;

            __end_point = end_point;
        }

        private PoloniexClient __user_client = null;

        private PoloniexClient UserClient
        {
            get
            {
                if (__user_client == null)
                    __user_client = new PoloniexClient(__connect_key, __secret_key);
                return __user_client;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, UserBalance>> GetBalances()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnCompleteBalances");
            }

            return await UserClient.CallApiPostAsync<Dictionary<string, UserBalance>>(__end_point, _params);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetDepositAddresses()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnDepositAddresses");
            }

            return await UserClient.CallApiPostAsync<Dictionary<string, string>>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<IDepositWithdrawal> GetDepositsAndWithdrawals(DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnDepositsWithdrawals");
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
            }

            return await UserClient.CallApiPostAsync<DepositWithdrawal>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name"></param>
        /// <returns></returns>
        public async Task<UserDepositAddress> GenerateNewDepositAddress(string currency_name)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "generateNewAddress");
                _params.Add("currency", currency_name);
            }

            return await UserClient.CallApiPostAsync<UserDepositAddress>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name"></param>
        /// <param name="amount"></param>
        /// <param name="address"></param>
        /// <param name="payment_id"></param>
        /// <returns></returns>
        public async Task<UserDepositAddress> Withdrawal(string currency_name, decimal amount, string address, string payment_id = null)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "withdraw");
                _params.Add("currency", currency_name);
                _params.Add("amount", amount);
                _params.Add("address", address);

                if (payment_id != null)
                    _params.Add("paymentId", payment_id);
            }

            return await UserClient.CallApiPostAsync<UserDepositAddress>(__end_point, _params);
        }
    } // end of class PUserApi*/


} // end of namespace
