using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi.Model
{
    public class Orderbook
    {
        public List<KeyValuePair<string, string>> asks { get; set; }
        public List<KeyValuePair<string, string>> bids { get; set; }
    }
}
