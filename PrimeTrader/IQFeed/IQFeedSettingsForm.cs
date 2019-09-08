//-----------------------------------------------------------
//-----------------------------------------------------------
//
//             System: IQFeed
//       Program Name: LaunchingTheFeed_VC#.exe
//        Module Name: LaunchingTheFeedForm.cs
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
// Module Description: Implementation of Launching IQFeed
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
using System.Diagnostics;
using Tools;
using static Tools.G;

namespace IQFeed
{
    public partial class IQFeedSettingsForm : Form
    {
        // global variables for socket communications
        AsyncCallback m_pfnAdminCallback;
        Socket m_sockAdmin;
        byte[] m_szAdminSocketBuffer = new byte[8096];
        string m_sAdminIncompleteRecord = "";
        bool m_bAdminNeedBeginReceive = true;

        // boolean to track if we have registered our app with the feed yet.
        bool m_bRegistered = false;

        /// <summary>
        /// Constructor for the form
        /// </summary>
        public IQFeedSettingsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LaunchingTheFeedForm_Load(object sender, EventArgs e)
        {
            IQ_Config config = new IQ_Config();
            txtProductID.Text = config.getProductID();
            /*
             * Start code to check if IQFeed is installed
             */

            // IQFeed Installation directory is stored in the registry key:
            // HKLM\Software\DTN\IQFeed\EXEDIR
            MyRegistryKey key = MyRegistry.LocalMachine.OpenSubKey("SOFTWARE\\DTN\\IQFeed", true);
            if (key == null)
            {
                // if it isn't in that location, it is possible the user is running and x64 OS.  Check the windows virtualized registry location
                key = MyRegistry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\DTN\\IQFeed", true);
            }
            if (key != null)
            {
                string sLocation = key.GetValue("EXEDIR", "").ToString();
                // close the key since we don't need it anymore
                key.Close();
                // verify there is a \ on the end before we append the exe name
                if (!(sLocation.EndsWith("\\") || sLocation.EndsWith("/")))
                {
                    sLocation += "\\";
                }
                sLocation += "IQConnect.exe";
                // update the location in the text box
                txtIQConnectLocation.Text = sLocation;
            }
            else
            {
                MessageBox.Show(String.Format("Unable to find IQFeed Installation.\nDid you forget Install IQFeed?\nMake sure you installed more than just the developer package."), "Could not find IQFeed");
                txtIQConnectLocation.Text = "IQFeed Not Installed";
            }
            /*
             * end code to check if IQFeed is installed
             */

            /*
             * Start code to grab IQFeed settings from the registry
             */

            // pull the login and password, save login info, and autoconnect settings out of the 
            // registry (if they are already stored)
            key = null;
            key = MyRegistry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup", true);

            // NOTE: we don't need to check for the virtualized registry key on x64 here since these values are in the HKEY_CURRENT_USER hive.

            if (key != null)
            {
                string sData = key.GetValue("Login", "").ToString();
                txtLoginID.Text = sData;
                sData = key.GetValue("Password", "").ToString();
                txtPassword.Text = sData;
                sData = key.GetValue("AutoConnect", "0").ToString();
                if (sData.Equals("1"))
                {
                    ckbxAutoconnect.Checked = true;
                }
                sData = key.GetValue("SaveLoginPassword", "0").ToString();
                if (sData.Equals("1"))
                {
                    ckbxSaveLoginInfo.Checked = true;
                }
            }

            /*
             * End code to check if IQFeed is installed
             */

            // populate the productversion from the assembly from this app
            txtProductVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// launches the feed based upon user input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLaunchFeed_Click(object sender, EventArgs e)
        {
            // IQFeed needs the following information from apps that connect to it:
            // Registered ProductID
            // Version number of the application connecting to the feed
            // LoginID for IQFeed
            // Password for IQFeed
            // If the user want's thier loginID and Password saved
            // If the user wants the feed to automatically connect when launched.
            
            // IQFeed provides 2 methods for providing this information.  You can send it via the command line
            // when you launch the feed.  
            // Or you can connect to the Admin port of IQFeed and send the information.

            // This app demonstrates both methods

            // Either way, we need to launch the feed.  The below code builds the command line parameters you chose.
            string sArguments = "";
            if (ckbxProductIDCmdLine.Checked && (txtProductID.Text.Length > 0))
            {
                sArguments += "-product " + txtProductID.Text + " ";
            }
            if (ckbxProductVersionCmdLine.Checked && (txtProductVersion.Text.Length > 0))
            {
                sArguments += "-version " + txtProductVersion.Text + " ";
            }
            if (ckbxLoginIDCmdLine.Checked && (txtLoginID.Text.Length > 0))
            {
                sArguments += "-login " + txtLoginID.Text + " ";
            }
            if (ckbxPasswordCmdLine.Checked && (txtPassword.Text.Length > 0))
            {
                sArguments += "-password " + txtPassword.Text + " ";
            }
            if (ckbxSaveLoginInfoCmdLine.Checked && ckbxSaveLoginInfo.Checked)
            {
                sArguments += "-savelogininfo ";
            }
            if (ckbxAutoconnectCmdLine.Checked && ckbxAutoconnect.Checked)
            {
                sArguments += "-autoconnect";
            }
            sArguments.TrimEnd(' ');

            // now that we have built our arguments string based on the user input, we will launch IQConnect.exe
            // NOTE:  we don't need to fully path the exe here because IQFeed adds its installtion directory to the PATH environment variable during installation
            System.Diagnostics.Process.Start("IQConnect.exe", sArguments);
            
            // At this point, the feed is launched, we want open up a connection to the Admin port for monitoring the status of the feed
            // We will also send any of the fields that weren't sent via command line as admin port commands once the feed is connected and recieveing data.

            // if we supplied the ProductID and ProductVersion via command line, set our registered flag for later validation
            if (ckbxProductIDCmdLine.Checked && ckbxProductVersionCmdLine.Checked)
                m_bRegistered = true;

            // at this point, IQConnect should be running but we don't know if it is acutally 
            // connected to the servers or not.  To find out this information we need to connect 
            // to the Admin port in IQFeed and monitor the stats messages.

            // create the socket
            m_sockAdmin = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipLocalhost = IPAddress.Parse("127.0.0.1");

            // the default port for admin connections is 9300 but any software could alter that so
            // we need to check the IQFeed Registry settings.
            int iPort = GetIQFeedPort("Admin");
            IPEndPoint ipendLocalhost = new IPEndPoint(ipLocalhost, iPort);

            try
            {
                // connect
                m_sockAdmin.Connect(ipendLocalhost);

                // this example is using asynchronous sockets to communicate with the feed.  As a result, we are using .NET's BeginReceive and EndReceive calls with a callback.
                // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
                WaitForData("Admin");
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Oops.  Did you forget to Login first?\nTake a Look at the LaunchingTheFeed example app\n{0}", ex.Message), "Error Connecting to IQFeed");
                DisableForm();
            }
        }

        /// <summary>
        /// we call this to notify the .NET Async socket to start listening for data to come in.  It must be called each time after we receive data
        /// </summary>
        private void WaitForData(string sSocketName)
        {
            if (sSocketName.Equals("Admin"))
            {
                // make sure we have a callback created
                if (m_pfnAdminCallback == null)
                {
                    m_pfnAdminCallback = new AsyncCallback(OnReceive);
                }
                // send the notification to the socket.  It is very important that we don't call Begin Reveive more than once per call
                // to EndReceive.  As a result, we set a flag to ignore multiple calls.
                if (m_bAdminNeedBeginReceive)
                {
                    m_bAdminNeedBeginReceive = false;
                    m_sockAdmin.BeginReceive(m_szAdminSocketBuffer, 0, m_szAdminSocketBuffer.Length, SocketFlags.None, m_pfnAdminCallback, sSocketName);
                }
            }
        }

        /// <summary>
        /// This is our callback that gets called by the .NET socket class when new data arrives on the socket
        /// </summary>
        /// <param name="asyn"></param>
        private void OnReceive(IAsyncResult asyn)
        {
            // first verify we received data from the correct socket.
            if (asyn.AsyncState.ToString().Equals("Admin"))
            {
                // read data from the socket
                int iReceivedBytes = 0;
                iReceivedBytes = m_sockAdmin.EndReceive(asyn);
                // set our flag back to true so we can call begin receive again
                m_bAdminNeedBeginReceive = true;
                // in this example, we will convert to a string for ease of use.
                string sData = Encoding.ASCII.GetString(m_szAdminSocketBuffer, 0, iReceivedBytes);

                // When data is read from the socket, you can get multiple messages at a time and there is no guarantee
                // that the last message you receive will be complete.  It is possible that only half a message will be read
                // this time and you will receive the 2nd half of the message at the next call to OnReceive.
                // As a result, we need to save off any incomplete messages while processing the data and add them to the beginning
                // of the data next time.
                sData = m_sAdminIncompleteRecord + sData;
                // clear our incomplete record string so it doesn't get processed next time too.
                m_sAdminIncompleteRecord = "";

                while (sData.Length > 0)
                {
                    int iNewLinePos = -1;
                    iNewLinePos = sData.IndexOf("\n");
                    string sLine;
                    if (iNewLinePos == -1)
                    {
                        // we have an incomplete record.  Save it off for the next call of OnRecieve.
                        m_sAdminIncompleteRecord = sData;
                        sData = "";
                    }
                    else
                    {
                        // we have a complate record.  Process it.
                        sLine = sData.Substring(0, iNewLinePos);
                        if (sLine.StartsWith("S,STATS,"))
                        {
                            // check if we registered using the command line parameters
                            if (!m_bRegistered)
                            {
                                // we need to register the feed, send off the S,REGISTER CLIENT APP command
                                string sCommand = "S,REGISTER CLIENT APP,";
                                sCommand += txtProductID.Text;
                                sCommand += ",";
                                sCommand += txtProductVersion.Text;
                                sCommand += "\r\n";

                                // and we send it to the feed via the Admin socket
                                byte[] szCommand = new byte[sCommand.Length];
                                szCommand = Encoding.ASCII.GetBytes(sCommand);
                                int iBytesToSend = szCommand.Length;
                                m_sockAdmin.Send(szCommand, iBytesToSend, SocketFlags.None);
                                // set our flag so that we don't send the command again with the next stats message.
                                m_bRegistered = true;
                            }
                        }
                        else if (sLine.StartsWith("S,REGISTER CLIENT APP COMPLETED"))
                        {
                            // our S,REGISTER CLIENT APP command completed. 

                            // Send our loginID
                            string sCommand = "";
                            if (!ckbxLoginIDCmdLine.Checked)
                            {
                                sCommand += "S,SET LOGINID,";
                                sCommand += txtLoginID.Text;
                                sCommand += "\r\n";
                            }
                            // Password
                            if (!ckbxPasswordCmdLine.Checked)
                            {
                                sCommand += "S,SET PASSWORD,";
                                sCommand += txtPassword.Text;
                                sCommand += "\r\n";
                            }
                            // save login info (based on user input or previous settings)
                            if (!ckbxSaveLoginInfoCmdLine.Checked)
                            {
                                sCommand += "S,SET SAVE LOGIN INFO,";
                                if (ckbxSaveLoginInfo.Checked)
                                    sCommand += "On\r\n";
                                else
                                    sCommand += "Off\r\n";
                            }
                            // autoconnect (based on user input or previous settings)
                            if (!ckbxAutoconnectCmdLine.Checked)
                            {
                                sCommand += "S,SET AUTOCONNECT,";
                                if (ckbxAutoconnect.Checked)
                                    sCommand += "On\r\n";
                                else
                                    sCommand += "Off\r\n";
                            }
                            // and issue a connect command.  If Autoconnect is NOT set, 
                            // this will prompt the user to click connect.
                            sCommand += "S,CONNECT\r\n";

                            // we send it to the feed via the Admin socket
                            byte[] szCommand = new byte[sCommand.Length];
                            szCommand = Encoding.ASCII.GetBytes(sCommand);
                            int iBytesToSend = szCommand.Length;
                            m_sockAdmin.Send(szCommand, iBytesToSend, SocketFlags.None);
                            m_bRegistered = true;
                        }
                        sData = sData.Substring(sLine.Length + 1);
                    }
                }

                // call wait for data to notify the socket that we are ready to receive another callback
                WaitForData("Admin");
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

        private void LaunchingTheFeedForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void btnConsoleBenchmark_Click(object sender, EventArgs e)
        {
            //status("IQFeed: Console Benchmark Tool");
            try
            {
                string exeFilename = ConsoleBenchmarkTool.Program.GetExecutableFilename();
                Process firstProc = new Process();
                firstProc.StartInfo.FileName = exeFilename;
                //firstProc.StartInfo.FileName = "notepad.exe";
                firstProc.EnableRaisingEvents = true;

                firstProc.Start();

                firstProc.WaitForExit();

                //You may want to perform different actions depending on the exit code.
                dout("First process exited: {0}", firstProc.ExitCode);

                /*Process secondProc = new Process();
                secondProc.StartInfo.FileName = "mspaint.exe";
                secondProc.Start();*/
            }
            catch (Exception ex)
            {
                ErrorMessage("An error occurred!!!: {0}", ex.Message);
                return;
            }

        }

    } // end of class
} // end of namespace