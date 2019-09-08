using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoTools.Backtest
{
    public class Backtest
    {

		public int Count => m_roundTrips.Count;
		public decimal TotalProfit => m_roundTrips.Sum(r => r.Profit);
		public decimal TotalDays => m_roundTrips.Sum(r => r.Days);

		private string m_name;
		private List<BacktestRoundTrip> m_roundTrips;

        public Backtest(string name = "")
        {
			m_name = name;
			m_roundTrips = new List<BacktestRoundTrip>();
        }

		public void Add(BacktestRoundTrip roundTrip)
		{
			m_roundTrips.Add(roundTrip);
		}

        public void PrintTrades()
		{
			Console.WriteLine("\n--- BACKTEST RESULTS    {0} ---", m_name);
            foreach (var rt in m_roundTrips)
			{
				Console.WriteLine(rt.ToString());
			}
			Console.WriteLine("\n{0} Trades (round-trip)", Count);
			Console.WriteLine("{0} total days (all trades)", TotalDays);
			Console.WriteLine("Total Profit: {0}", TotalProfit);
		}
    } // end of class Backtest

} // end of namespace
