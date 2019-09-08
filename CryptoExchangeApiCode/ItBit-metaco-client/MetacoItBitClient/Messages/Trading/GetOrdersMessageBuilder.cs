using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Metaco.ItBit
{
	internal class GetOrdersMessageBuilder : IMessageBuilder
	{
		private readonly Guid _walletId;
		private readonly Page _page;
		private readonly string _instrument;
		private readonly string _status;

		public GetOrdersMessageBuilder(Guid walletId, Page page=null, TickerSymbol? instrument=null, OrderStatus? status=null)
		{
			_walletId = walletId;
			_page = page;
			_instrument = instrument.HasValue 
				? Enum.GetName(typeof(TickerSymbol), instrument)
				: null;
			_status = status.HasValue
				? Enum.GetName(typeof(OrderStatus), status)
				: null;
		}

		public RequestMessage Build()
		{
			var qs = new List<string>();
			if(_page != null)
				qs.Add(_page.ToQueryString());
			if(_instrument != null)
				qs.Add("instrument={0}".Uri(_instrument));
			if (_status != null)
				qs.Add("status={0}".Uri(_status));

			var qss = qs.Any()
				? "?" + string.Join("&", qs)
				: string.Empty;

			return new RequestMessage {
				Method = HttpMethod.Get,
				RequestUri = new Uri("/v1/wallets/{0}/orders{1}".Uri(_walletId, qss), UriKind.Relative)
			};
		}
	}
}
