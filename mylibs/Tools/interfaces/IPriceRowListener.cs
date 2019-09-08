using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public interface IPriceRowListener
    {
        void UpdatePrices(Dictionary<string, IDataRow> updateDict);
        void UpdatePrices(List<IDataRow> updateList);
    }
} // end of namespace
