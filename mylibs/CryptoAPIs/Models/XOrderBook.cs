using System;
using System.Collections.Generic;

namespace CryptoApis.Models
{
    public class XOrderBook : XModel
    {
        public SortedDictionary<decimal, XOrderBookEntry> Bids { get; private set; }
        public SortedDictionary<decimal, XOrderBookEntry> Asks { get; private set; }

        public XOrderBook()
        {
        }
    } // end of class XOrderBook

} // end of namespace
