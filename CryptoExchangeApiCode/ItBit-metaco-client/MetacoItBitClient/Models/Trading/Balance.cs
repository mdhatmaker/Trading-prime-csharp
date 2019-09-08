using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Metaco.ItBit
{
	public class Balance
	{
		[JsonProperty("currency")]
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCode Currency { get; set; }

		[JsonProperty("availableBalance")]
		public decimal AvailableBalance { get; set; }

		[JsonProperty("totalBalance")]
		public decimal TotalBalance { get; set; }
	}
}