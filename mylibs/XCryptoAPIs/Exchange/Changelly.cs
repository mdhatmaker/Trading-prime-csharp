using System;
using System.Net;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tools;
using static Tools.G;

namespace CryptoAPIs.Exchange
{
    // https://changelly.com/developers
    // https://api-docs.changelly.com

    public class Changelly : BaseExchange
    {
        public override string BaseUrl { get { return "https://api.changelly.com"; } }
        public override string ExchangeName { get { return "CHANGELLY"; } }
        public override CryptoExch Exch => CryptoExch.CHANGELLY;

        // SINGLETON
        public static Changelly Instance { get { return m_instance; } }
        private static readonly Changelly m_instance = new Changelly();
        private Changelly() { }

        private static readonly WebClient Client = new WebClient();
        private static readonly Encoding U8 = Encoding.UTF8;

        class ChangellyResponse<T>
        {
            public string jsonrpc { get; set; }     // "2.0"
            public int id { get; set; }             // 1
            public T result { get; set; }
        }

        public override List<string> SymbolList
        {
            get
            {
                return GetSymbolList(false);
            }
        }

        public override List<string> GetSymbolList(bool forceUpdate)
        {
            if (m_symbolList == null || forceUpdate)
            {
                var set = new SortedSet<string>();

                var currencies = JsonRpc<List<string>>("getCurrencies");
                for (int i = 0; i < currencies.Count-1; ++i)
                {
                    for (int j = i+1; j < currencies.Count; ++j)
                    {
                        set.Add(currencies[i] + "_" + currencies[j]);
                        set.Add(currencies[j] + "_" + currencies[i]);
                    }
                }

                m_symbolList = set.ToList();
                m_symbolList.Sort();
            }
            return m_symbolList;
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

        public List<string> GetCurrencies()
        {
            var currencySymbols = JsonRpc<List<string>>("getCurrencies");
            return currencySymbols;
        }

        public List<ChangellyCurrencyFull> GetCurrenciesFull()
        {
            var currencies = JsonRpc<List<ChangellyCurrencyFull>>("getCurrenciesFull");
            return currencies;
        }

        // Given two currencies (from/to)
        // Return the Minimum Exchangeable Amount -- the minimum amount that can be exchanged but is larger than the exchange fee
        // Where fromCurrency/toCurrency like "ltc", "btc", "eth", ...
        public decimal GetMinAmount(string fromCurrency, string toCurrency)
        {
            //string str = string.Format("{\"from\": \"{0}\", \"to\": \"{1}\"}", fromCurrency, toCurrency);
            string str = "{\"from\": \"" + fromCurrency + "\", \"to\": \"" + toCurrency + "\"}";
            var minAmount = JsonRpc<decimal>("getMinAmount", str);
            return minAmount;
        }

        // Given two currencies (from/to) and the amount the user is going to exchange
        // Return the Estimated Exchange Amount -- the estimated amount of coins the user will receive as a result of the exchange
        // NOTE: Extra API fee is included in this amount.
        public decimal GetExchangeAmount(string fromCurrency, string toCurrency, decimal amountToExchange)
        {
            string str = "{\"from\": \"" + fromCurrency + "\", \"to\": \"" + toCurrency + "\", \"amount\": \"" + amountToExchange.ToString() + "\"}";
            var exchangeAmount = JsonRpc<decimal>("getExchangeAmount", str);
            return exchangeAmount;
        }

        private T JsonRpc<T>(string methodName, string methodParams = "[]")
        {
            string message = @"{
		        ""jsonrpc"": ""2.0"",
		        ""id"": 1,
		        ""method"": """ + methodName + @""",
		        ""params"": " + methodParams + @"
			}";

            HMACSHA512 hmac = new HMACSHA512(U8.GetBytes(ApiSecret));
            byte[] hashmessage = hmac.ComputeHash(U8.GetBytes(message));
            string sign = ToHexString(hashmessage);

            Client.Headers.Set("Content-Type", "application/json");
            Client.Headers.Add("api-key", ApiKey);
            Client.Headers.Add("sign", sign);

            string response = Client.UploadString(BaseUrl, message);
            dout(response);

            var obj = JsonConvert.DeserializeObject<ChangellyResponse<T>>(response);
            return obj.result;
        }

        public void Test()
        {
            var symbols = Changelly.Instance.SymbolList;

            decimal minAmount = Changelly.Instance.GetMinAmount("ltc", "eth");
            decimal exchangeAmount = Changelly.Instance.GetExchangeAmount("ltc", "eth", minAmount * 2);

            string fromCurrency = "ltc";
            string toCurrency = "eth";
            decimal amount = 3.99M;
            decimal minAmt = Changelly.Instance.GetMinAmount(fromCurrency, toCurrency);
            decimal exchangeAmt = Changelly.Instance.GetExchangeAmount(fromCurrency, toCurrency, amount);
            dout("{0}->{1} min:{2} {3}->{4}", fromCurrency, toCurrency, minAmt, amount, exchangeAmt);
        }

    } // end of class Changelly

    public class ChangellyCurrencyFull
    {
        public string name { get; set; }            // "btc"
        public string fullName { get; set; }        // "Bitcoin"
        public bool enabled { get; set; }           // true
    }


} // end of namespace
