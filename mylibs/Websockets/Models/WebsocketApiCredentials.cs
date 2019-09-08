using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Tools.Websockets.Models
{
    public class WebsocketApiCredentials
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("apiSecret")]
        public string ApiSecret { get; set; }

        public WebsocketApiCredentials(string apikey, string apisecret)
        {
            this.ApiKey = apikey;
            this.ApiSecret = apisecret;
        }

        public override string ToString()
        {
            return $@"{{'apiKey': '{ApiKey}', 'apiSecret': '{ApiSecret}'}}";
        }
    }

}
