using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi.Model
{
    public class Order
    {
        /// <summary>
        /// Order ID on the exchange
        /// </summary>
        public int orderId { get; set; }

        /// <summary>
        /// Order status
        /// new, partiallyFilled, filled, canceled, expired, rejected
        /// </summary>
        public string orderStatus { get; set; }

        /// <summary>
        /// UTC timestamp of the last change, in milliseconds
        /// </summary>
        public long lastTimestamp { get; set; }

        /// <summary>
        /// Order price
        /// </summary>
        public string orderPrice { get; set; }

        /// <summary>
        /// Order quantity, in lots
        /// </summary>
        public int orderQuantity { get; set; }

        /// <summary>
        /// Average price	
        /// </summary>
        public string avgPrice { get; set; }

        /// <summary>
        /// Remaining quantity, in lots
        /// </summary>
        public int quantityLeaves { get; set; }

        /// <summary>
        /// Type of an order
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Time in force
        /// GTC - Good-Til-Canceled, IOC - Immediate-Or-Cancel, OK - Fill-Or-Kill, DAY - day
        /// </summary>
        public string timeInForce { get; set; }

        /// <summary>
        /// Cumulative quantity
        /// </summary>
        public int cumQuantity { get; set; }

        /// <summary>
        /// Unique client-generated ID
        /// </summary>
        public string clientOrderId { get; set; }

        /// <summary>
        /// Currency symbol
        /// </summary>
        public string symbo { get; set; }

        /// <summary>
        /// Side of a trade
        /// </summary>
        public string side { get; set; }

        /// <summary>
        /// Last executed quantity, in lots
        /// </summary>
        public int execQuantity { get; set; }

    }

    public class Orders
    {
        public List<Order> orders { get; set; }
    }
}
