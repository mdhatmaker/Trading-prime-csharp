using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
	/// <summary>
	/// This class is used in the response of the `GetTicker` method in the `ExchangeRates` class.
	/// </summary>
	public class Currency
	{
		/// <summary>
		/// Current buy price
		/// </summary>
		[JsonProperty("buy", Required = Required.Always)]
		public double Buy { get; private set; }

		/// <summary>
		/// Current sell price
		/// </summary>
		[JsonProperty("sell", Required = Required.Always)]
		public double Sell { get; private set; }

		/// <summary>
		/// Most recent market price
		/// </summary>
		[JsonProperty("last", Required = Required.Always)]
		public double Last { get; private set; }

		/// <summary>
		/// 15 minutes delayed market price
		/// </summary>
		[JsonProperty("15m", Required = Required.Always)]
		public double Price15M { get; private set; }

		/// <summary>
		/// Currency symbol
		/// </summary>
		[JsonProperty("symbol", Required = Required.Always)]
		public string Symbol { get; private set; }
	}
}
