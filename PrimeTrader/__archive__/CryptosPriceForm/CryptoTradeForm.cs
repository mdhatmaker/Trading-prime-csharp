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
using GuiTools.Grid;
using CryptoAPIs;

namespace CryptosPriceForm
{
    public partial class CryptoTradeForm : Form
    {
        List<TraderMarketGridPanel> m_gridPanels = new List<TraderMarketGridPanel>();

        List<Color> ColorPalette1 = new List<Color> { Color.Cyan, Color.Green, Color.Orange, Color.DodgerBlue, Color.DarkGray, Color.Purple, Color.Brown, Color.Magenta, Color.Red, Color.Yellow };

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
            else if (i == 7)
                return panelCrypto8;
            else if (i == 8)
                return panelCrypto9;
            else
                return panelCrypto10;
        }

        public CryptoTradeForm()
        {
            bool success;
            string message;
            var li = BittrexMarket.GetList(out success, out message);
            var marketNames = new List<string>();
            foreach (var m in li)
                marketNames.Add(m.MarketName);

            //var trades = BittrexMarketHistoryTrade.GetList("BTC-DOGE", out success, out message);

            InitializeComponent();

            TraderMarketGridPanel gridPanel;

            var markets_list = new List<string> { "BTC-LTC", "BTC-USD", "BTC-DASH", "BTC-ETH", "BTC-XMY", "BTC-GLD", "ETH-ANT", "ETH-LTC" };


            for (int i = 0; i < markets_list.Count; ++i)
            {
                gridPanel = new TraderMarketGridPanel(GetPanel(i), markets_list[i], ColorPalette1[i], 20000);
                gridPanel.Initialize();
                gridPanel.InitializeColumns(BittrexTraderMarket.Columns, Color.White);
                //gridPanel.UpdateListFunction = UpdateBitstampTicker;
                m_gridPanels.Add(gridPanel);

            }

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

            m_gridPanels[0].EnableUpdates(true);
            m_gridPanels[1].EnableUpdates(true);
            //m_gridPanels[2].EnableUpdates(true);
            //m_gridPanels[3].EnableUpdates(true);
        }



        private void CryptoTradeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

    } // end of CLASS
} // end of NAMESPACE
