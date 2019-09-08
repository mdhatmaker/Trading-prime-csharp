using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
//using System.Windows.Forms;
// added for access to RegistryKey
//using Microsoft.Win32;
// added for access to socket classes
using System.Net;
using System.Net.Sockets;
using System.Threading;
using IQ_Config_Namespace;
using Tools;
using static Tools.G;

namespace IQFeed
{
    public class HistorySocketThread
    {
        AsyncCallback m_pfnLookupCallback;                      // socket communication global variables
        Socket m_sockLookup;                                    // socket communication global variables

        byte[] m_szLookupSocketBuffer = new byte[262144];       // we create the socket buffer global for performance
        string m_sLookupIncompleteRecord = "";                  // stores unprocessed data between reads from the socket    
        bool m_bLookupNeedBeginReceive = true;                  // flag for tracking when a call to BeginReceive needs called

        public delegate void UpdateDataHandler(string sMessage);    // delegate for updating the data display

        bool m_waitingForData = false;                              // TODO: make this bool thread-safe
        List<string> m_receivedData = new List<string>();           // received message data goes here

        private object m_Lock = new object();

        public bool WaitingForData {
            get { lock (m_Lock) { return m_waitingForData; } }
            set { lock (m_Lock) { m_waitingForData = value; } }
        }

        public List<string> ReceivedData { get { lock (m_Lock) { return m_receivedData; } } }

        public HistorySocketThread()
        {
            IQ_Config config = new IQ_Config();

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
                SendRequestToIQFeed(String.Format("S,SET PROTOCOL,{0}\r\n", config.getProtocol()));
            }
            catch (SocketException ex)
            {
                ErrorMessage("Could not create HistorySocketThread.  Did you forget to Login to IQFeed?\n{0}\n{1}", ex.Message, "Error Connecting to IQFeed");
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

                try
                {
                    // send the notification to the socket.  It is very important that we don't call Begin Reveive more than once per call
                    // to EndReceive.  As a result, we set a flag to ignore multiple calls.
                    if (m_bLookupNeedBeginReceive)
                    {
                        m_bLookupNeedBeginReceive = false;
                        // we pass in the sSocketName in the state parameter so that we can verify the socket data we receive is the data we are looking for
                        m_sockLookup.BeginReceive(m_szLookupSocketBuffer, 0, m_szLookupSocketBuffer.Length, SocketFlags.None, m_pfnLookupCallback, sSocketName);
                    }
                }
                catch (SocketException ex)
                {
                    m_bLookupNeedBeginReceive = true;
                    m_waitingForData = false;
                    ErrorMessage("Socket Error BeginReceive: SocketName='{0}'\r\n{1}", sSocketName, ex.Message);
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

                bool isEndOfMsg = false;

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
                    isEndOfMsg = true;
                }

                // update our internal list of received data
                if (isEndOfMsg == false)
                    UpdateReceived(sData);

                // clear the m_strData to verify it is empty for the next read off the socket
                sData = "";

                if (!isEndOfMsg)
                {
                    WaitForData("History");
                }

                m_waitingForData = false;

                /*if (isEndOfMsg)
                {
                    m_waitingForData = false;
                }

                // call wait for data to notify the socket that we are ready to receive another callback
                WaitForData("History");*/
            }
        }

        /// <summary>
        /// Sends a string to the socket connected to IQFeed
        /// </summary>
        /// <param name="sCommand"></param>
        private void SendRequestToIQFeed(string sCommand)
        {
            //tssMain.Text = sCommand;
            //Clipboard.SetData(DataFormats.Text, (Object)sCommand);

            // and we send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            try
            {
                int iBytesSent = m_sockLookup.Send(szCommand, iBytesToSend, SocketFlags.None);
                if (iBytesSent != iBytesToSend)
                {
                    Console.WriteLine(String.Format("Error Sending Request:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
                else
                {
                    Console.WriteLine(String.Format("Request Sent Successfully:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
            }
            catch (SocketException ex)
            {
                // handle socket errors
                ErrorMessage("Socket Error Sending Request:\r\n{0}\r\n{1}", sCommand.TrimEnd("\r\n".ToCharArray()), ex.Message);
            }
        }

        private void UpdateReceived(string sData)
        {
            List<String> lstMessages = new List<string>(sData.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            lock (m_Lock)
            {
                foreach (string msg in lstMessages)
                {
                    m_receivedData.Add(msg);
                }
            }
        }

        // This method can be called as a thread (process a raw request)
        // DON'T FORGET TO SET WaitingForData property TO True BEFORE CALLING
        public void SendRequest(string sRequest)
        {
            if (!sRequest.EndsWith("\r\n"))
                sRequest += "\r\n";

            lock (m_Lock) { m_receivedData.Clear(); }
            SendRequestToIQFeed(sRequest);
            WaitForData("History");
        }

        // This method can be called as a thread
        // DON'T FORGET TO SET WaitingForData property TO True BEFORE CALLING
        public void GetHistory(string symbol, int dayCount)
        {
            lock (m_Lock) { m_receivedData.Clear(); }
            string sRequest = string.Format("HID,{0},,{1},,,,,,,s\r\n", symbol, dayCount);
            SendRequestToIQFeed(sRequest);
            WaitForData("History");
        }


    } // end of class
} // end of namespace
