using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoApis.SharedModels;
using Bittrex.Net;
using Bittrex.Net.Objects;
using CryptoTools;
using CryptoTools.Cryptography;
using static CryptoTools.Cryptography.Cryptography;

namespace CryptoTrader
{
    // ---- QUANTITIES ----
    // NEO/USDT qty=0.20
    // BNB/USDT qty=1.00
    // BCC/USDT qty=0.014
    // QTUM/USDT qty=0.75
    // LTC/USDT qty = 0.09
    // ADA/USDT qty = 50.0
    
    public class TradeBittrexArbs
    {
        // https://support.bittrex.com/hc/en-us/articles/115003723911

        static Dictionary<string, Ticker> m_tickers = new Dictionary<string, Ticker>();
        static Dictionary<string, decimal> m_lastBuy = new Dictionary<string, decimal>();
        static Dictionary<string, decimal> m_lastSell = new Dictionary<string, decimal>();

        //static BinanceExchangeInfo m_exchangeInfo;
        static Dictionary<string, BinanceSymbolInfo> m_symbolInfo = new Dictionary<string, BinanceSymbolInfo>();

        static Dictionary<string, decimal> m_tradeQty = new Dictionary<string, decimal>();
        static Dictionary<string, int> m_position = new Dictionary<string, int>();

        static int m_minPosition, m_maxPosition;

        static Credentials m_creds;
        static BittrexApi m_api;
        static BittrexClient m_client;

        static TradeBittrexArbs()
        {
            //SetTradeSizes(1.0M);
            //InitializeArbPositions();
        }

        public static void InitializeApi(string encryptedCredentialsFile, string password)
        {
            m_creds = Credentials.LoadEncryptedJson(encryptedCredentialsFile, password);
            var cred = m_creds["BITTREX"];
            m_api = new BittrexApi(cred.Key, cred.Secret);

            BittrexClientOptions options = new BittrexClientOptions();
            options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(cred.Key, cred.Secret);
            m_client = new BittrexClient(options);

            //m_api.Test();
            //PrintAllBalances();
            //Ping(new string[] { "api.binance.com" });
            //Rebalance();

            //m_api.StartUserDataStream();
        }

        public static void Test()
        {
            var balances = m_client.GetBalances();
            foreach (var b in balances.Data)
                Console.WriteLine("{0} {1} {2} {3} {4}", b.Currency, b.Available, b.Pending, b.Balance, b.CryptoAddress);

            string currency = "BTC";
            var btcDeposit = m_client.GetDepositAddress(currency);

            var depositHistory = m_client.GetDepositHistory(null);
            var withdrawHistory = m_client.GetWithdrawalHistory(null);
            /*string address = "";
            decimal amount = 0.0M;
            m_client.Withdraw(currency, amount, address);*/

        }

       


    } // end of class TradeBittrexArbs

} // end of namespace

