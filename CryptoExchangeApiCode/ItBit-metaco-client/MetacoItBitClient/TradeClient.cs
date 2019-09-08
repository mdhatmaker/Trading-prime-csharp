using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Metaco.ItBit
{
	public class TradeClient
	{
		private readonly string _clientKey;
		private readonly string _secretKey;
		private readonly HttpRequestBuilder _requestBuilder;

		public TradeClient(string clientKey, string secretKey)
		{
			if (string.IsNullOrEmpty(clientKey))
				throw new ArgumentNullException("clientKey");
			if (string.IsNullOrEmpty(secretKey))
				throw new ArgumentNullException("secretKey");

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
			return client;
		}

		private async Task<HttpResponseMessage> SendAsync(IMessageBuilder messageBuilder)
		{
			var request = _requestBuilder.Build(messageBuilder);
			return await CreateClient().SendAsync(request).ConfigureAwait(false);
		}

		public Task<Wallet[]> GetAllWalletsAsync(Guid userId, Page page)
		{
			if (userId == Guid.Empty)
				throw new ArgumentException("userId");
			if (page == null)
				throw new ArgumentNullException("page");

			var request = new GetAllWalletsMessageBuilder(userId, page);
			return SendAsync(request).ReadAsAsync<Wallet[]>();
		}

		public Task<Wallet> GetWalletAsync(Guid walletId)
		{
			if (walletId == Guid.Empty)
				throw new ArgumentException("walletId");

			var request = new GetWalletMessageBuilder(walletId);
			return SendAsync(request).ReadAsAsync<Wallet>();
		}

		public Task<Balance> GetWalletBalanceAsync(Guid walletId, CurrencyCode currency)
		{
			if (walletId == Guid.Empty)
				throw new ArgumentException("walletId");
			if (!Enum.IsDefined(typeof(CurrencyCode), currency))
				throw new ArgumentException("currency");

			var request = new GetWalletBalanceMessageBuilder(walletId, currency);
			return SendAsync(request).ReadAsAsync<Balance>();
		}

		public Task<Order[]> GetOrdersAsync(Guid walletId)
		{
			if (walletId == Guid.Empty)
				throw new ArgumentException("walletId");

			var request = new GetOrdersMessageBuilder(walletId);
			return SendAsync(request).ReadAsAsync<Order[]>();
		}

		public Task<Order[]> GetOrdersAsync(Guid walletId, Page page, TickerSymbol instrument, OrderStatus status)
		{
			if (walletId == Guid.Empty)
				throw new ArgumentException("walletId");
			if (page == null)
				throw new ArgumentNullException("page");
			if (!Enum.IsDefined(typeof(TickerSymbol), instrument))
				throw new ArgumentException("instrument");
			if (!Enum.IsDefined(typeof(OrderStatus), status))
				throw new ArgumentException("status");

			var request = new GetOrdersMessageBuilder(walletId, page, instrument, status);
			return SendAsync(request).ReadAsAsync<Order[]>();
		}

		public Task <Order> GetOrderAsync(Guid walletId, Guid orderId)
		{
			if (walletId == Guid.Empty)
				throw new ArgumentException("walletId");
			if (orderId == Guid.Empty)
				throw new ArgumentException("orderId");

			var request = new GetOrderMessageBuilder(walletId, orderId);
			return SendAsync(request).ReadAsAsync<Order>();
		}

		public Task<Order> NewOrderAsync(Guid walletId, NewOrder order)
		{
			if (walletId == Guid.Empty)
				throw new ArgumentException("walletId");
			if (order == null)
				throw new ArgumentNullException("order");

			var request = new NewOrderMessageBuilder(walletId, order);
			return SendAsync(request).ReadAsAsync<Order>();
		}

		public Task<Order> CancelOrderAsync(Guid walletId, Guid orderId)
		{
			if (walletId == Guid.Empty)
				throw new ArgumentException("walletId");
			if (orderId == Guid.Empty)
				throw new ArgumentException("orderId");

			var request = new CancelOrderMessageBuilder(walletId, orderId);
			return SendAsync(request).ReadAsAsync<Order>();
		}

		public Task<CryptoCurrencyWithdrawalResult> WithdrawCryptoCurrencyAsync(Guid walletId, NewCryptoCurrencyWithdrawal withdraw)
		{
			if (walletId == Guid.Empty)
				throw new ArgumentException("walletId");
			if (withdraw == null)
				throw new ArgumentNullException("withdraw");

			var request = new NewCryptoCurrencyWithdrawalMessageBuilder(walletId, withdraw);
			return SendAsync(request).ReadAsAsync<CryptoCurrencyWithdrawalResult>();
		}

		public Task<CryptoCurrencyDepositResult> DepositCryptoCurrencyAsync(Guid walletId, NewCryptoCurrencyDeposit deposit)
		{
			if (walletId == Guid.Empty)
				throw new ArgumentException("walletId");
			if (deposit == null)
				throw new ArgumentNullException("deposit");

			var request = new NewCryptoCurrencyDepositMessageBuilder(walletId, deposit);
			return SendAsync(request).ReadAsAsync<CryptoCurrencyDepositResult>();
		}
	}
}