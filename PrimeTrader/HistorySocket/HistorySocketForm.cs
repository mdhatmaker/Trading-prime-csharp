//-----------------------------------------------------------
//-----------------------------------------------------------
//
//             System: IQFeed
//       Program Name: HistorySocket_VC#.exe
//        Module Name: HistorySocketForm.cs
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
// Module Description: Implementation of History via Sockets
//         References: None
//           Compiler: Microsoft Visual Studio Version 2010
//             Author: Steven Schumacher
//        Modified By: 
//
//-----------------------------------------------------------
//-----------------------------------------------------------
using System;
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
using System.Threading;
using System.Linq;
using System.IO;
using IQ_Config_Namespace;
using System.Diagnostics;
using Tools;
using static Tools.GDate;

namespace HistorySocket
{
    public partial class HistorySocketForm : Form
    {
        enum IntervalValues { Minute = 60, Hour = 3600 }

        Folders folders = Folders.DropboxFolders;

        public delegate void UpdateDataHandler(string sMessage);            // delegate for updating the data display

        HistorySocketThread m_historyThread = new HistorySocketThread();

        string m_protocol = "";

        /// <summary>
        /// Constructor for the form
        /// </summary>
        public HistorySocketForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls and initializing the connection to IQFeed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistorySocketForm_Load(object sender, EventArgs e)
        {
            //IQ_Config config = new IQ_Config();
            lstData.Columns.Add("Data received", -2);
            lstData.Columns[0].Width -= System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;

            // populate the request type dropdown   
            cboHistoryType.Items.Add("Tick Datapoints");
            cboHistoryType.Items.Add("Tick Days");
            cboHistoryType.Items.Add("Tick Timeframe");
            cboHistoryType.Items.Add("Interval Datapoints");
            cboHistoryType.Items.Add("Interval Days");
            cboHistoryType.Items.Add("Interval Timeframe");
            cboHistoryType.Items.Add("Daily Datapoints");
            cboHistoryType.Items.Add("Daily Timeframe");
            cboHistoryType.Items.Add("Weekly Datapoints");
            cboHistoryType.Items.Add("Monthly Datapoints");

            cboHistoryType.SelectedIndex = 7;

            UpdateAllDataLists();

            txtHistoricalFolder.Text = folders.df_folder;

            txtEndDateTime.Text = DateTime.Now.ToString("yyyyMMdd");

            cboFuturesInterval.SelectedIndex = 0;

            var tup = GetAllDataLists();
            var dataSymbols = tup.Item2;
            foreach (var txt in dataSymbols)
            {
                if (txt.Length <= 3)
                    cboSymbolRoot.Items.Add(txt);
                else
                    txtSymbol.Items.Add(txt);
            }

            var markets = RequestListedMarkets().ToArray();
            cboListedMarkets.Items.Clear();
            cboListedMarkets.Items.AddRange(markets);

            var sectypes = RequestSecurityTypes().ToArray();
            cboSecurityTypes.Items.Clear();
            cboSecurityTypes.Items.AddRange(sectypes);
            //cboSecurityTypes.SelectedIndex = 7;
        }

        // From the data download folder retrieve a list of all filenames that have the given root symbol and timeframe
        private List<string> GetDataFiles(string rootSymbol, string timeframe)
        {
            List<string> results = new List<string>();

            if (timeframe != null && rootSymbol != null)
            {
                string[] fileEntries = Directory.GetFiles(folders.df_folder);
                foreach (string pathName in fileEntries)
                {
                    string filename = Path.GetFileName(pathName);
                    if (GetRootSymbol(filename) == rootSymbol && GetTimeFrame(filename) == timeframe)
                        results.Add(filename);
                }
            }

            return results;
        }

        // Given a data filename, return the text preceding the timeframe (ex: text before "(Daily)")
        private string GetSymbol(string filename)
        {
            string tf = GetTimeFrame(filename);
            if (tf == null) return null;

            int ix = filename.IndexOf("(" + tf + ")");
            if (ix < 0) return null;

            return filename.Substring(0, ix).Trim();
        }

        // Given a data filename, retrieve the root symbol (ex: '@ES')
        private string GetRootSymbol(string filename)
        {
            string symbol = GetSymbol(filename);
            if (IsFutureSymbol(symbol))
                return filename.Substring(0, 3);
            else
                return symbol;
        }

        // Given a data filename, retrieve the timeframe (within the filename parentheses, ex: 'Daily')
        private string GetTimeFrame(string filename)
        {
            int i1 = filename.IndexOf('(');
            int i2 = -1;
            if (i1 > 0)
                i2 = filename.IndexOf(')', i1);

            string timeframe = null;
            if (i1 != -1 && i2 != -1)
                timeframe = filename.Substring(i1 + 1, i2 - i1 - 1);

            return timeframe;
        }

        private Tuple<List<string>,List<string>> GetAllDataLists()
        {
            var timeFrames = new List<string>();
            var dataSymbols = new List<string>();

            string[] fileEntries = Directory.GetFiles(folders.df_folder);
            foreach (string pathName in fileEntries)
            {
                string filename = Path.GetFileName(pathName);
                string timeframe = GetTimeFrame(filename);

                if (timeframe != null)
                {
                    if (!timeFrames.Contains(timeframe))
                        timeFrames.Add(timeframe);
                }

                string symbol = GetRootSymbol(filename);
                if (symbol == null) symbol = GetSymbol(filename);
                if (symbol != null && !dataSymbols.Contains(symbol))
                    dataSymbols.Add(symbol);
            }

            return new Tuple<List<string>, List<string>>(timeFrames, dataSymbols);
        }

        // Update the lists for "root symbol" and "time frame" (by examining files in the data download folder)
        private void UpdateAllDataLists()
        {
            var tup = GetAllDataLists();
            var timeFrames = tup.Item1;
            var dataSymbols = tup.Item2;

            listDataTimeFrames.Items.Clear();
            listDataTimeFrames.Items.AddRange(timeFrames.ToArray());
            listDataSymbols.Items.Clear();
            listDataSymbols.Items.AddRange(dataSymbols.ToArray());
        }

        // Based on selected root symbol and selected timeframe, update the filenames in the Data Files list
        private void UpdateDataFilesList()
        {
            ClearFileList();

            string timeframe = (string)listDataTimeFrames.SelectedItem;
            string rootSymbol = (string)listDataSymbols.SelectedItem;

            var files = GetDataFiles(rootSymbol, timeframe);
            DisplayFileList(files);
            //listDataFiles.Items.AddRange(files.ToArray());
        }

        /// <summary>
        /// We want to be able to update the winform status listbox from within the AsyncSocket Callback 
        /// so we need a delagate to resolve cross-threading issues.
        /// </summary>
        /// <param name="sData"></param>
        public void UpdateListview(string sData)
        {
            try
            {
                // check if we need to invoke the delegate
                if (lstData.InvokeRequired)
                {
                    // call this function again using a delegate
                    this.Invoke(new UpdateDataHandler(UpdateListview), sData);
                }
                else
                {
                    // delegate not required, just update the list box.
                    List<String> lstMessages = new List<string>(sData.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                    lstData.BeginUpdate();
                    lstMessages.ForEach(delegate (String sLine)
                    {
                        lstData.Items.Add(sLine);
                    });
                    lstData.Items[lstData.Items.Count - 1].EnsureVisible();
                    lstData.EndUpdate();

                    tssRight.Text = lstData.Items.Count.ToString() + " rows";
                }
            }
            catch (ObjectDisposedException)
            {
                // The list view object went away, ignore it since we're probably exiting.
            }
        }

        public void ClearListview()
        {
            lstData.Items.Clear();
            tssRight.Text = "0 rows";
        }

        /// <summary>
        /// Event handler for when the user clicks the GetFuturesContracts button.  It will iterate through the
        ///     selected years and submit appropriate IQFeed requests.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetFuturesContracts_Click(object sender, EventArgs e)
        {
            lstData.Items.Clear();
            Application.DoEvents();

            string symbol_root = cboSymbolRoot.Text.Trim().ToUpper();  // "QHO";

            int y1 = (int)numFirstYear.Value;
            int y2 = (int)numLastYear.Value;

            var today = DateTime.Now;
            //int mnow = today.Month;
            //int ynow = today.Year;
            //int mend = 3;
            //int yend = 2018;

            //GetHistory("QHO#C", 1);
            //GetHistory("QHOQ17", 1);
            //GetDailyHistory("QHO#C", new DateTime(2017, 1, 1), DateTime.Now);
            //GetIntervalHistory("QHOQ17", Interval.Hour, new DateTime(2017, 1, 1), DateTime.Now);
            //GetIntervalHistory("QHOQ17", Interval.Minute, new DateTime(2017, 7, 1), DateTime.Now);

            for (int y = y1; y <= y2; ++y)
            {
                for (int m = 1; m <= 12; ++m)
                {
                    //if (y >= yend && m > mend) break;

                    string symbol = symbol_root + GetmYY(m, y);

                    string interval = cboFuturesInterval.Text;

                    RequestHistoricalData(symbol, m, y, interval);
                }
            }

            UpdateListview("Done.");
        }

        /// <summary>
        /// Event handler for when the user clicks the GetContractData button.  It forms the request that will be sent to
        ///     IQFeed based upon user input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetContractData_Click(object sender, EventArgs e)
        {
            // clear previous request data
            string sRequest = "";

            // format request string based upon user input
            if (cboHistoryType.Text.Equals("Tick Datapoints"))
            {
                // request in the format:
                // HTX,SYMBOL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
                sRequest = String.Format("HTX,{0},{1},{2},{3},{4}\r\n", txtSymbol.Text, txtDatapoints.Text, txtDirection.Text, txtRequestID.Text, txtDatapointsPerSend.Text);
            }
            else if (cboHistoryType.Text.Equals("Tick Days"))
            {
                // request in the format:
                // HTD,SYMBOL,NUMDAYS,MAXDATAPOINTS,BEGINFILTERTIME,ENDFILTERTIME,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
                sRequest = String.Format("HTD,{0},{1},{2},{3},{4},{5},{6},{7}\r\n", txtSymbol.Text, txtDays.Text, txtDatapoints.Text, txtBeginFilterTime.Text, txtEndFilterTime.Text, txtDirection.Text, txtRequestID.Text, txtDatapointsPerSend.Text);
            }
            else if (cboHistoryType.Text.Equals("Tick Timeframe"))
            {
                // request in the format:
                // HTT,SYMBOL,BEGINDATE BEGINTIME,ENDDATE ENDTIME,MAXDATAPOINTS,BEGINFILTERTIME,ENDFILTERTIME,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
                sRequest = String.Format("HTT,{0},{1},{2},{3},{4},{5},{6},{7},{8}\r\n", txtSymbol.Text, txtBeginDateTime.Text, txtEndDateTime.Text, txtDatapoints.Text, txtBeginFilterTime.Text, txtEndFilterTime.Text, txtDirection.Text, txtRequestID.Text, txtDatapointsPerSend.Text);
            }
            else if (cboHistoryType.Text.Equals("Interval Datapoints"))
            {
                // validate interval type
                string sIntervalType = "s";
                if (rbVolume.Checked)
                {
                    sIntervalType = "v";
                }
                else if (rbTick.Checked)
                {
                    sIntervalType = "t";
                }

                // request in the format:
                // HIX,SYMBOL,INTERVAL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND,INTERVALTYPE<CR><LF>
                sRequest = String.Format("HIX,{0},{1},{2},{3},{4},{5},{6}\r\n", txtSymbol.Text, txtInterval.Text, txtDatapoints.Text, txtDirection.Text, txtRequestID.Text, txtDatapointsPerSend.Text, sIntervalType);
            }
            else if (cboHistoryType.Text.Equals("Interval Days"))
            {
                // validate interval type
                string sIntervalType = "s";
                if (rbVolume.Checked)
                {
                    sIntervalType = "v";
                }
                else if (rbTick.Checked)
                {
                    sIntervalType = "t";
                }

                // request in the format:
                // HID,SYMBOL,INTERVAL,NUMDAYS,MAXDATAPOINTS,BEGINFILTERTIME,ENDFILTERTIME,DIRECTION,REQUESTID,DATAPOINTSPERSEND,INTERVALTYPE<CR><LF>
                sRequest = String.Format("HID,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", txtSymbol.Text, txtInterval.Text, txtDays.Text, txtDatapoints.Text, txtBeginFilterTime.Text, txtEndFilterTime.Text, txtDirection.Text, txtRequestID.Text, txtDatapointsPerSend.Text, sIntervalType);
            }
            else if (cboHistoryType.Text.Equals("Interval Timeframe"))
            {
                string sIntervalType = GetIntervalType();

                // request in the format:
                // HIT,SYMBOL,INTERVAL,BEGINDATE BEGINTIME,ENDDATE ENDTIME,MAXDATAPOINTS,BEGINFILTERTIME,ENDFILTERTIME,DIRECTION,REQUESTID,DATAPOINTSPERSEND,INTERVALTYPE<CR><LF>
                sRequest = String.Format("HIT,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\r\n", txtSymbol.Text, txtInterval.Text, txtBeginDateTime.Text, txtEndDateTime.Text, txtDatapoints.Text, txtBeginFilterTime.Text, txtEndFilterTime.Text, txtDirection.Text, txtRequestID.Text, txtDatapointsPerSend.Text, sIntervalType);
            }
            else if (cboHistoryType.Text.Equals("Daily Datapoints"))
            {
                // request in the format:
                // HDX,SYMBOL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
                sRequest = String.Format("HDX,{0},{1},{2},{3},{4}\r\n", txtSymbol.Text, txtDatapoints.Text, txtDirection.Text, txtRequestID.Text, txtDatapointsPerSend.Text);
            }
            else if (cboHistoryType.Text.Equals("Daily Timeframe"))
            {
                // request in the format:
                // HDT,SYMBOL,BEGINDATE,ENDDATE,MAXDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
                sRequest = String.Format("HDT,{0},{1},{2},{3},{4},{5},{6}\r\n", txtSymbol.Text, txtBeginDateTime.Text, txtEndDateTime.Text, txtDatapoints.Text, txtDirection.Text, txtRequestID.Text, txtDatapointsPerSend.Text);
            }
            else if (cboHistoryType.Text.Equals("Weekly Datapoints"))
            {
                // request in the format:
                // HWX,SYMBOL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
                sRequest = String.Format("HDX,{0},{1},{2},{3},{4}\r\n", txtSymbol.Text, txtDatapoints.Text, txtDirection.Text, txtRequestID.Text, txtDatapointsPerSend.Text);
            }
            else if (cboHistoryType.Text.Equals("Monthly Datapoints"))
            {
                // request in the format:
                // HMX,SYMBOL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
                sRequest = String.Format("HDX,{0},{1},{2},{3},{4}\r\n", txtSymbol.Text, txtDatapoints.Text, txtDirection.Text, txtRequestID.Text, txtDatapointsPerSend.Text);
            }
            else
            {
                // something unexpected happened
                sRequest = "Error Processing Request.";
            }

            // verify we have formed a request string
            if (!sRequest.StartsWith("H"))
            {
                string sError = String.Format("{0}\r\nRequest type selected was: {1}", sRequest, cboHistoryType.Text);
                UpdateListview(sError);
                return;
            }

            string filename = GetContractFilename();
            SubmitIqFeedRequest(sRequest, filename);
        }

        private string GetDailyTimeframeRequest(string symbol, string beginDateTime, string endDateTime, string maxDatapoints="", string direction="", string requestId="", string datapointsPerSend="")
        {
            // request in the format:
            // HDT,SYMBOL,BEGINDATE,ENDDATE,MAXDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
            string sRequest = String.Format("HDT,{0},{1},{2},{3},{4},{5},{6}\r\n", symbol, beginDateTime, endDateTime, maxDatapoints, direction, requestId, datapointsPerSend);
            return sRequest;
        }

        // intervalType: "s" = time (seconds), "v" = volume, "t" = ticks
        private string GetIntervalTimeframeRequest(string symbol, string intervalType, string interval, string beginDateTime, string endDateTime, string maxDatapoints = "", string beginFilterTime="", string endFilterTime="", string direction = "", string requestId = "", string datapointsPerSend = "")
        {
            string sIntervalType = GetIntervalType();

            // request in the format:
            // HIT,SYMBOL,INTERVAL,BEGINDATE BEGINTIME,ENDDATE ENDTIME,MAXDATAPOINTS,BEGINFILTERTIME,ENDFILTERTIME,DIRECTION,REQUESTID,DATAPOINTSPERSEND,INTERVALTYPE<CR><LF>
            string sRequest = String.Format("HIT,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\r\n", symbol, interval, beginDateTime, endDateTime, maxDatapoints, beginFilterTime, endFilterTime, direction, requestId, datapointsPerSend, intervalType);
            return sRequest;
        }

        // Submit a request to IQFeed API and write the results to a file
        private void SubmitIqFeedRequest(string sRequest, string outputFilename)
        {
            display(sRequest);
            //Clipboard.SetText(sRequest);        // for debugging, copy the IQFeed request string to the clipboard
            SendRequest(sRequest);
            RemoveProtocolMessageFromReceived();

            UpdateListview(string.Format("{0}   count: {1}", txtSymbol.Text, m_historyThread.ReceivedData.Count));
            Application.DoEvents();

            // Store the retrieved data to a text file
            display(string.Format("Writing to file '{0}'...", outputFilename));
            List<string> data = m_historyThread.ReceivedData;
            WriteToFile(outputFilename, data, (int) numHourAdjustContract.Value);
            UpdateListview("Done.");
        }

        // Like method above except it does NOT write results to file (only displays them in GUI)
        // Returns a copy of the List<string> from the received data thread
        private List<string> SubmitIqFeedRequest(string sRequest)
        {
            display(sRequest);
            SendRequest(sRequest);
            RemoveProtocolMessageFromReceived();

            UpdateListview(string.Format("received count: {0}", m_historyThread.ReceivedData.Count));
            Application.DoEvents();

            List<string> data_list = new List<string>();
            foreach (string data in m_historyThread.ReceivedData)
            {
                UpdateListview(data);
                data_list.Add(data);
            }
            UpdateListview("Done.");

            return data_list;
        }

        // Return 's' (time), 'v' (volume) or 't' (ticks) for the currently selected interval type
        private string GetIntervalType()
        {
            // validate interval type
            string sIntervalType = "s";
            if (rbVolume.Checked)
            {
                sIntervalType = "v";
            }
            else if (rbTick.Checked)
            {
                sIntervalType = "t";
            }
            return sIntervalType;
        }

        // Generate a filename for the downloaded data based on the contract symbol and the type of data requested (Daily, etc.)
        private string GetContractFilename()
        {
            string filename = null;

            string symbol = txtSymbol.Text.Trim().ToUpper();
            string sIntervalType = GetIntervalType();

            string ext;
            if (chkDatesInFilename.Checked)
                ext = "." + txtBeginDateTime.Text.Trim() + "-" + txtEndDateTime.Text.Trim() + ".csv";
            else
                ext = ".csv";

            if (cboHistoryType.SelectedItem.ToString().StartsWith("Daily"))
            {
                filename = symbol + " (Daily)" + ext;
            }
            else if (cboHistoryType.SelectedItem.ToString().StartsWith("Interval") && (sIntervalType == "s" && txtInterval.Text == "60"))
            {
                filename = symbol + " (1 Minute)" + ext;
            }
            else if (cboHistoryType.SelectedItem.ToString().StartsWith("Interval") && (sIntervalType == "s" && txtInterval.Text == "3600"))
            {
                filename = symbol + " (1 Hour)" + ext;
            }
            else
            {
                System.Diagnostics.Debugger.Break();
            }
            return filename;
        }

        /// <summary>
        /// Handles when the user changes the data in the request type combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboHistoryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // grab the text from the newly selected value
            string sSelection = ((ComboBox)sender).Text;

            // disable all the controls.  we will re-enable them depending on the request type selected
            DisableForm();

            // enable controls that are used for every request
            lstData.Enabled = true;
            btnGetData.Enabled = true;
            txtSymbol.Enabled = true;
            cboHistoryType.Enabled = true;
            txtRequestID.Enabled = true;
            txtDirection.Enabled = true;
            txtDatapointsPerSend.Enabled = true;
            txtDatapoints.Enabled = true;
            rbTick.Enabled = false;
            rbTime.Enabled = false;
            rbVolume.Enabled = false;
            btnGetHistory.Enabled = true;

            if ((sSelection.Equals("Tick Datapoints")) || (sSelection.Equals("Interval Datapoints"))
                || (sSelection.Equals("Daily Datapoints")) || (sSelection.Equals("Weekly Datapoints"))
                || (sSelection.Equals("Monthly Datapoints")))
            {
                // for interval datapoints request, we also need the interval
                if (sSelection.Equals("Interval Datapoints"))
                {
                    txtInterval.Enabled = true;
                    rbTick.Enabled = true;
                    rbTime.Enabled = true;
                    rbVolume.Enabled = true;
                }
            }
            else if (sSelection.Equals("Tick Days"))
            {
                // enable controls available for tick days request
                txtDays.Enabled = true;
                txtBeginFilterTime.Enabled = true;
                txtEndFilterTime.Enabled = true;
            }
            else if (sSelection == "Tick Timeframe")
            {
                // enable controls available for tick timeframe request
                txtBeginDateTime.Enabled = true;
                txtEndDateTime.Enabled = true;
                txtBeginFilterTime.Enabled = true;
                txtEndFilterTime.Enabled = true;
            }
            else if (sSelection == "Interval Days")
            {
                // enable controls available for Interval days request
                txtInterval.Enabled = true;
                txtDays.Enabled = true;
                txtBeginFilterTime.Enabled = true;
                txtEndFilterTime.Enabled = true;
                rbTick.Enabled = true;
                rbTime.Enabled = true;
                rbVolume.Enabled = true;
            }
            else if (sSelection == "Interval Timeframe")
            {
                // enable controls available for Interval timeframe request
                txtInterval.Enabled = true;
                txtBeginDateTime.Enabled = true;
                txtEndDateTime.Enabled = true;
                txtBeginFilterTime.Enabled = true;
                txtEndFilterTime.Enabled = true;
                rbTick.Enabled = true;
                rbTime.Enabled = true;
                rbVolume.Enabled = true;
            }
            else if (sSelection == "Daily Timeframe")
            {
                // enable controls available for Daily timeframe request
                txtBeginDateTime.Enabled = true;
                txtEndDateTime.Enabled = true;
            }
        }

        /// <summary>
        /// Disables all the controls on the form.
        /// </summary>
        private void DisableForm()
        {
            // Disable this for now
            return;

            /*foreach (Control c in this.Controls)
            {
                if (!(c is Label))
                {
                    c.Enabled = false;
                }
            }*/
        }


        private void AfterHistoryReceived()
        {
            string text = tssMain.Text;
            display(text + " Done.");

            /*
            string exe_path = System.Reflection.Assembly.GetEntryAssembly().Location;
            string filename = System.IO.Path.Combine(exe_path, "my_output.txt");

            foreach (string msg in m_receivedData)
            {
                System.Console.WriteLine(msg);
            }*/
        }


        private void GetIqFeedHistory(string symbol, int dayCount)
        {
            ThreadStart work = delegate {
                m_historyThread.GetHistory(symbol, dayCount);
                //var result = m_historyThread.GetHistory("QHON17", 1);
                // push result somewhere; might involve a UI
                // thread switch
            };

            ExecuteThread(work);
        }

        private void RequestDailyHistory(string symbol, DateTime dtStart, DateTime dtEnd)
        {
            string startDate = GetIQDate(dtStart);
            string endDate = GetIQDate(dtEnd);
            string sRequest = string.Format("HDT,{0},{1},{2},,,,", symbol, startDate, endDate);
            SendRequest(sRequest);
        }

        private void RequestIntervalHistory(string symbol, Interval interval, DateTime dtStart, DateTime dtEnd)
        {
            RequestIntervalHistory(symbol, (int)interval, dtStart, dtEnd);
        }

        private void RequestIntervalHistory(string symbol, int intervalSeconds, DateTime dtStart, DateTime dtEnd)
        {
            string startDateTime = GetIQDateTime(dtStart);
            string endDateTime = GetIQDateTime(dtEnd);
            string sRequest = string.Format("HIT,{0},{1},{2},{3},,,,,,,s", symbol, intervalSeconds, startDateTime, endDateTime);
            SendRequest(sRequest);
        }

        private void ListResults()
        {
            lstData.Items.Clear();
            Application.DoEvents();
            foreach (string msg in m_historyThread.ReceivedData)
            {
                UpdateListview(msg);
            }
        }

        // If the "CURRENT PROTOCOL" message exists in our IQFeed received data, remove it (but store it first as m_protocol)
        private void RemoveProtocolMessageFromReceived()
        {
            if (m_historyThread.ReceivedData.Count > 0 && m_historyThread.ReceivedData[0].Contains("CURRENT PROTOCOL"))
            {
                m_protocol = m_historyThread.ReceivedData[0];
                Console.WriteLine(m_protocol);
                m_historyThread.ReceivedData.RemoveAt(0);
            }
        }

        // symbol: futures symbol (ex: "@ESH17")
        // m: month (1-12)
        // y: year (4-digit year)
        // interval: "Daily", "1 Minute", "15 Minute", "30 Minute", "60 Minute", "1 Hour", etc...
        private void RequestHistoricalData(string symbol, int m, int y, string interval="Daily")
        {
            var dt2 = new DateTime(y, m, 1);
            dt2 = dt2.AddMonths(1);
            dt2 = dt2.AddDays(-1);              // last day of expiration month
            var dt1 = dt2.AddDays(-365);        // start of historical data from one year earlier

            display(symbol + " Working...");

            string filename;
            if (interval == "Daily")            // daily interval
            {
                RequestDailyHistory(symbol, dt1, dt2);
                filename = symbol + " (Daily).csv";
            }
            else if (interval == "1 Minute")    // 1-minute interval
            {
                RequestIntervalHistory(symbol, Interval.Minute, dt1, dt2);
                filename = symbol + " (1 Minute).csv";
            }
            else if (interval == "1 Hour")      // 1-hour interval
            {
                RequestIntervalHistory(symbol, Interval.Hour, dt1, dt2);
                filename = symbol + " (1 Hour).csv";
            }
            else
            {
                display("Unknown interval: '" + interval + "'");
                return;
            }

            display("Done.");

            RemoveProtocolMessageFromReceived();
            UpdateListview(string.Format("{0}   count: {1}", symbol, m_historyThread.ReceivedData.Count));
            Application.DoEvents();

            // Store the retrieved data to a text file
            List<string> data = m_historyThread.ReceivedData;
            WriteToFile(filename, data, (int) numHourAdjustFutures.Value);
        }

        private void btnDisplayHistory_Click(object sender, EventArgs e)
        {
            ListResults();
        }

        private void display(string msg)
        {
            tssMain.Text = msg.Trim();
            Application.DoEvents();
        }

        private void WriteToFile(string filename, List<string> data, int hourAdjust=0)
        {
            if (data.Count == 0) return;        // nothing to write

            string folder = txtHistoricalFolder.Text.Trim();
            string pathname = Path.Combine(folder, filename);
            using (StreamWriter file = new StreamWriter(pathname))
            {
                file.WriteLine("DateTime,Open,High,Low,Close,Volume");
                for (int i = data.Count - 1; i >= 0; --i)
                {
                    var columns = data[i].Split(new char[] { ',' });
                    // TODO: convert time from exchange time to local time
                    // If we have Date (rather than DateTime), add a "00:00:00" to make it DateTime
                    /*if (columns[0].Length < 11)
                    {
                        columns[0] += " 00:00:00";
                    }*/
                    if (hourAdjust != 0)
                    {
                        var dt = DateTime.Parse(columns[0]);
                        dt = dt.AddHours(hourAdjust);
                        columns[0] = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    string line = string.Format("{0},{1},{2},{3},{4},{5}", columns[0], columns[3], columns[1], columns[2], columns[4], columns[6]);
                    file.WriteLine(line);
                }
            }

            display("Output to file '" + filename + "'.");
        }

        private void btnWriteToFile_Click(object sender, EventArgs e)
        {
            List<string> data = lstData.Items.Cast<ListViewItem>()
                     .Select(item => item.Text)
                     .ToList();

            if (cboHistoryType.SelectedItem.ToString().StartsWith("Daily"))
            {
                string symbol = txtSymbol.Text.Trim().ToUpper();
                string filename = symbol + " (Daily).csv";
                WriteToFile(filename, data);
            }
            else
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        private static string GetYYm(string rootSymbol)
        {
            string YYm = rootSymbol.Substring(4, 2) + rootSymbol.Substring(3, 1);
            return YYm;
        }

        private static int CompareFuturesDate(string x, string y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;
                else
                    return -1;
            }
            else
            {
                if (y == null)
                    return 1;
                else
                {
                    string cmpx = GetYYm(x);
                    string cmpy = GetYYm(y);
                    return cmpx.CompareTo(cmpy);
                }
            }
        }

        private void ClearFileList()
        {
            lvDataFiles.Items.Clear();
        }

        private void DisplayFileList(List<string> files, bool sortFutures=true)
        {
            files.Sort(CompareFuturesDate);
            foreach (string f in files)
            {
                //Console.WriteLine(f);
                ListViewItem lvi = new ListViewItem(f);
                //lvi.Text = f;
                DateTime modification = File.GetLastWriteTime(Path.Combine(folders.df_folder, f));
                lvi.SubItems.Add(modification.ToShortDateString());
                lvDataFiles.Items.Add(lvi);
            }

            lblDataFileCount.Text = string.Format("{0} Files", lvDataFiles.Items.Count);
        }

        private void btnRefreshSelectedSymbol_Click(object sender, EventArgs e)
        {
            lstData.Items.Clear();

            string selected = (string)listDataSymbols.SelectedItem;
            if (selected == null) return;

            RefreshDataForSymbol(selected);

            // In case we have updated any currently listed data files, update the list to reflect modified file dates
            UpdateDataFilesList();
        }

        private void btnRefreshAllData_Click(object sender, EventArgs e)
        {
            lstData.Items.Clear();

            string folder = txtHistoricalFolder.Text.Trim();

            // Retrieve the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(folder);

            foreach (string rootSymbol in listDataSymbols.Items)
            {
                RefreshDataForSymbol(rootSymbol);
            }

            // In case we have updated any currently listed data files, update the list to reflect modified file dates
            UpdateDataFilesList();
        }

        private void RefreshDataForSymbol(string rootSymbol)
        {
            DateTime dtNow = DateTime.Now;

            foreach (string timeframe in listDataTimeFrames.Items)
            {
                // Only retrieve latest historical data for symbol/timeframe combos we have ALREADY downloaded
                var files = GetDataFiles(rootSymbol, timeframe);
                if (files.Count == 0) continue;
                //files.Sort(CompareFuturesDate);
                //DisplayFileList(files);

                if (rootSymbol.Length > 3)
                {
                    string filename = null, sRequest = null;
                    string beginDateTime = txtBeginDateTime.Text;
                    string endDateTime = DateTime.Now.ToString("yyyyMMdd");
                    if (timeframe == "Daily")
                    {
                        sRequest = GetDailyTimeframeRequest(rootSymbol, beginDateTime, endDateTime);
                        filename = rootSymbol + " (Daily).csv";
                        SubmitIqFeedRequest(sRequest, filename);
                    }
                    else if (timeframe == "1 Minute")
                    {
                        sRequest = GetIntervalTimeframeRequest(rootSymbol, "s", IntervalValues.Minute.ToString(), beginDateTime, endDateTime);
                        filename = rootSymbol + " (1 Minute).csv";
                        SubmitIqFeedRequest(sRequest, filename);
                    }
                    else if (timeframe == "1 Hour")
                    {
                        sRequest = GetIntervalTimeframeRequest(rootSymbol, "s", IntervalValues.Hour.ToString(), beginDateTime, endDateTime);
                        filename = rootSymbol + " (1 Hour).csv";
                        SubmitIqFeedRequest(sRequest, filename);
                    }
                    else
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
                else
                {
                    // Starting with today's date, get data from 1 month previous to 4 months hence
                    int startMonth = dtNow.Month;
                    int startYear = dtNow.Year;
                    for (int i = -1; i <= 4; ++i)
                    {
                        string mYY = AddMonths(startMonth, startYear, i);
                        //Console.Write("({0} {1})", G.GetMonth(mYY), G.GetYear(mYY));
                        RequestHistoricalData(rootSymbol + mYY, GetMonth(mYY), GetYear(mYY), timeframe);
                    }
                }
            }
        }

        private void HistorySocketForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void listDataTimeFrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataFilesList();
        }

        private void listDataSymbols_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataFilesList();
        }

        private void lvDataFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // For now just handle clicking on the filename column.
            if (e.Column == 0)
            {
                ListViewItem[] items = new ListViewItem[lvDataFiles.Items.Count];
                lvDataFiles.Items.CopyTo(items, 0);
                lvDataFiles.Items.Clear();
                lvDataFiles.Items.AddRange(items.Reverse().ToArray());
            }
        }

        private void lstData_Resize(object sender, EventArgs e)
        {
            columnList.Width = lstData.Width;
        }

        private void btnIntervalMinute_Click(object sender, EventArgs e)
        {
            rbTime.Checked = true;
            cboHistoryType.SelectedItem = "Interval Timeframe";
            txtInterval.Text = ((int)(IntervalValues.Minute)).ToString();
        }

        private void btnIntervalHour_Click(object sender, EventArgs e)
        {
            rbTime.Checked = true;
            cboHistoryType.SelectedItem = "Interval Timeframe";
            txtInterval.Text = ((int)(IntervalValues.Hour)).ToString();
        }

        private List<string> RequestListedMarkets()
        {
            return RequestList("SLM");
        }

        private List<string> RequestSecurityTypes()
        {
            return RequestList("SST");
        }

        private List<string> RequestList(string request, string requestId="CSLISTS")
        {
            //string request = = "SST";
            string sRequest = String.Format("{0},{1}\r\n", request, requestId);
            List<string> results = SubmitIqFeedRequest(sRequest);
            // Remove the initial "CSxxxxxxx," from each line
            for (int i = 0; i < results.Count; ++i)
            {
                string[] array = results[i].Split(',');
                string firstElem = array.First();
                string restOfArray = string.Join(",", array.Skip(1));
                results[i] = restOfArray;
            }
            return results;
        }

        private string GetSearchRequest(string searchString, string fieldToSearch = "d", string filterType = "", string filterValue = "")
        {
            // fieldToSearch: "d" to search descriptions, "s" to search symbols
            // filterType: "e" to search within specific Listed Markets, "t" to search within specific Security Types
            // filterValue: a space-delimited list of Listed Markets or Security Types (based upon the filterType parameter)
            string searchingWhat = fieldToSearch == "d" ? "descriptions" : "symbols";
            UpdateListview(string.Format("Searching {0} for '{1}'...", searchingWhat, searchString));
            string requestId = "CSSYMBOL";
            string sRequest = String.Format("SBF,{0},{1},{2},{3},{4}\r\n", fieldToSearch, searchString, filterType, filterValue, requestId);
            Console.WriteLine(sRequest);
            return sRequest;
        }

        private string GetSelectedMarket()
        {
            string selected = "";
            if (cboListedMarkets.SelectedIndex > -1)
            {
                string txt = cboListedMarkets.SelectedItem.ToString();
                string[] array = txt.Split(',');
                selected = array.First();
            }
            return selected;
        }

        private string GetSelectedSecurityType()
        {
            string selected = "";
            if (cboSecurityTypes.SelectedIndex > -1)
            {
                string txt = cboSecurityTypes.SelectedItem.ToString();
                string[] array = txt.Split(',');
                selected = array.First();
            }
            return selected;
        }

        private void SymbolSearch()
        {
            ClearListview();

            string filterType = "";
            string filterValue = "";
            if (cboSecurityTypes.SelectedIndex > -1)
            {
                filterType = "t";
                filterValue = GetSelectedSecurityType();
            }
            else if (cboListedMarkets.SelectedIndex > -1)
            {
                filterType = "e";
                filterValue = GetSelectedMarket();
            }

            string sRequest;
            sRequest = GetSearchRequest(txtSymbolSearch.Text.Trim(), "d", filterType, filterValue);
            SubmitIqFeedRequest(sRequest);
            sRequest = GetSearchRequest(txtSymbolSearch.Text.Trim(), "s", filterType, filterValue);
            SubmitIqFeedRequest(sRequest);
        }

        private void btnSymbolSearch_Click(object sender, EventArgs e)
        {
            SymbolSearch();
        }

        private void btnClearSymbolSearch_Click(object sender, EventArgs e)
        {
            txtSymbolSearch.Text = "";
            cboListedMarkets.SelectedIndex = -1;
            cboSecurityTypes.SelectedIndex = -1;
        }

        private void cboSecurityTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSecurityTypes.SelectedIndex > -1)
                cboListedMarkets.SelectedIndex = -1;
        }

        private void cboListedMarkets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboListedMarkets.SelectedIndex > -1)
                cboSecurityTypes.SelectedIndex = -1;
        }

        private void tsbtnClearOutput_Click(object sender, EventArgs e)
        {
            ClearListview();
        }

        private void txtSymbolSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SymbolSearch();
        }


        private void ExecuteThread(ThreadStart work)
        {
            m_historyThread.WaitingForData = true;
            new Thread(work).Start();

            while (m_historyThread.WaitingForData == true)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("m_historyThread thread has finished");
        }

        private void SendRequest(string sRequest)
        {
            ThreadStart work = delegate {
                m_historyThread.SendRequest(sRequest);
            };

            ExecuteThread(work);
        }


    } // end of class
} // end of namespace





/*
Thread oThread = new Thread(new ThreadStart(m_historyThread.GetHistory));

// Start the thread
oThread.Start();

// Spin for a while waiting for the started thread to become
// alive:
while (!oThread.IsAlive) ;

// Put the Main thread to sleep for 1000 milliseconds to allow oThread
// to do some work:
Thread.Sleep(1000);

// Request that oThread be stopped
oThread.Abort();

// Wait until oThread finishes. Join also has overloads
// that take a millisecond interval or a TimeSpan object.
oThread.Join();
*/
