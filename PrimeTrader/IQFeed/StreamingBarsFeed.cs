using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using IQ_Config_Namespace;
using Tools;
using static Tools.G;
using static IQFeed.Helper;

namespace IQFeed
{
    public class StreamingBarsFeed
    {
        public string RequestID { get; set; }
        public int MaxDatapointsToReceive { get; set; }
        public int UpdateIntervalInSeconds { get; set; }

        // global variables for socket communications to the derivative socket
        AsyncCallback m_pfnDerivativeCallback;
        Socket m_sockDerivative;
        
        byte[] m_szDerivativeSocketBuffer = new byte[8096];                                 // we create the socket buffer global for performance        
        string m_sDerivativeIncompleteRecord = "";                                          // stores unprocessed data between reads from the socket        
        bool m_bDerivativeNeedBeginReceive = true;                                          // flag for tracking when a call to BeginReceive needs called


        public delegate void BarUpdateHandler(BarUpdateIQ update);
        public event BarUpdateHandler UpdateBars;

        private static StreamingBarsFeed m_instance;
        public static StreamingBarsFeed Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new StreamingBarsFeed();
                return m_instance;
            }
        }                                       // Singleton

        // Constructor for the form
        private StreamingBarsFeed()
        {
            this.RequestID = "STREAMING";
            this.MaxDatapointsToReceive = 100;
            this.UpdateIntervalInSeconds = 0;

            InitializeConnection();
        }

        // Initialize the connection to IQFeed
        private void InitializeConnection()
        {
            IQ_Config config = new IQ_Config();
            // create the socket and tell it to connect
            m_sockDerivative = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipLocalhost = IPAddress.Parse("127.0.0.1");

            // pull the level 1 port out of the registry.  we use the Level 1 port because we want streaming updates
            int iPort = GetIQFeedPort("Derivative");

            IPEndPoint ipendLocalhost = new IPEndPoint(ipLocalhost, iPort);

            try
            {
                // tell the socket to connect to IQFeed
                m_sockDerivative.Connect(ipendLocalhost);

                // Set the protocol for the socket
                SendRequestToIQFeed(String.Format("S,SET PROTOCOL,{0}\r\n", config.getProtocol()));

                // this example is using asynchronous sockets to communicate with the feed.  As a result, we are using .NET's BeginReceive and EndReceive calls with a callback.
                // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
                WaitForData("Derivative");
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
                        //UpdateListview(sLine);
                        dout(sLine.Trim());
                        // move on to the next message.  This isn't very efficient but it is simple (which is the focus of this example).
                        sData = sData.Substring(sLine.Length + 1);
                        if (sLine.StartsWith(string.Format("{0},BH,", this.RequestID)) || sLine.StartsWith(string.Format("{0},BC,", this.RequestID)))
                        {
                            UpdateBars?.Invoke(new BarUpdateIQ(sLine));

                        }
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
        /// Update / create a bar watch using the information in the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SubscribeBars(string symbol, HistoryIntervalType intervalType, int intervalValue, int numberOfDaysBack=1, string beginFilterTime="", string endFilterTime="", DateTime? beginDateTime=null)
        {
            string sBeginDateTime = "19700101000000";       // 1/1/1970 00:00:00
            if (beginDateTime != null)
                sBeginDateTime = beginDateTime.Value.ToIQString();

            // the command we need to send to turn on news is wSYMBOL\r\n
            string sCommand;
            sCommand = String.Format("BW,{0},{1},{2},{3},{4},{5},{6},{7},{8},,{9}\r\n",
                symbol,
                intervalValue,
                sBeginDateTime,
                numberOfDaysBack,
                this.MaxDatapointsToReceive,
                beginFilterTime,
                endFilterTime,
                this.RequestID,
                GetIntervalType(intervalType),                
                this.UpdateIntervalInSeconds);

            SendRequestToIQFeed(sCommand);
        }

        // Unwatch a bar request based on the symbol and request ID in the form
        public void UnsubscribeBars(string symbol)
        {
            string sCommand = String.Format("BR,{0},{1}\r\n", symbol, this.RequestID);
            SendRequestToIQFeed(sCommand);
        }

        // Request all the current bar watches
        public void RequestCurrentSubscriptions()
        {
            SendRequestToIQFeed("S,REQUEST WATCHES\r\n");
        }

        // Unwatch all the current bar watches
        public void UnsubscribeAll()
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
            //lstData.Items.Clear();

            // send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            try
            {
                int iBytesSent = m_sockDerivative.Send(szCommand, iBytesToSend, SocketFlags.None);
                if (iBytesSent != iBytesToSend)
                {
                    ErrorMessage("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray()));
                }
                else
                {
                    dout("Sent command: {0}", sCommand.TrimEnd("\r\n".ToCharArray()));
                }
            }
            catch (SocketException ex)
            {
                // handle socket errors
                ErrorMessage("Socket Error Sending Request:\r\n{0}\r\n{1}", sCommand.TrimEnd("\r\n".ToCharArray()), ex.Message);
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
            MyRegistryKey key = MyRegistry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup", true);
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


    } // end of class
} // end of namespace
