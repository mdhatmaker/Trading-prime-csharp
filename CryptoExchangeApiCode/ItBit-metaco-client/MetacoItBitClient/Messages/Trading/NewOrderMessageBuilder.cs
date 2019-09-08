using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Metaco.ItBit
{
	internal class NewOrderMessageBuilder : IMessageBuilder
	{
		private readonly Guid _walletId;
		private readonly NewOrder _order;

		public NewOrderMessageBuilder(Guid walletId, NewOrder order)
		{
			_walletId = walletId;
			_order = order;
		}

		public RequestMessage Build()
		{
			var body = JsonConvert.SerializeObject(_order, Formatting.None);

			return new RequestMessage {
				Method = HttpMethod.Post,
				RequestUri = new Uri("/v1/wallets/{0}/orders".Uri(_walletId), UriKind.Relative),
				Content = body
			};
		}
	}
}
