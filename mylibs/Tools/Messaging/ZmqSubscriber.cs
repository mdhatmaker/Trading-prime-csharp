using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using ZeroMQ;
//using NetMQ;
//using NetMQ.Sockets;

namespace Tools.Messaging
{
    public delegate void SubscriberReceive(string sUpdate);

    public class ZMQSubscriber
    {
        public event SubscriberReceive UpdateSubscriberReceive;

        ZContext m_context;
        ZSocket m_subscriber;
        //SubscriberSocket m_subscriber;

        //public void WUClient(string[] args)
        public ZMQSubscriber(string[] args = null)
        {
            //string[] args = null;

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
                if (args == null || args.Length < 1)
                    args = new string[] { "72622", "tcp://127.0.0.1:5556" };
                else
                    args = new string[] { args[0], "tcp://127.0.0.1:5556" };
            }

            string endpoint = args[1];

            // Socket to talk to server
            //using (var m_context = new ZContext())
            //using (m_subscriber = new ZSocket(m_context, ZSocketType.SUB))
            //{

            m_context = new ZContext();
            m_subscriber = new ZSocket(m_context, ZSocketType.SUB);

            string connect_to = args[1];
            Console.WriteLine("I: Connecting to {0}…", connect_to);
            m_subscriber.Connect(connect_to);

            /* foreach (IPAddress address in WUProxy_GetPublicIPs())
                {
                    var epgmAddress = string.Format("epgm://{0};239.192.1.1:8100", address);
                    Console.WriteLine("I: Connecting to {0}…", epgmAddress);
                    subscriber.Connect(epgmAddress);
                }
            } */

            /*string symbol = "@VIX.XO";
            subscriber.Subscribe(symbol);

            while (true)
            {
                using (var replyFrame = subscriber.ReceiveFrame())
                {
                    string reply = replyFrame.ReadString();
                    Console.WriteLine(reply);
                }
            }*/

            /*// Subscribe to zipcode
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
            Console.WriteLine("Average temperature for zipcode '{0}' was {1}°", zipCode, (total_temperature / i));*/
            //}
        }

        // symbol like "@ES#", "VIX.XO", "@VXZ18", ...
        public void Subscribe(string symbol, SubscriberReceive subReceive)
        {
            UpdateSubscriberReceive += subReceive;
            m_subscriber.Subscribe(symbol);
        }

        public void SubscriptionLoop()
        {
            while (true)
            {
                // ZeroMQ
                using (var replyFrame = m_subscriber.ReceiveFrame())
                {
                    string reply = replyFrame.ReadString();
                    //Console.WriteLine(reply);
                    UpdateSubscriberReceive?.Invoke(reply);
                }

                /*// NetMQ
                Msg replyMsg = new Msg();
                m_subscriber.Receive(ref replyMsg);
                string reply = replyMsg.ToString();
                //string reply = replyFrame.ReadString();
                //Console.WriteLine(reply);
                UpdateSubscriberReceive?.Invoke(reply);*/

                Thread.Sleep(50);
            }
        }

    } // end of class
} // end of namespace