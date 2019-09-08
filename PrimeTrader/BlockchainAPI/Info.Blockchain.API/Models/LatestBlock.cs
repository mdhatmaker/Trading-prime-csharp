using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
	/// <summary>
	/// Used as a response to the `GetLatestBlock` method in the `BlockExplorer` class.
	/// </summary>
	public class LatestBlock : SimpleBlock
	{
		[JsonConstructor]
		private LatestBlock() : base(true)
		{
		}

		/// <summary>
		/// Block index
		/// </summary>
		[JsonProperty("block_index", Required = Required.Always)]
		public long Index { get; private set; }

		/// <summary>
		/// Transaction indexes included in this block
		/// </summary>
		[JsonProperty("txIndexes", Required = Required.Always)]
		public ReadOnlyCollection<long> TransactionIndexes { get; private set; }
	}
}
