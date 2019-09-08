using Newtonsoft.Json;

namespace Metaco.ItBit 
{
	public class OrderBook
	{
		// currency pair for market. e.g. XBTUSD for USD Bitcoin market.
		[JsonProperty("asks")]
		public Trade[] Asks { get; set; }

		// highest bid price
		[JsonProperty("bids")]
		public Trade[] Bids { get; set; }
	}
}
