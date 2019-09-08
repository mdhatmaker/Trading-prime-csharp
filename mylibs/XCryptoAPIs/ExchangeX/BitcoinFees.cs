using System;
using System.Collections.Generic;
using Tools;
using static Tools.G;

namespace CryptoAPIs.ExchangeX
{
    // http://bitcoinfees.21.co/api
    public static class BitcoinFees
    {
        public static string BaseUrl = "https://bitcoinfees.21.co";

        public static void Test()
        {
            cout("BitcoinFees");

            var fees = GetTransactionFeesSummary();
            cout(fees);
        }

        // Returns a list of Fee objects that contain predictions about fees in the given range from minFee to maxFee in satoshis/byte.
        public static Dictionary<string, BitcoinFee> GetTransactionFeesSummary()
        {
            var summary = GET<Dictionary<string, BitcoinFee>>(@"https://bitcoinfees.21.co/api/v1/fees/list");
            cout(summary);
            return summary;
        }

        public static BitcoinRecommendedFee GetRecommendedTransactionFees()
        {
            return GET<BitcoinRecommendedFee>(@"https://bitcoinfees.21.co/api/v1/recommended");
        }

    } // end of class

    //------------------------------------------------------------------------------------------------------------------------

    /*
     * Error codes
     * Status 503: Service unavailable(please wait while predictions are being generated)
     * Status 429: Too many requests(API rate limit has been reached)
     */


    /*
     * fastestFee: The lowest fee (in satoshis per byte) that will currently result in the fastest transaction confirmations (usually 0 to 1 block delay).
     * halfHourFee: The lowest fee (in satoshis per byte) that will confirm transactions within half an hour (with 90% probability).
     * hourFee: The lowest fee (in satoshis per byte) that will confirm transactions within an hour (with 90% probability).
     */
    public class BitcoinRecommendedFee
    {
        public int fastestFee { get; set; }
        public int halfHourFee { get; set; }
        public int hourFee { get; set; }
    }

    /*
     * The Fee objects have the following properties(aside from the minFee-maxFee range they refer to):
     * dayCount: Number of confirmed transactions with this fee in the last 24 hours.
     * memCount: Number of unconfirmed transactions with this fee.
     * minDelay: Estimated minimum delay (in blocks) until transaction is confirmed (90% confidence interval).
     * maxDelay: Estimated maximum delay (in blocks) until transaction is confirmed (90% confidence interval).
     * minMinutes: Estimated minimum time (in minutes) until transaction is confirmed (90% confidence interval).
     * maxMinutes: Estimated maximum time (in minutes) until transaction is confirmed (90% confidence interval).
     */
    public class BitcoinFee
    {
        public int minFee { get; set; }
        public int maxFee { get; set; }
        public int dayCount { get; set; }
        public int memCount { get; set; }
        public int minDelay { get; set; }
        public int maxDelay { get; set; }
        public int minMinutes { get; set; }
        public int maxMinutes { get; set; }
    } // end of class BitcoinFee

   

} // end of namespace 
