using System;
using System.Collections.Generic;
using System.IO;
using CryptoTools.CryptoSystem;
using CsvHelper;

namespace CryptoTools.CryptoFile
{
	public class InputFile<T> : IDisposable
    {
		public string CsvHeaders => GFile.GetCsvHeaders<T>();
        
		private StreamReader m_reader;
        private CsvReader m_csv;

        // CTOR
		public InputFile(string filename, string path = null, bool addTimestamp = false)
		{
            string pathname;

            if (path == null)
                pathname = Path.Combine(GFile.ExePath, filename);                   // this adds nothing to the filename
            else
                pathname = GFile.GetDfCsvPathname(filename, path, addTimestamp);    // this automatically adds ".DF.csv" to the filename

            try
            {
                m_reader = new StreamReader(pathname);
                m_csv = new CsvReader(m_reader);

                // TODO: FIX THE READING OF HEADERS AND SUBSEQUENT RECORDS!!!
                //string headers = this.CsvHeaders;
                //string headers = m_reader.ReadLine();
                m_csv.Read();
                m_csv.ReadHeader();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}\nPress any key (Ctrl-C to exit)", ex.Message);
                Console.ReadKey();
            }
		}

        public T Read()
        {
            m_csv.Read();
            return m_csv.GetRecord<T>();
        }

        public IEnumerable<T> ReadAll()
        {
            while (m_csv.Read())
            {
                var record = m_csv.GetRecord<T>();
                yield return record;
            }
        }

        public string ReadLine()
		{
			return m_reader.ReadLine();
		}

        public void Close()
		{
			m_reader.Close();
		}

        // IDisposable implementation
		public void Dispose()
		{
			m_reader.Dispose();
		}

	} // end of class InputFile
} // end of namespace
