using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Metaco.ItBit
{
	internal class OrderBookMediaTypeFormatter : JsonMediaTypeFormatter
	{
		public OrderBookMediaTypeFormatter()
		{
			SerializerSettings.Converters.Add(new TradeConverter());
		}
		public override Task<object> ReadFromStreamAsync(Type type, Stream stream, HttpContent content, IFormatterLogger logger)
		{
			return base.ReadFromStreamAsync(type, stream, content, logger);
		}
	}
}
