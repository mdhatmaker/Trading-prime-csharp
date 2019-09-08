using Info.Blockchain.API.Json;
using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
	/// <summary>
	/// This class is used as a response object to the 'get' method in the 'Statistics' class
	/// </summary>
	public class StatisticsResponse
	{
		/// <summary>
		/// Trade volume in the past 24 hours (in BTC)
		/// </summary>
		[JsonProperty("trade_volume_btc", Required = Required.Always)]
		public double TradeVolumeBtc { get; private set; }

		/// <summary>
		/// Trade volume in the past 24 hours (in USD)
		/// </summary>
		[JsonProperty("trade_volume_usd", Required = Required.Always)]
		public double TradeVolumeUsd { get; private set; }

		/// <summary>
		/// Miners' revenue in BTC
		/// </summary>
		[JsonProperty("miners_revenue_btc", Required = Required.Always)]
		public double MinersRevenueBtc { get; private set; }

		/// <summary>
		/// Miners' revenue in USD
		/// </summary>
		[JsonProperty("miners_revenue_usd", Required = Required.Always)]
		public double MinersRevenueUsd { get; private set; }

		/// <summary>
		/// Current market price in USD
		/// </summary>
		[JsonProperty("market_price_usd", Required = Required.Always)]
		public double MarketPriceUsd { get; private set; }

		/// <summary>
		/// Estimated transaction volume in the past 24 hours
		/// </summary>
		[JsonProperty("estimated_transaction_volume_usd", Required = Required.Always)]
		public double EstimatedTransactionVolumeUsd { get; private set; }

		/// <summary>
		/// Total fees in the past 24 hours
		/// </summary>
		[JsonProperty("total_fees_btc", Required = Required.Always)]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue TotalFeesBtc { get; private set; }

		/// <summary>
		/// Total BTC sent in the past 24 hours
		/// </summary>
		[JsonProperty("total_btc_sent", Required = Required.Always)]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue TotalBtcSent { get; private set; }

		/// <summary>
		/// Estimated BTC sent in the past 24 hours
		/// </summary>
		[JsonProperty("estimated_btc_sent", Required = Required.Always)]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue EstimatedBtcSent { get; private set; }

		/// <summary>
		/// Number of BTC mined in the past 24 hours
		/// </summary>
		[JsonProperty("n_btc_mined", Required = Required.Always)]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue BtcMined { get; private set; }

		/// <summary>
		/// Current difficulty
		/// </summary>
		[JsonProperty("difficulty", Required = Required.Always)]
		public double Difficulty { get; private set; }

		/// <summary>
		/// Minutes between blocks
		/// </summary>
		[JsonProperty("minutes_between_blocks", Required = Required.Always)]
		public double MinutesBetweenBlocks { get; private set; }

		/// <summary>
		/// Number of transactions in the past 24 hours
		/// </summary>
		[JsonProperty("n_tx", Required = Required.Always)]
		public long NumberOfTransactions { get; private set; }

		/// <summary>
		/// Current hashrate in GH/s
		/// </summary>
		[JsonProperty("hash_rate", Required = Required.Always)]
		public double HashRate { get; set; }

		/// <summary>
		/// Timestamp of when this report was compiled (in ms)
		/// </summary>
		[JsonProperty("timestamp", Required = Required.Always)]
		public long Timestamp { get; private set; }

		/// <summary>
		/// Number of blocks mined in the past 24 hours
		/// </summary>
		[JsonProperty("n_blocks_mined", Required = Required.Always)]
		public long MinedBlocks { get; private set; }

		/// <summary>
		///
		/// </summary>
		[JsonProperty("blocks_size", Required = Required.Always)]
		public long BlocksSize { get; private set; }

		/// <summary>
		/// Total BTC in existence
		/// </summary>
		[JsonProperty("totalbc", Required = Required.Always)]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue TotalBtc { get; private set; }

		/// <summary>
		/// Total number of blocks in existence
		/// </summary>
		[JsonProperty("n_blocks_total", Required = Required.Always)]
		public long TotalBlocks { get; private set; }

		/// <summary>
		/// The next block height where the difficulty retarget will occur
		/// </summary>
		[JsonProperty("nextretarget", Required = Required.Always)]
		public long NextRetarget { get; private set; }
	}
}
