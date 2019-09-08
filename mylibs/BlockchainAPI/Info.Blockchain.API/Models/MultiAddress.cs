using System.Collections.Generic;
using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
    /// <summary>
    /// Represents a multiaddr response
    /// </summary>
    public class MultiAddress
    {
        [JsonProperty("addresses", Required = Required.Always)]
        public IEnumerable<Address> Addresses { get; private set; }

        /// <summary>
		/// List of up to 50 transactions associated with the specified address
		/// </summary>
		[JsonProperty("txs", Required = Required.Always)]
		public IEnumerable<Transaction> Transactions { get; private set; }
    }
}