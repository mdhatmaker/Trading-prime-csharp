//-----------------------------------------------------------
//-----------------------------------------------------------
//
//             System: IQ Feed
//       Program Name: StreamingBarsSocket
//        Module Name: StreamingBarsSocket.cs
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
// Module Description: Implementation of Streaming Interval 
//                     Bars Socket Sample
//         References: None
//           Compiler: Microsoft Visual Studio Version 2010
//             Author: 
//        Modified By: 
//
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
using Tools;

namespace IQFeed
{
    public partial class StreamingBarsForm : Form
    {
        // global variables for socket communications to the derivative socket
        AsyncCallback m_pfnDerivativeCallback;
        Socket m_sockDerivative;
        // we create the socket buffer global for performance
        byte[] m_szDerivativeSocketBuffer = new byte[8096];
        // stores unprocessed data between reads from the socket
        string m_sDerivativeIncompleteRecord = "";
        // flag for tracking when a call to BeginReceive needs called
        bool m_bDerivativeNeedBeginReceive = true;

        // delegate for updating the data display.
        public delegate void UpdateDataHandler(string sMessage);


        private global::IQFeed.StreamingBarsFeed m_streamingBars;

        /// <summary>
        /// Constructor for the form
        /// </summary>
        public StreamingBarsForm()
        {
            InitializeComponent();

            m_streamingBars = global::IQFeed.StreamingBarsFeed.Instance;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_streamingBars.UpdateBars += M_streamingBars_UpdateStreamingBars;
            DateTime dt = new DateTime(2017, 10, 16);
            //cbIntervalType.SelectedValue 
            m_streamingBars.SubscribeBars(txtSymbol.Text, IntervalType.Second, int.Parse(txtIntervalValue.Text));
        }

        private void M_streamingBars_UpdateStreamingBars(BarUpdateIQ update)
        {
            //dout("PRICE UPDATE: {0}", update.LastTradePrice);
            /*if (label9.InvokeRequired)
                label9.Invoke(new Action<BarUpdateIQ>(M_streamingBars_UpdateStreamingBars), new object[] { update });
            else
                label9.Text = string.Format("{0}", update.LastTradePrice);*/

            UpdateListview(update.ToString());
        }


        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls and initializing the connection to IQFeed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StreamingBarsSocket_Load(object sender, EventArgs e)
        {
            // Setup the list control
            lstData.Columns.Add("Data received", -2);
            lstData.Columns[0].Width -= System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;

            // create the socket and tell it to connect
            m_sockDerivative = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipLocalhost = IPAddress.Parse("127.0.0.1");

            // pull the derivate port out of the registry.  we use the derivative port because we want streaming updates
            int iPort = GetIQFeedPort("Derivative");

            IPEndPoint ipendLocalhost = new IPEndPoint(ipLocalhost, iPort);
            IQ_Config config = new IQ_Config();
            try
            {
                // tell the socket to connect to IQFeed
                m_sockDerivative.Connect(ipendLocalhost);

                SendRequestToIQFeed(String.Format("S,SET PROTOCOL,{0}\r\n",config.getProtocol()));

                // this example is using asynchronous sockets to communicate with the feed.  As a result, we are using .NET's BeginReceive and EndReceive calls with a callback.
                // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
                WaitForData("Derivative");
            }
            catch (SocketException ex)
            {
                MessageBox.Show(String.Format("Oops.  Did you forget to Login first?\nTake a Look at the LaunchingTheFeed example app\n{0}", ex.Message), "Error Connecting to IQFeed");
                DisableForm();
            }

            cbIntervalType.SelectedIndex = 0;
            txtDatapoints.Text = "100";
            txtDays.Text = "1";
            txtIntervalValue.Text = "60";
            txtUpdateIntervalInSeconds.Text = "0";
            txtSymbol.Text = "@ESZ17";
        }

        /// <summary>
        /// Since we are using an async socket, we just tell the socket that we are ready to recieve data.
        /// The .NET framework will then call our callback (OnReceive) when there is new data to be read off the socket
        /// </summary>
        private void WaitForData(string sSocketName)
        {
            if (sSocketName.Equals("Derivative"))
            {
                // make sure we have a callback created
                if (m_pfnDerivativeCallback == null)
                {
                    m_pfnDerivativeCallback = new AsyncCallback(OnReceive);
                }

                // send the notification to the socket.  It is very important that we don't call Begin Reveive more than once per call
                // to EndReceive.  As a result, we set a flag to ignore multiple calls.
                if (m_bDerivativeNeedBeginReceive)
                {
                    m_bDerivativeNeedBeginReceive = false;
                    // we pass in the sSocketName in the state parameter so that we can verify the socket data we receive is the data we are looking for
                    m_sockDerivative.BeginReceive(m_szDerivativeSocketBuffer, 0, m_szDerivativeSocketBuffer.Length, SocketFlags.None, m_pfnDerivativeCallback, sSocketName);
                }
            }
        }

        /// <summary>
        /// OnReceive is our Callback that gets called by the .NET socket class when new data arrives on the socket
        /// </summary>
        /// <param name="asyn"></param>
        private void OnReceive(IAsyncResult asyn)
        {
            // first verify we received data from the correct socket.  This check isn't really necessary in this example since we 
            // only have a single socket but if we had multiple sockets, we could use this check to use the same callback to recieve data from
            // multiple sockets
            if (asyn.AsyncState.ToString().Equals("Derivative"))
            {
                // read data from the socket.
                int iReceivedBytes = 0;
                iReceivedBytes = m_sockDerivative.EndReceive(asyn);
                // set our flag back to true so we can call begin receive again
                m_bDerivativeNeedBeginReceive = true;
                // in this example, we will convert to a string for ease of use.
                string sData = Encoding.ASCII.GetString(m_szDerivativeSocketBuffer, 0, iReceivedBytes);

                // When data is read from the socket, you can get multiple messages at a time and there is no guarantee
                // that the last message you receive will be complete.  It is possible that only half a message will be read
                // this time and you will receive the 2nd half of the message at the next call to OnReceive.
                // As a result, we need to save off any incomplete messages while processing the data and add them to the beginning
                // of the data next time.
                sData = m_sDerivativeIncompleteRecord + sData;
                // clear our incomplete record string so it doesn't get processed next time too.
                m_sDerivativeIncompleteRecord = "";

                // now we loop through the data breaking it appart into messages.  Each message on this port is terminated
                // with a newline character ("\n")
                string sLine = "";
                int iNewLinePos = -1;
                while (sData.Length > 0)
                {
                    iNewLinePos = sData.IndexOf("\n");
                    if (iNewLinePos > 0)
                    {
                        sLine = sData.Substring(0, iNewLinePos);
                        UpdateListview(sLine);
                        // move on to the next message.  This isn't very efficient but it is simple (which is the focus of this example).
                        sData = sData.Substring(sLine.Length + 1);
                    }
                    else
                    {
                        // we get here when there are no more newline characters in the data.  
                        // save off the rest of message for processing the next batch of data.
                        m_sDerivativeIncompleteRecord = sData;
                        sData = "";
                    }
                }

                // call wait for data to notify the socket that we are ready to receive another callback
                WaitForData("Derivative");
            }
        }

        /// <summary>
        /// Update the list control with the new messages
        /// </summary>
        /// <param name="sData"></param>
        public void UpdateListview(string sData)
        {
            try
            {
                if (lstData.InvokeRequired)
                {
                    this.Invoke(new UpdateDataHandler(UpdateListview), sData);
                }
                else
                {
                    List<String> lstMessages = new List<string>(sData.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                    lstData.BeginUpdate();
                    foreach (string sLine in lstMessages)
                    {
                        lstData.Items.Insert(0, sLine);
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
        /// Update / create a bar watch using the information in the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWatch_Click(object sender, EventArgs e)
        {
            string sIntervalType = "s";
            if (cbIntervalType.SelectedItem.ToString() == "tick")
            {
                sIntervalType = "t";
            }
            else if (cbIntervalType.SelectedItem.ToString() == "volume")
            {
                sIntervalType = "v";
            }
            else if (cbIntervalType.SelectedItem.ToString() == "second")
            {
                sIntervalType = "s";
            }

            int iIntervalValue = 0;
            try
            {
                iIntervalValue = Convert.ToInt32(txtIntervalValue.Text);
            }
            catch (Exception)
            {
                iIntervalValue = 120;
                txtIntervalValue.Text = "120";
                // the only request that doesn't have a number of datapoints parameter is "Interval Timeframe"
                MessageBox.Show(String.Format("Interval value must be numeric.  Defaulting to {0}", iIntervalValue));
            }

            short sNumberOfDays = 0;
            try
            {
                sNumberOfDays = Convert.ToInt16(txtDays.Text);
            }
            catch (Exception)
            {
                sNumberOfDays = 0;
                txtDays.Text = "0";
                MessageBox.Show(String.Format("Number of days must be numeric.  Defaulting to {0}", sNumberOfDays));
            }

            int iNumberOfDatapoints = 0;
            try
            {
                iNumberOfDatapoints = Convert.ToInt32(txtDatapoints.Text);
            }
            catch (Exception)
            {
                iNumberOfDatapoints = 100;
                
                MessageBox.Show(String.Format("Number of datapoints must be numeric.  Defaulting to {0}", iNumberOfDatapoints));
            }

            short sUpdateIntervalInSeconds = 0;
            try
            {
                sUpdateIntervalInSeconds = Convert.ToInt16(txtUpdateIntervalInSeconds.Text);
            }
            catch (Exception)
            {
                sUpdateIntervalInSeconds = 0;
                txtUpdateIntervalInSeconds.Text = "0";
                MessageBox.Show(String.Format("Update interval must be numeric.  Defaulting to {0}", sUpdateIntervalInSeconds));
            }

            // the command we need to send to turn on news is wSYMBOL\r\n
            string sCommand;
            sCommand = String.Format("BW,{0},{1},{2},{3},{4},{5},{6},{7},{8},,{9}\r\n",
                txtSymbol.Text,
                iIntervalValue,
                txtBeginDateTime.Text,
                sNumberOfDays,
                iNumberOfDatapoints,
                txtBeginFilterTime.Text,
                txtEndFilterTime.Text,
                txtRequestID.Text,
                sIntervalType,
                sUpdateIntervalInSeconds);
                        
            SendRequestToIQFeed(sCommand);
        }

        /// <summary>
        /// Unwatch a bar request based on the symbol and request ID in the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnwatch_Click(object sender, EventArgs e)
        {
            string sCommand;
            sCommand = String.Format("BR,{0},{1}\r\n",
                txtSymbol.Text,
                txtRequestID.Text);

            SendRequestToIQFeed(sCommand);
        }

        /// <summary>
        /// Request all the current bar watches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRequestWatches_Click(object sender, EventArgs e)
        {
            SendRequestToIQFeed("S,REQUEST WATCHES\r\n");
        }

        /// <summary>
        /// Unwatch all the current bar watches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnwatchAll_Click(object sender, EventArgs e)
        {
            SendRequestToIQFeed("S,UNWATCH ALL\r\n");
        }

        /// <summary>
        /// Sends a string to the socket connected to IQFeed
        /// </summary>
        /// <param name="sCommand"></param>
        private void SendRequestToIQFeed(string sCommand)
        {
            // Clear the results
            lstData.Items.Clear();

            // send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            try
            {
                int iBytesSent = m_sockDerivative.Send(szCommand, iBytesToSend, SocketFlags.None);
                if (iBytesSent != iBytesToSend)
                {
                    UpdateListview(String.Format("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
                else
                {
                    UpdateListview(String.Format("Sent command: {0}", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
            }
            catch (SocketException ex)
            {
                // handle socket errors
                UpdateListview(String.Format("Socket Error Sending Request:\r\n{0}\r\n{1}", sCommand.TrimEnd("\r\n".ToCharArray()), ex.Message));
            }
        }

        /// <summary>
        /// Disables all the controls on the form.
        /// </summary>
        private void DisableForm()
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
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

        private void StreamingBarsSocketForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}