using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CryptoTools.CryptoSystem;

namespace CryptoTools.CryptoFile
{
    public static class GFile
    {
        // is this the same as ExePath below?
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        // Get the path of an assembly containing a specified Type
        public static string GetAssemblyPath(Type type)
        {
            // get the full location of the assembly with DaoTests in it
            string fullPath = System.Reflection.Assembly.GetAssembly(type).Location;
            // get the folder that's in
            string theDirectory = Path.GetDirectoryName(fullPath);
            return theDirectory;
        }

        // Get path of folder containinng the currently executing assembly
        public static string ExePath => AppDomain.CurrentDomain.BaseDirectory;        

        // Given the filename of a text file (in the current assembly's directory)
        // Return the text from this single-line file
        public static string GetString(string filename, string defaultValue = "")
        {
            string pathname = Path.Combine(GFile.ExePath, filename);
            try
            {
                using (var reader = new StreamReader(pathname))
                {
                    var line = reader.ReadLine();
                    return (line == null ? defaultValue : line.Trim());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot read from file '{0}' ({1})", pathname, ex.Message);
                return defaultValue;
            }
        }

        // TODO: REMOVE THIS STATIC METHOD IN FAVOR OF OutputFile CLASS
        // Create an output file (StreamWriter)
        // where name like "tickers" (date/time and "DF.csv" will be automatically appended)
        // where headers like "DateTime,Symbol,Bid,Ask,Volume" (null for NO headers to output file)
        public static StreamWriter CreateOutputFile(string name, string csvHeaders = null, bool append = false)
        {
			StreamWriter writer;
            
            //var loc = Assembly.GetExecutingAssembly().Location;
            if (append)
            {
                writer = new StreamWriter(name, append: true);
            }
            else
            {
				string filename = string.Format("{0}_{1}.DF.csv", name, DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));
                name = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
                writer = new StreamWriter(name, append: false);
                if (csvHeaders != null) writer.WriteLine(csvHeaders);
            }

			return writer;
        }

		// where filename like "tickers" (".DF.csv" will be added automatically)
        // where path like "/Users/david/Documents" (null path will use current Assembly's directory)
        public static string GetDfCsvPathname(string filename, string path = null, bool addTimestamp = true, bool append = false)
        {
            string timestamp = ".DF.csv";
            if (addTimestamp) timestamp = string.Format("_{0}.DF.csv", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
            filename += timestamp;

            string pathname;
            if (path == null)
                pathname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            else
                pathname = Path.Combine(path, filename);

            return pathname;
        }

        public static string GetCsvHeaders<T>()
		{
			var props = Reflection.GetProperties<T>();
			return string.Join(",", props.Select(p => p.Name));
		}

		public static string GetCsv<T>(T obj) where T : class
		{
			var pvalues = Reflection.GetPropertyValues<T>(obj);
            return string.Join(",", pvalues);
		}

    } // end of class GFile

} // end of namespace
