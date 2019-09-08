using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace IQFeed
{
    public partial class SymbolSearchForm : Form
    {
        private DataFrame dfSymbols;

        public SymbolSearchForm()
        {
            InitializeComponent();

            dfSymbols = IQFeed.dfReadMarketSymbolsFile();
        }

    } // end of class

} // end of namespace
