using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZeroSumAPI;
using static Tools.G;

namespace ZeroSumAPI_Test
{
    public partial class Test2Form : Form, TradingEngineCallbacks
    {
        TengineT4 m_ttapi;
        TradingEngine te;

        public Test2Form()
        {
            InitializeComponent();

            m_ttapi = new TengineT4();
            te = m_ttapi as TradingEngine;
            te.SetTradingEngineCallbacks(this);

            te.Startup();
        }

        private void TestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!te.IsShutdown)
            {
                e.Cancel = true;
                te.Shutdown();
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        #region TradingEngine Callbacks
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
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            cout("Creating instruments...");
            te.CreateInstrument(111, "@ESZ17");
            te.CreateInstrument(222, "@VXV17");
            te.CreateInstrument(333, "@VXX17");
            te.CreateInstrument(444, "@VXZ17");
            te.CreateInstrument(555, "M.CU3=LX");
            te.CreateInstrument(666, "QHOZ17");
            te.CreateInstrument(777, "GASZ17");

            cout("Subscribing to instruments...");
            te.Subscribe(111);
            te.Subscribe(222);
            te.Subscribe(333);
            te.Subscribe(444);
            te.Subscribe(555);
            te.Subscribe(666);
            te.Subscribe(777);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            uint iid = 111;

            // Submit some orders...
            cout("Submitting orders...");
            te.SubmitOrder(te.CreateOrder(iid, ZOrderSide.Buy, 251175, 30));
            te.SubmitOrder(te.CreateOrder(iid, ZOrderSide.Sell, 251900, 10));
            //te.SubmitOrder(te.CreateOrder(iid, Side.Sell, 5010, 7));
        }


    } // end of CLASS

} // end of NAMESPACE
