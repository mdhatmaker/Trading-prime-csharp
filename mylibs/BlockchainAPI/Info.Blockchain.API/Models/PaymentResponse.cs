using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
	/// <summary>
	/// Used as a response object to the `send` and `sendMany` methods in the `Wallet` class.
	/// </summary>
	public class PaymentResponse
	{
		/// <summary>
		/// Response message from the server
		/// </summary>
		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; private set; }

		/// <summary>
		/// Transaction hash
		/// </summary>
		[JsonProperty("tx_hash", Required = Required.Always)]
		public string TxHash { get; private set; }

		/// <summary>
		/// Additional response message from the server
		/// </summary>
		[JsonProperty("notice", Required = Required.Always)]
		public string Notice { get; private set; }
	}
}