using Newtonsoft.Json;

namespace Metaco.ItBit
{
	public class ValidationsError : Error
	{
		[JsonProperty("validationErrors")]
		public ValidationError[] ValidationErrors { get; set; }
	}

	public class ValidationError
	{
		[JsonProperty("code")]
		public int Code { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("field")]
		public string Field { get; set; }
	}
}
