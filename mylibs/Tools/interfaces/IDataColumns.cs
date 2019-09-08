using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public interface IDataColumns<T>
    {
        Dictionary<T, string> GetColumns();
        Dictionary<T, int> GetKeyColumns();
        string Key { get; }

    } // end of INTERFACE

} // end of NAMESPACE
