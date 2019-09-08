using System;

namespace CryptoTools.MathStat
{
	public class iMACD : IIndicator
    {
        int pSlowEMA, pFastEMA, pSignalEMA;
        iEMA slowEMA, fastEMA, signalEMA;
        
		public bool IsPrimed => signalEMA.IsPrimed;

        // restriction: pPFastEMA < pPSlowEMA
        public iMACD(int pPFastEMA, int pPSlowEMA, int pPSignalEMA)
        {
            pFastEMA = pPFastEMA;
            pSlowEMA = pPSlowEMA;
            pSignalEMA = pPSignalEMA;

            slowEMA = new iEMA(pSlowEMA);
            fastEMA = new iEMA(pFastEMA);
            signalEMA = new iEMA(pSignalEMA);
        }
        
		public void ReceiveTick(decimal Val)
        {
            slowEMA.ReceiveTick(Val);
            fastEMA.ReceiveTick(Val);

            if (slowEMA.IsPrimed && fastEMA.IsPrimed)
            {
				signalEMA.ReceiveTick(fastEMA.Value - slowEMA.Value);
            }
        }

        // Rather than adding a new tick, update the value of the last tick
		public void UpdateLastTick(decimal Val)
        {
            slowEMA.UpdateLastTick(Val);
            fastEMA.UpdateLastTick(Val);

			if (slowEMA.IsPrimed && fastEMA.IsPrimed)
            {
				signalEMA.UpdateLastTick(fastEMA.Value - slowEMA.Value);
            }
        }

		public decimal Value => ResultValue().Hist;
		
		public ResultMACD ResultValue()
        {
			decimal MACD, signal, hist;
            if (signalEMA.IsPrimed)
            {
				MACD = (decimal)(fastEMA.Value - slowEMA.Value);
                signal = (decimal)signalEMA.Value;
                hist = (decimal)(MACD - signal);
            }
            else
            {
                MACD = 0;
                signal = 0;
                hist = 0;
            }
			return new ResultMACD(MACD, signal, hist);
        }

    } // end of class iMACD

} // end of namespace
