using System;
using System.Net.Http;

namespace Metaco.ItBit
{
	internal class GetOrderBookMessageBuilder : IMessageBuilder
	{
		private readonly TickerSymbol _symbol;

		public GetOrderBookMessageBuilder(TickerSymbol symbol)
		{
			_symbol = symbol;
		}

		public RequestMessage Build()
		{
			var symbol = Enum.GetName(typeof(TickerSymbol), _symbol);

			return new RequestMessage {
				Method = HttpMethod.Get,
				RequestUri = new Uri("/v1/markets/{0}/order_book".Uri(symbol), UriKind.Relative)
			};
		}
	}
}