using System;

namespace CryptoTools.Backtest
{
    public class BacktestRoundTrip
    {
        public decimal Days => (decimal)Math.Round(Exit.Date.Subtract(Entry.Date).TotalDays, 1);
		public int Minutes => (int) Exit.Date.Subtract(Entry.Date).TotalMinutes;

		public decimal Profit
		{
			get
			{
				if (Entry.Side == OrderSide.Buy && Exit.Side == OrderSide.Sell)
				{
					return Exit.Price - Entry.Price;
				}
				else if (Entry.Side == OrderSide.Sell && Exit.Side == OrderSide.Buy)
				{
					return Entry.Price - Exit.Price;
				}
				else
				{
					throw new ArgumentException(string.Format("Entry/Exit orders must be opposite OrderSide. Given Entry:{0} Exit:{1}", Entry.Side, Exit.Side));
				}
			}
		}

		public decimal MaxDrawdown => 0.0M;

        public BacktestTrade Entry { get; set; }
        public BacktestTrade Exit { get; set; }

        public decimal ProfitPer(int minutes)
		{
			return (decimal) Math.Round(Minutes / (double) minutes, 2);
		}

		public override string ToString()
		{
			return string.Format("{0} Days:{1} Profit:{2} ProfitPerDay:{3} MaxDrawdown:{4}", Entry.Date, Days, Profit, ProfitPer(24 * 60), MaxDrawdown);
		}
	} // end of class BacktestRoundTrip
 
} // end of namespace
