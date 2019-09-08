using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoApis.WebsocketApi
{
    
    public class GeminiEvent
    {
        public string type { get; set; }                // "change"
        public string side { get; set; }                // "bid"
        public decimal price { get; set; }              // "9250.37"
        public decimal remaining { get; set; }          // "0.67333675"
        public decimal delta { get; set; }              // "-2.491"
        public string reason { get; set; }              // "initial", "cancel", "place"

        public override string ToString()
        {
            return string.Format("[{0} {1} {2} {3} {4} {5}]", type, side, price, remaining, delta, reason);
        }
    }

    public class GeminiUpdate
    {
        public string type { get; set; }                // "update"
        public long eventId { get; set; }               // 3608804479
        public long timestamp { get; set; }             // 1525102643
        public long timestampms { get; set; }           // 1525102643396
        public int socket_sequence { get; set; }        // 0, 1, 2, 3, ....
        public List<GeminiEvent> events { get; set; }

        public override string ToString()
        {
            return string.Format("<{0}> {1}  {2}/{3}  ({4}) {5}", socket_sequence, type, timestamp, timestampms, events.Count, events[0]);
        }
    }

} // end of namespace
