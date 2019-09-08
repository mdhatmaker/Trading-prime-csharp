using System;
using System.IO;

namespace CryptoTools.CryptoFile
{
    public static class Folder
    {
		public static string charts_folder => Path.Combine(m_root, "CHARTS");
		public static string crypto_folder => Path.Combine(m_root, "DF_CRYPTO");
		public static string data_folder => Path.Combine(m_root, "DF_DATA");
		public static string quandl_folder => Path.Combine(m_root, "DF_QUANDL");
		public static string excel_folder => Path.Combine(m_root, "EXCEL");
		public static string misc_folder => Path.Combine(m_root, "MISC");
		public static string project_folder => Path.Combine(m_root, "projects");
		public static string rawdata_folder => Path.Combine(m_root, "RAW_DATA");
		public static string system_folder => Path.Combine(m_root, "SYSTEM");

		private static string m_root;

        static Folder()
        {
            //m_root = "/Users/michael/Dropbox/alvin/data";
            //m_root = @"X:\Users\Trader\Dropbox\alvin\data";
            //var path = GFile.GetAssemblyPath(typeof(Folder));
            //path = GFile.AssemblyDirectory;
            var path = GFile.ExePath;
            var pathname = Path.Combine(path, "data.path.txt");
            m_root = GFile.GetString(pathname) ?? "";
        }

    } // end of class Folder

} // end of namespace
