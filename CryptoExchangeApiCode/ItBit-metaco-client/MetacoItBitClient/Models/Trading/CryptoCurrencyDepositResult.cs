using Newtonsoft.Json;

namespace Metaco.ItBit
{
	public class CryptoCurrencyDepositResult
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("walletID")]
		public string WalletId { get; set; }

		[JsonProperty("depositAddress")]
		public string DepositAddress { get; set; }
	}
}
