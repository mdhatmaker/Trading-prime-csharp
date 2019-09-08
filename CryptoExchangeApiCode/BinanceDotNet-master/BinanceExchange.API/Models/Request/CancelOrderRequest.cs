using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace BinanceExchange.API.Models.Request
{
    /// <summary>
    /// Request object used to cancel a Binance order
    /// </summary>
    [DataContract]
    public class CancelOrderRequest : IRequest
    {
        [DataMember(Order = 1)]
        public string Symbol { get; set; }

        [DataMember(Order = 2)]
        [JsonProperty(PropertyName = "orderId")]
        public long OrderId { get; set; }

        [DataMember(Order = 3)]
        [JsonProperty(PropertyName = "origClientOrderId")]
        //public long OriginalClientOrderId { get; set; }
        public string OriginalClientOrderId { get; set; }

        [DataMember(Order = 4)]
        [JsonProperty(PropertyName = "newClientOrderId")]
        //public long NewClientOrderId { get; set; }
        public string NewClientOrderId { get; set; }
    }
}