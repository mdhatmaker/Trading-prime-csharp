using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Metaco.ItBit
{
	public enum OrderStatus
	{
		submitted,
		open,
		filled,
		cancelled,
		rejected
	}

	public class Order
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }

		[JsonProperty("walletId")]
		public Guid WalletId { get; set; }

		[JsonProperty("side")]
		[JsonConverter(typeof(StringEnumConverter))]
		public OrderSide Side { get; set; }

		[JsonProperty("instrument")]
		[JsonConverter(typeof(StringEnumConverter))]
		public TickerSymbol Instrument { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("currency")]
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCode Currency { get; set; }

		[JsonProperty("amount")]
		public decimal Amount { get; set; }

		[JsonProperty("price")]
		public decimal Price { get; set; }

		[JsonProperty("amountFilled")]
		public decimal AmountFilled { get; set; }

		[JsonProperty("VolumeWeightedAveragePrice")]
		public decimal VolumeWeightedAveragePrice { get; set; }

		[JsonProperty("createdTime")]
		public DateTime CreatedTime { get; set; }

		[JsonProperty("status")]
		[JsonConverter(typeof(StringEnumConverter))]
		public OrderStatus Status { get; set; }

		[JsonProperty("clientOrderIdentifier")]
		public string ClientOrderIdentifier { get; set; }
	}
}
