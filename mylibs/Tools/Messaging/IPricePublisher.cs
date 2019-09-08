using System;
using Tools.Messaging;

namespace Tools.Messaging
{
    public interface IPricePublisher
    {       
        void StartPricePublisher(string address, int port);
        void RequestPriceUpdates(string symbol);
    }

} // end of namespace
