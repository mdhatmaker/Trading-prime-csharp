using System;

namespace CryptoTools.Models
{
    public class XCandle
    {
		public DateTime Timestamp { get; set; }
		public string ExchangeName { get; set; }
		public string Name { get; set; }
		public int PeriodSeconds { get; set; }
		public decimal OpenPrice { get; set; }
		public decimal HighPrice { get; set; }
		public decimal LowPrice { get; set; }
		public decimal ClosePrice { get; set; }
		public decimal BaseVolume { get; set; }
		public decimal ConvertedVolume { get; set; }
		public decimal WeightedAverage { get; set; }

        public XCandle()
        { }

        public XCandle(string exchangeName, string name, DateTime timestamp, int periodSeconds, decimal open, decimal high, decimal low, decimal close, decimal baseVolume, decimal convertedVolume, decimal weightedAverage)
        {
			this.ExchangeName = exchangeName;
			this.Name = name;
			this.Timestamp = timestamp;
			this.PeriodSeconds = periodSeconds;
			this.OpenPrice = open;
			this.HighPrice = high;
			this.LowPrice = low;
			this.ClosePrice = close;
			this.BaseVolume = baseVolume;
			this.ConvertedVolume = convertedVolume;
			this.WeightedAverage = weightedAverage;
        }


    } // end of class XMarketCandle

} // end of namespace
