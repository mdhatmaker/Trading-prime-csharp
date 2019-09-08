using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSumAPI
{
    public class ZPriceLevel
    {
        public int Price { get; private set; }
        public uint Qty { get; private set; }

        public ZPriceLevel(int price, uint qty)
        {
            Price = price;
            Qty = qty;
        }

    } // end of CLASS

    public class ZPriceLevels
    {
        List<ZPriceLevel> levels = new List<ZPriceLevel>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < levels.Count; ++i)
            {
                sb.Append(string.Format("{0}:{1}  ", levels[i].Price, levels[i].Qty));
            }
            return sb.ToString();
        }
    } // end of CLASS

} // end of NAMESPACE
