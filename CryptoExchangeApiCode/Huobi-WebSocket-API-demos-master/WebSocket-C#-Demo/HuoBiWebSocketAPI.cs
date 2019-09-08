using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;

namespace Huobi.Market.WebSocketAPI
{
    public static class HuobiMarket
    {
        #region 私有属性
        private static WebSocket websocket;
        private static Dictionary<string, string> topicDic = new Dictionary<string, string>();
        private static bool isOpened = false;
        private const string HUOBI_WEBSOCKET_API = "ws://api.huobi.pro/ws";
        #endregion
        #region  市场信息常量
        public const string MARKET_KLINE = "market.{0}.kline.{1}";
        public const string MARKET_DEPTH = "market.{0}.depth.{1}";
        public const string MARKET_TRADE_DETAIL = "market.{0}.trade.detail";
        public const string MARKET_DETAIL = "market.{0}.detail";
        #endregion
        /// <summary>
        /// 接收WebScoket消息事件
        /// </summary>
        public static event EventHandler<HuoBiMessageReceivedEventArgs> OnMessage;
        /// <summary>
        /// 初始化WebSocket
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
            try
            {
                websocket = new WebSocket(HUOBI_WEBSOCKET_API);

                websocket.Error += (sender, e) =>
                {
                    Console.WriteLine("Error:" + e.Exception.Message.ToString());
                };
                websocket.DataReceived += ReceviedMsg;
                websocket.Opened += OnOpened;
                websocket.Open();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
            }
            return true;
        }


        #region Opened&心跳响应&触发消息事件
        /// <summary>
        /// 连通WebSocket，发送订阅消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnOpened(object sender, EventArgs e)
        {
            Console.WriteLine($"OnOpened Topics Count:{topicDic.Count}");
            isOpened = true;
            foreach (var item in topicDic)
            {
                SendSubscribeTopic(item.Value);
            }

        }

        /// <summary>
        /// 响应心跳包&接收消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void ReceviedMsg(object sender, DataReceivedEventArgs args)
        {
            var msg = GZipHelper.GZipDecompressString(args.Data);
            if (msg.IndexOf("ping") != -1) //响应心跳包
            {
                var reponseData = msg.Replace("ping", "pong");
                websocket.Send(reponseData);
            }
            else//接收消息
            {
                OnMessage?.Invoke(null, new HuoBiMessageReceivedEventArgs(msg));
            }

        }
        #endregion


        #region 订阅相关
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="id"></param>
        public static void Subscribe(string topic, string id)
        {
            if (topicDic.ContainsKey(topic))
                return;
            var msg = $"{{\"sub\":\"{topic}\",\"id\":\"{id}\"}}";
            topicDic.Add(topic, msg);
            if (isOpened)
            {
                SendSubscribeTopic(msg);
            }
        }


        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="id"></param>

        public static void UnSubscribe(string topic, string id)
        {
            if (!topicDic.ContainsKey(topic) || !isOpened)
                return;
            var msg = $"{{\"unsub\":\"{topic}\",\"id\":\"{id}\"}}";
            topicDic.Remove(topic);
            SendSubscribeTopic(msg);
            Console.WriteLine($"UnSubscribed, Topics Count:{topicDic.Count}");

        }
        private static void SendSubscribeTopic(string msg)
        {
            websocket.Send(msg);
            Console.WriteLine(msg);
        }
        #endregion


    }
    /// <summary>
    /// 事件内容
    /// </summary>
    public class HuoBiMessageReceivedEventArgs : EventArgs
    {
        public HuoBiMessageReceivedEventArgs(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}
