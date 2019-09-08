using Rokolab.BitstampClient.Models;
using System.Collections.Generic;

namespace Rokolab.BitstampClient
{
    public interface IBitstampClient
    {
        TickerResponse GetTicker();
        BalanceResponse GetBalance();
        OrderStatusResponse GetOrderStatus(string orderId);
        List<OpenOrderResponse> GetOpenOrders();
        List<TransactionResponse> GetTransactions(int offset, int limit);
        bool CancelOrder(string orderId);
        bool CancelAllOrders();
        BuySellResponse Buy(double amount, double price);
        BuySellResponse Sell(double amount, double price);
    }
}