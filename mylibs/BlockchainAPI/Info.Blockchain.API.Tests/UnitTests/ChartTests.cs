using System;
using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
    public class ChartTests
    {
        [Fact]
        public async void GetChart_WrongName_OutOfRangeException()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.statisticsExplorer.GetChartAsync("wrong-chart-name");
                }
            });
        }

        [Fact]
        public async void GetChart_WrongTimespan_OutOfRangeException()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.statisticsExplorer.GetChartAsync("hash-rate", "wrong-timespan-format");
                }
            });

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.statisticsExplorer.GetPoolsAsync(0);
                }
            });
        }
    }
}