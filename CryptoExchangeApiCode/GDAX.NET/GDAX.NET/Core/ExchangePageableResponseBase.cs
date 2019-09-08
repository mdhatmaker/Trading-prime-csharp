namespace GDAX.NET.Core {
    using System;
    using System.Linq;

    public abstract class ExchangePageableResponseBase {

        protected ExchangePageableResponseBase( ExchangeResponse response ) {

            var beforeHeader = response.Headers.LastOrDefault( x => x.Key != null && x.Key.ToUpper() == "CB-BEFORE" );
            var afterHeader = response.Headers.LastOrDefault( x => x.Key != null && x.Key.ToUpper() == "CB-AFTER" );

            if ( beforeHeader.Value != null ) {
                this.BeforePaginationToken = beforeHeader.Value.LastOrDefault();
            }
            if ( afterHeader.Value != null ) {
                this.AfterPaginationToken = afterHeader.Value.LastOrDefault();
            }
        }

        public String BeforePaginationToken { get; set; }
        public String AfterPaginationToken { get; set; }
    }
}
