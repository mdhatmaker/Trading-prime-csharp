using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
    public class CreateWalletRequest
	{
		[JsonConstructor]
		public CreateWalletRequest() {}

		[JsonProperty("api_code")]
		public string ApiCode { get; set; }
		[JsonProperty("email")]
		public string Email { get; set; }
		[JsonProperty("label")]
		public string Label { get; set; }
		[JsonProperty("password")]
		public string Password { get; set; }
		[JsonProperty("privateKey")]
		public string PrivateKey { get; set; }
	}
}