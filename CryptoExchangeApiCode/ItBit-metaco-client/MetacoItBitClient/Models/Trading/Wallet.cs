using System;
using Newtonsoft.Json;

namespace Metaco.ItBit
{
	public class Wallet
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }

		[JsonProperty("userId")]
		public Guid UserId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("balances")]
		public Balance[] Balances { get; set; }
	}
}
