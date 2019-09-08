using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class BarUpdate
    {
        public string Symbol { get; protected set; }
        public DateTime BarTime { get; protected set; }
        public double Open { get; protected set; }
        public double High { get; protected set; }
        public double Low { get; protected set; }
        public double Close { get; protected set; }
        public int TotalVolume { get; protected set; }
        public int BarVolume { get; protected set; }

        //public static BarUpdate Empty { get { return } }

        public override string ToString()
        {
            return string.Format("{0} {1}   o:{2}  h:{3}  l:{4}  c:{5}   v:{6} vbar:{7}", Symbol, BarTime, Open, High, Low, Close, TotalVolume, BarVolume);
        }

    } // end of class
} // end of namespace