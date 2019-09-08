//-----------------------------------------------------------
//-----------------------------------------------------------
//
//             System: IQFeed
//       Program Name: MarketDepthSocket_VC#.exe
//        Module Name: MarketDepthSocketForm.cs
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
// Module Description: Implementation of Level 2/MarketDepth Streaming Quotes
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

namespace MarketDepthSocket
{
    public partial class MarketDepthSocketForm : Form
    {
        // global variables for socket communications
        AsyncCallback m_pfnMarketDepthCallback;
        Socket m_sockMarketDepth;
        // we create the socket buffer global for performance
        byte[] m_szMarketDepthSocketBuffer = new byte[8096];
        // we create a global parsing string for performance
        string m_sMarketDepthData = "";
        // stores unprocessed data between reads from the socket
        private string m_sMarketDepthIncompleteRecord = "";
        private bool m_bMarketDepthNeedBeginReceive = true;

        // delegate for updating the data display.
        public delegate void UpdateDataHandler(string sMessage);
        // delegate for updating the controls
        public delegate void UpdateControlsHandler();

        /// <summary>
        /// Constructor for the form
        /// </summary>
        public MarketDepthSocketForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls and initializing the connection to IQFeed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarketDepthSocketForm_Load(object sender, EventArgs e)
        {
            //Load the config object namespace which contains the latest protocol and your product ID.
            IQ_Config config = new IQ_Config();
            // Set the protocol for the socket
            string sRequest = String.Format("S,SET PROTOCOL,{0}\r\n", config.getProtocol());
            // create the socket and tell it to connect
            m_sockMarketDepth = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipLocalhost = IPAddress.Parse("127.0.0.1");

            // Market Depth data is received from IQFeed on the Level2 port.
            // pull the Level2 port out of the registry
            int iPort = GetIQFeedPort("Level2");
            IPEndPoint ipendLocalhost = new IPEndPoint(ipLocalhost, iPort);

            try
            {
                // connect the socket
                m_sockMarketDepth.Connect(ipendLocalhost);

                byte[] szRequest = new byte[sRequest.Length];
                szRequest = Encoding.ASCII.GetBytes(sRequest);
                int iBytesToSend = szRequest.Length;
                int iBytesSent = m_sockMarketDepth.Send(szRequest, iBytesToSend, SocketFlags.None);

                // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
                WaitForData("MarketDepth");
            }
            catch (SocketException ex)
            {
                MessageBox.Show(String.Format("Oops.  Did you forget to Login first?\nTake a Look at the LaunchingTheFeed example app\n{0}", ex.Message), "Error Connecting to IQFeed");
                DisableForm();
            }
        }

        /// <summary>
        /// we call this to notify the .NET Async socket to start listening for data to come in.  It must be called each time after we receive data
        /// </summary>
        /// <param name="sSocketName"></param>
        private void WaitForData(string sSocketName)
        {
            if (sSocketName.Equals("MarketDepth"))
            {
                // make sure we have a callback created
                if (m_pfnMarketDepthCallback == null)
                {
                    m_pfnMarketDepthCallback = new AsyncCallback(OnReceive);
                }

                // send the notification to the socket.  It is very important that we don't call Begin Reveive more than once per call
                // to EndReceive.  As a result, we set a flag to ignore multiple calls.
                if (m_bMarketDepthNeedBeginReceive)
                {
                    m_bMarketDepthNeedBeginReceive = false;
                    // we pass in the sSocketName in the state parameter so that we can verify the socket data we receive is the data we are looking for
                    m_sockMarketDepth.BeginReceive(m_szMarketDepthSocketBuffer, 0, m_szMarketDepthSocketBuffer.Length, SocketFlags.None, m_pfnMarketDepthCallback, sSocketName);
                }
            }
        }

        /// <summary>
        /// Our Callback that gets called by the .NET socket class when new data arrives on the socket
        /// </summary>
        /// <param name="asyn"></param>
        private void OnReceive(IAsyncResult asyn)
        {
            // first verify we received data from the correct socket.  This check isn't really necessary in this example since we 
            // only have a single socket but if we had multiple sockets, we could use this check to use the same function to recieve data from
            // multiple sockets
            if (asyn.AsyncState.ToString().Equals("MarketDepth"))
            {
                // read data from the socket
                int iReceivedBytes = 0;
                iReceivedBytes = m_sockMarketDepth.EndReceive(asyn);
                // set our flag back to true so we can call begin receive again
                m_bMarketDepthNeedBeginReceive = true;
                // add the data received from the socket to any data that was left over.  m_strIncompleteRecord is populated during
                // processing when the last record recieved is detected as being incomplete
                m_sMarketDepthData = m_sMarketDepthIncompleteRecord + Encoding.ASCII.GetString(m_szMarketDepthSocketBuffer, 0, iReceivedBytes);
                // clear the incomplete record string so it doesn't get added again next time we read from the socket
                m_sMarketDepthIncompleteRecord = "";
                // market depth and level 2 data will be read off the socket in groups of messages.  We have no control over how many messages will be
                // read off the socket at each read.  Likewise we have no guarantee that we won't get an incomplete message at the beginning
                // or ending of the group of messages.  Our processing needs to handle this.
                // market depth and level 2 messages are always terminated with cariage return and newline characters ("\r\n").  
                // we verify a record is complete by finding the newline character.
                int iNewLinePos = m_sMarketDepthData.IndexOf("\n");
                int iPos = 0;
                // loop through the group of messages
                while (iNewLinePos >= 0)
                {
                    // at this point, we know we have a complete message between iPos (start of the message) and iNewLinePos (end)
                    // here we could add message processing for this single line of data but in this example, we just display the raw data
                    // so we just keep looping through the messages
                    iPos = iNewLinePos + 1;
                    iNewLinePos = m_sMarketDepthData.IndexOf("\n", iPos);
                }
                // at this point, iPos (start of the current message) will be less than m_strData.Length if we had an incomplete message
                // at the end of the data.  We detect this and save off the incomplete message
                if (m_sMarketDepthData.Length > iPos)
                {
                    // left an incomplete record in the buffer
                    m_sMarketDepthIncompleteRecord = m_sMarketDepthData.Substring(iPos);
                    // remove the incomplete message from the message
                    m_sMarketDepthData = m_sMarketDepthData.Remove(iPos);
                }

                UpdateListbox(m_sMarketDepthData);

                // clear the m_sMarketDepthData to verify it is empty for the next read off the socket
                m_sMarketDepthData = "";
                // call wait for data to notify the socket that we are ready to receive another callback
                WaitForData("MarketDepth");
                LimitListItems();
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
                // check if we need to invoke the delegate
                if (lstData.InvokeRequired)
                {
                    // call this function again using a delegate
                    this.Invoke(new UpdateDataHandler(UpdateListbox), sData);
                }
                else
                {
                    // delegate not required, just update the list box.
                    List<String> lstMessages = new List<string>(sData.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                    lstData.BeginUpdate();
                    lstMessages.ForEach(delegate(String sLine)
                    {
                        lstData.Items.Insert(0, sLine);
                    });
                    // Since this is just a sample app, for efficiency, we limit the results to the last 100 messages received.
                    while (lstData.Items.Count > 100)
                    {
                        lstData.Items.RemoveAt(100);
                    }
                    lstData.EndUpdate();
                }
            }
            catch (ObjectDisposedException)
            {
                // The list view object went away, ignore it since we're probably exiting.
            }
        }

        /// <summary>
        /// Handles when the user clicks the Watch button
        ///     Sends a request to IQFEED to start receiving updates for the specified symbol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWatch_Click(object sender, EventArgs e)
        {
            // When you watch a symbol, you will get a snapshot of data that is currently available in the servers
            // for each Market Maker or Depth level (depending on data type) for that symbol.  
            // After that, you will continue to get Update messages anytime a field is updated until you issue an unwatch for the symbol.

            // the command we need to send to watch a symbol is wSYMBOL\r\n
            string sCommand = String.Format("w{0}\r\n",txtRequest.Text);

            // and we send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            int iBytesSent = m_sockMarketDepth.Send(szCommand, iBytesToSend, SocketFlags.None);
            if (iBytesSent != iBytesToSend)
            {
                UpdateListbox(String.Format("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray())));
            }
        }

        /// <summary>
        /// Handles when the user clicks the Remove button
        ///     Sends a request to IQFEED to stop receiving updates for the specified symbol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            // when you remove a symbol, you simply tell the server that you no longer want to receive data for that symbol.

            // the command we need to send to unwatch a symbol is rSYMBOL\r\n
            string sCommand = String.Format("r{0}\r\n", txtRequest.Text);

            // and we send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            int iBytesSent = m_sockMarketDepth.Send(szCommand, iBytesToSend, SocketFlags.None);
            if (iBytesSent != iBytesToSend)
            {
                UpdateListbox(String.Format("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray())));
            }
        }

        /// <summary>
        /// Handles when the user clicks the MMID button
        ///     Sends a request to IQFEED for a MMID description.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMMID_Click(object sender, EventArgs e)
        {
            // Use this command to request the Description of a MMID.

            // the command we need to send to request a market maker ID is mMMID\r\n
            string sCommand = String.Format("m{0}\r\n", txtRequest.Text);

            // and we send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            int iBytesSent = m_sockMarketDepth.Send(szCommand, iBytesToSend, SocketFlags.None);
            if (iBytesSent != iBytesToSend)
            {
                UpdateListbox(String.Format("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray())));
            }
        }

        /// <summary>
        /// Handles when the user clicks the Connect button
        ///     Sends a request to IQFEED to connect to the MarketDepth Server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // This command tells IQFeed to connect to the server.  The only time you will ever need to use this command
            // is if you tell it to disconnect.  All other types of connection/disconnection should be handled automatically.

            // the command we need to send to connect is c\r\n
            string sCommand = "c\r\n";

            // and we send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            int iBytesSent = m_sockMarketDepth.Send(szCommand, iBytesToSend, SocketFlags.None);
            if (iBytesSent != iBytesToSend)
            {
                UpdateListbox(String.Format("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray())));
            }
        }

        /// <summary>
        /// Handles when the user clicks the Disconnect button
        ///     Sends a request to IQFEED to disconnect from the MarketDepth Server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // This command tells IQFeed to disconnect from the server.  The only time you will ever need to use this command
            // is if your app needs the feed to stop temporarily (most likely for troubleshooting).  
            // DO NOT SEND THIS COMMAND WHEN SHUTTING DOWN YOUR APP!  All your app should do when shutting down is close your
            // socket connection and the feed will handle everything else

            // the command we need to send to disconnect is x\r\n
            string sCommand = "x\r\n";

            // and we send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            int iBytesSent = m_sockMarketDepth.Send(szCommand, iBytesToSend, SocketFlags.None);
            if (iBytesSent != iBytesToSend)
            {
                UpdateListbox("Error: Disconnect command not sent to IQConnect");
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

        /// <summary>
        /// Since this is just a sample app, for efficiency, we limit the results to the last 100 messages received.
        /// </summary>
        private void LimitListItems()
        {
            try
            {
                // check if we need a delegate for this call
                if (lstData.InvokeRequired)
                {
                    this.Invoke(new UpdateControlsHandler(LimitListItems));
                }
                else
                {
                    while (lstData.Items.Count > 100)
                    {
                        lstData.Items.RemoveAt(100);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // the listbox went away.  Ingore it since we are probably exiting
            }
        }
    }
}