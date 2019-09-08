using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static Tools.G;

namespace Tools
{
    public delegate void OrderBookUpdateHandler(object sender, OrderBookUpdateArgs e);

    public enum MarketSide { Ask = 0, Bid = 1 }

    public class ZOrderBook
    {
        public SortedDictionary<double, double> Bids { get { return m_bids; } }
        public SortedDictionary<double, double> Asks { get { return m_asks; } }

        public int LevelCount { get; set; }

        private SortedDictionary<double, double> m_bids = new SortedDictionary<double, double>();
        private SortedDictionary<double, double> m_asks = new SortedDictionary<double, double>();

        public ZOrderBook(int levelCount = 25)
        {
            this.LevelCount = levelCount;
        }

        // Use this to get the rows to pass to a GuiTools Grid
        public SortedDictionary<double, IDataRow> GetRows()
        {
            var bestAskPrices = m_asks.Keys.Take(Math.Min(LevelCount, m_asks.Count));
            var bestBidPrices = m_bids.Keys.Reverse().Take(Math.Min(LevelCount, m_bids.Count));
            
            var rows = new SortedDictionary<double, IDataRow>();
            ZOrderBookRow zrow;
            foreach (double price in bestAskPrices.Reverse())
            {
                zrow = new ZOrderBookRow();
                zrow.SetAsk(price, m_asks[price]);
                rows.Add(price, zrow);
            }
            foreach (double price in bestBidPrices)
            {
                if (rows.ContainsKey(price))
                {
                    (rows[price] as ZOrderBookRow).SetBid(price, m_bids[price]);
                }
                else
                {
                    zrow = new ZOrderBookRow();
                    zrow.SetBid(price, m_bids[price]);
                    rows.Add(price, zrow);
                }
            }
            return rows;
        }

        // Given another ZOrderBook, append the Bid and Ask volume to this ZOrderBook
        public void AggregateRows(ZOrderBook appendOrderBook)
        {
            try
            {
                var bestAsks = appendOrderBook.m_asks.Take(Math.Min(LevelCount, appendOrderBook.m_asks.Count)).ToArray();
                var bestBids = appendOrderBook.m_bids.Reverse().Take(Math.Min(LevelCount, appendOrderBook.m_bids.Count)).ToArray();

                foreach (var kv in bestBids)
                {
                    if (m_bids.ContainsKey(kv.Key))
                    {
                        m_bids[kv.Key] += kv.Value;
                    }
                    else
                    {
                        m_bids.Add(kv.Key, kv.Value);
                    }
                }
                foreach (var kv in bestAsks)
                {
                    if (m_asks.ContainsKey(kv.Key))
                    {
                        m_asks[kv.Key] += kv.Value;
                    }
                    else
                    {
                        m_asks.Add(kv.Key, kv.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                dout("EXCEPTION occurred in AggregateRows: {0}", ex.Message);
            }
        }

        /*public SortedDictionary<float, ZOrderBookRow> GetRowsNumeric()
        {
            var bestAsks = m_asks.Keys.Take(Math.Min(LevelCount, m_asks.Count));
            var bestBids = m_bids.Keys.Reverse().Take(Math.Min(LevelCount, m_bids.Count));

            var rows = new SortedDictionary<float, ZOrderBookRow>();
            ZOrderBookRow zrow;
            foreach (float price in bestAsks.Reverse())
            {
                zrow = new ZOrderBookRow();
                zrow.SetAsk(price, m_asks[price]);
                rows.Add(price, zrow);
            }
            foreach (float price in bestBids)
            {
                if (rows.ContainsKey(price))
                {
                    (rows[price] as ZOrderBookRow).SetBid(price, m_bids[price]);
                }
                else
                {
                    zrow = new ZOrderBookRow();
                    zrow.SetBid(price, m_bids[price]);
                    rows.Add(price, zrow);
                }
            }
            return rows;
        }*/

        public void Clear()
        {
            ClearBids();
            ClearAsks();
        }

        public void ClearBids()
        {
            m_bids.Clear();
        }

        public void ClearAsks()
        {
            m_asks.Clear();
        }

        public void UpdateBid(decimal price, decimal amount)
        {
            if (amount == 0.0M)
            {
                m_bids.Remove((double)price);
            }
            else
            {
                m_bids[(double)price] = (double)amount;
            }
        }

        public void UpdateBid(string price, string amount)
        {
            //string amt = amount.TrimEnd(new char[] { '\0' });
            /*int ix0 = amount.IndexOf('\0');
            string amt;
            if (ix0 >= 0)
                amt = amount.Substring(0, ix0 + 1);
            else
                amt = amount;*/
            string prc = price.Replace("\0", String.Empty).Trim();
            string amt = amount.Replace("\0", String.Empty).Trim();
            //string amt = amount.Replace("\0", String.Empty).Trim();
            /*if (float.Parse(amt) == 0.0)
                m_bids.Remove(price);
            else*/
            double dprice, damount;
            if (double.TryParse(prc, out dprice) && double.TryParse(amt, out damount))
            {
                if (damount == 0.0)
                {
                    m_bids.Remove(dprice);
                    //dout("remove bid [{0}]", fprice);
                }
                else
                {
                    m_bids[dprice] = damount;
                    //dout("bids[{0}]={1}", fprice, amt);
                }
            }
            else
                cout("ERROR: could not parse bid '{0}':'{1}' to double", price, amount);
        }

        public void UpdateAsk(decimal price, decimal amount)
        {
            if (amount == 0.0M)
            {
                m_asks.Remove((double)price);
            }
            else
            {
                m_asks[(double)price] = (double)amount;
            }
        }

        public void UpdateAsk(string price, string amount)
        {
            //string amt = amount.TrimEnd(new char[] { '\0' });
            /*int ix0 = amount.IndexOf('\0');
            string amt;
            if (ix0 >= 0)
                amt = amount.Substring(0, ix0 + 1);
            else
                amt = amount;*/
            string prc = price.Replace("\0", String.Empty).Trim();
            string amt = amount.Replace("\0", String.Empty).Trim();
            //string amt = amount.Replace("\0", String.Empty).Trim();
            /*if (float.Parse(amt) == 0.0)
                m_asks.Remove(price);
            else*/
            double dprice, damount;
            if (double.TryParse(prc, out dprice) && double.TryParse(amt, out damount))
            {
                if (damount == 0.0)
                {
                    m_asks.Remove(dprice);
                    //dout("remove ask [{0}]", fprice);
                }
                else
                {
                    m_asks[dprice] = damount;
                    //dout("asks[{0}]={1}", fprice, amt);
                }
            }
            else
                cout("ERROR: could not parse ask '{0}':'{1}' to double", price, amount);
        }

        // An integer quantity assumes unit of Satoshi, so divide by 100,000,000
        public void UpdateBid(string price, int quantity)
        {
            UpdateBid(price, (quantity/1000000.0).ToString());
        }

        // An integer quantity assumes unit of Satoshi, so divide by 100,000,000
        public void UpdateAsk(string price, int quantity)
        {
            UpdateAsk(price, (quantity / 1000000.0).ToString());
        }

        /*public void UpdateBid(JArray b)
        {
            UpdateBid(b[0].ToString(), b[1].ToString());
        }

        public void UpdateAsk(JArray a)
        {
            UpdateAsk(a[0].ToString(), a[1].ToString());
        }*/

        public override string ToString()
        {
            var rows = GetRows().Values.Reverse();
            StringBuilder sb = new StringBuilder();
            foreach (var r in rows)
            {
                sb.AppendLine(string.Join(",", r.GetCells()));
            }
            return sb.ToString();
        }

    } // end of class ZOrderBook

    public class ZOrderBookRow : IDataRow
    {
        public string Key { get { return m_price; } set { ; } }
        public double Price { get { return m_fprice; } }
        public double BidQty { get { return m_fbidQty; } }
        public double AskQty { get { return m_faskQty; } }

        //public bool IsValid { get { return m_price != null; } }

        public static ZOrderBookRow Empty = new ZOrderBookRow();

        private string m_price = "";
        private string m_bidQty = "";
        private string m_askQty = "";

        private double m_fprice = double.NaN;
        private double m_fbidQty = double.NaN;
        private double m_faskQty = double.NaN;

        private string m_priceFormat;
        private string m_amountFormat;

        public ZOrderBookRow(string priceFormat = "0.00", string amountFormat = "0.00000000")
        {
            m_priceFormat = priceFormat;
            m_amountFormat = amountFormat;
        }

        public string[] GetCells()
        {
            return new string[] { m_bidQty, m_price, m_askQty };
        }

        public void SetBid(double bid, double bidQty)
        {
            m_price = bid.ToString(m_priceFormat);
            m_bidQty = bidQty.ToString(m_amountFormat);
            m_fprice = bid;
            m_fbidQty = bidQty;
        }

        public void SetAsk(double ask, double askQty)
        {
            m_price = ask.ToString(m_priceFormat);
            m_askQty = askQty.ToString(m_amountFormat);
            m_fprice = ask;
            m_faskQty = askQty;
        }

        public void AddBidQty(double bidQty)
        {
            if (double.IsNaN(bidQty)) return;

            if (double.IsNaN(m_fbidQty))
            {
                m_fbidQty = bidQty;
            }
            else
            {
                m_fbidQty += bidQty;
            }
        }

        public void AddAskQty(double askQty)
        {
            if (double.IsNaN(askQty)) return;

            if (double.IsNaN(m_faskQty))
            {
                m_faskQty = askQty;
            }
            else
            {
                m_faskQty += askQty;
            }
        }

    } // end of class ZOrderBookRow

    public class OrderBookUpdateArgs : EventArgs
    {
        public SortedDictionary<double, IDataRow> OrderBook { get { return m_orderBook; } }

        private SortedDictionary<double, IDataRow> m_orderBook;

        public OrderBookUpdateArgs(SortedDictionary<double, IDataRow> orderBook)
        {
            m_orderBook = orderBook;
        }
    } // end of class OrderBookUpdateArgs


} // end of namespace
