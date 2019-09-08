using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CryptoApis.SharedModels
{
    public class AggregatedOrderBook
    {
        public List<OrderBookEntry> Bids { get; private set; }
        public List<OrderBookEntry> Asks { get; private set; }
        public int BidCount => Bids.Count;
        public int AskCount => Asks.Count;

        public AggregatedOrderBook(List<OrderBookEntry> bids, List<OrderBookEntry> asks)
        {
            Bids = bids.OrderByDescending(b => b.Price).ToList();
            Asks = asks.OrderBy(a => a.Price).ToList();
        }
    } // end of class ConsolidatedOrderBook

} // end of namespace
