namespace GDAX.NET.Endpoints.Account {
    using System;
    using System.Threading.Tasks;
    using Core;

    public class AccountClient : ExchangeClientBase {
        public AccountClient( CBAuthenticationContainer authContainer ) : base( authContainer ) { }

        public async Task< ListAccountsResponse > ListAccounts( String accountId = null, String cursor = null, Int64 recordCount = 100, RequestPaginationType paginationType = RequestPaginationType.After ) {
            var request = new ListAccountsRequest( accountId, cursor, recordCount, paginationType );
            var response = await this.GetResponse( request );
            var accountResponse = new ListAccountsResponse( response );
            return accountResponse;
        }

        public async Task< GetAccountHistoryResponse > GetAccountHistory( String accountId ) {
            var request = new GetAccountHistoryRequest( accountId );
            var response = await this.GetResponse( request );
            var accountHistoryResponse = new GetAccountHistoryResponse( response );
            return accountHistoryResponse;
        }

        public async Task< GetAccountHoldsResponse > GetAccountHolds( String accountId ) {
            var request = new GetAccountHoldsRequest( accountId );
            var response = await this.GetResponse( request );
            var accountHoldsResponse = new GetAccountHoldsResponse( response );
            return accountHoldsResponse;
        }
    }
}
