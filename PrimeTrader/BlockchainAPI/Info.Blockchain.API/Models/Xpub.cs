using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Info.Blockchain.API.Models
{
    public class Xpub : Address
    {
        [JsonProperty("change_index")]
        public int ChangeIndex { get; private set; }

        [JsonProperty("account_index")]
        public int AccountIndex { get; private set; }

        [JsonProperty("gap_limit")]
        public int GapLimit { get; private set; }

        public static Xpub Deserialize(string xpubJson)
		{
			JObject xpubJObject = JObject.Parse(xpubJson);
            JToken xpubOutput = xpubJObject["addresses"].AsJEnumerable().FirstOrDefault();
            xpubOutput["txs"] = xpubJObject["txs"];
			return xpubOutput.ToObject<Xpub>();
		}
    }
}