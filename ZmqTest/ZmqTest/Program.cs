using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace ZmqTest
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "1")
                WUClient(new string[] { "72622", "tcp://127.0.0.1:5556" });
            else if (args[0] == "2")
                WUServer(new string[] { });
        }
    }

    partial class Program
    {
        public static void WUClient(string[] args)
        {
            //
            // Weather update client
            // Connects SUB socket to tcp://127.0.0.1:5556
            // Collects weather updates and finds avg temp in zipcode
            //
            // Author: metadings
            //
            if (args == null || args.Length < 2)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: ./{0} WUClient [ZipCode] [Endpoint]", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine();
                Console.WriteLine("    ZipCode   The zip code to subscribe. Default is 72622 Nürtingen");
                Console.WriteLine("    Endpoint  Where WUClient should connect to.");
                Console.WriteLine("              Default is tcp://127.0.0.1:5556");
                Console.WriteLine();
                if (args.Length < 1)
                    args = new string[] { "72622", "tcp://127.0.0.1:5556" };
                else
                    args = new string[] { args[0], "tcp://127.0.0.1:5556" };
            }
            string endpoint = args[1];
            // Socket to talk to server
            using (var context = new ZContext())
            using (var subscriber = new ZSocket(context, ZSocketType.SUB))
            {
                string connect_to = args[1];
                Console.WriteLine("I: Connecting to {0}…", connect_to);
                subscriber.Connect(connect_to);
                /* foreach (IPAddress address in WUProxy_GetPublicIPs())
                    {
                        var epgmAddress = string.Format("epgm://{0};239.192.1.1:8100", address);
                        Console.WriteLine("I: Connecting to {0}…", epgmAddress);
                        subscriber.Connect(epgmAddress);
                    }
                } */
                // Subscribe to zipcode
                string zipCode = args[0];
                Console.WriteLine("I: Subscribing to zip code {0}…", zipCode);
                subscriber.Subscribe(zipCode);
                // Process 10 updates
                int i = 0;
                long total_temperature = 0;
                for (; i < 20; ++i)
                {
                    using (var replyFrame = subscriber.ReceiveFrame())
                    {
                        string reply = replyFrame.ReadString();
                        Console.WriteLine(reply);
                        total_temperature += Convert.ToInt64(reply.Split(' ')[1]);
                    }
                }
                Console.WriteLine("Average temperature for zipcode '{0}' was {1}°", zipCode, (total_temperature / i));
            }
        }
    }

    partial class Program
    {
        public static void WUServer(string[] args)
        {
            //
            // Weather update server
            // Binds PUB socket to tcp://*:5556
            // Publishes random weather updates
            //
            // Author: metadings
            //
            // Prepare our context and publisher
            using (var context = new ZContext())
            using (var publisher = new ZSocket(context, ZSocketType.PUB))
            {
                string address = "tcp://*:5556";
                Console.WriteLine("I: Publisher.Bind'ing on {0}", address);
                publisher.Bind(address);
                /* foreach (IPAddress localAddress in WUProxy_GetPublicIPs())
                {
                    var epgmAddress = string.Format("epgm://{0};239.192.1.1:8100", localAddress);
                    Console.WriteLine("I: Publisher.Bind'ing on {0}…", epgmAddress);
                    publisher.Bind(epgmAddress);
                } */
                // Initialize random number generator
                var rnd = new Random();
                while (true)
                {
                    // Get values that will fool the boss
                    int zipcode = rnd.Next(99999);
                    int temperature = rnd.Next(-55, +45);
                    // Send message to all subscribers
                    var update = string.Format("{0:D5} {1}", zipcode, temperature);
                    using (var updateFrame = new ZFrame(update))
                    {
                        publisher.Send(updateFrame);
                    }
                }
            }
        }
    }
}
