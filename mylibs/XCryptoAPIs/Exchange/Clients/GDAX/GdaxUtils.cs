using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoAPIs.Exchange.Clients.GDAX
{
    #region --------------------------- ENUMS ----------------------------------------------------------------------
    public enum FundingStatus
    {
        Outstanding,
        Settled,
        Rejected
    }

    public enum GoodTillTime
    {
        Min,
        Hour,
        Day
    }

    public enum OrderSide
    {
        Buy,
        Sell
    }

    public enum OrderType
    {
        Limit,
        Market,
        Stop
    }

    public enum TimeInForce
    {
        Gtc,
        Ioc,
        Fok
    }

    public enum ProductLevel
    {
        One = 1,
        Two = 2,
        Three = 3
    }

    public enum GdaxCurrency
    {
        USD,
        EUR,
        GBP,
        BTC,
        LTC,
        ETH,
        BCH
    }

    public enum ProductType
    {
        BtcUsd,
        BtcEur,
        BtcGbp,
        EthUsd,
        EthEur,
        EthBtc,
        LtcUsd,
        LtcEur,
        LtcBtc,
        BchUsd,
        BchEur,
        BchBtc
    }
    #endregion -----------------------------------------------------------------------------------------------------



    public interface IHttpClient
    {
        Task<HttpResponseMessage> SendASync(HttpRequestMessage httpRequestMessage);

        Task<string> ReadAsStringAsync(HttpResponseMessage httpRequestMessage);
    }

    public class HttpClient : IHttpClient
    {
        public async Task<HttpResponseMessage> SendASync(HttpRequestMessage httpRequestMessage)
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var result = await httpClient.SendAsync(httpRequestMessage);
                return result;
            }
        }

        public async Task<string> ReadAsStringAsync(HttpResponseMessage httpRequestMessage)
        {
            var result = await httpRequestMessage.Content.ReadAsStringAsync();
            return result;
        }
    }

    public interface IHttpRequestMessageService
    {
        HttpRequestMessage CreateHttpRequestMessage(
            HttpMethod httpMethod,
            IAuthenticator authenticator,
            string requestUri,
            string contentBody = "");
    }

    public class HttpRequestMessageService : IHttpRequestMessageService
    {
        private const string apiUri = "https://api.gdax.com";

        private const string sandBoxApiUri = "https://api-public.sandbox.gdax.com";

        private readonly IClock clock;

        private readonly bool sandBox;

        public HttpRequestMessageService(IClock clock, bool sandBox)
        {
            this.clock = clock;
            this.sandBox = sandBox;
        }

        public HttpRequestMessage CreateHttpRequestMessage(
            HttpMethod httpMethod, 
            IAuthenticator authenticator, 
            string requestUri, 
            string contentBody = "")
        {
            var baseUri = sandBox
                ? sandBoxApiUri
                : apiUri;

            var requestMessage = new HttpRequestMessage(httpMethod, new Uri(new Uri(baseUri), requestUri))
            {
                Content = contentBody == string.Empty
                    ? null
                    : new StringContent(contentBody, Encoding.UTF8, "application/json")
            };

            var timeStamp = clock.GetTime().ToTimeStamp();
            var signedSignature = ComputeSignature(httpMethod, authenticator.UnsignedSignature, timeStamp, requestUri, contentBody);

            AddHeaders(requestMessage, authenticator, signedSignature, timeStamp);
            return requestMessage;
        }

        private string ComputeSignature(HttpMethod httpMethod, string secret, double timestamp, string requestUri, string contentBody = "")
        {
            var convertedString = Convert.FromBase64String(secret);
            var prehash = timestamp.ToString("F0", CultureInfo.InvariantCulture) + httpMethod.ToString().ToUpper() + requestUri + contentBody;
            return HashString(prehash, convertedString);
        }

        private string HashString(string str, byte[] secret)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var hmaccsha = new HMACSHA256(secret))
            {
                return Convert.ToBase64String(hmaccsha.ComputeHash(bytes));
            }
        }

        private void AddHeaders(
            HttpRequestMessage httpRequestMessage,
            IAuthenticator authenticator,
            string signedSignature,
            double timeStamp)
        {
            httpRequestMessage.Headers.Add("User-Agent", "GDAXClient");
            httpRequestMessage.Headers.Add("CB-ACCESS-KEY", authenticator.ApiKey);
            httpRequestMessage.Headers.Add("CB-ACCESS-TIMESTAMP", timeStamp.ToString("F0", CultureInfo.InvariantCulture));
            httpRequestMessage.Headers.Add("CB-ACCESS-SIGN", signedSignature);
            httpRequestMessage.Headers.Add("CB-ACCESS-PASSPHRASE", authenticator.Passphrase);
        }
    } // end of class HttpRequestMessageService

    public interface IClock
    {
        DateTime GetTime();
    }

    public class Clock : IClock
    {
        public DateTime GetTime()
        {
            return DateTime.UtcNow;
        }
    }

    public interface IQueryBuilder
    {
        string BuildQuery(params KeyValuePair<string, string>[] queryParameters);
    }

    public class QueryBuilder : IQueryBuilder
    {
        public string BuildQuery(params KeyValuePair<string, string>[] queryParameters)
        {
            var queryString = new StringBuilder("?");

            foreach (var queryParameter in queryParameters)
            {
                if (queryParameter.Value != string.Empty)
                {
                    queryString.Append(queryParameter.Key.ToLower() + "=" + queryParameter.Value + "&");
                }
            }

            return queryString.ToString().TrimEnd('&');
        }
    }

    public interface IAuthenticator
    {
        string ApiKey { get; }

        string UnsignedSignature { get; }

        string Passphrase { get; }
    }

    public class Authenticator : IAuthenticator
    {
        public Authenticator(
            string apiKey,
            string unsignedSignature,
            string passphrase)
        {
            ApiKey = apiKey;
            UnsignedSignature = unsignedSignature;
            Passphrase = passphrase;
        }

        public string ApiKey { get; }

        public string UnsignedSignature { get; }

        public string Passphrase { get; }
    } // end of class Authenticator



    public static class DateExtensions
    {
        public static double ToTimeStamp(this DateTime date)
        {
            return (date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }

    public static class ProductTypeExtensions
    {
        public static string ToDasherizedUpper(this ProductType orderType)
        {
            var orderTypeString = orderType.ToString();

            return orderTypeString.Insert(3, "-").ToUpper();
        }
    }


} // end of namespace
