using System;
using System.Collections.Generic;
using System.Text;
using Prowl;

namespace CryptoTools.Messaging
{
    public class ProwlPub
    {
        private ProwlClient m_prowl;

        public ProwlPub(string apiKey, string appName = "(general)")
        {
            var configuration = new ProwlClientConfiguration();
            // Provider keys are used by developers to allow applications to connect with Prowl with an
            // increased request limit and allow for retrieving new API keys from user's accounts.
            //configuration.ProviderKey = apiKey;
            configuration.ApiKeychain = apiKey;
            configuration.ApplicationName = appName;
            m_prowl = new ProwlClient(configuration);
        }

        public void Send(string myevent, string mydesc, ProwlNotificationPriority priority = ProwlNotificationPriority.Normal)
        {
            var notification = new ProwlNotification();
            notification.Event = myevent;
            notification.Description = mydesc;
            notification.Priority = priority;
            m_prowl.PostNotification(notification);
        }

    } // end of class ProwlPub

} // end of namespace
