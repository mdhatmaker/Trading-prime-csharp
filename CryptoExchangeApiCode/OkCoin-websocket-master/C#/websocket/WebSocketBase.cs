using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using System.IO;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
namespace websocket
{
    class WebSocketBase
    {
        private WebSocket webSocketClient = null;
        private WebSocketService webService = null;
        private Boolean isRecon = false;
        private string url = null;
        private System.Timers.Timer t = new System.Timers.Timer(2000); //
        public WebSocketBase(string url,WebSocketService webService)
        {
            this.webService = webService;
            this.url = url;
        }
        public void start() {
            webSocketClient = new WebSocket(url);
            webSocketClient.OnError += new EventHandler<WebSocketSharp.ErrorEventArgs>(webSocketClient_Error);
            webSocketClient.OnOpen += new EventHandler(webSocketClient_Opened);
            webSocketClient.OnClose += new EventHandler<WebSocketSharp.CloseEventArgs>(webSocketClient_Closed);
            webSocketClient.OnMessage += new EventHandler<MessageEventArgs>(webSocketClient_MessageReceived);
            webSocketClient.ConnectAsync();
            while (!webSocketClient.IsAlive) {
                Console.WriteLine("Waiting WebSocket connnet......");
                Thread.Sleep(1000);
            }
            t.Elapsed += new System.Timers.ElapsedEventHandler(heatBeat);
            t.Start();
        }

        private void heatBeat(object sender, System.Timers.ElapsedEventArgs e) {

            this.send("{'event':'ping'}");
        }
        private void webSocketClient_Error(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            
        }
        private void webSocketClient_MessageReceived(object sender, MessageEventArgs e)
        {
           
            webService.onReceive(e.Data);
        }

        private void webSocketClient_Closed(object sender, WebSocketSharp.CloseEventArgs e)
        {
            if (!webSocketClient.IsAlive)
            {
                isRecon = true;
                webSocketClient.ConnectAsync();
            }
        }
        public string Decompress(byte[] baseBytes)
        {
            string resultStr = string.Empty;
            using (MemoryStream memoryStream = new MemoryStream(baseBytes))
            {
                using (InflaterInputStream inf = new InflaterInputStream(memoryStream))
                {
                    using (MemoryStream buffer = new MemoryStream())
                    {
                        byte[] result = new byte[1024];

                        int resLen;
                        while ((resLen = inf.Read(result, 0, result.Length)) > 0)
                        {
                            buffer.Write(result, 0, resLen);
                        }
                        resultStr = Encoding.Default.GetString(result);
                    }
                }
            }
            return resultStr;
        }
        private void webSocketClient_Opened(object sender, EventArgs e)
        {
            this.send("{'event':'ping'}");
        }
       
        public Boolean isReconnect()
        {
            if (isRecon) {
                if (webSocketClient.IsAlive)
                {
                    isRecon = false;
                }
                return true;
            }
            return false;
        }
        public void send(string channle) {
            webSocketClient.Send(channle);
        }
        public void stop() {
            if (webSocketClient!=null)
                webSocketClient.Close();
        }
      
    }
    interface WebSocketService
    {
        void onReceive(String msg);
    }
}
