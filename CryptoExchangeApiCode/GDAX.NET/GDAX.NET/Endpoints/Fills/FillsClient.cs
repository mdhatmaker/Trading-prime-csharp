namespace GDAX.NET.Endpoints.Fills {
    using System.Threading.Tasks;
    using Core;

    public class FillsClient : ExchangeClientBase {
        public FillsClient( CBAuthenticationContainer authenticationContainer ) : base( authenticationContainer ) { }

        public async Task< GetFillsResponse > GetFills() {
            var request = new GetFillsRequest();
            var response = await this.GetResponse( request );
            var accountHistoryResponse = new GetFillsResponse( response );
            return accountHistoryResponse;
        }
    }
}
