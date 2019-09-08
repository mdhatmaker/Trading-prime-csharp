using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace CryptoAPIs
{
    public interface IExchangeWebSocket
    {
        void WebSocketMessageHandler(MessageArgs e);
        void StartWebSocket(string[] args);
        void SubscribeWebSocket(string[] args);
    } // end of interface

} // end of namespace
