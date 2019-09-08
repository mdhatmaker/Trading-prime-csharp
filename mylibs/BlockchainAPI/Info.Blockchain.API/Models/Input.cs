using Newtonsoft.Json;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Info.Blockchain.API.Models
{
	/// <summary>
	/// Represents a transaction input. If the PreviousOutput object is null,
	/// this is a coinbase input.
	/// </summary>
	public class Input
	{
		[JsonConstructor]
		private Input()
		{
		}

		/// <summary>
		/// Previous output. If null, this is a coinbase input.
		/// </summary>
		[JsonProperty("prev_out")]
		public Output PreviousOutput { get; private set; }

		/// <summary>
		/// Sequence number of the input
		/// </summary>
		[JsonProperty("sequence", Required = Required.Always)]
		public long Sequence { get; private set; }

		/// <summary>
		/// Script signature
		/// </summary>
		[JsonProperty("script", Required = Required.Always)]
		public string ScriptSignature { get; private set; }
	}
}
