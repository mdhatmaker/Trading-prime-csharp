using System;
using System.Linq;
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
using KrakenCore;
using CryptoTools.Net;
using static CryptoTools.Global;

namespace CryptoApis.RestApi
{
    // https://github.com/discosultan/KrakenCore

    public class KrakenRestApi : ICryptoRestApi
    {
        private KrakenClient m_client;
              
        public KrakenRestApi(string apiKey, string apiSecret)
        {
            m_client = new KrakenClient(apiKey, apiSecret);
        }

        // This TEST method should show sample code for each of the following:
        // method: Get Account Balances (for each currency), Get Deposit Addresses (for each currency)
        // method: Get Deposit History, Get Withdrawal History
        // method: Withdraw (to cryptoAddress)
        public async Task Test()
        {

            // *** BALANCES ***
            Console.WriteLine("\n--- BALANCES ---");
            var resB = await m_client.GetAccountBalance();
            foreach (var currency in resB.Result)
                Console.WriteLine("{0} {1,14:0.00000000}", currency.Key, currency.Value);

            Console.WriteLine("\n--- ASSET INFO ---");
            var resA = await m_client.GetAssetInfo();
            foreach (var kv in resA.Result)
                Console.WriteLine("{0} : {1} {2} {3} {4}", kv.Key, kv.Value.AlternateName, kv.Value.AssetClass, kv.Value.Decimals, kv.Value.DisplayDecimals);

            Console.WriteLine("\n--- LEDGERS ---");
            var resL = await m_client.GetLedgersInfo();
            foreach (var kv in resL.Result.Ledgers)
                Console.WriteLine("{0} : {1} {2} {3} {4} {5} {6} {7} {8}", kv.Key, kv.Value.Asset, kv.Value.AssetClass, kv.Value.Amount, kv.Value.Fee, kv.Value.Balance, kv.Value.RefId, kv.Value.Type, kv.Value.Time);

            /*// Withdraw to this crypto address
            string cryptoAddress = "37WnP6QXmjwBrP6YkrCUTGBcmQWbFZDpBi";
            await TestWithdraw(cryptoAddress, 0.0001M, Currency.BTC);

            await TestTransactionHistory();*/
        }

        public async Task<XTickerMap> GetTickers(IEnumerable<string> symbols)
        {
            string pairs = string.Join(",", symbols);
            var res = await m_client.GetTickerInformation(pairs);
            return new XTickerMap(res.Result);
        }

        #region ---------- ICryptoApi ---------------------------------------------------------------
        public string Exchange { get { return "KRAKEN"; } }

        public List<string> GetAllSymbols()
        {
            var result = new List<string>();
            var res = m_client.GetTradableAssetPairs();
            res.Wait();
            foreach (var kv in res.Result.Result)
            {
                result.Add(kv.Key);
                // kv.Value is KrakenCore.Models.AssetPair
            }
            return result;
        }

        public string GetSymbol(string symbolId)
        {
            if (symbolId == "btcusd") return "XBTUSD";
            else if (symbolId == "ethusd") return "ETHUSD";
            else if (symbolId == "ethbtc") return "ETHBTC";
            else
            {
                Console.WriteLine("ERROR: Symbol ID not found.");
                return null;
            }
        }

        public async Task<XTicker> GetTicker(string symbolId)
        {
            // pairs is comma-delimited list of symbols
            string pairs = GetSymbol(symbolId);
            var res = await m_client.GetTickerInformation(pairs);
            string key = res.Result.Keys.First();
            var value = res.Result[key];
            //return new XTicker(res.Result.Values.ToList()[0]);            
            return new XTicker(value);
        }

        public async Task<XBalanceMap> GetBalances()
        {
            var res = await m_client.GetAccountBalance();
            /*IEnumerable<string> assets = res.Result.Keys;
            var li = new List<string>();
            li.Add("XXBTZUSD");
            li.Add("XXBTZEUR");
            var tickers = GetTickers(assets);*/
            // TODO: Get Wallet cryptoAddresses
            //var res = await m_client.GetAssetInfo();
            var map = new XBalanceMap(res.Result);
            return map;
        }
        #endregion ----------------------------------------------------------------------------------



    } // end of class KrakenApi
} // end of namespace


/*
The client supports two extensibility points: one right before a request to Kraken is dispatched and one
right after a response is received. These points provide additional context specific information (for example,
the cost of a particular call) and can be used to implement features such as rate limiting or logging.

var client = new KrakenClient(ApiKey, PrivateKey)
{
    InterceptRequest = async req =>
    {
        // Log request.
        output.WriteLine(req.HttpRequest.ToString());
        string content = await req.HttpRequest.Content.ReadAsStringAsync();
        if (!string.IsNullOrWhiteSpace(content)) output.WriteLine(content);

        // Wait if we have hit the API rate limit.
        RateLimiter limiter = req.HttpRequest.RequestUri.OriginalString.Contains("/private/")
            ? privateApiRateLimiter
            : publicApiRateLimiter;
        await limiter.WaitAccess(req.ApiCallCost);
    }
};
*/
