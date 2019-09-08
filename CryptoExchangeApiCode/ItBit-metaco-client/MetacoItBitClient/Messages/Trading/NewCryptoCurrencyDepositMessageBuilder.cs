using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Metaco.ItBit
{
	internal class NewCryptoCurrencyDepositMessageBuilder : IMessageBuilder
	{
		private readonly Guid _walletId;
		private readonly NewCryptoCurrencyDeposit _deposit;

		public NewCryptoCurrencyDepositMessageBuilder(Guid walletId, NewCryptoCurrencyDeposit deposit)
		{
			_walletId = walletId;
			_deposit = deposit;
		}

		public RequestMessage Build()
		{
			return new RequestMessage {
				Method = HttpMethod.Post,
				RequestUri = new Uri("/v1/wallets/{0}/cryptocurrency_deposits".Uri(_walletId), UriKind.Relative),
				Content = JsonConvert.SerializeObject(_deposit, Formatting.None)
			};
		}
	}
}
