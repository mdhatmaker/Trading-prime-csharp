using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web; 

namespace Tools.Messaging.Prowl
{
    public class ProwlClient : IMessenger
    {
        private static string prowlApiUrl = "https://api.prowlapp.com/publicapi/";

        private string m_apiKey;
        private string m_appName;
        private string m_eventName;

        public ProwlClient(string apikey, string appName, string eventName)
        {
            m_apiKey = apikey;
            m_appName = appName;
            m_eventName = eventName;
        }

        /*var notification = new ProwlNotification()
        {
            ApiKey = "my_api_key",
            Application = "Send To Prowl",
            Event = "Test Notification",
            Description = "Just testing notifications",
            Priority = 1
        };

        var client = new ProwlClient();
        client.SendNotification(notification);*/

        public void Send(string msg)
        {
            var notification = new ProwlNotification()
            {
                ApiKey = m_apiKey,
                Application = m_appName,
                Event = m_eventName,
                Description = msg,
                Priority = 1
            };

            SendNotification(notification);
        }

        public void SendNotification(ProwlNotification notification)
        {
            notification.Validate();

            var notificationRequestUriString = this.CreateNotificationAddRequestUriString(notification);

            var notificationRequest = HttpWebRequest.Create(notificationRequestUriString);
            notificationRequest.ContentLength = 0;
            notificationRequest.ContentType = "application/x-www-form-urlencoded";
            notificationRequest.Method = "POST";

            try
            {
                using (WebResponse response = notificationRequest.GetResponse())
                {
                    var httpResponse = (HttpWebResponse)response;
                    if (httpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        throw new WebException(httpResponse.StatusDescription);
                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    throw new InvalidDataException(string.Format("An error occurred whilst sending the notification to Prowl: {0}", httpResponse.StatusDescription));
                }
            }
        }

        private string CreateNotificationAddRequestUriString(ProwlNotification notification)
        {
            var notificationAddRequestFormat = "add?apikey={0}&application={1}&description={2}&event={3}&priority={4}";

            var addCommand = new StringBuilder();
            addCommand.Append(prowlApiUrl);

            addCommand.AppendFormat(
                notificationAddRequestFormat,
                HttpUtility.UrlEncode(notification.ApiKey),
                HttpUtility.UrlEncode(notification.Application),
                HttpUtility.UrlEncode(notification.Description),
                HttpUtility.UrlEncode(notification.Event),
                notification.Priority);

            return addCommand.ToString();
        }
    }
} // end of namespace
