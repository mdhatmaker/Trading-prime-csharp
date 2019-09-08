using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
// added for access to RegistryKey
using Microsoft.Win32;
// added for access to socket classes
using System.Net;
using System.Net.Sockets;
using IQ_Config_Namespace;
using System.IO;
//using GuiTools.Grid;
using Tools.Messaging;
using Tools;
using IQFeed;
using static Tools.G;
using static Tools.GFile;
using static Tools.GSystem;

namespace PriceServer
{
    public class PricesConsole
    {
        public delegate void UpdateDataHandler(string sMessage);    // delegate for updating the data display.        
        public delegate void UpdateControlsHandler();               // delegate for updating the controls

        private Dictionary<string, PriceRow> m_updatePriceRows = new Dictionary<string, PriceRow>();                // symbol -> PriceRow for that symbol
        private Dictionary<string, List<SpreadRow>> m_updateSpreadRows = new Dictionary<string, List<SpreadRow>>(); // symbol -> SpreadRow(s) for that symbol

        private string m_symbolsFile = "PrimeTrader_SYMBOLS.DF.csv";
        private string m_spreadsFile = "PrimeTrader_SPREADS.DF.csv";

        //private ZMQPublisher m_publisher;
        //private ZMQSubscriber m_subscriber;
        //private RedisIQFeed m_redis;
        private IPriceSubscriber m_pubsub;

        //private Task m_publisherServerTask;
        //private Task m_subscriberClientTask;

        private ConcurrentDictionary<string, PriceUpdate> m_prices = new ConcurrentDictionary<string, PriceUpdate>();
        private ConcurrentQueue<string> m_updates = new ConcurrentQueue<string>();

        private static readonly int MONTH_COUNT = 6;
        private static readonly int CONTANGO_COUNT = MONTH_COUNT - 1;
        private string[] m_vxMonth = new string[MONTH_COUNT];           // @VX months ("mYY" format)
        private decimal[] m_contango = new decimal[CONTANGO_COUNT];     // contango values for first 6 months
        private string[] m_contangoMonth = new string[CONTANGO_COUNT];  // contango months ("mYYmYY" format)
        //string m_contangoFrontMonth = "H18";                            // like "H18"

        private static string m_es = "@ESH18";
        private static string m_vix = "VIX.XO";

        private decimal m_vixDiscount;                                  // VIX.XO - @VX (front month)

        public PricesConsole(string ip, int port, bool publish = false)
        {
            m_es = "@ESH18";                                                // front-month ES symbol

            // Populate the VX and Contango month strings
            m_vxMonth[0] = "H18";                                           // front-month @VX ("mYY" only, not full symbol)
            for (int i = 1; i < MONTH_COUNT; ++i)
            {
                m_vxMonth[i] = GDate.AddMonths(m_vxMonth[i - 1], 1);
                m_contangoMonth[i - 1] = m_vxMonth[i - 1] + m_vxMonth[i];
            }

            //m_pubsub = new RedisIQFeed(ip, port);
            m_pubsub = new ZmqIQFeed();
            m_pubsub.StartPriceSubscriber(ip, port);
            m_pubsub.OnSubscriberReceive += M_subscriber_Receive;

            //m_publisher = new ZMQPublisher();
            //m_subscriber = new ZMQSubscriber();
            //m_subscriberClientTask = Task.Factory.StartNew(() => m_subscriber.SubscriptionLoop());

            Task.Run(() => DisplaySubs());
            Task.Run(() => DisplayVIXES());

            if (publish)
            {
                // Load symbols and spread formulas (and subscribe to these symbols)
                ReloadSymbols();
                ReloadSpreads();
            }
        }

        public void Spin(int secondsToSleep = 60)
        {
            for (; ; )
            {
                Thread.Sleep(secondsToSleep * 1000);
            }
        }

        private void DisplayVIXES(int sleepSeconds = 2)
        {
            for (;;)
            {
                var tes = GetTicker(m_es);
                if (!string.IsNullOrEmpty(tes)) cout(tes);
                /*var tvix = GetTicker(m_vix);
                if (!string.IsNullOrEmpty(tvix)) cout(tvix);*/
                PriceUpdate vix, vx;
                if (m_prices.TryGetValue("VIX.XO", out vix))
                {
                    cout("VIX: {0}", vix.LastTradePrice);
                }
                if (m_prices.TryGetValue("VIX.XO", out vix) && m_prices.TryGetValue(m_vxMonth[0], out vx))
                {
                    cout("VIX Discount: {0}", vix.LastTradePrice - vx.Mid);
                }
                IEnumerable<string> ctgos = Enumerable.Range(0, CONTANGO_COUNT).Select(i => string.Format("{0}={1}", m_contangoMonth[i], m_contango[i]));
                string str = string.Join("  ", ctgos);
                cout("Contango: {0}\n", str);

                Thread.Sleep(sleepSeconds * 1000);
            }
        }

        // Launch this method as a thread that will display the latest prices every 5 seconds
        private void DisplaySubs(int sleepSeconds = 10)
        {
            for (;;)
            {
                /*foreach (var k in m_prices.Keys)
                {
                    Console.WriteLine("{0}: {1}", k, m_prices[k].ToString());
                }
                Console.WriteLine("");*/

                var kvs = m_prices.Keys.Select(k => string.Format("{0}:{1}", k, m_prices[k].LastTradePrice));
                string str = string.Join(" ", kvs);
                dout("\nSUB <<  {0}\n", str);

                Thread.Sleep(sleepSeconds * 1000);
            }
        }

        //private PriceUpdate _update;
        private void M_subscriber_Receive(string sUpdate)
        {
            // Update messages are sent to the client anytime one of the fields in the current fieldset are updated.
            //var x = "Q,@ESU17,2463.50,2,08:26:54.256829,43,170391,2463.25,116,2463.50,159,2460.25,2463.75,2456.50,2459.75,a,01,";

            //m_updates.Enqueue(sUpdate);

            //cout(sUpdate);
            //return;

            var update = new PriceUpdate(sUpdate);
            //cout("{0} [{1}] {2}:{3}-{4}:{5}", _update.Symbol, _update.LastTradePrice, _update.BidSize, _update.Bid, _update.Ask, _update.AskSize);

            //if (!m_updatePriceRows.ContainsKey(_update.Symbol)) return;
            //var row = m_updatePriceRows[_update.Symbol];
            //row.UpdateValues(_update);

            m_prices[update.Symbol] = update;
            //cout("SUB << {0}", _update.ToString());

            if (update.Symbol.StartsWith("@VX"))
            {
                UpdateContango();
            }
            /*else if (update.Symbol.StartsWith("@ES"))
            {
                var tes = GetTicker(m_es);
                if (!string.IsNullOrEmpty(tes)) cout(tes);
            }
            else if (update.Symbol.StartsWith("VIX.XO"))
            {
                var tvix = GetTicker(m_vix);
                if (!string.IsNullOrEmpty(tvix)) cout(tvix);
                PriceUpdate vix, vx;
                if (m_prices.TryGetValue("VIX.XO", out vix) && m_prices.TryGetValue(m_vxMonth[0], out vx))
                {
                    cout("VIX Discount: {0}", vix.LastTradePrice - vx.Mid);
                }
            }*/
            //Main,RISK ind,,,,,,,[+< VIX.XO >][+<JYVIX.XO>] [+<GVZ.XO>] [+<TYX.XO>]
            /*else if (_update.Symbol.StartsWith("QHO") || _update.Symbol.StartsWith("GAS"))
            {
                
            }*/

            // Price Update to Level 1 grid
            //if (row.NotifyGrids.Contains(m_priceGrid))
            //    m_priceGrid.UpdateRow(row);

            // Price Update to Spread grid
            //if (row.NotifyGrids.Contains(m_spreadGrid))
            //    UpdateSpreads(row);
        }

        // Iterate through all contango values to see if the values have changed from those stored in m_contango[]
        private void UpdateContango()
        {
            PriceUpdate[] vx = new PriceUpdate[CONTANGO_COUNT];
            bool updated = false;

            for (int i = 0; i < CONTANGO_COUNT; ++i)
            {
                if (m_prices.TryGetValue("@VX" + m_vxMonth[i], out vx[i]) && m_prices.TryGetValue("@VX" + m_vxMonth[i+1], out vx[i+1]))
                {
                    var ctgo = GMath.Contango(vx[i].Mid, vx[i + 1].Mid);
                    if (ctgo != m_contango[i])
                    {
                        updated = true;
                        m_contango[i] = ctgo;
                    }
                }
            }

            if (updated)
            {
                var tes = GetTicker(m_es);
                if (!string.IsNullOrEmpty(tes)) cout(tes);
                /*var tvix = GetTicker(m_vix);
                if (!string.IsNullOrEmpty(tvix)) cout(tvix);*/
                PriceUpdate puvix, puvx;
                if (m_prices.TryGetValue("VIX.XO", out puvix))
                {
                    cout("VIX: {0}", puvix.LastTradePrice);
                }
                if (m_prices.TryGetValue("VIX.XO", out puvix) && m_prices.TryGetValue(m_vxMonth[0], out puvx))
                {
                    cout("VIX Discount: {0}", puvix.LastTradePrice - puvx.Mid);
                }
                IEnumerable<string> ctgos = Enumerable.Range(0, CONTANGO_COUNT).Select(i => string.Format("{0}={1}", m_contangoMonth[i], m_contango[i]));
                string str = string.Join("  ", ctgos);
                cout("Contango: {0}\n", str);
            }
        }

        private string GetTicker(string symbol)
        {
            PriceUpdate pu;
            if (m_prices.TryGetValue(symbol, out pu))
            {
                return pu.ToTickerString();
            }
            else
                return "";
        }

        /*private void PricesForm_Load(object sender, EventArgs e)
        {
            //m_appPath = Path.GetDirectoryName(Application.ExecutablePath);

            ReloadSymbols();
            ReloadSpreads();
        }*/

        // Update the right-most ToolStripLabel in the status bar
        private void UpdateStatusRight(string sData)
        {
            Console.WriteLine(sData.Trim());
            /*if (this.statusStrip1.InvokeRequired)
                this.statusStrip1.Invoke(new MethodInvoker(() => this.statusLabel2.Text = sData.Trim()));
            else
                statusLabel2.Text = sData.Trim();*/
        }

        private void SubscribePrices(string symbol)
        {
            //m_pubsub.RequestPriceUpdates(symbol);
        }

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
            //m_level1Grid.WriteCsvFile(symbols_pathname, 1);                         // write first column only to file            
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
            //m_spreadGrid.WriteCsvFile(spreads_pathname);
        }
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------


        // TODO: WHAT IF WE SUBSCRIBE TO A PRICE FIRST IN THE SPREAD GRID? WE NEED TO CHECK FOR EXISTING m_updatePriceRows ENTRY AND USE IF EXISTS
        /*private void AddPriceRow(string symbol)
        {
            if (m_updatePriceRows.ContainsKey(symbol))      // if the symbol is already "subscribed"...
            {
                var row = m_updatePriceRows[symbol];        // use the existing PriceRow
                row.AddNotifyGrid(m_priceGrid);            // add m_level1Grid to the grids that should be notified of updates
                m_priceGrid.AddRow(row);                   // display the row in the "Price" grid
            }
            else
            {
                var row = new PriceRow(symbol);             // create a new PriceRow for this symbol
                row.AddNotifyGrid(m_priceGrid);
                m_updatePriceRows[symbol] = row;
                m_priceGrid.AddRow(row);
                //m_prices.SubscribePrices(symbol);
            }
        }*/

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
                    //m_spreadGrid.UpdateRow(spreadRow);
                }
            }
        }

        // Reload the SYMBOLS file into the "Prices" grid
        private void ReloadSymbols()
        {
            //m_level1Grid.ClearRows();

            var df = dfReadSymbols();
            // If our symbols text file exists, then read it and request price updates on the symbols in the file
            foreach (var row in df.Rows)
            {
                string group = row[0];
                string symbol = row[1];
                //AddPriceRow(symbol);
                SubscribePrices(symbol);
                //AddPriceRow(row["Symbol"]);
                //string group = row[0];
                //AddPriceRow(row[1]);
            }
        }

        // Reload the SPREADS file into the "Spreads" grid
        private void ReloadSpreads()
        {
            //m_spreadGrid.ClearRows();

            // If our symbols text file exists, then read it and request price updates on the symbols in the file
            var df = dfReadSpreads();
            foreach (var row in df.Rows)
            {
                //string group = row[0];
                string symbol = row[1];
                string formula = row[8];
                var spreadRow = new SpreadRow(symbol, formula);
                //m_spreadGrid.AddRow(spreadRow);

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
                        SubscribePrices(sym);
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


    } // end of class PricesConsole

} // end of namespace