namespace GDAX.NET.Endpoints.OrderBook {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Newtonsoft.Json.Linq;

    public class GetProductOrderBookResponse : ExchangeResponseBase {
        public GetProductOrderBookResponse( ExchangeResponse response ) : base( response ) {
            var json = response.ContentBody;
            var jObject = JObject.Parse( json );

            var bids = jObject[ "bids" ].Select( x => ( JArray )x ).ToArray();
            var asks = jObject[ "asks" ].Select( x => ( JArray )x ).ToArray();

            this.Sequence = jObject[ "sequence" ].Value< Int32 >();

            this.Sells = asks.Select( this.GetBidAskOrderFromJToken ).ToList();
            this.Buys = bids.Select( this.GetBidAskOrderFromJToken ).ToList();
        }

        public Int32 Sequence { get; private set; }
        public IReadOnlyList< BidAskOrder > Sells { get; private set; }
        public IReadOnlyList< BidAskOrder > Buys { get; private set; }

        private BidAskOrder GetBidAskOrderFromJToken( JArray jArray ) { return new BidAskOrder { Price = jArray[ 0 ].Value< Decimal >(), Size = jArray[ 1 ].Value< Decimal >(), Id = jArray[ 2 ].Value< String >() }; }
    }
}
