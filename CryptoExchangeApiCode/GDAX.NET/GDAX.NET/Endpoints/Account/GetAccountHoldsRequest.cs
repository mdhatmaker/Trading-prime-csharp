namespace GDAX.NET.Endpoints.Account {
    using System;
    using Core;

    public class GetAccountHoldsRequest : ExchangePageableRequestBase {
        public GetAccountHoldsRequest( String accountId ) : base( "GET" ) {
            if ( String.IsNullOrWhiteSpace( accountId ) ) {
                throw new ArgumentNullException( nameof( accountId ) );
            }

            var urlFormat = $"/accounts/{accountId}/holds";
            this.RequestUrl = urlFormat;
        }
    }
}
