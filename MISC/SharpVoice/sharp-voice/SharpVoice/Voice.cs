using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;

namespace SharpVoice
{
    public class Voice : IDisposable
    {   
        private static String rnrSEE = null;
	    String user = null;
	    String pass = null;
		private static CookieCollection cookies = new CookieCollection();
        public static CookieContainer cookiejar = new CookieContainer();

        public static int MaxPages = 9999;

        /*
		 * Links Imported from https://pygooglevoice.googlecode.com/hg/googlevoice/settings.py
		 * Made to be more compatible with porting future python code.
		*/
		//string LOGIN = "https://www.google.com/accounts/ServiceLoginAuth?service=grandcentral";
		//string LOGIN = "https://www.google.com/accounts/ClientLogin";

        //string[] FEEDS = new String[]{"inbox", "starred", "all", "spam", "trash", "voicemail", "sms",
        //        "recorded", "placed", "received", "missed"};

        #region urls
        const string BASE = "https://www.google.com/voice/";
        const string XML_RECENT = BASE + "inbox/recent/";

        public static Dictionary<string, string> dict = new Dictionary<string, string>(){
            {"BASE","https://www.google.com/voice/"},
            {"LOGIN","https://accounts.google.com/ServiceLogin?service=grandcentral"},
            {"RNRSE","https://accounts.google.com/ServiceLogin?service=grandcentral&continue=https://www.google.com/voice/&followup=https://www.google.com/voice/&ltmpl=open"},
            {"LOGOUT",BASE + "account/signout"},
    		{"INBOX",BASE + "#inbox"},
    		{"CALL",BASE + "call/connect/"},
    		{"CANCEL",BASE + "call/cancel/"},
    		{"DEFAULT_FORWARD",BASE + "settings/editDefaultForwarding/"},
    		{"FORWARD",BASE + "settings/editForwarding/"},
            {"ARCHIVE", BASE + "inbox/archiveMessages/"},
    		{"DELETE",BASE + "inbox/deleteMessages/"},
    		{"MARK",BASE + "inbox/mark/"},
    		{"STAR",BASE + "inbox/star/"},
    		{"SMS",BASE + "sms/send/"},
    		{"DOWNLOAD",BASE + "media/send_voicemail/"},
    		{"BALANCE",BASE + "settings/billingcredit/"},
    		{"XML_SEARCH",BASE + "inbox/search/"},
    		{"XML_CONTACTS",BASE + "contacts/"},
    		{"XML_INBOX",XML_RECENT + "inbox/"},
    		{"XML_STARRED",XML_RECENT + "starred/"},
    		{"XML_ALL",XML_RECENT + "all/"},
    		{"XML_SPAM",XML_RECENT + "spam/"},
    		{"XML_TRASH",XML_RECENT + "trash/"},
    		{"XML_VOICEMAIL",XML_RECENT + "voicemail/"},
    		{"XML_SMS",XML_RECENT + "sms/"},
    		{"XML_RECORDED",XML_RECENT + "recorded/"},
    		{"XML_PLACED",XML_RECENT + "placed/"},
    		{"XML_RECEIVED",XML_RECENT + "received/"},
    		{"XML_MISSED",XML_RECENT + "missed/"},
            {"SMSAUTH","https://accounts.google.com/SmsAuth?service=grandcentral"}
        };
        #endregion

        /// <summary>
        /// Login with email address and password. No persist. No pin.
        /// </summary>
        /// <param name="user">Email address for account</param>
        /// <param name="pass">Password for account</param>
        public Voice(string user, string pass) : this(user, pass, false, "") { }

        /// <summary>
        /// Login with email, password, and persist. No pin.
        /// </summary>
        /// <param name="user">Email address for account</param>
        /// <param name="pass">Password for account</param>
        /// <param name="persist">Should we stay logged in?</param>
        public Voice(string user, string pass, bool persist) : this(user, pass, persist, "") { }

        /// <summary>
        /// Login with email, password, and 2-factor authentication pin. No persist.
        /// </summary>
        /// <param name="user">Email address for account</param>
        /// <param name="pass">Password for account</param>
        /// <param name="pin">2-factor auth code (6-8 digits)</param>
        public Voice(string user, string pass, string pin) : this(user, pass, false, pin) { }

        /// <summary>
        /// Login with email, password, persist, and 2-factor authentication pin.
        /// </summary>
        /// <param name="user">Email address for account</param>
        /// <param name="pass">Password for account</param>
        /// <param name="persist">Should we stay logged in?</param>
        /// <param name="pin">2-factor auth code (6-8 digits)</param>
        public Voice(string user, string pass, bool persist, string pin )
        {
            this.user = user;
            
            Login(pass, pin, persist);
        }
		
		~Voice(){
			Logout();
		}
		
		/*
		inbox - Recent, unread messages
		starred - Starred messages
		all - All messages
		spam - Messages likely to be spam
		trash - Deleted messages
		voicemail - Voicemail messages
		sms - Text messages
		recorded - Recorced messages
		placed - Outgoing messages
		received - Incoming messages
		missed - Messages not received
		*/

		#region Folders
        private string getInbox()
        {
            return getInbox(1);
        }

        private string getInbox(int page){
            return Request(dict["XML_INBOX"]+"?page=p" + page);
	    }

        public string getStarred()
        {
            return getStarred(1);
        }
	    public string getStarred(int page){
            return Request(dict["XML_STARRED"] + "?page=p" + page);
	    }
        public string getRecent(int page)
        {
            return Request(dict["XML_ALL"] + "?page=p" + page);
	    }
        public String getSpam(int page)
        {
        	return Request(dict["XML_SPAM"] + "?page=p" + page);
	    }
        public String getRecorded(int page)
        {
            return Request(dict["XML_RECORDED"] + "?page=p" + page);
	    }
        public String getPlaced(int page)
        {
            return Request(dict["XML_PLACED"] + "?page=p" + page);
	    }
        public String getReceived(int page)
        {
            return Request(dict["XML_RECEIVED"] + "?page=p" + page);
	    }
        public String getMissed(int page)
        {
            return Request(dict["XML_MISSED"] + "?page=p" + page);
	    }
        private String getSMS(int page)
        {
            return Request(dict["XML_SMS"] + "?page=p" + page);
	    }
        private string getAll(int page)
        {
            return Request(dict["XML_ALL"] + "?page=p" + page);
        }
        private string getVoicemail(int page)
        {
            return Request(dict["XML_VOICEMAIL"] + "?page=p" + page);
        }

        public Folder All
        {
            get
            {
                return this.GetFolder(FolderType.All);
            }
        }

        public Folder Inbox
        {
            get
            {
                return this.GetFolder(FolderType.Inbox);
            }
        }

        public Folder SMS
        {
            get
            {
                return this.GetFolder(FolderType.SMS);
            }
        }

        public Folder Voicemail
        {
            get
            {
                return this.GetFolder(FolderType.Voicemail);
            }
        }

        private Folder buildFolder(FolderType t)
        {
            Folder returnFolder = null;
            buildFolder(t, ref returnFolder);
            return returnFolder;
        }

        /// <summary>
        /// Iterate through pages of xml to pull all messages for folder
        /// </summary>
        /// <param name="t">Folder type (All, Inbox, SMS, Unread, or Voicemail)</param>
        /// <returns>Folder for specified type</returns>
        private void buildFolder(FolderType t, ref Folder returnFolder)
        {
            int page = 0;
            bool seen = false;
            Folder tmp = null;
            Folder staging = returnFolder;
            do
            {
                page++;
                string xml = "";
                switch (t)
                {
                    case FolderType.All:
                        xml = this.getAll(page);
                        break;
                    case FolderType.Inbox:
                        xml = this.getInbox(page);
                        break;
                    case FolderType.SMS:
                        xml = this.getSMS(page);
                        break;
                    case FolderType.Unread:
                        throw new NotImplementedException();
                        break;
                    case FolderType.Voicemail:
                        xml = this.getVoicemail(page);
                        break;
                    default:
                        xml = this.getInbox(page);
                        break;
                }
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xml);
                string json = xd.SelectSingleNode("//response/json").InnerText;
                tmp = JsonConvert.DeserializeObject<Folder>(json);

                if (page == 1 && returnFolder == null)
                {
                    staging = tmp;
                }
                else
                {
                    if (returnFolder != null)
                    {
                        staging.ResultsPerPage = tmp.ResultsPerPage;
                        staging.UnreadCounts = tmp.UnreadCounts;
                    }
                    foreach (Message m in tmp.Messages)
                    {
                        if (returnFolder != null && returnFolder[m.ID] != null)
                        {
                            seen = true;
                            break;
                        }
                        staging[m.ID] = m;
                    }
                }
            } while (tmp.Messages.Length >= staging.ResultsPerPage && !seen && page < MaxPages);

            staging.LastUpdate = DateTime.Now;
            staging.voiceConnection = this;
            staging.Type = t;
            foreach (Message m in staging.Messages)
                m.connection = this;
            returnFolder = staging;
        }

        List<Folder> FolderCollection = new List<Folder>();
        Folder GetFolder(FolderType t)
        {
            Folder folder = FolderCollection.Find(f => f.Type == t);

            if (folder != null)
            {
                Debug.WriteLine("Get existing folder");
                if (folder.NeedsUpdate)
                {
                    int folderIndex = FolderCollection.IndexOf(folder);
                    buildFolder(t, ref folder);
                    FolderCollection[folderIndex] = folder;
                }
            }
            else
            {
                Debug.WriteLine("Get new folder");
                folder = buildFolder(t);
                FolderCollection.Add(folder);
            }
            return folder;
        }

		#endregion

        private static Dictionary<string, string> get_post_vars()
        {
            return get_post_vars(null);
        }

		private static Dictionary<string,string> get_post_vars(ICollection<string> names)
        {
            Dictionary<string,string> Post_Vars = new Dictionary<string,string>(){};
            string response = Request("base");
            response = response.Replace("\n", "");

            MatchCollection vars = Regex.Matches(response, "name=['\"](.*?)['\"].*?(value=['\"](.*?)['\"]|>)", RegexOptions.Multiline);
            int num_matches = vars.Count;
            string name = "";
            string value = "";
            for (int i = 0; i < num_matches; i++)
            {
                //clear
                name = "";
                value = "";

                //set
                name = vars[i].Groups[1].ToString().Trim();
                value = vars[i].Groups[3].ToString().Trim();

                /*/trim trailing " or '
                name = name.Remove(name.Length - 1, 1);
                value = value.Remove(value.Length - 1, 1);
                */
                if (!Post_Vars.ContainsKey(name) && (names == null || names.Contains(name)))
                    Post_Vars.Add(name, value);
            }

            return Post_Vars;
        }
		
        /// <summary>
        /// Login with email address and password.  Retrieve the _rnr_se key.
        /// </summary>
        public void Login(string pass, string smsPin, bool persist)
        {
            Debug.WriteLine("get:Login");
            if (user == null)
                throw new InvalidOperationException("No email defined");
            if (pass == null)
                throw new InvalidOperationException("No password defined");

            Dictionary<string, string> loginData = get_post_vars(new string[]{"GALX","_rnr_se"});

            if(persist && loginData.ContainsKey("_rnr_se")){
                rnrSEE = loginData["_rnr_se"];
                return;
            }
            else if (loginData.ContainsKey("_rnr_se") && !persist)
            {
                Logout();
                loginData = get_post_vars();
            }

            if (loginData.ContainsKey("Email"))
                loginData["Email"] = user;
            else
                loginData.Add("Email", user);

            if (loginData.ContainsKey("Passwd"))
                loginData["Passwd"] = pass;
            else
                loginData.Add("Passwd", pass);

            string loginResponse = Request("rnrse", loginData);

            if (Regex.Match(loginResponse, "smsUserPin").Success)
            {
                Dictionary<string, string> smsAuthData = new Dictionary<string, string>() { 
                    {"smsUserPin", smsPin },
                    {"smsVerifyPin","Verify"},
                    {"exp","smsauthnojs"},
                    {"PersistantCookie",persist?"yes":"no"}
                };

                string smsResponse = Request("smsauth",smsAuthData);
                string smsToken = Regex.Match(smsResponse, "name=\"smsToken\"[ ]+value=\"([^\"]+)\"").Groups[1].Value;
                Debug.WriteLine("smsToken:" + smsToken);
                loginData = get_post_vars(new string[] { "GALX", "smsToken"});
                loginData.Add("smsToken", smsToken);
                loginResponse = Request("rnrse", loginData);
            }

            rnrSEE = get_rnrse(loginResponse);

            if (string.IsNullOrEmpty(rnrSEE))
            {
                throw new Exception("Could not login");
            }
        }

        /// <summary>
        /// Deauth oneself with this method.
        /// </summary>
		public void Logout()
		{
            Debug.WriteLine("get:Logout");
			Request("logout");
			rnrSEE = null;

		}
		
		private string get_rnrse(string t)
        {
            t = t.Replace("\n", "");
            Match m = Regex.Match(t, "<input.*?name=[\'\"]_rnr_se[\'\"].*?(value=[\'\"](.*?)[\'\"]|/>)");
            if (m.Success)
                return m.Groups[2].ToString().Trim();
            return "";
        }

        public void SmsUserPin(string pin)
        {

        }

        /// <summary>
        /// Initiate call.
        /// </summary>
        /// <param name="callTo">Phone number to call</param>
        /// <param name="callFrom">Phone you are calling from or your email (for gtalk)</param>
        /// <returns>json string</returns>
        public string Call(string callTo, string callFrom){
            return Call(callTo, callFrom, "undefined");
        }

        public string Call(string callTo, string callFrom, PhoneType phoneType)
        {
            return Call(callTo, callFrom, "undefined", phoneType);
        }

        public string Call(string callTo, string callFrom, string subscriberNumber){
            PhoneType phoneType = PhoneType.mobile; // default
            if (callFrom == this.user) // call from google talk
                phoneType = PhoneType.gtalk;

            return Call(callTo, callFrom, subscriberNumber, phoneType);
	    }

        public string Call(string callTo, string callFrom, string subscriberNumber, PhoneType phoneType)
        {
            Dictionary<string, string> callData = new Dictionary<string, string>(){
                {"outgoingNumber",callTo},
                {"forwardingNumber",callFrom},
                {"subscriberNumber",subscriberNumber},
                {"remember","0"},
                {"phoneType",((int)phoneType).ToString()}
            };

            return Request("call", callData);
        }

        public string SendSMS(String destinationNumber, String txt){
		    
            Dictionary<string, string> smsData = new Dictionary<string, string>(){
                //{"auth",authToken},
                {"phoneNumber",destinationNumber},
                {"text",txt}
            };
			
            return Request("sms", smsData);
	    }

        public static string MarkRead(string msgID, bool read)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("messages", msgID);
            data.Add("read", read ? "1" : "0");
            return Request("mark", data);
        }

        public static string Archive(string msgID, bool archive)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("messages", msgID);
            data.Add("archive", archive ? "1" : "0");
            return Request("archive", data);
        }

        public static string Delete(string msgID, bool trash)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("messages", msgID);
            data.Add("trash", trash ? "1" : "0");
            return Request("delete", data);
        }

        private static object makeRequest(string page, object data)
        {
            Debug.WriteLine("request:" + page);
            
            string url = "";
            if (dict.ContainsKey(page.ToUpper()))
                url = dict[page.ToUpper()];
            else
                url = page;
            
            string dataString = "";
            
            if(data is string){
	            if(page.ToUpper() == "DOWNLOAD")
    	            url += data;
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = @"sharpVoice / 0.1";
                request.CookieContainer = cookiejar;
                
                request.Method = "GET";
                
                if (data != null)
                {
                    if (data is Dictionary<string, string>)
                    {
                        Dictionary<string, string> dicdata = data as Dictionary<string, string>;
                        
                        if(!string.IsNullOrEmpty(rnrSEE))
                        	dicdata.Add("_rnr_se", rnrSEE);
                        
                        foreach (KeyValuePair<string, string> h in dicdata)
                        {
                            dataString += h.Key + "=" + HttpUtility.UrlEncode(h.Value, Encoding.UTF8);
                            dataString += "&";
                        }
                        dataString = dataString.TrimEnd(new char[] { '&' });
                        request.Method = "POST";
                        
                        request.ContentLength = dataString.Length;
                    }
                    request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                }
                
                if (request.ContentLength > 0)
                {
                    using (Stream writeStream = request.GetRequestStream())
                    {
                        UTF8Encoding encoding = new UTF8Encoding();
                        byte[] bytes = encoding.GetBytes(dataString);
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }

                using (WebResponse response = request.GetResponse())
                {
                    if (request.CookieContainer != null)
                    {
                        cookiejar = request.CookieContainer;
                    }

                    Stream s = response.GetResponseStream();

                    switch(page.ToUpper()){
                        default:
                            using (StreamReader reader = new StreamReader(s, Encoding.UTF8))
                                return reader.ReadToEnd();
                        case "DOWNLOAD":
                            MemoryStream m = new MemoryStream();
                            byte[] buffer = new byte[1024];
                            int bytesSize = 0;
                            while ((bytesSize = s.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                m.Write(buffer, 0, bytesSize);
                            }
                            return m.ToArray();
                        case "LOGOUT":
                            return "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public static string Request(string page){
        	return Request(page, null);
        }
        
        public static string Request(string page, object data){
        	return (string)makeRequest(page,data);
        }

        public static byte[] Download(string page, object data){
            return (byte[])makeRequest(page,data);
        }
        
		public static string SaveVoicemail(string voiceID, string location){
			File.WriteAllBytes(location,Download("download", voiceID));
            return location;
		}


        #region IDisposable Members

        public void Dispose()
        {
            this.Logout();
        }

        #endregion
    }
}