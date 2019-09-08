using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Info.Blockchain.API.Client;
using Info.Blockchain.API.Models;

namespace Info.Blockchain.API.Receive
{
    public class Receive
    {
        private readonly IHttpClient httpClient;

        public Receive(IHttpClient httpClient = null)
        {
            this.httpClient = (httpClient == null)
                ? new BlockchainHttpClient("https://api.blockchain.info/v2")
                : httpClient;
        }

        /// <summary>
        /// Generate a new address for an Xpub
        /// </summary>
        /// <param name="xpub">The Xpub to generate a new address from</param>
        /// <param name="callback">The callback URL to be notified when a payment is received</param>
        /// <param name="key">The blockchain.info receive payments v2 api key</param>
        /// <param name="gapLimie">Optional. How many unused addresses are allowed before erroring out</param>
        /// <returns>ReceivePaymentResponse object</returns>
        public async Task<ReceivePaymentResponse> GenerateAddressAsync(string xpub, string callback, string key, int? gapLimit)
        {
            var queryString = new QueryString();
            queryString.Add("xpub", xpub);
            queryString.Add("callback", callback);
            queryString.Add("key", key);
            if (gapLimit.HasValue) { queryString.Add("gap_limit", gapLimit.ToString()); }

            try
            {
                return await httpClient.GetAsync<ReceivePaymentResponse>("receive", queryString);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid xpub format"))
                {
                    throw new ArgumentException(nameof(xpub), "the xpub provided is invalid");
                }
                if (ex.Message.Contains("API Key is not valid"))
                {
                    throw new ArgumentException(nameof(key), "the api key provided is invalid");
                }
                throw;
            }
        }

        /// <summary>
        /// Check the index gap between last address paid to and the last
        /// address generated using the checkgap endpoint
        /// </summary>
        /// <param name="xpub">The Xpub to check</param>
        /// <param name="key">The blockchain.info receive payments v2 api key</param>
        /// <returns>XpubGap object</returns>
        public async Task<XpubGap> CheckAddressGapAsync(string xpub, string key)
        {
            var queryString = new QueryString();
            queryString.Add("xpub", xpub);
            queryString.Add("key", key);

            try
            {
                return await httpClient.GetAsync<XpubGap>("receive/checkgap", queryString);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid xpub format"))
                {
                    throw new ArgumentException(nameof(xpub), "the xpub provided is invalid");
                }
                if (ex.Message.Contains("API Key is not valid"))
                {
                    throw new ArgumentException(nameof(key), "the api key provided is invalid");
                }
                throw;
            }
        }

        /// <summary>
        /// Get logs related to callback attempts
        /// </summary>
        /// <param name="callback">The callback url to check</param>
        /// <param name="key">The blockchain.info receive payments v2 api key</param>
        /// <returns>Logs related to the callback url supplied</returns>
        public async Task<IEnumerable<CallbackLog>> GetCallbackLogsAsync(string callback, string key)
        {
            var queryString = new QueryString();
            queryString.Add("callback", callback);
            queryString.Add("key", key);

            try
            {
                return await httpClient.GetAsync<IEnumerable<CallbackLog>>("receive/callback_log", queryString);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("API Key is not valid"))
                {
                    throw new ArgumentException(nameof(key), "the api key provided is invalid");
                }
                throw;
            }
        }
    }
}