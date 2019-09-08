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
using CryptoApis.RestApi;

namespace WebSocketTest
{
    // ---- QUANTITIES ----
    // NEO/USDT qty=0.20
    // BNB/USDT qty=1.00
    // BCC/USDT qty=0.014
    // QTUM/USDT qty=0.75
    // LTC/USDT qty = 0.09
    // ADA/USDT qty = 50.0
    
    public class BittrexArbs
    {
        // https://support.bittrex.com/hc/en-us/articles/115003723911

        static Dictionary<string, Ticker> m_tickers = new Dictionary<string, Ticker>();
        static Dictionary<string, decimal> m_lastBuy = new Dictionary<string, decimal>();
        static Dictionary<string, decimal> m_lastSell = new Dictionary<string, decimal>();

        //static BinanceExchangeInfo m_exchangeInfo;
        //static Dictionary<string, BinanceSymbolInfo> m_symbolInfo = new Dictionary<string, BinanceSymbolInfo>();

        static Dictionary<string, decimal> m_tradeQty = new Dictionary<string, decimal>();
        static Dictionary<string, int> m_position = new Dictionary<string, int>();

        static int m_minPosition, m_maxPosition;

        static BittrexRestApi m_api;

        static BittrexArbs()
        {
            SetTradeSizes(1.0M);
            InitializeArbPositions();
        }

        public static void InitializeApi()
        {
            m_api = new BittrexRestApi("1321c848f08b4ba9826770980f0dbd36", "9c4231e8ef1d42b399c3e2a3286f9671");

            //m_api.Test();
            //PrintAllBalances();
            //Ping(new string[] { "api.binance.com" });
            //Rebalance();

            //m_api.StartUserDataStream();

            //var balances = m_api.GetBalances();
            //var markets = m_api.GetMarkets();
            //Console.WriteLine("got markets");
        }

        // For each symbol (currency pair), set the trade qty to be used in our ARBS
        // where multiplier like 1.0M that will adjust the trade quantity for each symbol (currency pair)
        private static void SetTradeSizes(decimal multiplier = 1.0M)
        {
            m_tradeQty["neousdt"] = 0.20M;
            m_tradeQty["bnbusdt"] = 1.00M;
            m_tradeQty["bccusdt"] = 0.020M;
            m_tradeQty["qtumusdt"] = 0.75M;
            m_tradeQty["ltcusdt"] = 0.09M;
            m_tradeQty["adausdt"] = 50.0M;

            foreach (var k in m_tradeQty.Keys.ToList())
            {
                m_tradeQty[k] = m_tradeQty[k] * multiplier;
            }
        }

        // For each arb (i.e. "neobtc", "neoeth", "qtumbtc", etc.), set the position to zero
        private static void InitializeArbPositions()
        {
            m_position["neobtc"] = 0;
            m_position["bnbbtc"] = 0;
            m_position["bccbtc"] = 0;
            m_position["qtumbtc"] = 0;
            m_position["ltcbtc"] = 0;
            m_position["adabtc"] = 0;

            m_position["neoeth"] = 0;
            m_position["bnbeth"] = 0;
            m_position["bcceth"] = 0;
            m_position["qtumeth"] = 0;
            m_position["ltceth"] = 0;
            m_position["adaeth"] = 0;

            // Set minimum and maximum allowable positions for the arbs
            m_minPosition = -3;
            m_maxPosition = +3;
        }



    } // end of class BittrexArbs

} // end of namespace

