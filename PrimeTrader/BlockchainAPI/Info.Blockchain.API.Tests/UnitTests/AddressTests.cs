using System;
using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
    public class AddressTests
	{
		[Fact]
		public async void GetAddress_NullHash_ArgumentNullException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetHash160AddressAsync("");
				}
			});
		}

		[Fact]
		public async void GetAddress_NegativeTransactions_ArgumentOutOfRangeException()
		{
			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetBase58AddressAsync("test", -1);
				}
			});
		}

        public async void GetXpub_NullXpub_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.blockExplorer.GetXpub(null);
                }
            });
        }
	}
}
