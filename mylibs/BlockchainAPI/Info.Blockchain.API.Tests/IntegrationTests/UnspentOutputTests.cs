using System.Collections.ObjectModel;
using Info.Blockchain.API.Models;
using Info.Blockchain.API.Client;
using Xunit;
using System.Collections.Generic;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
	public class UnspentOutputTests
	{
		[Fact]
		public async void GetUnspent_ByAdress_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				const string address = "1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa";
				ReadOnlyCollection<UnspentOutput> outputs = await apiHelper.blockExplorer.GetUnspentOutputsAsync(new List<string>() {address});
				Assert.NotNull(outputs);
			}
		}
	}
}
