namespace CoinbaseExchange.Tests {
    using System;
    using GDAX.NET.Core;
    using GDAX.NET.Endpoints.Account;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AccountTest {

        [TestMethod]
        public void TestListAccounts() {
            var accounts = GetAccounts();
            foreach ( var account in accounts.Accounts ) {
                Console.WriteLine( "{0:F8} {1} available", account.Available, account.Currency );
            }
            // Do something with the response.
        }

        [TestMethod]
        public void TestGetAccountHistory() {
            var accounts = GetAccounts().Accounts;
            foreach ( var account in accounts ) {
                var authContainer = GetAuthenticationContainer();
                var accountClient = new AccountClient( authContainer );
                var response = accountClient.GetAccountHistory( account.Id ).Result;

                Assert.IsTrue( response.AccountHistoryRecords != null );
            }
        }

        [TestMethod]
        public void TestGetAccountHolds() {
            var accounts = GetAccounts().Accounts;
            // Do something with the response.

            foreach ( var account in accounts ) {
                var authContainer = GetAuthenticationContainer();
                var accountClient = new AccountClient( authContainer );
                var response = accountClient.GetAccountHolds( account.Id ).Result;

                Assert.IsTrue( response.AccountHolds != null );
            }
        }

        private ListAccountsResponse GetAccounts() {
            var authContainer = GetAuthenticationContainer();
            var accountClient = new AccountClient( authContainer );
            var response = accountClient.ListAccounts().Result;
            return response;
        }

        private CBAuthenticationContainer GetAuthenticationContainer() {
            var passphrase = Configuration.Ask( Configuration.Passphrase, "What is the passphrase?" );
            var apikey = Configuration.Ask( Configuration.Apikey, "What is the API key?" );
            var secret = Configuration.Ask( Configuration.Secret, "What is the secret?" );

            var authenticationContainer = new CBAuthenticationContainer( apiKey: apikey, passphrase: passphrase, secret: secret );

            return authenticationContainer;
        }
    }
}
