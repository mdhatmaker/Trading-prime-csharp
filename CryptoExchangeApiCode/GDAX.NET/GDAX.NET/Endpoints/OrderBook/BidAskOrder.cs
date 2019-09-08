namespace GDAX.NET.Endpoints.OrderBook {
    using System;

    public class BidAskOrder {
        public Decimal Price { get; set; }
        public Decimal Size { get; set; }
        public String Id { get; set; }
    }
}
