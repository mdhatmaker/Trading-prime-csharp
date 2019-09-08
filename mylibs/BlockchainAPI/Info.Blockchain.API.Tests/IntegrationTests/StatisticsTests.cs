using Info.Blockchain.API.Client;
using Info.Blockchain.API.Models;
using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
    public class StatisticsTests
	{
		[Fact]
		public async void GetStatistics_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				StatisticsResponse statisticsResponse = await apiHelper.statisticsExplorer.GetStatsAsync();
				Assert.NotNull(statisticsResponse);
			}
		}

        [Fact]
        public async void GetChart_Valid()
        {
            using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
            {
                var chartResponse = await apiHelper.statisticsExplorer.GetChartAsync("hash-rate");
                Assert.NotNull(chartResponse);
            }
        }

        [Fact]
        public async void GetPools_Valid()
        {
            using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
            {
                var chartResponse = await apiHelper.statisticsExplorer.GetPoolsAsync();
                Assert.NotNull(chartResponse);
            }
        }
	}
}
