namespace GDAX.NET.Endpoints.Account {
    using System;
    using Newtonsoft.Json.Linq;

    public class AccountHold {
        public AccountHold( JToken jToken ) {
            this.CreatedAt = jToken[ "created_at" ].Value< DateTime >();
            this.UpdatedAt = jToken[ "updated_at" ].Value< DateTime >();
            this.OrderId = jToken[ "order_id" ].Value< String >();
            this.Amount = jToken[ "amount" ].Value< Decimal >();
            this.AccountId = jToken[ "account_id" ].Value< String >();
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public String OrderId { get; set; }
        public Decimal Amount { get; set; }
        public String AccountId { get; set; }
    }
}
