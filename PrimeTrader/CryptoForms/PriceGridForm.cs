using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuiTools.Grid;

namespace CryptoForms
{
    public partial class PriceGridForm : Form
    {
        public DataGridView Grid { get { return gridPrices; } }
        public HeatMapGrid HeatMapGrid { get; set; }

        public PriceGridForm()
        {
            InitializeComponent();
        }
    }
}
