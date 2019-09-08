using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Tools;
using GuiTools.Grid;
using static Tools.G;
using CryptoAPIs;
using CryptoAPIs.Exchange;
using CryptoAPIs.ExchangeX;

namespace CryptoForms
{
    
    public partial class CryptoInfoForm : Form
    {
        List<DynamicDisplayGridPanel> m_gridPanels = new List<DynamicDisplayGridPanel>();

        public CryptoInfoForm()
        {
            InitializeComponent();

            Crypto.InitializeExchanges();

            //Bittrex.Test();

            DynamicDisplayGridPanel gridPanel;

            gridPanel = new DynamicDisplayGridPanel(panelCrypto1, "CoinMarketCapTicker", Color.Gray, 15000);
            gridPanel.Initialize();
            gridPanel.InitializeColumns(CoinMarketCapTicker.Columns, Color.White);
            gridPanel.UpdateListFunction = UpdateCoinMarketCapTicker;
            m_gridPanels.Add(gridPanel);

            gridPanel = new DynamicDisplayGridPanel(panelCrypto2, "BlockchainInfoTicker", Color.Gray, 3000);
            gridPanel.Initialize();
            gridPanel.InitializeColumns(BlockchainInfoTicker.Columns, Color.White);
            gridPanel.UpdateDictionaryFunction = UpdateBlockchainInfoTicker;
            m_gridPanels.Add(gridPanel);

            gridPanel = new DynamicDisplayGridPanel(panelCrypto3, "BitCoinChartsWeightedPrices", Color.Gray, 5000);
            gridPanel.Initialize();
            gridPanel.InitializeColumns(BitcoinChartsWeightedPrices.Columns, Color.White);
            gridPanel.UpdateDictionaryFunction = UpdateBitcoinChartsWeightedPrices;
            m_gridPanels.Add(gridPanel);

            gridPanel = new DynamicDisplayGridPanel(panelCrypto4, "BitstampTicker", Color.Gray, 20000);
            gridPanel.Initialize();
            gridPanel.InitializeColumns(BitstampTicker.Columns, Color.White);
            gridPanel.UpdateListFunction = UpdateBitstampTicker;
            m_gridPanels.Add(gridPanel);

            m_gridPanels[0].EnableUpdates(true);
            m_gridPanels[1].EnableUpdates(true);
            m_gridPanels[2].EnableUpdates(true);
            m_gridPanels[3].EnableUpdates(true);
        }


        private List<IDataRow> UpdateBitstampTicker()
        {
            var li = BitstampTicker.GetList();
            List<IDataRow> li2 = new List<IDataRow>();
            foreach (var item in li)
            {
                li2.Add(item as IDataRow);
            }
            return li2;
        }

        private Dictionary<string, IDataRow> UpdateBlockchainInfoTicker()
        {
            var dict = BlockchainInfoTicker.GetDictionary();
            Dictionary<string, IDataRow> d2 = new Dictionary<string, IDataRow>();
            foreach (var k in dict.Keys)
            {
                d2.Add(k, dict[k] as IDataRow);
            }
            return d2;
        }

        private List<IDataRow> UpdateCoinMarketCapTicker()
        {
            var li = CoinMarketCapTicker.GetList();
            List<IDataRow> li2 = new List<IDataRow>();
            foreach (var item in li)
            {
                li2.Add(item as IDataRow);
            }
            return li2;
        }

        private Dictionary<string, IDataRow> UpdateBitcoinChartsWeightedPrices()
        {
            var dict = BitcoinChartsWeightedPrices.GetDictionary();
            Dictionary<string, IDataRow> d2 = new Dictionary<string, IDataRow>();
            foreach (var k in dict.Keys)
            {
                d2.Add(k, dict[k] as IDataRow);
            }
            return d2;
        }

        private void CryptoPricesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    } // end of CLASS


} // end of NAMESPACE
