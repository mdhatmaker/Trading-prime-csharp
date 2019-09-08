using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    
    public abstract class ZKline
    {
        public abstract long Timestamp { get; }
        public abstract decimal Open { get; }
        public abstract decimal High { get; }
        public abstract decimal Low { get; }
        public abstract decimal Close { get; }
        public abstract decimal Volume { get; }


        public ZKline()
        {
        }

        public override string ToString()
        {
            return string.Format("[{0}],o:{1},h:{2},l:{3},c:{4},volume:{5}", Timestamp, Open, High, Low, Close, Volume);
        }
    } // end of abstract class ZKline


} // end of namespace
