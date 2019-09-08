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
using IQFeed;

namespace IQFeed.GUI
{
    public partial class PricesForm : Form
    {
        //global::IQFeed.PriceFeed m_prices;
        private IQFeed.PriceFeed m_prices;
        
        public delegate void UpdateDataHandler(string sMessage);    // delegate for updating the data display.        
        public delegate void UpdateControlsHandler();               // delegate for updating the controls

        Dictionary<string, PriceRow> m_priceRowForSymbol = new Dictionary<string, PriceRow>();                // symbol -> PriceRow for that symbol
        Dictionary<string, List<SpreadRow>> m_spreadRowsForSymbol = new Dictionary<string, List<SpreadRow>>(); // symbol -> SpreadRow(s) for that symbol

        // Which grid(s) should be notified for each symbol (priceGrid, spreadGrid, or both)
        Dictionary<string, HashSet<DynamicDisplayGrid>> m_notifyGrids = new Dictionary<string, HashSet<DynamicDisplayGrid>>();

        Dictionary<string, PriceUpdateIQ> m_latestPrices = new Dictionary<string, PriceUpdateIQ>();

        string m_symbolsFile = "PrimeTrader_SYMBOLS.DF.csv";
        string m_spreadsFile = "PrimeTrader_SPREADS.DF.csv";

        DynamicDisplayGrid m_priceGrid;
        DynamicDisplayGrid m_spreadGrid;

        /// <summary>
        /// Constructor for the form
        /// </summary>
        public PricesForm()
        {
            InitializeComponent();

            //m_prices = global::IQFeed.PriceFeed.Instance;
            m_prices = IQFeed.PriceFeed.Instance;
            m_prices.UpdatePrices += M_prices_UpdatePrices;

            m_priceGrid = new DynamicDisplayGrid(gridLevel1, this);
            m_spreadGrid = new DynamicDisplayGrid(gridSpreads, this);

            m_priceGrid.InitializeColumns(PriceRow.Columns, Color.White);
            m_spreadGrid.InitializeColumns(SpreadRow.Columns, Color.White);

            m_spreadGrid.Columns["Formula"].MinimumWidth = 300;
            m_spreadGrid.Columns["Formula"].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            m_priceGrid.Grid.CellFormatting += PriceGrid_CellFormatting;
            m_spreadGrid.Grid.CellFormatting += SpreadGrid_CellFormatting;
        }

        private const int COL_LAST = 1, COL_LASTSIZE = 2, COL_BIDSIZE = 3, COL_BID = 4, COL_ASK = 5, COL_ASKSIZE = 6;
        private void PriceGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int ri = e.RowIndex;
            int ci = e.ColumnIndex;

            decimal value;
            if (!decimal.TryParse(m_priceGrid.Grid[ci, ri].Value.ToString(), out value)) return;    // if can't parse as decimal, just return

            // Hide any invalid values by painting them in foreground-color white
            if (ci == COL_BID)
            {                
                e.CellStyle.ForeColor = (value == decimal.MinValue ? Color.White : Color.Blue);
            }
            else if (ci == COL_ASK)
            {
                e.CellStyle.ForeColor = (value == decimal.MinValue ? Color.White : Color.Red);
            }
            else if (ci == COL_BIDSIZE)
            {
                e.CellStyle.ForeColor = (value == 0 ? Color.White : Color.Blue);
            }
            else if (ci == COL_ASKSIZE)
            {
                e.CellStyle.ForeColor = (value == 0 ? Color.White : Color.Red);
            }
            else if (ci == COL_LASTSIZE)
            {
                e.CellStyle.ForeColor = (value == 0 ? Color.White : Color.Black);
            }
        }

        private void SpreadGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int ri = e.RowIndex;
            int ci = e.ColumnIndex;

            decimal value;
            if (!decimal.TryParse(m_spreadGrid.Grid[ci, ri].Value.ToString(), out value)) return;    // if can't parse as decimal, just return

            // Hide any invalid values by painting them in foreground-color white
            if (ci == COL_LAST)
            {
                e.CellStyle.BackColor = (value >= 0 ? Color.LightGreen : Color.LightCoral);
            }
        }

        private void M_prices_UpdatePrices(PriceUpdateIQ update)
        {
            // Update messages are sent to the client anytime one of the fields in the current fieldset are updated.
            //var x = "Q,@ESU17,2463.50,2,08:26:54.256829,43,170391,2463.25,116,2463.50,159,2460.25,2463.75,2456.50,2459.75,a,01,";

            if (!m_priceRowForSymbol.ContainsKey(update.Symbol)) return;

            m_latestPrices[update.Symbol] = update;

            var row = m_priceRowForSymbol[update.Symbol];
            row.UpdateValues(update);

            //Console.WriteLine("CHANGE the NotifyGrids Property method of updating grids!");
            // Price Update to Level 1 grid
            //if (row.NotifyGrids.Contains(m_level1Grid))
            if (m_notifyGrids[update.Symbol].Contains(m_priceGrid))
                m_priceGrid.UpdateRow(row);

            // Price Update to Spread grid
            //if (row.NotifyGrids.Contains(m_spreadGrid))
            if (m_notifyGrids[update.Symbol].Contains(m_spreadGrid))
                UpdateSpreads(row);
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls and initializing the connection to IQFeed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PricesForm_Load(object sender, EventArgs e)
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
            m_priceGrid.WriteCsvFile(symbols_pathname, 1);                         // write first column only to file            
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

        private void PricesForm_FormClosing(object sender, FormClosingEventArgs e)
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
            // Add the priceGrid to the list of grids associated with this symbol
            if (!m_notifyGrids.ContainsKey(symbol)) m_notifyGrids[symbol] = new HashSet<DynamicDisplayGrid>();
            m_notifyGrids[symbol].Add(m_priceGrid);

            // 
            if (m_priceRowForSymbol.ContainsKey(symbol))      // if the symbol is already "subscribed"...
            {
                var row = m_priceRowForSymbol[symbol];        // use the existing PriceRow
                //row.AddNotifyGrid(m_level1Grid);            // add m_level1Grid to the grids that should be notified of updates
                m_priceGrid.AddRow(row);                   // display the row in the "Price" grid
            }
            else
            {
                var row = new PriceRow(symbol);             // create a new PriceRow for this symbol
                //row.AddNotifyGrid(m_level1Grid);
                m_priceRowForSymbol[symbol] = row;
                m_priceGrid.AddRow(row);
                m_prices.SubscribePrices(symbol);
            }
        }

        private void AddSpreadRow(string symbol, SpreadRow spreadRow)
        {
            // Add the spreadGrid to the list of grids associated with this symbol
            if (!m_notifyGrids.ContainsKey(symbol)) m_notifyGrids[symbol] = new HashSet<DynamicDisplayGrid>();
            m_notifyGrids[symbol].Add(m_spreadGrid);

            // If this symbol does not have an entry in m_spreadRowsForSymbol dictionary, then create one
            if (!m_spreadRowsForSymbol.ContainsKey(symbol))
            {
                m_spreadRowsForSymbol[symbol] = new List<SpreadRow>() { spreadRow };
            }
            else
            {
                m_spreadRowsForSymbol[symbol].Add(spreadRow);
            }
        }

        private void UpdateSpreads(PriceRow row)
        {
            //cout("UPDATE SPREADS: {0}", row.Symbol);
            if (m_spreadRowsForSymbol.ContainsKey(row.Symbol))
            {
                var li = m_spreadRowsForSymbol[row.Symbol];
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
            m_priceGrid.ClearRows();

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
                    dout("Formula Symbol: {0}", sym);
                    if (m_priceRowForSymbol.ContainsKey(sym))
                    {
                        var priceRow = m_priceRowForSymbol[sym];
                        //priceRow.AddNotifyGrid(m_spreadGrid);
                        AddSpreadRow(sym, spreadRow);
                    }
                    else
                    {
                        var priceRow = new PriceRow(sym);
                        //priceRow.AddNotifyGrid(m_spreadGrid);
                        m_priceRowForSymbol[sym] = priceRow;
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
            var hitTest = m_priceGrid.Grid.HitTest(e.X, e.Y);
            int ri = hitTest.RowIndex;
            int ci = hitTest.ColumnIndex;
            Console.WriteLine("MOUSE CLICK: {0} {1}   {2}", ci, ri, buttonStr);
            if (buttonStr == "RIGHT")
            {
                m_priceGrid.Grid.Rows[ri].Selected = true;
                gridContextMenu.Show(m_priceGrid.Grid, e.X, e.Y);
            }
        }

        private void deleteRowContextMenuItem_Click(object sender, EventArgs e)
        {
            m_priceGrid.Grid.Rows.Remove(m_priceGrid.Grid.SelectedRows[0]);
            m_priceGrid.DeselectAllRows();
            WriteSymbolsFile();
        }

        private void cancelContextMenuItem_Click(object sender, EventArgs e)
        {
            m_priceGrid.DeselectAllRows();
        }

    } // end of CLASS

} // end of NAMESPACE