using System;
using System.Collections.Generic;
using System.Linq;
using CryptoApis;
using CryptoApis.RestApi;
using ExchangeSharp;
using CryptoTools;
using CryptoTools.Models;
using CryptoTools.MathStat;
using static CryptoTools.Global;

namespace CryptoAnalysis.Charts
{
    public class Renko
    {
		private ExchangeSharpApi m_api;
		private CandlestickMaker m_maker;
		private IList<XCandle> m_hourCandles;        // 1-hour candles
		private IList<XCandle> m_minuteCandles;      // 1-minute candles
		private string m_exchange;
		private string m_symbol;
		private int m_nbars;

		// CTOR:
        // Given an exchange and symbol and number of bars to use in RealizedVolatility calculation
		// Construct the Renko
        public Renko(string exchange, string symbol, int numberOfBars)
        {
            m_api = new ExchangeSharpApi();
            m_api.LoadCredentials(Credentials.CredentialsFile, Credentials.CredentialsPassword);
			m_maker = new CandlestickMaker();
			m_exchange = exchange;
			m_symbol = symbol;
			m_nbars = numberOfBars;
			ConstructRenko();
		}

		// CTOR:
        // Given an exchange and symbol and number of bars to use in RealizedVolatility calculation
		// AND Given ExchangeSharpRestApi object
		// Construct the Renko
		public Renko(string exchange, string symbol, int numberOfBars, ExchangeSharpApi xsapi)
		{
			m_api = xsapi;
			m_maker = new CandlestickMaker();
			m_exchange = exchange;
			m_symbol = symbol;
			m_nbars = numberOfBars;
			ConstructRenko();
		}

        private void ConstructRenko()
		{
			cout("Exchange: {0}     Symbol: {1}     lookback: {2}", m_exchange, m_symbol, m_nbars);

			//m_hourCandles = m_maker.GetCandles(m_exchange, m_symbol, minutes: 60, iterationCount: 1, force: true);
			m_hourCandles = m_maker.ReadCandles(m_exchange, m_symbol, minutes: 60);

			var rvol = new RealizedVolatility(m_hourCandles, m_nbars, numerator: 252 * 24);
            var heights = rvol.RangeStdValues;
            var candleMap = rvol.CandleMap;

            /*var nstddev = 2.0M;                 // two standard deviations
            foreach (var kv in heights)
            {
                var ts = kv.Key;
                var stdValue = kv.Value;
                var candle = candleMap[ts];
                cout("{0}    {1:0.00000000}", ts, 2 * nstddev * stdValue); // stddev goes up and down, so multiply by 2
            }*/

            // Now iterate through the 1-minute bars to construct the Renko boxes
			//m_minuteCandles = m_maker.GetCandles(m_exchange, m_symbol, minutes: 1, iterationCount: 20, force: true);
			m_minuteCandles = m_maker.ReadCandles(m_exchange, m_symbol, minutes: 1);
			//cout("minute candles count: {0}", m_minuteCandles.Count);
            
			//var stdMult = 2.0M;     // multiply stddev * 2 (+/- 2 stddev of price will be used)
			var stdMult = 2.8M;     // multiply stddev * 2 (+/- 2 stddev of price will be used)
			decimal price = decimal.MinValue;
			decimal boxHeight;
			decimal triggerUp = 0, triggerDown = 0;
			DateTime startTime = DateTime.MinValue, endTime = DateTime.MinValue;
			foreach (var mc in m_minuteCandles)
			{
				var hourTimestamp = m_hourCandles.Where(c => c.Timestamp <= mc.Timestamp).Last().Timestamp;
                if (price == decimal.MinValue)
				{
					price = mc.OpenPrice;
					boxHeight = stdMult * heights[hourTimestamp];
					triggerUp = price + boxHeight;
					triggerDown = price - boxHeight;
					startTime = mc.Timestamp;
				}
                
				var h = mc.HighPrice;
				var l = mc.LowPrice;
				if (h > triggerUp || l < triggerDown)
				{
					endTime = mc.Timestamp;
					var startTimeStr = startTime.ToString("MM/dd HH:mm");
					var endTimeStr = endTime.ToString("MM/dd HH:mm");
					if (h > triggerUp && l < triggerDown)
					{
						cout("ERROR! These should not BOTH be true!");
					}
					if (h > triggerUp)
					{
						//cout("HIGH minute bar timestamp: {0}    hour bar timestamp: {1}     {2:0.00000000}  {3:0.00000000}   {4} to {5}", mc.Timestamp, hourTimestamp, h, triggerUp, startTime, endTime);
						cout("UP   bar     {0:0.00000000} {1:0.00000000} {2:0.00000000}   {3} to {4}     {5,6:0.0} minutes", price, triggerUp, triggerUp-price, startTimeStr, endTimeStr, endTime.Subtract(startTime).TotalMinutes);
						price = triggerUp;
					}
					else if (l < triggerDown)
					{
						//cout("LOW  minute bar timestamp: {0}    hour bar timestamp: {1}     {2:0.00000000}  {3:0.00000000}   {4} to {5}", mc.Timestamp, hourTimestamp, l, triggerDown, startTime, endTime);
						cout("DOWN bar     {0:0.00000000} {1:0.00000000} {2:0.00000000}   {3} to {4}     {5,6:0.0} minutes", price, triggerDown, price-triggerDown, startTimeStr, endTimeStr, endTime.Subtract(startTime).TotalMinutes);
						price = triggerDown;
					}
                    boxHeight = stdMult * heights[hourTimestamp];
                    triggerUp = price + boxHeight;
                    triggerDown = price - boxHeight;
                    startTime = mc.Timestamp;
				}
			}
		}

        
    } // end of class Renko

} // end of namespace
