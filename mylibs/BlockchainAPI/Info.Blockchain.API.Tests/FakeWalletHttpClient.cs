using Info.Blockchain.API.Client;
using System;
using System.Threading.Tasks;
using Info.Blockchain.API.Models;

namespace Info.Blockchain.API.Tests
{
    public class FakeWalletHttpClient : IHttpClient
    {

        public string ApiCode { get; set; }

        public void Dispose()
        {
        }

        public Task<T> GetAsync<T>(string route, QueryString queryString = null, Func<string, T> customDeserialization = null)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PostAsync<TPost, TResponse>(string route, TPost postObject,
            Func<string, TResponse> customDeserialization = null,
            bool multiPartContent = false, string contentType = null)
        {
            CreateWalletResponse walletResponse = ReflectionUtil.DeserializeFile<CreateWalletResponse>("create_wallet_mock");
            if (walletResponse is TResponse)
            {
                return Task.FromResult((TResponse) (object) walletResponse);
            }
            return Task.FromResult(default(TResponse));
        }
    }
}