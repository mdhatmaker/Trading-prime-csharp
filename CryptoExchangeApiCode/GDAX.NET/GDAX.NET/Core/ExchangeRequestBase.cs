namespace GDAX.NET.Core {

    using System;
    using Utilities;

    public abstract class ExchangeRequestBase {

        protected ExchangeRequestBase( String method ) {
            this.Method = method;
            this.TimeStamp = DateTime.UtcNow.ToUnixTimestamp();
        }

        /// <summary>
        /// More than 30 seconds off?
        /// </summary>
        public Boolean IsExpired => this.GetCurrentUnixTimeStamp() - this.TimeStamp >= 30;

        public String Method {
            get; private set;
        }

        public String RequestBody {
            get; protected set;
        }

        public String RequestUrl {
            get; protected set;
        }

        public Double TimeStamp {
            get;
        }

        protected virtual Double GetCurrentUnixTimeStamp() {
            return DateTime.UtcNow.ToUnixTimestamp();
        }
    }
}