namespace GDAX.NET.Endpoints.Account {
    using System;
    using Core;

    public class ListAccountsRequest : ExchangePageableRequestBase {
        public ListAccountsRequest( String accountId = null, String cursor = null, Int64 recordCount = 100, RequestPaginationType paginationType = RequestPaginationType.After ) : base( "GET" ) {
            var urlFormat = $"/accounts/{accountId ?? String.Empty}";
            this.RequestUrl = urlFormat;
        }
    }
}
