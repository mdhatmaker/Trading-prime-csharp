using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Messaging
{
    //public delegate void SubscriberReceiveHandler(string sMessage);    // delegate for receiving Subscription updates        

    public interface ISubscriber
    {
        event SubscriberReceiveHandler OnSubscriberReceive;
        void StartSubscriber(params object[] args);
    }
} // end of namespace
