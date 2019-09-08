using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public delegate void TradesUpdateHandler(object sender, TradesUpdateArgs e);

    public abstract class ZTrade : IDataRow
    {
        public string Key { get { return Symbol; } set { Symbol = value; } }

        public string Symbol { get; set; }
        public abstract string Price { get; }
        public abstract string Amount { get; }
        public abstract string TradeId { get; }
        public abstract string TradeType { get; }
        public abstract string Type { get; }
        public abstract string Timestamp { get; }


        public static string[] Columns = { "Symbol", "Price", "Amount", "TradeType", "Type", "TradeId", "Timestamp" };


        private string[] m_cells;

        public ZTrade()
        {
            m_cells = new string[Columns.Length];
        }

        public string[] GetCells()
        {
            m_cells[0] = this.Symbol;
            m_cells[1] = this.Price;
            m_cells[2] = this.Amount;
            m_cells[3] = this.TradeType;
            m_cells[4] = this.Type;
            m_cells[5] = this.TradeId;
            DateTime dt;
            if (DateTime.TryParse(this.Timestamp, out dt))
                m_cells[6] = dt.ToString("HH:mm:ss");
            else
                m_cells[6] = this.Timestamp;
            return m_cells;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}", Price, Amount, TradeType, Type, TradeId, Timestamp);
        }
    } // end of abstract class ZTrade


    public class TradesUpdateArgs : EventArgs
    {
        public Dictionary<string, ZTrade> Trades { get { return m_trades; } }

        private Dictionary<string, ZTrade> m_trades;

        public TradesUpdateArgs(Dictionary<string, ZTrade> trades)
        {
            m_trades = trades;
        }
    } // end of class TradesUpdateArgs

} // end of namespace
