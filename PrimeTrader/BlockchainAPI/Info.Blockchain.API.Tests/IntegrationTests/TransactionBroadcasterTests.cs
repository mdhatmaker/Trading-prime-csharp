using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
	public class TransactionPusherTests
	{
		[Fact]
		public async void PushTransaction_BadTransaction_ServerError()
		{
			// Don't want to add transactions, check to see if the server responds
			ServerApiException serverApiException = await Assert.ThrowsAsync<ServerApiException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
				{
					await apiHelper.transactionBroadcaster.PushTransactionAsync("Test");
				}
			});
			Assert.Contains("Parse", serverApiException.Message);
		}
	}
}
