using System;
using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
    public class TransactionPusherTests
	{
		[Fact]
		public async void GetTransaction_BadIds_ArgumentExecption()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.transactionBroadcaster.PushTransactionAsync(null);
				}
			});
		}
	}
}
