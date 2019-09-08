using System;

namespace CryptoTools.MathStat
{
	public class iEMA : IIndicator
    {
        private int tickcount;
        private int lastTickcount;
        private int periods;
        private decimal dampen;
		private decimal emav;
		private decimal lastEmav;
        
		public bool IsPrimed => (tickcount > periods);

        public iEMA(int pPeriods)
        {
            periods = pPeriods;
			dampen = 2 / ((decimal)1.0 + periods);
        }
        
		public void ReceiveTick(decimal Val)
        {
            lastEmav = emav;
            lastTickcount = tickcount;

            if (tickcount < periods)
                emav += Val;
            if (tickcount == periods)
                emav /= periods;
            if (tickcount > periods)
                emav = (dampen * (Val - emav)) + emav;

            if (tickcount <= (periods + 1))
            {
                // avoid overflow by stopping use of tickcount
                // when indicator is fully primed
                tickcount++;
            }
        }
              
        // Rather than adding a new tick, update the value of the last tick
		public void UpdateLastTick(decimal Val)
        {
            emav = lastEmav;
            tickcount = lastTickcount;
            ReceiveTick(Val);
        }
        
		public decimal Value => (decimal)DecimalValue();
        
		private decimal DecimalValue()
        {
            decimal v;

			if (IsPrimed)
                v = emav;
            else
                v = 0;

            return v;
        }

        /*public bool isPrimed()
        {
            bool v = false;
            if (tickcount > periods)
            {
                v = true;
            }
            return v;
        }*/
    } // end of class iEMA
    
} // end of namespace
