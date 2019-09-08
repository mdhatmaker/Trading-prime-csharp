namespace GDAX.NET.Endpoints.Account {
    using System;
    using Newtonsoft.Json.Linq;

    public class AccountHistoryTransfer : AccountHistory {
        public AccountHistoryTransfer( JToken jToken ) : base( jToken ) {
            if ( this.Type != "transfer" ) {
                throw new InvalidOperationException( "Transfer record can only be created from a valid transfer type json object" );
            }

            var details = jToken[ "details" ];
            var transferIdToken = details[ "transfer_id" ];
            var transferTypeToken = details[ "transfer_type" ];

            this.TransferId = transferIdToken?.Value< String >();
            this.TransferType = transferTypeToken?.Value< String >();
        }

        public String TransferId { get; set; }
        public String TransferType { get; set; }
    }
}