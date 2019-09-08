using System;
using System.Collections.Generic;

namespace CryptoAPIs
{
    public interface IOrderExchange
    {
        bool EnableLiveOrders { get; set; }
        OrderNew SubmitLimitOrder(string pair, OrderSide side, decimal price, decimal qty);
        OrderCxl CancelOrder(string pair, string orderId);
        IEnumerable<ZOrder> GetWorkingOrders(string pair);                           // allow NULL = all pairs
        IEnumerable<ZTrade> GetTrades(string pair);                                  // allow NULL = all pairs
        IDictionary<string, ZAccountBalance> GetAccountBalances();                   // allow NULL = all pairs
    }

} // end of namespace
