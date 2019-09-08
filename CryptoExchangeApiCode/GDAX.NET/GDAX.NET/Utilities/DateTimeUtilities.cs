namespace GDAX.NET.Utilities {
    using System;

    public static class DateTimeUtilities {

        public static readonly DateTime UnixEpoch = new DateTime( year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc );

        /// <summary>
        /// Seconds since January 1st, 1970.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static Double ToUnixTimestamp( this DateTime dateTime ) {
            return ( dateTime - UnixEpoch ).TotalSeconds;
        }
    }
}
