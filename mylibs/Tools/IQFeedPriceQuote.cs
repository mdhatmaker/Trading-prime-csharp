using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQFeed
{

    public class IQFeedPriceQuote
    {
        public string Symbol { get; set; }
        public double Last { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volatility { get; set; }
        public TimeSpan LastTradeTime { get; set; }
        public int Delay { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }

        public IQFeedPriceQuote()
        {
        }

        public IQFeedPriceQuote(string quoteMessage)
        {
            Parse(quoteMessage);
        }

        internal void Parse(string quoteMessage)
        {
            string[] data = quoteMessage.Split(',');

            // http://www.iqfeed.net/dev/api/betaDocs/UpdateSummaryMessageFormat.cfm
            try
            {
                Symbol = data[1];
                Last = SafeParseDouble(data[3]);
                High = SafeParseDouble(data[8]);
                Low = SafeParseDouble(data[9]);

                if (data.Length > 10)
                    Bid = SafeParseDouble(data[10]);
                else
                    Bid = -1;

                if (data.Length > 11)
                    Ask = SafeParseDouble(data[11]);
                else
                    Bid = -1;

                if (data.Length > 24)
                    Delay = Convert.ToInt32(SafeParseDouble(data[24]));
                else
                    Delay = -1;

                if (data.Length > 44)
                    Volatility = SafeParseDouble(data[44]);
                else
                    Volatility = -1;

                if (data.Length > 19)
                    Open = SafeParseDouble(data[19]);
                else
                    Open = -1;

                if (data.Length > 20)
                    Close = SafeParseDouble(data[20]);
                else
                    Close = -1;

                if (data.Length > 17)
                    LastTradeTime = TimeSpan.Parse(data[17].TrimEnd('a', 'b', 't', 'T', 'o'));
                else
                    LastTradeTime = DateTime.Now.TimeOfDay;//TimeSpan.Zero;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to parse quote message: " + quoteMessage, ex);
            }
        }

        private double SafeParseDouble(string numeric)
        {
            if (string.IsNullOrEmpty(numeric))
                return 0;
            else
                return Double.Parse(numeric);
        }
    }
}