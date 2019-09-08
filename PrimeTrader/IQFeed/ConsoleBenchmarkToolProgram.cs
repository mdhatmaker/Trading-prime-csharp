//-----------------------------------------------------------
//-----------------------------------------------------------
//
//             System: IQFeed
//       Program Name: ConsoleBenchmarkTool_VC#.exe
//        Module Name: Program.cs
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
using System.Text;
// added for access to RegistryKey
//using Microsoft.Win32;
// added for access to socket classes
using System.Net;
using System.Net.Sockets;
using IQ_Config_Namespace;
using Tools;

namespace ConsoleBenchmarkTool
{
    public class Program
    {
        static public string GetExecutableFilename()
        {
            //return System.Reflection.Assembly.GetEntryAssembly().Location;
            return System.Reflection.Assembly.GetExecutingAssembly().Location;
        }

        static void Main(string[] args)
        {

            IQ_Config config = new IQ_Config();
            Console.Title = "C# Console Benchmark Tool - Press esc to exit";
            Console.WindowWidth = 120;

            // Launch IQConnect.  
            Console.WriteLine("Launching IQConnect...");
            // This is a simple way to launch the feed letting the user specify thier own login/password.
            // If you need to see all the options for launching the feed, take a look at the "LaunchingTheFeed" app.
            System.Diagnostics.Process.Start("IQConnect.exe", String.Format("-product {0} -version {1}", config.getProductID(), System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            
            // now that the feed is launched, connect to the admin port of IQFeed to get the status of the feed.  This allows us to wait until the user clicks connect before trying to process data.
            // create the socket and tell it to connect
            Socket sockIQFeed = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Admin socket created...");

            IPAddress ipLocalhost = IPAddress.Parse("127.0.0.1");

            // pull the admin port out of the registry.  we use the Level 1 port because we want streaming updates
            int iPort = GetIQFeedPort("Admin");
            IPEndPoint ipendLocalhost = new IPEndPoint(ipLocalhost, iPort);
            Console.WriteLine("IPEndPoint created...");

            try
            {
                // tell the socket to connect to IQFeed
                sockIQFeed.Connect(ipendLocalhost);
                Console.WriteLine("Admin socket connected...");
                // Since we are in a console app and don't have the message pump provided by the Form
                // we are going to use blocking socket calls instead of creating the complexity of our own timer to use async socket calls.
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Connecting to IQFeed's Admin Port\n{0}", ex.Message);
            }

            // now that we are connected to the admin port, we need to read off that socket until we get a message indicating that IQFeed is connected to the server
            byte[] szSocketBuffer = new byte[Int16.MaxValue];
            // since we are using a blocking socket, a Receive value of zero indicates that the socket has been closed.
            int iBytesReceived = 0;
            bool bShutDown = false;
            while (!bShutDown && (iBytesReceived = sockIQFeed.Receive(szSocketBuffer)) > 0)
            {
                // with this connection, we aren't worried about efficiency of data processing
                // since there isn't going to be a lot of data delivered to the socket.  As a result
                // we just read the data off the socket and display it to the console and then process it.
                string sData = Encoding.ASCII.GetString(szSocketBuffer, 0, iBytesReceived);
                Console.WriteLine(sData);

                // there is a lot of useful data in the admin socket messages, but for this app, we only want to make sure the feed is connected
                // before closing the socket and moving on to the Level 1 data
                if (sData.Contains(",Connected,"))
                {
                    Console.WriteLine("IQFeed is connected to the server.");
                    Console.WriteLine("Closing Admin Socket...");
                    sockIQFeed.Shutdown(SocketShutdown.Both);
                    sockIQFeed.Close();
                    bShutDown = true;
                }
            }
            // IQFeed is connected.  Lets read in our commands file that was supplied in the command line arguments.
            string[] saCommands;
            string sAllFileData = String.Format("S,SET PROTOCOL,{0}",config.getProtocol());
            try
            {
                string sFile = "";
                if (args.GetLength(0) > 0)
                {
                    sFile = args[0];
                }
                Console.WriteLine("Start reading commands from file {0}", sFile);
                
                if (sFile.Length > 0)
                {
                    // read commands in from the file
                    int iCounter = 0;
                    string sLine;

                    // Read the file and load it into the array.
                    System.IO.StreamReader srFile = new System.IO.StreamReader(sFile);
                    while ((sLine = srFile.ReadLine()) != null)
                    {
                        sAllFileData += sLine;
                        sAllFileData += "\n";
                        iCounter++;
                    }
                    srFile.Close();
                }

                Console.WriteLine("Done reading commands from file {0}", sFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Reading commands file\n{0}", ex.Message);
            }
            saCommands = sAllFileData.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            // we have our list of commands, we know IQFeed is running, lets start getting some data
            Console.WriteLine("Connecting to Level 1 Port...");
            sockIQFeed = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            iPort = GetIQFeedPort("Level1");
            ipendLocalhost = new IPEndPoint(ipLocalhost, iPort);
            Console.WriteLine("IPEndPoint updated...");

            try
            {
                // tell the socket to connect to IQFeed
                sockIQFeed.Connect(ipendLocalhost);
                Console.WriteLine("Level1 socket connected...");
                // Since we are in a console app and don't have the message pump provided by the Form
                // we are going to use blocking socket calls instead of creating the complexity of our own timer to use async socket calls.
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Connecting to IQFeed's Level1 Port\n{0}", ex.Message);
            }

            // send commands (from file) to IQConnect.
            foreach (string sCommand in saCommands)
            {
                byte[] szCommand = new byte[sCommand.Length + 2];
                szCommand = Encoding.ASCII.GetBytes(sCommand + "\r\n");
                int iBytesToSend = szCommand.Length;
                int iBytesSent = sockIQFeed.Send(szCommand, iBytesToSend, SocketFlags.None);
                if (iBytesToSend == iBytesSent)
                {
                    Console.WriteLine("Sent command: {0} to IQFeed...", sCommand);
                }
                else
                {
                    Console.WriteLine("Unable to send command: {0} to IQFeed...", sCommand);
                }
            }


            // start processing data received from IQConnect.
            // variables for data processing
            iBytesReceived = 0;
            bShutDown = false;
            byte byteLineFeed = 10;
            int iBytesLeftover = 0;
            int iBytesToParse = 0;

            // variables for stats tracking
			int iFMessages = 0;
            int iPMessages = 0;
            int iQMessages = 0;
            int iTMessages = 0;
            int iSMessages = 0;
            int iNMessages = 0;
            int iRMessages = 0;
            int iMsgsLastSecond = 0;
            int iTotalMsgs = 0;
            double dSeconds = 0.0;
            System.Diagnostics.Stopwatch watchTimer = new System.Diagnostics.Stopwatch();
            DateTime dtCurrent;
            watchTimer.Start();

            // loop while there is data to be read and the socket is open.
            while (!bShutDown && (iBytesReceived = sockIQFeed.Receive(szSocketBuffer, iBytesLeftover, szSocketBuffer.Length - iBytesLeftover, SocketFlags.None)) > 0)
            {
                iBytesToParse = iBytesReceived;
                iBytesToParse += iBytesLeftover;
                iBytesLeftover = 0;
                // unlike with the admin port connection and with the other example apps,
                // with this connection, we ARE worried about efficiency of data processing.
                // Additionally, want to make sure this app is using the least amount of CPU possible.
                // As a result, we are not converting data to strings for processing.
                // Since we only want to know what type of message was received so we only need to
                // look at the starting character of each message and search for the delimiting character.

                // each line of output from the level 1 port of IQFeed is delimited with a line feed character (10)
                int iNewLinePos = Array.IndexOf(szSocketBuffer, byteLineFeed);
                int iMessagePos = 0;

                // as long as we found one, we know there is still a complete message in the buffer.  process it
                while (iNewLinePos > -1)
                {
                    iTotalMsgs++;
                    iMsgsLastSecond++;
                    switch (szSocketBuffer[iMessagePos])
                    {
                        case 81: // Q
                            // Update Message
                            iQMessages++;
                            break;
                        case 84: // T
                            // Timestamp Message
                            iTMessages++;
                            // we trigger our output to the screen when we receive a timestamp message.

                            // skip the first writeout if a complete second hasn't elapsed
                            dSeconds = watchTimer.Elapsed.TotalSeconds;
                            dtCurrent = DateTime.Now;
                            if (dSeconds > 0.0)
                            {
                                Console.WriteLine(String.Format("F:{0}\tP:{1}\tQ:{2}\tT:{3}\tS:{4}\tN:{5}\tR:{6}\tLM:{7}\tTMS:{8:0.0}\tTIME:{9:.000000}", iFMessages, iPMessages, iQMessages, iTMessages, iSMessages, iNMessages, iRMessages, iMsgsLastSecond, iTotalMsgs / dSeconds, dtCurrent.ToOADate()));
                                // reset our "Messages in the last second" counter
                                iMsgsLastSecond = 0;
                            }
                            break;
                        case 70: // F
                            // Fundamental Message
                            iFMessages++;
                            break;
                        case 80: // P
                            // Summary Message
                            iPMessages++;
                            break;
                        case 82: // R
                            // Regional Message
                            iRMessages++;
                            break;
                        case 78: // N
                            // News Message
                            iNMessages++;
                            break;
                        case 83: // S
                            // System Message
                            iSMessages++;
                            break;
                    }
                    // find the beginning of our next message.  If we have an incomplete message in the buffer
                    // or if we reach the end of the buffer normally, iNewLinePos will be -1 and we well exit the loop
                    iMessagePos = iNewLinePos + 1;
                    iNewLinePos = Array.IndexOf(szSocketBuffer, byteLineFeed, iMessagePos);
                }
                // now we need to check for an incomplete message and copy it to the beginning of the buffer for the next read from the socket
                if (iMessagePos < iBytesToParse)
                {
                    // we have an incomplete message
                    iBytesLeftover = iBytesToParse;
                    iBytesLeftover -= iMessagePos;
                    // copy the left over bytes to the front of the buffer
                    Buffer.BlockCopy(szSocketBuffer, iMessagePos, szSocketBuffer, 0, iBytesLeftover);
                }

                // zero the rest of the buffer that was used to prevent parsing data multiple times in the next read
                for (int i = iBytesLeftover; i < iBytesToParse; i++)
                {
                    szSocketBuffer[i] = 0;
                }

                // check if the user hit the esc key to quit.
                if (CheckForUserBreak())
                {
                    Console.WriteLine("Interupted by user...");
                    Console.WriteLine("Closing Level1 Socket...");
                    sockIQFeed.Shutdown(SocketShutdown.Both);
                    sockIQFeed.Close();
                    bShutDown = true;
                }
            }
        }

        /// <summary>
        /// Gets local IQFeed socket ports from the registry
        /// </summary>
        /// <param name="sPort"></param>
        /// <returns></returns>
        private static int GetIQFeedPort(string sPort)
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
        /// check if the user hit the esc key to quit.
        /// </summary>
        /// <returns></returns>
        private static bool CheckForUserBreak()
        {
            bool bReturn = false;
            if (Console.KeyAvailable && (Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                bReturn = true;
            }
            return bReturn;
        }
    }
}
