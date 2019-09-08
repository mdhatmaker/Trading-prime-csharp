using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
    public class XpubGap
    {
        [JsonProperty("gap")]
        public int Gap { get; private set; }
    }
}