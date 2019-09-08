using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tools.Messaging;

namespace PriceServer
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n*** WELCOME TO PRICE SERVER ***");

            if (args[0] == "1")
                ZmqProgram.WUClient(new string[] { "72622", "tcp://localhost:5556" });
            else if (args[0] == "2")
                ZmqProgram.WUServer(new string[] { });

            return;

            string address = "tcp://127.0.0.1";
            int port = 5556;

            // Fire up Price Server...
            Task.Run(() =>
            {
                //var ps = new PriceServerConsole(address, port);
                var ps = new PriceServerConsole("tcp://*", port);
                ps.Spin();
            });

            // Create a PricesConsole to listen for price updates
            Task.Run(() =>
            {
                //var pc = new PricesConsole("10.0.0.8", 6379, false);
                var pc = new PricesConsole(address, port, false);
                pc.Spin();
            });

            Console.WriteLine("(back in MAIN)");
            for (;;)
            {
                Thread.Sleep(60000);
            }
        }

        /*/// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PricesForm());
        }*/
    }
}
