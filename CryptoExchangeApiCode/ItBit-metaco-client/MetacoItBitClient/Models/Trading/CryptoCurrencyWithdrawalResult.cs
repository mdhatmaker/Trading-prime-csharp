using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Metaco.ItBit
{
	public class CryptoCurrencyWithdrawalResult
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("completionDate")]
		public DateTime CompletionDate { get; set; }

		[JsonProperty("currency")]
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCode Currency { get; set; }

		[JsonProperty("amount")]
		public decimal Amount { get; set; }

		[JsonProperty("address")]
		public string Address { get; set; }
	}
}
