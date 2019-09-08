namespace GDAX.NET.Endpoints.OrderBook {
    using System;
    using Newtonsoft.Json.Linq;

    public class RealtimeMessage {
        protected RealtimeMessage( JToken jToken ) {
            this.Type = jToken[ "type" ].Value< String >();
            this.Sequence = jToken[ "sequence" ].Value< Int64 >();
            this.Price = jToken[ "price" ].Value< Decimal >();
        }

        public String Type { get; set; }
        public Int64 Sequence { get; set; }
        public Decimal Price { get; set; }
    }

    public class RealtimeReceived : RealtimeMessage {
        public RealtimeReceived( JToken jToken ) : base( jToken ) {
            this.OrderId = jToken[ "order_id" ].Value< String >();
            this.Size = jToken[ "size" ].Value< Decimal >();
            this.Side = jToken[ "side" ].Value< String >();
        }

        public String OrderId { get; set; }
        public Decimal Size { get; set; }
        public String Side { get; set; }
    }

    public class RealtimeOpen : RealtimeMessage {
        public RealtimeOpen( JToken jToken ) : base( jToken ) {
            this.OrderId = jToken[ "order_id" ].Value< String >();
            this.RemainingSize = jToken[ "remaining_size" ].Value< Decimal >();
            this.Side = jToken[ "side" ].Value< String >();
        }

        public String OrderId { get; set; }
        public Decimal RemainingSize { get; set; }
        public String Side { get; set; }
    }

    public class RealtimeDone : RealtimeMessage {
        public RealtimeDone( JToken jToken ) : base( jToken ) {
            this.OrderId = jToken[ "order_id" ].Value< String >();
            this.RemainingSize = jToken[ "remaining_size" ].Value< Decimal >();
            this.Side = jToken[ "side" ].Value< String >();
            this.Reason = jToken[ "reason" ].Value< String >();
        }

        public String OrderId { get; set; }
        public Decimal RemainingSize { get; set; }
        public String Side { get; set; }
        public String Reason { get; set; }
    }

    public class RealtimeMatch : RealtimeMessage {
        public RealtimeMatch( JToken jToken ) : base( jToken ) {
            this.TradeId = jToken[ "trade_id" ].Value< Decimal >();
            this.MakerOrderId = jToken[ "maker_order_id" ].Value< String >();
            this.TakerOrderId = jToken[ "taker_order_id" ].Value< String >();
            this.Time = jToken[ "time" ].Value< DateTime >();
            this.Price = jToken[ "price" ].Value< Decimal >();
            this.Side = jToken[ "side" ].Value< String >();
        }

        public Decimal TradeId { get; set; }
        public String MakerOrderId { get; set; }
        public String TakerOrderId { get; set; }
        public DateTime Time { get; set; }
        public Decimal Price { get; set; }
        public String Side { get; set; }
    }

    public class RealtimeChange : RealtimeMessage {
        public RealtimeChange( JToken jToken ) : base( jToken ) {
            this.OrderId = jToken[ "order_id" ].Value< String >();
            this.Time = jToken[ "time" ].Value< DateTime >();
            this.NewSize = jToken[ "new_size" ].Value< Decimal >();
            this.OldSize = jToken[ "old_size" ].Value< Decimal >();
            this.Side = jToken[ "side" ].Value< String >();
        }

        public String OrderId { get; set; }
        public DateTime Time { get; set; }
        public Decimal NewSize { get; set; }
        public Decimal OldSize { get; set; }
        public String Side { get; set; }
    }

    public class RealtimeError {}
}
