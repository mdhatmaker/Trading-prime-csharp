using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeTrader
{
    public partial class TSxTradingSuiteMain : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        ///
        /// Handling the window messages
        ///
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        string m_selectedTab = "Trade";
        public bool m_dragging = false;

        int m_tab_width = 91;
        int m_tab_height = 38;
        int m_tab_top = 119;
        int m_tab_gap = 5;
        int m_tab_offset = 23;

        TSxTabWindow m_tabWin = null;

        TSxPairPicker m_ucPairPicker = new TSxPairPicker();
        TSxInventory m_ucInventory = new TSxInventory();
        TSxTransfer m_ucTransfer = new TSxTransfer();
        TSxTrade m_ucTrade = new TSxTrade();
        TSxImages m_img = new TSxImages();

        static List<string> m_tab_order = new List<string>(new string[] { "Trade", "Inventory","Transfer", "Research", "Automate"});
        List<string> m_tabs = new List<string>(m_tab_order.ToArray());

        string[] m_symbols1 = { "BCH", "BTC", "ETH", "LTC" };
        string[] m_symbols2 = { "USD" };
        string[] m_exchanges = { "GDAX", "Gemini", "Genesis", "itBit", "Kraken", "Bitstamp", "Bitfinex", "BitFlyer", "B2C2" };

        List<Button> m_tabs_buttons = new List<Button>();

        public TSxTradingSuiteMain()
        {
            InitializeComponent();
            titleLabel.Text = "ONECHAIN CAPITAL \\ Trading Suite";

            List<string> ex = m_exchanges.ToList();
            ex.Sort();
            m_exchanges = ex.ToArray();

            this.ControlBox = false;
            this.Text = String.Empty;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.Width = TSx.m_width;
            this.Top = 0;
            this.Left = 0;
            this.Height = TSx.m_height;

            mainTabRegionBlank.Width = TSx.m_width;
            mainTabRegionBlank.BackColor = Color.Black;
            mainTabRegionBlank.Height = TSx.m_height;
            mainTabRegionBlank.Top = m_tab_top + m_tab_height;
            mainTabRegionBlank.Left = 0;
            mainTabRegionBlank.Visible = true;

            this.BackgroundImage = m_img.m_skyline; ;

            closeButton.Left = this.Width - closeButton.Width - 18;
            closeButton.Top = 3 ;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.BackColor = TSx.m_buttonBackColorHighlight;
            closeButton.ForeColor = TSx.m_buttonForeColor;
            closeButton.FlatAppearance.BorderColor = TSx.m_buttonForeColor;
            closeButton.FlatAppearance.MouseOverBackColor = TSx.m_buttonBackColor;

            timer1.Interval = 1000;
            timer1.Start();

            addPairPicker();
            addInventoryControl();
            addTransferControl();

            m_ucTrade.Left = (this.Width / 2) - (m_ucTrade.Width / 2);
            m_ucTrade.Top = m_tab_top + m_tab_height + 20;
            m_ucTrade.Visible = false;
            m_ucTrade.BackColor = Color.Black;
            this.Controls.Add(m_ucTrade);
            m_ucTrade.BringToFront();

            TSxBuySell de1 = (m_ucTrade.Controls.Find("distributedExchange1", false)[0] as TSxBuySell);
            de1.setButtonStrings(m_exchanges);
            de1.TitleText = "Distributed Trade";

            TSxBuySell se1 = (m_ucTrade.Controls.Find("singleExchange1", false)[0] as TSxBuySell);
            se1.setButtonStrings(m_exchanges);
            se1.TitleText = "Single-Exchange Trade";
            se1.SetButtonMode(TSxBuySell.MarketButtonMode.RadioButtonMode);

            m_tabWin = new TSxTabWindow();
            m_tabWin.Top = this.Top + 400;
            m_tabWin.Left = this.Left + 400;
            m_tabWin.Show();
            m_tabWin.Hide();

            plusButton.Text = "";
            plusButton.Image = m_img.m_untab_black;
            plusButton.FlatStyle = FlatStyle.Flat;
            plusButton.FlatAppearance.BorderSize = 0;
            plusButton.BackColor = TSx.m_buttonBackColorHighlight;
            plusButton.ForeColor = TSx.m_buttonForeColor;
            plusButton.FlatAppearance.BorderColor = TSx.m_buttonForeColor;
            plusButton.FlatAppearance.MouseOverBackColor = TSx.m_buttonBackColor;


            drawTabs();
        }
        private void addPairPicker()
        {
            m_ucPairPicker.Left = (this.Width / 2) - (m_ucPairPicker.Width / 2);
            m_ucPairPicker.BackColor = Color.Black;
            m_ucPairPicker.Top = m_tab_top + m_tab_height + 20;
            m_ucPairPicker.Visible = true;
            m_ucPairPicker.SetLists(m_symbols1.ToList<string>(), m_symbols2.ToList<string>());
            m_ucPairPicker.NewPairEventCallbackHandler += ucPairPicker_NewPairEventCallbackHandler;
            this.Controls.Add(m_ucPairPicker);
            m_ucPairPicker.BringToFront();
        }
        private void drawTabs()
        {
            preserveTabOrder();
            // Make sure buttons exist for all strings in m_tabs
            int i = 0;
            foreach (string s in m_tabs)
            {
                if (i > (m_tabs_buttons.Count-1))
                {
                    NewTabButton(s);
                }
                i++;
            }
            repaintTabButtons();
        }

        private void preserveTabOrder()
        {
            List<string> ordered = new List<string>();
            List<string> copy = m_tabs;
            foreach(string s in m_tab_order)
            {
                if (copy.Contains(s))
                    ordered.Add(s);
            }
            foreach(string s in copy)
            {
                if (!m_tab_order.Contains(s))
                    ordered.Add(s);
            }
            m_tabs = ordered;

        } 

        private void repaintTabButtons()
        {
            // Now draw them
            int i = 0;
            foreach (Button b in m_tabs_buttons)
            {
                if (i > (m_tabs.Count - 1))
                    b.Visible = false;
                else
                {
                    b.Text = m_tabs[i];
                    b.Visible = true;
                }
                b.Left = ((m_tab_gap + m_tab_width) * i) + m_tab_offset;
                if (b.Text == m_selectedTab)
                {
                    b.BackColor = TSx.m_buttonBackColorSelected;
                    b.ForeColor = TSx.m_buttonForeColorSelected;
                }
                else
                {
                    b.BackColor = TSx.m_buttonBackColor;
                    b.ForeColor = TSx.m_buttonForeColor;
                }
                i++;
            }
        }

        private void NewTabButton(string s)
        {
            Button b = new Button();
            b.Name = s;
            b.Text = s;
            b.Top = m_tab_top;
            b.Width = m_tab_width;
            b.Height = m_tab_height;
            b.Font = new System.Drawing.Font("Segoe UI", 11F);
            b.BackColor = TSx.m_buttonBackColor;
            b.ForeColor = TSx.m_buttonForeColor;
            b.Click += new System.EventHandler(this.tab_Click);
            this.Controls.Add(b);
            tabHighlight(b);
            m_tabs_buttons.Add(b);
            b.Left = ((m_tab_gap + m_tab_width) * (m_tabs_buttons.Count-1)) + m_tab_offset;
        }

        private void tabHighlight(Button b)
        {   
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = TSx.m_buttonBackColor;
            b.ForeColor = TSx.m_buttonForeColor;
            b.FlatAppearance.BorderColor = TSx.m_buttonBackColor;
            b.MouseEnter += (s, e) =>
            {

                b.BackColor = TSx.m_buttonBackColorHighlight;// (58, 99, 154);// (60, 129, 181);
                b.FlatAppearance.BorderColor = TSx.m_buttonBackColorHighlight;// (58, 99, 154);// (60, 129, 181);
                b.ForeColor = TSx.m_buttonForeColorHighlight;
            };
            b.MouseLeave += (s, e) =>
            {
                if (b.Text == m_selectedTab)
                {
                    b.BackColor = TSx.m_buttonBackColorSelected;
                    b.ForeColor = TSx.m_buttonForeColorSelected;
                    b.FlatAppearance.BorderColor = TSx.m_buttonBackColorSelected;
                }
                else
                {
                    b.BackColor = TSx.m_buttonBackColor;
                    b.ForeColor = TSx.m_buttonForeColor;
                    b.FlatAppearance.BorderColor = TSx.m_buttonBackColor;
                }
            };
        }

        void tabClick(string s)
        {
            m_selectedTab = s;
            drawTabs();
            if (s.IndexOf("_") > 0)
            {
                m_ucTrade.Visible = true;
                m_ucTrade.SetPair(s);
                if (m_tabs.Contains("Trade"))
                    m_ucPairPicker.Visible = false;
                m_ucInventory.Visible = false;
                m_ucTransfer.Visible = false;
            }
            if (s == "Trade")
            {
                m_ucPairPicker.Visible = true;
                m_ucTrade.Visible = false;
                m_ucInventory.Visible = false;
                m_ucTransfer.Visible = false;
            }
            if (s == "Inventory")
            {
                m_ucPairPicker.Visible = false;
                m_ucTrade.Visible = false;
                m_ucInventory.Visible = true;
                m_ucTransfer.Visible = false;
                m_ucInventory.BringToFront();
            }
            if (s == "Transfer")
            {
                m_ucPairPicker.Visible = false;
                m_ucTrade.Visible = false;
                m_ucInventory.Visible = false;
                m_ucTransfer.Visible = true;
                m_ucTransfer.BringToFront();
            }

        }
        private void tab_Click(object sender, EventArgs e)
        {
            tabClick(((Button)sender).Text);
        }

 
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
        private void addInventoryControl()
        {
            m_ucInventory.Left = (this.Width / 2) - (m_ucInventory.Width / 2);
            m_ucInventory.Top = m_tab_top + m_tab_height + 20 + 52;
            m_ucInventory.Visible = false;
            this.Controls.Add(m_ucInventory);
            m_ucInventory.setButtonStrings(m_exchanges);
        }
        private void addTransferControl()
        {
            m_ucTransfer.Left = (this.Width / 2) - (m_ucTransfer.Width / 2);
            m_ucTransfer.Top = m_tab_top + m_tab_height + 20;
            m_ucTransfer.Visible = false;

            this.Controls.Add(m_ucTransfer);
            m_ucTransfer.setButtonStrings(m_exchanges);
        }
        private void TSxTraderSuiteForm_Resize(object sender, EventArgs e)
        {
            closeButton.Left = this.Width - closeButton.Width - 18;

            m_ucPairPicker.Left = (this.Width / 2) - (m_ucPairPicker.Width / 2);
            m_ucInventory.Left = (this.Width / 2) - (m_ucInventory.Width / 2);
            m_ucTransfer.Left = (this.Width / 2) - (m_ucTransfer.Width / 2);
            m_ucTrade.Left = (this.Width / 2) - (m_ucTrade.Width / 2);

            //674,634
            m_ucTransfer.Size = new Size(627, 640);

            timeLabel.Left = this.Width - timeLabel.Width - 20;
            plusButton.Left = this.Width - plusButton.Width - 20;
            plusButton.Top = mainTabRegionBlank.Top + 10;
            plusButton.BringToFront();

            if (m_ucPairPicker.Left < 10)
                m_ucPairPicker.Left = 10;
            if (m_ucInventory.Left < 10)
                m_ucInventory.Left = 10;
            if (m_ucTransfer.Left < 10)
                m_ucTransfer.Left = 10;
            if (m_ucTrade.Left < 10)
                m_ucTrade.Left = 10;
        }


 

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToLocalTime().ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLocalTime().ToShortTimeString();
            timeLabel.Left = this.Width - timeLabel.Width - 20;
           
        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            m_tabWin.Top = this.mainTabRegionBlank.Top  +80;
            m_tabWin.Left = this.mainTabRegionBlank.Left + 80;
            m_tabWin.setActiveControl(this, "Trade", m_ucPairPicker);
            m_tabWin.Show();
            m_tabWin.Activate();

            string s = "Trade";
            int index = m_tabs_buttons.FindIndex(item => item.Text == s);
            m_tabs.RemoveAt(index);
            //this.Controls.RemoveByKey(s);
            //m_tabs_buttons.RemoveAt(index);
            drawTabs();
        }

        public void ReTabify(Control c)
        {
            string s = c.Name;
            m_tabs.Add(s);
            m_ucPairPicker = (TSxPairPicker)c;
            addPairPicker();
            m_selectedTab = s;
            drawTabs();
            tabClick(s);
        }

  

    private void ucPairPicker_NewPairEventCallbackHandler(object sender, NewPairEventArgs e)
        {
            int index = m_tabs_buttons.FindIndex(item => item.Text == e.Pair);
            if (index == -1)
            {
                m_tabs.Add(e.Pair);
            }
            tabClick(e.Pair);
        }

    }
}
