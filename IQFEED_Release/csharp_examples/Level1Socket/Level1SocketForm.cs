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

namespace Level1Socket
{
    public partial class Level1SocketForm : Form
    {
        // global variables for socket communications to the level1 socket
        AsyncCallback m_pfnLevel1Callback;
        Socket m_sockLevel1;
        // we create the socket buffer global for performance
        byte[] m_szLevel1SocketBuffer = new byte[8096];
        // stores unprocessed data between reads from the socket
        string m_sLevel1IncompleteRecord = "";
        // flag for tracking when a call to BeginReceive needs called
        bool m_bLevel1NeedBeginReceive = true;

        // delegate for updating the data display.
        public delegate void UpdateDataHandler(string sMessage);
        // delegate for updating the controls
        public delegate void UpdateControlsHandler();

        /// <summary>
        /// Constructor for the form
        /// </summary>
        public Level1SocketForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls and initializing the connection to IQFeed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Level1SocketForm_Load(object sender, EventArgs e)
        {
            IQ_Config config = new IQ_Config();
            // create the socket and tell it to connect
            m_sockLevel1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipLocalhost = IPAddress.Parse("127.0.0.1");

            // pull the level 1 port out of the registry.  we use the Level 1 port because we want streaming updates
            int iPort = GetIQFeedPort("Level1");

            IPEndPoint ipendLocalhost = new IPEndPoint(ipLocalhost, iPort);

            try
            {
                // tell the socket to connect to IQFeed
                m_sockLevel1.Connect(ipendLocalhost);

                // Set the protocol for the socket
                SendRequestToIQFeed(String.Format("S,SET PROTOCOL,{0}\r\n",config.getProtocol()));

                // this example is using asynchronous sockets to communicate with the feed.  As a result, we are using .NET's BeginReceive and EndReceive calls with a callback.
                // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
                WaitForData("Level1");
            }
            catch (SocketException ex)
            {
                MessageBox.Show(String.Format("Oops.  Did you forget to Login first?\nTake a Look at the LaunchingTheFeed example app\n{0}", ex.Message), "Error Connecting to IQFeed");
                DisableForm();
            }
        }

        /// <summary>
        /// Since we are using an async socket, we just tell the socket that we are ready to recieve data.
        /// The .NET framework will then call our callback (OnReceive) when there is new data to be read off the socket
        /// </summary>
        private void WaitForData(string sSocketName)
        {
            if (sSocketName.Equals("Level1"))
            {
                // make sure we have a callback created
                if (m_pfnLevel1Callback == null)
                {
                    m_pfnLevel1Callback = new AsyncCallback(OnReceive);
                }

                // send the notification to the socket.  It is very important that we don't call Begin Reveive more than once per call
                // to EndReceive.  As a result, we set a flag to ignore multiple calls.
                if (m_bLevel1NeedBeginReceive)
                {
                    m_bLevel1NeedBeginReceive = false;
                    // we pass in the sSocketName in the state parameter so that we can verify the socket data we receive is the data we are looking for
                    m_sockLevel1.BeginReceive(m_szLevel1SocketBuffer, 0, m_szLevel1SocketBuffer.Length, SocketFlags.None, m_pfnLevel1Callback, sSocketName);
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
            if (asyn.AsyncState.ToString().Equals("Level1"))
            {
                // read data from the socket.
                int iReceivedBytes = 0;
                iReceivedBytes = m_sockLevel1.EndReceive(asyn);
                // set our flag back to true so we can call begin receive again
                m_bLevel1NeedBeginReceive = true;
                // in this example, we will convert to a string for ease of use.
                string sData = Encoding.ASCII.GetString(m_szLevel1SocketBuffer, 0, iReceivedBytes);

                // When data is read from the socket, you can get multiple messages at a time and there is no guarantee
                // that the last message you receive will be complete.  It is possible that only half a message will be read
                // this time and you will receive the 2nd half of the message at the next call to OnReceive.
                // As a result, we need to save off any incomplete messages while processing the data and add them to the beginning
                // of the data next time.
                sData = m_sLevel1IncompleteRecord + sData;
                // clear our incomplete record string so it doesn't get processed next time too.
                m_sLevel1IncompleteRecord = "";

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
                        // we know what type of message was recieved by the first character in the message.
                        switch (sLine[0])
                        {
                            case 'Q':
                                ProcessUpdateMsg(sLine);
                                break;
                            case 'F':
                                ProcessFundamentalMsg(sLine);
                                break;
                            case 'P':
                                ProcessSummaryMsg(sLine);
                                break;
                            case 'N':
                                ProcessNewsHeadlineMsg(sLine);
                                break;
                            case 'S':
                                ProcessSystemMsg(sLine);
                                break;
                            case 'R':
                                ProcessRegionalMsg(sLine);
                                break;
                            case 'T':
                                ProcessTimestamp(sLine);
                                break;
                            case 'E':
                                ProcessErrorMsg(sLine);
                                break;
                            default:
                                // we processed something else we weren't expecting.  Ignore it
                                break;
                        }
                        // move on to the next message.  This isn't very efficient but it is simple (which is the focus of this example).
                        sData = sData.Substring(sLine.Length + 1);
                    }
                    else
                    {
                        // we get here when there are no more newline characters in the data.  
                        // save off the rest of message for processing the next batch of data.
                        m_sLevel1IncompleteRecord = sData;
                        sData = "";
                    }
                }

                // call wait for data to notify the socket that we are ready to receive another callback
                WaitForData("Level1");
                LimitListItems();
            }
        }

        /// <summary>
        /// Process an update message from the feed.
        /// </summary>
        /// <param name="sLine"></param>
        private void ProcessUpdateMsg(string sLine)
        {
            // Update messages are sent to the client anytime one of the fields in the current fieldset are updated.
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the update message, please check the documentation page UpdateSummary Message Format.
            UpdateListbox(sLine);
        }

        /// <summary>
        /// Process a fundamental message from the feed
        /// </summary>
        /// <param name="sLine"></param>
        private void ProcessFundamentalMsg(string sLine)
        {
            // fundamental data will contain data about the stock symbol that does not frequently change (at most once a day).
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the fundamental message, please check the documentation page Fundamental Message Format.
            UpdateListbox(sLine);
        }

        /// <summary>
        /// Process a summary message from the feed.
        /// </summary>
        /// <param name="sLine"></param>
        private void ProcessSummaryMsg(string sLine)
        {
            // summary data will be in the same format as the Update messages and will contain the most recent data for each field at the 
            //      time you watch the symbol.
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the Summary message, please check the documentation page UpdateSummaryMessage Format.
            UpdateListbox(sLine);
        }

        /// <summary>
        /// Process a news headline message from the feed.
        /// </summary>
        /// <param name="sLine"></param>
        private void ProcessNewsHeadlineMsg(string sLine)
        {
            // News messages are received anytime a new news story is received for a news type you are authorized to receive AND only when you have streaming news turned on
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the news message, please check the documentation page Streaming News Data Message Format.
            UpdateListbox(sLine);
        }

        /// <summary>
        /// Process a system message from the feed
        /// </summary>
        /// <param name="sLine"></param>
        private void ProcessSystemMsg(string sLine)
        {
            // system messages are sent to inform the client about current system information.
            // In this example, we just display the data to the user.
            // For a list of system messages that can be sent and the fields each contains, please check the documentation page System Messages.
            UpdateListbox(sLine);
        }

        /// <summary>
        /// Process a timestamp message from the feed.
        /// </summary>
        /// <param name="sLine"></param>
        private void ProcessTimestamp(string sLine)
        {
            // Timestamp messages are sent to the client once a second.  These timestamps are generated by our servers and can be used as a "server time"
            // In this example, we just display the data to the user.
            UpdateListbox(sLine);
        }

        /// <summary>
        /// Process an error message from the feed
        /// </summary>
        /// <param name="sLine"></param>
        private void ProcessErrorMsg(string sLine)
        {
            // Error messages are sent to the client to inform the client of problems.
            // In this example, we just display the data to the user.
            UpdateListbox(sLine);
        }

        /// <summary>
        /// Process a regional message from the feed.
        /// </summary>
        /// <param name="sLine"></param>
        private void ProcessRegionalMsg(string sLine)
        {
            // Regional messages are sent to the client anytime one of the fields for a region updates AND only when the client has requested 
            //      to watch regionals for a specific symbol.
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the regional message, please check the documentation page Regional Message Format.
            UpdateListbox(sLine);
        }

        /// <summary>
        /// we have to use a delegate to update controls in the dialog to resolve cross-threading issues built into the .NET framework
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
                    LimitListItems();
                    lstData.EndUpdate();
                }
            }
            catch (ObjectDisposedException)
            {
                // The list view object went away, ignore it since we're probably exiting.
            }
        }

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

            // the command we need to send to watch a symbol is wSYMBOL\r\n
            SendRequestToIQFeed(String.Format("w{0}\r\n", txtRequest.Text));
        }

        /// <summary>
        /// Event that fires when the Trades Only Watch Button is pressed.  Sends the trades only watch command to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTradesOnly_Click(object sender, EventArgs e)
        {
            // When you issue a trades only watch, you will get a snapshot of data that is currently available in the servers
            // for that symbol.  The snapshot will include a Fundamental message followed by a Summary message and then
            // you will continue to get Update messages anytime a trade occurs until you issue an unwatch for the symbol.

            // the command we need to send to issue a trades only watch is tSYMBOL\r\n
            SendRequestToIQFeed(String.Format("t{0}\r\n", txtRequest.Text));
        }

        /// <summary>
        /// Event that fires when the Remove Button is pressed.  Sends the remove watch command to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            // when you remove a symbol, you simply tell the server that you no longer want to receive data for that symbol.

            // the command we need to send to remove a symbol is rSYMBOL\r\n
            SendRequestToIQFeed(String.Format("r{0}\r\n", txtRequest.Text));
        }

        /// <summary>
        /// Event that fires when the Watch Regionals Button is pressed.  Sends the watch regionals command to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWatchRegionals_Click(object sender, EventArgs e)
        {
            // Issuing a regional watch for a symbol will also automatically issue a regular watch 
            // In addition to the messages you would expect to receive with a regular watch request, you will also
            // receive all of the current Best Bid/Offer for each regional exchange.

            // the command we need to send to turn on regionals is S,REGON,SYMBOL\r\n
            SendRequestToIQFeed(String.Format("S,REGON,{0}\r\n", txtRequest.Text));
        }

        /// <summary>
        /// Event that fires when the Remove Regionals Button is pressed.  Sends the remove regionals watch command to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveRegionals_Click(object sender, EventArgs e)
        {
            // Send a regional unwatch command if you no longer want to receive regional messages for a symbol but you 
            // still want to receive regular watch messages.  If you want to completely unwatch the symbol, you need to
            // issue a regular unwatch request.

            // the command we need to send to turn off regionals is S,REGOFF,SYMBOL\r\n
            SendRequestToIQFeed(String.Format("S,REGOFF,{0}\r\n", txtRequest.Text));
        }

        /// <summary>
        /// Event that fires when the Timestamp Button is pressed.  Sends the timestamp request command to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTimestamp_Click(object sender, EventArgs e)
        {
            // You can request a timestamp from the servers at anytime by sending a Timestamp request.

            // the command we need to send to request a timestamp is T\r\n
            SendRequestToIQFeed("T\r\n");
        }

        /// <summary>
        /// Event that fires when the Force Button is pressed.  Sends the force command to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForce_Click(object sender, EventArgs e)
        {
            // The force command can be used for force a new snapshot (fundamental and summary message) from the server
            // for any symbol you are currently watching.

            // the command we need to send to force a symbol is fSYMBOL\r\n
            SendRequestToIQFeed(String.Format("f{0}\r\n", txtRequest.Text));
        }

        /// <summary>
        /// Event that fires when the News On Button is pressed.  Sends the command to start receiving streaming news headlines to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewsOn_Click(object sender, EventArgs e)
        {
            // You can receive news headlines inline with your streaming quotes for any news types your account is authorized.  To start
            // receiving news headlines, send a News On request to the feed.

            // the command we need to send to turn on news is S,NEWSON\r\n
            SendRequestToIQFeed("S,NEWSON\r\n");
        }

        /// <summary>
        /// Event that fires when the News Off Button is pressed.  Sends the command to stop receiving streaming news headlines to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewsOff_Click(object sender, EventArgs e)
        {
            // If you no longer want to receve the streaming news headlines.  Send a News Off request.

            // the command we need to send to turn on news is S,NEWSOFF\r\n
            SendRequestToIQFeed("S,NEWSOFF\r\n");
        }

        /// <summary>
        /// Event that fires when the Set Fieldset Button is pressed.  Sends the command for changing fieldsets to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetFieldset_Click(object sender, EventArgs e)
        {
            // You can adjust the fields you receive in Update messages by issuing a Set Fieldset command.
            // this enables you to reduce the number of unwanted fields you get in each update message and 
            // reduce the number of messages (if no fields are updated, your app will not recieve an update message)

            // the command we need to send to select a new fieldset is S,SELECT UPDATE FIELDS,FIELD1,FIELD2\r\n
            SendRequestToIQFeed(String.Format("S,SELECT UPDATE FIELDS,{0}\r\n", txtRequest.Text));
        }

        /// <summary>
        /// Event that fires when the Get Fieldset Button is pressed.  Sends the command to retrieve the current fieldset to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetFieldset_Click(object sender, EventArgs e)
        {
            // You can query IQFeed to find out what the current fieldset is using a GetFieldsets

            // the command we need to send to get the current fieldset  is S,REQUEST CURRENT UPDATE FIELDNAMES\r\n
            SendRequestToIQFeed("S,REQUEST CURRENT UPDATE FIELDNAMES\r\n");
        }

        /// <summary>
        /// Event that fires when the Connect Button is pressed.  Sends the command to tell iqconnect to connect to the server
        ///     via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // This command tells IQFeed to connect to the server.  The only time you will ever need to use this command
            // is if you tell it to disconnect.  All other types of connection/disconnection should be handled automatically.

            // the command we need to send to connect is S,CONNECT\r\n
            SendRequestToIQFeed("S,CONNECT\r\n");
        }

        /// <summary>
        /// Event that fires when the Connect Button is pressed.  Sends the command to tell iqconnect to disconnect from the server
        ///     via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // This command tells IQFeed to disconnect from the server.  The only time you will ever need to use this command
            // is if your app needs the feed to stop temporarily (most likely for troubleshooting).  
            // DO NOT SEND THIS COMMAND WHEN SHUTTING DOWN YOUR APP!  All your app should do when shutting down is close your
            // socket connection and the feed will handle everything else

            // the command we need to send to disconnect is S,DISCONNECT\r\n
            SendRequestToIQFeed("S,DISCONNECT\r\n");
        }

        /// <summary>
        /// Event that fires when the Get Watches Button is pressed.  Sends the command to retrieve the current symbollist that 
        ///     you are watching to the server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetCurrentWatches_Click(object sender, EventArgs e)
        {
            // You can request a list of your currently watched symbols on this socket connection.

            // the command we need to send to get current watches is S,REQUEST WATCHES\r\n
            SendRequestToIQFeed("S,REQUEST WATCHES\r\n");
        }

        /// <summary> 
        /// Event that fires when the Remove All Watches Button is pressed.  Sends the command to remove all current watches to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveAllWatches_Click(object sender, EventArgs e)
        {
            // if you need to unwatch all symbols, you can use this command instead of cycling through each symbol you have watched previously.

            // the command we need to send to unwatch all symbols is S,UNWATCH ALL\r\n
            SendRequestToIQFeed("S,UNWATCH ALL\r\n");
        }

        /// <summary>
        /// Event that fires when the Get Fundamental Fields Button is pressed.  Sends the command to get all fields to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetFundamentalFields_Click(object sender, EventArgs e)
        {
            // the command we need to send to get all fundamental fields is S,REQUEST FUNDAMENTAL FIELDNAMES\r\n
            SendRequestToIQFeed("S,REQUEST FUNDAMENTAL FIELDNAMES\r\n");
        }

        /// <summary>
        /// Event that fires when the Get Update/Summary Fields Button is pressed.  Sends the command to get all fields to the 
        ///     server via the Level 1 socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetUpdateSummaryFields_Click(object sender, EventArgs e)
        {
            // the command we need to send to get all update fields is S,REQUEST ALL UPDATE FIELDNAMES\r\n
            SendRequestToIQFeed("S,REQUEST ALL UPDATE FIELDNAMES\r\n");
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
                int iBytesSent = m_sockLevel1.Send(szCommand, iBytesToSend, SocketFlags.None);
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