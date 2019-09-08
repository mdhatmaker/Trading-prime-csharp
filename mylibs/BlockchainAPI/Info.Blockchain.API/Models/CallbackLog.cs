using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
    public class CallbackLog
    {
        [JsonProperty("callback")]
        public string CallbackUrl { get; private set; }

        [JsonProperty("called_at")]
        public string CallDateString { get; private set; }

        [JsonProperty("raw_response")]
        public string RawResponse { get; private set; }

        [JsonProperty("response_code")]
        public int ResponseCode { get; private set; }
    }
}