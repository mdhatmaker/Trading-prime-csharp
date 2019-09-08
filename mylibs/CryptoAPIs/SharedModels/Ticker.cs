using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoApis.SharedModels
{
    public struct Ticker
    {
        public decimal Bid { get; set; }
        public decimal BidSize { get; set; }
        public decimal Ask { get; set; }
        public decimal AskSize { get; set; }
        public long Time { get; set; }

        public Ticker(decimal bid, decimal bidSize, decimal ask, decimal askSize, long time)
        {
            Bid = bid;
            BidSize = bidSize;
            Ask = ask;
            AskSize = askSize;
            Time = time;
        }

        public Ticker(JObject jo)
        {
            Time = jo["E"].Value<long>();
            Bid = jo["b"].Value<decimal>();
            BidSize = jo["B"].Value<decimal>();
            Ask = jo["a"].Value<decimal>();
            AskSize = jo["A"].Value<decimal>();
        }
    } // end of struct Ticker

} // end of namespace
