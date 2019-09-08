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
using TradingTechnologies.TTAPI;
using TradingTechnologies.TTAPI.WinFormsHelpers;
using ZeroSumAPI;
using static Tools.G;

namespace ZeroSumAPI_Test
{
    public partial class Test3Form : Form, TradingEngineCallbacks
    {
        private TengineTT4 m_api;
        private TradingEngine m_te;

        public Test3Form(frmPriceUpdate ttForm)
        {
            InitializeComponent();

            ttForm.Show();

            m_api = new TengineTT4(ttForm);
            m_te = m_api as TradingEngine;
            m_te.SetTradingEngineCallbacks(this);

            //te.Startup();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            //Start();

            try
            {
                this.cboProductType.DataSource = ProductType.AllAvailableValues.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void Fill(uint iid, uint oid, int price, uint qty)
        {
            cout("FILL iid={0} oid={1} price={2} qty={3}", iid, oid, price, qty);
        }

        public void Trade(uint iid, int price, uint qty)
        {
            cout("TRADE iid={0} price={1} qty={2}", iid, price, qty);
        }

        public void MarketUpdate(uint iid, ZPriceLevels bids, ZPriceLevels asks)
        {
            cout("MARKET_UPDATE iid={0}", iid);
            cout("[asks] " + asks.ToString());
            cout("[bids] " + bids.ToString());
        }

        public void Reject(uint iid, uint oid)
        {
            cout("REJECT iid={0} oid={1}", iid, oid);
        }

        private void TestForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!m_te.IsShutdown)
            {
                e.Cancel = true;
                m_te.Shutdown();
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private void CreateInstruments()
        {
            cout("Creating instruments...");
            m_te.CreateInstrument(111, "@ESZ17");
            m_te.CreateInstrument(222, "@VXV17");
            m_te.CreateInstrument(333, "@VXX17");
            m_te.CreateInstrument(444, "@VXZ17");
            m_te.CreateInstrument(555, "M.CU3=LX");

            cout("Subscribing to instruments...");
            m_te.Subscribe(111);
            m_te.Subscribe(222);
            m_te.Subscribe(333);
            m_te.Subscribe(444);
            m_te.Subscribe(555);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateInstruments();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            uint iid = 111;

            // Submit some orders...
            cout("Submitting orders...");
            m_te.SubmitOrder(m_te.CreateOrder(iid, ZOrderSide.Buy, 251175, 30));
            m_te.SubmitOrder(m_te.CreateOrder(iid, ZOrderSide.Sell, 251900, 10));
            //te.SubmitOrder(te.CreateOrder(iid, Side.Sell, 5010, 7));
        }

        #region Drag and Drop
        private void TestForm_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.HasInstrumentKeys())
                e.Effect = DragDropEffects.Copy;
        }

        private void TestForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.HasInstrumentKeys())
                //m_ttapi.FindInstrument(e.Data.GetInstrumentKeys());
                ;
        }
        #endregion

        private void UpdateStatusBar(string text)
        {
            Console.WriteLine("STATUS: " + text);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            UpdateStatusBar("Connecting to Instrument...");
            
            // Create a product key from the given values
            ProductKey productKey = new ProductKey(this.txtExchange.Text,
                                            (ProductType)this.cboProductType.SelectedItem,
                                            this.txtProduct.Text);

            InstrumentKey key = new InstrumentKey(productKey, this.txtContract.Text);

            //m_ttapi.FindInstrument(key);
        }



    } // end of CLASS

} // end of NAMESPACE
