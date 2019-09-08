using System;
using System.Collections.Generic;
using System.Linq;
using Bittrex.Net;
using Bittrex.Net.Objects;
using CryptoTools;

namespace VwapCoin
{
    public class Bittrex
    {
		private Credentials m_creds;

		public Bittrex(Credentials creds)
		{
			m_creds = creds;
		}

		public BittrexClient GetBittrexClient()
        {
            var exchange = "BITTREX";
            var apiKey = m_creds[exchange].Key;
            var apiSecret = m_creds[exchange].Secret;
            var options = new BittrexClientOptions();
            options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(apiKey, apiSecret);
            var client = new BittrexClient(options);
            return client;
        }
        
		public void BittrexSymbols()
        {
            var client = GetBittrexClient();
            var res = client.GetMarketSummaries();
            res.Data.ToList().ForEach(m => Console.WriteLine("{0}", m.MarketName));
        }

        // BITTREX
        // where symbol like "USDT-BTC"
        // where settleTime is in UniversalTime (use .ToUniversalTime method)
        // where minutes is length of "window" from which to pull VWAP trade price/quantity
		public void BittrexVwap(string symbol, DateTime settleTime, int minutes)
        {
			//var tlist = new List<BinanceRecentTrade>();
			var tlist = new List<string>();

            var client = GetBittrexClient();
            var startTime = settleTime.Subtract(TimeSpan.FromMinutes(minutes));

            var res = client.GetMarketHistory(symbol);
            res.Data.ToList().ForEach(m => PrintTrade(m));

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

		public void PrintTrade(BittrexMarketHistory m)
        {
            Console.WriteLine("{0} id:{1} price:{2} qty:{3} otype:{4} ftype:{5} total:{6}", m.Timestamp.ToDisplay(), m.Id, m.Price, m.Quantity, m.OrderType, m.FillType, m.Total);
        }

    }
} // end of namespace
