//-----------------------------------------------------------------------
// <copyright file="ProwlClient.cs" company="Andrew Beaton">
//     Copyright (c) Andrew Beaton. All rights reserved. 
// </copyright>
//-----------------------------------------------------------------------
namespace SendToProwl
{
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web; 

    public class ProwlClient
    {
        private static string prowlApiUrl = "https://api.prowlapp.com/publicapi/";

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
}
