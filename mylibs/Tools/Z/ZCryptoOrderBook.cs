using System;
using System.Text;
using System.Collections.Generic;

namespace Tools
{
    public abstract class ZCryptoOrderBook
    {
        public abstract List<ZCryptoOrderBookEntry> Bids { get; }
        public abstract List<ZCryptoOrderBookEntry> Asks { get; }

        public int BidCount { get { return Bids == null ? 0 : Bids.Count; } }
        public int AskCount { get { return Asks == null ? 0 : Asks.Count; } }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("BIDS\n");
            int ix = 0;
            foreach (var b in Bids)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}  {3}\n", ++ix, b.Price, b.Amount, b.Timestamp));
            }
            sb.Append("ASKS\n");
            ix = 0;
            foreach (var a in Asks)
            {
                sb.Append(string.Format(" {0,4}: {1}  {2}  {3}\n", ++ix, a.Price, a.Amount, a.Timestamp));
            }
            return sb.ToString();
        }
    } // end of abstract class ZCryptoOrderBook

    public class ZCryptoOrderBookEntry
    {
        // BidAsk: BID/ASK enum
        // Exchange: CryptoExchange enum
        // Symbol: exchange symbol as string (ex: "BTCUSD")
        // SymbolId: Symbol enum (universal identifier across exchanges)
        public virtual decimal Price { get; private set; }
        public virtual decimal Amount { get; private set; }
        public virtual string Timestamp { get; private set; }

        //public decimal _Price { get { return float.Parse(Price); } }
        //public decimal _Amount { get { return float.Parse(Amount); } }
        public DateTime _Timestamp { get { return string.IsNullOrEmpty(Timestamp) ? DateTime.MinValue : GDate.UnixTimeStampToDateTime(double.Parse(Timestamp)); } }
    
        public ZCryptoOrderBookEntry()
        {
        }

        public ZCryptoOrderBookEntry(string price, string amount, long timestamp)
        {
            this.Price = decimal.Parse(price);
            this.Amount = decimal.Parse(amount);
            this.Timestamp = Tools.GDate.UnixTimeStampToDateTime(timestamp).ToString();
        }

        public ZCryptoOrderBookEntry(decimal price, decimal amount, long timestamp)
        {
            this.Price = price; //.ToString();
            this.Amount = amount;   //.ToString();
            this.Timestamp = Tools.GDate.UnixTimeStampToDateTime(timestamp).ToString();
        }

    } // end of class ZCryptoOrderBookEntry

} // end of namespace
