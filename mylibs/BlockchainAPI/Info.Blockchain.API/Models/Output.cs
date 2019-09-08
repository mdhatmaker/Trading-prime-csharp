using Newtonsoft.Json;
using Info.Blockchain.API.Json;

namespace Info.Blockchain.API.Models
{
	/// <summary>
	/// Represents a transaction output.
	/// </summary>
	public class Output
	{
		[JsonConstructor]
		private Output()
		{
		}

		/// <summary>
		/// Index of the output in a transaction
		/// </summary>
		[JsonProperty("n", Required = Required.Always)]
		public int N { get; private set; }

		/// <summary>
		/// Value of the output
		/// </summary>
		[JsonProperty("value", Required = Required.Always)]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue Value { get; private set; }

		/// <summary>
		/// Address that the output belongs to
		/// </summary>
		[JsonProperty("addr")]
		public string Address { get; private set; }

		/// <summary>
		/// Transaction index
		/// </summary>
		[JsonProperty("tx_index", Required = Required.Always)]
		public long TxIndex { get; private set; }

		/// <summary>
		/// Output script
		/// </summary>
		[JsonProperty("script", Required = Required.Always)]
		public string Script { get; private set; }

		/// <summary>
		/// Whether the output is spent
		/// </summary>
		[JsonProperty("spent", Required = Required.Always)]
		public bool Spent { get; private set; }
	}
}
