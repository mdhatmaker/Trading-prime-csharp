using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
    public class ReceivePaymentResponse
    {
        /// <summary>
		/// The newly generated address
		/// </summary>
		[JsonProperty("address", Required = Required.Always)]
		public string Address { get; private set; }

		/// <summary>
		/// The current index of the provided xpub
		/// </summary>
		[JsonProperty("index", Required = Required.Always)]
		public int Index { get; private set; }

		/// <summary>
		/// The callback URL
		/// </summary>
		[JsonProperty("callback", Required = Required.Always)]
		public string Callback { get; private set; }
    }
}