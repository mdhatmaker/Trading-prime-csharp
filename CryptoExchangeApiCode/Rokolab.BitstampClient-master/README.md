# Rokolab.BitstampClient

Bitstamp API C# Client

API to get you started building bots for bitstamp.

Simple usage:

ILogFactory logFactory = new NLogFactory();
IRequestAuthenticator ra = new RequestAuthenticator(_apiKey, _apiSecret, _clientId);
IBitstampClient client = new BitstampClient(ra, logFactory);

var ticker = client.GetTicker();

Console.WriteLine("Last bitcoin market value: " + ticker.last);

Console.ReadLine();
