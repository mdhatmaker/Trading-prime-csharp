using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Tools;

namespace CryptoAPIs.ExchangeX
{
    // https://developers.coinbase.com/docs/wallet/coinbase-connect/integrating
    // https://github.com/iYalovoy/demibyte.coinbase
    // https://developers.coinbase.com/api/v2

    public class Coinbase
    {
        private string ApiVersion = "2017-11-14";
        private string ApiKey;
        private string ApiSecret;

        private string m_host = "https://api.coinbase.com/";

        public Coinbase(string apikey, string apisecret)
        {
            ApiKey = apikey;
            ApiSecret = apisecret;
        }

        class SignatureHeaders
        {
            public string Signature { get; private set; }
            public Int32 Timestamp { get; private set; }

            public SignatureHeaders(string signature, Int32 timestamp)
            {
                Signature = signature;
                Timestamp = timestamp;
            }
        } // end of class SignatureHeaders

        public void GetTime()
        {
            string ep = string.Format("time");
            SignatureHeaders sig = GetSignature(ep);
            var time = m_host
                .AppendPathSegment("v2/time")
                .WithHeader("CB-ACCESS-SIGN", sig.Signature)
                .WithHeader("CB-ACCESS-TIMESTAMP", sig.Timestamp)
                .WithHeader("CB-ACCESS-KEY", ApiKey)
                .WithHeader("CB-VERSION", ApiVersion)
                .GetJsonAsync<dynamic>()
                .Result;
            Console.WriteLine(time.ToString(Formatting.None)+"\n");
        }

        public void GetSpotPrices(string currency = "USD")
        {
            string ep = string.Format("prices/spot?currency={0}", currency);
            SignatureHeaders sig = GetSignature(ep);
            var price = m_host
                .AppendPathSegment("v2/prices/spot")
                .SetQueryParam("currency", currency)
                .WithHeader("CB-ACCESS-SIGN", sig.Signature)
                .WithHeader("CB-ACCESS-TIMESTAMP", sig.Timestamp)
                .WithHeader("CB-ACCESS-KEY", ApiKey)
                .WithHeader("CB-VERSION", ApiVersion)
                .GetJsonAsync<dynamic>()
                .Result;
            Console.WriteLine(price.ToString(Formatting.None) + "\n");
        }

        public void GetAccounts()
        {
            string ep = string.Format("accounts");
            SignatureHeaders sig = GetSignature(ep);
            var response = m_host
                .AppendPathSegment("v2/accounts")
                //.SetQueryParam("currency", "BCH")
                .WithHeader("CB-ACCESS-SIGN", sig.Signature)
                .WithHeader("CB-ACCESS-TIMESTAMP", sig.Timestamp)
                .WithHeader("CB-ACCESS-KEY", ApiKey)
                .WithHeader("CB-VERSION", ApiVersion)
                .GetJsonAsync<dynamic>()
                .Result;
            string strResponse = response.ToString(Formatting.None);
            Console.WriteLine(strResponse + "\n");
            var accts = JsonConvert.DeserializeObject<AccountResponse>(strResponse);
            foreach (var a in accts.data)
            {
                Console.WriteLine("'{0}' {1,-12} {2}   id:{3}", a.name, a.balance.amount, a.balance.currency, a.id);
            }
        }

        public void WalletSend()
        {
            //ep = string.Format("accounts/c73dd250-68bb-5ddc-aa30-e2206f3743a2/transactions?type=send&to=1FAVRhRkbJbjkYfN7NyKjQEr1o4PaSMyHw&amount=0.1&currency=BCH&idem=999999");
            //ep = string.Format("accounts/c73dd250-68bb-5ddc-aa30-e2206f3743a2/transactions");
            //ep = string.Format("accounts/383a26ee-40e0-5ff5-9cc3-b25f7a945fe8/transactions");
            string ep = string.Format("accounts/383a26ee-40e0-5ff5-9cc3-b25f7a945fe8/transactions?type=send&to=0xd126e31e03d5ca00cbccf12146a2eb7694b5f51c&amount=0.1&currency=ETH&idem=9316dd16-0c05");
            SignatureHeaders sig = GetSignature(ep, "POST");
            //System.Net.Http.HttpResponseMessage response;
            dynamic response;
            response = m_host
                .AppendPathSegment("v2/accounts/383a26ee-40e0-5ff5-9cc3-b25f7a945fe8/transactions")
                /*.SetQueryParam("type", "send")
                .SetQueryParam("to", "0xd126e31e03d5ca00cbccf12146a2eb7694b5f51c")
                .SetQueryParam("amount", "0.1")
                .SetQueryParam("currency", "ETH")
                .SetQueryParam("idem", "9316dd16-0c05")*/
                .WithHeader("CB-ACCESS-SIGN", sig.Signature)
                .WithHeader("CB-ACCESS-TIMESTAMP", sig.Timestamp)
                .WithHeader("CB-ACCESS-KEY", ApiKey)
                .WithHeader("CB-VERSION", ApiVersion)
                .PostStringAsync("type=send&to=0xd126e31e03d5ca00cbccf12146a2eb7694b5f51c&amount=0.1&currency=ETH&idem=9316dd16-0c05")
                //.GetJsonAsync<dynamic>()
                .Result;
            
            string strResponse = response.ToString(Formatting.None);
            Console.WriteLine(strResponse + "\n");
            var sent = JsonConvert.DeserializeObject<SendMoneyResponse>(strResponse);

            /*foreach (var a in accts.data)
            {
                Console.WriteLine("'{0}' {1} {2}   id:{3}", a.name, a.balance.amount, a.balance.currency, a.id);
            }*/
        }


        // Where endpoint like "/prices/spot?currency=USD"
        private SignatureHeaders GetSignature(string endpoint, string method="GET")
        {
            var unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            //var currency = "USD";
            //var message = string.Format("{0}GET/v2/prices/spot?currency={1}", unixTimestamp.ToString(), currency);
            var message = string.Format("{0}{1}/v2/{2}", unixTimestamp.ToString(), method, endpoint);

            byte[] secretKey = Encoding.UTF8.GetBytes(ApiSecret);
            HMACSHA256 hmac = new HMACSHA256(secretKey);
            hmac.Initialize();
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            byte[] rawHmac = hmac.ComputeHash(bytes);
            var signature = rawHmac.ByteArrayToHexString();

            return new SignatureHeaders(signature, unixTimestamp);
        }

        //{"data":{"iso":"2018-04-13T17:46:40Z","epoch":1523641600}}
        public class CoinbaseTime
        {
            public CoinbaseTimeData data { get; set; }

            // Convenience methods
            public string Iso { get { return data.iso; } }
            public long Epoch { get { return data.epoch; } }
        }
        public class CoinbaseTimeData
        {
            public string iso { get; set; }
            public long epoch { get; set; }
        }

        //{"data":{"base":"BTC","currency":"USD","amount":"8125.00"},"warnings":[{"id":"missing_version","message":"Please supply API version (YYYY-MM-DD) as CB-VERSION header","url":"https://developers.coinbase.com/api#versioning"}]}
        public class CoinbaseSpotPrices
        {
            public CoinbaseSpotPricesData data { get; set; }
        }
        public class CoinbaseSpotPricesData
        {
            public string @base { get; set; }
            public string currency { get; set; }
            public decimal amount { get; set; }
            public List<CoinbaseWarning> warnings { get; set; }
        }
        public class CoinbaseWarning
        {
            public string id { get; set; }
            public string mesage { get; set; }
            public string url { get; set; }
        }

        public class Currency
        {
            public string code { get; set; }            // "BCH"
            public string name { get; set; }            // "Bitcoin Cash"
            public string color { get; set; }           // "#8DC451"
            public int exponent { get; set; }           // 8
            public string type { get; set; }            // "crypto"
            public string address_regex { get; set; }   // "^([13][a-km-zA-HJ-NP-Z1-9]{25,34})|^((bitcoincash:)?(q|p)[a-z0-9]{41})|^((BITCOINCASH:)?(Q|P)[A-Z0-9]{41})$"
        }

        public class Balance
        {
            public decimal amount { get; set; }         // "39.59000000"
            public string currency { get; set; }        // "BTC"
        }

        public class Account
        {
            public string id { get; set; }              // "2bbf394c-193b-5b2a-9155-3b4732659ede"
            public string name { get; set; }            // "My Wallet"
            public string type { get; set; }            // "wallet"
            public Currency currency { get; set; }      
            public Balance balance { get; set; }
            public string created_at { get; set; }      // "2015-01-31T20:49:02Z"
            public string updated_at { get; set; }      // "2015-01-31T20:49:02Z"
            public string resource { get; set; }        // "account"
            public string resource_path { get; set; }   // "/v2/accounts/2bbf394c-193b-5b2a-9155-3b4732659ede"
        }

        public class AccountResponse
        {
            public List<Account> data { get; set; }
        }

        public class Network
        {
            public string status { get; set; }
            public string hash { get; set; }
            public string name { get; set; }
        }

        public class Address
        {
            public string resource { get; set; }
            public string address { get; set; }
        }

        public class Details
        {
            public string title { get; set; }
            public string subtitle { get; set; }
        }

        public class SendMoney
        {
            public string id { get; set; }
            public string type { get; set; }
            public string status { get; set; }
            public Balance amount { get; set; }
            public Balance native_amount { get; set; }
            public string description { get; set; }
            public string created_at { get; set; }      // "2015-01-31T20:49:02Z"
            public string updated_at { get; set; }      // "2015-01-31T20:49:02Z"
            public string resource { get; set; }        // "account"
            public string resource_path { get; set; }   // "/v2/accounts/2bbf394c-193b-5b2a-9155-3b4732659ede"
            public Network network { get; set; }
            public Address to { get; set; }
            public Details details { get; set; }
        }

        public class SendMoneyResponse
        {
            public SendMoney data { get; set; }
        }

    } // end of class Coinbase
} // end of namespace
