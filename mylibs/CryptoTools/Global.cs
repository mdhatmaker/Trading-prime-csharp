using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryptoTools
{
    public enum OrderSide { Buy, Sell };
    public enum OrderResult { Canceled, Error, Filled, FilledPartially, Pending, PendingCancel, Unknown };

    public static class Global
    {
        private static StreamWriter m_coutWriter;

        private static DateTime m_unixTimestampZero = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
		public static void cout(string output, params object[] p)
		{
			if (p.Length != 0)
			{
				output = string.Format(output, p);
			}

			if (m_coutWriter != null)
			{
				m_coutWriter.WriteLine(output);
			}
           
			Console.WriteLine(output);
		}

        public static void OpenCoutFile(string pathname)
		{
			m_coutWriter = new StreamWriter(pathname);
		}

        public static void CloseCoutFile()
		{
			m_coutWriter.Flush();
			m_coutWriter.Close();
			m_coutWriter.Dispose();
			m_coutWriter = null;
		}

        public static string GetByteString(byte[] array)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                sb.AppendFormat("{0:x2}", array[i]);
            }
            return sb.ToString();
        }

        public static int ToTimestampSeconds(DateTime dt)
        {
            return (int)(dt.ToUniversalTime() - m_unixTimestampZero).TotalSeconds;
        }

        public static long ToTimestampMilliseconds(DateTime dt)
        {
            return (long)(dt.ToUniversalTime() - m_unixTimestampZero).TotalMilliseconds;
        }

        // TODO: Convert from UTC to local DateTime
        public static DateTime FromTimestampSeconds(int unixtimeSeconds)
        {
            return m_unixTimestampZero.AddSeconds(unixtimeSeconds).ToLocalTime();
        }

        public static DateTime FromTimestampMilliseconds(long unixtimeMilliseconds)
        {
            return m_unixTimestampZero.AddMilliseconds(unixtimeMilliseconds).ToLocalTime();
        }


        // Print the byte array in a readable format.
        public static void PrintByteArray(byte[] array)
        {
            int i;
            for (i = 0; i < array.Length; i++)
            {
                Console.Write(String.Format("{0:X2}", array[i]));
                if ((i % 4) == 3) Console.Write(" ");
            }
            Console.WriteLine();
        }

        // Print the byte array in a readable format.
        public static void PrintByteArrayPretty(byte[] array)
        {
            int i;
            for (i = 0; i < array.Length; i++)
            {
                Console.Write(String.Format("{0:X2}", array[i]));
                if ((i % 4) == 3) Console.Write(" ");
            }
            Console.WriteLine();
        }

        // Determine whether or not the specified decimal represents an integer number
        public static bool IsInteger(decimal d)
        {
            return ((d % 1) == 0);
        }

        // Given a number of seconds (int)
        // Return the bar period (string) that describes the bar period ("1m", "3m", "1h", "4h", "1d", ...")
        public static string GetBarPeriod(int seconds)
		{
			int secondsPerDay = 60 * 60 * 24;
			int secondsPerHour = 60 * 60;
			int secondsPerMinute = 60;

			if (seconds % secondsPerDay == 0)
				return string.Format("{0}d", seconds / secondsPerDay);
			else if (seconds % secondsPerHour == 0)
				return string.Format("{0}h", seconds / secondsPerHour);
			else if (seconds % secondsPerMinute == 0)
				return string.Format("{0}m", seconds / secondsPerMinute);
			else
				return string.Format("{0}", seconds);
		}



        //----- EXTENSION METHODS ---------------------------------------------------------------------
        public static string ToDisplay(this DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd HH:mm:ss");
		}

		// Ex: collection.TakeLast(5);
        /*public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }*/

        // Return the standard deviation of an array of decimals.
        //
        // If the second argument is True, evaluate as a sample (you have a subset of the values for a population and want to deduce something about the population as a whole)
        // If the second argument is False, evaluate as a population (you have complete values for every member of group)
        public static decimal StdDev(this IEnumerable<decimal> values, bool as_sample = true)
        {
            // Get the mean.
            decimal mean = values.Sum() / values.Count();

            // Get the sum of the squares of the differences
            // between the values and the mean.
            var squares_query =
                from decimal value in values
                select (value - mean) * (value - mean);
            decimal sum_of_squares = squares_query.Sum();

            if (as_sample)
            {
                decimal d = (sum_of_squares / (values.Count() - 1));
                return (decimal)Math.Sqrt((double)d);
            }
            else
            {
                decimal d = sum_of_squares / values.Count();
                return (decimal)Math.Sqrt((double)d);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
        //---------------------------------------------------------------------------------------------

    } // end of class
} // end of namespace
