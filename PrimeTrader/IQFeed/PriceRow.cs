using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using GuiTools.Grid;
using Tools;
using static Tools.G;

namespace IQFeed
{
    public class PriceRow : IDataRow
    {
        static public string[] Columns = { "Symbol", "Last", "LastSize", "BidSize", "Bid", "Ask", "AskSize", "Volume", "Open", "High", "Low", "Close" };

        public string Key { get { return Symbol; } set {; } }

        public string Symbol { get; set; }
        public decimal LastTradePrice { get; set; }
        public int LastTradeSize { get; set; }
        public decimal Bid { get; set; }
        public int BidSize { get; set; }
        public decimal Ask { get; set; }
        public int AskSize { get; set; }
        public int TotalVolume { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }

        public string PriceFormat { get; set; }

        /*public List<DynamicDisplayGrid> NotifyGrids { get { return m_notifyGridsOfUpdate; } }

        //int ix { get; set; }

        private List<DynamicDisplayGrid> m_notifyGridsOfUpdate;*/
        private string[] m_cellValues = new string[12];

        public PriceRow(string symbol, string priceFormat = "0.0000")
        {
            this.Symbol = symbol;                                   // the symbol is also the Key
            this.PriceFormat = priceFormat;                              // default price format string
            //m_notifyGridsOfUpdate = new List<DynamicDisplayGrid>();
        }

        public override string ToString()
        {
            return "PriceRow: " + Str(this);
        }

        public string[] GetCells()
        {
            m_cellValues[0] = Symbol;
            m_cellValues[1] = LastTradePrice.ToString(this.PriceFormat);
            m_cellValues[2] = LastTradeSize.ToString();
            m_cellValues[3] = BidSize.ToString();
            m_cellValues[4] = Bid.ToString(this.PriceFormat);
            m_cellValues[5] = Ask.ToString(this.PriceFormat);
            m_cellValues[6] = AskSize.ToString();
            m_cellValues[7] = TotalVolume.ToString();
            m_cellValues[8] = OpenPrice.ToString(this.PriceFormat);
            m_cellValues[9] = HighPrice.ToString(this.PriceFormat);
            m_cellValues[10] = LowPrice.ToString(this.PriceFormat);
            m_cellValues[11] = ClosePrice.ToString(this.PriceFormat);
            return m_cellValues;
        }

        /*// Maintain a List of DisplayGrid objects that should be notified when this PriceRow is updated
        public void AddNotifyGrid(DynamicDisplayGrid displayGrid)
        {
            if (!m_notifyGridsOfUpdate.Contains(displayGrid))
                m_notifyGridsOfUpdate.Add(displayGrid);
        }*/

        // Simple method to update the properties of the PriceRow
        //public void UpdateValues(double lastTradePrice, int lastTradeSize, double bid, int bidSize, double ask, int askSize, int totalVolume, double openPrice, double highPrice, double lowPrice, double closePrice)
        public void UpdateValues(PriceUpdateIQ update)
        {
            this.LastTradePrice = update.LastTradePrice;
            this.LastTradeSize = update.LastTradeSize;
            this.Bid = update.Bid;
            this.BidSize = update.BidSize;
            this.Ask = update.Ask;
            this.AskSize = update.AskSize;
            this.TotalVolume = update.TotalVolume;
            this.OpenPrice = update.OpenPrice;
            this.HighPrice = update.HighPrice;
            this.LowPrice = update.LowPrice;
            this.ClosePrice = update.ClosePrice;
        }

    } // end of CLASS

} // end of NAMESPACE
