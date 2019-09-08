using System;
using System.Collections.Generic;
using System.Linq;
using Bitfinex.Net;
using Bitfinex.Net.Objects;
using CryptoTools;

namespace VwapCoin
{
    public class BitFinex
    {
		private Credentials m_creds;

		public BitFinex(Credentials creds)
		{
			m_creds = creds;
		}

		public BitfinexClient GetBitfinexClient()
        {
            var exchange = "BITFINEX";
            var apiKey = m_creds[exchange].Key;
            var apiSecret = m_creds[exchange].Secret;
            var options = new BitfinexClientOptions();
            options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(apiKey, apiSecret);
            var client = new BitfinexClient(options);
            return client;
        }

        // https://api.bitfinex.com/v1/symbols
		public void BitfinexSymbols()
        {
            var client = GetBitfinexClient();
            //client.
            //var res = client.GetMarketSummaries();
            //res.Data.ToList().ForEach(m => Console.WriteLine("{0}", m.MarketName));
        }

        // BITFINEX
        // where symbol like "btcusd"
        // where settleTime is in UniversalTime (use .ToUniversalTime method)
        // where minutes is length of "window" from which to pull VWAP trade price/quantity
		public void BitfinexVwap(string symbol, DateTime settleTime, int minutes)
        {
            var tlist = new List<BitfinexTradeSimple>();

            var client = GetBitfinexClient();
            var startTime = settleTime.Subtract(TimeSpan.FromMinutes(minutes));

            int? limit = null;
            var endTime = settleTime;
            //var res = client.GetTrades(symbol, limit, startTime.ToLocalTime(), endTime.ToLocalTime(), Sorting.OldFirst);
            //var res = client.GetTrades(symbol);
            //var trades = res.Data.ToList();
            //trades.ForEach(t => PrintTrade(t));

            //var res = client.GetTradeHistory(symbol);
            //var mytrades = res.Data.ToList();
            //mytrades.ForEach(t => PrintTrade(t));

            //var res = client.GetMovements(symbol);
            //var res = client.GetStats(symbol, StatKey.TotalOpenPosition, StatSide.Long, StatSection.History);


            /*while (trades[].Time > startTime)
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
            /*var xt = client.GetRecentTrades(symbol);
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
            Console.WriteLine("\n\n");
            tlist.ForEach(t => PrintTrade(t));
            int tradeCount = tlist.Count;
            Console.WriteLine("\n({0} trades)", tradeCount);
            var sumPQ = tlist.Sum(t => t.Price * t.Quantity);
            var sumQ = tlist.Sum(t => t.Quantity);
            var vwap = sumPQ / sumQ;
            Console.WriteLine("({0} {1}) VWAP = {2:0.00000000}", "BITTREX", symbol, vwap);*/
        }

		public static void PrintTrade(BitfinexTradeSimple t)
        {
            Console.WriteLine("{0} id:{1} price:{2} qty:{3}", t.Timestamp.ToDisplay(), t.Id, t.Price, t.Amount);
        }

        public static void PrintTrade(BitfinexTradeDetails t)
        {
            Console.WriteLine("{0} id:{1} price:{2} qty:{3} maker:{4}", t.TimestampCreated.ToDisplay(), t.Id, t.ExecutedPrice, t.ExecutedAmount, t.Maker);
        }
	
	}
} // namespace
