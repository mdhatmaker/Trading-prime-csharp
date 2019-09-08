using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using static Tools.GFile;
using static Tools.G;

namespace Tools
{
    public class DataFrame
    {
        public DataFrameColumnCollection Columns => m_columns;
        public DataFrameRowCollection Rows => m_rows;
        
        private DataFrameColumnCollection m_columns = new DataFrameColumnCollection();
        private DataFrameRowCollection m_rows = new DataFrameRowCollection();

        private string m_pathname;
        private bool m_createIndex;
        private char m_separatorChar;

        public static DataFrame Empty { get { return new DataFrame(); } }

        // CTOR: Create a new empty DataFrame
        public DataFrame()
        {
        }

        // CTOR: Create a new DataFrame given a comma-delimited string of column names
        public DataFrame(string columns, char ch=',') : this(columns.Split(new char[] { ch }, StringSplitOptions.RemoveEmptyEntries))
        {
        }

        // CTOR: Create new DataFrame given an array of column names
        public DataFrame(string[] columns) : base()
        {   
            foreach (string c in columns)
            {
                m_columns.Add(new DataFrameColumn(c));
            }
        }

        // Given an integer index
        // Return the DataFrameRow at that index
        public DataFrameRow this[int ix]
        {
            get
            {
                return m_rows[ix];
            }
        }

        // Given a string key (value in FIRST COLUMN of DataFrameRow)
        // Return the DataFrameRow at that index
        public DataFrameRow this[string rowKey]
        {
            get
            {
                return m_rows[rowKey];
            }
        }

        // Given a string key (value in FIRST COLUMN of DataFrameRow) AND a column name
        // Return the (string) value at that row/column
        public string this[string rowKey, string colKey]
        {
            get
            {
                //string result = null;
                int ci = m_columns[colKey].Index;   // .ColumnIndex(colKey);
                return m_rows[rowKey][ci];
            }
        }

        // Given a string array of values
        // Add a new DataFrameRow containing these values to the DataFrame
        public void AddRowValues(string[] row, bool createIndex=false)
        {
            for (int i = 0; i < row.Length; ++i)
            {
                row[i] = row[i].Trim();                             // remove any leading/trailing spaces in each row value
            }                
            m_rows.Add(new DataFrameRow(row), createIndex);
        }

        public override string ToString()
        {
            return string.Format("DataFrame: {0} rows x {1} columns", m_rows.Count, m_columns.Count);
        }

        // A quick-and-dirty way to print DataFrame objects
        public void print()
        {
            cout(m_columns.ToString());
            cout(m_rows.ToString());
        }

        // Given a directory path
        // Return a string array containing the DataFrame (*.DF.csv) files in that directory
        public static string[] GetDataFrameFileList(string path)
        {
            return GetFilesEndingWith(path, ".DF.csv");
        }

        // Given the pathname of a DataFrame file
        // Read the DataFrame file and return a populated DataFrame object
        public static DataFrame ReadDataFrame(string pathname, bool createIndex=false, char separatorChar=',')
        {
            DataFrame df = DataFrame.Empty;

            try
            {
                using (StreamReader reader = new StreamReader(pathname))
                {
                    string line;

                    if ((line = reader.ReadLine()) == null)
                        return DataFrame.Empty;
                    else
                        df = new DataFrame(line, separatorChar);

                    while ((line = reader.ReadLine()) != null)
                    {
                        // "fancier" RegEx code if we need it at some point (but for now we'll just split by commas)
                        //Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");        //Define pattern
                        //string[] X = CSVParser.Split(line);                                           //Separating columns to array
                        if (string.IsNullOrWhiteSpace(line))
                            dout("(ignoring blank line in dataframe file '{0}')", pathname);
                        else
                            df.AddRowValues(line.Split(separatorChar), createIndex);
                    }
                }
                df.m_pathname = pathname;
                df.m_createIndex = createIndex;
                df.m_separatorChar = separatorChar;
            }
            catch (Exception ex)
            {
                ErrorMessage("Error reading DataFrame file '{0}': {1}", pathname, ex.Message);
            }
            return df;
        }

        // Reload data from a previously loaded DataFrame file
        public void Reload()
        {
            if (m_pathname == null) return;

            var df = ReadDataFrame(m_pathname, m_createIndex, m_separatorChar);
            this.m_columns = df.m_columns;
            this.m_rows = df.m_rows;
        }

    } // end of class DataFrame

    //------------------------------------------------------------------------------------------------------------------

    public class DataFrameColumn
    {
        public string Name => m_columnName;
        public int Index => m_columnIndex;

        private string m_columnName = "";
        private int m_columnIndex = -1;

        /*public DataFrameColumn()
        {
        }*/

        public DataFrameColumn(string columnName)
        {
            m_columnName = columnName;
        }

        /*public string this[int ix] { get { return m_rowValues[ix]; } }*/

        public void SetIndex(int cix)
        {
            m_columnIndex = cix;
        }

        public override string ToString()
        {
            return m_columnName;
        }
    } // end of class DataFrameColumn

    public class DataFrameColumnCollection : IEnumerable<DataFrameColumn>
    {
        public int Count => m_columns.Count;

        private OrderedDictionary m_columns = new OrderedDictionary();
        private Dictionary<string, int> m_columnIndex = new Dictionary<string, int>();

        //private DataFrameColumnCollection() { }

        public DataFrameColumnCollection(bool createIndexColumn = false)
        {
            if (createIndexColumn) Add(new DataFrameColumn("Index"));
        }

        // Can access the columns in the collection by either (1) the integer column index or (2) the key, which is the NAME of the column
        public DataFrameColumn this[int ix] { get { return m_columns[ix] as DataFrameColumn; } }
        public DataFrameColumn this[string key] { get { return m_columns[key] as DataFrameColumn; } }

        // The name of each column (columnKey) should be unique
        public void Add(DataFrameColumn column)
        {
            int ix = m_columns.Count;
            column.SetIndex(ix);
            m_columnIndex[column.Name] = ix;       // store the integer index for this column name
            m_columns.Add(column.Name, column);                 // store this DataFrameColumn
        }

        /*// Return the integer index for the column with the specified name
        public int ColumnIndex(string columnName)
        {
            return m_columnIndex[columnName];
        }*/

        public override string ToString()
        {
            //return string.Join(",", m_columns.Keys);
            StringBuilder sb = new StringBuilder();
            foreach (string k in m_columns.Keys)
            {
                if (sb.Length > 0) sb.Append(",");
                sb.Append('"');
                sb.Append(m_columns[k]);
                sb.Append('"');
            }
            return sb.ToString();
        }

        #region IEnumerable
        public IEnumerator<DataFrameColumn> GetEnumerator()
        {
            foreach (DataFrameColumn column in m_columns.Values)
            {
                yield return column;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

    } // end of class DataFrameColumnCollection

    public class DataFrameRow
    {
        private List<string> m_rowValues = new List<string>();

        public DataFrameRow()
        {
        }

        public DataFrameRow(string[] values)
        {
            m_rowValues.AddRange(values);
        }

        public string this[int ix] { get { return m_rowValues[ix]; } }

        public override string ToString()
        {
            return string.Join(",", m_rowValues);
        }
    } // end of class DataFrameRow

    public class DataFrameRowCollection : IEnumerable<DataFrameRow>
    {
        public int Count => m_rows.Count;

        private OrderedDictionary m_rows = new OrderedDictionary();

        public DataFrameRowCollection()
        {
        }

        // Can access the rows in the collection by either (1) the integer row index or (2) the key, which is the value of the FIRST COLUMN in each row
        public DataFrameRow this[int ix] { get { return m_rows[ix] as DataFrameRow; } }
        public DataFrameRow this[string key] { get { return m_rows[key] as DataFrameRow; } }

        // The value in the first column (rowKey) should be unique, so pass createIndex=true if it is NOT unique
        public void Add(DataFrameRow row, bool createIndex=false)
        {
            if (createIndex == true)
                m_rows.Add(m_rows.Count, row);
            else
                m_rows.Add(row[0], row);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var k in m_rows.Keys)
            {
                sb.Append("[");
                sb.Append(string.Join(",", m_rows[k]));
                sb.Append("]");
            }
            return sb.ToString();
        }

        #region IEnumerable
        public IEnumerator<DataFrameRow> GetEnumerator()
        {
            foreach (DataFrameRow row in m_rows.Values)
            {
                yield return row;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

    } // end of class DataFrameRowCollection

} // end of namespace
