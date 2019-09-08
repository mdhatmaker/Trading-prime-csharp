using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using CryptoAPIs;
using GuiTools.Grid;
using Tools;
using static Tools.G;

namespace PrimeTrader
{
    public partial class CryptoPricesForm : Form
    {
        private IQFeed.PriceFeed m_prices;

        private Dictionary<string, PriceUpdateIQ> m_fxUpdates;
        private Dictionary<string, TickerGridPanel> m_gridPanels = new Dictionary<string, TickerGridPanel>();

        //private TickerGridPanel m_currentDisplayedPanel;
        private Dictionary<string, System.Windows.Forms.Panel> m_panels;

        private static readonly string CURRENCY_SYMBOLS_FILE = "PrimeTrader_CurrencySymbols.DF.csv";

        private DataFrame m_dfSym, m_dfSymFx;

        public CryptoPricesForm()
        {
            InitializeComponent();

            //EtherChain.Instance.GetBlock("101010");
            //EtherChain.Instance.GetLastBlocks();
            //EtherChain.Instance.GetBasicStats();
            //EtherChain.Instance.GetHistorical();
            //Poloniex.Instance.getit();
            //Kraken.Instance.getit();

            // Read crypto symbol files and populate the second list with the valid symbol ids
            m_dfSym = ZCryptoSymbol.dfReadCryptoSymbols();
            m_dfSymFx = ZCryptoSymbol.dfReadCryptoSymbolsFx();
            foreach (var r in m_dfSym.Rows)
            {
                listSymbolIds.Items.Add(r[0]);
            }

            //Console.SetOut(new MyLogger(rtbConsoleOutput));

            // Define all FX symbols here:
            //var symbols = new List<string>() { "@AD#", "@BP#", "@JY#", "@EU#", "@SF#" };

            // Create empty dictionary of symbol->price updates
            m_fxUpdates = new Dictionary<string, PriceUpdateIQ>();
            string pathname = Folders.system_path(CURRENCY_SYMBOLS_FILE);
            var df = DataFrame.ReadDataFrame(pathname);

            foreach (var r in df.Rows)
            {
                string symbol_root = r[1];      // r["IQSymbolRoot"]
                string continuousSymbol = symbol_root + "#";
                if (!m_fxUpdates.ContainsKey(continuousSymbol))
                    m_fxUpdates.Add(continuousSymbol, null);
            }
            /*foreach (string symbol_root in Crypto.CurrencySymbols.Values)
            {
                m_fxUpdates.Add(symbol_root + "#", null);
            }*/

            m_prices = IQFeed.PriceFeed.Instance;
            m_prices.UpdatePrices += prices_UpdatePrices;

            //SubscribeFXPrices();      // moved to Form_Load

            m_panels = new Dictionary<string, System.Windows.Forms.Panel>();
            int i = 0;
            foreach (var exch in Crypto.ExchangeList)
            {
                listExchanges.Items.Add(exch.ExchangeName);
                var pnl = CreatePanel("panel" + exch.ExchangeName);
                panelTickers.Controls.Add(pnl);
                pnl.Visible = false;
                pnl.Dock = DockStyle.Fill;

                TickerGridPanel gridPanel = new TickerGridPanel(pnl, exch.ExchangeName, ColorPalette(1, ++i), ZTicker.Columns);
                gridPanel.Initialize();
                m_gridPanels[exch.ExchangeName] = gridPanel;

                m_panels[exch.ExchangeName] = pnl;
            }

            Kraken.Instance.UpdateTickersEvent += m_gridPanels["KRAKEN"].Handle_UpdateTickers;
            Bithumb.Instance.UpdateTickersEvent += m_gridPanels["BITHUMB"].Handle_UpdateTickers;
            ItBit.Instance.UpdateTickersEvent += m_gridPanels["ITBIT"].Handle_UpdateTickers;
            Bittrex.Instance.UpdateTickersEvent += m_gridPanels["BITTREX"].Handle_UpdateTickers;
            OkCoin.Instance.UpdateTickersEvent += m_gridPanels["OKCOIN"].Handle_UpdateTickers;
            BTCC.Instance.UpdateTickersEvent += m_gridPanels["BTCC"].Handle_UpdateTickers;
            //BTER.Instance.UpdateTickersEvent += m_gridPanels["BTER"].Handle_UpdateTickers;
            Poloniex.Instance.UpdateTickersEvent += m_gridPanels["POLONIEX"].Handle_UpdateTickers;
            //Coinigy.Instance.UpdateTickersEvent += m_gridPanels["COINIGY"].Handle_UpdateTickers;
            Wex.Instance.UpdateTickersEvent += m_gridPanels["WEX"].Handle_UpdateTickers;
            Bitfinex.Instance.UpdateTickersEvent += m_gridPanels["BITFINEX"].Handle_UpdateTickers;
            Bitsquare.Instance.UpdateTickersEvent += m_gridPanels["BITSQUARE"].Handle_UpdateTickers;
            Bitstamp.Instance.UpdateTickersEvent += m_gridPanels["BITSTAMP"].Handle_UpdateTickers;
            Cex.Instance.UpdateTickersEvent += m_gridPanels["CEX"].Handle_UpdateTickers;
            HitBTC.Instance.UpdateTickersEvent += m_gridPanels["HITBTC"].Handle_UpdateTickers;
            Gemini.Instance.UpdateTickersEvent += m_gridPanels["GEMINI"].Handle_UpdateTickers;
            GDAX.Instance.UpdateTickersEvent += m_gridPanels["GDAX"].Handle_UpdateTickers;

            Kraken.Instance.StartTickerTimer();
            Bithumb.Instance.StartTickerTimer();
            ItBit.Instance.StartTickerTimer();
            Bittrex.Instance.StartTickerTimer();
            OkCoin.Instance.StartTickerTimer();
            BTCC.Instance.StartTickerTimer();
            //BTER.Instance.StartTickerTimer();
            Poloniex.Instance.StartTickerTimer();
            //Coinigy.Instance.StartTickerTimer();
            Wex.Instance.StartTickerTimer();
            Bitfinex.Instance.StartTickerTimer(60000);      // Bitfinex (API 1.0) has limit of 60 requests per minute
            Bitsquare.Instance.StartTickerTimer();
            Bitstamp.Instance.StartTickerTimer();
            Cex.Instance.StartTickerTimer();
            HitBTC.Instance.StartTickerTimer();
            Gemini.Instance.StartTickerTimer();
            GDAX.Instance.StartTickerTimer();

            DisplayExchangePrices("BITTREX");
        }

        private void Instance_UpdateTickersEvent(object sender, TickersUpdateArgs e)
        {
            throw new NotImplementedException();
        }

        private void CryptoPricesForm_Load(object sender, EventArgs e)
        {
            G.COutput += G_COutput;

            SubscribeFXPrices();
        }

        private System.Windows.Forms.Panel CreatePanel(string name)
        {
            System.Windows.Forms.Panel panel = new Panel();
            panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            panel.Location = new System.Drawing.Point(480, 12);
            panel.Name = name;
            panel.Size = new System.Drawing.Size(829, 1234);
            panel.TabIndex = 3;
            panel.Visible = false;
            return panel;
        }

        private void DisplayExchangePrices(string exchangeName)
        {
            m_panels[exchangeName].Visible = true;

            foreach (var exchName in m_panels.Keys)
            {
                if (exchName != exchangeName)
                {
                    m_panels[exchName].Visible = false;
                }
            }            
        }

        /*private Panel GetPanel(int i)
        {
            if (i == 0)
                return panelTickers;
            else if (i == 1)
                return panelCrypto2;
            else if (i == 2)
                return panelCrypto3;
            else if (i == 3)
                return panelCrypto4;
            else if (i == 4)
                return panelCrypto5;
            else if (i == 5)
                return panelCrypto6;
            else if (i == 6)
                return panelCrypto7;
            else
                return null;
        }*/

        private void AppendText(string text)
        {
            if (rtbConsoleOutput.InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendText), text);
            }
            else
            {
                const int MAX_TEXT_LENGTH = 65536;
                if (rtbConsoleOutput.TextLength > MAX_TEXT_LENGTH)
                {
                    rtbConsoleOutput.Clear();
                }
                rtbConsoleOutput.AppendText(text + "\n");
            }
        }

        private void G_COutput(MessageArgs e)
        {
            AppendText(e.Text);
        }

        private void listExchanges_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayExchangePrices(listExchanges.SelectedItem as string);
        }

        private void listSymbolIds_SelectedIndexChanged(object sender, EventArgs e)
        {
            string symbol = listSymbolIds.SelectedItem as string;
            cout(symbol);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new IQFeed.SymbolSearchForm()).Show();
            //ForceFXPriceRefresh();
            
            //Bitsquare.Test();
            //Coindesk.Test();
            //Changelly.Instance.Test();

            /*var btt = Bithumb.GetBithumbTicker();
            var btob = Bithumb.GetBithumbOrderBook();
            var btrt = Bithumb.GetBithumbRecentTransactions();

            //Kraken.Test();

            //CryptoAPIs.
            //var s = 

            ChartCoindeskBPI();

            MarketCapAnalysis();*/
        }

        private void SubscribeFXPrices()
        {
            foreach (string symbol in m_fxUpdates.Keys)
            {
                m_prices.SubscribePrices(symbol);
            }

            /*m_prices.SubscribePrices("@AD#");           // Australian Dollar
            m_prices.SubscribePrices("@BP#");           // British Pound
            m_prices.SubscribePrices("@JY#");           // Japanese Yen
            m_prices.SubscribePrices("@EU#");           // Euro FX*/
        }

        private void ForceFXPriceRefresh()
        {
            foreach (string symbol in m_fxUpdates.Keys)
            {
                m_prices.ForcePriceRefresh(symbol);
            }
        }

        private void prices_UpdatePrices(Tools.PriceUpdateIQ update)
        {
            if (!m_fxUpdates.Keys.Contains(update.Symbol)) return;

            if (update.Symbol == "@DX#")
            {
                UpdateCurrencyPrice(lblDX, string.Format("DX: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblDXPctChg, update.PercentChange);
            }
            else if (update.Symbol == "@EU#")
            {
                UpdateCurrencyPrice(lblEUR, string.Format("EUR: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblEURPctChg, update.PercentChange);
            }
            else if (update.Symbol == "@BP#")
            {
                UpdateCurrencyPrice(lblGBP, string.Format("GBP: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblGBPPctChg, update.PercentChange);
            }
            else if (update.Symbol == "@JY#")
            {
                UpdateCurrencyPrice(lblJPY, string.Format("JPY: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblJPYPctChg, update.PercentChange);
            }
            else if (update.Symbol == "@CD#")
            {
                UpdateCurrencyPrice(lblCAD, string.Format("CAD: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblCADPctChg, update.PercentChange);
            }
            else if (update.Symbol == "@AD#")
            {
                UpdateCurrencyPrice(lblAUD, string.Format("AUD: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblAUDPctChg, update.PercentChange);
            }
            else if (update.Symbol == "@RB#")
            {
                UpdateCurrencyPrice(lblCNY, string.Format("CNY: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblCNYPctChg, update.PercentChange);
            }
            else if (update.Symbol == "@ANE#")
            {
                UpdateCurrencyPrice(lblNZD, string.Format("NZD: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblNZDPctChg, update.PercentChange);
            }
            else if (update.Symbol == "@SF#")
            {
                UpdateCurrencyPrice(lblCHF, string.Format("CHF: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblCHFPctChg, update.PercentChange);
            }
            else if (update.Symbol == "@RU#")
            {
                UpdateCurrencyPrice(lblRUB, string.Format("RUB: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblRUBPctChg, update.PercentChange);
            }
            else if (update.Symbol == "@KRW#")
            {
                UpdateCurrencyPrice(lblKRW, string.Format("KRW: {0}", update.LastTradePrice));
                UpdateCurrencyPctChg(lblKRWPctChg, update.PercentChange);
            }
            else
                dout("Unknown currency symbol: {0}", update.Symbol);
            //dout("{0} {1}:{2}-{3}:{4}", update.Symbol, update.BidSize, update.Bid, update.Ask, update.AskSize);
        }

        private void UpdateCurrencyPrice(Label lbl, string text)
        {
            if (lbl.InvokeRequired) { this.Invoke(new Action<Label, string>(UpdateCurrencyPrice), lbl, text); }
            else
            {
                lbl.Text = text;
            }
        }

        private void UpdateCurrencyPctChg(Label lbl, double pctChg)
        {
            if (lbl.InvokeRequired) { this.Invoke(new Action<Label, double>(UpdateCurrencyPctChg), lbl, pctChg); }
            else
            {
                lbl.Text = string.Format("{0}%", pctChg);
                if (pctChg > 0)
                    lbl.ForeColor = Color.Green;
                else if (pctChg < 0)
                    lbl.ForeColor = Color.Red;
                else
                    lbl.ForeColor = Color.Black;
            }
        }

        private void CryptoPricesForm_Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void ChartCoindeskBPI()
        {
            DateTime dt1 = new DateTime(2017, 1, 1);
            DateTime dt2 = DateTime.Now;
            var bpiHistorical = Coindesk.GetBPIHistorical(dt1, dt2);
            dout(Str(bpiHistorical));
            chart.Series[0].Name = "Coindesk BPI";
            this.chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
            this.chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
            foreach (string sDate in bpiHistorical.bpi.Keys)
            {
                chart.Series[0].Points.AddXY(sDate, bpiHistorical.bpi[sDate]);
            }
        }

        // Prime Weighted Index
        private float MarketCapAnalysis()
        {
            var tickers = CoinMarketCap.GetTickers(20);
            var elements = from t in tickers
                           select (long)float.Parse(t.market_cap_usd);
            long marketCapSum = elements.Sum();

            var mcPercentage = new Dictionary<string, float>();
            float indexValue = 0.0f;
            foreach (var t in tickers)
            {
                long mc = (long)float.Parse(t.market_cap_usd);
                mcPercentage[t.symbol] = mc / (float)marketCapSum;
                float contribution = mcPercentage[t.symbol] * float.Parse(t.price_usd);
                indexValue += contribution;
                dout("{0,-5} ({1,5:0.00}%)  index_value:${2,15:n0}  market_cap:${3,15:n0}  price:${4,8:0.00}  contribution_to_index:${5,8:0.00}", t.symbol, mcPercentage[t.symbol] * 100, marketCapSum, (long)float.Parse(t.market_cap_usd), float.Parse(t.price_usd), contribution);
            }
            dout("TOTAL INDEX VALUE: $ {0:0.00}", indexValue);
            return indexValue;
        }

    } // end of class


    class MyLogger : System.IO.TextWriter
    {
        private RichTextBox m_rtb;
        private Form m_parentForm;
        private Form parentForm { get { return m_parentForm ?? (m_parentForm = m_rtb.FindForm()); } }
        public MyLogger(RichTextBox rtb) { m_rtb = rtb; }
        public override Encoding Encoding { get { return null; } }
        public override void Write(char value)
        {
            if (value != '\r')
            {
                //rtb.AppendText(new string(value, 1));
                AppendText(new string(value, 1));
            }
        }

        public void AppendText(string text)
        {
            if (m_rtb.InvokeRequired)
            {
                parentForm.Invoke(new Action<string>(AppendText), text);
            }
            else
            {
                m_rtb.AppendText(text);
            }
        }
    }



} // end of namespace
