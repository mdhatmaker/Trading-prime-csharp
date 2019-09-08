using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tools.IQFeed;
using StackExchange.Redis;
using System.Text;
using static Tools.G;

namespace Tools.Messaging
{
    public class RedisIQFeed : IPricePublisher
    {
        public event SubscriberReceiveHandler OnSubscriberReceive;        // event for receiving Subscription updates

        private static readonly string IQ_CHANNEL = "IQ";

        private static PriceFeedIQ m_priceFeed = PriceFeedIQ.Instance;

        private static ConnectionMultiplexer m_redisPub; // = ConnectionMultiplexer.Connect("localhost");       // store and re-use this!!!
        private static ConnectionMultiplexer m_redisSub;
        //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("server1:6379,server2:6379"); // master/slave setup

        private ISubscriber m_sub;
        private ISubscriber m_pub;

        private ConcurrentDictionary<string, long> m_publishCounts = new ConcurrentDictionary<string, long>();
        private ConcurrentDictionary<string, long> m_receiveCounts = new ConcurrentDictionary<string, long>();
        private ConcurrentBag<string> m_activePriceUpdateSymbols = new ConcurrentBag<string>();


        public RedisIQFeed()
        {
        }

        /*public void Subscribe(string channel, Action<RedisChannel, RedisValue> handler)
        {
            sub.Subscribe(channel, handler);
        }

        public void Publish(string channel, string message)
        {
            sub.Publish(channel, message);
        }*/

        public void StartPriceSubscriber(string ip, int port = 6379) //string symbol)
        {
            m_redisSub = ConnectRedis(ip, port);
            m_sub = m_redisSub.GetSubscriber();

            dout("RedisIQFeed::SubscribePriceUpdates()");
            m_sub.SubscribeAsync(IQ_CHANNEL, (channel, message) => {
                OnSubscriberReceive?.Invoke((string)message);
            });
        }

        public void StartPricePublisher(string ip, int port = 6379)
        {
            m_redisPub = ConnectRedis(ip, port);
            m_pub = m_redisSub.GetSubscriber();

            Task.Run(() => DisplayPublishCounts());
        }

        public void RequestPriceUpdates(string symbol)    //, PriceFeed.PriceUpdateHandler updateHandler)
        {
            dout("RedisIQFeed::RequestPriceUpdates({0})", symbol);
            //if (m_activePriceUpdateSymbols.Contains(symbol)) return;        // symbol is already active, so return

            m_activePriceUpdateSymbols.Add(symbol);
            m_priceFeed.SubscribePrices(symbol);
            m_priceFeed.UpdatePrices += M_priceFeed_UpdatePrices;
            cout("\nSubscribed to IQFeed symbol: {0}", symbol);
        }

        private void M_priceFeed_UpdatePrices(Tools.IQFeed.PriceUpdateIQ update)
        {
            //var sUpdate = string.Format("{0},{1},{2},{3},{4},{5}", update.Symbol, update.LastTradePrice, update.Bid, update.BidSize, update.Ask, update.AskSize);
            var sUpdate = update.ToString();
            m_pub.PublishAsync(IQ_CHANNEL, sUpdate);
            //Console.WriteLine("PUB to channel {0} => {1}", GetChannel(update.Symbol), sUpdate);
            //Console.Write("(PUB:{0})  ", update.Symbol);
            long count;
            if (m_publishCounts.TryGetValue(update.Symbol, out count))
            {
                m_publishCounts[update.Symbol] = ++count;
            }
            else
            {
                m_publishCounts[update.Symbol] = 1;
            }
        }

        /*private string GetChannel(string symbol)
        {
            return string.Format("{0}:{1}", IQ_CHANNEL, symbol);
        }*/

        private ConnectionMultiplexer ConnectRedis(string ip, int port)
        {
            ConfigurationOptions co = new ConfigurationOptions()
            {
                SyncTimeout = 500000,
                EndPoints =
                {
                    { ip, port }
                },
                AbortOnConnectFail = false      // auto-reconnect in the background if a network blip occurs 
            };
            return ConnectionMultiplexer.Connect(co);
        }



        // Launch this method as a thread that will display the publish counts every 30 seconds
        private void DisplayPublishCounts(int sleepSeconds = 30)
        {
            for (;;)
            {
                dout("\nconnected:{0} status:{1}", m_redisPub.IsConnected,  m_redisPub.GetStatus()); //m_redis.GetEndPoints()[0],

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

    } // end of class RedisClient
} // end of namespace
