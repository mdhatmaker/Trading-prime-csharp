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
    public partial class TSxInventory : UserControl
    {
        public TSxInventory()
        {
            InitializeComponent();
        }
        string[] m_markets = null;
        int m_buttonMargin = 5;
        int m_buttonWidth = 85;
        int m_buttonHeight = 32;
        TSxImages m_img;
        List<string> m_lowInventory = (new string[] { "GDAX" }).ToList();
        List<string> m_noInventory = (new string[] { "itBit" }).ToList();
        List<Button> m_marketButtons = new List<Button>();
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

        private void TSxInventory_Load(object sender, EventArgs e)
        {
            if (Assembly.GetEntryAssembly() != null)
            {
                loadImages();
                foreach (Button b in m_marketButtons)
                {
                    iconUpdate(b, MarketButtonState.MarketInactive);
                }
            }
            this.BackColor = TSx.m_buttonBackColorHighlight;

        }
        public void setButtonStrings(string[] bs)
        {
            List<string> more_markets = new List<string>(bs);
            more_markets.Insert(0, "");
            more_markets.Add("Silvergate");
            m_markets = bs;

            List<string> more_cryptos = new List<string>(m_cryptos);
            more_cryptos.Add("USD");

            int j = -1;
            foreach (string s in more_markets)
            {
                Button b = createButton(s);
                this.Controls.Add(b);
                int i = 0;
                string label = "";
                foreach(string cc in more_cryptos)
                {
                    if (j >= 0)
                        label = m_data[(j * (more_cryptos.Count+1)) + i + 1];
                    else
                        label = more_cryptos[i];
                    Button bb = createButton2(s,i, b.Top, label);
                    this.Controls.Add(bb);
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
                b.Top = this.titleLabel.Bottom + 10;// + m_buttonHeight + m_buttonMargin;
            else
                b.Top = m_marketButtons[m_marketButtons.Count-1].Bottom + m_buttonMargin;
            b.Left = this.titleLabel.Left;
            b.Width = m_buttonWidth;
            b.Height = m_buttonHeight;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.BorderColor = TSx.m_buttonBackColorHighlight;
            b.Font = new System.Drawing.Font("Segoe UI", 11F);
            b.ForeColor = TSx.m_buttonForeColorHighlight;
            b.BackColor = TSx.m_buttonBackColorHighlight;
             
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.ImageAlign = ContentAlignment.MiddleRight;

            m_marketButtons.Add(b);
            return b;
        }
        private Button createButton2(string s,int x,int y,string t)
        {
            Button b = new Button();
            b.Text = t;
            b.Top = y - m_buttonMargin;
            b.Left = 14 + (x +1)* (m_buttonWidth+25);
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
                b.TextAlign = ContentAlignment.MiddleCenter;
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
    }
}
