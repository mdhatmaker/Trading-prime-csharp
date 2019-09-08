using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using System.Collections;

namespace CryptoTools.Models
{
    public class XOrderList : IEnumerable<XOrder>
    {
        private ConcurrentBag<XOrder> m_orders;

        public XOrderList()
        {
            m_orders = new ConcurrentBag<XOrder>();
        }

        public XOrder GetOrder(string orderId)
        {
            return m_orders.Where(o => o.OrderId == orderId).FirstOrDefault();
        }

        public void Add(XOrder o)
        {
            m_orders.Add(o);
        }

        public IEnumerator<XOrder> GetEnumerator()
        {
            return m_orders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    } // end of class OrderList

} // namespace
