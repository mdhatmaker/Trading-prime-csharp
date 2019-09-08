namespace GDAX.NET.Endpoints.Account {
    using System;
    using Core;

    public class GetAccountHistoryRequest : ExchangePageableRequestBase {
        public GetAccountHistoryRequest( String accountId ) : base( "GET" ) {
            if ( String.IsNullOrWhiteSpace( accountId ) ) {
                throw new ArgumentNullException( nameof( accountId ) );
            }

            var urlFormat = $"/accounts/{accountId}/ledger";
            this.RequestUrl = urlFormat;
        }
    }
}
