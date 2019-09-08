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
using CryptoApis.Models;
using GeminiApi;
using GeminiApi.Models.Requests;
using GeminiApi.Models.Responses;
using CryptoTools.Net;
using static CryptoTools.Global;

namespace CryptoApis.RestApi
{
    // https://github.com/pkochubey/GeminiApi

    public class GeminiRestApi
    {        
        private GeminiRequest m_client;

        public GeminiRestApi(string apiKey, string apiSecret)
        {                 
            m_client = new GeminiRequest(apiKey, apiSecret);
        }

        // This TEST method should show sample code for each of the following:
        // method: Get Account Balances (for each currency), Get Deposit Addresses (for each currency)
        // method: Get Deposit History, Get Withdrawal History
        // method: Withdraw (to cryptoAddress)
        public async Task Test()
        {
            // *** BALANCES ***
            Console.WriteLine("\n--- BALANCES ---");
            var balances = m_client.GetAvailableBalances();
            foreach (var b in balances)
                Console.WriteLine("{0} {1,14:0.00000000} {2} {3}", b.Currency, b.Amount, b.Available, b.AvailableForWithdrawal);

            /*// Withdraw to this crypto address
            string cryptoAddress = "37WnP6QXmjwBrP6YkrCUTGBcmQWbFZDpBi";
            await TestWithdraw(cryptoAddress, 0.0001M, Currency.BTC);

            await TestTransactionHistory();*/
        }

        #region ---------- ICryptoApi ---------------------------------------------------------------
        public string GetSymbol(string symbolId)
        {
            if (symbolId == "btcusd") return "btcusd";
            else if (symbolId == "ethusd") return "ethusd";
            else if (symbolId == "ethbtc") return "ethbtc";
            else
            {
                Console.WriteLine("ERROR: Symbol ID not found.");
                return null;
            }
        }

        /*public async Task<XTicker> GetTicker(string symbolId)
        {
            
            // pairs is comma-delimited list of symbols
            string pairs = GetSymbol(symbolId);
            var res = await m_client.GetTickerInformation(pairs);
            return new XTicker(res.Result[pairs]);
        }

        public async Task<XBalances> GetBalances()
        {
            var res = await m_client.GetAccountBalance();
            return new XBalances(res.Result);
        }*/
        #endregion ----------------------------------------------------------------------------------


    } // end of class GeminiApi
} // end of namespace
