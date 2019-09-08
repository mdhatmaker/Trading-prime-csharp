using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryptoAPIs;
using GuiTools.Grid;
using Tools;
using static Tools.G;

namespace CryptoForms
{
    public partial class CryptoAlgoForm : Form
    {
        Dictionary<int, List<AlgoArgument>> m_algoArgs = new Dictionary<int, List<AlgoArgument>>();

        private TickerGridPanel m_algoSummaryPanel;
        private TickerGridPanel m_orderGridPanel;
        private TickerGridPanel m_tradeGridPanel;

        public CryptoAlgoForm()
        {
            InitializeComponent();

            ReadAlgoArguments();

            string[] atsColumns = { "Instance Name", "Template", "StartTime", "Trades", "Profit", "Status" };
            m_algoSummaryPanel = new TickerGridPanel(panelCrypto1, "Automated Trade Summary", Color.Gray, atsColumns);
            m_algoSummaryPanel.Initialize();
            
            m_orderGridPanel = new TickerGridPanel(panelCrypto2, "Orders", Color.Gray, ZOrder.Columns);
            m_orderGridPanel.Initialize();
            m_orderGridPanel.Grid.CellClick += OrderGrid_CellClick;
            //m_gridPanels.Add(m_orderGridPanel);

            m_tradeGridPanel = new TickerGridPanel(panelCrypto3, "Trades", Color.Gray, ZTrade.Columns);
            m_tradeGridPanel.Initialize();
            //m_gridPanels.Add(m_tradeGridPanel);

            PopulateGrids();
        }

        private void OrderGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cout("CLICK!!!");
        }

        private void PopulateGrids()
        {
            m_algoSummaryPanel.Grid.Rows.Add("NEO_ETH 15-minute MACD", "Binance MACD Crossover", "2018-Mar-6 5:15 PM", "17", "$2,475", "running");
            m_algoSummaryPanel.Grid.Rows.Add("BTC_USD 1-hour MACD", "Kraken MACD Crossover", "2018-Mar-4 5:27 PM", "8", "$3,730", "running");
            m_algoSummaryPanel.Grid.Rows.Add("Liquidate POE", "Modified Iceberg (VWAP)", "2018-Mar=5 10:32 AM", "21", "$72,650", "running");

            for (int ri = 0; ri < 3; ++ri)
            {
                m_algoSummaryPanel.Grid[0, ri].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                m_algoSummaryPanel.Grid[1, ri].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            m_orderGridPanel.Grid.Rows.Add("BINANCE", "POE_BTC", "Limit", "Buy", "0.000004164", "272", "0", "Active", "9:38 AM", "x");
            m_orderGridPanel.Grid.Rows.Add("BINANCE", "POE_BTC", "Limit", "Sell", "0.000004214", "4864", "0", "Active", "9:38 AM", "x");

            m_tradeGridPanel.Grid.Rows.Add("BINANCE", "POE_BTC", "Sell", "0.000004358", "3642", "6:31 PM");
            m_tradeGridPanel.Grid.Rows.Add("BINANCE", "POE_BTC", "Buy", "0.000004093", "166", "6:00 PM");
            m_tradeGridPanel.Grid.Rows.Add("KRAKEN", "BTC_USD", "Sell", "9271.50", "0.5", "5:43 PM");
            m_tradeGridPanel.Grid.Rows.Add("BINANCE", "NEO_ETH", "Buy", "0.129691", "15.0", "5:36 PM");
            m_tradeGridPanel.Grid.Rows.Add("BINANCE", "NEO_ETH", "Sell", "0.127143", "15.0", "12:43 PM");
            //m_orderGridPanel.Grid.Rows.Add()
        }

        private void ReadAlgoArguments()
        {
            //string pathname = Folders.misc_path("AlgoArguments.txt"); AlgoArguments.
            string pathname = @"C:\temp\data_folders\DROPBOX\alvin\data\MISC\algo_arguments.txt";
            using (var f = new StreamReader(pathname))
            {
                string line;
                while ((line = f.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var split = line.Split(',');
                    int algoIndex = int.Parse(split[0]);
                    string setting = split[1];
                    string defaultValue = split[2];
                    var arg = new AlgoArgument(algoIndex, setting, defaultValue);
                    if (!m_algoArgs.ContainsKey(algoIndex)) m_algoArgs[algoIndex] = new List<AlgoArgument>();
                    m_algoArgs[algoIndex].Add(arg);
                }
            }
        }

        private void rdoMain_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void listAlgos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listAlgos.SelectedIndex < 0) return;
            gridArgs.Rows.Clear();
            foreach (var arg in m_algoArgs[listAlgos.SelectedIndex])
            {
                gridArgs.Rows.Add(arg.Setting, arg.DefaultValue);
            }
        }


    } // end of class CryptoAlgoForm

    public class AlgoArgument
    {
        public int AlgoIndex { get; private set; }
        public string Setting { get; private set; }
        public string DefaultValue { get; private set; }
        public string Value { get; private set; }

        public AlgoArgument(int index, string setting, string defaultValue)
        {
            AlgoIndex = index;
            Setting = setting;
            DefaultValue = defaultValue;
        }
    } // end of class AlgoArgument

} // end of namespace
