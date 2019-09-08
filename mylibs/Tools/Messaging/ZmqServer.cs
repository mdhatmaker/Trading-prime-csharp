using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZeroMQ;
//using NetMQ;
//using NetMQ.Sockets;

namespace Tools.Messaging
{
    public static partial class ZMQ
    {
        public static void HWServer(string[] args)
        {
            //
            // Hello World server
            //
            // Author: metadings
            //

            if (args == null || args.Length < 1)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: ./{0} HWServer [Name]", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine();
                Console.WriteLine("    Name   Your name. Default: World");
                Console.WriteLine();
                args = new string[] { "World" };
            }

            string name = args[0];

            // Create
            // ZeroMQ
            using (var context = new ZContext())
            using (var responder = new ZSocket(context, ZSocketType.REP))
            {
                // Bind
                responder.Bind("tcp://*:5555");

                while (true)
                {
                    // Receive
                    using (ZFrame request = responder.ReceiveFrame())
                    {
                        Console.WriteLine("Received {0}", request.ReadString());

                        // Do some work
                        //Thread.Sleep(1);

                        // Send
                        responder.Send(new ZFrame(name));
                    }
                }
            }

            /*// NetMQ
            using (var responder = new ResponseSocket())
            {
                // Bind
                responder.Bind("tcp://*:5555");

                while (true)
                {
                    // Receive
                    string request = responder.ReceiveFrameString();

                    Console.WriteLine("Received {0}", request);

                    // Do some work
                    Thread.Sleep(1);

                    // Send
                    responder.SendFrame(name);
                }
            }*/

            
            /*using (var ctx = ZContext.Create())
            //using (var ctx = ZContext.Create())
            {
                using (var socket = ctx.CreateSocket(SocketType.PUB))
                {
                    foreach (var endPoint in options.bindEndPoints)
                        socket.Bind(endPoint);

                    long msgCptr = 0;
                    int msgIndex = 0;
                    while (true)
                    {
                        if (msgCptr == long.MaxValue)
                            msgCptr = 0;
                        msgCptr++;
                        if (options.maxMessage >= 0)
                            if (msgCptr > options.maxMessage)
                                break;
                        if (msgIndex == options.altMessages.Count())
                            msgIndex = 0;
                        var msg = options.altMessages[msgIndex++].Replace("#nb#", msgCptr.ToString("d2"));
                        Thread.Sleep(options.delay);
                        Console.WriteLine("Publishing: " + msg);
                        socket.Send(msg, Encoding.UTF8);
                    }
                }
            }*/
        }
    }
}