using Info.Blockchain.API.Models;
using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
    public class AddressTests
	{
		[Fact]
		public async void GetAddress_ByAddress_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				const string addressString = "13k5KUK2vswXRdjgjxgCorGoY2EFGMFTnu";
				Address address = await apiHelper.blockExplorer.GetBase58AddressAsync(addressString);
				Assert.NotNull(address);
				Assert.Equal(address.Base58Check, addressString);
			}
		}

		[Fact]
		public async void GetAddress_ByHash_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				const string hash = "1e15be27e4763513af36364674eebdba5a047323";
				Address address = await apiHelper.blockExplorer.GetHash160AddressAsync(hash);
				Assert.NotNull(address);
				Assert.Equal(address.Hash160, hash);
			}
		}

		[Theory]
		[InlineData(50)]
		[InlineData(45)]
		[InlineData(35)]
		[InlineData(1)]
		[InlineData(3)]
		public async void GetAddress_LimitTransactions_Valid(int transactionCount)
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				const string hash = "1e15be27e4763513af36364674eebdba5a047323";
				Address address = await apiHelper.blockExplorer.GetBase58AddressAsync(hash, transactionCount);
				Assert.NotNull(address);
				Assert.Equal(address.Transactions.Count, transactionCount);
			}
		}

        public async void GetXpub_IsValid()
        {
            using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
            {
                const string xpub = "xpub6CmZamQcHw2TPtbGmJNEvRgfhLwitarvzFn3fBYEEkFTqztus7W7CNbf48Kxuj1bRRBmZPzQocB6qar9ay6buVkQk73ftKE1z4tt9cPHWRn";
                Xpub response = await apiHelper.blockExplorer.GetXpub(xpub);
                Assert.NotNull(response);
            }
        }
	}
}
