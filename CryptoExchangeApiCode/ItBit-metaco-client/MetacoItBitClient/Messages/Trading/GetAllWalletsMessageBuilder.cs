using System;
using System.Net.Http;

namespace Metaco.ItBit
{
	internal class GetAllWalletsMessageBuilder : IMessageBuilder
	{
		private readonly Guid _userId;
		private readonly Page _page;

		public GetAllWalletsMessageBuilder(Guid userId)
			: this(userId, Page.Default)
		{
		}

		public GetAllWalletsMessageBuilder(Guid userId, Page page)
		{
			_userId = userId;
			_page = page;
		}

		public RequestMessage Build()
		{
			return new RequestMessage {
				Method = HttpMethod.Get,
				RequestUri = new Uri("/v1/wallets?userId={0}&{1}"
					.Uri(_userId, _page.ToQueryString()), UriKind.Relative)
			};
		}
	}
}