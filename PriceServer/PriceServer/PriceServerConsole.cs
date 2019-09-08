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
    public class PriceServerConsole
    {
        public delegate void UpdateDataHandler(string sMessage);    // delegate for updating the data display.        
        public delegate void UpdateControlsHandler();               // delegate for updating the controls

        private Dictionary<string, PriceRow> m_updatePriceRows = new Dictionary<string, PriceRow>();                // symbol -> PriceRow for that symbol
        private Dictionary<string, List<SpreadRow>> m_updateSpreadRows = new Dictionary<string, List<SpreadRow>>(); // symbol -> SpreadRow(s) for that symbol

        private string m_symbolsFile = "PrimeTrader_SYMBOLS.DF.csv";
        private string m_spreadsFile = "PrimeTrader_SPREADS.DF.csv";

        private IPricePublisher m_pubsub;

        private ConcurrentDictionary<string, PriceUpdate> m_prices = new ConcurrentDictionary<string, PriceUpdate>();
        private ConcurrentQueue<string> m_updates = new ConcurrentQueue<string>();

        public PriceServerConsole(string ip, int port)
        {
            //m_pubsub = new RedisIQFeed(ip, port);
            m_pubsub = new ZmqIQFeed();
            m_pubsub.StartPricePublisher(ip, port);
            //m_pubsub.SubscribePriceUpdates();
            //m_pubsub.OnSubscriberReceive += M_subscriber_Receive;

            //m_publisher = new ZMQPublisher();
            //m_subscriber = new ZMQSubscriber();
            //m_subscriberClientTask = Task.Factory.StartNew(() => m_subscriber.SubscriptionLoop());

            //Task.Run(() => DisplaySubs());

            // Load symbols and spread formulas (and subscribe to these symbols)
            ReloadSymbols();
            ReloadSpreads();
        }

        public void Spin(int secondsToSleep = 60)
        {
            for (; ;)
            {
                Thread.Sleep(secondsToSleep * 1000);
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

        private void StartPublishPriceUpdates(string symbol)
        {
            //m_publisher.PublishPriceUpdates(symbol);
            //m_subscriber.Subscribe(symbol, M_subscriber_Receive);
            m_pubsub.RequestPriceUpdates(symbol);
            //m_redis.SubscribePriceUpdates(symbol);
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
                StartPublishPriceUpdates(symbol);
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
                        StartPublishPriceUpdates(sym);
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


    } // end of class PriceServerConsole

} // end of namespace