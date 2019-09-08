using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public delegate void TickerUpdateHandler(object sender, TickersUpdateArgs e);

    public abstract class ZTicker : IDataRow
    {
        public string Key { get { return Symbol; } set { Symbol = value; } }

        public string Symbol { get; set; }
        public abstract decimal Bid { get; }
        public virtual decimal BidSize { get { return 0.0M; }}
        public abstract decimal Ask { get; }
        public virtual decimal AskSize { get { return 0.0M; }}
        public abstract decimal Last { get; }
        public abstract decimal High { get; }
        public abstract decimal Low { get; }
        public abstract decimal Volume { get; }
        public abstract string Timestamp { get; }

        public decimal Mid { get { return (Bid + Ask) / 2; }}
        public decimal BidAskSpread { get { return (Ask - Bid); }}

        public static string[] Columns = { "Symbol", "Bid", "Ask", "Last", "High", "Low", "Volume", "Timestamp" };

        //private string m_symbol;

        private string[] m_cells;

        public ZTicker()
        {
            m_cells = new string[Columns.Length];
        }

        public string[] GetCells()
        {
            m_cells[0] = this.Symbol;
            m_cells[1] = this.Bid.ToString();
            m_cells[2] = this.Ask.ToString();
            m_cells[3] = this.Last.ToString();
            m_cells[4] = this.High.ToString();
            m_cells[5] = this.Low.ToString();
            m_cells[6] = this.Volume.ToString();
            m_cells[7] = (this.Timestamp ?? "").ToString();
            DateTime dt;
            if (DateTime.TryParse(this.Timestamp, out dt))
                m_cells[7] = dt.ToString("HH:mm:ss");
            else
                m_cells[7] = this.Timestamp;
            return m_cells;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}", Bid, Ask, Last, High, Low, Volume, Timestamp);
        }

        public string ToDisplay(string title = null)
        {
            // Single Ticker, so display it on the same line as the title
            string str = string.Format("{0} {1} {2}", title ?? "", Symbol, string.Format("{0}:{1}-{2}:{3}", BidSize, Bid, Ask, AskSize));
            return str;
        }

        public void Display(string title = null)
        {
            string str = ToDisplay(title);
            Console.WriteLine(str + "\n");
        }
    } // end of abstract class


    public class TickersUpdateArgs : EventArgs
    {
        public CryptoExch Exch { get { return m_exch; } }
        public Dictionary<string, ZTicker> Tickers { get { return m_tickers; } }

        private CryptoExch m_exch;
        private Dictionary<string, ZTicker> m_tickers;

        public TickersUpdateArgs(CryptoExch exch, Dictionary<string, ZTicker> tickers)
        {
            m_exch = exch;
            m_tickers = tickers;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0}\n", m_exch));
            foreach (var kv in m_tickers)
            {
                var ticker = kv.Value;
                sb.Append(string.Format("     {0}     {1}  {2}:{3}--{4}:{5}\n", kv.Key, ticker.Symbol, ticker.BidSize, ticker.Bid, ticker.Ask, ticker.AskSize));
            }
            return sb.ToString();
        }
    } // end of class TickersUpdateArgs


} // end of namespace
