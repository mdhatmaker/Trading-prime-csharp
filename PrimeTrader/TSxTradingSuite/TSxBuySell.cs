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
    public partial class TSxBuySell : UserControl
    {
        int m_buttonMargin = 5;
        int m_buttonWidth = 85;
        int m_buttonHeight = 32;
        TSxImages m_img;
        List<string> m_lowInventory = (new string[] { "GDAX" }).ToList();
        List<string> m_noInventory = (new string[] { "itBit" }).ToList();
        List<Button> m_marketButtons = new List<Button>();
        MarketButtonMode m_buttonMode = MarketButtonMode.NormalButtonMode;
        public enum MarketButtonState { MarketActive, MarketInactive };
        public enum MarketButtonMode { NormalButtonMode, RadioButtonMode };
        public List<bool> m_buttonEnabled = new List<bool>();

        public void SetButtonMode(MarketButtonMode bm)
        {
            m_buttonMode = bm;
        }
        public TSxBuySell()
        {
            InitializeComponent();

        }
        [Description("Title text"), Category("Appearance")]
        public string TitleText
        {
            get { return this.titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        public void setButtonStrings(string[] bs)
        {
            foreach(string s in bs) {
                Button b = createButton(s);

                this.Controls.Add(b);
                m_buttonEnabled.Add(true);
            }
        }

        private Button createButton(string t)
        {
            Button b = new Button();
            b.Text = t;
            if (m_marketButtons.Count == 0)
                b.Top = this.dataGridView1.Top;
            else
                b.Top = m_marketButtons[m_marketButtons.Count - 1].Bottom + m_buttonMargin;
            b.Left = this.dataGridView1.Right;
            b.Width = m_buttonWidth;
            b.Height = m_buttonHeight;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.BorderColor = TSx.m_buttonBackColor;
            b.Font = new System.Drawing.Font("Segoe UI", 11F);
            b.BackColor = TSx.m_buttonBackColor;
            b.ForeColor = TSx.m_buttonForeColor;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Click += new System.EventHandler(this.marketButton_Click);
            b.ImageAlign = ContentAlignment.MiddleRight;

            m_marketButtons.Add(b);
            return b;
        }

        private void marketButton_Click(object sender, EventArgs e)
        {
            //tabClick(((Button)sender).Text);
            if (m_buttonMode == MarketButtonMode.NormalButtonMode)
                invertColors((Button)sender);
            else
            {
                radioClick((Button)sender);
            }
        }
        private void radioClick(Button b)
        {
            disableAll();
            invertColors(b);
            dataGridLabel.Text = b.Text;
        }
        public void SetPair(string p)
        {
            this.size1Label.Text = p.Substring(0, 3);
            this.size2Label.Text = p.Substring(4, 3);
        }
        private void disableAll()
        {
            foreach(Button b in m_marketButtons)
            {
                b.ForeColor = TSx.m_buttonForeColorHighlight;
                b.BackColor = TSx.m_buttonBackColorHighlight;
                b.FlatAppearance.BorderColor = TSx.m_buttonBackColorHighlight;
                iconUpdate(b, MarketButtonState.MarketInactive);
            }

        }

        private void iconUpdate(Button b, MarketButtonState bs) 
        {
            if (bs == MarketButtonState.MarketInactive)
            {
                if (m_lowInventory.Contains(b.Text))
                    b.Image = m_img.m_low_white;
                if (m_noInventory.Contains(b.Text))
                    b.Image = m_img.m_none_white;
            }
            else
            {
                if (m_lowInventory.Contains(b.Text))
                    b.Image = m_img.m_low_black;
                if (m_noInventory.Contains(b.Text))
                    b.Image = m_img.m_none_black;
            }
        }
        private void invertColors(Button b)
        {
            if (b.ForeColor == TSx.m_buttonForeColor)
            {
                b.ForeColor = TSx.m_buttonForeColorHighlight;
                b.BackColor = TSx.m_buttonBackColorHighlight;
                b.FlatAppearance.BorderColor = TSx.m_buttonBackColorHighlight;
                iconUpdate(b, MarketButtonState.MarketInactive);

            }
            else {
                b.ForeColor = TSx.m_buttonForeColor;
                b.BackColor = TSx.m_buttonBackColor;
                b.FlatAppearance.BorderColor = TSx.m_buttonBackColor;
                iconUpdate(b, MarketButtonState.MarketActive);
            }
        }
        private void configDataGrid(DataGridView d)
        {
            d.Columns.Add("Bid Qty", "Bid Qty");
            d.Columns.Add("Price", "Price");
            d.Columns.Add("Ask Qty", "Ask Qty");
            d.Columns[0].Width = d.Width / 3;
            d.Columns[1].Width = d.Width / 3;
            d.Columns[2].Width = d.Width / 3 - 1;
            d.RowHeadersVisible = false;
            d.ColumnHeadersVisible = true;
            d.Rows.Add("0.123", "0.123", "0.123");
            d.Rows.Add("0.123", "0.123", "0.123");
            d.Rows.Add("0.123", "0.123", "0.123");
            d.Rows.Add("0.123", "0.123", "0.123");
            d.ColumnHeadersDefaultCellStyle.BackColor = TSx.m_buttonForeColorSelected;
            d.ColumnHeadersDefaultCellStyle.ForeColor = TSx.m_buttonBackColorSelected;
            d.EnableHeadersVisualStyles = false;
            d.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            d.ClearSelection();
            d.Columns[0].DefaultCellStyle.ForeColor = TSx.m_brightBlue;
            d.Columns[2].DefaultCellStyle.ForeColor = Color.Red;
            d.DefaultCellStyle.SelectionBackColor = d.DefaultCellStyle.BackColor;
            d.DefaultCellStyle.SelectionForeColor = TSx.m_brightBlue;
        }
        private void size1textBox_Click(object sender, EventArgs e)
        {
            size2textBox.ForeColor = TSx.m_buttonForeColorHighlight;
            size2textBox.BackColor = TSx.m_buttonBackColorHighlight;
            size1textBox.ForeColor = TSx.m_buttonForeColorSelected;
            size1textBox.BackColor = TSx.m_buttonBackColorSelected;
        }

        private void size2textBox_Click(object sender, EventArgs e)
        {
            size1textBox.ForeColor = TSx.m_buttonForeColorHighlight;
            size1textBox.BackColor = TSx.m_buttonBackColorHighlight;
            size2textBox.ForeColor = TSx.m_buttonForeColorSelected;
            size2textBox.BackColor = TSx.m_buttonBackColorSelected;
        }
        private void loadImages()
        {
            m_img = new TSxImages();
            askButton.Image = m_img.m_copy_white;
            twoButton.Image = m_img.m_copy_white;
            bidButton.Image = m_img.m_copy_white;
            foreach (Button b in m_marketButtons)
            {
                iconUpdate(b, MarketButtonState.MarketActive);
            }

            if (m_buttonMode == MarketButtonMode.RadioButtonMode)
            {
                radioClick(m_marketButtons[0]);
            }
        }

        private void TSxBuySell_Load(object sender, EventArgs e)
        {

            buttonHighlightBuySell(buyButton, Color.Red);
            buttonHighlightBuySell(sellButton, Color.Blue);
            configDataGrid(this.dataGridView1);
            this.BackColor = TSx.m_buttonBackColorHighlight;
            size2textBox.ForeColor = TSx.m_buttonForeColorHighlight;
            size2textBox.BackColor = TSx.m_buttonBackColorHighlight;
            size1textBox.ForeColor = TSx.m_buttonForeColorSelected;
            size1textBox.BackColor = TSx.m_buttonBackColorSelected;

            if (Assembly.GetEntryAssembly() != null)
            {
                loadImages();
            }
        }

        private void buttonHighlightBuySell(Button b, Color c)
        {
            turnOn(b);
            b.MouseEnter += (s, e) =>
            {

                b.BackColor = c;
                b.FlatAppearance.BorderColor = c;// (58, 99, 154);// (60, 129, 181);
                b.ForeColor = Color.White;
            };
            b.MouseLeave += (s, e) =>
            {
                turnOn(b);
            };
        }
        private void turnOn(Button bb)
        {
            bb.BackColor = TSx.m_buttonBackColor;
            bb.ForeColor = TSx.m_buttonForeColor;
            bb.FlatAppearance.BorderColor = TSx.m_buttonBackColor;
        }

        private void marketWidthTextBox_TextChanged(object sender, EventArgs e)
        {
            double b1 = Convert.ToDouble(buyLabel1.Text);
            double s1 = Convert.ToDouble(sellLabel1.Text);
            double mw = Convert.ToDouble(marketWidthTextBox.Text);
            buyLabel2.Text = "" + (int)(b1 + mw) + ".00";
            sellLabel2.Text = "" + (int)(s1 - mw) + ".00";
            size2textBox.Text = buyLabel2.Text;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    panelTrade.Visible = false;
        //    m_selectedTab = "Trade";
        //}
    }
}
 
