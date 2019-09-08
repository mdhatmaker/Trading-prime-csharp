using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeTrader
{
    public class Backtest
    {
        private DataFrameFile m_dfFile;
        private float m_openBuy;
        private float m_openSell;
        private float m_closeBuy;
        private float m_closeSell;

        public Backtest(DataFrameFile dfFile, float openBuy=float.NaN, float openSell=float.NaN, float closeBuy=float.NaN, float closeSell=float.NaN)
        {
            m_dfFile = dfFile;
            m_openBuy = openBuy;
            m_openSell = openSell;
            m_closeBuy = closeBuy;
            m_closeSell = closeSell;
        }

        public void RunTest()
        {
            var rows = m_dfFile.ReadRows();
            foreach (var row in rows.Skip(1))       // skip first row (which contains column names)
            {
                
            }
        }
    }
} // end of namespace
