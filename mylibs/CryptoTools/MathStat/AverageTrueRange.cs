using System;
using System.Collections.Generic;
using System.Linq;
using CryptoTools.Models;

namespace CryptoTools.MathStat
{
	// The average true range formula records the maximum values of the following three
	// differences, and calculates the moving average of the resulting data series:
    // 1. Between the previous day's high and low prices.
    // 2. Between the previous day's close price and the current day's high price.
    // 3. Between the previous day's close price and the current day's low price.
    //
    // The average true range indicator is a good measure of commitment.
	// A high value often indicates market bottom due to panic sell.
	// A low value often indicates market top.

    public class AverageTrueRange
    {
		public SortedDictionary<DateTime, double> Values => m_values;

		private SortedDictionary<DateTime, double> m_values;
		private IEnumerable<XCandle> m_candles;
		private int m_atrLength;

        public AverageTrueRange(IEnumerable<XCandle> candles, int atrLength)
        {
			m_values = new SortedDictionary<DateTime, double>();

			m_candles = candles.OrderBy(c => c.Timestamp);
			m_atrLength = atrLength;
            
			var cl = m_candles.ToList();
			for (int i = 0; i < cl.Count - atrLength; ++i)
            {
                var atrCandles = cl.Skip(i).Take(atrLength);
				var atr = CalcATR(atrCandles);
				var lastTime = atrCandles.Last().Timestamp;
				m_values[lastTime] = atr;
            }
        }

		public static double CalcATR(IEnumerable<XCandle> candles)
		{
			var cl = candles.ToList();
			var tr = new double[cl.Count - 1];
			for (int i = 1; i < cl.Count; ++i)
			{
				tr[i - 1] = CalcTrueRange(cl[i - 1], cl[i]);
			}
			return tr.Average();
		}

		public static double CalcTrueRange(XCandle prevDay, XCandle currentDay)
		{
			var d = new double[3];
			d[0] = (double) (prevDay.HighPrice - prevDay.ClosePrice);
			d[1] = Math.Abs((double) (prevDay.ClosePrice - currentDay.HighPrice));
			d[2] = Math.Abs((double) (prevDay.ClosePrice - currentDay.LowPrice));
			return d.Max();
		}

    } // end of class AverageTrueRange
} // end of namespace
