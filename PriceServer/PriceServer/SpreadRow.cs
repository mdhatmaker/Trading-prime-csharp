using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using static Tools.G;

namespace IQFeed
{
    public class SpreadRow : IDataRow
    {
        static public string[] Columns = { "Symbol", "Last", "LastSize", "BidSize", "Bid", "Ask", "AskSize", "Formula" };

        public string Key { get { return Symbol; } set {; } }

        public string Symbol { get; set; }
        public double LastTradePrice { get { return m_spreadFormula.LastTradePrice; } }
        public int LastTradeSize { get { return m_spreadFormula.LastTradeSize; } }
        public double Bid { get { return m_spreadFormula.Bid; } }
        public int BidSize { get { return m_spreadFormula.BidSize; } }
        public double Ask { get { return m_spreadFormula.Ask; } }
        public int AskSize { get { return m_spreadFormula.AskSize; } }
        public string Formula { get { return m_spreadFormula.Formula; } }
        public List<string> FormulaSymbols { get { return m_spreadFormula.FormulaSymbols; } }

        public string PriceFormat { get; set; }

        private SpreadFormula m_spreadFormula;
        private string[] m_cellValues = new string[8];

        public SpreadRow(string symbol, string formula, string priceFormat = "0.0000")
        {
            this.Symbol = symbol;                                   // the symbol is also the Key
            SetFormula(formula);                                    // formula (string) detailing the spread calculation
            this.PriceFormat = priceFormat;                              // default price format string
        }

        public override string ToString()
        {
            return "SpreadRow: " + Str(this);
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
            m_cellValues[7] = Formula;
            return m_cellValues;
        }

        public void SetFormula(string formula)
        {
            m_spreadFormula = new SpreadFormula(formula);
        }

        public void SymbolUpdate(PriceRow row)
        {
            m_spreadFormula.SymbolUpdate(row);
        }

    } // end of CLASS



} // end of NAMESPACE
