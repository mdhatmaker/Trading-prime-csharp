namespace GDAX.NET.Endpoints.Fills {
    using System;
    using Newtonsoft.Json.Linq;

    public class Fill {
        public Fill( JToken jToken ) {
            this.TradeId = jToken[ "trade_id" ].Value< String >();
            this.ProductId = jToken[ "product_id" ].Value< String >();
            this.Price = jToken[ "price" ].Value< String >();
            this.Size = jToken[ "size" ].Value< String >();
            this.OrderId = jToken[ "order_id" ].Value< String >();
            this.Time = jToken[ "created_at" ].Value< DateTime >();
            this.Fee = jToken[ "fee" ].Value< String >();
            this.Settled = jToken[ "settled" ].Value< Boolean >();
            this.Side = jToken[ "size" ].Value< String >();
        }

        public String TradeId { get; }
        public String ProductId { get;  }
        public String Price { get;  }
        public String Size { get;  }
        public String OrderId { get;  }
        public DateTime Time { get;  }
        public String Fee { get;  }
        public Boolean Settled { get;  }
        public String Side { get;  }
    }
}
