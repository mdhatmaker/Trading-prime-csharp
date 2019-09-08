using System.Collections.Generic;
using Info.Blockchain.API.Client;
using Info.Blockchain.API.Models;
using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
	public class CurrencyTests
	{
		[Fact]
		public async void GetTicker_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				Dictionary<string, Currency> currencies = await apiHelper.exchangeRateExplorer.GetTickerAsync();
				Assert.NotNull(currencies);
				Assert.True(currencies.Count > 0);
			}
		}

		[Fact]
		public async void ToBtc_FromUs_HasValue()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				double btcValue = await apiHelper.exchangeRateExplorer.ToBtcAsync("USD", 1000);
				Assert.True(btcValue > 0);
			}
		}

        [Fact]
        public async void FromBtc_ToUs_HasValue()
        {
            using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
                var btc = new BitcoinValue(new decimal(0.4));
				double btcValue = await apiHelper.exchangeRateExplorer.FromBtcAsync(btc);
				Assert.True(btcValue > 0);
			}
        }

        [Fact]
        public async void FromBtc_ToGbp_HasValue()
        {
            using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
                var btc = new BitcoinValue(new decimal(0.4));
				double btcValue = await apiHelper.exchangeRateExplorer.FromBtcAsync(btc, "GBP");
				Assert.True(btcValue > 0);
			}
        }
	}
}
