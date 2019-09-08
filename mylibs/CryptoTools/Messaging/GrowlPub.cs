using System;
using System.Collections.Generic;
using System.Text;
using Growl.Connector;
using System.Security.Cryptography;

namespace CryptoTools.Messaging
{
    public class GrowlPub
    {
        private GrowlConnector growl;
        private NotificationType notificationType;
        private Growl.Connector.Application application;
        private string m_notificationType;                  // = "SAMPLE_NOTIFICATION";
        private string m_notificationDisplayName;           // "Sample Notification"

        // where notificationType like "SAMPLE_NOTIFICATION" and notificationDisplayName like "Sample Notification"
        public GrowlPub(string notificationType, string notificationDisplayName)
        {
            m_notificationType = notificationType;
            m_notificationDisplayName = notificationDisplayName;
            Initialize();
        }

        private void Initialize()
        {
            this.notificationType = new NotificationType(m_notificationType, m_notificationDisplayName);

            this.growl = new GrowlConnector();
            //this.growl = new GrowlConnector("password");    // use this if you need to set a password - you can also pass null or an empty string to this constructor to use no password
            //this.growl = new GrowlConnector("password", "hostname", GrowlConnector.TCP_PORT);   // use this if you want to connect to a remote Growl instance on another machine

            this.growl.NotificationCallback += new GrowlConnector.CallbackEventHandler(growl_NotificationCallback);

            // set this so messages are sent in plain text (easier for debugging)
            this.growl.EncryptionAlgorithm = Growl.Connector.Cryptography.SymmetricAlgorithmType.PlainText;
            
        }

        // where growlAppName like "Sample App", "CryptoTrader", "blinkie", ...
        public void RegisterApplication(string growlAppName)
        {
            this.application = new Growl.Connector.Application(growlAppName);
            this.growl.Register(application, new NotificationType[] { notificationType });
        }

        public void SendNotification(string notificationTitle, string notificationText)
        {
            CallbackContext callbackContext = new CallbackContext("some fake information", "fake data");

            Notification notification = new Notification(this.application.Name, this.notificationType.Name, DateTime.Now.Ticks.ToString(), notificationTitle, notificationText);
            this.growl.Notify(notification, callbackContext);
        }

        void growl_NotificationCallback(Response response, CallbackData callbackData, object state)
        {
            string text = String.Format("Response Type: {0}\r\nNotification ID: {1}\r\nCallback Data: {2}\r\nCallback Data Type: {3}\r\n", callbackData.Result, callbackData.NotificationID, callbackData.Data, callbackData.Type);
            Console.WriteLine("Callback received: {0}", text);
        }
    }
}
