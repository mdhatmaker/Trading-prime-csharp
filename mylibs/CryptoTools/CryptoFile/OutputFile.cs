using System;
using System.Collections.Generic;
using System.IO;
using CryptoTools.CryptoSystem;
using CsvHelper;

namespace CryptoTools.CryptoFile
{
	public class OutputFile<T> : IDisposable
    {
		public string CsvHeaders => GFile.GetCsvHeaders<T>();
		//public string ToCsv() => GFile.GetCsv<T>();
		//public StreamWriter Writer => m_writer;

		private StreamWriter m_writer;
        private CsvWriter m_csv;

        // CTOR
		public OutputFile(string filename, string path = null, bool addTimestamp = true, bool append = false)
		{
            if (path == null) path = GFile.ExePath;

			string pathname = GFile.GetDfCsvPathname(filename, path, addTimestamp, append);
			//m_writer = new StreamWriter(pathname, append);

			if (append)
            {
				m_writer = new StreamWriter(pathname, append: true);
                m_csv = new CsvWriter(m_writer);
            }
            else
            {
                m_writer = new StreamWriter(pathname, append: false);
				//string headers = this.CsvHeaders;
				//m_writer.WriteLine(headers);
                m_csv = new CsvWriter(m_writer);
                m_csv.WriteHeader<T>();
                m_csv.NextRecord();
            }
		}

		public void Write(T record)
		{
			m_csv.WriteRecord(record);
		}

        public void WriteLine(object obj)
		{
			m_writer.WriteLine(obj);
		}

        public void WriteAll(IEnumerable<T> records)
        {
            m_csv.WriteRecords(records);
        }

        public void Close()
		{
			m_writer.Close();
		}

        // IDisposable implementation
		public void Dispose()
		{
			m_writer.Dispose();
		}

	} // end of class OutputFile
} // end of namespace
