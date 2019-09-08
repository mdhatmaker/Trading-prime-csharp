using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;

namespace Metaco.ItBit 
{
	public class Error
	{
		[JsonProperty("code")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ErrorCodes Code { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("requestId")]
		public Guid RequestId { get; set; }
	}
}
