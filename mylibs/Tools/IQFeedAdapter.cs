using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace IQFeed
{
    public delegate void MessageReceivedDelegate(string message);
    public delegate void QuoteReceivedDelegate(IQFeedPriceQuote quote);

    internal delegate void ClientConnectedDelegate(int x, int y);

    public class IQFeedAdapter : IDisposable
    {
        public event MessageReceivedDelegate MessageReceived;
        public event QuoteReceivedDelegate QuoteReceived;

        #region COM connection functions
        [DllImport("IQ32.dll")]
        private static extern void SetCallbackFunction(ClientConnectedDelegate SetConnected);
        [DllImport("IQ32.dll")]
        private static extern int RegisterClientApp(IntPtr hClient, string szProductName,
            string szProductKey, string szProductVersion);
        [DllImport("IQ32.dll")]
        private static extern void RemoveClientApp(IntPtr hClient);

        private ClientConnectedDelegate IQFeed_CallBack;
        private static bool isConnected = false;

        private static void SetConnected(int x, int y)
        {
            isConnected = (x == 0 && y == 0);

            if (x == 1 && y == 1)
            {
                Console.WriteLine("IQFeed delivered close message");
                RemoveClientApp(IntPtr.Zero);
            }
        }
        #endregion

        #region private properties
        private NetworkStream messageStream;
        private StreamReader messageStreamReader;
        private TcpClient client;

        private Thread messageThread;
        private Thread eventThread;
        private AutoResetEvent messageAvailable;

        private bool monitoringActive;
        private List<string> messageQueue;
        #endregion

        #region public properties
        public string ServerAddress = "127.0.0.1";
        public int ServerPort = 5009;
        public bool IsConnected { get { return isConnected; } }
        #endregion

        #region public methods

        public void MonitorSymbol(string symbol)
        {
            WriteStream(messageStream, "w" + symbol + "\n");
        }

        public void UnmonitorSymbol(string symbol)
        {
            WriteStream(messageStream, "r" + symbol + "\n");
        }

        public void SendRawMessage(string message)
        {
            WriteStream(messageStream, message);
        }

        public void Connect(string appName, string appKey, string appVersion)
        {
            messageQueue = new List<string>();
            messageAvailable = new AutoResetEvent(false);

            IQFeed_CallBack = new ClientConnectedDelegate(IQFeedAdapter.SetConnected);
            SetCallbackFunction(IQFeed_CallBack);

            RegisterClientApp(IntPtr.Zero, appName, appKey, appVersion);

            while (!isConnected)
                Thread.Sleep(100);

            try
            {
                client = new TcpClient(ServerAddress, ServerPort);
                messageStream = client.GetStream();
                messageStreamReader = new StreamReader(messageStream);

                string keyData = ReadStream(messageStreamReader);
                keyData = keyData.Split('\n')[0] + "\n";
                WriteStream(messageStream, keyData);
                keyData = ReadStream(messageStreamReader);

                monitoringActive = true;
                messageThread = new Thread(new ThreadStart(ReceiveMessages));
                eventThread = new Thread(new ThreadStart(ConsumeMessages));

                messageThread.Start();
                eventThread.Start();
            }
            catch (ArgumentNullException e)
            {
                throw e;
            }

            catch (SocketException e)
            {
                throw e;
            }
        }

        public void Disconnect()
        {
            monitoringActive = false;
            WriteStream(messageStream, "S,DISCONNECT\n");

            eventThread.Join(15000);
            messageThread.Join(15000);

            client.Close();
            RemoveClientApp(IntPtr.Zero);

            isConnected = false;
            GC.KeepAlive(IQFeed_CallBack);
        }

        ~IQFeedAdapter()
        {
            if (isConnected)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (isConnected)
            {
                Disconnect();
            }
        }

        #endregion

        #region private methods

        private void ReceiveMessages()
        {
            while (monitoringActive)
            {
                string message = ReadStream(messageStreamReader);

                foreach (string messagePart in message.Split('\n'))
                    messageQueue.Add(messagePart);

                messageAvailable.Set();
            }
        }

        private void ConsumeMessages()
        {
            while (monitoringActive)
            {
                messageAvailable.WaitOne();

                while (messageQueue.Count > 0)
                {
                    string message = messageQueue[0];

                    if (message != null)
                    {
                        if (message.StartsWith("Q,"))
                            if (QuoteReceived != null)
                                QuoteReceived(new IQFeedPriceQuote(message));

                        if (MessageReceived != null)
                            MessageReceived(message);
                    }

                    messageQueue.RemoveAt(0);
                }
            }
        }

        private static string ReadStream(StreamReader messageStreamReader)
        {
            //byte[] data = new byte[5120];
            //String responseData = String.Empty;
            //Int32 bytes = messageStream.Read(data, 0, data.Length);
            //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            return messageStreamReader.ReadLine();
        }

        private static void WriteStream(NetworkStream messageStream, string message)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            messageStream.Write(data, 0, data.Length);
        }

        #endregion
    }
}