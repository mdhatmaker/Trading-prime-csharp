using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Metaco.ItBit
{
	public class MarketClient
	{
		private HttpClient CreateClient()
		{
			var client = new HttpClient
			{
				BaseAddress = new Uri("https://api.itbit.com/v1", UriKind.RelativeOrAbsolute)
			};
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			return client;
		}

		private async Task<HttpResponseMessage> SendAsync(IMessageBuilder messageBuilder)
		{
			if (messageBuilder == null)
				throw new ArgumentNullException("messageBuilder");

			var message = messageBuilder.Build();
			var request = new HttpRequestMessage(message.Method, message.RequestUri);
			return await CreateClient().SendAsync(request).ConfigureAwait(false);
		}

		public Task<Ticker> GetTickerAsync(TickerSymbol symbol)
		{
			if (!Enum.IsDefined(typeof(TickerSymbol), symbol))
				throw new ArgumentException("symbol");

			var request = new GetTickerMessageBuilder(symbol);
			return SendAsync(request).ReadAsAsync<Ticker>();
		}

		public Task<OrderBook> GetOrderBookAsync(TickerSymbol symbol)
		{
			if (!Enum.IsDefined(typeof(TickerSymbol), symbol))
				throw new ArgumentException("symbol");

			var request = new GetOrderBookMessageBuilder(symbol);
			return SendAsync(request).ReadAsAsync<OrderBook, OrderBookMediaTypeFormatter>();
		}

		public Task<RecentTrades> GetRecentTradesAsync(TickerSymbol symbol, int? since = null)
		{
			if (!Enum.IsDefined(typeof(TickerSymbol), symbol))
				throw new ArgumentException("symbol");

			var request = new GetRecentTradesMessageBuilder(symbol, since);
			return SendAsync(request).ReadAsAsync<RecentTrades>();
		}
	}
}