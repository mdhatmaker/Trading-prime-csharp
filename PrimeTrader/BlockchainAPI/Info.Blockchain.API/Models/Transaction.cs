using System;
using System.Collections.ObjectModel;
using Info.Blockchain.API.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Info.Blockchain.API.Models
{
	/// <summary>
	/// Represents a transaction.
	/// </summary>
	public class Transaction
	{
		[JsonConstructor]
		private Transaction()
		{
		}

		/// <summary>
		/// Whether the transaction is a double spend
		/// </summary>
		[JsonProperty("double_spend")]
		public bool DoubleSpend { get; private set; }

		/// <summary>
		/// Block height of the parent block. -1 for unconfirmed transactions.
		/// </summary>
		[JsonProperty("block_height")]
		public long BlockHeight { get; private set; } = -1;

		/// <summary>
		/// Timestamp of the transaction
		/// </summary>
		[JsonProperty("time", Required = Required.Always)]
		[JsonConverter(typeof(UnixDateTimeJsonConverter))]
		public DateTime Time { get; private set; }

		/// <summary>
		/// IP address that relayed the transaction
		/// </summary>
		[JsonProperty("relayed_by", Required = Required.Always)]
		public string RelayedBy { get; private set; }

		/// <summary>
		/// Transaction hash
		/// </summary>
		[JsonProperty("hash", Required = Required.Always)]
		public string Hash { get; private set; }

		/// <summary>
		/// Transaction index
		/// </summary>
		[JsonProperty("tx_index", Required = Required.Always)]
		public long Index { get; private set; }

		/// <summary>
		/// Transaction format version
		/// </summary>
		[JsonProperty("ver", Required = Required.Always)]
		public int Version { get; private set; }

		/// <summary>
		/// Serialized size of the transaction
		/// </summary>
		[JsonProperty("size", Required = Required.Always)]
		public long Size { get; private set; }

		/// <summary>
		/// List of inputs
		/// </summary>
		[JsonProperty("inputs", Required = Required.Always)]
		public ReadOnlyCollection<Input> Inputs { get; private set; }

		/// <summary>
		/// List of outputs
		/// </summary>
		[JsonProperty("out", Required = Required.Always)]
		public ReadOnlyCollection<Output> Outputs { get; private set; }

		public static ReadOnlyCollection<Transaction> DeserializeMultiple(string transactionsJson)
		{
			JObject jObject = JObject.Parse(transactionsJson);

			return jObject["txs"].ToObject<ReadOnlyCollection<Transaction>>();
		}
	}
}
