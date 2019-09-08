using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tools.G;

namespace Tools
{
    public class ZCandlestickMap : IEnumerable<KeyValuePair<DateTime, ZCandlestick>>, IEnumerator<KeyValuePair<DateTime, ZCandlestick>>
    {
        public CryptoExch Exch { get { return m_exch; } }
        public string Symbol { get { return m_symbol; } }
        public string Interval { get { return m_interval; } }

        public int Count { get { return m_map.Count(); } }

        private SortedDictionary<DateTime, ZCandlestick> m_map = new SortedDictionary<DateTime, ZCandlestick>();
        private CryptoExch m_exch;
        private string m_symbol;
        private string m_interval;

        public ZCandlestickMap()
        {
            // Ctor
        }

        public ZCandlestickMap(CryptoExch exch, string symbol, string interval)
        {
            m_exch = exch;
            m_symbol = symbol;
            m_interval = interval;
        }

        public void Add(DateTime dt, ZCandlestick zc)
        {
            m_map.Add(dt, zc);
        }

        public bool ContainsKey(DateTime dt)
        {
            return m_map.ContainsKey(dt);
        }

        public ZCandlestick this[DateTime dt]
        {
            get
            {
                ZCandlestick zc;
                if (m_map.TryGetValue(dt, out zc))
                {
                    return zc;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                m_map[dt] = value;
            }
        }

        object IEnumerator.Current => m_map.GetEnumerator().Current;

        public KeyValuePair<DateTime, ZCandlestick> Current
        {
            get
            {
                try
                {
                    return m_map.GetEnumerator().Current;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<DateTime, ZCandlestick>> GetEnumerator()
        {
            return (IEnumerator<KeyValuePair<DateTime, ZCandlestick>>)m_map.GetEnumerator();
        }

        public bool MoveNext()
        {
            //m_position++;
            //return m_position < m_map.Count;
            return m_map.GetEnumerator().MoveNext();
        }

        public void Reset()
        {
            //m_position = -1;
            //m_enumerator = m_map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_map.GetEnumerator();
        }

        // Given the pathname to a specific dataframe file ("*.DF.csv")
        // Read the data into a ZCandlestickMap
        public static ZCandlestickMap ReadFromFile(string pathname)
        {
            try
            {
                string filename = Path.GetFileName(pathname);
                ZCandlestickMap candlesticks;
                using (var f = new StreamReader(pathname))
                {
                    var split = filename.Split('.');
                    if (filename.EndsWith(".DF.csv") && split.Length == 5)
                    {
                        var exch = (CryptoExch) Enum.Parse(typeof(CryptoExch), split[0], ignoreCase: true);
                        string symbol = split[1];
                        string interval = split[2];
                        candlesticks = new ZCandlestickMap(exch, symbol, interval);
                    }
                    else if (filename.EndsWith(".DF.csv") && split.Length == 4)
                    {
                        //var exch = (CryptoExch)Enum.Parse(typeof(CryptoExch), split[0], ignoreCase: true);
                        var exch = CryptoExch.VAULTORO;
                        string symbol = split[0];
                        string interval = split[1];
                        candlesticks = new ZCandlestickMap(exch, symbol, interval);
                    }
                    else
                    {
                        candlesticks = new ZCandlestickMap();
                    }
                    // Read column headers from first line of DataFrame file (ex: "DateTime,Open,High,Low,Close,Volume")
                    string[] columns = f.ReadLine().Split(',');
                    // Read data from file
                    string line;
                    while ((line = f.ReadLine()) != null)
                    {
                        var zc = new ZCandlestick(line, columns);
                        candlesticks[zc.time] = zc;
                    }
                }
                return candlesticks;
            }
            catch (Exception ex)
            {
                ErrorMessage("ZCandlestickMap::ReadFromFile => Cannot read from file: {0}", ex.Message);
                return null;
            }
        }

        // Write this ZCandlestickMap object data to a dataframe file ("*.DF.csv")
        public bool WriteToFile()
        {
            if (m_symbol == null)
            {
                ErrorMessage("ZCandlestickMap::WriteToFile => Cannot write to file: No symbol provided.");
                return false;
            }
            var filename = string.Format("{0}.{1}.{2}.DF.csv", m_exch, m_symbol, m_interval);
            var pathname = Folders.crypto_path(filename);
            cout("ZCandlestickMap::WriteToFile => Writing to file: '{0}'", pathname);
            using (var f = new StreamWriter(pathname))
            {
                f.WriteLine("DateTime,Open,High,Low,Close,Volume");
                foreach (var kv in m_map)
                {
                    f.WriteLine("{0},{1},{2},{3},{4},{5}", kv.Value.time.ToLocalTime(), kv.Value.open, kv.Value.high, kv.Value.low, kv.Value.close, kv.Value.volume);
                }
            }
            return true;
        }
    } // end of class ZCandlestickMap


    public class ZCandlestick
    {
        protected decimal o, h, l, c, v;
        protected decimal vw;
        protected DateTime t;
        protected int n;

        public virtual decimal open { get { return o; } set { o = value; } }
        public virtual decimal high { get { return h; } set { h = value; } }
        public virtual decimal low { get { return l; } set { l = value; } }
        public virtual decimal close { get { return c; } set { c = value; } }
        public virtual DateTime time { get { return t; } set { t = value; } }
        public virtual decimal volume { get { return v; } private set { v = value; } }
        public virtual decimal vwap { get { return vw; } private set { vw = value; } }
        public virtual int count { get { return n; } }

        public static string ColumnNames => "DateTime,Open,High,Low,Close,Volume";
        public virtual string ColumnText => string.Format("{0},{1},{2},{3},{4},{5}", time.ToDateTimeString(), open, high, low, close, volume);

        public ZCandlestick() { }

        public ZCandlestick(decimal o, decimal h, decimal l, decimal c, int timestamp)
        {
            this.o = o;
            this.h = h;
            this.l = l;
            this.c = c;
            this.t = timestamp.ToDateTime();
        }

        // Parse line from file in format: "DateTime,Open,High,Low,Close,Volume"
        public ZCandlestick(string lineFromFile, string[] columns)
        {
            int i;
            var split = lineFromFile.Split(',');
            i = ColumnIndex(columns, "Open");
            this.o = decimal.Parse(split[i]);
            i = ColumnIndex(columns, "High");
            this.h = decimal.Parse(split[i]);
            i = ColumnIndex(columns, "Low");
            this.l = decimal.Parse(split[i]);
            i = ColumnIndex(columns, "Close");
            this.c = decimal.Parse(split[i]);
            i = ColumnIndex(columns, "DateTime");
            this.t = DateTime.Parse(split[i]);
        }

        // Given column header text, return the (int) index of the associated column
        private int ColumnIndex(string[] columns, string columnHeader)
        {
            int result = -1;
            for (int i = 0; i < columns.Length; ++i)
            {
                if (columns[i].ToUpper() == columnHeader.ToUpper())
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        // Return the high/low/open/close values for this candlestick (useful for charting with MSChart)
        public double[] yValues
        {
            get
            {
                return new double[] { (double)high, (double)low, (double)open, (double)close };
            }
        }

        public override string ToString()
        {
            return string.Format("[OHLC:{0}]  o:{1} h:{2} l:{3} c:{4} volume:{5} vwap:{6}  {7}", count, open, high, low, close, volume, vwap, time.ToDateTimeString());
        }
    } // end of class ZCandlestick

} // end of namespace
