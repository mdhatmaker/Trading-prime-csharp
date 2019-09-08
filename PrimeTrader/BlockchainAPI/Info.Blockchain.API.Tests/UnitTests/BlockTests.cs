using System;
using System.Collections.Generic;
using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
	public class BlockTests
	{
		[Fact]
		public async void GetBlock_BadIds_ArgumentException()
		{
			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetBlockByIndexAsync(-1);
				}
			});


			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetBlockByHashAsync(null);
				}
			});
		}

		[Fact]
		public async void GetBlocks_BadParameters_ArgumentException()
		{
			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetBlocksByTimestampAsync(-1);
				}
			});


			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetBlocksByTimestampAsync(1000);
				}
			});

			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetBlocksByTimestampAsync(int.MaxValue);
				}
			});


			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetBlocksByDateTimeAsync(DateTime.MinValue);
				}
			});

			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetBlocksByDateTimeAsync(DateTime.MaxValue);
				}
			});
		}

		[Fact]
		public async void GetBlocksByHeight_BadParameters_ArgumentException()
		{
			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetBlocksAtHeightAsync(-1);
				}
			});
		}

        [Fact]
        public async void GetAddress_BadParameters_ArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.blockExplorer.GetBase58AddressAsync("");
                }
            });

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.blockExplorer.GetBase58AddressAsync("some-address", 60);
                }
            });

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.blockExplorer.GetBase58AddressAsync("some-address", offset: -1);
                }
            });

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.blockExplorer.GetMultiAddressAsync(new List<string>());
                }
            });

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.blockExplorer.GetMultiAddressAsync(new List<string>() {"address"}, offset: -1);
                }
            });

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.blockExplorer.GetMultiAddressAsync(new List<string>() {"address"}, 60);
                }
            });
        }
	}
}
