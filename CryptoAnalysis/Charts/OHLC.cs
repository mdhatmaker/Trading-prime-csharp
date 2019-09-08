using System;
namespace CryptoAnalysis.Charts
{
    public struct OHLC
    {
		public decimal Open { get; private set; }
		public decimal High { get; private set; }
		public decimal Low { get; private set; }
		public decimal Close { get; private set; }

        public OHLC(decimal open, decimal high, decimal low, decimal close)
        {
			Open = open;
			High = high;
			Low = low;
			Close = close;
        }
    } // end of class OHLC

} // end of namespace
