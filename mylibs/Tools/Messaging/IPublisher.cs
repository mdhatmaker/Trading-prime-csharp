using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Messaging
{
    public interface IPublisher
    {
        void StartPublisher(params object[] args);
        void RequestUpdates(string channel);
    } 
} // end of namespace
