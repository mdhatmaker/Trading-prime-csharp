using System;
using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
	public class UnspentOutputTests
	{
		[Fact]
		public async void GetUnspentOutputs_NullAddress_ArgumentNullException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
				{
					await apiHelper.blockExplorer.GetUnspentOutputsAsync(null);
				}
			});
		}
	}
}