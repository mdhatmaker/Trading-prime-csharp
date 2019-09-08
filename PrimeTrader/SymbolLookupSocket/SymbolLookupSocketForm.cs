//-----------------------------------------------------------
//-----------------------------------------------------------
//
//             System: IQFeed
//       Program Name: SymbolLookupSocket_VC#.exe
//        Module Name: SymbolLookupSocketForm.cs
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
// Module Description: Implementation of Symbol Lookup via Sockets
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

namespace SymbolLookupSocket
{
    public partial class SymbolLookupSocketForm : Form
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
        public SymbolLookupSocketForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls and initializing the connection to IQFeed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SymbolLookupSocketForm_Load(object sender, EventArgs e)
        {
            //Load the config object namespace which contains the latest protocol and your product ID.
            IQ_Config config = new IQ_Config();
            // Set the protocol for the socket
            string sRequest = String.Format("S,SET PROTOCOL,{0}\r\n", config.getProtocol());
            // create the socket and tell it to connect
            m_sockLookup = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipLocalhost = IPAddress.Parse("127.0.0.1");

            // Symbol Lookup data is received from IQFeed on the Lookup port.
            // pull the Lookup port out of the registry
            int iPort = GetIQFeedPort("Lookup");
            IPEndPoint ipendLocalhost = new IPEndPoint(ipLocalhost, iPort);

            try
            {
                // connect the socket
                m_sockLookup.Connect(ipendLocalhost);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(String.Format("Oops.  Did you forget to Login first?\nTake a Look at the LaunchingTheFeed example app\n{0}", ex.Message), "Error Connecting to IQFeed");
                DisableForm();
            }
            byte[] szRequest = new byte[sRequest.Length];
            szRequest = Encoding.ASCII.GetBytes(sRequest);
            int iBytesToSend = szRequest.Length;
            int iBytesSent = m_sockLookup.Send(szRequest, iBytesToSend, SocketFlags.None);
            WaitForData("Symbol");
            // default to searching the symbol field.
            rbtnSymbol.Checked = true;
        }

        /// <summary>
        /// we call this to notify the .NET Async socket to start listening for data to come in.  It must be called each time after we receive data
        /// </summary>
        private void WaitForData(string sSocketName)
        {
            if (sSocketName.Equals("Symbol"))
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
            if (asyn.AsyncState.ToString().Equals("Symbol"))
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
                // symbol data will be read off the socket in groups of messages.  We have no control over how many messages will be
                // read off the socket at each read.  Likewise we have no guarantee that we won't get an incomplete message at the beginning
                // or ending of the group of messages.  Our processing needs to handle this.
                // chains data is always terminated with a cariage return and newline characters ("\r\n").  
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
                // at this point, iPos (start of the current message) will be less than sData.Length if we had an incomplete message
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
                    // end of message. trim off the end msg
                    sData = sData.Remove(sData.Length - "!ENDMSG!,\r\n".Length);
                }

                UpdateListbox(sData);

                // call wait for data to notify the socket that we are ready to receive another callback
                WaitForData("Symbol");
            }
        }

        /// <summary>
        /// Event that fires when the user clicks the Get Data Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            // clear the listbox before making the next request
            lstData.Items.Clear();

            string sCommand = "";

            // here we make the request for data to the feed via the socket based on user input
            if (rbtnSymbol.Checked)
            {
                // if (Symbol Checked in Search By Group box
                if (rbtnListedMarket.Checked)
                {
                    // if (Listed Market checked in Filter Group box
                    sCommand = String.Format("SBF,s,{0},e,{1},{2}\r\n", txtSearchString.Text, txtFilterValue.Text, txtRequestID.Text);
                }
                else if (rbtnSecurityType.Checked)
                {
                    // if (Security Type checked in Filter Group Box
                    sCommand = String.Format("SBF,s,{0},t,{1},{2}\r\n", txtSearchString.Text, txtFilterValue.Text, txtRequestID.Text);
                }
                else
                {
                    // if (no filter checked in Filter Group Box
                    sCommand = String.Format("SBF,s,{0},,,{1}\r\n", txtSearchString.Text, txtRequestID.Text);
                }
            }
            else if (rbtnDescription.Checked)
            {
                // if (Description Checked in Search By Group box
                if (rbtnListedMarket.Checked)
                {
                    // if (Listed Market checked in Filter Group box
                    sCommand = String.Format("SBF,d,{0},e,{1},{2}\r\n", txtSearchString.Text, txtFilterValue.Text, txtRequestID.Text);
                }
                else if (rbtnSecurityType.Checked)
                {
                    // if (Security Type checked in Filter Group Box
                    sCommand = String.Format("SBF,d,{0},t,{1},{2}\r\n", txtSearchString.Text, txtFilterValue.Text, txtRequestID.Text);
                }
                else
                {
                    // if (no filter checked in Filter Group Box
                    sCommand = String.Format("SBF,d,{0},,,{1}\r\n", txtSearchString.Text, txtRequestID.Text);
                }
            }
            else if (rbtnSIC.Checked)
            {
                // if SIC Checked in Search By Group box
                sCommand = String.Format("SBS,{0},{1}\r\n", txtSearchString.Text, txtRequestID.Text);
            }
            else if (rbtnNAICS.Checked)
            {
                // if NAICS Checked in Search By Group box
                sCommand = String.Format("SBN,{0},{1}\r\n", txtSearchString.Text, txtRequestID.Text);
            }

            if (sCommand.Length > 0)
            {
                // send it to the feed via the socket
                SendRequestToIQFeed(sCommand);

                // call wait for data to notify the socket that we are ready to receive data
                WaitForData("Symbol");
            }
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
                    UpdateListbox(String.Format("Error Sending Request:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
                else
                {
                    UpdateListbox(String.Format("Request Sent Successfully:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
            }
            catch (SocketException ex)
            {
                // handle socket errors
                UpdateListbox(String.Format("Socket Error Sending Request:\r\n{0}\r\n{1}", sCommand.TrimEnd("\r\n".ToCharArray()), ex.Message));
            }
        }

        /// <summary>
        /// We want to be able to update the winform status listbox from within the AsyncSocket Callback 
        /// so we need a delagate to resolve cross-threading issues.
        /// </summary>
        /// <param name="sData"></param>
        public void UpdateListbox(string sData)
        {
            try
            {
                if (lstData.InvokeRequired)
                {
                    this.Invoke(new UpdateDataHandler(UpdateListbox), sData);
                }
                else
                {
                    List<String> lstMessages = new List<string>(sData.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                    lstData.BeginUpdate();
                    foreach (string sLine in lstMessages)
                    {
                        lstData.Items.Add(sLine);
                    }
                    lstData.EndUpdate();
                }
            }
            catch (ObjectDisposedException)
            {
                // The listbox object went away, ignore it since we're probably exiting.
            }
        }

        /// <summary>
        /// Handles when the user clicks the "Launch System Info" button.
        ///     This just launches the SystemInfo example app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLaunch_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("SystemInfoSocket_C#.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error Launching SystemInfoSocket_C#.exe\n{0}", ex.Message), "Error Launching SystemInfo");
            }
        }

        /// <summary>
        /// Event that fires when the user clicks the Symbol radio Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnSymbol_CheckedChanged(object sender, EventArgs e)
        {
            // enable the filter group box since these results can be filtered
            gbFilter.Enabled = true;
        }

        /// <summary>
        /// Event that fires when the user clicks the Description radio Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnDescription_CheckedChanged(object sender, EventArgs e)
        {
            // enable the filter group box since these results can be filtered
            gbFilter.Enabled = true;
        }

        /// <summary>
        /// Event that fires when the user clicks the Description SIC Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnSIC_CheckedChanged(object sender, EventArgs e)
        {
            // disable the filter group box since these results can not be filtered
            gbFilter.Enabled = false;
        }

        /// <summary>
        /// Event that fires when the user clicks the NAICS radio Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnNAICS_CheckedChanged(object sender, EventArgs e)
        {
            // disable the filter group box since these results can not be filtered
            gbFilter.Enabled = false;
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