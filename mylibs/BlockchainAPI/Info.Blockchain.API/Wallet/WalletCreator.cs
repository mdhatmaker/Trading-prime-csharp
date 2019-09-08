using System;
using System.Threading.Tasks;
using Info.Blockchain.API.Client;
using Info.Blockchain.API.Models;

namespace Info.Blockchain.API.Wallet
{
    /// <summary>
    /// This class reflects the functionality documented at https://blockchain.info/api/create_wallet.
    /// <summary>
    public class WalletCreator
    {
        private readonly IHttpClient httpClient;

        public WalletCreator()
        {
            httpClient = new BlockchainHttpClient(uri: "http://127.0.0.1:3000");
        }

        public WalletCreator(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
		/// Creates a new Blockchain.info wallet if the user's API code has the 'generate wallet' permission.
		/// It can be created containing a pre-generated private key or will otherwise generate a new private key.
		/// </summary>
		/// <param name="password">Password for the new wallet. At least 10 characters.</param>
		/// <param name="privateKey">Private key to add to the wallet</param>
		/// <param name="label">Label for the first address in the wallet</param>
		/// <param name="email">Email to associate with the new wallet</param>
		/// <returns>An instance of the CreateWalletResponse class</returns>
		/// <exception cref="ServerApiException">If the server returns an error</exception>
		public async Task<CreateWalletResponse> CreateAsync(string password, string privateKey = null, string label = null, string email = null)
		{
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (string.IsNullOrWhiteSpace(httpClient.ApiCode))
            {
                throw new ArgumentNullException("Api code must be specified", innerException: null);
            }

            var request = new CreateWalletRequest {
                Password = password,
                ApiCode = httpClient.ApiCode,
                PrivateKey = privateKey,
                Label = label,
                Email = email
            };

            var newWallet = await httpClient.PostAsync<CreateWalletRequest, CreateWalletResponse>("api/v2/create/", request, contentType: "application/json");
            return newWallet;
		}
    }
}