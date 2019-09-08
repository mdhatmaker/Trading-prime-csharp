using System.Collections.ObjectModel;
using Info.Blockchain.API.Models;
using Info.Blockchain.API.Client;
using KellermanSoftware.CompareNetObjects;
using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
	public class TransactionTests
	{
		[Fact(Skip = "Test freezes because of comparison tool")]
		public async void GetTransaction_ByHash_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				Transaction knownTransaction = ReflectionUtil.DeserializeFile<Transaction>("single_transaction");
				Transaction receivedTransaction = await apiHelper.blockExplorer.GetTransactionByHashAsync(knownTransaction.Hash);

				CompareLogic compareLogic = new CompareLogic();
				ComparisonResult comparisonResult = compareLogic.Compare(knownTransaction, receivedTransaction);
				Assert.True(comparisonResult.AreEqual);
			}
		}

		[Fact(Skip = "Test freezes because of comparison tool")]
		public async void GetTransaction_ByIndex_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				Transaction knownTransaction = ReflectionUtil.DeserializeFile<Transaction>("single_transaction");
				Transaction receivedTransaction = await apiHelper.blockExplorer.GetTransactionByIndexAsync(knownTransaction.Index);

				CompareLogic compareLogic = new CompareLogic();
				ComparisonResult comparisonResult = compareLogic.Compare(knownTransaction, receivedTransaction);
				Assert.True(comparisonResult.AreEqual);
			}
		}

		[Fact]
		public async void GetUnconfirmedTransaction_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				ReadOnlyCollection<Transaction> unconfirmedTransactions = await apiHelper.blockExplorer.GetUnconfirmedTransactionsAsync();

				Assert.NotNull(unconfirmedTransactions);
			}
		}
	}
}
