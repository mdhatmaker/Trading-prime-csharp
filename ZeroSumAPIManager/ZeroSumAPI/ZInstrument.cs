using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSumAPI
{
    public class ZInstrument
    {
        public uint Iid { get; private set; }
        public string Symbol { get; private set; }
        public string Description { get; private set; }

        public ZInstrument(uint iid, string symbol, string description="")
        {
            this.Iid = iid;
            this.Symbol = symbol;
            this.Description = description;
        }

        public override string ToString()
        {
            return string.Format("iid={0} '{1}' \"{5}\"", Iid, Symbol, Description);
        }

    } // end of CLASS

} // end of NAMESPACE
