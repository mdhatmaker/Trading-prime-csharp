using Rokolab.BitstampClient.Logging;
using System;

namespace Rokolab.BitstampClient.Example
{
    internal class Program
    {
        public static readonly string _clientId = ""; // your client id
        public static readonly string _apiKey = ""; // your api key
        public static readonly string _apiSecret = ""; // your api secret

        private static void Main(string[] args)
        {
            ILogFactory logFactory = new NLogFactory();
            IRequestAuthenticator ra = new RequestAuthenticator(_apiKey, _apiSecret, _clientId);
            IBitstampClient client = new BitstampClient(ra, logFactory);

            var ticker = client.GetTicker();

            Console.WriteLine("Last bitcoin market value: " + ticker.last);

            Console.ReadLine();
        }
    }
}