using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tools.G;

namespace IQFeed
{
    public class SpreadFormula
    {
        public string Formula { get; private set; }                 // string containing the full formula for this spread
        public double LastTradePrice { get { return m_lastTradePrice; } }
        public int LastTradeSize { get { return m_lastTradeSize; } }
        public double Bid { get { return m_bid; } }
        public int BidSize { get { return m_bidSize; } }
        public double Ask { get { return m_ask; } }
        public int AskSize { get { return m_askSize; } }

        public List<string> FormulaSymbols { get { return m_formulaSymbols; } }

        //private string m_formula;                                   
        private double m_lastTradePrice;
        private int m_lastTradeSize;
        private double m_bid;
        private int m_bidSize;
        private double m_ask;
        private int m_askSize;

        private List<string> m_formulaSymbols;                      // list of all symbols used by this spread's formula
        private Dictionary<string, string> m_legs;                  // symbol -> leg (string) containing that symbol
        private Dictionary<string, double> m_legValues;             // symbol -> calculated value of leg

        public SpreadFormula(string formula)
        {
            m_formulaSymbols = new List<string>();
            m_legs = new Dictionary<string, string>();
            m_legValues = new Dictionary<string, double>();

            this.Formula = formula;

            // TODO: PARSE THE FORMULA FOR NEEDED SYMBOLS AND ADD THESE TO m_formulaSymbols List
            // example: [+12.6*<QHOZ17>][-400*<GASZ17>]
            var legs = formula.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string leg in legs)
            {
                int i1 = leg.IndexOf('<');
                int i2 = -1;
                while (i1 > 0)
                {
                    i2 = leg.IndexOf('>', i1);
                    if (i2 > i1)
                    {
                        string symbol = leg.Substring(i1 + 1, i2 - i1 - 1);
                        cout("SYMBOL: {0}", symbol);
                        m_formulaSymbols.Add(symbol);
                        m_legs[symbol] = leg;
                        m_legValues[symbol] = double.NaN;
                        i1 = leg.IndexOf('<', i2);
                    }
                    else
                    {
                        i1 = -1;
                    }
                }
            }
        }

        private string ReplaceSymbolWithValue(string legFormula, string symbol, decimal value)
        {
            string s = "<" + symbol + ">";
            string lf = legFormula;
            int i1 = lf.IndexOf(s);
            while (i1 >= 0)
            {
                lf = legFormula.Replace(s, value.ToString());
                i1 = lf.IndexOf(s);
            }
            return lf;
            /*int i1 = legFormula.IndexOf(s);
            if (i1 >= 0)
                return legFormula.Replace(s, value.ToString());
            else
                return legFormula;*/
        }

        public void SymbolUpdate(PriceRow row)
        {
            string legFormula = m_legs[row.Symbol];
            // TODO: SUPER inefficient to loop through all spreads here!
            /*foreach (var leg in m_legs.Values)
            {
                string expressionToEvaluate = ReplaceSymbolWithValue(legFormula, row.Symbol, row.LastTradePrice);
            }
            m_legValues[row.Symbol] = Eval(expressionToEvaluate);*/

            string expressionToEvaluate = ReplaceSymbolWithValue(legFormula, row.Symbol, row.LastTradePrice);
            m_legValues[row.Symbol] = Eval(expressionToEvaluate);

            // TODO: if all leg values are valid, assign values to each of the member values
            if (!m_legValues.Values.Contains(double.NaN))
            {
                m_lastTradePrice = 0.0;
                foreach (var value in m_legValues.Values)
                {
                    m_lastTradePrice += value;
                }
                m_bid = 0.0;
                m_ask = 0.0;
                m_lastTradeSize = 0;
                m_bidSize = 0;
                m_askSize = 0;
            }
            //cout("SymbolUpdate: {0} {1} {2}", row.Symbol, m_legs[row.Symbol], row.LastTradePrice);
        }

    } // end of CLASS
} // end of NAMESPACE
