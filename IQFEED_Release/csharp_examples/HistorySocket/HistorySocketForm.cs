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
using IQ_Config_Namespace;

namespace HistorySocket
{
    public partial class HistorySocketForm : Form
    {
        // socket communication global variables
        AsyncCallback m_pfnLookupCallback;
        Socket m_sockLookup;
        // we create the socket buffer global for performance
        byte[] m_szLookupSocketBuffer = new byte[262144];
        // stores unprocessed data between reads from the socket
        string m_sLookupIncompleteRecord = "";
        // flag for tracking when a call to BeginReceive needs called
        bool m_bLookupNeedBeginReceive = true;

        // delegate for updating the data display.
        public delegate void UpdateDataHandler(string sMessage);

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
            IQ_Config config = new IQ_Config();
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
            cboHistoryType.SelectedIndex = 0;

            // create the socket
            m_sockLookup = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipLocalhost = IPAddress.Parse("127.0.0.1");

            // Historical data is received from IQFeed on the Lookup port.
            // pull the Lookup port out of the registry
            int iPort = GetIQFeedPort("Lookup");
            IPEndPoint ipendLocalhost = new IPEndPoint(ipLocalhost, iPort);

            try
            {
                // connect the socket
                m_sockLookup.Connect(ipendLocalhost);

                // Set the protocol for the lsocket
                SendRequestToIQFeed(String.Format("S,SET PROTOCOL,{0}\r\n",config.getProtocol()));
            }
            catch (SocketException ex)
            {
                MessageBox.Show(String.Format("Oops.  Did you forget to Login first?\nTake a Look at the LaunchingTheFeed example app\n{0}", ex.Message), "Error Connecting to IQFeed");
                DisableForm();
            }

            cboHistoryType.SelectedIndex = 0;
        }

        /// <summary>
        /// we call this to notify the .NET Async socket to start listening for data to come in.  It must be called each time after we receive data
        /// </summary>
        private void WaitForData(string sSocketName)
        {
            if (sSocketName.Equals("History"))
            {
                // make sure we have a callback created
                if (m_pfnLookupCallback == null)
                {
                    m_pfnLookupCallback = new AsyncCallback(OnReceive);
                }

                // send the notification to the socket.  It is very important that we don't call Begin Reveive more than once per call
                // to EndReceive.  As a result, we set a flag to ignore multiple calls.
                if (m_bLookupNeedBeginReceive)
                {
                    m_bLookupNeedBeginReceive = false;
                    // we pass in the sSocketName in the state parameter so that we can verify the socket data we receive is the data we are looking for
                    m_sockLookup.BeginReceive(m_szLookupSocketBuffer, 0, m_szLookupSocketBuffer.Length, SocketFlags.None, m_pfnLookupCallback, sSocketName);
                }
            }
        }

        /// <summary>
        /// This is our callback that gets called by the .NET socket class when new data arrives on the socket
        /// </summary>
        /// <param name="asyn"></param>
        private void OnReceive(IAsyncResult asyn)
        {
            // first verify we received data from the correct socket.  This check isn't really necessary in this example since we 
            // only have a single socket but if we had multiple sockets, we could use this check to use the same callback to recieve data from
            // multiple sockets
            if (asyn.AsyncState.ToString().Equals("History"))
            {
                // read data from the socket.  The call to EndReceive tells the Framework to copy data available on the socket into our socket buffer
                // that we supplied in the BeginReceive call.
                int iReceivedBytes = 0;
                iReceivedBytes = m_sockLookup.EndReceive(asyn);
                m_bLookupNeedBeginReceive = true;
                // add the data received from the socket to any data that was left over from the previous read off the socket.
                string sData = m_sLookupIncompleteRecord + Encoding.ASCII.GetString(m_szLookupSocketBuffer, 0, iReceivedBytes);
                // clear the incomplete record string so it doesn't get added again next time we read from the socket
                m_sLookupIncompleteRecord = "";
                // history data will be read off the socket in groups of messages.  We have no control over how many messages will be
                // read off the socket at each read.  Likewise we have no guarantee that we won't get an incomplete message at the beginning
                // or ending of the group of messages.  Our processing needs to handle this.
                // history data is always terminated with a cariage return and newline characters ("\r\n").  
                // we verify a record is complete by finding the newline character.
                int iNewLinePos = sData.IndexOf("\n");
                int iPos = 0;
                // loop through the group of messages
                while (iNewLinePos >= 0)
                {
                    // at this point, we know we have a complete message between iPos (start of the message) and iNewLinePos (end)
                    // here we could add message processing for this single line of data but in this example, we just display the raw data
                    // so we just keep looping through the messages
                    iPos = iNewLinePos + 1;
                    iNewLinePos = sData.IndexOf("\n", iPos);
                }
                // at this point, iPos (start of the current message) will be less than m_strData.Length if we had an incomplete message
                // at the end of the data.  We detect this and save off the incomplete message
                if (sData.Length > iPos)
                {
                    // left an incomplete record in the buffer
                    m_sLookupIncompleteRecord = sData.Substring(iPos);
                    // remove the incomplete message from the message
                    sData = sData.Remove(iPos);
                }
                else if (sData.EndsWith("!ENDMSG!,\r\n"))
                {
                    // end of message.
                }
                
                // display the data to the user
                UpdateListview(sData);
                
                // clear the m_strData to verify it is empty for the next read off the socket
                sData = "";

                // call wait for data to notify the socket that we are ready to receive another callback
                WaitForData("History");
            }
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
            lstMessages.ForEach(delegate(String sLine)
            {
                lstData.Items.Add(sLine);
            });
                    lstData.EndUpdate();
        }
            }
            catch (ObjectDisposedException)
            {
                // The list view object went away, ignore it since we're probably exiting.
            }
        }

        /// <summary>
        /// Event handler for when the user clicks the GetData button.  It forms the request that will be sent to
        ///     IQFeed based upon user input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            // clear previous request data
            string sRequest = "";
            lstData.Items.Clear();

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
            if (sRequest.StartsWith("H"))
            {
                // send it to the feed via the socket
                SendRequestToIQFeed(sRequest);
                }
                else
                {
                string sError = String.Format("{0}\r\nRequest type selected was: {1}", sRequest, cboHistoryType.Text);
                UpdateListview(sError);
                }

            // tell the socket we are ready to receive data
            WaitForData("History");
            }

        /// <summary>
        /// Sends a string to the socket connected to IQFeed
        /// </summary>
        /// <param name="sCommand"></param>
        private void SendRequestToIQFeed(string sCommand)
        {
            // and we send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            try
            {
                int iBytesSent = m_sockLookup.Send(szCommand, iBytesToSend, SocketFlags.None);
                if (iBytesSent != iBytesToSend)
                {
                    UpdateListview(String.Format("Error Sending Request:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
            else
            {
                    UpdateListview(String.Format("Request Sent Successfully:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray())));
            }
        }
            catch (SocketException ex)
            {
                // handle socket errors
                UpdateListview(String.Format("Socket Error Sending Request:\r\n{0}\r\n{1}", sCommand.TrimEnd("\r\n".ToCharArray()), ex.Message));
            }
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
            foreach (Control c in this.Controls)
            {
                if (!(c is Label))
                {
                    c.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Gets local IQFeed socket ports from the registry
        /// </summary>
        /// <param name="sPort"></param>
        /// <returns></returns>
        private int GetIQFeedPort(string sPort)
        {
            int iReturn = 0;
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup");
            if (key != null)
            {
                string sData = "";
                switch (sPort)
                {
                    case "Level1":
                        // the default port for Level 1 data is 5009.
                        sData = key.GetValue("Level1Port", "5009").ToString();
                        break;
                    case "Lookup":
                        // the default port for Lookup data is 9100.
                        sData = key.GetValue("LookupPort", "9100").ToString();
                        break;
                    case "Level2":
                        // the default port for Level 2 data is 9200.
                        sData = key.GetValue("Level2Port", "9200").ToString();
                        break;
                    case "Admin":
                        // the default port for Admin data is 9300.
                        sData = key.GetValue("AdminPort", "9300").ToString();
                        break;
                    case "Derivative":
                        // the default port for derivative data is 9400
                        sData = key.GetValue("DerivativePort", "9400").ToString();
                        break;
                }
                iReturn = Convert.ToInt32(sData);
            }
            return iReturn;
        }
    }
}