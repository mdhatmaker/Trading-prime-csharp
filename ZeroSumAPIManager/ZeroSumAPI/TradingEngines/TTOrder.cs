using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTechnologies.TTAPI;
using static Tools.G;

namespace ZeroSumAPI
{
    // Define the product (see TTAPI.ProductKey) AND a specific contract (ex: "Dec17")
    // (the actual Instrument will be updated when the FoundInstrument method is called)
    public class TTOrder
    {
        //public string Key { get; private set; }
        public string Key { get { return m_orderProfile.SiteOrderKey; } }
        public OrderProfile Profile { get { return m_orderProfile; } }
        public Order Order { get { return m_order; } }

        //private string m_orderKey;
        private OrderProfile m_orderProfile;
        private Order m_order;

        public TTOrder(OrderProfile op)
        {
            //m_orderKey = op.SiteOrderKey;
            m_orderProfile = op;
            m_order = null;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} {2}", this.Key, this.Profile, this.Order);
        }

        public void AddedOrder(Order order)
        {
            dout("ADDED ORDER: {0}", order.ToString());
            m_order = order;
        }

    } // end of class
} // end of namespace
