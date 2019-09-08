using System.Collections.ObjectModel;
using Info.Blockchain.API.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Info.Blockchain.API.Models
{
	/// <summary>
	/// Represents an unspent transaction output.
	/// </summary>
	public class UnspentOutput
	{
		[JsonConstructor]
		private UnspentOutput()
		{
		}

		/// <summary>
		/// Index of the output in a transaction
		/// </summary>
		[JsonProperty("tx_output_n", Required = Required.Always)]
		public int N { get; private set; }

		/// <summary>
		/// Transaction hash
		/// </summary>
		[JsonProperty("tx_hash", Required = Required.Always)]
		public string TransactionHash { get; private set; }

		/// <summary>
		/// Transaction index
		/// </summary>
		[JsonProperty("tx_index", Required = Required.Always)]
		public long TransactionIndex { get; private set; }

		/// <summary>
		/// Output script
		/// </summary>
		[JsonProperty("script", Required = Required.Always)]
		public string Script { get; private set; }

		/// <summary>
		/// Value of the output
		/// </summary>
		[JsonProperty("value", Required = Required.Always)]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue Value { get; private set; }

		/// <summary>
		/// Number of confirmations
		/// </summary>
		[JsonProperty("confirmations", Required = Required.Always)]
		public long Confirmations { get; private set; }

		public static ReadOnlyCollection<UnspentOutput> DeserializeMultiple(string outputsJson)
		{
			JObject jObject = JObject.Parse(outputsJson);
			return jObject["unspent_outputs"].ToObject<ReadOnlyCollection<UnspentOutput>>();
		}
	}
}