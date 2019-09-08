using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Metaco.ItBit
{
	public class NewCryptoCurrencyWithdrawal
	{
		[JsonProperty("currency")]
		[JsonConverter(typeof (StringEnumConverter))]
		public CurrencyCode Currency { get; set; }

		[JsonProperty("amount")]
		[JsonConverter(typeof(NumberToStringConverter))]
		public decimal Amount { get; set; }

		[JsonProperty("address")]
		public string Address { get; set; }

		public NewCryptoCurrencyWithdrawal(CurrencyCode currency, decimal amount, string address)
		{
			Currency = currency;
			Amount = amount;
			Address = address;
		}
	}
}
