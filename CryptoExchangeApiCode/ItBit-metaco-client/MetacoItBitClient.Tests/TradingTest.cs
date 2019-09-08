using System;
using Metaco.ItBit;
using NUnit.Framework;

namespace MetacoItBit.Tests
{
	[TestFixture]
	public class TradingTest
	{
		private const string _secretKey = "your-secret-key";
		private readonly TradeClient _client = new TradeClient("your-client-key", _secretKey);
		private readonly Guid _walletId = Guid.Parse("6376ab90-d46a-467e-8ea9-e159e653a7b9");


		[Test]
		public async void CanGetWallets()
		{
			var userId = Guid.Parse("ADFD5DB1-D462-4C59-BA3E-5480D081D76A");
			var wallets = await _client.GetAllWalletsAsync(userId, Page.Default);
			Assert.IsNotNull(wallets);
			Assert.AreEqual("Wallet", wallets[0].Name);
			Assert.AreEqual(userId, wallets[0].UserId);
		}

		[Test]
		public async void CanGetSpecifiedWallet()
		{
			var wallet = await _client.GetWalletAsync(_walletId);
			Assert.IsNotNull(wallet);
			Assert.AreEqual("Wallet", wallet.Name);
		}

		[Test]
		public async void CanGetSpecifiedWalletBalance()
		{
			var balance = await _client.GetWalletBalanceAsync(_walletId, CurrencyCode.XBT);
			Assert.IsNotNull(balance);
			Assert.AreEqual(CurrencyCode.XBT, balance.Currency);
		}

		[Test]
		public async void CanPlaceAndCancelAndOrder()
		{
			var buyOrder = NewOrder.Buy(TickerSymbol.XBTUSD, CurrencyCode.XBT, 1, 210);
			var newOrder = await _client.NewOrderAsync(_walletId, buyOrder);

			var order = await _client.GetOrderAsync(_walletId, newOrder.Id);

			await _client.CancelOrderAsync(_walletId, order.Id);
			order = await _client.GetOrderAsync(_walletId, newOrder.Id);
			Assert.AreEqual(OrderStatus.cancelled, order.Status);
		}

		[Test]
		public async void CanWithdrawCryptoCurrency()
		{
			var withdrawal = new NewCryptoCurrencyWithdrawal(CurrencyCode.XBT, 10, "18e143cdVYYkQesFsycgSoDEQAQQ2p9uWC");
			var withdrawalResult = await _client.WithdrawCryptoCurrencyAsync(_walletId, withdrawal);

		}

		[Test]
		public async void CanSignRequest()
		{
			var signer = new HttpRequestMessageSigner();

			var signature = signer.Sign(_secretKey, "GET", "https://api.itbit.com/v1", "", "123", "1234567890");

			Assert.AreEqual("IbGoBftlsOV3oAzG1wkn5WHcBdS7cRzezJwm9CrwUetWp4ZheefJdI7mINW8bDgy38nI/w0TdmQqPfqjlK1Fqg==", signature);
		}
	}
}
