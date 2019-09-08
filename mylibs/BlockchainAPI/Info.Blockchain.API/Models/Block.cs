using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Info.Blockchain.API.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Info.Blockchain.API.Models
{
	/// <summary>
	/// This class is a full representation of a block. For simpler representations, see SimpleBlock and LatestBlock.
	/// </summary>
	public class Block : SimpleBlock
	{
		private DateTime receivedTime { get; set; } = DateTime.MinValue;

		[JsonConstructor]
		private Block() {}

		/// <summary>
		/// Block version as specified by the protocol
		/// </summary>
		[JsonProperty("ver", Required = Required.Always)]
		public int Version { get; private set; }

		/// <summary>
		/// Hash of the previous block
		/// </summary>
		[JsonProperty("prev_block", Required = Required.Always)]
		public string PreviousBlockHash { get; private set; }

		/// <summary>
		/// Merkle root of the block
		/// </summary>
		[JsonProperty("mrkl_root", Required = Required.Always)]
		public string MerkleRoot { get; private set; }

		/// <summary>
		/// Representation of the difficulty target for this block
		/// </summary>
		[JsonProperty("bits", Required = Required.Always)]
		public long Bits { get; private set; }

		/// <summary>
		/// Total transaction fees from this block
		/// </summary>
		[JsonProperty("fee", Required = Required.Always)]
		[JsonConverter(typeof(BitcoinValueJsonConverter))]
		public BitcoinValue Fees { get; private set; }

		/// <summary>
		/// Block nonce
		/// </summary>
		[JsonProperty("nonce", Required = Required.Always)]
		public long Nonce { get; private set; }

		/// <summary>
		/// Serialized size of this block
		/// </summary>
		[JsonProperty("size", Required = Required.Always)]
		public long Size { get; private set; }

		/// <summary>
		/// Index of this block
		/// </summary>
		[JsonProperty("block_index", Required = Required.Always)]
		public long Index { get; private set; }

		/// <summary>
		/// The time this block was received by Blockchain.info
		/// </summary>
		[JsonProperty("received_time")]
		[JsonConverter(typeof(UnixDateTimeJsonConverter))]
		public DateTime ReceivedTime
		{
			get
			{
				return this.receivedTime == DateTime.MinValue ? this.Time : this.receivedTime;
			}
			private set
			{
				this.receivedTime = value;
			}
		}

		/// <summary>
		/// IP address that relayed the block
		/// </summary>
		[JsonProperty("relayed_by")]
		public string RelayedBy { get; private set; } = "0.0.0.0";

		/// <summary>
		/// Transactions in the block
		/// </summary>!
		[JsonProperty("tx", Required = Required.Always)]
		public ReadOnlyCollection<Transaction> Transactions { get; private set; }


		//Hack to add the missing block_height value into transactions
		public static Block Deserialize(string blockJson)
		{
			JObject blockJObject = JObject.Parse(blockJson);
			foreach (JToken jToken in blockJObject["tx"].AsJEnumerable())
			{
				jToken["block_height"] = blockJObject["height"];
				jToken["double_spend"] = false;
			}
			return blockJObject.ToObject<Block>();
		}

		public new static ReadOnlyCollection<Block> DeserializeMultiple(string blocksJson)
		{
			JObject blocksJObject = JObject.Parse(blocksJson);

			List<Block> blocks = blocksJObject["blocks"]
				.AsJEnumerable()
				.Select(jToken => Block.Deserialize(jToken.ToString())).
				ToList();
			return new ReadOnlyCollection<Block>(blocks);
		}
	}
}
