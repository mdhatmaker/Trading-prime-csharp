//-----------------------------------------------------------
//-----------------------------------------------------------
//
//             System: IQFeed
//       Program Name: Level1ActiveX_VC#.exe
//        Module Name: Level1ActiveXForm.cs
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
using IQ_Config_Namespace;

namespace Level1ActiveX
{
    public partial class Level1ActiveXForm : Form
    {
        /// <summary>
        /// Constructor for the form
        /// </summary>
        public Level1ActiveXForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls and initializing launching the feed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Level1ActiveXForm_Load(object sender, EventArgs e)
        {
            IQ_Config config = new IQ_Config();
            // launch the feed.
            string sProductID = config.getProductID();
            string sProductVersion = "1.0";
            string sDeprecated = "0.11111111";
            axIQFeedY1.RegisterClientApp(ref sProductID, ref sDeprecated, ref sProductVersion);
        }

        /// <summary>
        /// Event that fires when the Watch Button is pressed.  Sends the watch command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWatch_Click(object sender, EventArgs e)
        {
            // When you watch a symbol, you will get a snapshot of data that is currently available in the servers
            // for that symbol.  The snapshot will include a Fundamental message followed by a Summary message and then
            // you will continue to get Update messages anytime a field is updated until you issue an unwatch for the symbol.
            string sSymbol = txtRequest.Text;
            axIQFeedY1.WatchSymbol(ref sSymbol);
        }

        private void btnTradesOnly_Click(object sender, EventArgs e)
        {
            // When you issue a trades only watch, you will get a snapshot of data that is currently available in the servers
            // for that symbol.  The snapshot will include a Fundamental message followed by a Summary message and then
            // you will continue to get Update messages anytime a trade occurs until you issue an unwatch for the symbol.
            string sSymbol = txtRequest.Text;
            axIQFeedY1.WatchTradesOnly(ref sSymbol);
        }

        /// <summary>
        /// Event that fires when the Remove Button is pressed.  Sends the Remove command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            // when you remove a symbol, you simply tell the server that you no longer want to receive data for that symbol.

            string sSymbol = txtRequest.Text;
            axIQFeedY1.RemoveSymbol(ref sSymbol);
        }

        /// <summary>
        /// Event that fires when the Watch Regionals Button is pressed.  Sends the Regional Watch command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWatchRegionals_Click(object sender, EventArgs e)
        {
            // Issuing a regional watch for a symbol will also automatically issue a regular watch 
            // In addition to the messages you would expect to receive with a regular watch request, you will also
            // receive all of the current Best Bid/Offer for each regional exchange.

            string sSymbol = txtRequest.Text;
            axIQFeedY1.WatchRegionals(ref sSymbol);
        }

        /// <summary>
        /// Event that fires when the Remove Regional Button is pressed.  Sends the Remove Regional Watch command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveRegionals_Click(object sender, EventArgs e)
        {
            // Send a regional unwatch command if you no longer want to receive regional messages for a symbol but you 
            // still want to receive regular watch messages.  If you want to completely unwatch the symbol, you need to
            // issue a regular unwatch request.

            string sSymbol = txtRequest.Text;
            axIQFeedY1.RemoveRegionals(ref sSymbol);
        }

        /// <summary>
        /// Event that fires when the News On Button is pressed.  Sends the Start Streaming News command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewsOn_Click(object sender, EventArgs e)
        {
            // You can receive news headlines inline with your streaming quotes for any news types your account is authorized.  To start
            // receiving news headlines, send a News On request to the feed.

            axIQFeedY1.TurnOnNews();
        }

        /// <summary>
        /// Event that fires when the News Off Button is pressed.  Sends the Stop Streamin News command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewsOff_Click(object sender, EventArgs e)
        {
            // If you no longer want to receve the streaming news headlines.  Send a News Off request.

            axIQFeedY1.TurnOffNews();
        }

        /// <summary>
        /// Event that fires when the Set AutoLogin Button is pressed.  Sends Set Autologin command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetAutoLogin_Click(object sender, EventArgs e)
        {
            // This command sends three commands to the admin port of IQFeed to set the LoginID, Password and turn on AutoConnect.
            if (txtLoginID.Text.Length != 0 && txtPassword.Text.Length != 0)
            {
                string sUserID = txtLoginID.Text;
                string sPassword = txtPassword.Text;
                axIQFeedY1.SetAutoLogin(ref sUserID, ref sPassword);
            }
        }

        /// <summary>
        /// Event that fires when the Clear AutoLogin Button is pressed.  Sends the Remove Autologin command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearAutoLogin_Click(object sender, EventArgs e)
        {
            // This command sends a command to the admin port of IQFeed to turn off autoconnect.
            axIQFeedY1.ClearAutoLogin();
        }

        /// <summary>
        /// Event that fires when the Connect Button is pressed.  Sends the Server Connect command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // This command tells IQFeed to connect to the server.  The only time you will ever need to use this command
            // is if you tell it to disconnect.  All other types of connection/disconnection should be handled automatically.

            axIQFeedY1.RequestReconnect();
        }

        /// <summary>
        /// Event that fires when the Disconnect Button is pressed.  Sends the Server Disconnect command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // This command tells IQFeed to disconnect from the server.  The only time you will ever need to use this command
            // is if your app needs the feed to stop temporarily (most likely for troubleshooting).  
            // DO NOT SEND THIS COMMAND WHEN SHUTTING DOWN YOUR APP!  All your app should do when shutting down is close your
            // socket connection and the feed will handle everything else

            axIQFeedY1.RequestDisconnect();
        }

        /// <summary>
        /// Event that fires when the Echo Button is pressed.  Sends the Echo command to the 
        ///     server via IQFeedY activeX control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEcho_Click(object sender, EventArgs e)
        {
            // this command is primairly used for troubleshooting connectivity to IQConnect.exe
            axIQFeedY1.RequestEcho();
        }

        /// <summary>
        /// Event that fires when a Fundamental Message is recieved from the feed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axIQFeedY1_FundamentalMessage(object sender, AxIQFEEDYLib._DIQFeedYEvents_FundamentalMessageEvent e)
        {
            // fundamental data will contain data about the stock symbol that does not frequently change (at most once a day).
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the fundamental message, please check the documentation page Fundamental Message Format.
            lstData.Items.Insert(0, e.strFundamentalData);
            LimitListItems();
        }

        /// <summary>
        /// Event that fires when a Summary Message is recieved from the feed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axIQFeedY1_SummaryMessage(object sender, AxIQFEEDYLib._DIQFeedYEvents_SummaryMessageEvent e)
        {
            // summary data will be in the same format as the Update messages and will contain the most recent data for each field at the 
            //      time you watch the symbol.
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the Summary message, please check the documentation page UpdateSummaryMessage Format.
            lstData.Items.Insert(0, e.strSummaryData);
            LimitListItems();
        }

        /// <summary>
        /// Event that fires when a Update Message is recieved from the feed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axIQFeedY1_QuoteMessage(object sender, AxIQFEEDYLib._DIQFeedYEvents_QuoteMessageEvent e)
        {
            // Update messages are sent to the client anytime one of the fields in the current fieldset are updated.
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the update message, please check the documentation page UpdateSummary Message Format.
            lstData.Items.Insert(0, e.strQuoteData);
            LimitListItems();
        }
        
        /// <summary>
        /// Event that fires when a Timestamp Message is recieved from the feed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axIQFeedY1_TimeStampMessage(object sender, AxIQFEEDYLib._DIQFeedYEvents_TimeStampMessageEvent e)
        {
            // Timestamp messages are sent to the client once a second.  These timestamps are generated by our servers and can be used as a "server time"
            // In this example, we just display the data to the user.
            lstData.Items.Insert(0, e.strTimeStampData);
            LimitListItems();
        }

        /// <summary>
        /// Event that fires when a System Message is recieved from the feed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axIQFeedY1_SystemMessage(object sender, AxIQFEEDYLib._DIQFeedYEvents_SystemMessageEvent e)
        {
            // system messages are sent to inform the client about current system information.
            // In this example, we use the S,SERVER CONNECTED message to trigger setting the IQFeed Protocol we need.
            // For a list of system messages that can be sent and the fields each contains, please check the documentation page System Messages.
            lstData.Items.Insert(0, e.strSystemData);
            IQ_Config config = new IQ_Config();
            if (e.strSystemData.Contains("S,SERVER CONNECTED"))
            {
                string sProtocolVersion = config.getProtocol();
                axIQFeedY1.SetProtocol(ref sProtocolVersion);
            }
            LimitListItems();
        }

        /// <summary>
        /// Event that fires when a News Message is recieved from the feed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axIQFeedY1_NewsMessage(object sender, AxIQFEEDYLib._DIQFeedYEvents_NewsMessageEvent e)
        {
            // News messages are received anytime a new news story is received for a news type you are authorized to receive AND only when you have streaming news turned on
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the news message, please check the documentation page Streaming News Data Message Format.
            lstData.Items.Insert(0, e.strNewsData);
            LimitListItems();
        }

        /// <summary>
        /// Event that fires when a Regional Message is recieved from the feed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axIQFeedY1_RegionalMessage(object sender, AxIQFEEDYLib._DIQFeedYEvents_RegionalMessageEvent e)
        {
            // Regional messages are sent to the client anytime one of the fields for a region updates AND only when the client has requested 
            //      to watch regionals for a specific symbol.
            // In this example, we just display the data to the user.
            // For a list of fields that are contained in the regional message, please check the documentation page Regional Message Format.

            lstData.Items.Insert(0, e.strRegionalData);
            LimitListItems();
        }

        /// <summary>
        /// Since this is just a sample app, for efficiency, we limit the results to the last 100 messages received.
        /// </summary>
        private void LimitListItems()
        {
            try
            {
                while (lstData.Items.Count > 100)
                {
                    lstData.Items.RemoveAt(100);
                }
            }
            catch (ObjectDisposedException)
            {
                // the listbox went away.  Ingore it since we are probably exiting
            }
        }
    }
}