using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools.IQFeed;
using ZeroMQ;
//using NetMQ;
//using NetMQ.Sockets;
using static Tools.G;

namespace Tools.Messaging
{
    public class ZmqIQFeed : IPricePublisher, IPriceSubscriber
    {
        public event SubscriberReceiveHandler OnSubscriberReceive;        // event for receiving Subscription updates

        private static readonly string IQ_CHANNEL = "IQ";

        static PriceFeedIQ m_priceFeed = PriceFeedIQ.Instance;

        // Prepare our context, publisher and subscriber
        ZContext m_pubContext = new ZContext();
        ZContext m_subContext = new ZContext();
        ZSocket m_publisherSocket;
        ZSocket m_subscriberSocket;

        //PublisherSocket m_publisherSocket;            // NetMQ (when testing in place of ZeroMQ)

        string m_pub_address;
        string m_sub_address;

        private ConcurrentDictionary<string, long> m_publishCounts = new ConcurrentDictionary<string, long>();
        private ConcurrentBag<string> m_activePriceUpdateSymbols = new ConcurrentBag<string>();

        //
        // Price Update Server
        // Binds PUB socket to given address (SubscribePriceUpdates connects SUB to same address)
        // Publishes IQFeed Price updates
        //
        public ZmqIQFeed()
        {
            m_publisherSocket = new ZSocket(m_pubContext, ZSocketType.PUB);
            m_subscriberSocket = new ZSocket(m_subContext, ZSocketType.SUB);

            //foreach (IPAddress localAddress in WUProxy_GetPublicIPs())
            //{
            //    var epgmAddress = string.Format("epgm://{0};239.192.1.1:8100", localAddress);
            //    Console.WriteLine("I: Publisher.Bind'ing on {0}…", epgmAddress);
            //    publisher.Bind(epgmAddress);
            //}
        }

        public void StartPriceSubscriber(string address = "tcp://127.0.0.1", int port = 5556)
        {
            m_sub_address = string.Format("{0}:{1}", address, port);
            dout("ZmqPubSub::StartPriceSubscriber> Connecting to {0}…", m_sub_address);
            m_subscriberSocket.Connect(m_sub_address);
            m_subscriberSocket.Subscribe(IQ_CHANNEL);

            int i = 0;
            for (; i < 20; ++i)
            {
                using (var replyFrame = m_subscriberSocket.ReceiveFrame())
                {
                    string reply = replyFrame.ReadString();
                    Console.WriteLine("REPLY: " + reply);
                    //total_temperature += Convert.ToInt64(reply.Split(' ')[1]);
                }
            }
            //Task.Run(() => SubscriptionLoop());           
        }

        public void SubscriptionLoop(int threadSleepMilliseconds = 50)
        {
            while (true)
            {
                // ZeroMQ
                using (var replyFrame = m_subscriberSocket.ReceiveFrame())
                {
                    string reply = replyFrame.ReadString();
                    //Console.WriteLine(reply);
                    OnSubscriberReceive?.Invoke(reply);
                }

                //Msg replyMsg = new Msg();                           // NetMQ
                //m_subscriber.Receive(ref replyMsg);
                //string reply = replyMsg.ToString();
                ////string reply = replyFrame.ReadString();
                ////Console.WriteLine(reply);
                //UpdateSubscriberReceive?.Invoke(reply);

                Thread.Sleep(threadSleepMilliseconds);
            }
        }

        /*private void M_ZmqPubSub_OnSubscriberReceive(string sMessage)
        {
            Console.WriteLine("OnSubscriberReceive->" + sMessage);
            OnSubscriberReceive?.Invoke(sMessage);
        }*/

        public void StartPricePublisher(string address = "tcp://*", int port = 5556)   // "tcp://*:5556");
        {
            m_pub_address = string.Format("{0}:{1}", address, port);
            dout("ZmqPubSub::Ctor> Publisher.Binding on {0}…", m_pub_address);
            m_publisherSocket.Bind(m_pub_address);

            Task.Run(() => DisplayPublishCounts());
        }

        public void RequestPriceUpdates(string symbol)    //, PriceFeed.PriceUpdateHandler updateHandler)
        {
            dout("ZmqPubSub::RequestPriceUpdates({0})", symbol);
            m_priceFeed.SubscribePrices(symbol);
            m_priceFeed.UpdatePrices += M_priceFeed_OnPriceUpdate;
            cout("\nZmqPubSub> Subscribed to IQFeed symbol: {0}", symbol);
        }

        private void M_priceFeed_OnPriceUpdate(Tools.IQFeed.PriceUpdateIQ update)
        {
            //var sUpdate = string.Format("{0},{1},{2},{3},{4},{5}", update.Symbol, update.LastTradePrice, update.Bid, update.BidSize, update.Ask, update.AskSize);
            // Prepend the CHANNEL (+space) to the string we will send:
            var sUpdate = string.Format("{0} {1}", IQ_CHANNEL, update.ToString());
            using (var updateFrame = new ZFrame(sUpdate))
            {
                //Console.WriteLine("OnPriceUpdate->" + sUpdate.ToString());
                m_publisherSocket.Send(updateFrame);
            }
            long count;
            if (m_publishCounts.TryGetValue(update.Symbol, out count))
                m_publishCounts[update.Symbol] = ++count;
            else
                m_publishCounts[update.Symbol] = 1;

            //m_publisherSocket.SendFrame(sUpdate);         // NetMQ (when testing in place of ZeroMQ)
        }



        // Launch this method as a thread that will display the publish counts every 30 seconds
        private void DisplayPublishCounts(int sleepSeconds = 30)
        {
            for (; ; )
            {
                //dout("\nconnected:{0} status:{1}", m_redis.IsConnected, m_redis.GetStatus());

                StringBuilder sb = new StringBuilder();
                if (m_publishCounts.Count > 0)
                {
                    sb.Append(string.Format("\n[{0}] PUB >>  ", DateTime.Now.ToShortTimeString()));
                    foreach (var k in m_publishCounts.Keys)
                    {
                        sb.Append(string.Format("{0}:{1} ", k, m_publishCounts[k]));
                    }
                    dout(sb.ToString());
                }

                Thread.Sleep(sleepSeconds * 1000);
            }
        }

    } // end of class



    public static partial class ZmqProgram
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

    public static partial class ZmqProgram
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


} // end of namespace