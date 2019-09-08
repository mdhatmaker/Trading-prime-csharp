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
    public partial class TSxTransfer : UserControl
    {
        public TSxTransfer()
        {
            InitializeComponent();
            askButton.Font = new System.Drawing.Font("Wingdings", 28F);
            askButton.TextAlign = ContentAlignment.MiddleCenter;
            askButton.Text = "";

            panel1.BackColor = TSx.m_buttonBackColorHighlight;
            panel2.BackColor = TSx.m_buttonBackColorHighlight;

            comboBox1.ForeColor = TSx.m_buttonForeColor;//Color.White;
            comboBox1.BackColor = Color.Black; //  TSx.m_buttonBackColor;
            comboBox1.DrawItem += new DrawItemEventHandler(comboBox1_DrawItem);
            comboBox1.FlatStyle = FlatStyle.Flat;

            comboBox2.ForeColor = TSx.m_buttonForeColor;//Color.White;
            comboBox2.BackColor = Color.Black; //  TSx.m_buttonBackColor;
            comboBox2.DrawItem += new DrawItemEventHandler(comboBox2_DrawItem);
            comboBox2.FlatStyle = FlatStyle.Flat;



            buttonHighlightBuySell(buyButton, Color.Red);
            buyButton.ForeColor = Color.White;
            buyButton.BackColor = Color.Black;
            buyButton.FlatAppearance.BorderColor = Color.White;
                

            this.BackColor = Color.Black;
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
        string[] m_markets = null;
        int m_buttonMargin = 5;
        int m_buttonWidth = 85;
        int m_buttonHeight = 32;
        TSxImages m_img;
        List<string> m_lowInventory = (new string[] { "GDAX" }).ToList();
        List<string> m_noInventory = (new string[] { "itBit" }).ToList();
        List<Button> m_marketButtons = new List<Button>();
        List<Button> m_dataButtons = new List<Button>();
        string[] m_cryptos = { "BCH", "BTC", "ETH", "LTC" };

        public enum MarketButtonState { MarketActive, MarketInactive };
        public enum MarketButtonMode { NormalButtonMode, RadioButtonMode };


        string[] m_data =
        {
        "B2C2","88.13443","10.13536","","543.12844","395,002.50",
        "Bitfinex","3.17406","","","","14,225.55",
        "BitFlyer","14.80247","","27.90676","","66,341.98",
        "Bitstamp","32.12925","3.69483","60.57254","","143,997.44",
        "GDAX","0.96402","0.11086","1.81744","5.94076","4,320.55",
        "Gemini","","45.88633","","2,458.93394","1,788,315.59",
        "Genesis","735.73827","84.60904","1,387.07062","4,533.98710","3,297,445.16",
        "itBit","","0.00000","0.00000","","0.00",
        "Kraken","126.46250","14.54304","238.41687","779.32515","566,781.93",
        "Silvergate","","","","","10,427,556.00"
    };


        public void setButtonStrings(string[] bs)
        {
            List<string> more_markets = new List<string>(bs);
            more_markets.Insert(0, "");
            more_markets.Add("Silvergate");
            m_markets = bs;

            List<string> more_cryptos = new List<string>(m_cryptos);
            more_cryptos.Add("USD");

            foreach (string cr in more_cryptos)
            {
                comboBox1.Items.Add(cr);
            }
            comboBox1.SelectedIndex = more_cryptos.Count - 1;

            foreach (string cr in more_markets)
            {
                comboBox2.Items.Add(cr);
            }
            comboBox2.SelectedIndex = more_markets.Count - 1;


            int j = -1;
            foreach (string s in more_markets)
            {
                Button b = createButton(s);
                this.panel1.Controls.Add(b);
                int i = 0;
                string label = "";
                foreach (string cc in more_cryptos)
                {
                    if (cc.Equals("USD"))
                    {
                        if (j >= 0)
                            label = m_data[(j * (more_cryptos.Count + 1)) + i + 1];
                        else
                            label = more_cryptos[i];
                        Button bb = createButton2(s, 0, b.Top, label);
                        this.panel1.Controls.Add(bb);
                        m_dataButtons.Add(bb);
                    }
                    i++;
                }
                j++;
            }

        }

        private Button createButton(string t)
        {
            Button b = new Button();
            b.Text = t;
            if (m_marketButtons.Count == 0)
                b.Top = 0;// + m_buttonHeight + m_buttonMargin;
            else
                b.Top = m_marketButtons[m_marketButtons.Count - 1].Bottom + m_buttonMargin;
            b.Left = this.comboBox1.Left;
            b.Width = m_buttonWidth;
            b.Height = m_buttonHeight;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.BorderColor = TSx.m_buttonBackColorHighlight;
            b.Font = new System.Drawing.Font("Segoe UI", 11F);
            b.ForeColor = TSx.m_buttonForeColorHighlight;
            b.BackColor = TSx.m_buttonBackColorHighlight;
            b.Click += new System.EventHandler(this.marketButton_Click);
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.ImageAlign = ContentAlignment.MiddleRight;

            m_marketButtons.Add(b);
            return b;
        }
        private void marketButton_Click(object sender, EventArgs e)
        {
            (sender as Button).BackColor = TSx.m_buttonBackColor;
            (sender as Button).ForeColor = Color.Black;
            (sender as Button).Image = m_img.m_low_black;
            //tabClick(((Button)sender).Text);
            int i = 0;
            foreach(Button b in m_dataButtons)
            {
                if (i == 5)
                    b.BackColor = Color.Black;
                else
                    b.BackColor = TSx.m_buttonBackColorHighlight;
                b.ForeColor = Color.White;
                i++;
            }
            turnOn(buyButton);
        }
        private Button createButton2(string s, int x, int y, string t)
        {
            Button b = new Button();
            b.Text = t;
            b.Top = y - m_buttonMargin;
            b.Left = 14 + (x + 1) * (m_buttonWidth + 25);
            b.Width = m_buttonWidth + 25;
            b.Height = m_buttonHeight + m_buttonMargin;
            b.FlatStyle = FlatStyle.Flat;
            this.Width = b.Right + 20;
            if (s == "") // heading 
            {
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = TSx.m_buttonBackColorHighlight;
                b.ForeColor = TSx.m_buttonForeColorHighlight;
                b.BackColor = TSx.m_buttonBackColorHighlight;
                b.TextAlign = ContentAlignment.BottomRight;
            }
            else
            {
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = Color.White;
                b.ForeColor = Color.White;
                b.BackColor = Color.Black;
                b.TextAlign = ContentAlignment.MiddleRight;
            }
            if ((t.IndexOf("0.") == 0) || (t.IndexOf("4,3") == 0))
            {
                b.BackColor = Color.FromArgb(191, 153, 0);
                b.ForeColor = Color.Black;
            }

            if (t.IndexOf("0.0") == 0)
            {
                b.BackColor = Color.FromArgb(155, 0, 0);
                b.ForeColor = Color.White;
            }

            b.Font = new System.Drawing.Font("Segoe UI", 11F);
            //b.ImageAlign = ContentAlignment.MiddleRight;
            return b;
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
        private void loadImages()
        {
            m_img = new TSxImages();

        }

        private void askButton_Click(object sender, EventArgs e)
        {
            int left = panel1.Left;
            panel1.Left = panel2.Left;
            panel2.Left = left;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.White; //  Black;
            e.DrawBackground();
            e.Graphics.DrawString(comboBox1.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();

        }

        private void comboBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.White; //  Black;
            e.DrawBackground();
            e.Graphics.DrawString(comboBox2.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Focus();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void TSxTransfer_Load(object sender, EventArgs e)
        {
            if (Assembly.GetEntryAssembly() != null)
            {
                loadImages();
                foreach (Button b in m_marketButtons)
                {
                    iconUpdate(b, MarketButtonState.MarketInactive);
                }
            }

        }
    }
}



