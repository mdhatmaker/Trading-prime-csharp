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
using CryptoTools.Cryptography;
using CryptoTools.Net;
using static CryptoTools.Global;
using System.Web;

namespace CryptoApis.Exchange
{
    // https://github.com/Biboxcom/api_reference/wiki/home_en

    public class BiboxRestApi : RestApiBase
    {
        private const string URL = "https://api.bibox.com";

        private CoinExchangeMarketList m_markets;
        private Dictionary<string, CoinExchangeMarket> m_marketMap;

        public BiboxRestApi(string apiKey, string apiSecret) : base(URL, apiKey, apiSecret)
        {
        }

        public async Task Test()
        {
            // API test code goes here
            /*var apikey = "52135958969bedca0809ac10b9caba758022b0a6";
            var secret = "7b58254791ada6c0194e6341953f862aff9a91b5";
            var cmds = @"[{""cmd"":""user/userInfo"",""body"":{}}]";
            //var sign = CryptoJS.HmacMD5(cmds, secret).toString();   //6a21e39e3f68b6fc2227c4074c7e6a6c
            var sign = GetHashValueMD5(cmds, secret);*/

            //var cmds = @"[{""cmd"":""user/userInfo"",""body"":{}}]";
            //var cmds = @"[{""cmd"":""userInfo"",""body"":{}}]";
            var cmds = @"[{cmd:""user/userInfo"",body:{}}]";
            var sign = GetHashValueMD5(cmds, m_apiSecret);
            var sreq = GetRequestString(cmds, m_apiKey, sign);
            var res = await Post<BiboxResult<BiboxResultTrade>>(m_url + "/v1/user", sreq);
            /*foreach (var r in res.result)
            {
                Console.WriteLine("BiboxResultEntry: {0} {1}", r.cmd, r.result);
            }*/

            /*var cmds = @"[{cmd:""pairList"",body:{}}]";
            var sign = GetHashValueMD5(cmds, m_apiSecret);
            var sreq = GetRequestString(cmds, m_apiKey, sign);
            var res = await Post<BiboxResult>(m_url + "/v1/mdata", sreq);*/

            //var bbres = Get<BiboxResult<BiboxResultPair>>("https://api.bibox.com/v1/mdata?cmd=pairList");

            //var bbres = Get<BiboxResult<List<BiboxResultKline>>>("https://api.bibox.com/v1/mdata?cmd=kline&pair=BIX_BTC&period=1min&size=100");
            //pairs，example: BIX_BTC
            //k line period，value ['1min', '3min', '5min', '15min', '30min', '1hour', '2hour', '4hour', '6hour', '12hour', 'day', 'week']
            //how many，1-1000，if not passed will return 1000

            //var bbres = Get<BiboxResult<BiboxResultDepth>>("https://api.bibox.com/v1/mdata?cmd=depth&pair=BIX_BTC&size=50");
            //pairs，example: BIX_BTC
            //size，1-200，if not passed will return 200

            //var bbres = Get<BiboxResult<List<BiboxResultTrade>>>("https://api.bibox.com/v1/mdata?cmd=deals&pair=BIX_BTC&size=50");
            //pairs，example: BIX_BTC
            //size，1-200，if not passed will return 200

            //var bbres = Get<BiboxResult<BiboxResultTicker>>("https://api.bibox.com/v1/mdata?cmd=ticker&pair=BIX_BTC");
            //pairs，example: BIX_BTC


            //var bbres = Get<BiboxResult<List<BiboxResultMarket>>>("https://api.bibox.com/v1/mdata?cmd=marketAll");

            //var bbres = Get<BiboxResult<BiboxResultMarket>>("https://api.bibox.com/v1/mdata?cmd=market&pair=BIX_BTC");
            //pairs，example: BIX_BTC

        }

        private string GetRequestString(string cmds, string apikey, string sign)
        {
            /*var sreq = string.Format(@"{{
                ""cmds"": {0},
                ""apikey"": ""{1}"",
                ""sign"": ""{2}""
            }}", cmds, apikey, sign);*/
            /*var sreq = string.Format(@"{{
                cmds: {0},
                apikey: ""{1}"",
                sign: ""{2}""
            }}", cmds, apikey, sign);*/
            var sreq = string.Format(@"{{cmds:{0},apikey:""{1}"",sign:""{2}""}}", cmds, apikey, sign);
            return sreq;
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
        }
        */

        #region ---------- ICryptoApi ---------------------------------------------------------------
        public override string Exchange { get { return "BIBOX"; } }

        public override List<string> GetAllSymbols()
        {
            var result = new List<string>();
            /*var symbols = GetSymbols();
            foreach (var s in symbols.result)
            {
                result.Add(s.MarketAssetCode);
            }*/
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


    } // end of class BiboxRestApi
} // end of namespace

