using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Metaco.ItBit
{
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
			var client =  new HttpClient {
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
}
