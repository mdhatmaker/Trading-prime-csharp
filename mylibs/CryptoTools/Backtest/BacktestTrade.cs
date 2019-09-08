using System;
using CryptoTools.Models;

namespace CryptoTools.Backtest
{
    public class BacktestTrade
    {
        public OrderSide Side { get; set; }
		public decimal Price { get; set; }
        public XCandle Candle { get; set; }

		public DateTime Date => Candle.Timestamp;
        
        public BacktestTrade(OrderSide side, decimal price, XCandle candle)
        {
            Side = side;
			Price = price;
            Candle = candle;
        }
    } // end of class BacktestTrade

} // end of namespace
