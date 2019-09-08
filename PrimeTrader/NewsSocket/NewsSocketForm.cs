//-----------------------------------------------------------
//-----------------------------------------------------------
//
//             System: IQFeed
//       Program Name: NewsSocket_VC#.exe
//        Module Name: NewsSocketForm.cs
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
// Module Description: Implementation of Text News via Sockets
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

// added for access to XML parser
using System.Xml;

namespace NewsSocket
{
    public partial class NewsSocketForm : Form
    {
        // socket communication global variables
        AsyncCallback m_pfnLookupCallback;
        Socket m_sockLookup;
        // we create the socket buffer global for performance
        byte[] m_szLookupSocketBuffer = new byte[262144];
        // stores unprocessed data between reads from the socket
        string m_sLookupIncompleteRecord = "";
        // flag for tracking when a call to BeginReceive needs called
        bool m_bLookupNeedBeginReceive = true;
        // used to store the request type so we know how to process the data that is returned
        enum eRequestType
        {
            ertConfig = 0,
            ertHeadlines = 1,
            ertStory = 2,
            ertStoryCount = 3,
        }
        eRequestType m_ertRequestType;

        // delegate for updating the data display.
        public delegate void UpdateDataHandler(string sMessage);

        /// <summary>
        /// Constructor for the form
        /// </summary>
        public NewsSocketForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls and initializing the connection to IQFeed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewsSocketForm_Load(object sender, EventArgs e)
        {
            cboRequestType.Items.Add("Request News Config");
            cboRequestType.Items.Add("Request Headlines by Selected Type(s)");
            cboRequestType.Items.Add("Request Headlines by Symbol(s)");
            cboRequestType.Items.Add("Request Story for Selected Headline");
            cboRequestType.Items.Add("Request Story be emailed");
            cboRequestType.Items.Add("Request Story Counts");
            cboRequestType.SelectedIndex = 0;

            // set default to XML results
            rbXML.Checked = true;

            // create the socket and tell it to connect
            m_sockLookup = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipLocalhost = IPAddress.Parse("127.0.0.1");

            // News data is received from IQFeed on the Lookup port.
            // pull the Lookup port out of the registry
            int iPort = GetIQFeedPort("Lookup");
            IPEndPoint ipendLocalhost = new IPEndPoint(ipLocalhost, iPort);
            
            try
            {

                IQ_Config config = new IQ_Config();
                // Set the protocol for the socket
                string sRequest = String.Format("S,SET PROTOCOL,{0}\r\n", config.getProtocol());

                // connect the socket
                m_sockLookup.Connect(ipendLocalhost);

                byte[] szRequest = new byte[sRequest.Length];
                szRequest = Encoding.ASCII.GetBytes(sRequest);
                int iBytesToSend = szRequest.Length;
                int iBytesSent = m_sockLookup.Send(szRequest, iBytesToSend, SocketFlags.None);
                WaitForData("News");
                // now that we are connected, we want to send a request for a news configuration
                RequestNewsConfig();
            }
            catch (SocketException ex)
            {
                MessageBox.Show(String.Format("Oops.  Did you forget to Login first?\nTake a Look at the LaunchingTheFeed example app\n{0}", ex.Message), "Error Connecting to IQFeed");
                UpdateForm(true, 0);
            }

        }

        /// <summary>
        /// Fires when the user clicks the Submit button.
        ///     We use this to initiate requests to IQFeed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            switch (cboRequestType.SelectedItem.ToString())
            {
                case "Request News Config":
                    RequestNewsConfig();
                    break;
                case "Request Headlines by Selected Type(s)":
                    RequestHeadlinesByType();
                    break;
                case "Request Headlines by Symbol(s)":
                    RequestHeadlinesBySymbol();
                    break;
                case "Request Story for Selected Headline":
                    RequestStory(false);
                    break;
                case "Request Story be emailed":
                    RequestStory(true);
                    break;
                case "Request Story Counts":
                    RequestStoriesCount();
                    break;
            }
        }

        /// <summary>
        /// Requests a list of all news sources
        /// </summary>
        private void RequestNewsConfig()
        {
            // disable controls on the form so the user doesn't make another request until we are ready
            UpdateForm(true, 0);
            
            // save off that we are sending a config request so we know what kind of data to process later
            m_ertRequestType = eRequestType.ertConfig;

            // we request the news config to get a list of available news types
            // the command will be in the format:
            // NCG,TEXT/XML,REQUESTID<CR><LF>
            string sCommand = "NCG";
            
            // check if the user wants XML or Text returned
            if (rbXML.Checked)
            {
                sCommand += ",x";
            }
            else
            {
                sCommand += ",t";
            }
            
            // check for the existance of a requestID
            if (txtRequestID.Text.Length > 0)
            {
                sCommand += "," + txtRequestID.Text;
            }
            // add the <CR><LF> to the end of the request
            sCommand += "\r\n";

            // and we send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            int iBytesSent = m_sockLookup.Send(szCommand, iBytesToSend, SocketFlags.None);
            if (iBytesSent != iBytesToSend)
            {
                UpdateResults(String.Format("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray())));
            }

            // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
            WaitForData("News");
        }

        /// <summary>
        /// Issues a request to IQFeed for a list of headlines associated with a specific news source.
        /// </summary>
        private void RequestHeadlinesByType()
        {
            // disable controls on the form so the user doesn't make another request until we are ready
            UpdateForm(true, 0);
            
            // save off that we are sending a headline request so we know what kind of data to process later
            m_ertRequestType = eRequestType.ertHeadlines;
            
            // clear the current headlines
            lstHeadlines.Items.Clear();

            // we request news headlines based upon the selected news type.
            // the command format for this request can be found in the NewsViaTCPIP page in the IQFeed SDK documentation.

            // check if the user wants XML or Text returned
            string sTextXML = "x";
            if (rbText.Checked)
            {
                sTextXML = "t";
            }
            
            // convert the limit to an integer
            int iLimit = 10;
            if (txtLimit.Text.Length > 0)
            {
                try
                {
                    iLimit = Convert.ToInt32(txtLimit.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Limit must be numeric.  Defaulting to 10.");
                }
            }

            // format the request string
            string sCommand = String.Format("NHL,{0},,{1},{2},{3},{4}\r\n", txtNewsTypes.Text, sTextXML, iLimit, txtDate.Text, txtRequestID.Text);

            // and we send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            int iBytesSent = m_sockLookup.Send(szCommand, iBytesToSend, SocketFlags.None);
            if (iBytesSent != iBytesToSend)
            {
                UpdateResults(String.Format("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray())));
            }


            // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
            WaitForData("News");
        }

        /// <summary>
        /// Issues a request to IQFeed for a list of headlines associated with a specific symbol.
        /// </summary>
        private void RequestHeadlinesBySymbol()
        {
            // disable controls on the form so the user doesn't make another request until we are ready
            UpdateForm(true, 0);
            
            // save off that we are sending a headline request so we know what kind of data to process later
            m_ertRequestType = eRequestType.ertHeadlines;
            
            // clear the current headlines
            lstHeadlines.Items.Clear();

            // we request news stories based upon the selected news headline.
            // the command format for this request can be found in the NewsViaTCPIP page in the IQFeed SDK documentation.

            // check if the user wants XML or Text returned
            string sTextXML = "x";
            if (rbText.Checked)
            {
                sTextXML = "t";
            }
            
            // convert the limit to an integer
            int iLimit = 10;
            if (txtLimit.Text.Length > 0)
            {
                try
                {
                    iLimit = Convert.ToInt32(txtLimit.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Limit must be numeric.  Defaulting to 10.");
                }
            }

            // format the request string
            string sCommand = String.Format("NHL,{0},{1},{2},{3},,{4}\r\n", txtNewsTypes.Text, txtSymbol.Text, sTextXML, iLimit, txtRequestID.Text);

            // and we send it to the feed via the socket
            byte[] szCommand = new byte[sCommand.Length];
            szCommand = Encoding.ASCII.GetBytes(sCommand);
            int iBytesToSend = szCommand.Length;
            int iBytesSent = m_sockLookup.Send(szCommand, iBytesToSend, SocketFlags.None);
            if (iBytesSent != iBytesToSend)
            {
                UpdateResults(String.Format("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray())));
            }

            // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
            WaitForData("News");
        }

        /// <summary>
        /// Issues a request to IQFeed for a Story Text
        /// </summary>
        /// <param name="bEmail"></param>
        private void RequestStory(bool bEmail)
        {
            // save off that we are sending a headline request so we know what kind of data to process later
            m_ertRequestType = eRequestType.ertStory;

            // we request news stories based upon the selected news headline.
            // the command format for this request can be found in the NewsViaTCPIP page in the IQFeed SDK documentation.

            // verify user has a storyID populated
            if (!txtStoryID.Text.Equals(""))
            {
                // clear the textbox where the results will be displayed
                txtResults.Text = "";
                // disable controls on the form so the user doesn't make another request until we are ready
                UpdateForm(true, 0);
                // check if user is emailing story or requesting it be delivered via the feed
                string sCommand;
                if (bEmail)
                {
                    // send the request
                    sCommand = String.Format("NSY,{0},e,{1}\r\n", txtStoryID.Text, txtEmail.Text);
                    // there is no result sent back from the feed on this request so just enable the form.
                    UpdateForm(false, cboRequestType.SelectedIndex);
                }
                else
                {
                    // check if the user wants XML or Text returned
                    string sTextXML = "x";
                    if (rbText.Checked)
                    {
                        sTextXML = "t";
                    }
                    // send the request
                    sCommand = String.Format("NSY,{0},{1},,{2}\r\n", txtStoryID.Text, sTextXML, txtRequestID.Text);
                }

                // and we send it to the feed via the socket
                byte[] szCommand = new byte[sCommand.Length];
                szCommand = Encoding.ASCII.GetBytes(sCommand);
                int iBytesToSend = szCommand.Length;
                int iBytesSent = m_sockLookup.Send(szCommand, iBytesToSend, SocketFlags.None);
                if (iBytesSent != iBytesToSend)
                {
                    UpdateResults(String.Format("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray())));
                }

                // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
                WaitForData("News");
            }
            else
            {
                MessageBox.Show("StoryID required to request a story");
            }
        }

        /// <summary>
        /// Issues a request to IQFeed for the number of stories available for a given symbol
        /// </summary>
        private void RequestStoriesCount()
        {
            // we request news stories based upon the selected news headline.
            // the command format for this request can be found in the NewsViaTCPIP page in the IQFeed SDK documentation.

            // save off that we are sending a story count request so we know what kind of data to process later
            m_ertRequestType = eRequestType.ertStoryCount;

            // verify a symbol is entered
            if (!txtSymbol.Text.Equals(""))
            {
                // clear the textbox where the results will be displayed
                txtResults.Text = "";
                // disable controls on the form so the user doesn't make another request until we are ready
                UpdateForm(true, 0);

                // check if the user wants XML or Text returned
                string sTextXML = "x";
                if (rbText.Checked)
                {
                    sTextXML = "t";
                }

                // format the request string
                string sCommand = String.Format("NSC,{0},{1},{2},{3},{4}\r\n", txtSymbol.Text, sTextXML, txtNewsTypes.Text, txtDate.Text, txtRequestID.Text);

                // and we send it to the feed via the socket
                byte[] szCommand = new byte[sCommand.Length];
                szCommand = Encoding.ASCII.GetBytes(sCommand);
                int iBytesToSend = szCommand.Length;
                int iBytesSent = m_sockLookup.Send(szCommand, iBytesToSend, SocketFlags.None);
                if (iBytesSent != iBytesToSend)
                {
                    UpdateResults(String.Format("Error: {0} command not sent to IQConnect", sCommand.TrimEnd("\r\n".ToCharArray())));
                }

                // we call our WaitForData function (see below) to notify the socket that we are ready to receive callbacks when new data arrives
                WaitForData("News");
            }
            else
            {
                MessageBox.Show("Symbol(s) required to request a story counts by symbol");
            }
        }

        /// <summary>
        /// we call this to notify the .NET Async socket to start listening for data to come in.  It must be called each time after we receive data
        /// </summary>
        private void WaitForData(string sSocketName)
        {
            if (sSocketName.Equals("News"))
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

        /// <summary>
        /// This is our callback that gets called by the .NET socket class when new data arrives on the socket
        /// </summary>
        /// <param name="asyn"></param>
        private void OnReceive(IAsyncResult asyn)
        {
            // first verify we received data from the correct socket.  This check isn't really necessary in this example since we 
            // only have a single socket but if we had multiple sockets, we could use this check to use the same function to recieve data from
            // multiple sockets
            if (asyn.AsyncState.ToString().Equals("News"))
            {
                // read data from the socket
                int iReceivedBytes = 0;
                iReceivedBytes = m_sockLookup.EndReceive(asyn);
                m_bLookupNeedBeginReceive = true;
                // add the data received from the socket to any data that was left over.  m_strIncompleteRecord is populated during
                // processing when the last record recieved is detected as being incomplete
                string sData = m_sLookupIncompleteRecord + Encoding.ASCII.GetString(m_szLookupSocketBuffer, 0, iReceivedBytes);
                // clear the incomplete record string so it doesn't get added again next time we read from the socket
                m_sLookupIncompleteRecord = "";
                if (sData.Contains("S,CURRENT PROTOCOL"))
                {
                    sData = "";
                }
                
                // make sure we have complete request results before processing
                if (sData.EndsWith("!ENDMSG!,\r\n"))
                {
                    switch (m_ertRequestType)
                    {
                        case eRequestType.ertConfig:
                            // this is a News Config
                            // update the treeview.  We use a delegate for this to resolve cross threading issues 
                            // within the framework (the Socket Callback gets executed on a different thread than the form).
                            UpdateNewsConfig(sData);
                            break;
                        case eRequestType.ertHeadlines:
                            // this is a list of headlines
                            // update the listbox.  We use a delegate for this to resolve cross threading issues 
                            // within the framework (the Socket Callback gets executed on a different thread than the form).
                            UpdateHeadlines(sData);
                            break;
                        case eRequestType.ertStory:
                            // this is a news story
                            // update the results TextBox.  We use a delegate for this to resolve cross threading issues 
                            // within the framework (the Socket Callback gets executed on a different thread than the form).
                            UpdateResults(sData);
                            break;
                        case eRequestType.ertStoryCount:
                            // this is a news story
                            // update the results TextBox.  We use a delegate for this to resolve cross threading issues 
                            // within the framework (the Socket Callback gets executed on a different thread than the form).
                            UpdateResults(sData);
                            break;
                    }
                }
                else
                {
                    m_sLookupIncompleteRecord = sData;
                    // if the data isn't complete, we need more data from the socket
                    WaitForData("News");
                }
            }
        }
        
        /// <summary>
        /// We want to be able to update the news configuration treeview from within the AsyncSocket Callback 
        /// so we need a delagate to resolve cross-threading issues.
        /// </summary>
        /// <param name="data"></param>
        public void UpdateNewsConfig(string sData)
        {
            try
            {
                // check if we need to use a delegate for this call.
                if (treeNewsTypes.InvokeRequired || txtResults.InvokeRequired)
                {
                    this.Invoke(new UpdateDataHandler(UpdateNewsConfig), sData);
                }
                else
                {
                    // clear existing items in the tree.
                    treeNewsTypes.Nodes.Clear();

                    // update the raw data textbox
                    txtResults.Text = sData;

                    // check if there was a request ID specified and remove it 
                    if (txtRequestID.Text.Length > 0)
                    {
                        string sRequestID = txtRequestID.Text + ",";
                        sData = sData.Replace(sRequestID, "");
                    }

                    // trim off the !ENDMSG!
                    sData = sData.Substring(0, sData.IndexOf("!ENDMSG!,"));
                    sData = sData.TrimEnd("\r\n".ToCharArray());

                    // process the data
                    if (rbXML.Checked)
                    {
                        // data should be xml
                        TreeNode tnCategory = new TreeNode();
                        TreeNode tnMajorType = new TreeNode();
                        TreeNode tnMinorType = new TreeNode();

                        // we are adding a bunch of stuff to a tree, we don't want it trying to repaint while we are adding stuff
                        treeNewsTypes.BeginUpdate();

                        // parse news config into the tree.

                        // The XML news config is formatted as follows (from the documentation page NewsviaTCPIP):

                        /* Response:   The response to a news configuration query is in the form of an XML document. 
                                   Elements Category, major_type, and minor_type may repeat. 
                                   The following document object tree defines this document's structure. 
                                   When all elements have been received, you will receive a <CR><LF>!ENDMSG!<CR><LF> message. Example: 
                        
                                   Document: DynamicNewsConf()
                                       Element: Category(Name)
                                       Element: major_type()
                                           Attribute: Type()
                                           Attribute: Name()
                                           Attribute: auth_code()
                                           Attribute: icon_id()
                                       Element: minor_type()
                                           Attribute: Type()
                                           Attribute: Name()
                                           Attribute: auth_code()
                                           Attribute: icon_id()
                         */

                        XmlDocument xmlDoc = new XmlDocument();

                        // load the XML
                        xmlDoc.LoadXml(sData);

                        // setup our xml Lists
                        XmlNodeList xmlCategoryList = xmlDoc.SelectNodes("/DynamicNewsConf/category");
                        XmlNodeList xmlMajorList = xmlDoc.SelectNodes("/DynamicNewsConf/category/major_type");
                        XmlNodeList xmlMinorList = xmlDoc.SelectNodes("/DynamicNewsConf/category/major_type/minor_type");
                        string sNameAttribute = "name";
                        string sTypeAttribute = "type";
                        // loop through the category list and populate cboCategory
                        foreach (XmlNode xmlCatNode in xmlCategoryList)
                        {
                            string sCategory = xmlCatNode.Attributes.GetNamedItem("name").Value;

                            // add the category to the tree
                            tnCategory = new TreeNode(sCategory);

                            // loop through the majors for this category
                            foreach (XmlNode xmlMajorNode in xmlMajorList)
                            {
                                string sCurrentMajorsCategory = xmlMajorNode.ParentNode.Attributes.GetNamedItem(sNameAttribute).Value;
                                if (sCurrentMajorsCategory.Equals(sCategory))
                                {
                                    string sMajor = xmlMajorNode.Attributes.GetNamedItem(sNameAttribute).Value;
                                    string sMajorType = xmlMajorNode.Attributes.GetNamedItem(sTypeAttribute).Value;
                                    // add a node to the treeview for this major type
                                    tnMajorType = new TreeNode(String.Format("{0},{1}", sMajorType, sMajor));

                                    // loop through the minor nodes for this major
                                    foreach (XmlNode xmlMinorNode in xmlMinorList)
                                    {
                                        string sCurrentMinorsMajor = xmlMinorNode.ParentNode.Attributes.GetNamedItem(sNameAttribute).Value;
                                        if (sCurrentMinorsMajor.Equals(sMajor))
                                        {
                                            string sMinor = xmlMinorNode.Attributes.GetNamedItem(sNameAttribute).Value;
                                            string sMinorType = xmlMinorNode.Attributes.GetNamedItem(sTypeAttribute).Value;

                                            // add the minor type to the treeview.
                                            tnMinorType = new TreeNode(String.Format("{0},{1}", sMinorType, sMinor));
                                            // minor types never have children, add it and move on to the next minor type
                                            tnMajorType.Nodes.Add(tnMinorType);
                                        }
                                    }
                                    // add the current major type to the treeview and then move on to the next major type
                                    if (!tnMajorType.Text.Equals(""))
                                    {
                                        tnCategory.Nodes.Add(tnMajorType);
                                    }
                                }
                            }
                            // add the current category to the tree and move on to the next one
                            if (!tnCategory.Text.Equals(""))
                            {
                                treeNewsTypes.Nodes.Add(tnCategory);
                            }
                        }

                        // verify that the config parsed properly
                        if (treeNewsTypes.Nodes.Count == 0)
                        {
                            MessageBox.Show("There was a problem parsing the News Config");
                        }
                        // once we are done processing the news config, tell the treeview object we are done so it can repaint
                        treeNewsTypes.EndUpdate();

                    }
                    else
                    {
                        // data should be text

                        string sCurrentCategory = "";
                        string sCurrentMajorType = "";
                        string sCurrentMinorType = "";
                        TreeNode tnCategory = new TreeNode();
                        TreeNode tnMajorType = new TreeNode();
                        TreeNode tnMinorType = new TreeNode();

                        // we are adding a bunch of stuff to a tree, we don't want it trying to repaint while we are adding stuff
                        treeNewsTypes.BeginUpdate();

                        // parse news config into the tree.
                        // The text news config is formatted as follows (from the documentation page NewsviaTCPIP):
                        //Response: The resulting data comes as a sequence of messages. 
                        //          The fields in each message are separated by commas, and each message is terminated with a <LF>.
                        //          Elements CATEGORY, MAJOR, and MINOR may repeat. 
                        //          When all elements have been received, you will receive a <CR><LF>!ENDMSG!<CR><LF> message. Example: 
                        //
                        //  CATEGORY,[category name]<LF> 
                        //      MAJOR,[type],[name],[auth code],[icon id]<LF> 
                        //          MINOR,[type],[name],[auth code],[icon id]<LF>
                        //  <CR><LF>!ENDMSG!<CR><LF>

                        // as a result, we split the string on newline characters into a List
                        List<string> lstMessages = new List<string>(sData.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                        // and then we loop through the list adding to the tree
                        foreach (string sLine in lstMessages)
                        {
                            if (sLine.StartsWith("CATEGORY"))
                            {
                                if (!sCurrentCategory.Equals(""))
                                {
                                    // not the first category, add the previous node
                                    treeNewsTypes.Nodes.Add(tnCategory);
                                }
                                sCurrentCategory = sLine.Substring(sLine.IndexOf(",") + 1);
                                tnCategory = new TreeNode(sCurrentCategory);
                            }
                            else if (sLine.StartsWith("MAJOR"))
                            {
                                sCurrentMajorType = sLine.Substring(sLine.IndexOf(",") + 1);
                                tnMajorType = new TreeNode(sCurrentMajorType);
                                if (!tnCategory.Text.Equals(""))
                                {
                                    tnCategory.Nodes.Add(tnMajorType);
                                }
                            }
                            else if (sLine.StartsWith("MINOR"))
                            {
                                sCurrentMinorType = sLine.Substring(sLine.IndexOf(",") + 1);
                                tnMinorType = new TreeNode(sCurrentMinorType);
                                if (!tnMajorType.Text.Equals(""))
                                {
                                    tnMajorType.Nodes.Add(tnMinorType);
                                }
                            }
                        }
                        // add the last category node
                        if (!tnCategory.Text.Equals(""))
                        {
                            treeNewsTypes.Nodes.Add(tnCategory);
                        }
                        // we are done.  tell the tree it can once again repaint
                        treeNewsTypes.EndUpdate();
                    }
                    // restore form controls
                    UpdateForm(false, cboRequestType.SelectedIndex);
                }
            }
            catch (ObjectDisposedException)
            {
                // One of the objects went away, ignore it since we're probably exiting.
            }
        }

        /// <summary>
        /// We want to be able to update the winform headline listbox from within the AsyncSocket Callback 
        /// so we need a delagate to resolve cross-threading issues.
        /// </summary>
        /// <param name="data"></param>
        public void UpdateHeadlines(string sData)
        {
            try
            {
                // check if we need to use a delegate for this call.
                if (lstHeadlines.InvokeRequired || txtResults.InvokeRequired)
                {
                    this.Invoke(new UpdateDataHandler(UpdateHeadlines), sData);
                }
                else
                {
                    // clear the previous request results
                    lstHeadlines.Items.Clear();

                    // update the raw data textbox
                    txtResults.Text = sData;

                    // check if there was a request ID specified and remove it 
                    if (txtRequestID.Text.Length > 0)
                    {
                        string sRequestID = txtRequestID.Text + ",";
                        sData = sData.Replace(sRequestID, "");
                    }

                    // trim off the !ENDMSG!
                    sData = sData.Substring(0, sData.IndexOf("!ENDMSG!,"));
                    sData = sData.TrimEnd("\r\n".ToCharArray());

                    // now parse the output for display
                    if (rbXML.Checked)
                    {
                        // The XML news headlines are formatted as follows (from the documentation page NewsviaTCPIP):

                        /* Response: The response to a news headlines query is in the form of an XML document. 
                                   In the "news_headlines document, the "news_headline" element may repeat. 
                                   The "id" element is the unique identifier of the story used for retrieving the story's text. 
                                   The "source" element is used to list the story's distribution Type. 
                                   The time stamp is the date and time of the story's release in the format "YYYYMMDDHHMMSS". 
                                   The "text" element is the text of the headline. The following document object tree defines this document's structure. 
                                   When all elements have been received, you will receive a <CR><LF>!ENDMSG!<CR><LF> message. Example: 

                                   Document: news_headlines()
                                       Element: news_headline()
                                           Element: id()
                                           Element: source()
                                           Element: timestamp()
                                           Element: symbols()
                                           Element: Text()
                         */

                        if (sData.Length > 0)
                        {
                            // populate the listbox

                            XmlDocument xmlHeadlineDoc = new XmlDocument();
                            XmlNodeList xmlHeadlineList;

                            // load the XML string into the xmlHeadlineDoc
                            xmlHeadlineDoc.LoadXml(sData);

                            // populate headline list
                            xmlHeadlineList = xmlHeadlineDoc.SelectNodes("/news_headlines/news_headline");

                            // loop through headlinelist and parse the headlines
                            foreach (XmlNode xmlChildNode in xmlHeadlineList)
                            {
                                string sID = "";
                                string sSource = "";
                                string sText = "";
                                // each node in headlinelist has 3 child nodes.  Loop through and store the values
                                foreach (XmlNode xmlHeadlineNode in xmlChildNode.ChildNodes)
                                {
                                    if (xmlHeadlineNode.LocalName.Equals("id"))
                                    {
                                        sID = xmlHeadlineNode.InnerText;
                                    }
                                    else if (xmlHeadlineNode.LocalName.Equals("source"))
                                    {
                                        sSource = xmlHeadlineNode.InnerText;
                                    }
                                    else if (xmlHeadlineNode.LocalName.Equals("text"))
                                    {
                                        sText = xmlHeadlineNode.InnerText;
                                    }
                                }
                                // add the headline to the list in the same format as the text request is returned 
                                // so we can use the same processing to parse it
                                lstHeadlines.Items.Add(String.Format("N,{0},{1},{2}", sSource, sID, sText));
                            }
                        }
                    }
                    else
                    {
                        // data should be text
                        List<String> lstMessages = new List<string>(sData.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                        lstHeadlines.BeginUpdate();
                        lstMessages.ForEach(delegate(String sLine)
                        {
                            lstHeadlines.Items.Add(sLine);
                        });
                        lstHeadlines.EndUpdate();
                    }
                    
                    if (lstHeadlines.Items.Count < 1)
                    {
                        // no headlines found
                        lstHeadlines.Items.Add("<NONE>");
                    }

                    // restore form controls
                    UpdateForm(false, cboRequestType.SelectedIndex);
                }
            }
            catch (ObjectDisposedException)
            {
                // One of the objects went away, ignore it since we're probably exiting.
            }
        }

        /// <summary>
        /// We want to be able to update the winform results textbox from within the AsyncSocket Callback 
        /// so we need a delagate to resolve cross-threading issues.
        /// </summary>
        /// <param name="strData"></param>
        public void UpdateResults(string sData)
        {
            try
            {
                // check if we need to use a delegate for this call.
                if (lstHeadlines.InvokeRequired || txtResults.InvokeRequired || txtStory.InvokeRequired)
                {
                    this.Invoke(new UpdateDataHandler(UpdateResults), sData);
                }
                else
                {
                    // this processes both story and story counts requests
                    // for both requests there are a few steps we always do to prepare the data.

                    // update the raw data textbox
                    txtResults.Text = sData;

                    // check if there was a request ID specified and remove it 
                    if (txtRequestID.Text.Length > 0)
                    {
                        string sRequestID = txtRequestID.Text + ",";
                        sData = sData.Replace(sRequestID, "");
                    }

                    // trim off the !ENDMSG!
                    int iEndMsgPos = sData.IndexOf("!ENDMSG!,");
                    if (iEndMsgPos > -1)
                    {
                        sData = sData.Substring(0, iEndMsgPos);
                        sData = sData.TrimEnd("\r\n".ToCharArray());

                        // now we will have different processing depending on what kind of request we have
                        if (m_ertRequestType == eRequestType.ertStory)
                        {
                            // clear any previous results
                            txtStory.Text = "";
                            if (rbXML.Checked)
                            {
                                // The XML news stories are formatted as follows (from the documentation page NewsviaTCPIP):

                                /* Response: The response to a news story query is in the form of an XML document. In the 
                                           "news_stories" document, "news_story" may repeat. The element "is_link" indicates 
                                           a "Y" or a "N" to indicate the story is a linked story. The body of the story is 
                                           in "story_text". The following document object tree defines this document's structure. 
                                           When all elements have been received, you will receive a <CR><LF>!ENDMSG!<CR><LF> message.

                                 Document:   news_stories()
                                       Element:    news_story()
                                       Element:    is_link()
                                       Element:    story_text()
                                 <CR><LF>!ENDMSG!<CR><LF> 
                                */

                                if (sData.Length > 0)
                                {
                                    XmlDocument xmlStoryDoc = new XmlDocument();
                                    XmlNode xmlStoryNode;

                                    // load the XML string into the xmlStoryDoc
                                    xmlStoryDoc.LoadXml(sData);

                                    // there should only be one node containing story text.  There are other nodes but we
                                    // don't care about them for this app.
                                    xmlStoryNode = xmlStoryDoc.SelectSingleNode("/news_stories/news_story/story_text");

                                    // save it in the text box
                                    txtStory.Text = xmlStoryNode.InnerText;
                                }
                            }
                            else
                            {
                                // results are in text wrapped in <BEGIN> and <END> tags.
                                // just strip out those tags and display in the text box
                                sData = sData.Substring("<BEGIN>".Length);
                                txtStory.Text = sData.Substring(0, sData.Length - "<END>\r\n".Length);
                            }

                            // restore form controls
                            UpdateForm(false, cboRequestType.SelectedIndex);
                            // show the story textbox
                            txtStory.Visible = true;
                            txtStory.Enabled = true;
                            lstHeadlines.Visible = false;
                            lstHeadlines.Enabled = false;
                        }
                        else if (m_ertRequestType == eRequestType.ertStoryCount)
                        {
                            // parse the output for display
                            if (sData.Length > 0)
                            {
                                // clear previous results
                                lstHeadlines.Items.Clear();

                                // populate the listbox
                                if (rbXML.Checked)
                                {
                                    // results should be XML

                                    XmlDocument xmlStoryCountsDoc = new XmlDocument();
                                    XmlNodeList xmlStoryCountList;

                                    // load the XML string into the xmlStoryCountsDoc
                                    xmlStoryCountsDoc.LoadXml(sData);

                                    // populate storycount list
                                    xmlStoryCountList = xmlStoryCountsDoc.SelectNodes("/story_counts");

                                    // loop through storycount list and parse the xml nodes
                                    foreach (XmlNode xmlChildNode in xmlStoryCountList)
                                    {
                                        string sSymbol = "";
                                        string sCount = "";
                                        // each node in the storycount list has 2 attributes.  Loop through and store the values
                                        foreach (XmlNode xmlStoryCountNode in xmlChildNode.ChildNodes)
                                        {
                                            // get the attributes
                                            sSymbol = xmlStoryCountNode.Attributes.GetNamedItem("Name").Value;
                                            sCount = xmlStoryCountNode.Attributes.GetNamedItem("StoryCount").Value;
                                            // add the results to the listbox
                                            lstHeadlines.Items.Add(String.Format("{0} has {1} stories.", sSymbol, sCount));
                                        }
                                    }
                                }
                                else
                                {
                                    // data from the feed was not XML

                                    // the story counts received should be received in the following format:
                                    // StoryCounts:SYMBOL1,SYMOL1COUNT:SYMBOL2,SYMBOL2COUNT:...:SYMBOLN,SYMBOLNCOUNT<CR><LF>
                                    // populate the listbox
                                    foreach (string sStoryCount in sData.Split("\r\n:".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        int iIndex = sStoryCount.IndexOf(",");
                                        // ignore any line without a comma
                                        if (iIndex > -1)
                                        {
                                            // make the results readable and add to the listbox
                                            lstHeadlines.Items.Add(String.Format("{0} has {1} stories.", sStoryCount.Substring(0, iIndex), sStoryCount.Substring(iIndex + 1)));
                                        }
                                    }
                                }
                            }

                            // display something to the user to indicate nothing was returned if necessary
                            if (lstHeadlines.Items.Count < 1)
                            {
                                // no headlines found
                                lstHeadlines.Items.Add("<NONE>");
                            }

                            // restore form controls
                            UpdateForm(false, cboRequestType.SelectedIndex);
                        }
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // One of the objects went away, ignore it since we're probably exiting.
            }
        }

        /// <summary>
        /// Fires when something is checked or unchecked in the treeview.
        ///     We use this to populate the Types Box based on what is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeNewsTypes_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // clear out anythign already in the box.
            txtNewsTypes.Text = "";
            bool bSomethingIsChecked = false;

            // loop through the categories (top level of the tree)
            foreach (TreeNode tnCategory in treeNewsTypes.Nodes)
            {
                // for each category, loop through the major types (middle level of the tree)
                foreach (TreeNode tnMajor in tnCategory.Nodes)
                {
                    // Major types are only used if they have no minor types
                    if ((tnMajor.Nodes.Count == 0) && tnMajor.Checked)
                    {
                        bSomethingIsChecked = true;
                        // we have something checked, add it to our comma delimited list of types
                        txtNewsTypes.Text += tnMajor.Text.Substring(0, tnMajor.Text.IndexOf(","));
                        txtNewsTypes.Text += ":";
                    }
                    else
                    {
                        foreach (TreeNode tnMinor in tnMajor.Nodes)
                        {
                            if (tnMinor.Checked)
                            {
                                bSomethingIsChecked = true;
                                // we have something checked, add it to our comma delimited list of types
                                txtNewsTypes.Text += tnMinor.Text.Substring(0, tnMinor.Text.IndexOf(","));
                                txtNewsTypes.Text += ":";
                            }
                        }
                    }
                }
            }
            // trim off the trailing comma if it exists
            txtNewsTypes.Text = txtNewsTypes.Text.TrimEnd(":".ToCharArray());

            // if anything is currently checked, update the requestType Combo so the user doesn't have to
            if (bSomethingIsChecked)
            {
                cboRequestType.SelectedIndex = cboRequestType.FindString("Request Headlines by Selected Type(s)");
            }
        }

        /// <summary>
        /// Fires when a new item is seleceted in the Request Type Combo Box
        ///     We use this to enable/disable controls on the form as appropriate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboRequestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateForm(false, cboRequestType.SelectedIndex);
        }

        /// <summary>
        /// Fires when a user clicks on a headline.
        ///     We use this to populate the storyID TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstHeadlines_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the selected headline from the listbox
            string sHeadline = lstHeadlines.SelectedItem.ToString();
            // The headline should be in the format:
            // N,[source],[id],[symbols],[timestamp],[text]<CR><LF>
            // get the headline ID out of the headline.
            List<string> lstHeadlineParts = new List<string>(sHeadline.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            txtStoryID.Text = lstHeadlineParts[2];
            // we set the combo to the request story for the headline (just for ease of use of the example app)
            cboRequestType.SelectedIndex = cboRequestType.FindString("Request Story for Selected Headline");
        }

        /// <summary>
        /// Enables / Disables the controls on the form.
        /// </summary>
        private void UpdateForm(bool bDisableAll, int iSelectedIndex)
        {
            // disable all forms on the control
            foreach (Control c in this.Controls)
            {
                // handle the group box
                if (c is GroupBox)
                {
                    foreach (Control gbControl in c.Controls)
                    {
                        if (!(gbControl is Label))
                        {
                            gbControl.Enabled = false;
                        }
                    }
                }
                else if (!(c is Label))
                {
                    c.Enabled = false;
                }
            }
            // if bDisableAll is not set, then update for the currently selected request type
            if (!bDisableAll)
            {
                // these controls are always enabled
                cboRequestType.Enabled = true;
                btnSubmit.Enabled = true;
                treeNewsTypes.Enabled = true;
                txtRequestID.Enabled = true;
                rbXML.Enabled = true;
                rbText.Enabled = true;
                txtResults.Enabled = true;
                switch (cboRequestType.Items[iSelectedIndex].ToString())
                {
                    case "Request News Config":
                        // nothing else needs enabled
                        break;
                    case "Request Headlines by Symbol(s)":
                        txtSymbol.Enabled = true;
                        lstHeadlines.Enabled = true;
                        lstHeadlines.Visible = true;
                        txtStory.Visible = false;
                        txtNewsTypes.Enabled = true;
                        txtLimit.Enabled = true;
                        break;
                    case "Request Headlines by Selected Type(s)":
                        lstHeadlines.Enabled = true;
                        lstHeadlines.Visible = true;
                        txtStory.Visible = false;
                        txtNewsTypes.Enabled = true;
                        txtLimit.Enabled = true;
                        txtDate.Enabled = true;
                        break;
                    case "Request Story for Selected Headline":
                        lstHeadlines.Enabled = true;
                        txtResults.Enabled = true;
                        txtStoryID.Enabled = true;
                        break;
                    case "Request Story be emailed":
                        lstHeadlines.Enabled = true;
                        txtStoryID.Enabled = true;
                        txtEmail.Enabled = true;
                        rbXML.Enabled = false;
                        rbText.Enabled = false;
                        txtRequestID.Enabled = false;
                        break;
                    case "Request Story Counts":
                        txtSymbol.Enabled = true;
                        txtDate.Enabled = true;
                        txtNewsTypes.Enabled = true;
                        txtResults.Enabled = true;
                        lstHeadlines.Enabled = true;
                        lstHeadlines.Visible = true;
                        txtStory.Visible = false;
                        break;
                }
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
    }
}