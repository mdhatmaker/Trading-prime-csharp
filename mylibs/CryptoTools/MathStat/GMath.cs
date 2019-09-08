using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;
using CryptoTools.Models;

namespace CryptoTools.MathStat
{
    public static class GMath
    {
        //MathNet.Numerics.LinearRegression.DirectRegressionMethod
        //MathNet.Numerics.

        public static decimal Contango(decimal m0, decimal m1)
        {
            decimal diff = m1 - m0;
            decimal contango = Math.Round(diff / m0 * 100, 2);
            return contango;
        }

        // Given an array of doubles
        // Return the STANDARD DEVIATION of the values
        public static double StdDev(double[] someDoubles)
        {
            double average = someDoubles.Average();
            double sumOfSquaresOfDifferences = someDoubles.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / someDoubles.Length);
            return sd;
        }

        // Given an array of doubles
        // Return the STANDARD DEVIATION of the values
        public static double StdDev(List<double> someDoubles)
        {
            double average = someDoubles.Average();
            double sumOfSquaresOfDifferences = someDoubles.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / someDoubles.Count);
            return sd;
        }

        // Given an IEnumerable of doubles
        // Return the STANDARD DEVIATION of the values
        public static double StdDev(IEnumerable<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {                
                double avg = values.Average();                      // Compute the Average      
                double sum = values.Sum(d => Math.Pow(d - avg, 2)); // Perform the Sum of (value-avg)_2_2                
                ret = Math.Sqrt((sum) / (values.Count() - 1));      // Put it all together      
            }
            return ret;
        }

        // Given two IEnumerables of doubles
        // Return the CORRELATION of the two collections
        public static double Correl(IEnumerable<double> dataA, IEnumerable<double> dataB)
        {
            double ret = Correlation.Pearson(dataA, dataB);
            return ret;
        }

        // Given two IEnumerables of MarketCandle objects
        // Return the BETA of the two collections (beta of A vs B)
        // if convertPricesA is true, the open/close price of A are multiplied by open/close price of B to calculate returns
        // (i.e. if candlesB is BTCUSD and candlesA is ZRXBTC, set convertPricesA to true to get ZRXUSD vs BTCUSD)
        public static double Beta(List<XCandle> candlesA, List<XCandle> candlesB, bool convertPricesA = false)
        {
            if (candlesA.Count == 0 || candlesB.Count == 0) return 0;

            double ret = 0;
            var dtA1 = candlesA.First().Timestamp;
            var dtB1 = candlesB.First().Timestamp;
            var dtA2 = candlesA.Last().Timestamp;
            var dtB2 = candlesB.Last().Timestamp;
            var dt1 = DateTime.Compare(dtA1, dtB1) > 0 ? dtA1 : dtB1;   // Max of start dates
            var dt2 = DateTime.Compare(dtA2, dtB2) < 0 ? dtA2 : dtB2;   // Min of end dates
            var cA = candlesA.Where(x => x.Timestamp >= dt1 && x.Timestamp <= dt2);
            var cB = candlesB.Where(x => x.Timestamp >= dt1 && x.Timestamp <= dt2);
            if (cA.Count() != cB.Count())
            {
                Console.WriteLine("ERROR: Candle counts must equal (n{0}={1}  n{2}={3})", cA.First().Name, cA.Count(), cB.First().Name, cB.Count());
                return 0;
            }
            double stdA, stdB, cor;
            if (convertPricesA)
            {
                var cX = cA.Zip(cB, (x, y) => (x.OpenPrice * y.OpenPrice, x.ClosePrice * y.ClosePrice));
                var returnsA = cX.Select(x => (double)((x.Item2 - x.Item1) / x.Item1));
                var returnsB = cB.Select(x => (double)((x.ClosePrice - x.OpenPrice) / x.OpenPrice));
                stdA = StdDev(returnsA);
                stdB = StdDev(returnsB);
                cor = Correl(returnsA, returnsB);
            }
            else
            {
                var returnsA = cA.Select(x => (double)((x.ClosePrice - x.OpenPrice) / x.OpenPrice));
                var returnsB = cB.Select(x => (double)((x.ClosePrice - x.OpenPrice) / x.OpenPrice));
                stdA = StdDev(returnsA);
                stdB = StdDev(returnsB);
                cor = Correl(returnsA, returnsB);
            }
            ret = cor * (stdA / stdB);            
            return ret;
        }

        public static IndicatorResults<ResultMACD> GetCandlesMACD(List<XCandle> candles, int periodFastEMA, int periodSlowEMA, int periodSignalEMA)
        {
			var results = new IndicatorResults<ResultMACD>();
            var macd = new iMACD(periodFastEMA, periodSlowEMA, periodSignalEMA);
            foreach (var c in candles)
            {
                //var timeString = c.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                //Console.WriteLine("{0} {1} {2} o:{3} h:{4} l:{5} c:{6} vol:{7} period:{8} wavg:{9}", timeString, c.ExchangeName, c.Name, c.OpenPrice, c.HighPrice, c.LowPrice, c.ClosePrice, c.BaseVolume, c.PeriodSeconds, c.WeightedAverage);
                macd.ReceiveTick(c.ClosePrice);
                if (macd.IsPrimed)
                {
					results.Add(c, macd.ResultValue());
                }
            }
            return results;
        }

		public static IndicatorResults<decimal> GetCandlesEMA(List<XCandle> candles, int periodEMA)
        {
            var results = new IndicatorResults<decimal>();
			var ema = new iEMA(periodEMA);
            foreach (var c in candles)
            {
                //var timeString = c.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                //Console.WriteLine("{0} {1} {2} o:{3} h:{4} l:{5} c:{6} vol:{7} period:{8} wavg:{9}", timeString, c.ExchangeName, c.Name, c.OpenPrice, c.HighPrice, c.LowPrice, c.ClosePrice, c.BaseVolume, c.PeriodSeconds, c.WeightedAverage);
                ema.ReceiveTick(c.ClosePrice);
                if (ema.IsPrimed)
                {
                    results.Add(c, ema.Value);
                }
            }
            return results;
        }

    } // end of class GMath

} // end of namespace
