using System;
using Xunit;
using Info.Blockchain.API.Client;

namespace Info.Blockchain.API.Tests.UnitTests
{
	public class TransactionTests
	{
		[Fact]
		public async void GetTransaction_BadIds_ArgumentExecption()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetTransactionByHashAsync(null);
				}
			});

			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetTransactionByIndexAsync(-1);
				}
			});
		}
	}
}
