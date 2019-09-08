using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoTools.Net
{
    public static class Net
    {
        public static void SendJson(ClientWebSocket socket, string json)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            ArraySegment<byte> subscriptionMessageBuffer = new ArraySegment<byte>(bytes);
            socket.SendAsync(subscriptionMessageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        // Given host address and KeyValuePair enumerable of parameters ("name", "value")
        // where host like "http://www.yoursite.com"
        public static string GetQueryString(string host, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var url = HttpUtility.UrlEncode(
                string.Format("{0}?{1}",
                host,
                string.Join("&", parameters.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value))))
            );
            return url;
        }

        // where method like "GET"/"HEAD"/"POST"/"PUT"/"DELETE"
        // where relativeUri like "/api/v3/account"
        // where body like "recvWindow=5000"
        public static string Invoke(this HttpClient cl, string method, string relativeUri, string body)
        {
            string _contentType = "application/json";
            HttpResponseMessage response;
            var _method = new HttpMethod(method);

            switch (_method.ToString().ToUpper())
            {
                case "GET":
                case "HEAD":
                    // synchronous request without the need for .ContinueWith() or await
                    //Console.WriteLine("GET {0}", relativeUri);                    
                    response = cl.GetAsync(relativeUri).Result;
                    break;
                case "POST":
                    {
                        // Construct an HttpContent from a StringContent
                        HttpContent _body = new StringContent(body);
                        // and add the header to this object instance
                        // optional: add a formatter option to it as well
                        _body.Headers.ContentType = new MediaTypeHeaderValue(_contentType);
                        // synchronous request without the need for .ContinueWith() or await
                        response = cl.PostAsync(relativeUri, _body).Result;
                    }
                    break;
                case "PUT":
                    {
                        // Construct an HttpContent from a StringContent
                        HttpContent _body = new StringContent(body);
                        // and add the header to this object instance
                        // optional: add a formatter option to it as well
                        _body.Headers.ContentType = new MediaTypeHeaderValue(_contentType);
                        // synchronous request without the need for .ContinueWith() or await
                        response = cl.PutAsync(relativeUri, _body).Result;
                    }
                    break;
                case "DELETE":
                    //Console.WriteLine("DELETE {0}", relativeUri);
                    response = cl.DeleteAsync(relativeUri).Result;
                    break;
                default:
                    throw new NotImplementedException();
                    //break;
            }
            // either this - or check the status to retrieve more information
            response.EnsureSuccessStatusCode();
            // get the rest/content of the response in a synchronous way
            var content = response.Content.ReadAsStringAsync().Result;

            return content;
        }

        #region ---------- Ping ---------------------------------------------------------------------------------------
        // args[0] can be an IPaddress or host name
        public static void Ping(string[] args)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(args[0], timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Reply from {0}: bytes={1} time={2}ms TTL={3}", reply.Address.ToString(), reply.Buffer.Length, reply.RoundtripTime, reply.Options.Ttl);
                //Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
            }
        }

        // args[0] can be an IPaddress or host name
        public static void PingAsync(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("Ping needs a host or IP Address.");

            string who = args[0];
            AutoResetEvent waiter = new AutoResetEvent(false);

            Ping pingSender = new Ping();

            // When the PingCompleted event is raised,
            // the PingCompletedCallback method is called.
            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            // Wait 12 seconds for a reply.
            int timeout = 12000;

            // Set options for transmission:
            // The data can go through 64 gateways or routers
            // before it is destroyed, and the data packet
            // cannot be fragmented.
            PingOptions options = new PingOptions(64, true);

            Console.WriteLine("Time to live: {0}", options.Ttl);
            Console.WriteLine("Don't fragment: {0}", options.DontFragment);

            // Send the ping asynchronously.
            // Use the waiter as the user token.
            // When the callback completes, it can wake up this thread.
            pingSender.SendAsync(who, timeout, buffer, options, waiter);

            // Prevent this example application from ending.
            // A real application should do something useful
            // when possible.
            waiter.WaitOne();
            Console.WriteLine("Ping example completed.");
        }

        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            // If the operation was canceled, display a message to the user.
            if (e.Cancelled)
            {
                Console.WriteLine("Ping canceled.");

                // Let the main thread resume. 
                // UserToken is the AutoResetEvent object that the main thread 
                // is waiting for.
                ((AutoResetEvent)e.UserState).Set();
            }

            // If an error occurred, display the exception to the user.
            if (e.Error != null)
            {
                Console.WriteLine("Ping failed:");
                Console.WriteLine(e.Error.ToString());

                // Let the main thread resume. 
                ((AutoResetEvent)e.UserState).Set();
            }

            PingReply reply = e.Reply;

            DisplayPingReply(reply);

            // Let the main thread resume.
            ((AutoResetEvent)e.UserState).Set();
        }

        public static void DisplayPingReply(PingReply reply)
        {
            if (reply == null)
                return;

            Console.WriteLine("ping status: {0}", reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}\n", reply.Buffer.Length);                
            }
        }
        #endregion ----------------------------------------------------------------------------------------------------

        #region ---------- GET/POST -----------------------------------------------------------------------------------
        // GET (with HMAC SHA256 encryption)
        // POST (no encryption)
        // where method like "/api/v3/order" and query like "symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.01&recvWindow=5000"
        // where T like BinanceNewOrder
        public static async Task<T> Post<T>(this HttpClient client, string method, string queryString) where T : NullableObject
        {
            //Console.WriteLine(queryString);
            HttpContent content = new StringContent(queryString);
            var result = await client.PostAsync(method, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(resultContent);

            var obj = JsonConvert.DeserializeObject<T>(resultContent);

            if (obj.IsNull)     // an error occurred during POST
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;
        }

        // POST (no encryption)
        // where method like "/api/v1/userDataStream"
        // where T like BinanceStartUserStream
        public static T Post<T>(this HttpClient client, string method) where T : NullableObject
        {
            //Console.WriteLine(method);
            string resultContent = client.Invoke("POST", method, "");
            //Console.WriteLine(resultContent);

            var obj = JsonConvert.DeserializeObject<T>(resultContent);

            if (obj.IsNull)     // an error occurred during POST
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;
        }

        // GET (no encryption)
        // where method like "/api/v3/ticker/bookTicker"
        // where T like BinanceBookTicker
        public static T Get<T>(this HttpClient client, string method) where T : NullableObject
        {
            //Console.WriteLine(method);
            string resultContent = client.Invoke("GET", method, "");
            //Console.WriteLine(resultContent);

            var obj = JsonConvert.DeserializeObject<T>(resultContent);

            if (obj.IsNull)     // an error occurred during POST
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;
        }

        // PUT (no encryption)
        // where method like "/api/v1/userDataStream?listenKey=pqia91ma19a5s61cv6a81va65sdf19v8a65a1a5s61cv6a81va65sdf19v8a65a1"
        // where T like BinanceBookTicker
        public static T Put<T>(this HttpClient client, string method) where T : NullableObject
        {
            //Console.WriteLine(method);
            string resultContent = client.Invoke("PUT", method, "");
            //Console.WriteLine(resultContent);

            var obj = JsonConvert.DeserializeObject<T>(resultContent);

            if (obj.IsNull)     // an error occurred during POST
            {
                var err = JsonConvert.DeserializeObject<BinanceError>(resultContent);
                Console.WriteLine("ERROR: {0}  {1}", err.code, err.msg);
            }

            return obj;
        }
        #endregion ----------------------------------------------------------------------------------------------------



    } // end of class Net



    public class BinanceEmpty : NullableObject
    {
        public bool IsNull { get { return false; } }
    }

    public interface NullableObject
    {
        bool IsNull { get; }
    }

    public class BinanceError
    {
        public int code { get; set; }
        public string msg { get; set; }
    }

} // end of namespace
