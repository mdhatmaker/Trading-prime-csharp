using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Info.Blockchain.API.Client;
using Info.Blockchain.API.Models;

namespace Info.Blockchain.API.ExchangeRates
{
	/// <summary>
	/// This class reflects the functionality documented at https://blockchain.info/api/exchange_rates_api.
	/// It allows users to fetch the latest ticker data and convert amounts between BTC and fiat currencies.
	/// </summary>
	public class ExchangeRateExplorer
	{
		private readonly IHttpClient httpClient;

		public ExchangeRateExplorer()
		{
			httpClient = new BlockchainHttpClient();
		}

		internal ExchangeRateExplorer(IHttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		/// <summary>
		/// Gets the price ticker from https://blockchain.info/ticker
		/// </summary>
		/// <returns>A dictionary of currencies where the key is a 3-letter currency symbol
		/// and the value is the `Currency` class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<Dictionary<string, Currency>> GetTickerAsync()
		{
			return await httpClient.GetAsync<Dictionary<string, Currency>>("ticker");
		}

		/// <summary>
		/// Converts x value in the provided currency to BTC.
		/// </summary>
		/// <param name="currency">Currency code</param>
		/// <param name="value">Value to convert</param>
		/// <returns>Converted value in BTC</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<double> ToBtcAsync(string currency, double value)
		{
			if (string.IsNullOrWhiteSpace(currency))
			{
				throw new ArgumentNullException(nameof(currency));
			}
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than 0");
			}
			QueryString queryString = new QueryString();
			queryString.Add("currency", currency);
			queryString.Add("value", value.ToString());

			return await httpClient.GetAsync<double>("tobtc", queryString);
		}

        /// <summary>
        /// Converts a BitcoinValue object to its value in the specified currency
        /// </summary>
        /// <param name="btc">BitcoinValue representing the value to convert from</param>
        /// <param name="currency">Currency code (default USD)</param>
        /// <returns>Converted value in currency of choice</returns>
        public async Task<double> FromBtcAsync(BitcoinValue btc, string currency = "")
        {
            if (btc == null)
            {
                throw new ArgumentNullException(nameof(btc));
            }
            if (btc.GetBtc() <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(btc), "BitcoinValue must represent a value higher than 0");
            }
            QueryString queryString = new QueryString();
            queryString.Add("currency", currency);
            queryString.Add("value", btc.Satoshis.ToString());

            return await httpClient.GetAsync<double>("frombtc", queryString);
        }
	}
}