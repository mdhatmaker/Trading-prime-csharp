//-----------------------------------------------------------
//-----------------------------------------------------------
//
//             System: IQFeed
//       Program Name: Level1Socket_VC#.exe
//        Module Name: Level1SocketForm.cs
//
//-----------------------------------------------------------
//
//            Proprietary Software Product
//
//                    Telvent DTN
//           9110 West Dodge Road Suite 200
//               Omaha, Nebraska  68114
//
//          Copyright (c) by Schneider Electric 2015
//                 All Rights Reserved
//
//
//-----------------------------------------------------------
// Module Description: Implementation of Level 1 Streaming Quotes
//         References: None
//           Compiler: Microsoft Visual Studio Version 2010
//             Author: Steven Schumacher
//        Modified By: 
//
//-----------------------------------------------------------
//-----------------------------------------------------------
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
// added for access to RegistryKey
using Microsoft.Win32;
// added for access to socket classes
using System.Net;
using System.Net.Sockets;
using IQ_Config_Namespace;
using System.IO;
using GuiTools.Grid;
using Tools;
using static Tools.G;
using static Tools.GFile;
using static Tools.GSystem;

namespace IQFeed
{
    public partial class PricesForm : Form
    {
        //global::IQFeed.PriceFeed m_prices;
        PriceFeed m_prices;

        public delegate void UpdateDataHandler(string sMessage);    // delegate for updating the data display.        
        public delegate void UpdateControlsHandler();               // delegate for updating the controls

        Dictionary<string, PriceRow> m_updatePriceRows = new Dictionary<string, PriceRow>();                // symbol -> PriceRow for that symbol
        Dictionary<string, List<SpreadRow>> m_updateSpreadRows = new Dictionary<string, List<SpreadRow>>(); // symbol -> SpreadRow(s) for that symbol

        string m_symbolsFile = "PrimeTrader_SYMBOLS.DF.csv";
        string m_spreadsFile = "PrimeTrader_SPREADS.DF.csv";

        DynamicDisplayGrid m_level1Grid;
        DynamicDisplayGrid m_spreadGrid;

        /// <summary>
        /// Constructor for the form
        /// </summary>
        public PricesForm()
        {
            InitializeComponent();

            //m_prices = global::IQFeed.PriceFeed.Instance;
            m_prices = PriceFeed.Instance;
            m_prices.UpdatePrices += M_prices_UpdatePrices;

            m_level1Grid = new DynamicDisplayGrid(gridLevel1, this);
            m_spreadGrid = new DynamicDisplayGrid(gridSpreads, this);

            m_level1Grid.InitializeColumns(PriceRow.Columns, Color.White);
            m_spreadGrid.InitializeColumns(SpreadRow.Columns, Color.White);

            m_spreadGrid.Columns["Formula"].MinimumWidth = 300;
            m_spreadGrid.Columns["Formula"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void M_prices_UpdatePrices(PriceUpdateIQ update)
        {
            // Update messages are sent to the client anytime one of the fields in the current fieldset are updated.
            //var x = "Q,@ESU17,2463.50,2,08:26:54.256829,43,170391,2463.25,116,2463.50,159,2460.25,2463.75,2456.50,2459.75,a,01,";

            if (!m_updatePriceRows.ContainsKey(update.Symbol)) return;

            var row = m_updatePriceRows[update.Symbol];
            row.UpdateValues(update);

            Console.WriteLine("CHANGE the NotifyGrids Property method of updating grids!");
            /*// Price Update to Level 1 grid
            if (row.NotifyGrids.Contains(m_level1Grid))
                m_level1Grid.UpdateRow(row);

            // Price Update to Spread grid
            if (row.NotifyGrids.Contains(m_spreadGrid))
                UpdateSpreads(row);*/
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls and initializing the connection to IQFeed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Level1SocketForm_Load(object sender, EventArgs e)
        {
            //m_appPath = Path.GetDirectoryName(Application.ExecutablePath);

            ReloadSymbols();
            ReloadSpreads();
        }

        // Update the right-most ToolStripLabel in the status bar
        private void UpdateStatusRight(string sData)
        {
            //Console.WriteLine(sData.Trim());
            if (this.statusStrip1.InvokeRequired)
                this.statusStrip1.Invoke(new MethodInvoker(() => this.statusLabel2.Text = sData.Trim()));
            else
                statusLabel2.Text = sData.Trim();
            /*if (this.status.InvokeRequired)
                this.status.Invoke(new MethodInvoker(() => this.tslblMain.Text = sData.Trim()));
            else
                this.tslblMain.Text = sData.Trim();*/
        }

        #region UI Button Click handlers
        /// <summary>
        /// Event that fires when the Watch Button is pressed.  Sends the watch command to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWatch_Click(object sender, EventArgs e)
        {
            // When you watch a symbol, you will get a snapshot of data that is currently available in the servers
            // for that symbol.  The snapshot will include a Fundamental message followed by a Summary message and then
            // you will continue to get Update messages anytime a field is updated until you issue an unwatch for the symbol.

            m_prices.SubscribePrices(txtRequest.Text);
        }
        #endregion


        #region Read/Write Files (symbols file, spreads file, etc.) --------------------------------------------------------------------------------------------
        private DataFrame dfReadSymbols()
        {
            string symbols_pathname = Folders.system_path(m_symbolsFile);
            var df = DataFrame.ReadDataFrame(symbols_pathname, createIndex: true);
            return df;
        }

        private void WriteSymbolsFile()
        {
            throw new NotImplementedException();
            string symbols_pathname = Folders.system_path(m_symbolsFile);
            m_level1Grid.WriteCsvFile(symbols_pathname, 1);                         // write first column only to file            
        }

        private DataFrame dfReadSpreads()
        {
            string spreads_pathname = Folders.system_path(m_spreadsFile);
            var df = DataFrame.ReadDataFrame(spreads_pathname, createIndex: true);
            return df;
        }

        private void WriteSpreadsFile()
        {
            throw new NotImplementedException();
            string spreads_pathname = Folders.system_path(m_spreadsFile);
            m_spreadGrid.WriteCsvFile(spreads_pathname);
        }
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------

        private void Level1SocketForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO: Notify the user if the symbols (and/or spreads) file has changed and allow them the option to save
            //WriteSymbolsFile();
            this.Hide();
            e.Cancel = true;
        }

        private void txtRequest_KeyDown(object sender, KeyEventArgs e)
        {
            // Default to WATCH symbol typed in text box if ENTER key is pressed
            if (e.KeyCode == Keys.Enter)
                m_prices.SubscribePrices(txtRequest.Text);
        }

        // TODO: WHAT IF WE SUBSCRIBE TO A PRICE FIRST IN THE SPREAD GRID? WE NEED TO CHECK FOR EXISTING m_updatePriceRows ENTRY AND USE IF EXISTS
        private void AddPriceRow(string symbol)
        {
            if (m_updatePriceRows.ContainsKey(symbol))      // if the symbol is already "subscribed"...
            {
                var row = m_updatePriceRows[symbol];        // use the existing PriceRow
                //row.AddNotifyGrid(m_level1Grid);            // add m_level1Grid to the grids that should be notified of updates
                m_level1Grid.AddRow(row);                   // display the row in the "Price" grid
            }
            else
            {
                var row = new PriceRow(symbol);             // create a new PriceRow for this symbol
                //row.AddNotifyGrid(m_level1Grid);
                m_updatePriceRows[symbol] = row;
                m_level1Grid.AddRow(row);
                m_prices.SubscribePrices(symbol);
            }
        }

        private void AddSpreadRow(string symbol, SpreadRow spreadRow)
        {
            if (!m_updateSpreadRows.ContainsKey(symbol))
            {
                m_updateSpreadRows[symbol] = new List<SpreadRow>() { spreadRow };
            }
            else
            {
                m_updateSpreadRows[symbol].Add(spreadRow);
            }
        }

        private void UpdateSpreads(PriceRow row)
        {
            //cout("UPDATE SPREADS: {0}", row.Symbol);
            if (m_updateSpreadRows.ContainsKey(row.Symbol))
            {
                var li = m_updateSpreadRows[row.Symbol];
                foreach (var spreadRow in li)
                {
                    spreadRow.SymbolUpdate(row);
                    m_spreadGrid.UpdateRow(spreadRow);
                }
            }
        }

        // Reload the SYMBOLS file into the "Prices" grid
        private void ReloadSymbols()
        {
            m_level1Grid.ClearRows();

            var df = dfReadSymbols();
            // If our symbols text file exists, then read it and request price updates on the symbols in the file
            foreach (var row in df.Rows)
            {
                //AddPriceRow(row["Symbol"]);
                //string group = row[0];
                AddPriceRow(row[1]);
            }
        }

        // Reload the SPREADS file into the "Spreads" grid
        private void ReloadSpreads()
        {
            m_spreadGrid.ClearRows();

            // If our symbols text file exists, then read it and request price updates on the symbols in the file
            var df = dfReadSpreads();
            foreach (var row in df.Rows)
            {
                //string group = row[0];
                string symbol = row[1];
                string formula = row[8];
                var spreadRow = new SpreadRow(symbol, formula);
                m_spreadGrid.AddRow(spreadRow);

                foreach (var sym in spreadRow.FormulaSymbols)
                {
                    cout("Formula Symbol: {0}", sym);
                    if (m_updatePriceRows.ContainsKey(sym))
                    {
                        var priceRow = m_updatePriceRows[sym];
                        //priceRow.AddNotifyGrid(m_spreadGrid);
                        AddSpreadRow(sym, spreadRow);
                    }
                    else
                    {
                        var priceRow = new PriceRow(sym);
                        //priceRow.AddNotifyGrid(m_spreadGrid);
                        m_updatePriceRows[sym] = priceRow;
                        AddSpreadRow(sym, spreadRow);
                        //m_level1Grid.AddRow(row);
                        m_prices.SubscribePrices(sym);
                    }
                }

                //SubscribePrices(symbol1);
                //SubscribePrices(symbol2);
            }
        }

        private void btnEditSymbolsFile_Click(object sender, EventArgs e)
        {
            string symbols_pathname = Folders.system_path(m_symbolsFile);
            ProcessStart("notepad.exe", symbols_pathname);
        }

        private void btnEditSpreadsFile_Click(object sender, EventArgs e)
        {
            string spreads_pathname = Folders.system_path(m_spreadsFile);
            ProcessStart("notepad.exe", spreads_pathname);
        }

        private void btnReloadSymbols_Click(object sender, EventArgs e)
        {
            ReloadSymbols();
            ReloadSpreads();
        }

        private void gridLevel1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int ri = e.RowIndex, ci = e.ColumnIndex;
            // Double-click on symbol:
            if (ci == 0)
            {
                Console.WriteLine(gridLevel1[ci, ri].Value);
            }
        }

        private void gridLevel1_MouseClick(object sender, MouseEventArgs e)
        {
            string buttonStr = "LEFT";
            if (e.Button == MouseButtons.Middle)
                buttonStr = "MIDDLE";
            else if (e.Button == MouseButtons.Right)
                buttonStr = "RIGHT";
            var hitTest = m_level1Grid.Grid.HitTest(e.X, e.Y);
            int ri = hitTest.RowIndex;
            int ci = hitTest.ColumnIndex;
            Console.WriteLine("MOUSE CLICK: {0} {1}   {2}", ci, ri, buttonStr);
            if (buttonStr == "RIGHT")
            {
                m_level1Grid.Grid.Rows[ri].Selected = true;
                gridContextMenu.Show(m_level1Grid.Grid, e.X, e.Y);
            }
        }

        private void deleteRowContextMenuItem_Click(object sender, EventArgs e)
        {
            m_level1Grid.Grid.Rows.Remove(m_level1Grid.Grid.SelectedRows[0]);
            m_level1Grid.DeselectAllRows();
            WriteSymbolsFile();
        }

        private void cancelContextMenuItem_Click(object sender, EventArgs e)
        {
            m_level1Grid.DeselectAllRows();
        }

    } // end of CLASS

} // end of NAMESPACE