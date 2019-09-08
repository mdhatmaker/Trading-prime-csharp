using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MathNet.Numerics;

namespace Tools
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
        public static double Std(double[] someDoubles)
        {
            double average = someDoubles.Average();
            double sumOfSquaresOfDifferences = someDoubles.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / someDoubles.Length);
            return sd;
        }

        // Given an array of doubles
        // Return the STANDARD DEVIATION of the values
        public static double Std(List<double> someDoubles)
        {
            double average = someDoubles.Average();
            double sumOfSquaresOfDifferences = someDoubles.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / someDoubles.Count);
            return sd;
        }
    } // end of class GMath


    public class iEMA
    {
        private int tickcount;
        private int lastTickcount;
        private int periods;
        private double dampen;
        private double emav;
        private double lastEmav;

        public iEMA(int pPeriods)
        {
            periods = pPeriods;
            dampen = 2 / ((double)1.0 + periods);
        }

        public void ReceiveTick(double Val)
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
        public void UpdateLastTick(double Val)
        {
            emav = lastEmav;
            tickcount = lastTickcount;
            ReceiveTick(Val);
        }

        public double Value()
        {
            double v;

            if (isPrimed())
                v = emav;
            else
                v = 0;

            return v;
        }

        public bool isPrimed()
        {
            bool v = false;
            if (tickcount > periods)
            {
                v = true;
            }
            return v;
        }
    }

    public class iMACD
    {
        int pSlowEMA, pFastEMA, pSignalEMA;
        iEMA slowEMA, fastEMA, signalEMA;

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

        public void ReceiveTick(double Val)
        {
            slowEMA.ReceiveTick(Val);
            fastEMA.ReceiveTick(Val);

            if (slowEMA.isPrimed() && fastEMA.isPrimed())
            {
                signalEMA.ReceiveTick(fastEMA.Value() - slowEMA.Value());
            }
        }

        // Rather than adding a new tick, update the value of the last tick
        public void UpdateLastTick(double Val)
        {
            slowEMA.UpdateLastTick(Val);
            fastEMA.UpdateLastTick(Val);

            if (slowEMA.isPrimed() && fastEMA.isPrimed())
            {
                signalEMA.UpdateLastTick(fastEMA.Value() - slowEMA.Value());
            }
        }

        public void Value(out double MACD, out double signal, out double hist)
        {
            if (signalEMA.isPrimed())
            {
                MACD = fastEMA.Value() - slowEMA.Value();
                signal = signalEMA.Value();
                hist = MACD - signal;
            }
            else
            {
                MACD = 0;
                signal = 0;
                hist = 0;
            }
        }

        public bool isPrimed()
        {
            bool v = false;
            if (signalEMA.isPrimed())
            {
                v = true;
            }
            return v;
        }
    }
} // end of namespace
