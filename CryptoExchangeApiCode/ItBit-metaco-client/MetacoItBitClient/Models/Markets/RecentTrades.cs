using Newtonsoft.Json;

namespace Metaco.ItBit
{
	public class RecentTrades
	{
		[JsonProperty("recentTrades")]
		public RecentTrade[] Trades { get; set; }
	}
}
