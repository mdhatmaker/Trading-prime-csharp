namespace GDAX.NET.Endpoints.OrderBook {
    using System;
    using Core;

    public class GetProductOrderBookRequest : ExchangeRequestBase {
        public GetProductOrderBookRequest( String productId, Int64 level = 1 ) : base( "GET" ) {
            if ( String.IsNullOrWhiteSpace( productId ) ) {
                throw new ArgumentNullException( nameof( productId ) );
            }

            this.RequestUrl = $"/products/{productId}/book?level={level}";
        }
    }
}
