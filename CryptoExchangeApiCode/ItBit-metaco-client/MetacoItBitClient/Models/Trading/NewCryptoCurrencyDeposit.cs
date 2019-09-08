using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Metaco.ItBit
{
	public class NewCryptoCurrencyDeposit
	{
		[JsonProperty("currency")]
		[JsonConverter(typeof (StringEnumConverter))]
		public CurrencyCode Currency { get; set; }

		public NewCryptoCurrencyDeposit(CurrencyCode currency)
		{
			Currency = currency;
		}
	}
}
