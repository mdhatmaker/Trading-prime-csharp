using System;
using System.Collections.Generic;

namespace CryptoTools.MathStat
{
	public class ResultMACD
	{
		public decimal MACD { get; private set; }
		public decimal Signal { get; private set; }
		public decimal Hist { get; private set; }

        public ResultMACD(decimal macd, decimal signal, decimal hist)
		{
			MACD = macd;
			Signal = signal;
			Hist = hist;
		}

		public override string ToString()
		{
			return string.Format("{0:0.0000} {1:0.0000} {2:0.0000}", Hist, MACD, Signal);
		}
	} // end of class ResultMACD


} // end of namespace
