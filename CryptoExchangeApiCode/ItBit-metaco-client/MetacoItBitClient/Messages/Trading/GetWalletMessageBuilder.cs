using System;
using System.Net.Http;

namespace Metaco.ItBit
{
	internal class GetWalletMessageBuilder : IMessageBuilder
	{
		private readonly Guid _walletId;

		public GetWalletMessageBuilder(Guid walletId)
		{
			_walletId = walletId;
		}

		public RequestMessage Build()
		{
			return new RequestMessage {
				Method = HttpMethod.Get,
				RequestUri = new Uri("/v1/wallets/{0}".Uri(_walletId), UriKind.Relative)
			};
		}
	}
}