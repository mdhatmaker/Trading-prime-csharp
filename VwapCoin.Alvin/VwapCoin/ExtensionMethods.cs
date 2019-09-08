using System;

namespace VwapCoin
{
    public static class ExtensionMethods
    {
		// Given a DateTime object
		// Return a string in format like "2018-05-02 18:24:33"
		public static string ToDisplay(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
} // end of namespace
