using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using IQ_Config_Namespace;
using Tools;
using static Tools.G;

namespace IQFeed
{
    public class PriceFeed
    {

        // global variables for socket communications to the level1 socket
        private AsyncCallback m_pfnLevel1Callback;
        private Socket m_sockLevel1;

        private byte[] m_szLevel1SocketBuffer = new byte[8096];     // we create the socket buffer global for performance        
        private string m_sLevel1IncompleteRecord = "";              // stores unprocessed data between reads from the socket        
        private bool m_bLevel1NeedBeginReceive = true;              // flag for tracking when a call to BeginReceive needs called
        private string m_serverTime = "";                           // we'll update this when we receive Timestamp messages from the server

        public delegate void PriceUpdateHandler(PriceUpdateIQ update);
        public event PriceUpdateHandler UpdatePrices;

        private static PriceFeed m_instance;
        public static PriceFeed Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new PriceFeed();
                return m_instance;
            }
        }

        // Constructor for the form
        private PriceFeed()
        {
            InitializeConnection();
        }

        // Initialize the connection to IQFeed
        private void InitializeConnection()
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
                SendRequestToIQFeed(String.Format("S,SET PROTOCOL,{0}\r\n", config.getProtocol()));

                // this example is using asynchronous sockets to communicate with the feed.  As a result, we are using .NET's BeginReceive and EndReceive calls with a callback.
                // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
                WaitForData("Level1");
            }
            catch (SocketException ex)
            {
                ErrorMessage("Did you forget to Login to IQFeed?\n{0}", ex.Message);
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
            }
        }

        // Process an update message from the feed.
        PriceUpdateIQ m_priceUpdate = new PriceUpdateIQ();
        private void ProcessUpdateMsg(string sLine)
        {
            // Update messages are sent to the client anytime one of the fields in the current fieldset are updated.
            //var x = "Q,@ESU17,2463.50,2,08:26:54.256829,43,170391,2463.25,116,2463.50,159,2460.25,2463.75,2456.50,2459.75,a,01,";

            m_priceUpdate.FromUpdateMsg(sLine);
            UpdatePrices?.Invoke(m_priceUpdate);
        }

        #region ProcessXXXMsg methods to process various IQFeed messages
        // Process a fundamental message from the feed
        private void ProcessFundamentalMsg(string sLine)
        {
            // fundamental data will contain data about the stock symbol that does not frequently change (at most once a day).
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the fundamental message, please check the documentation page Fundamental Message Format.
            output(sLine);
        }

        // Process a summary message from the feed.
        private void ProcessSummaryMsg(string sLine)
        {
            // Summary data will be in the same format as the Update messages and will contain the most recent data for each field at the time you watch the symbol.
            //var x = "P,@RB#,0.15150,0,20:10:53.968110,110,0,,,0.15058,1,,,,0.14994,Cascv,01,"

            m_priceUpdate.FromUpdateMsg(sLine);
            UpdatePrices?.Invoke(m_priceUpdate);
            output(sLine);
        }

        // Process a news headline message from the feed.
        private void ProcessNewsHeadlineMsg(string sLine)
        {
            // News messages are received anytime a new news story is received for a news type you are authorized to receive AND only when you have streaming news turned on
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the news message, please check the documentation page Streaming News Data Message Format.
            output(sLine);
        }

        // Process a system message from the feed
        private void ProcessSystemMsg(string sLine)
        {
            // system messages are sent to inform the client about current system information.
            // In this example, we just display the data to the user.
            // For a list of system messages that can be sent and the fields each contains, please check the documentation page System Messages.
            output(sLine);
        }

        // Process a timestamp message from the feed.
        private void ProcessTimestamp(string sLine)
        {
            // Timestamp messages are sent to the client once a second.  These timestamps are generated by our servers and can be used as a "server time"
            // In this example, we just display the data to the user.            
            //output(sLine);
            m_serverTime = sLine;
        }

        // Process an error message from the feed
        private void ProcessErrorMsg(string sLine)
        {
            // Error messages are sent to the client to inform the client of problems.
            // In this example, we just display the data to the user.
            output(sLine);
        }

        // Process a regional message from the feed.
        private void ProcessRegionalMsg(string sLine)
        {
            // Regional messages are sent to the client anytime one of the fields for a region updates AND only when the client has requested 
            //      to watch regionals for a specific symbol.
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the regional message, please check the documentation page Regional Message Format.
            output(sLine);
        }
        #endregion

        public void SubscribePrices(string symbol)
        {
            // the command we need to send to watch a symbol is wSYMBOL\r\n
            SendRequestToIQFeed(String.Format("w{0}\r\n", symbol));
            SendRequestToIQFeed(String.Format("f{0}\r\n", symbol));                 // Forces a refresh from the server for the symbol specified
        }

        public void ForcePriceRefresh(string symbol)
        {
            // Forces a refresh from the server for the symbol specified
            SendRequestToIQFeed(String.Format("f{0}\r\n", symbol));
        }

        // Sends a string to the socket connected to IQFeed
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
                    output(String.Format("Error Sending Request:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
                else
                {
                    output(String.Format("Request Sent Successfully:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
            }
            catch (SocketException ex)
            {
                // handle socket errors
                ErrorMessage("Socket Error Sending Request:\n{0}\n{1}", sCommand.TrimEnd("\r\n".ToCharArray()), ex.Message);
            }
        }

        // Gets local IQFeed socket ports from the registry
        private int GetIQFeedPort(string sPort)
        {
            int iReturn = 0;
            //RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup");
            MyRegistryKey key = MyRegistry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup");
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

        // Sends the trades only watch command to the server via the Level 1 socket.
        public void TradesOnly(string symbol)
        {
            // When you issue a trades only watch, you will get a snapshot of data that is currently available in the servers
            // for that symbol.  The snapshot will include a Fundamental message followed by a Summary message and then
            // you will continue to get Update messages anytime a trade occurs until you issue an unwatch for the symbol.

            // the command we need to send to issue a trades only watch is tSYMBOL\r\n
            SendRequestToIQFeed(String.Format("t{0}\r\n", symbol));
        }

        // Sends the remove watch command to the server via the Level 1 socket.
        public void Remove(string symbol)
        {
            // when you remove a symbol, you simply tell the server that you no longer want to receive data for that symbol.

            // the command we need to send to remove a symbol is rSYMBOL\r\n
            SendRequestToIQFeed(String.Format("r{0}\r\n", symbol));
        }

        // Sends the watch regionals command to the server via the Level 1 socket.
        public void WatchRegionals(string symbol)
        {
            // Issuing a regional watch for a symbol will also automatically issue a regular watch 
            // In addition to the messages you would expect to receive with a regular watch request, you will also
            // receive all of the current Best Bid/Offer for each regional exchange.

            // the command we need to send to turn on regionals is S,REGON,SYMBOL\r\n
            SendRequestToIQFeed(String.Format("S,REGON,{0}\r\n", symbol));
        }

        // Sends the remove regionals watch command to the server via the Level 1 socket.
        public void RemoveRegionals(string symbol)
        {
            // Send a regional unwatch command if you no longer want to receive regional messages for a symbol but you 
            // still want to receive regular watch messages.  If you want to completely unwatch the symbol, you need to
            // issue a regular unwatch request.

            // the command we need to send to turn off regionals is S,REGOFF,SYMBOL\r\n
            SendRequestToIQFeed(String.Format("S,REGOFF,{0}\r\n", symbol));
        }

        // Sends the timestamp request command to the server via the Level 1 socket.
        public void Timestamp_Click()
        {
            // You can request a timestamp from the servers at anytime by sending a Timestamp request.

            // the command we need to send to request a timestamp is T\r\n
            SendRequestToIQFeed("T\r\n");
        }

        // Sends the force command to the server via the Level 1 socket.
        public void Force(string symbol)
        {
            // The force command can be used for force a new snapshot (fundamental and summary message) from the server
            // for any symbol you are currently watching.

            // the command we need to send to force a symbol is fSYMBOL\r\n
            SendRequestToIQFeed(String.Format("f{0}\r\n", symbol));
        }

        // Sends the command to start receiving streaming news headlines to the server via the Level 1 socket.
        public void NewsOn()
        {
            // You can receive news headlines inline with your streaming quotes for any news types your account is authorized.  To start
            // receiving news headlines, send a News On request to the feed.

            // the command we need to send to turn on news is S,NEWSON\r\n
            SendRequestToIQFeed("S,NEWSON\r\n");
        }

        // Sends the command to stop receiving streaming news headlines to the server via the Level 1 socket.
        public void NewsOff(object sender, EventArgs e)
        {
            // If you no longer want to receve the streaming news headlines.  Send a News Off request.

            // the command we need to send to turn on news is S,NEWSOFF\r\n
            SendRequestToIQFeed("S,NEWSOFF\r\n");
        }

        // Sends the command for changing fieldsets to the server via the Level 1 socket.
        public void SetFieldset(string fields)
        {
            // You can adjust the fields you receive in Update messages by issuing a Set Fieldset command.
            // this enables you to reduce the number of unwanted fields you get in each update message and 
            // reduce the number of messages (if no fields are updated, your app will not recieve an update message)

            // the command we need to send to select a new fieldset is S,SELECT UPDATE FIELDS,FIELD1,FIELD2\r\n
            SendRequestToIQFeed(String.Format("S,SELECT UPDATE FIELDS,{0}\r\n", fields));
        }

        // Sends the command to retrieve the current fieldset to the server via the Level 1 socket.
        public void GetFieldset()
        {
            // You can query IQFeed to find out what the current fieldset is using a GetFieldsets

            // the command we need to send to get the current fieldset  is S,REQUEST CURRENT UPDATE FIELDNAMES\r\n
            SendRequestToIQFeed("S,REQUEST CURRENT UPDATE FIELDNAMES\r\n");
        }

        // Sends the command to tell iqconnect to connect to the server via the Level 1 socket.
        public void Connect()
        {
            // This command tells IQFeed to connect to the server.  The only time you will ever need to use this command
            // is if you tell it to disconnect.  All other types of connection/disconnection should be handled automatically.

            // the command we need to send to connect is S,CONNECT\r\n
            SendRequestToIQFeed("S,CONNECT\r\n");
        }

        // Event that fires when the Connect Button is pressed.  Sends the command to tell iqconnect to disconnect from the server via the Level 1 socket.
        public void Disconnect()
        {
            // This command tells IQFeed to disconnect from the server.  The only time you will ever need to use this command
            // is if your app needs the feed to stop temporarily (most likely for troubleshooting).  
            // DO NOT SEND THIS COMMAND WHEN SHUTTING DOWN YOUR APP!  All your app should do when shutting down is close your
            // socket connection and the feed will handle everything else

            // the command we need to send to disconnect is S,DISCONNECT\r\n
            SendRequestToIQFeed("S,DISCONNECT\r\n");
        }

        // Sends the command to retrieve the current symbollist that you are watching to the server via the Level 1 socket.
        public void GetCurrentWatches()
        {
            // You can request a list of your currently watched symbols on this socket connection.

            // the command we need to send to get current watches is S,REQUEST WATCHES\r\n
            SendRequestToIQFeed("S,REQUEST WATCHES\r\n");
        }

        // Sends the command to remove all current watches to the server via the Level 1 socket.
        public void RemoveAllWatches()
        {
            // if you need to unwatch all symbols, you can use this command instead of cycling through each symbol you have watched previously.

            // the command we need to send to unwatch all symbols is S,UNWATCH ALL\r\n
            SendRequestToIQFeed("S,UNWATCH ALL\r\n");
        }

        // Sends the command to get all fields to the server via the Level 1 socket.
        public void GetFundamentalFields()
        {
            // the command we need to send to get all fundamental fields is S,REQUEST FUNDAMENTAL FIELDNAMES\r\n
            SendRequestToIQFeed("S,REQUEST FUNDAMENTAL FIELDNAMES\r\n");
        }

        // Sends the command to get all fields to the server via the Level 1 socket.
        public void GetUpdateSummaryFields()
        {
            // the command we need to send to get all update fields is S,REQUEST ALL UPDATE FIELDNAMES\r\n
            SendRequestToIQFeed("S,REQUEST ALL UPDATE FIELDNAMES\r\n");
        }

        private void output(string text)
        {
            dout(text);
        }

    } // end of class
} // end of namespace
