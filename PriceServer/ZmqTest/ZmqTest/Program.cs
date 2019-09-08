using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Messaging;

namespace ZmqTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n*** WELCOME TO 0MQ TEST ***");

            if (args[0] == "1")
                ZmqProgram.WUClient(new string[] { "72622", "tcp://localhost:5556" });
            else if (args[0] == "2")
                ZmqProgram.WUServer(new string[] { });

        }
    }
}
