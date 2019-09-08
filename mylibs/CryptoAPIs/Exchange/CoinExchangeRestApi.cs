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
    // http://coinexchangeio.github.io/slate/

    public class CoinExchangeRestApi : RestApiBase
    {
        private const string URL = "https://www.coinexchange.io";

        private CoinExchangeMarketList m_markets;
        private Dictionary<string, CoinExchangeMarket> m_marketMap;

        public CoinExchangeRestApi(string apiKey, string apiSecret) : base(URL, apiKey, apiSecret)
        {
        }

        public void Test()
        {
            // API test code goes here
        }

        public CoinExchangeMarketList GetSymbols()
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
        }

        #region ---------- ICryptoApi ---------------------------------------------------------------
        public override string Exchange { get { return "COINEXCHANGE"; } }

        public override List<string> GetAllSymbols()
        {
            var result = new List<string>();
            var symbols = GetSymbols();
            foreach (var s in symbols.result)
            {
                result.Add(s.MarketAssetCode);
            }
            return result;
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
            string symbol = GetSymbol(symbolId);
            var s = m_marketMap[symbol];
            var ticker = GetOneTicker(s.MarketID);
            return new XTicker(ticker);
        }

        public override async Task<XBalanceMap> GetBalances()
        {
            //var res = await m_client.GetAccountInfoAsync();
            //return new XBalanceMap(res.Data);
            return null;
        }
        #endregion ----------------------------------------------------------------------------------

    } // end of class CoinExchangeRestApi
} // end of namespace

