using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using ZeroMQ;
//using NetMQ;
//using NetMQ.Sockets;

namespace Tools
{
    public static class GNetwork
    {
        // ZeroMQ


        /*public static void TestZMQ()
        {
            using (var context = new ZContext())
            {
                using (var socket = new ZSocket(context, ZSocketType.REQ))
                {
                    socket.Connect("tcp://127.0.0.1:5000");
                    //socket.Send("My Reply", Encoding.UTF8);
                    socket.Send(new ZMessage(new[] { new ZFrame(32) }));
                    var replyMsg = socket.ReceiveMessage();     // (Encoding.UTF8);
                }
            }
        }

        public static void MultipleSocketReader()
        {
            //
            // Reading from multiple sockets
            // This version uses a simple recv loop
            //
            // Author: metadings
            //
            
            using (var context = new ZContext())
            using (var receiver = new ZSocket(context, ZSocketType.PULL))
            using (var subscriber = new ZSocket(context, ZSocketType.SUB))
            {
                // Connect to task ventilator
                receiver.Connect("tcp://127.0.0.1:5557");

                // Connect to weather server
                subscriber.Connect("tcp://127.0.0.1:5556");
                subscriber.SetOption(ZSocketOption.SUBSCRIBE, "10001 ");

                // Process messages from both sockets
                // We prioritize traffic from the task ventilator
                ZError error;
                ZFrame frame;
                while (true)
                {
                    while (true)
                    {
                        if (null != (frame = receiver.ReceiveFrame(ZSocketFlags.DontWait, out error)))
                        {
                            // Process task
                        }
                        else
                        {
                            if (error == ZError.ETERM)
                                return; // Interrupted
                            if (error != ZError.EAGAIN)
                                throw new ZException(error);

                            break;
                        }
                    }

                    while (true)
                    {
                        if (null != (frame = subscriber.ReceiveFrame(ZSocketFlags.DontWait, out error)))
                        {
                            // Process weather update
                        }
                        else
                        {
                            if (error == ZError.ETERM)
                                return; // Interrupted
                            if (error != ZError.EAGAIN)
                                throw new ZException(error);

                            break;
                        }
                    }

                    // No activity, so sleep for 1 msec
                    Thread.Sleep(1);
                }
            }
        }

        public static void MultipleSocketPoller()
        {
            //
            // Reading from multiple sockets
            // This version uses zmq_poll()
            //
            // Author: metadings
            //

            using (var context = new ZContext())
            using (var receiver = new ZSocket(context, ZSocketType.PULL))
            using (var subscriber = new ZSocket(context, ZSocketType.SUB))
            {
                // Connect to task ventilator
                receiver.Connect("tcp://127.0.0.1:5557");

                // Connect to weather server
                subscriber.Connect("tcp://127.0.0.1:5556");
                subscriber.SetOption(ZSocketOption.SUBSCRIBE, "10001 ");

                var sockets = new ZSocket[] { receiver, subscriber };
                var polls = new ZPollItem[] { ZPollItem.CreateReceiver(), ZPollItem.CreateReceiver() };

                // Process messages from both sockets
                ZError error;
                ZMessage[] msg;
                while (true)
                {
                    if (sockets.PollIn(polls, out msg, out error, TimeSpan.FromMilliseconds(64)))
                    {
                        if (msg[0] != null)
                        {
                            // Process task
                        }
                        if (msg[1] != null)
                        {
                            // Process weather update
                        }
                    }
                    else
                    {
                        if (error == ZError.ETERM)
                            return; // Interrupted
                        if (error != ZError.EAGAIN)
                            throw new ZException(error);
                    }
                }
            }
        }*/

        /*
        //  Round-trip demonstrator
        //  While this example runs in a single process, that is just to make
        //  it easier to start and stop the example. The client task signals to
        //  main when it's ready.
        public static void Tripping(string[] args)
        {
            CancellationTokenSource cancellor = new CancellationTokenSource();
            Console.CancelKeyPress += (s, ea) =>
            {
                ea.Cancel = true;
                cancellor.Cancel();
            };

            using (ZContext ctx = new ZContext())
            {
                using (var client = new ZActor(ctx, Tripping_ClientTask))
                {
                    (new Thread(() => Tripping_WorkerTask(ctx))).Start();
                    (new Thread(() => Tripping_BrokerTask(ctx))).Start();
                    client.Start();
                    using (var signal = client.Frontend.ReceiveFrame())                        
                        if (Verbose)
                            signal.ToString().DumpString();
                }
            }
        }

        static void Tripping_ClientTask(ZContext ctx, ZSocket pipe, CancellationTokenSource cancellor, object[] args)
        {
            using (ZSocket client = new ZSocket(ctx, ZSocketType.DEALER))
            {
                client.Connect("tcp://127.0.0.1:5555");
                "Setting up test...".DumpString();
                Thread.Sleep(100);

                int requests;
                "Synchronous round-trip test...".DumpString();
                var start = DateTime.Now;
                Stopwatch sw = Stopwatch.StartNew();
                for (requests = 0; requests < 10000; requests++)
                {
                    using (var outgoing = new ZFrame("hello"))
                    {
                        client.Send(outgoing);
                        using (var reply = client.ReceiveFrame())
                        {
                            if (Verbose)
                                reply.ToString().DumpString();
                        }
                    }
                }
                sw.Stop();
                " {0} calls - {1} ms => {2} calls / second".DumpString(requests, sw.ElapsedMilliseconds, requests * 1000 / sw.ElapsedMilliseconds);

                "Asynchronous round-trip test...".DumpString();
                sw.Restart();
                for (requests = 0; requests < 100000; requests++)
                    using (var outgoing = new ZFrame("hello"))
                        client.SendFrame(outgoing);

                for (requests = 0; requests < 100000; requests++)
                    using (var reply = client.ReceiveFrame())
                        if (Verbose)
                            reply.ToString().DumpString();
                sw.Stop();
                " {0} calls - {1} ms => {2} calls / second".DumpString(requests, sw.ElapsedMilliseconds, requests * 1000 / sw.ElapsedMilliseconds);
                using (var outgoing = new ZFrame("done"))
                    pipe.SendFrame(outgoing);
            }
        }

        //  .split worker task
        //  Here is the worker task. All it does is receive a message, and
        //  bounce it back the way it came:
        static void Tripping_WorkerTask(ZContext ctx)
        {
            using (var worker = new ZSocket(ctx, ZSocketType.DEALER))
            {
                worker.Connect("tcp://127.0.0.1:5556");

                while (true)
                {
                    ZError error;
                    ZMessage msg = worker.ReceiveMessage(out error);
                    if (error == null && worker.Send(msg, out error))
                        continue;
                    // errorhandling, context terminated or sth else
                    if (error.Equals(ZError.ETERM))
                        return; // Interrupted
                    throw new ZException(error);
                }
            }
        }

        //  .split broker task
        //  Here is the broker task. It uses the {{zmq_proxy}} function to switch
        //  messages between frontend and backend:
        static void Tripping_BrokerTask(ZContext ctx)
        {
            using (var frontend = new ZSocket(ctx, ZSocketType.DEALER))
            using (var backend = new ZSocket(ctx, ZSocketType.DEALER))
            {
                frontend.Bind("tcp://*:5555");
                backend.Bind("tcp://*:5556");

                ZError error;
                if (!ZContext.Proxy(frontend, backend, out error))
                {
                    if (Equals(error, ZError.ETERM))
                        return; // Interrupted
                    throw new ZException(error);
                }
            }
        }
        */


    } // end of class



    /*
    //
    //  mdcliapi class - Majordomo Protocol Client API
    //  Implements the MDP/Worker spec at http://rfc.zeromq.org/spec:7.
    //
    // Author: metadings
    //
    public class MajordomoClient : IDisposable
    {
        //  Structure of our class
        //  We access these properties only via class methods
        // Our context
        readonly ZContext _context;
        // Majordomo broker
        public string Broker { get; protected set; }
        //  Socket to broker
        public ZSocket Client { get; protected set; }
        //  Print activity to console
        public bool Verbose { get; protected set; }
        //  Request timeout
        public TimeSpan Timeout { get; protected set; }
        public void ConnectToBroker()
        {
            //  Connect or reconnect to broker. In this asynchronous class we use a
            //  DEALER socket instead of a REQ socket; this lets us send any number
            //  of requests without waiting for a reply.
            Client = new ZSocket(_context, ZSocketType.DEALER);
            Client.Connect(Broker);
            if (Verbose)
                "I: connecting to broker at '{0}'…".DumpString(Broker);

        }
        //  The constructor and destructor are the same as in mdcliapi, except
        //  we don't do retries, so there's no retries property.
        //  ---------------------------------------------------------------------
        public MajordomoClient(string broker, bool verbose)
        {
            if (broker == null)
                throw new InvalidOperationException();
            _context = new ZContext();
            Broker = broker;
            Verbose = verbose;
            Timeout = TimeSpan.FromMilliseconds(2500);
            ConnectToBroker();
        }
        ~MajordomoClient()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Destructor
                if (Client != null)
                {
                    Client.Dispose();
                    Client = null;
                }
                ////Do not Dispose Context: cuz of weird shutdown behavior, stucks in using calls //
            }
        }
        //  Set request timeout
        public void Set_Timeout(int timeoutInMs)
        {
            Timeout = TimeSpan.FromMilliseconds(timeoutInMs);
        }
        //  The send method now just sends one message, without waiting for a
        //  reply. Since we're using a DEALER socket we have to send an empty
        //  frame at the start, to create the same envelope that the REQ socket
        //  would normally make for us:
        public int Send(string service, ZMessage request, CancellationTokenSource cancellor)
        {
            if (request == null)
                throw new NotImplementedException();
            if (cancellor.IsCancellationRequested
                    || (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
                _context.Shutdown();
            //  Prefix request with protocol frames
            //  Frame 0: empty (REQ emulation)
            //  Frame 1: "MDPCxy" (six bytes, MDP/Client x.y)
            //  Frame 2: Service name (printable string)
            request.Prepend(new ZFrame(service));
            request.Prepend(new ZFrame(MdpCommon.MDPC_CLIENT));
            request.Prepend(new ZFrame(string.Empty));
            if (Verbose)
                request.DumpZmsg("I: send request to '{0}' service:", service);
            ZError error;
            if (!Client.Send(request, out error)) ;
            {
                if (Equals(error, ZError.ETERM))
                    cancellor.Cancel(); // Interrupted
                                        //throw new ZException(error);
            }
            return 0;
        }
        //  The recv method waits for a reply message and returns that to the //
        //  caller.
        //  ---------------------------------------------------------------------
        //  Returns the reply message or NULL if there was no reply. Does not
        //  attempt to recover from a broker failure, this is not possible
        //  without storing all unanswered requests and resending them all…
        public ZMessage Recv(CancellationTokenSource cancellor)
        {
            //  Poll socket for a reply, with timeout
            var p = ZPollItem.CreateReceiver();
            ZMessage msg;
            ZError error;
            //  On any blocking call, libzmq will return -1 if there was
            //  an error; we could in theory check for different error codes,
            //  but in practice it's OK to assume it was EINTR (Ctrl-C)://
            // Poll the client Message
            if (Client.PollIn(p, out msg, out error, Timeout))
            {
                //  If we got a reply, process it
                if (Verbose)
                    msg.DumpZmsg("I: received reply");
                //  Don't try to handle errors, just assert noisily
                if (msg.Count < 4)
                    throw new InvalidOperationException();
                using (ZFrame empty = msg.Pop())
                    if (!empty.ToString().Equals(string.Empty))
                        throw new InvalidOperationException();
                using (ZFrame header = msg.Pop())
                    if (!header.ToString().Equals(MdpCommon.MDPC_CLIENT))
                        throw new InvalidOperationException();
                using (ZFrame replyService = msg.Pop())
                { }
                return msg;
            }
            else if (Equals(error, ZError.ETERM))
            {
                "W: interrupt received, killing client…\n".DumpString();
                cancellor.Cancel();
            }
            else
            {
                if (Verbose)
                    "W: permanent error, abandoning Error: {0}".DumpString(error);
            }
            return null;
        }
    }
    */




} // end of namespace
