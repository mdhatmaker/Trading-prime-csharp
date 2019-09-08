using System;
using System.Net.Http;

namespace Metaco.ItBit
{
	internal class CancelOrderMessageBuilder : IMessageBuilder
	{
		private readonly Guid _walletId;
		private readonly Guid _orderId;

		public CancelOrderMessageBuilder(Guid walletId, Guid orderId)
		{
			_walletId = walletId;
			_orderId = orderId;
		}

		public RequestMessage Build()
		{
			return new RequestMessage {
				Method = HttpMethod.Delete,
				RequestUri = new Uri("/v1/wallets/{0}/orders/{1}".Uri(_walletId, _orderId), UriKind.Relative)
			};
		}
	}
}
