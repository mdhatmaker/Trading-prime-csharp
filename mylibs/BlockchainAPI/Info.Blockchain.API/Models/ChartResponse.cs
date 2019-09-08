using System.Collections.Generic;
using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
    /// <summary>
	/// This class is used as a response object to the chart 'get' method in the 'Statistics' class
	/// </summary>
    public class ChartResponse
    {
        [JsonConstructor]
        private ChartResponse() {}

        /// <summary>
		/// Chart name
		/// </summary>
		[JsonProperty("name", Required = Required.Always)]
		public string ChartName { get; private set; }

        /// <summary>
		/// Measuring unit
		/// </summary>
		[JsonProperty("unit", Required = Required.Always)]
		public string Unit { get; private set; }

        /// <summary>
		/// The timespan covered in this chart response
		/// </summary>
        [JsonProperty("period", Required = Required.Always)]
		public string Timespan { get; private set; }

        /// <summary>
		/// A description of the chart
		/// </summary>
		[JsonProperty("description", Required = Required.Always)]
		public string Description { get; private set; }

        /// <summary>
		/// Chart values
		/// </summary>
		[JsonProperty("values", Required = Required.Always)]
		public IEnumerable<ChartValue> Values { get; private set; }

    }
}