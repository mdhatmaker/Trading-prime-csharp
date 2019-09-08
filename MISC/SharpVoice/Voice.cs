using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Diagnostics;


namespace GoogleTests
{
    class Voice
    {   
        String rnrSEE = null;
	    String source = null;
	    String user = null;
	    String pass = null;
	    String authToken = null;
        //const String baseURL = "https://www.google.com/voice/inbox/recent/";
	    String inboxURLString = "https://www.google.com/voice/inbox/recent/inbox/";
	    String starredURLString = "https://www.google.com/voice/inbox/recent/starred/";
	    String recentAllURLString = "https://www.google.com/voice/inbox/recent/all/";
	    String spamURLString = "https://www.google.com/voice/inbox/recent/spam/";
	    String trashURLString = "https://www.google.com/voice/inbox/recent/spam/";
	    String voicemailURLString = "https://www.google.com/voice/inbox/recent/voicemail/";
	    String smsURLString = "https://www.google.com/voice/inbox/recent/sms/";
	    String recordedURLString = "https://www.google.com/voice/inbox/recent/recorded/";
	    String placedURLString = "https://www.google.com/voice/inbox/recent/placed/";
	    String receivedURLString = "https://www.google.com/voice/inbox/recent/received/";
	    String missedURLString = "https://www.google.com/voice/inbox/recent/missed/";
        
        public Voice(String user, String pass, String source, String rnrSee){

		    this.user = user;
		    this.pass = pass;
		    this.rnrSEE = rnrSee;
		    this.source = source;

		    login();
	    }
        
        public String getInbox(){
		    return get(inboxURLString);
	    }
	    public String getStarred(){
		    return get(starredURLString);
	    }
	    public String getRecent(){
		    return get(recentAllURLString);
	    }
	    public String getSpam(){
		    return get(spamURLString);
	    }
	    public String getRecorded(){
		    return get(recordedURLString);
	    }
	    public String getPlaced(){
		    return get(placedURLString);
	    }
	    public String getReceived(){
		    return get(receivedURLString);
	    }
	    public String getMissed(){
		    return get(missedURLString);
	    }
	    public String getSMS(){
		    return get(smsURLString);
	    }
        
        private String get(String urlString){
		    string result = null;

            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString + "?auth=" + authToken);
                request.Method = "GET";
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                throw new IOException(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }
		    return result;
	    }

        public void login(){

            String data = HttpUtility.UrlEncode("accountType", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode("GOOGLE", Encoding.UTF8);
		    data += "&" + HttpUtility.UrlEncode("Email", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode(user, Encoding.UTF8);
		    data += "&" + HttpUtility.UrlEncode("Passwd", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode(pass, Encoding.UTF8);
		    data += "&" + HttpUtility.UrlEncode("service", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode("grandcentral", Encoding.UTF8);
		    data += "&" + HttpUtility.UrlEncode("source", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode(source, Encoding.UTF8);

            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com/accounts/ClientLogin");
                
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (Stream writeStream = request.GetRequestStream())
                {
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] bytes = encoding.GetBytes(data);
                    writeStream.Write(bytes, 0, bytes.Length);
                }
                response = request.GetResponse();
                
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                
                String line;
                string[] stringSeparators = new string[] { "=" };
                while ((line = reader.ReadLine()) != null)
                {
                    // System.out.println(line);
                    Debug.WriteLine(line);
                    if (line.Contains("Auth="))
                    {
                        this.authToken = line.Split(stringSeparators,2,StringSplitOptions.None)[1].Trim();
                        Console.WriteLine("AUTH TOKEN =" + this.authToken);
                    }
                    else if (line.Contains("Error="))
                    {
                        throw new IOException(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                throw new IOException(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }
            
		    if (this.authToken == null) {
			    throw new IOException("No Authorization Received.");
		    }
	    }

        public String call(String originNumber, String destinationNumber){
		    String calldata = HttpUtility.UrlEncode("auth", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode(authToken, Encoding.UTF8);
		    calldata += "&" + HttpUtility.UrlEncode("outgoingNumber", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode(destinationNumber, Encoding.UTF8);
		    calldata += "&" + HttpUtility.UrlEncode("forwardingNumber", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode(originNumber, Encoding.UTF8);
		    calldata += "&" + HttpUtility.UrlEncode("subscriberNumber", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode("undefined", Encoding.UTF8);
		    calldata += "&" + HttpUtility.UrlEncode("remember", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode("0", Encoding.UTF8);
		    calldata += "&" + HttpUtility.UrlEncode("_rnr_se", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode(rnrSEE, Encoding.UTF8);
		    // POST /voice/call/connect/ outgoingNumber=[number to
		    // call]&forwardingNumber=[forwarding
		    // number]&subscriberNumber=undefined&remember=0&_rnr_se=[pull from
		    // page]

            return this.makeRequest("https://www.google.com/voice/call/connect/", calldata);
	    }

        public String sendSMS(String destinationNumber, String txt){
		    String smsdata = HttpUtility.UrlEncode("auth", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode(authToken, Encoding.UTF8);

		    smsdata += "&" + HttpUtility.UrlEncode("phoneNumber", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode(destinationNumber, Encoding.UTF8);
		    smsdata += "&"
				    + HttpUtility.UrlEncode("text", Encoding.UTF8)
				    + "="
                    + HttpUtility.UrlEncode(txt, Encoding.UTF8);
		    smsdata += "&" + HttpUtility.UrlEncode("_rnr_se", Encoding.UTF8) + "="
				    + HttpUtility.UrlEncode(rnrSEE, Encoding.UTF8);

            return this.makeRequest("https://www.google.com/voice/sms/send/", smsdata);
	    }

        private String makeRequest(String url, String data)
        {
            string result = null;

            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (Stream writeStream = request.GetRequestStream())
                {
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] bytes = encoding.GetBytes(data);
                    writeStream.Write(bytes, 0, bytes.Length);
                }
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                throw new IOException(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }

            if (result.Equals(""))
            {
                throw new IOException("No Response Data Received.");
            }

            return result;
        }
    }
}