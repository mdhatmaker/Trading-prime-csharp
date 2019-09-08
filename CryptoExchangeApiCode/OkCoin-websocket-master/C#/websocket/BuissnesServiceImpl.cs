using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace websocket
{
    class BuissnesServiceImpl:WebSocketService
    {
        public void onReceive(string msg) {
            Console.WriteLine(msg);
        }
    }
}
