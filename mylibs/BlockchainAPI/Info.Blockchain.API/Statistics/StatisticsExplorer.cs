using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Info.Blockchain.API.Client;
using Info.Blockchain.API.Models;

namespace Info.Blockchain.API.Statistics
{
	/// <summary>
	/// This class allows users to get the bitcoin network statistics.
	/// It reflects the functionality documented at https://blockchain.info/api/charts_api
	/// </summary>
	public class StatisticsExplorer
	{
		private readonly IHttpClient httpClient;
		public StatisticsExplorer()
		{
			httpClient = new BlockchainHttpClient("https://api.blockchain.info");
		}
		internal StatisticsExplorer(IHttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		/// <summary>
		/// Gets the network statistics.
		/// </summary>
		/// <returns>An instance of the StatisticsResponse class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<StatisticsResponse> GetStatsAsync()
		{
            var queryString = new QueryString();
            queryString.Add("format","json");
			return await httpClient.GetAsync<StatisticsResponse>("stats", queryString);
		}

        /// <summary>
        /// Gets chart data for a specified chart
        /// </summary>
        /// <param name="chartType">Chart name</param>
        /// <param name="timespan">Optional timespan to include</param>
        /// <param name="rollingAverage">Optional duration over which data should be averaged</param>
        /// <returns>Chart response object</returns>
        /// <exception cref="ServerApiException">If the server returns an error</exception>
        public async Task<ChartResponse> GetChartAsync(string chartType, string timespan = null, string rollingAverage = null)
        {
            var queryString = new QueryString();
            queryString.Add("format","json");
            if (timespan != null)
            {
                queryString.Add("timespan", timespan);
            }
            if (rollingAverage != null)
            {
                queryString.Add("rollingAverage", rollingAverage);
            }
            try
            {
                return await httpClient.GetAsync<ChartResponse>("charts/" + chartType, queryString);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("No chart with this name"))
                {
                    throw new ArgumentOutOfRangeException(nameof(chartType), "This chart name does not exist");
                }
                if (ex.Message.Contains("Could not parse timestring"))
                {
                    throw new ArgumentOutOfRangeException(nameof(timespan), "Incorrect timespan format");
                }
                throw;
            }
        }

        /// <summary>
        /// Get a dictionary of pool names and the number of blocks
        /// mined in the last `timespan` days
        /// </summary>
        /// <param name="timespan">Number of days to display mined blocks for</param>
        /// <returns>A dictionary of pool names and number of blocks mined</returns>
        public async Task<IDictionary<string,int>> GetPoolsAsync(int timespan = 4)
        {
            if (timespan < 1 || timespan > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(timespan), "Timespan must be between 1 to 10");
            }

            var queryString = new QueryString();
            queryString.Add("format","json");
            queryString.Add("timespan", timespan + "days");

            return await httpClient.GetAsync<Dictionary<string,int>>("pools", queryString);
        }
	}
}
