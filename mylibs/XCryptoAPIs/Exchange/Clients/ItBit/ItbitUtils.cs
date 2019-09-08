using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Tools;

namespace CryptoAPIs.Exchange.Clients.ItBit
{
    internal static class Configuration
    {
        public static Uri ApiUrl
        {
            get
            {
                ///var itBitApiUrl = ConfigurationManager.AppSettings["itbit-rest-api-baseurl"];
                var itBitApiUrl = AppConfig.Instance["itbit-rest-api-baseurl"];
                return new Uri(itBitApiUrl, UriKind.RelativeOrAbsolute);
            }
        }
    }

    class NumberToStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = Convert.ToString(value, CultureInfo.InvariantCulture);
            writer.WriteValue(val);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(decimal).IsAssignableFrom(objectType);
        }
    }

    sealed class TradeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var type = typeof(Trade);
            return type.IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            var list = (decimal[])serializer.Deserialize(reader, typeof(decimal[]));
            return Activator.CreateInstance(objectType, list[0], list[1]);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }

    internal class OrderBookMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public OrderBookMediaTypeFormatter()
        {
            SerializerSettings.Converters.Add(new TradeConverter());
        }
        public override Task<object> ReadFromStreamAsync(Type type, Stream stream, HttpContent content, IFormatterLogger logger)
        {
            return base.ReadFromStreamAsync(type, stream, content, logger);
        }
    }

    internal class MetacoHttpClient
    {
        private readonly string _clientKey;
        private readonly string _secretKey;
        private readonly HttpRequestBuilder _requestBuilder;

        public MetacoHttpClient()
            : this(null, null)
        {
        }

        public MetacoHttpClient(string clientKey, string secretKey)
        {
            _clientKey = clientKey;
            _secretKey = secretKey;
            _requestBuilder = new HttpRequestBuilder(_clientKey, _secretKey);
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient
            {
                BaseAddress = Configuration.ApiUrl
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public async Task<HttpResponseMessage> SendAsync(IMessageBuilder messageBuilder)
        {
            var request = _requestBuilder.Build(messageBuilder);
            return await CreateClient().SendAsync(request).ConfigureAwait(false);
        }
    }

    class HttpRequestBuilder
    {
        private readonly string _clientKey;
        private readonly string _secret;
        private static uint _nonce;
        private readonly HttpRequestMessageSigner _signer;
        private static readonly DateTime JanuryFirst1970 = new DateTime(1970, 1, 1);

        private string Nonce
        {
            get
            {
                return Convert.ToString(++_nonce);
            }
        }

        private string Timestamp
        {
            get
            {
                var time = DateTime.UtcNow - JanuryFirst1970;
                var timestamp = (long)time.TotalMilliseconds;
                return Convert.ToString(timestamp);
            }
        }

        public HttpRequestBuilder(string clientKey, string secret)
        {
            _clientKey = clientKey;
            _secret = secret;
            _signer = new HttpRequestMessageSigner();
        }

        public HttpRequestMessage Build(IMessageBuilder messageBuilder)
        {
            var message = messageBuilder.Build();
            var request = new HttpRequestMessage(message.Method, message.RequestUri);
            var body = string.Empty;
            if ((message.Method == HttpMethod.Post || message.Method == HttpMethod.Post)
                && !string.IsNullOrEmpty(message.Content))
            {
                body = message.Content;
                request.Content = new StringContent(body);
            }

            var timestamp = Timestamp;
            var nonce = Nonce;

            var url = new Uri(Configuration.ApiUrl, request.RequestUri);
            var signature = _signer.Sign(_secret, request.Method.Method, url.ToString(), body, nonce, timestamp);

            request.Headers.TryAddWithoutValidation("Authorization", _clientKey + ':' + signature);
            request.Headers.Add("X-Auth-Timestamp", timestamp);
            request.Headers.Add("X-Auth-Nonce", nonce);
            request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
            return request;
        }
    }

    public class HttpRequestMessageSigner
    {
        public string Sign(string secret, string verb, string url, string body, string nonce, string timestamp)
        {
            var msg = JsonConvert.SerializeObject(
                new[] { verb, url, body, nonce, timestamp });
            var noncedMsg = Encoding.UTF8.GetBytes(nonce + msg);

            var sha256 = new SHA256Managed();
            var hashedNoncedMessage = sha256.ComputeHash(noncedMsg);

            var toSign = Encoding.UTF8.GetBytes(url).Concat(hashedNoncedMessage);
            var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secret));
            var hmacDigest = hmac.ComputeHash(toSign.ToArray());
            return Convert.ToBase64String(hmacDigest);
        }
    }

    public static class HttpResponseMessageExtensions
    {
        internal static async Task<T> ReadAsAsync<T>(this Task<HttpResponseMessage> task)
        {
            var response = await task;
            return await response.ReadAsAsync<T>(new MediaTypeFormatter[] { new JsonMediaTypeFormatter() });
        }

        internal static async Task<TResult> ReadAsAsync<TResult, TMediaTypeFormatter>(this Task<HttpResponseMessage> task) where TMediaTypeFormatter : MediaTypeFormatter, new()
        {
            var response = await task;
            return await response.ReadAsAsync<TResult>(new MediaTypeFormatter[] { new TMediaTypeFormatter() });
        }

        internal static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response, params MediaTypeFormatter[] formatters)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = response.Content;
                var mediaTypes = new MediaTypeFormatter[] { new JsonMediaTypeFormatter() };
                var error = response.StatusCode == (HttpStatusCode)422
                    ? await content.ReadAsAsync<ValidationsError>(mediaTypes)
                    : await content.ReadAsAsync<Error>(mediaTypes);
                throw new ItBitApiException(error, response.StatusCode, response.ReasonPhrase);
            }

            return await response.Content.ReadAsAsync<T>(formatters);
        }

    }





    static class StringExtensions
    {
        public static string Uri(this string str, params object[] args)
        {
            return string.Format(str, args);
        }
    }

} // end of namespace
