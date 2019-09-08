using System;
using System.Collections.Generic;
using System.Linq;
using Binance.Net;
using Binance.Net.Objects;
using CryptoTools;

namespace VwapCoin
{
    public class Binance
    {
		private Credentials m_creds;

		public Binance(Credentials creds)
		{
			m_creds = creds;
		}

		public BinanceClient GetBinanceClient()
        {
            var exchange = "BINANCE";
            var apiKey = m_creds[exchange].Key;
            var apiSecret = m_creds[exchange].Secret;
            var options = new BinanceClientOptions();
            options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(apiKey, apiSecret);
            var client = new BinanceClient(options);
            return client;
        }

        // BINANCE
        // where symbol like "BTCUSDT"
        // where settleTime is in UniversalTime (use .ToUniversalTime method)
        // where minutes is length of "window" from which to pull VWAP trade price/quantity
        public void BinanceVwap(string symbol, DateTime settleTime, int minutes, bool displayTrades = false)
        {
            var tlist = new List<BinanceRecentTrade>();

            var client = GetBinanceClient();
            var startTime = settleTime.Subtract(TimeSpan.FromMinutes(minutes));

            var xt = client.GetRecentTrades(symbol);
            if (xt.Error != null)
            {
                Console.WriteLine("{0} {1}", xt.Error.Code, xt.Error.Message);
            }
            var trades = xt.Data;
            //trades.ToList().ForEach(t => PrintTrade(t));
            tlist.AddRange(trades);

            int limit = 500;
            while (tlist[0].Time > startTime)
            {
                Console.WriteLine("Getting more trade data: {0} vs {1}", tlist[0].Time, startTime);
                long fromId = tlist[0].Id - limit;
                xt = client.GetHistoricalTrades(symbol, limit, fromId);
                if (xt.Error != null)
                {
                    Console.WriteLine("{0} {1}", xt.Error.Code, xt.Error.Message);
                }
                trades = xt.Data;
                tlist.InsertRange(0, trades);
            }

            tlist.RemoveAll(t => t.Time < startTime);
            Console.WriteLine();
            if (displayTrades) tlist.ForEach(t => PrintTrade(t));
            int tradeCount = tlist.Count;
            Console.WriteLine("{0} trades", tradeCount);
            var sumPQ = tlist.Sum(t => t.Price * t.Quantity);
            var sumQ = tlist.Sum(t => t.Quantity);
            var vwap = sumPQ / sumQ;
            Console.WriteLine("({0} {1}) VWAP = {2:0.00000000}", "BINANCE", symbol, vwap);
        }

		public static void PrintTrade(BinanceRecentTrade t)
        {
            Console.WriteLine("{0} id:{1} price:{2} qty:{3} match:{4} maker:{5}", t.Time.ToDisplay(), t.Id, t.Price, t.Quantity, t.IsBestMatch, t.IsBuyerMaker);
        }

    }
} // end of namespace
