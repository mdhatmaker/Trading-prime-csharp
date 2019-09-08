using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;

namespace Metaco.ItBit 
{
	public class Ticker
	{
		// currency pair for market. e.g. XBTUSD for USD Bitcoin market.
		[JsonProperty("pair")]
		[JsonConverter(typeof(StringEnumConverter))]
		public TickerSymbol Pair { get; set; }

		// highest bid price
		[JsonProperty("bid")]
		public decimal Bid { get; set; }

		// highest bid amount
		[JsonProperty("bidAmt")]
		public decimal BidAmount { get; set; }

		// lowest ask price
		[JsonProperty("ask")]
		public decimal Ask { get; set; }

		// lowest ask amount
		[JsonProperty("askAmt")]
		public decimal AskAmount { get; set; }

		// last traded price
		[JsonProperty("lastPrice")]
		public decimal LastPrice { get; set; }

		// last traded amount
		[JsonProperty("lastAmt")]
		public decimal LastAmount { get; set; }

		// total traded volume in the last 24 hours
		[JsonProperty("volume24h")]
		public decimal Volume24Hs { get; set; }

		// total traded volume since midnight UTC
		[JsonProperty("volumeToday")]
		public decimal VolumeToday { get; set; }

		// highest traded price in the last 24 hours
		[JsonProperty("high24h")]
		public decimal HighestPrice24Hs { get; set; }

		// lowest traded price in the last 24 hours
		[JsonProperty("low24h")]
		public decimal LowestPrice24Hs { get; set; }

		// highest traded price since midnight UTC
		[JsonProperty("highToday")]
		public decimal HighestPriceToday { get; set; }

		// lowest traded price since midnight UTC
		[JsonProperty("lowToday")]
		public decimal LowestPriceToday { get; set; }

		// first traded price since midnight UTC
		[JsonProperty("openToday")]
		public decimal OpenToday { get; set; }

		// volume weighted average price traded since midnight UTC
		[JsonProperty("vwapToday")]
		public string vwapToday { get; set; }

		// volume weighted average price traded in the last 24 hours
		[JsonProperty("vwap24h")]
		public string vwap24h { get; set; }

		// server time in UTC
		[JsonProperty("serverTimeUTC")]
		public DateTime ServerTimeUtc { get; set; }
	}
}
