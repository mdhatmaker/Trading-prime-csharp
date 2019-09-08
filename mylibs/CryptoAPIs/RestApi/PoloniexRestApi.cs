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
using Poloniex;
using CryptoTools.Net;
using static CryptoTools.Global;

namespace CryptoApis.RestApi
{
    // https://github.com/ChristopherHaws/poloniex-dotnet
    // https://m.poloniex.com/support/api/

    public class PoloniexRestApi : ICryptoRestApi
    {
        private PoloniexClient m_client;

        public PoloniexRestApi(string apiKey, string apiSecret)
        {
            m_client = new PoloniexClient(apiKey, apiSecret);
        }

        // This TEST method should show sample code for each of the following:
        // method: Get Account Balances (for each currency), Get Deposit Addresses (for each currency)
        // method: Get Deposit History, Get Withdrawal History
        // method: Withdraw (to cryptoAddress)
        public static void Test()
        {
            // API test code goes here
        }

        #region ---------- ICryptoApi ---------------------------------------------------------------
        public string Exchange { get { return "POLONIEX"; } }

        public List<string> GetAllSymbols()
        {
            var result = new List<string>();
            var res = m_client.GetTickersAsync();
            foreach (var t in res.Result)
            {
                result.Add(t.Key);
            }
            return result;
        }

        public string GetSymbol(string symbolId)
        {
            if (symbolId == "btcusdt") return "USDT_BTC";
            else if (symbolId == "ethusdt") return "USDT_ETH";
            else if (symbolId == "ethbtc") return "BTC_ETH";
            else
            {
                Console.WriteLine("ERROR: Symbol ID not found.");
                return null;
            }
        }

        public async Task<XTicker> GetTicker(string symbolId)
        {
            string symbol = GetSymbol(symbolId);
            //var res = await m_client.GetTickerAsync(symbol);
            var res = await m_client.GetTickersAsync();
            return new XTicker(res[symbol]);
        }

        public async Task<XBalanceMap> GetBalances()
        {
            var res = await m_client.GetBalancesAsync();
            return new XBalanceMap(res);
        }
        #endregion ----------------------------------------------------------------------------------


    } // end of class PoloniexRestApi
} // end of namespace

