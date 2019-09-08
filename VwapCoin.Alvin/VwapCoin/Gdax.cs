using System;
using System.Collections.Generic;
using System.Linq;
using GDAXSharp;
using GDAXSharp.Services.Products.Models;
using CryptoTools;

namespace VwapCoin
{
    public class Gdax
    {
		private Credentials m_creds;

		public Gdax(Credentials creds)
        {
			m_creds = creds;
        }
        
		public GDAXClient GetGdaxClient()
        {
            var exchange = "GDAX";
            var apiKey = m_creds[exchange].Key;
            var apiSecret = m_creds[exchange].Secret;
            var passphrase = m_creds[exchange].Passphrase;
            var auth = new GDAXSharp.Network.Authentication.Authenticator(apiKey, apiSecret, passphrase);
            var client = new GDAXClient(auth);
            return client;
        }

        // GDAX
        // where symbol like "BtcUsd"
        // where settleTime is in UniversalTime (use .ToUniversalTime method)
        // where minutes is length of "window" from which to pull VWAP trade price/quantity
        public void GdaxVwap(string symbol, DateTime settleTime, int minutes)
        {
            var tlist = new List<ProductTrade>();

            var client = GetGdaxClient();
            var startTime = settleTime.Subtract(TimeSpan.FromMinutes(minutes));

            var productId = GDAXSharp.Shared.Types.ProductType.BtcUsd;
            int limit = 100;
            int numberOfPages = 5;
            var res = client.ProductsService.GetTradesAsync(productId, limit, numberOfPages);
            res.Wait();
            foreach (var page in res.Result)
            {
                foreach (var trade in page)
                {
                    PrintTrade(trade);
                }
            }

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

		public static void PrintTrade(ProductTrade t)
        {
            Console.WriteLine("{0} id:{1} price:{2} qty:{3} side:{4}", t.Time.ToDisplay(), t.TradeId, t.Price, t.Size, t.Side);
        }

    }
} // end of namespace
