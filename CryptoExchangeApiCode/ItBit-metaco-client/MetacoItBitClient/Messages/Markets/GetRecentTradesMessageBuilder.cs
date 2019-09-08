using System;
using System.Net.Http;

namespace Metaco.ItBit
{
	internal class GetRecentTradesMessageBuilder : IMessageBuilder
	{
		private readonly TickerSymbol _symbol;
		private readonly int? _since;

		public GetRecentTradesMessageBuilder(TickerSymbol symbol, int? since)
		{
			_symbol = symbol;
			_since = since;
		}

		public RequestMessage Build()
		{
			var symbol = Enum.GetName(typeof(TickerSymbol), _symbol);

			var uri = _since.HasValue
				? "/v1/markets/{0}/trades?since={1}".Uri(symbol, _since)
				: "/v1/markets/{0}/trades".Uri(symbol);

			return new RequestMessage {
				Method = HttpMethod.Get,
				RequestUri = new Uri(uri, UriKind.Relative)
			};
		}
	}
}