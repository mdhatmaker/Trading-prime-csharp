using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Tools;
using GuiTools.Grid;
using CryptoAPIs.Exchange;
using static Tools.G;

namespace CryptoForms
{
    public partial class CryptoGatorForm : Form
    {
        List<OrderBookGridPanel> m_gridPanels = new List<OrderBookGridPanel>();

        private System.Timers.Timer m_aggregateTimer = new System.Timers.Timer();

        private Panel GetPanel(int i)
        {
            if (i == 0)
                return panelCrypto1;
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
        }

        public CryptoGatorForm()
        {
            bool success;
            string message;
            /*var li = BittrexMarket.GetList(out success, out message);
            var marketNames = new List<string>();
            foreach (var m in li)
                marketNames.Add(m.MarketName);*/

            //var trades = BittrexMarketHistoryTrade.GetList("BTC-DOGE", out success, out message);

            InitializeComponent();

            OrderBookGridPanel gridPanel;

            //string result = Bitfinex.Instance.Authenticate().Result;
            //return;

            gridPanel = new OrderBookGridPanel(GetPanel(0), "GATOR", ColorPalette(1, 0));
            gridPanel.Initialize();
            m_gridPanels.Add(gridPanel);


            // GDAX: WORKS!!!
            GDAX.Instance.StartWebSocket();
            string[] args = { @"""type"":""subscribe""", @"""product_ids"":[""btc-usd""]", @"""channels"":[""level2""]" };
            GDAX.Instance.SubscribeWebSocket(args);
            gridPanel = new OrderBookGridPanel(GetPanel(1), "GDAX", ColorPalette(1, 1));
            gridPanel.Initialize();
            GDAX.Instance.UpdateOrderBookEvent += gridPanel.Handle_UpdateOrderBook;
            m_gridPanels.Add(gridPanel);

            // BitMEX: WORKS!!!
            BitMEX.Instance.StartWebSocket();
            BitMEX.Instance.SubscribeWebSocket();
            gridPanel = new OrderBookGridPanel(GetPanel(2), "BitMEX", ColorPalette(1, 2));
            gridPanel.Initialize();
            BitMEX.Instance.UpdateOrderBookEvent += gridPanel.Handle_UpdateOrderBook;
            m_gridPanels.Add(gridPanel);

            // HitBTC: WORKS!!!
            HitBTC.Instance.StartWebSocket();
            HitBTC.Instance.SubscribeWebSocket();
            gridPanel = new OrderBookGridPanel(GetPanel(3), "HitBTC", ColorPalette(1, 3));
            gridPanel.Initialize();
            HitBTC.Instance.UpdateOrderBookEvent += gridPanel.Handle_UpdateOrderBook;
            m_gridPanels.Add(gridPanel);

            // Bitfinex: WORKS!!!
            Bitfinex.Instance.StartWebSocket();
            Bitfinex.Instance.SubscribeWebSocket();
            gridPanel = new OrderBookGridPanel(GetPanel(4), "Bitfinex", ColorPalette(1, 4));
            gridPanel.Initialize();
            Bitfinex.Instance.UpdateOrderBookEvent += gridPanel.Handle_UpdateOrderBook;
            m_gridPanels.Add(gridPanel);

            // Gemini: WORKS!!!
            Gemini.Instance.StartWebSocket(new string[] { "BTCUSD" });
            Gemini.Instance.SubscribeWebSocket();
            gridPanel = new OrderBookGridPanel(GetPanel(5), "Gemini", ColorPalette(1, 5));
            gridPanel.Initialize();
            Gemini.Instance.UpdateOrderBookEvent += gridPanel.Handle_UpdateOrderBook;
            m_gridPanels.Add(gridPanel);

            // Cex: WORKS!!! ... uh, cannot connect to socket?!?
            /*Cex.Instance.StartWebSocket();
            Cex.Instance.SubscribeWebSocket();
            gridPanel = new OrderBookGridPanel(GetPanel(6), "Cex", ColorPalette(1, 6));
            gridPanel.Initialize();
            Cex.Instance.UpdateOrderBookEvent += gridPanel.Handle_UpdateOrderBook;
            m_gridPanels.Add(gridPanel);*/



            //var markets_list = new List<string> { "BTC-LTC", "BTC-USD", "BTC-DASH", "BTC-ETH", "BTC-XMY", "BTC-GLD", "ETH-ANT", "ETH-LTC" };

            


            m_aggregateTimer.Elapsed += aggregateTimer_Elapsed;
            m_aggregateTimer.Interval = 1000;
            m_aggregateTimer.Enabled = true;


            /*gridPanel = new TraderMarketGridPanel(panelCrypto2, markets_list[1], Color.DodgerBlue, 20000);
            gridPanel.Initialize();
            gridPanel.InitializeColumns(BittrexTraderMarket.Columns, Color.White);
            //gridPanel.UpdateListFunction = UpdateBitstampTicker;
            m_gridPanels.Add(gridPanel);*/

            /*
            gridPanel = new SmallGridPanel(panelCrypto2, "BlockchainInfoTicker", Color.Green, 3000);
            gridPanel.Initialize();
            gridPanel.InitializeColumns(BlockchainInfoTicker.Columns, Color.White);
            gridPanel.UpdateDictionaryFunction = UpdateBlockchainInfoTicker;
            m_gridPanels.Add(gridPanel);

            gridPanel = new SmallGridPanel(panelCrypto3, "CoinMarketCapTicker", Color.DodgerBlue, 15000);
            gridPanel.Initialize();
            gridPanel.InitializeColumns(CoinMarketCapTicker.Columns, Color.White);
            gridPanel.UpdateListFunction = UpdateCoinMarketCapTicker;
            m_gridPanels.Add(gridPanel);

            gridPanel = new SmallGridPanel(panelCrypto4, "BitCoinChartsWeightedPrices", Color.Red, 5000);
            gridPanel.Initialize();
            gridPanel.InitializeColumns(BitcoinChartsWeightedPrices.Columns, Color.White);
            gridPanel.UpdateDictionaryFunction = UpdateBitcoinChartsWeightedPrices;
            m_gridPanels.Add(gridPanel);*/

            //m_gridPanels[0].EnableUpdates(true);
            //m_gridPanels[1].EnableUpdates(true);
            //m_gridPanels[2].EnableUpdates(true);
            //m_gridPanels[3].EnableUpdates(true);
        }

        private ZOrderBook m_orderBook = new ZOrderBook();
        private void aggregateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_aggregateTimer.Enabled = false;

            //dout("AGGREGATING ORDER BOOKS...");
            m_orderBook.Clear();
            m_orderBook.AggregateRows(GDAX.Instance.OrderBook);
            m_orderBook.AggregateRows(BitMEX.Instance.OrderBook);
            m_orderBook.AggregateRows(HitBTC.Instance.OrderBook);
            m_orderBook.AggregateRows(Bitfinex.Instance.OrderBook);
            m_orderBook.AggregateRows(Gemini.Instance.OrderBook);

            m_gridPanels[0].Handle_UpdateOrderBook(this, new OrderBookUpdateArgs(m_orderBook.GetRows()));
            //dout("DONE AGGREGATING.");

            //SortedDictionary<float, IDataRow> rows = new SortedDictionary<float, IDataRow>();
            //var bestAsks = numericRows.Keys.Take(Math.Min(LEVEL_COUNT, numericRows. m_asks.Count));
            //var bestBids = m_bids.Keys.Reverse().Take(Math.Min(LevelCount, m_bids.Count));

            m_aggregateTimer.Enabled = true;            
        }

        private void CryptoTradeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

    } // end of CLASS
} // end of NAMESPACE
