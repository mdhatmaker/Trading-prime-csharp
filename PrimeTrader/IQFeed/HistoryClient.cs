using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using IQ_Config_Namespace;
using System.Threading;
using Tools;
using static Tools.GDate;

namespace IQFeed
{
    public enum HistoryType { TickDatapoints, TickDays, TickTimeframe, IntervalDatapoints, IntervalDays, IntervalTimeframe, DailyDatapoints, DailyTimeframe, WeeklyDatapoints, MonthlyDatapoints }
    public enum HistoryDataDirection { Backward = 0, Forward = 1 }
    public enum HistoryIntervalType { Time, Volume, Tick }
    public enum HistoryInterval { Time5s = 5, Time10s = 10, Time15s = 15, Time30s = 30, Time1m = 60, Time5m = 300, Time15m = 900, Time30m = 1800, Time1h = 3600, Time2h = 7200, Time3h = 10800, Time4h = 14400, Time6h = 21600, Time8h = 28800, Time12h = 43200, Time1d = 86400 }

    public class HistoryClient
    {
        const string REQUEST_ID = "hist_req";
        const string NO_DATA = "!NO_DATA!";
        const string END_MSG = "!ENDMSG!";
        const string EARLIEST_DATE = "20010101";

        // socket communication global variables
        AsyncCallback m_pfnLookupCallback;
        Socket m_sockLookup;
        
        byte[] m_szLookupSocketBuffer = new byte[262144];   // we create the socket buffer global for performance
        string m_sLookupIncompleteRecord = "";              // stores unprocessed data between reads from the socket
        bool m_bLookupNeedBeginReceive = true;              // flag for tracking when a call to BeginReceive needs called
        bool m_bEndOfMessageReceived = false;               // this will indicate we can exit our processing of the current history request

        string m_symbol;                                    // current output file symbol (like "@VXF18")

        string m_filename;                                  // holds filename for output file (like "@VX_futures.hour.DF.csv" or "QHG_continuous.minute.DF.csv")
        StreamWriter m_outfile = null;                      // output file (StreamWriter)
        
        // delegate for updating the data display.
        public delegate void UpdateDataHandler(string sMessage);


        // Default CTOR
        public HistoryClient()
        {
            InitializeHistorySocket();
        }

        // CTOR that specifies IP Address and Port of the IQFeed socket
        public HistoryClient(string ip, int port)
        {
            InitializeHistorySocket(ip, port);
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls and initializing the connection to IQFeed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitializeHistorySocket(string ip = "127.0.0.1", int port = -1)
        {
            IQ_Config config = new IQ_Config();

            // create the socket
            m_sockLookup = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipLocalhost = IPAddress.Parse(ip);

            // Historical data is received from IQFeed on the Lookup port.
            // pull the Lookup port out of the registry
            int iPort = (port > 0 ? port : Helper.GetIQFeedPort("Lookup"));
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
                DisplayError(String.Format("Oops.  Did you forget to Login first?\nTake a Look at the LaunchingTheFeed example app\n{0}", ex.Message));
                // exit - catastrophic error (can't connect to IQFeed socket)
            }

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

                // send the notification to the socket.  It is very important that we don't call Begin Receive more than once per call
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
                    DisplayMessage(sData);
                    m_bEndOfMessageReceived = true;
                    return;
                }

                // display the data to the user
                DisplayMessage(sData);

                // clear the m_strData to verify it is empty for the next read off the socket
                sData = "";

                // call wait for data to notify the socket that we are ready to receive another callback
                WaitForData("History");
            }
        }

        #region ---------- DISPLAY and APPEND DATA --------------------------------------------------------------------
        // Append new data that has been received.
        // If the data begins with REQUEST_ID (and does NOT contain "!ENDMSG!"), then rearrange it:
        // DateTime,Open,High,Low,Close,Volume,oi,Symbol
        private void AppendData(string sLine)
        {            
            if (!sLine.StartsWith(REQUEST_ID) || sLine.Contains(END_MSG) || sLine.Contains(NO_DATA))
                Console.WriteLine(sLine);
            else
            {
                var columns = sLine.Split(',');
                string sOutputLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", columns[1], columns[4], columns[2], columns[3], columns[5], columns[6], columns[7], m_symbol);
                //Console.WriteLine(sOutputLine);
                if (m_outfile == null)
                {
                    m_outfile = new StreamWriter(m_filename);
                    m_outfile.WriteLine("DateTime,Open,High,Low,Close,Volume,oi,Symbol");
                }
                m_outfile.WriteLine(sOutputLine);
            }
        }

        /// <summary>
        /// We want to be able to display output from within the AsyncSocket Callback so we
        /// need to resolve cross-threading issues (with a delegate for UI, for example).
        /// </summary>
        /// <param name="sData"></param>
        private void DisplayMessage(string sData)
        {
            // Display individual lines in the message
            List<String> lstMessages = new List<string>(sData.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            //Console.WriteLine(lstMessages.Count);
            lstMessages.ForEach(sLine => AppendData(sLine));
        }

        // Display an error message to the user
        private void DisplayError(string msg)
        {
            m_bEndOfMessageReceived = true;     // because of error, force indication that we have received "!ENDMSG!" marker

            Console.WriteLine(string.Format("ERROR:: {0}", msg));
        }
        #endregion ----------------------------------------------------------------------------------------------------

        public void HistoryContractIntervalTimeframe(string symbol, HistoryIntervalType itype, HistoryInterval interval, string beginDateTime = null, string endDateTime = null, HistoryDataDirection direction = HistoryDataDirection.Forward, string datapointsPerSend = "", string maxDatapoints = "", string beginFilterTime = "", string endFilterTime = "")
        {
            m_filename = Folders.df_path(GFile.GetDfContractFilename(symbol, (int)interval));
            m_outfile = null;

            beginDateTime = beginDateTime ?? EARLIEST_DATE;
            endDateTime = endDateTime ?? DateTime.Now.AddMonths(1).ToString("yyyyMMdd");

            string sInterval = ((int)interval).ToString();
            m_symbol = symbol;
            HistoryIntervalTimeframe(m_symbol, itype, sInterval, beginDateTime, endDateTime, direction, datapointsPerSend, maxDatapoints, beginFilterTime, endFilterTime);
            
            if (m_outfile != null)
            {
                m_outfile.Close();
                Console.WriteLine("\nOutput to file: {0}\n", m_filename);
            }
        }

        public void HistoryFuturesIntervalTimeframe(string rootSymbol, HistoryIntervalType itype, HistoryInterval interval, string beginDateTime = null, string endDateTime = null, HistoryDataDirection direction = HistoryDataDirection.Forward, string datapointsPerSend = "", string maxDatapoints = "", string beginFilterTime = "", string endFilterTime = "")
        {
            m_filename = Folders.df_path(GFile.GetDfFuturesFilename(rootSymbol, (int)interval));
            m_outfile = null;

            int addMonthsToEnd = 6;
            beginDateTime = beginDateTime ?? EARLIEST_DATE;
            endDateTime = endDateTime ?? DateTime.Now.AddMonths(addMonthsToEnd).ToString("yyyyMMdd");

            var mcList = GetMonthCodeList(beginDateTime, endDateTime);

            string sInterval = ((int)interval).ToString();
            foreach (var mYY in mcList)
            {
                m_symbol = rootSymbol + mYY;
                // use mYY and the mYY from 6 months prior to calculate a begin and end date for contract historical data
                string mYYplus1 = AddMonths(mYY, 1);
                string mYYminus6 = AddMonths(mYY, -6);
                string dt1 = GetDateTimeMYY(mYYminus6).ToYYYYMMDD();
                string dt2 = GetDateTimeMYY(mYYplus1).ToYYYYMMDD();

                HistoryIntervalTimeframe(m_symbol, itype, sInterval, dt1, dt2, direction, datapointsPerSend, maxDatapoints, beginFilterTime, endFilterTime);
            }

            if (m_outfile != null)
            {
                m_outfile.Close();
                Console.WriteLine("\nOutput to file: {0}\n", m_filename);
            }
        }

        #region ---------- RETRIEVE HISTORICAL DATA -------------------------------------------------------------------
        public void HistoryTickDatapoints(string symbol, HistoryDataDirection direction = HistoryDataDirection.Forward, string datapointsPerSend = "", string maxDatapoints = "")
        {
            string sRequest = GetRequestTickDatapoints(symbol, direction, datapointsPerSend, maxDatapoints);
            GetData(sRequest);
        }

        public void HistoryIntervalTimeframe(string symbol, HistoryIntervalType itype, HistoryInterval interval, string beginDateTime = null, string endDateTime = null, HistoryDataDirection direction = HistoryDataDirection.Forward, string datapointsPerSend = "", string maxDatapoints = "", string beginFilterTime = "", string endFilterTime = "")
        {
            string sInterval = ((int)interval).ToString();
            HistoryIntervalTimeframe(symbol, itype, sInterval, beginDateTime, endDateTime, direction, datapointsPerSend, maxDatapoints, beginFilterTime, endFilterTime);
        }

        public void HistoryIntervalTimeframe(string symbol, HistoryIntervalType itype, string interval, string beginDateTime = null, string endDateTime = null, HistoryDataDirection direction = HistoryDataDirection.Forward, string datapointsPerSend = "", string maxDatapoints = "", string beginFilterTime = "", string endFilterTime = "")
        {
            beginDateTime = beginDateTime ?? EARLIEST_DATE;
            endDateTime = endDateTime ?? DateTime.Now.ToString("yyyyMMdd");
            string sRequest = GetRequestIntervalTimeframe(symbol, itype, interval, beginDateTime, endDateTime, direction, datapointsPerSend, maxDatapoints, beginFilterTime, endFilterTime);
            GetData(sRequest);
        }

        // Request string for HistoryType.TickDatapoints
        public string GetRequestTickDatapoints(string symbol, HistoryDataDirection direction = HistoryDataDirection.Forward, string datapointsPerSend = "", string maxDatapoints = "")
        {
            string sDirection = string.Format("{0}", (int)direction);

            // request in the format:
            // HTX,SYMBOL,NUMDATAPOINTS,DIRECTION,REQUESTID,DATAPOINTSPERSEND<CR><LF>
            return String.Format("HTX,{0},{1},{2},{3},{4}\r\n", symbol, maxDatapoints, sDirection, REQUEST_ID, datapointsPerSend);
        }

        // Request string for HistoryType.IntervalTimeframe
        public string GetRequestIntervalTimeframe(string symbol, HistoryIntervalType itype, HistoryInterval interval, string beginDateTime, string endDateTime, HistoryDataDirection direction = HistoryDataDirection.Forward, string datapointsPerSend = "", string maxDatapoints = "", string beginFilterTime = "", string endFilterTime = "")
        {
            return GetRequestIntervalTimeframe(symbol, itype, ((int)interval).ToString(), beginDateTime, endDateTime, direction, datapointsPerSend, maxDatapoints, beginFilterTime, endFilterTime);
        }

        // Request string for HistoryType.IntervalTimeframe
        public string GetRequestIntervalTimeframe(string symbol, HistoryIntervalType itype, string interval, string beginDateTime, string endDateTime, HistoryDataDirection direction = HistoryDataDirection.Forward, string datapointsPerSend = "", string maxDatapoints = "", string beginFilterTime = "", string endFilterTime = "")
        {
            string sIntervalType = itype.ToIntervalTypeString();        // interval type: "s" for Time, "v" for Volume, "t" for Tick
            string sDirection = string.Format("{0}", (int)direction);

            // request in the format:
            // HIT,SYMBOL,INTERVAL,BEGINDATE BEGINTIME,ENDDATE ENDTIME,MAXDATAPOINTS,BEGINFILTERTIME,ENDFILTERTIME,DIRECTION,REQUESTID,DATAPOINTSPERSEND,INTERVALTYPE<CR><LF>
            return String.Format("HIT,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\r\n", symbol, interval, beginDateTime, endDateTime, maxDatapoints, beginFilterTime, endFilterTime, sDirection, REQUEST_ID, datapointsPerSend, sIntervalType);
        }
        #endregion ----------------------------------------------------------------------------------------------------

        /// <summary>
        /// Take a pre-formed request (using one of the GetRequestXXX methods) and send it to IQFeed.
        /// Then wait until the History request completes.
        /// </summary>
        /// <param name="sRequest"></param>
        private void GetData(string sRequest)
        {
            /*// format request string based upon user input
            if (cboHistoryType.Text.Equals("Tick Days"))
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
            }*/

            m_bEndOfMessageReceived = false;

            // verify we have formed a request string
            if (sRequest.StartsWith("H"))
            {
                // send it to the feed via the socket
                SendRequestToIQFeed(sRequest);
            }
            else
            {
                DisplayError(String.Format("Error with selected request: {0}", sRequest));
            }

            // tell the socket we are ready to receive data
            WaitForData("History");

            // Wait for the "!ENDMSG!" marker to be received
            while (m_bEndOfMessageReceived == false)
            {
                Thread.Sleep(100);
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
                    DisplayError(String.Format("Error Sending Request:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
                else
                {
                    DisplayMessage(String.Format("Request Sent Successfully:\r\n{0}", sCommand.TrimEnd("\r\n".ToCharArray())));
                }
            }
            catch (SocketException ex)
            {
                // handle socket errors
                DisplayError(String.Format("Socket Error Sending Request:\r\n{0}\r\n{1}", sCommand.TrimEnd("\r\n".ToCharArray()), ex.Message));
            }
        }


    } // end of class HistoryClient
} // end of namespace