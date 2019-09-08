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
using static Tools.G;

namespace PrimeTrader
{
    public partial class PriceGridForm : Form
    {

        private IQFeed.PriceFeed m_prices;

        public PriceGridForm()
        {
            InitializeComponent();

            m_prices = IQFeed.PriceFeed.Instance;
        }

        private void PriceGridForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_prices.UpdatePrices += M_prices_UpdatePrices;
            m_prices.SubscribePrices("@ESZ17");
        }

        private void M_prices_UpdatePrices(Tools.PriceUpdateIQ update)
        {
            //dout("PRICE UPDATE: {0}", update.LastTradePrice);
            if (label1.InvokeRequired)
                label1.Invoke(new Action<PriceUpdateIQ>(M_prices_UpdatePrices), new object[] { update });
            else
                label1.Text = string.Format("{0}", update.LastTradePrice);
        }

    } // end of class
} // end of namespace
