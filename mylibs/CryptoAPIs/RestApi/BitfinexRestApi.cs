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
using Bitfinex.Net;
using Bitfinex.Net.Objects;
using CryptoTools.Net;
using static CryptoTools.Global;

namespace CryptoApis.RestApi
{
    // https://github.com/JKorf/Bitfinex.Net
    // https://bitfinex.readme.io/v1/reference

    public class BitfinexRestApi : ICryptoRestApi
    {
        private BitfinexClient m_client;

        public BitfinexRestApi(string apiKey, string apiSecret)
        {
            BitfinexClientOptions options = new BitfinexClientOptions();
            options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(apiKey, apiSecret);
            m_client = new BitfinexClient(options);
        }

        // This TEST method should show sample code for each of the following:
        // method: Get Account Balances (for each currency), Get Deposit Addresses (for each currency)
        // method: Get Deposit History, Get Withdrawal History
        // method: Withdraw (to cryptoAddress)
        public async Task Test()
        {

            /*// *** BALANCES ***
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

            // Withdraw to this crypto address
            string cryptoAddress = "37WnP6QXmjwBrP6YkrCUTGBcmQWbFZDpBi";
            await TestWithdraw(cryptoAddress, 0.0001M, Currency.BTC);

            await TestTransactionHistory();*/
        }

        #region ---------- ICryptoApi ---------------------------------------------------------------
        public string Exchange { get { return "BITFINEX"; } }

        public List<string> GetAllSymbols()
        {
            var result = new List<string>();
            // https://api.bitfinex.com/v1/symbols
            // TODO: Fix to retrieve Bitfinex symbols dynamically
            // m_client does not seem to have access to the "symbols" method, so let's hack it for now:
            result = new List<string>() { "btcusd", "ltcusd", "ltcbtc", "ethusd", "ethbtc", "etcbtc", "etcusd", "rrtusd", "rrtbtc", "zecusd", "zecbtc", "xmrusd", "xmrbtc", "dshusd", "dshbtc", "btceur", "btcjpy", "xrpusd", "xrpbtc", "iotusd", "iotbtc", "ioteth", "eosusd", "eosbtc", "eoseth", "sanusd", "sanbtc", "saneth", "omgusd", "omgbtc", "omgeth", "bchusd", "bchbtc", "bcheth", "neousd", "neobtc", "neoeth", "etpusd", "etpbtc", "etpeth", "qtmusd", "qtmbtc", "qtmeth", "avtusd", "avtbtc", "avteth", "edousd", "edobtc", "edoeth", "btgusd", "btgbtc", "datusd", "datbtc", "dateth", "qshusd", "qshbtc", "qsheth", "yywusd", "yywbtc", "yyweth", "gntusd", "gntbtc", "gnteth", "sntusd", "sntbtc", "snteth", "ioteur", "batusd", "batbtc", "bateth", "mnausd", "mnabtc", "mnaeth", "funusd", "funbtc", "funeth", "zrxusd", "zrxbtc", "zrxeth", "tnbusd", "tnbbtc", "tnbeth", "spkusd", "spkbtc", "spketh", "trxusd", "trxbtc", "trxeth", "rcnusd", "rcnbtc", "rcneth", "rlcusd", "rlcbtc", "rlceth", "aidusd", "aidbtc", "aideth", "sngusd", "sngbtc", "sngeth", "repusd", "repbtc", "repeth", "elfusd", "elfbtc", "elfeth", "btcgbp", "etheur", "ethjpy", "ethgbp", "neoeur", "neojpy", "neogbp", "eoseur", "eosjpy", "eosgbp", "iotjpy", "iotgbp", "iosusd", "iosbtc", "ioseth", "aiousd", "aiobtc", "aioeth", "requsd", "reqbtc", "reqeth", "rdnusd", "rdnbtc", "rdneth", "lrcusd", "lrcbtc", "lrceth", "waxusd", "waxbtc", "waxeth", "daiusd", "daibtc", "daieth", "cfiusd", "cfibtc", "cfieth", "agiusd", "agibtc", "agieth", "bftusd", "bftbtc", "bfteth", "mtnusd", "mtnbtc", "mtneth", "odeusd", "odebtc", "odeeth" };
            return result;
        }

        public string GetSymbol(string symbolId)
        {
            return symbolId;
            /*if (symbolId == "btcusd") return "BTCUSD";
            else if (symbolId == "ethusd") return "ETHUSD";
            else if (symbolId == "ethbtc") return "ETHBTC";
            else
            {
                Console.WriteLine("ERROR: Symbol ID not found.");
                return null;
            }*/
        }

        public async Task<XTicker> GetTicker(string symbolId)
        {
            string symbol = GetSymbol(symbolId);
            CancellationToken ct = default;
            var res = await m_client.GetTickerAsync(ct, new string[] { symbol });
            var data = res.Data;
            var res2 = await m_client.GetSymbolsAsync(ct);
            var data2 = res2.Data;
            var first = data.First();
            return new XTicker(first);
        }

        public async Task<XBalanceMap> GetBalances()
        {
            var res = await m_client.GetBalancesAsync();    //GetWalletsAsync();
            return new XBalanceMap(res.Data);
        }
        #endregion ----------------------------------------------------------------------------------


    } // end of class BitfinexApi
} // end of namespace

