using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Metaco.ItBit
{
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
			if((message.Method == HttpMethod.Post || message.Method == HttpMethod.Post) 
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
				new[] { verb, url, body, nonce, timestamp});
			var noncedMsg = Encoding.UTF8.GetBytes(nonce + msg);

			var sha256 = new SHA256Managed();
			var hashedNoncedMessage = sha256.ComputeHash(noncedMsg);

			var toSign = Encoding.UTF8.GetBytes(url).Concat(hashedNoncedMessage);
			var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secret));
			var hmacDigest = hmac.ComputeHash(toSign.ToArray());
			return Convert.ToBase64String(hmacDigest);
		}
	}
}
