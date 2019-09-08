using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Messaging
{
    public delegate void SubscriberReceiveHandler(string sMessage);    // delegate for receiving Subscription updates        

    public interface IPriceSubscriber
    {
        event SubscriberReceiveHandler OnSubscriberReceive;
        void StartPriceSubscriber(string address, int port);
    }

} // end of namespace
