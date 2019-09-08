using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Metaco.ItBit
{
	public class NewOrder
	{
		[JsonProperty("side")]
		[JsonConverter(typeof (StringEnumConverter))]
		public OrderSide Side { get; private set; }

		[JsonProperty("instrument")]
		[JsonConverter(typeof (StringEnumConverter))]
		public TickerSymbol Instrument { get; private set; }

		[JsonProperty("type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public OrderType Type { get; private set; }

		[JsonProperty("currency")]
		[JsonConverter(typeof (StringEnumConverter))]
		public CurrencyCode Currency { get; private set; }

		[JsonProperty("amount")]
		[JsonConverter(typeof(NumberToStringConverter))]
		public decimal Amount { get; private set; }

		[JsonProperty("display")]
		[JsonConverter(typeof(NumberToStringConverter))]
		public decimal Display { get; set; }

		[JsonProperty("price")]
		[JsonConverter(typeof(NumberToStringConverter))]
		public decimal Price { get; private set; }

		[JsonProperty("clientOrderIdentifier", NullValueHandling = NullValueHandling.Ignore)]
		public string ClientOrderIdentifier { get; set; }

		public static NewOrder Buy(TickerSymbol instrument, CurrencyCode currency, decimal amount, decimal price)
		{
			return new NewOrder {
				Side = OrderSide.buy,
				Instrument = instrument,
				Type = OrderType.limit,
				Currency = currency,
				Price = price,
				Amount = amount,
				Display = amount
			};
		}

		public static NewOrder Sell(TickerSymbol instrument, CurrencyCode currency, decimal amount, decimal price)
		{
			return new NewOrder
			{
				Side = OrderSide.sell,
				Instrument = instrument,
				Type = OrderType.limit,
				Currency = currency,
				Price = price,
				Amount = amount,
				Display = amount
			};
		}

		private NewOrder()
		{
		}
	}
}
