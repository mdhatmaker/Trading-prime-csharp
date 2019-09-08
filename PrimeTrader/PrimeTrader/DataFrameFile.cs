using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PrimeTrader
{
    public class DataFrameFile
    {
        public string Pathname { get; private set; }
        public string Filename { get { return Path.GetFileName(this.Pathname); } }
        public string[] Columns { get; private set; }
        public string[] Rows { get; private set; }
        public int ColumnCount { get { return this.Columns.Length; } }
        public int RowCount { get { return this.Rows.Length; } }
        public string FirstRowIndex { get; private set; }
        public string LastRowIndex { get; private set; }
        public bool IsIndexDateTime { get { return m_isIndexDateTime; } }                   // if first column is DateTime IsIndexDateTime is true and we will populate the m_rowDates array
        public string FirstIndexString { get { return m_firstIndexString; } }
        public string LastIndexString { get { return m_lastIndexString; } }
        public DateTime[] RowDates { get { return m_rowDates; } }

        bool m_isIndexDateTime;
        string m_firstIndexString;
        string m_lastIndexString;

        DateTime[] m_rowDates;

        public DataFrameFile(string pathname)
        {
            this.Pathname = pathname;

            this.Columns = GetColumns();
            this.Rows = GetRows();

            var first_row = Head(1);
            if (first_row.Count > 0)
                this.FirstRowIndex = first_row[0][0];
            var last_row = Tail(1);
            if (last_row.Count > 0)
                this.LastRowIndex = last_row[0][0];

            ReadIndexColumn();
        }

        private void ReadIndexColumn()
        {
            m_rowDates = null;
            DateTime dt;
            if ((this.RowCount > 0) && (DateTime.TryParse(this.FirstRowIndex, out dt)))
            {
                m_isIndexDateTime = true;

                // Parse each of the row values into DateTime
                var rowDates = new List<DateTime>();
                foreach (string str in this.Rows)
                {
                    rowDates.Add(DateTime.Parse(str));
                }
                m_rowDates = rowDates.ToArray();

                m_firstIndexString = DateTime.Parse(this.FirstRowIndex).ToShortDateString();
                m_lastIndexString = DateTime.Parse(this.LastRowIndex).ToShortDateString();
            }
            else
            {
                m_isIndexDateTime = false;

                m_firstIndexString = "1";
                m_lastIndexString = this.RowCount.ToString();
            }
        }

        // Given a (1-based, not 0-based) integer row number
        // Return a string representing the index at that row (DateTime if IsIndexDateTime is true, otherwise the integer row number)
        public string GetIndexString(int rowNumber)
        {
            if (m_isIndexDateTime)
                return (rowNumber > 0 && rowNumber <= this.RowCount) ? m_rowDates[rowNumber - 1].ToShortDateString() : "?";
            else
                return rowNumber.ToString();
        }

        public List<string[]> Head(int rowCount)
        {
            var first_rows = ReadRows(1, rowCount);
            return first_rows;
        }

        public List<string[]> Tail(int rowCount)
        {
            int r1 = Math.Max(this.RowCount - rowCount, 1);
            var last_rows = ReadRows(r1, this.RowCount);
            return last_rows;
        }

        private string[] GetColumns()
        {
            var rows = ReadRows(0, 0);
            return rows[0];
        }

        public int GetColumnIndex(string columnName)
        {
            return Array.IndexOf(Columns, columnName);
        }

        private string[] GetRows()
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(this.Pathname))
            {
                string line;
                line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] columns = line.Trim().Split(',');
                    //rows.Add(DateTime.Parse(columns[0]));
                    rows.Add(columns[0]);
                }
            }
            return rows.ToArray();
        }

        public List<string[]> ReadRows(int ixStart = 0, int ixEnd = int.MaxValue)
        {
            List<string[]> rows = new List<string[]>();
            string line = null;

            int i = 0;
            using (StreamReader sr = new StreamReader(this.Pathname))
            {
                while (i < ixStart)
                {
                    line = sr.ReadLine();
                    ++i;
                }

                while (i <= ixEnd)
                {
                    line = sr.ReadLine();
                    if (line == null) break;
                    
                    //Console.WriteLine(line);
                    string[] columns = line.Trim().Split(',');
                    rows.Add(columns);
                    ++i;
                }
            }
            return rows;
        }

        public string GetTemporaryExcelFilename()
        {
            string name = Path.GetFileNameWithoutExtension(this.Pathname);
            string ext = Path.GetExtension(this.Pathname);
            return string.Format("{0}.EXCEL{1}", name, ext);
        }

        public void CopyTo(string pathname)
        {
            File.Copy(this.Pathname, pathname, true);           // copy WITH overwrite
        }

    } // end of CLASS
} // end of NAMESPACE
