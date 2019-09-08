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
using Binance.Net;
using Binance.Net.Objects;
using CryptoTools.Net;
using static CryptoTools.Global;

namespace CryptoApis.Exchange
{
    // https://info.shapeshift.io

    /* For any of the api calls if there is a problem the error message will be returned as the following JSON:
    {error:"Error message"}

    Many of the requests require a 'coin pair'. A coin pair is of the format deposit_withdrawal.
    Example: 'btc_ltc'. Valid pairs are any combination of the below listed valid coins.
    
    The list will grow as we add more:
    btc, ltc, ppc, drk, doge, nmc, ftc, blk, nxt, btcd, qrk, rdd, nbt, bts, bitusd, xcp, xmr

    If a particular coin goes offline any pairs using it will return a message stating that pair
    is temporarily unavailable.

    All requests are only available via HTTPS, in the interest of security best practices we do not support
    API calls over HTTP. */

    public class ShapeshiftRestApi : RestApiBase
    {
        private const string URL = "https://shapeshift.io";

        //private CoinExchangeMarketList m_markets;
        private List<string> m_coinSymbols;
        private ShapeshiftCoinMap m_coinMap;

        public ShapeshiftRestApi(string apiKey, string apiSecret) : base(URL, apiKey, apiSecret)
        {
        }

        public void Test()
        {
            // API test code goes here
        }

        // Given a currency pair (symbol).
        // Return the current rate offered by Shapeshift.
        // where pair like "btc_ltc", "ltc_btc", ...
        public ShapeshiftRate GetRate(string pair)
        {
            string method = string.Format("rate/{0}", pair);
            var res = Get<ShapeshiftRate>(method);
            return res;
        }

        // Given a currency pair (symbol).
        // Return the current deposit limit set by Shapeshift.
        // where pair like "btc_ltc", "ltc_btc", ...
        // Amounts deposited over this limit will be sent to the return address if one was entered,
        // otherwise the user will need to contact ShapeShift support to retrieve their coins. This
        // is an estimate because a sudden market swing could move the limit.
        public ShapeshiftDepositLimit GetDepositLimit(string pair)
        {
            string method = string.Format("limit/{0}", pair);
            var res = Get<ShapeshiftDepositLimit>(method);
            return res;
        }

        // Given a currency pair (symbol).
        // Return the market info (pair, rate, minimum limit, miner fee).
        // where pair like "btc_ltc", "ltc_btc", ...
        public ShapeshiftMarketInfo GetMarketInfo(string pair)
        {
            string method = string.Format("marketinfo/{0}", pair);
            var res = Get<ShapeshiftMarketInfo>(method);
            return res;
        }



        // Return the list of supported coins (along with info and icon for each).
        public ShapeshiftCoinMap GetCoins()
        {
            if (m_coinMap == null)
            {
                string method = "getcoins";
                m_coinMap = Get<ShapeshiftCoinMap>(method);
            }
            return m_coinMap;
        }

       


        /*public CoinExchangeMarketList GetSymbols()
        {
            if (m_markets != null) return m_markets;
            string method = "api/v1/getmarkets";
            var symbols = Get<CoinExchangeMarketList>(method);
            foreach (var s in symbols.result)
            {
                var symbol = s.MarketAssetCode + "-" + s.BaseCurrencyCode;
                m_marketMap[symbol] = s;
            }
            return symbols;
        }

        public CoinExchangeTickerList GetAllTickers()
        {
            string method = "/api/v1/getmarketsummaries";
            var tickers = Get<CoinExchangeTickerList>(method);
            return tickers;
        }

        public CoinExchangeTicker GetOneTicker(int marketId)
        {
            string method = string.Format("/api/v1/getmarketsummary?market_id={0}", marketId);
            var ticker = Get<CoinExchangeTicker>(method);
            return ticker;
        }

        public CoinExchangeOrderBook GetOrderBook(int marketId)
        {
            string method = string.Format("/api/v1/getorderbook?market_id={0}", marketId);
            var book = Get<CoinExchangeOrderBook>(method);
            return book;
        }

        public CoinExchangeCurrencyList GetCurrencies()
        {
            string method = "/api/v1/getcurrencies";
            var currencies = Get<CoinExchangeCurrencyList>(method);
            return currencies;
        }

        public CoinExchangeCurrencyResult GetCurrency(int currencyId)
        {
            string method = string.Format("/api/v1/getcurrency?currency_id={0}", currencyId);
            var currency = Get<CoinExchangeCurrencyResult>(method);
            return currency;
        }

        // where tickerCode like "BTC"
        public CoinExchangeCurrencyResult GetCurrency(string tickerCode)
        {
            string method = string.Format("/api/v1/getcurrency?ticker_code={0}", tickerCode);
            var currency = Get<CoinExchangeCurrencyResult>(method);
            return currency;
        }*/

        #region ---------- ICryptoApi ---------------------------------------------------------------
        public override string Exchange { get { return "SHAPESHIFT"; } }

        public override List<string> GetAllSymbols()
        {
            if (m_coinSymbols == null)
            {
                m_coinSymbols = new List<string>();
                foreach (var kv in GetCoins())
                {
                    string coinSymbol = kv.Key;
                    m_coinSymbols.Add(coinSymbol);
                }
            }
            return m_coinSymbols;
        }

        public override string GetSymbol(string symbolId)
        {
            if (m_symbolManager == null) m_symbolManager = new CryptoTools.SymbolManager();

            string symbol = m_symbolManager.GetSymbol(Exchange, symbolId);
            if (symbol != null)
                return symbol;
            else
            {
                Console.WriteLine("ERROR: Symbol ID not found.");
                return null;
            }
        }

        public override async Task<XTicker> GetTicker(string symbolId)
        {
            /*string symbol = GetSymbol(symbolId);
            var s = m_marketMap[symbol];
            var ticker = GetOneTicker(s.MarketID);
            return new XTicker(ticker);*/
            return null;
        }

        public override async Task<XBalanceMap> GetBalances()
        {
            //var res = await m_client.GetAccountInfoAsync();
            //return new XBalanceMap(res.Data);
            return null;
        }
        #endregion ----------------------------------------------------------------------------------

    } // end of class ShapeshiftRestApi
} // end of namespace

