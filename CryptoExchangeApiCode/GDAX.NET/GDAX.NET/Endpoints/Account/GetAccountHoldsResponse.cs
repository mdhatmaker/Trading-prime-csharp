namespace GDAX.NET.Endpoints.Account {
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Newtonsoft.Json.Linq;

    public class GetAccountHoldsResponse : ExchangePageableResponseBase {
        public GetAccountHoldsResponse( ExchangeResponse response ) : base( response ) {
            var json = response.ContentBody;
            var jArray = JArray.Parse( json );

            this.AccountHolds = jArray.Select( elem => new AccountHold( elem ) ).ToList();
        }

        public IEnumerable< AccountHold > AccountHolds { get; private set; }
    }
}
