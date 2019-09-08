using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace PrimeTrader
{
    public partial class TSxTrade : UserControl
    {
        private TSxImages m_img = null;
        public TSxTrade()
        {
            InitializeComponent();


        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.White; //  Black;
            e.DrawBackground();
            e.Graphics.DrawString(comboBox1.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();

        }

        public void SetPair(string p)
        {
            this.distributedExchange1.SetPair(p);
            this.singleExchange1.SetPair(p);
        }

        private void TSxTrade_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Contoso Ltd");
            comboBox1.Items.Add("Forest Park FX");
            comboBox1.Items.Add("ONECHAIN: Will Zhang");
            comboBox1.SelectedIndex = 0;
            comboBox1.ForeColor =  TSx.m_buttonForeColor;//Color.White;
            comboBox1.BackColor = Color.Black; //  TSx.m_buttonBackColor;
            comboBox1.DrawItem += new DrawItemEventHandler(comboBox1_DrawItem);
            comboBox1.FlatStyle = FlatStyle.Flat;
            if (Assembly.GetEntryAssembly() != null)
            {
                m_img = new TSxImages();
                pictureBox1.Image = m_img.Resize(m_img.m_fake_orders, pictureBox1.Width, pictureBox1.Height);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            distributedExchange1.Focus();
        }
    }
}
