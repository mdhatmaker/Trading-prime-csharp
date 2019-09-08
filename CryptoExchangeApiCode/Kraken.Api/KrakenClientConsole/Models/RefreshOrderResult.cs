using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrakenClientConsole
{

    public class RefreshOrderResult
    {
        public RefreshOrderResultType ResultType { get; set; }

        //Set only if ResultType = error
        public List<string> Errors { get; set; }

        //Set only if ResultType = exception
        public Exception Exception { get; set; }
    }

    public enum RefreshOrderResultType
    {
        error,
        exception,
        order_not_found,
        success,
    }
}
