using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeTrader
{
    static class Program
    {
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DialogResult r = MessageBox.Show("[Yes] Original UI\r\n[No] Revised UI", "Pick UI Mode", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
                Application.Run(new PrimeTraderForm());
            else
                Application.Run(new TSxTradingSuiteMain());
        }
    }
}
