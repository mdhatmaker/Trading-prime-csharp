using System;
using System.Text;
using System.Collections.Generic;
using static Tools.G;

namespace Tools
{
    public class OrderBookEntry
    {
        string Price { get; set; }
        string Amount { get; set; }

        public OrderBookEntry(string price, string amount)
        {
            this.Price = price;
            this.Amount = amount;
        }

        public override string ToString()
        {
            return string.Format("{0} @ {1}", this.Amount, this.Price);
        }
    } // end of class OrderBookEntry

    public class OrderBook
    {
        public SortedDictionary<string, OrderBookEntry> bids { get; set; }
        public SortedDictionary<string, OrderBookEntry> asks { get; set; }

        public OrderBook(List<List<string>> bids, List<List<string>> asks)
        {
            bids = new List<List<string>>();
            asks = new List<List<string>>();
            foreach (var b in bids)
            {
                this.bids.Add(b[0], new OrderBookEntry(b[0], b[1]));
            }
            foreach (var a in asks)
            {
                this.asks.Add(a[0], new OrderBookEntry(a[0], a[1]));
            }
        }

        public OrderBook(List<List<string>> depth)
        {
            SortedDictionary<string, string> sortedBids = new SortedDictionary<string, string>();
            SortedDictionary<string, string> sortedAsks = new SortedDictionary<string, string>();

            foreach (var d in depth)
            {
                if (d[2].StartsWith("-"))
                {
                    //cout("{0} {1}", d[0], d[2]);
                    sortedAsks.Add(d[0], d[2].Substring(1));
                }
                else
                {
                    //cout("{0} {1}", d[0], d[2]);
                    sortedBids.Add(d[0], d[2]);
                }
            }

            //this.bids = new SortedDictionary<string, OrderBookEntry>();
            this.asks = new SortedDictionary<string, OrderBookEntry>();
            this.bids = new SortedDictionary<string, OrderBookEntry>(Comparer<string>.Create((x, y) => y.CompareTo(x)));

            foreach (var bk in sortedBids.Keys)
            {
                this.bids.Add(bk, new OrderBookEntry(bk, sortedBids[bk]));
            }
            foreach (var ak in sortedAsks.Keys)
            {
                this.asks.Add(ak, new OrderBookEntry(ak, sortedAsks[ak]));
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ASKS:\n");
            if (this.asks == null)
                sb.Append("(null)\n");
            else
            {
                foreach (string x in this.asks.Keys)
                {
                    sb.Append(asks[x].ToString() + "\n");
                }
            }
            sb.Append("BIDS:\n");
            if (this.bids == null)
                sb.Append("(null)\n");
            else
            {
                foreach (var x in this.bids.Keys)
                {
                    sb.Append(bids[x].ToString() + "\n");
                }
            }
            return sb.ToString();
        }

    } // end of class

} // end of namespace
