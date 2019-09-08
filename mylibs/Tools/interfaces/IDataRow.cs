using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public interface IDataRow
    {
        string[] GetCells();
        string Key { get; set; }

    } // end of INTERFACE

} // end of NAMESPACE
