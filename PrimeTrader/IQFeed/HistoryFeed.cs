using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
//using System.Windows.Forms;
// added for access to RegistryKey
using Microsoft.Win32;
// added for access to socket classes
using System.Net;
using System.Net.Sockets;
using IQ_Config_Namespace;
using Tools;
using static Tools.G;

namespace IQFeed
{
    public class HistoryFeed
    {
        // socket communication global variables
        AsyncCallback m_pfnLookupCallback;
        Socket m_sockLookup;
        
        byte[] m_szLookupSocketBuffer = new byte[262144];                       // we create the socket buffer global for performance
        string m_sLookupIncompleteRecord = "";                                  // stores unprocessed data between reads from the socket
        bool m_bLookupNeedBeginReceive = true;                                  // flag for tracking when a call to BeginReceive needs called

        string m_protocolVersion;                                               // store the PROTOCOL version from the IQFeed API

        // delegate for updating the data display.
        public delegate void HistoryUpdateDataHandler(string sData);
        public event HistoryUpdateDataHandler UpdateHistory;

        public string ProtocolVersion { get { return m_protocolVersion; } }

        private static HistoryFeed m_instance;
        public static HistoryFeed Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new HistoryFeed();
                return m_instance;
            }
        }

        // Constructor for the form
        public HistoryFeed()
        {
            InitializeConnection();            
        }

        // Initialize the connection to IQFeed
        private void InitializeConnection()
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
                ErrorMessage("Did you forget to Login to IQFeed?\n{0}", ex.Message);
            }
        }

        /// <summary>
        /// we call this to notify the .NET Async socket to start listening for data to come in.  It must be called each time after we receive data
        /// </summary>
        private void WaitForData(string sSocketName)
        {
            try
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
            catch (SocketException ex)
            {
                ErrorMessage("Did you forget to Login to IQFeed?\n{0}", ex.Message);
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
                    dout("end message");
                }

                // "S,CURRENT PROTOCOL,5.2\r\n"
                int i1 = sData.IndexOf("S,CURRENT PROTOCOL,");
                if (i1 >= 0)
                {
                    int i2 = sData.IndexOf("\r\n", i1 + 1);
                    string protocol = sData.Substring(i1, i2 - i1 + 2);
                    m_protocolVersion = protocol.Split(',')[2].Trim();
                    sData = sData.Remove(i1, i2 - i1 + 2);
                }

                // display the data to the user
                //UpdateListview(sData);
                //output(sData);
                if (!string.IsNullOrEmpty(sData))
                    UpdateHistory?.Invoke(sData);

                // clear the m_strData to verify it is empty for the next read off the socket
                sData = "";

                // call wait for data to notify the socket that we are ready to receive another callback
                WaitForData("History");
            }
        }

        #region Public Data Request Methods --------------------------------------------------------------------------------------------------------------------
        public void GetTickDatapoints(string symbol, string datapoints, string direction, string requestID, string datapointsPerSend)
        {
            // request in the format:
            // HTX,SYMBOL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
            string sRequest = String.Format("HTX,{0},{1},{2},{3},{4}\r\n", symbol, datapoints, direction, requestID, datapointsPerSend);
            SendRequestAndWaitForData(sRequest);
        }

        public void GetTickDays(string symbol, string days, string datapoints, string beginFilterTime, string endFilterTime, string direction, string requestID, string datapointsPerSend)
        {
            // request in the format:
            // HTD,SYMBOL,NUMDAYS,MAXDATAPOINTS,BEGINFILTERTIME,ENDFILTERTIME,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
            string sRequest = String.Format("HTD,{0},{1},{2},{3},{4},{5},{6},{7}\r\n", symbol, days, datapoints, beginFilterTime, endFilterTime, direction, requestID, datapointsPerSend);
            SendRequestAndWaitForData(sRequest);
        }

        public void GetTickTimeframe(string symbol, string beginDateTime, string endDateTime, string datapoints, string beginFilterTime, string endFilterTime, string direction, string requestID, string datapointsPerSend)
        {
            // request in the format:
            // HTT,SYMBOL,BEGINDATE BEGINTIME,ENDDATE ENDTIME,MAXDATAPOINTS,BEGINFILTERTIME,ENDFILTERTIME,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
            string sRequest = String.Format("HTT,{0},{1},{2},{3},{4},{5},{6},{7},{8}\r\n", symbol, beginDateTime, endDateTime, datapoints, beginFilterTime, endFilterTime, direction, requestID, datapointsPerSend);
            SendRequestAndWaitForData(sRequest);
        }

        public void GetIntervalDatapoints(string symbol, string interval, string datapoints, string direction, string requestID, string datapointsPerSend)
        {
            // validate interval type
            string sIntervalType = "s";     // time (seconds)
            sIntervalType = "v";            // volume
            sIntervalType = "t";            // tick
            
            // request in the format:
            // HIX,SYMBOL,INTERVAL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND,INTERVALTYPE<CR><LF>
            string sRequest = String.Format("HIX,{0},{1},{2},{3},{4},{5},{6}\r\n", symbol, interval, datapoints, direction, requestID, datapointsPerSend, sIntervalType);
            SendRequestAndWaitForData(sRequest);
        }

        public void GetIntervalDays(string symbol, string interval, string days, string datapoints, string beginFilterTime, string endFilterTime, string direction, string requestID, string datapointsPerSend)
        {
            // validate interval type
            string sIntervalType = "s";     // time (seconds)
            sIntervalType = "v";            // volume
            sIntervalType = "t";            // tick

            // request in the format:
            // HID,SYMBOL,INTERVAL,NUMDAYS,MAXDATAPOINTS,BEGINFILTERTIME,ENDFILTERTIME,DIRECTION,REQUESTID,DATAPOINTSPERSEND,INTERVALTYPE<CR><LF>
            string sRequest = String.Format("HID,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\r\n", symbol, interval, days, datapoints, beginFilterTime, endFilterTime, direction, requestID, datapointsPerSend, sIntervalType);
            SendRequestAndWaitForData(sRequest);
        }

        public void GetIntervalTimeframe(string symbol, string interval, string beginDateTime, string endDateTime, string datapoints, string beginFilterTime, string endFilterTime, string direction, string requestID, string datapointsPerSend)
        {
            // validate interval type
            string sIntervalType = "s";     // time (seconds)
            sIntervalType = "v";            // volume
            sIntervalType = "t";            // tick

            // request in the format:
            // HIT,SYMBOL,INTERVAL,BEGINDATE BEGINTIME,ENDDATE ENDTIME,MAXDATAPOINTS,BEGINFILTERTIME,ENDFILTERTIME,DIRECTION,REQUESTID,DATAPOINTSPERSEND,INTERVALTYPE<CR><LF>
            string sRequest = String.Format("HIT,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\r\n", symbol, interval, beginDateTime, endDateTime, datapoints, beginFilterTime, endFilterTime, direction, requestID, datapointsPerSend, sIntervalType);
            SendRequestAndWaitForData(sRequest);
        }

        public void GetDailyDatapoints(string symbol, string datapoints, string direction, string requestID, string datapointsPerSend)
        {
            // request in the format:
            // HDX,SYMBOL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
            string sRequest = String.Format("HDX,{0},{1},{2},{3},{4}\r\n", symbol, datapoints, direction, requestID, datapointsPerSend);
            SendRequestAndWaitForData(sRequest);
        }

        public void GetDailyTimeframe(string symbol, string beginDateTime, string endDateTime, string datapoints, string direction, string requestID, string datapointsPerSend)
        {
            // request in the format:
            // HDT,SYMBOL,BEGINDATE,ENDDATE,MAXDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
            string sRequest = String.Format("HDT,{0},{1},{2},{3},{4},{5},{6}\r\n", symbol, beginDateTime, endDateTime, datapoints, direction, requestID, datapointsPerSend);
            SendRequestAndWaitForData(sRequest);
        }

        public void GetWeeklyDatapoints(string symbol, string datapoints, string direction, string requestID, string datapointsPerSend)
        {
            // request in the format:
            // HWX,SYMBOL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
            string sRequest = String.Format("HWX,{0},{1},{2},{3},{4}\r\n", symbol, datapoints, direction, requestID, datapointsPerSend);
            SendRequestAndWaitForData(sRequest);
        }

        public void GetMonthlyDatapoints(string symbol, string datapoints, string direction, string requestID, string datapointsPerSend)
        {
            // request in the format:
            // HMX,SYMBOL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
            string sRequest = String.Format("HMX,{0},{1},{2},{3},{4}\r\n", symbol, datapoints, direction, requestID, datapointsPerSend);
            SendRequestAndWaitForData(sRequest);
        }

        public void RequestListedMarkets()
        {
            string sRequest = String.Format("SLM,LISTS\r\n");
            SendRequestAndWaitForData(sRequest);
        }

        public void RequestSecurityTypes()
        {
            string sRequest = String.Format("SST,LISTS\r\n");
            SendRequestAndWaitForData(sRequest);
        }

        /*private List<string> RequestList(string request, string requestId = "CSLISTS")
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
        }*/


        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------


        public void SendRequestAndWaitForData(string sRequest)
        {
            SendRequestToIQFeed(sRequest);
            WaitForData("History");                 // tell the socket we are ready to receive data
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
                    ErrorMessage("Error Sending Request:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray()));
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

        /// <summary>
        /// Gets local IQFeed socket ports from the registry
        /// </summary>
        /// <param name="sPort"></param>
        /// <returns></returns>
        private int GetIQFeedPort(string sPort)
        {
            int iReturn = 0;
            MyRegistryKey key = MyRegistry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup");
            //RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup");
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

        // local output method to replace output to UI List
        private void output(string text)
        {
            dout(text);
        }

    } // end of class
} // end of namespace