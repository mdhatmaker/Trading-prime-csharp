using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GDAXSharp.Network.Authentication;
using GDAXSharp.Shared.Types;
using GDAXSharp.Services.Products.Types;
using GDAXSharp.Services.Orders.Types;
using GDAXSharp.Services.Orders.Models.Responses;
using CryptoApis.Models;
using CryptoTools.Net;
using static CryptoTools.Global;

namespace CryptoApis.RestApi
{
    // https://github.com/dougdellolio/gdax-csharp

    public class GdaxRestApi : ICryptoRestApi
    {
        private GDAXSharp.GDAXClient m_client;

        public GdaxRestApi(string apiKey, string apiSecret, string passphrase)
        {
            //create an authenticator with your apiKey, apiSecret and passphrase
            var authenticator = new Authenticator(apiKey, apiSecret, passphrase);

            //create the GDAX client
            m_client = new GDAXSharp.GDAXClient(authenticator);
        }

        public async Task<ProductTicker> GetTicker(ProductType productId = ProductType.BtcUsd)
        {
            var res = await m_client.ProductsService.GetProductTickerAsync(productId);
            return res;
        }

        public async Task<OrderResponse> LimitOrder(ProductType productId, OrderSide side, decimal size, decimal price, bool postOnly = true, Guid? clientOid = null)
        {
            var res = await m_client.OrdersService.PlaceLimitOrderAsync(side, productId, size, price, GoodTillTime.Day, postOnly, clientOid);
            return res;
        }

        // This TEST method should show sample code for each of the following:
        // method: Get Account Balances (for each currency), Get Deposit Addresses (for each currency)
        // method: Get Deposit History, Get Withdrawal History
        // method: Withdraw (to cryptoAddress)
        public async Task Test()
        {
            await TestAccountBalances();

            // Withdraw to this crypto address
            string cryptoAddress = "37WnP6QXmjwBrP6YkrCUTGBcmQWbFZDpBi";
            await TestWithdraw(cryptoAddress, 0.0001M, Currency.BTC);

            await TestTransactionHistory();
        }

        // Display the account balances
        private async Task TestAccountBalances()
        {
            // *** BALANCES ***
            var allAccounts = await m_client.AccountsService.GetAllAccountsAsync();
            Console.WriteLine("--- GDAX ---");
            foreach (var acct in allAccounts)
                Console.WriteLine("{0} {1,10:0.0000} {2,14:0.00000000} {3,14:0.00000000}     {4}", acct.Currency, acct.Available, acct.Hold, acct.Balance, acct.Id);  //, acct.ProfileId);
            var coinbaseAccounts = await m_client.CoinbaseAccountsService.GetAllAccountsAsync();
            Console.WriteLine("\n---Coinbase---");
            foreach (var acct in coinbaseAccounts)
                Console.WriteLine("{0,-14} {1}   {2,-8} {3,14:0.00000000}     {4}", acct.Name, acct.Currency, acct.CoinbaseAccountType, acct.Balance, acct.Id);    //, acct.Active, acct.Primary);
        }

        // Withdraw funds to specified crypto address
        // where currency like GDAX Currencies: BCH, BTC, ETH, EUR, GBP, LTC, USD
        private async Task TestWithdraw(string cryptoAddress, decimal amount = 0.0001M, Currency currency = Currency.BTC)
        {
            // *** DEPOSIT ***
            //var res = await m_client.DepositsService.DepositFundsAsync(paymentMethodId, amount, currency);
            //var res = await m_client.DepositsService.DepositCoinbaseFundsAsync(coinbaseAccountId, amount, currency);
            // *** WITHDRAW ***
            //var res = await m_client.WithdrawalsService.WithdrawFundsAsync(paymentMethodId, amount, currency);
            //var res = await m_client.WithdrawalsService.WithdrawToCoinbaseAsync(coinbaseAccountId, amount, currency);
            var res = await m_client.WithdrawalsService.WithdrawToCryptoAsync(cryptoAddress, amount, currency);
            Console.WriteLine("\nWithdraw Response: {0} {1}   {2}", res.Currency, res.Amount, res.Id);
        }

        // Display Transaction history
        private async Task TestTransactionHistory(string id = "21f38a0e-0ffa-44c9-9b0d-a942b2e04f5f")
        {
            // *** DEPOSIT AND WITHDRAWAL HISTORY ***
            // GDAX USD      21f38a0e-0ffa-44c9-9b0d-a942b2e04f5f
            // GDAX BCH      55f2db4c-4f60-47d6-84ed-b2dc4ae6c796
            // Coinbase BTC  aca5ce8d-85b9-5bc2-a9ac-c6e9c27b12a0
            var history = await m_client.AccountsService.GetAccountHistoryAsync(id, limit: 100, numberOfPages: 0);
            foreach (var r in history)
            {
                Console.WriteLine("\n{0}", id);
                // ahl: accountHistoryList
                foreach (var ahl in r)
                {
                    Console.WriteLine("{0}  {1}  {2}  {3}  {4}", ahl.Balance, ahl.CreatedAt, ahl.AccountEntryType, ahl.Amount, ahl.Id);
                    // ahl.Details.ProductId, ahl.Details.OrderId, ahl.Details.TradeId
                }
            }
        }

        public async Task PrintCoinbaseAccounts()
        {
            var coinbaseAccounts = await m_client.CoinbaseAccountsService.GetAllAccountsAsync();
            Console.WriteLine("\n\n--- COINBASE ---");
            foreach (var acct in coinbaseAccounts)
                Console.WriteLine("{0,-14} {1}   {2,-8} {3,14:0.00000000}     {4}", acct.Name, acct.Currency, acct.CoinbaseAccountType, acct.Balance, acct.Id);    //, acct.Active, acct.Primary);
        }

        /*public void GetTickers()
        {
            m_client.ProductsService.GetProductTickerAsync();
        }*/


        #region ---------- ICryptoApi ---------------------------------------------------------------
        public string Exchange { get { return "GDAX"; } }

        public List<string> GetAllSymbols()
        {
            var result = new List<string>();
            var res = m_client.ProductsService.GetAllProductsAsync();
            res.Wait();
            foreach (var p in res.Result)
            {
                result.Add(p.Id.ToString());
            }
            return result;
        }

        public string GetSymbol(string symbolId)
        {
            if (symbolId == "btcusd") return "BTC-USD";
            else if (symbolId == "ethusd") return "ETH-USD";
            else if (symbolId == "ethbtc") return "ETH-BTC";
            else
            {
                Console.WriteLine("ERROR: Symbol ID not found.");
                return null;
            }
        }

        private ProductType? GetProductType(string symbolId)
        {
            if (symbolId == "btcusd") return ProductType.BtcUsd;
            else if (symbolId == "ethusd") return ProductType.EthUsd;
            else if (symbolId == "ethbtc") return ProductType.EthBtc;
            else
            {
                Console.WriteLine("ERROR: Symbol ID not found.");
                return null;
            }  
        }

        public async Task<XTicker> GetTicker(string symbolId)
        {
            var productId = GetProductType(symbolId);
            var res = await m_client.ProductsService.GetProductTickerAsync(productId.Value);
            return new XTicker(res);
        }

        public async Task<XBalanceMap> GetBalances()
        {
            var res = await m_client.AccountsService.GetAllAccountsAsync();
            return new XBalanceMap(res);
        }
        #endregion ----------------------------------------------------------------------------------

    } // end of class GdaxApi
} // end of namespace
