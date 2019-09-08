using System;
using System.Collections.Generic;
using Info.Blockchain.API.Models;
using Info.Blockchain.API.Client;
using Xunit;

namespace Info.Blockchain.API.Tests.UnitTests
{
    public class WalletTests
    {

        private Wallet.Wallet GetWallet(BlockchainApiHelper apiHelper)
        {
            return apiHelper.InitializeWallet("Test", "Test");
        }

        [Fact]
        public async void ArchiveAddress_NullAddress_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    Wallet.Wallet wallet = GetWallet(apiHelper);
                    await wallet.ArchiveAddressAsync(null);
                }
            });
        }

        [Fact]
        public async void GetAddress_BadParameters_ArgumentExceptions()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    Wallet.Wallet wallet = this.GetWallet(apiHelper);
                    await wallet.GetAddressAsync(null);
                }
            });
        }

        [Fact]
        public async void Send_BadParameters_ArgumentExceptions()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    Wallet.Wallet wallet = this.GetWallet(apiHelper);
                    await wallet.SendAsync(null, BitcoinValue.Zero);
                }
            });

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    Wallet.Wallet wallet = this.GetWallet(apiHelper);
                    await wallet.SendAsync("Test", BitcoinValue.FromBtc(-1));
                }
            });
        }

        [Fact]
        public async void SendMany_NullReeipients_ArgumentNUllException()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    Wallet.Wallet wallet = this.GetWallet(apiHelper);
                    await wallet.SendManyAsync(null);
                }
            });
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    Wallet.Wallet wallet = this.GetWallet(apiHelper);
                    await wallet.SendManyAsync(new Dictionary<string, BitcoinValue>());
                }
            });
        }

        [Fact]
        public async void Unarchive_NullAddress_ArgumentNulException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    Wallet.Wallet wallet = this.GetWallet(apiHelper);
                    await wallet.UnarchiveAddressAsync(null);
                }
            });
        }

        [Fact]
        public async void CreateWallet_NullPassword_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper("APICODE"))
                {
                    await apiHelper.walletCreator.CreateAsync(null);
                }
            });
        }

        [Fact]
        public async void CreateWallet_NullApiCode_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (BlockchainApiHelper apiHelper = UnitTestUtil.GetFakeHelper())
                {
                    await apiHelper.walletCreator.CreateAsync("password");
                }
            });
        }

        [Fact]
        public async void CreateWallet_MockRequest_Valid()
        {
            using (BlockchainApiHelper apiHelper = new BlockchainApiHelper("123", new FakeWalletHttpClient(),
                                                        "123", new FakeWalletHttpClient()))
            {
                CreateWalletResponse walletResponse = await apiHelper.walletCreator.CreateAsync("Password");
                Assert.NotNull(walletResponse);

                Assert.Equal(walletResponse.Address, "12AaMuRnzw6vW6s2KPRAGeX53meTf8JbZS");
                Assert.Equal(walletResponse.Identifier, "4b8cd8e9-9480-44cc-b7f2-527e98ee3287");
                Assert.Equal(walletResponse.Label, "My Blockchain Wallet");
            }
        }
	}
}