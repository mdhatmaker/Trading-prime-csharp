using KrakenClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrakenClientConsole
{
    public class GetOrderResult
    {
        public GetOrderResultType ResultType { get; set; }

        //Set only if ResultType = error
        public List<string> Errors { get; set; }

        //Set only if ResultType = exception
        public Exception Exception { get; set; }

        public KrakenOrder Order { get; set; }
    }

    public enum GetOrderResultType
    {
        error,
        exception,
        order_not_found,
        success,
    }
}
