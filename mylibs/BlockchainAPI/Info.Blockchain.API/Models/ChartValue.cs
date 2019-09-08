using Newtonsoft.Json;

namespace Info.Blockchain.API.Models
{
    /// <summary>
    /// A class representing a single chart value
    /// </summary>
    public class ChartValue
    {
        /// <summary>
		/// X Value
		/// </summary>
		[JsonProperty("x", Required = Required.Always)]
		public double X { get; private set; }

        /// <summary>
		/// Y Value
		/// </summary>
		[JsonProperty("y", Required = Required.Always)]
		public double Y { get; private set; }
    }
}