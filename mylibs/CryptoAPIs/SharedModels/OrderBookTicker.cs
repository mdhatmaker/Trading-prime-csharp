using System;
using System.Collections.Generic;
using System.Text;
using CryptoTools.Net;

namespace CryptoApis.SharedModels
{
    public struct OrderBookTicker : NullableObject
    {
        public string symbol { get; set; }
        public decimal bidPrice { get; set; }
        public decimal bidQty { get; set; }
        public decimal askPrice { get; set; }
        public decimal askQty { get; set; }

        public decimal MidPrice { get { return (bidPrice + askPrice) / 2.0M; } }
        public decimal BidAskSpread { get { return askPrice - bidPrice; } }

        public bool IsNull { get { return symbol == null; } }
    } // OrderBookTicker

} // end of namespace
