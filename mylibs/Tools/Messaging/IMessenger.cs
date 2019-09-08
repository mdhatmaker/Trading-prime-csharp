using System;

namespace Tools.Messaging
{
    public interface IMessenger
    {
        void Send(string msg);
    }
}
