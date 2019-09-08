using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;
using WebSocketX;

[TestClass]
public class WebsocketTests
{
    private static readonly ManualResetEvent resetEvent = new ManualResetEvent(false);
    private static WebsocketCoinigy socket;

    [TestMethod]
    public void Subscribe_And_Listen()
    {
        // Initialize an instance of our socket
        socket = new WebsocketCoinigy(new ApiCredentials
        {
            ApiKey = "[YOUR-API-KEY]",
            ApiSecret = "[YOUR-API-SECRET]"
        });

        // Subscribe to OnClientReady-event so we know when we can subscribe to trade channels
        socket.OnClientReady += Socket_OnClientReady;

        // Subscribe to the OnTradeMessage-event so we can receive trade messages
        socket.OnTradeMessage += Socket_OnTradeMessage;

        // Finally we can connect to our socket and wait for incoming messages
        socket.Connect();

        // Forces the methods not to exit
        resetEvent.WaitOne();
    }

    private void WriteLog(string message)
    {
        Debug.WriteLine($"{DateTime.UtcNow}: {message}");
    }

    private void Socket_OnTradeMessage(string exchange, string primaryCurrency,
                                       string secondaryCurrency, TradeItem trade)
    {
        WriteLog($"Received new trade for (exchange) market {primaryCurrency}/{secondaryCurrency} price {trade.Price}");

        /*WriteLog($"Received new trade for {exchange} market
                 { primaryCurrency}/{ secondaryCurrency}
        price { trade.Price}
        ");*/
    }

    private void Socket_OnClientReady()
    {
        // Subscribe to a new trade channel
        socket.SubscribeToTradeChannel("BMEX", "XBT", "USD");
    }

    // unit test code  
    [TestMethod]  
    public void MyFirstTestMethod()  
    {  
        // arrange  
        double beginningBalance = 11.99;  
        double debitAmount = 4.55;  
        double expected = 7.44;

        // assert  
        double actual = beginningBalance + debitAmount;
        Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");  
    }  



} // end of namespace