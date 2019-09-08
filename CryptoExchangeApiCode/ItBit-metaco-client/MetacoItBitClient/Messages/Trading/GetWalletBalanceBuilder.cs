using System;
using System.Net.Http;

namespace Metaco.ItBit
{
	internal class GetWalletBalanceMessageBuilder : IMessageBuilder
	{
		private readonly Guid _walletId;
		private readonly string _currencyCode;

		public GetWalletBalanceMessageBuilder(Guid walletId, CurrencyCode currencyCode)
		{
			_walletId = walletId;
			_currencyCode = Enum.GetName(typeof (CurrencyCode), currencyCode);
		}

		public RequestMessage Build()
		{
			return new RequestMessage {
				Method = HttpMethod.Get,
				RequestUri = new Uri("/v1/wallets/{0}/balances/{1}".Uri(_walletId, _currencyCode), UriKind.Relative)
			};
		}
	}
}
