using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Metaco.ItBit
{
	internal class NewCryptoCurrencyWithdrawalMessageBuilder : IMessageBuilder
	{
		private readonly Guid _walletId;
		private readonly NewCryptoCurrencyWithdrawal _withdrawal;

		public NewCryptoCurrencyWithdrawalMessageBuilder(Guid walletId, NewCryptoCurrencyWithdrawal withdrawal)
		{
			_walletId = walletId;
			_withdrawal = withdrawal;
		}

		public RequestMessage Build()
		{
			return new RequestMessage {
				Method = HttpMethod.Post,
				RequestUri = new Uri("/v1/wallets/{0}/cryptocurrency_withdrawals".Uri(_walletId), UriKind.Relative),
				Content = JsonConvert.SerializeObject(_withdrawal, Formatting.None)
			};
		}
	}
}
