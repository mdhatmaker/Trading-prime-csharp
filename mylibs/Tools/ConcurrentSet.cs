using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Tools
{
    // Simulate a ConcurrentSet by using the ConcurrentDictionary object
    public class ConcurrentSet<T> : IEnumerable<T>
    {
        public int Count => m_dict.Count;

        private ConcurrentDictionary<T, byte> m_dict = new ConcurrentDictionary<T, byte>();

        public void Add(T value)
        {
            m_dict[value] = 0;
        }

        public void Remove(T value)
        {
            m_dict.TryRemove(value, out byte b);
        }

        public bool Contains(T value)
        {
            return m_dict.ContainsKey(value);
        }

        public void Clear()
        {
            m_dict.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_dict.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_dict.Keys.GetEnumerator();
        }
    } // end of class ConcurrentSet
} // end of namespace
