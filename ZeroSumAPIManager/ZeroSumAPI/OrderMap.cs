using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSumAPI
{
    public class OrderMap<Tkey, Tvalue>
    {
        private Dictionary<Tkey, Tvalue> m_orders;              // orders indexed by the api key returning the api order
        private Dictionary<uint, Tvalue> m_oidOrders;           // orders indexed by the ZeroSumAPI uint OID returning the api order
        private Dictionary<Tkey, uint> m_keyToOid;
        private Dictionary<uint, Tkey> m_OidToKey;

        public OrderMap()
        {
            m_orders = new Dictionary<Tkey, Tvalue>();
            m_oidOrders = new Dictionary<uint, Tvalue>();
            m_keyToOid = new Dictionary<Tkey, uint>();
            m_OidToKey = new Dictionary<uint, Tkey>();
        }

        public Tvalue this[Tkey key]
        {
            get
            {
                return m_orders[key];
            }
            /*set
            {
                m_orders[key] = value;
            }*/
        }

        public Tvalue this[uint oid]
        {
            get
            {
                return m_oidOrders[oid];
            }
            /*set
            {
                m_oidOrders[oid] = value;
            }*/
        }

        public void Add(Tkey key, Tvalue value, uint oid)
        {
            m_orders[key] = value;
            m_oidOrders[oid] = value;
            m_keyToOid[key] = oid;
            m_OidToKey[oid] = key;
        }

        public Tkey GetKey(uint oid)
        {
            return m_OidToKey[oid];
        }

        public uint GetOid(Tkey key)
        {
            return m_keyToOid[key];
        }

    } // end of class
} // end of namespace
