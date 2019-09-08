using System;
using System.Collections.Generic;
using CryptoTools.Models;

namespace CryptoTools.MathStat
{
	public class IndicatorResults<T> : IEnumerable<KeyValuePair<XCandle, T>>, IComparer<XCandle>
	{
		private SortedDictionary<XCandle, T> m_results;

		public IndicatorResults()
		{
			m_results = new SortedDictionary<XCandle, T>(this);
		}

        public void Add(XCandle candle, T res)
        {
			/*if (m_results.ContainsKey(candle))
			{
				var x = m_results[candle];
				Console.WriteLine(candle.Timestamp);
				return;
			}*/
			m_results[candle] = res;
            //m_results.Add(candle, res);
        }

		public int Compare(XCandle x, XCandle y)
		{
			if (x.Timestamp < y.Timestamp)
				return -1;
			else if (x.Timestamp > y.Timestamp)
				return +1;
			else
				return 0;
		}

		// IEnumerable Members
		public IEnumerator<KeyValuePair<XCandle, T>> GetEnumerator()
        {
			foreach (var kv in m_results)   //  T o in m_results)
            {
                /*// Lets check for end of list (its bad code since we used arrays)
                if (kv.Key == null)
                {
                    break;
                }*/

                // Return the current element and then on next function call 
                // resume from next element rather than starting all over again;
                yield return kv;
            }
        }

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            // Lets call the generic version here
            return this.GetEnumerator();
        }

    } // end of class IndicatorResults
} // end of namespace
