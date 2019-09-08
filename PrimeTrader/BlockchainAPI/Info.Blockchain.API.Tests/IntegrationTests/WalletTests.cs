using System;
using System.Collections.Generic;
using System.Linq;
using Info.Blockchain.API.Models;
using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.IntegrationTests
{
	public class WalletTests
	{
		private const string WALLET_ID = "773d4edb-bffe-4790-8712-8d232ce04b0c";
		private const string WALLET_PASSWORD = "Password1!";
		private const string WALLET_PASSWORD2 = "Password2!";
		private const string FIRST_ADDRESS = "17VYDFsDxBMovM1cKGEytgeqdijNcr4L5";

        [Fact(Skip = "service-my-wallet-v3 not mocked")]
        public async void SendPayment_SendBtc_NoFreeOutputs()
		{
			ServerApiException apiException = await Assert.ThrowsAsync<ServerApiException>(async () => {
				using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
				{
					Wallet.Wallet wallet = apiHelper.InitializeWallet(WalletTests.WALLET_ID, WalletTests.WALLET_PASSWORD, WalletTests.WALLET_PASSWORD2);
					await wallet.SendAsync(WalletTests.FIRST_ADDRESS, BitcoinValue.FromBtc(1));
				}
			});
			Assert.Contains("No free", apiException.Message);
		}


        [Fact(Skip = "service-my-wallet-v3 not mocked")]
        public async void SendPayment_SendMultiBtc_NoFreeOutputs()
		{
			ServerApiException apiException = await Assert.ThrowsAsync<ServerApiException>(async () => {
				using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
				{
					Wallet.Wallet wallet = apiHelper.InitializeWallet(WalletTests.WALLET_ID, WalletTests.WALLET_PASSWORD, WalletTests.WALLET_PASSWORD2);
					Dictionary<string, BitcoinValue> recipients = new Dictionary<string, BitcoinValue>()
					{
						{"17VYDFsDxBMovM1cKGEytgeqdijNcr4L5", BitcoinValue.FromBtc(1)}
					};
					await wallet.SendManyAsync(recipients);
				}
			});
			Assert.Contains("No free", apiException.Message);
		}

        [Fact(Skip = "service-my-wallet-v3 not mocked")]
        public async void ArchiveAddress_BadAddress_ServerApiException()
		{
			ServerApiException apiException = await Assert.ThrowsAsync<ServerApiException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
				{
					Wallet.Wallet wallet = apiHelper.InitializeWallet(WalletTests.WALLET_ID, WalletTests.WALLET_PASSWORD,
						WalletTests.WALLET_PASSWORD2);
					await wallet.ArchiveAddressAsync("badAddress");
				}
			});
			Assert.Contains("Checksum", apiException.Message);
		}

        [Fact(Skip = "service-my-wallet-v3 not mocked")]
        public async void GetAddresses_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				Wallet.Wallet wallet = apiHelper.InitializeWallet(WalletTests.WALLET_ID, WalletTests.WALLET_PASSWORD, WalletTests.WALLET_PASSWORD2);
				List<WalletAddress> addresses = await wallet.ListAddressesAsync();
				Assert.NotNull(addresses);
				Assert.NotEmpty(addresses);
				Assert.True(addresses.Any(a => string.Equals(a.AddressStr, WalletTests.FIRST_ADDRESS)));
			}
		}

        [Fact(Skip = "service-my-wallet-v3 not mocked")]
        public async void Unarchive_BadAddress_ServerApiException()
		{
			ServerApiException apiException = await Assert.ThrowsAsync<ServerApiException>(async () => {
				using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
				{
					Wallet.Wallet walletHelper = apiHelper.InitializeWallet(WalletTests.WALLET_ID, WalletTests.WALLET_PASSWORD, WalletTests.WALLET_PASSWORD2);
					await walletHelper.UnarchiveAddressAsync("BadAddress");
				}
			});
			Assert.Contains("Checksum", apiException.Message);
		}

        [Fact(Skip = "service-my-wallet-v3 not mocked")]
        public async void NewAddress_ArchiveThenConsolidate_Valid()
		{
			using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
			{
				Wallet.Wallet wallet = apiHelper.InitializeWallet(WalletTests.WALLET_ID, WalletTests.WALLET_PASSWORD, WalletTests.WALLET_PASSWORD2);
				WalletAddress address = await wallet.NewAddressAsync("Test");
				Assert.NotNull(address);

				string archivedAddress = await wallet.ArchiveAddressAsync(address.AddressStr);
				Assert.NotNull(archivedAddress);


				string unarchivedAddress = await wallet.UnarchiveAddressAsync(archivedAddress);
				Assert.NotNull(unarchivedAddress);
			}
		}

        [Fact(Skip = "service-my-wallet-v3 not mocked")]
        public async void CreateWallet_BadCredentials_ServerApiException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				using (BlockchainApiHelper apiHelper = new BlockchainApiHelper())
				{
					await apiHelper.walletCreator.CreateAsync("badpassword");
				}
			});
		}
	}
}