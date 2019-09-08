using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
//using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Drawing;
using System.Xml;
using static Tools.G;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace Tools
{
    public static class GFile
    {
        static public string GetExecutableFilename()
        {
            return System.AppDomain.CurrentDomain.FriendlyName;
        }

        static public string GetExecutablePath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }

        static public string GetExecutablePathname()
        {
            return Path.Combine(GetExecutablePath(), GetExecutableFilename());
        }

        public static List<string> ReadTextFileLines(string pathname)
        {
            List<string> lines = new List<string>();

            var f = File.OpenText(pathname);
            string line = f.ReadLine();
            while (line != null)
            {
                lines.Add(line);
                line = f.ReadLine();
            }

            f.Close();
            return lines;
        }

        // Given the full pathname to a comma-delimted text file
        // Return a string array containing the lines in the file
        public static string[] ReadCsvFileLines(string pathname)
        {
            List<string> lines = new List<string>();                // List of strings for the data we read in
            if (File.Exists(pathname))                              // If the csv text file exists, then read it
            {
                using (var reader = File.OpenText(pathname))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line.Trim());
                    }
                }
            }
            return lines.ToArray();
        }

        public static string ReadTextFile(string pathname, bool createIfNotExist = false)
        {
            try
            {
                // Do we create the file if it does not exist?
                if (createIfNotExist && !File.Exists(pathname))
                {
                    var fs = File.OpenWrite(pathname);
                    fs.Close();
                }
                // Open the text file and read all the text
                using (var f = File.OpenText(pathname))
                {
                    return f.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("Error occurred attempting to read text file: '{0}': {1}", pathname, ex.Message);
            }
            return null;
        }

        private static XmlDocument ReadXMLFile(string xmlFilename, bool unzip=false)
        {
            var xmldoc = new System.Xml.XmlDocument();
            FileStream fs = new FileStream(xmlFilename, FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            return xmldoc;

            /*System.Xml.XmlNodeList xmlnode;
            string str = null;
            int i = 0;            
            xmlnode = xmldoc.GetElementsByTagName("Exchange");
            for (i = 0; i <= xmlnode.Count - 1; i++)
            {
                xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                str = xmlnode[i].ChildNodes.Item(0).InnerText.Trim() + "  " + xmlnode[i].ChildNodes.Item(1).InnerText.Trim();   // + "  " + xmlnode[i].ChildNodes.Item(2).InnerText.Trim();
                System.Console.WriteLine(str);
            }*/
        }

        private static void ReadEncryptedFile()
        {
            throw new NotImplementedException();

            // This is where the data will be written to.
            MemoryStream dataStream = new MemoryStream();

            // The encryption vectors
            byte[] key = { 145, 12, 32, 245, 98, 132, 98, 214, 6, 77, 131, 44, 221, 3, 9, 50 };
            byte[] iv = { 15, 122, 132, 5, 93, 198, 44, 31, 9, 39, 241, 49, 250, 188, 80, 7 };

            // Build the encryption mathematician
            using (TripleDESCryptoServiceProvider encryption = new TripleDESCryptoServiceProvider())
            using (ICryptoTransform transform = encryption.CreateEncryptor(key, iv))
            using (Stream encryptedOutputStream = new CryptoStream(dataStream, transform, CryptoStreamMode.Write))
            using (StreamWriter writer = new StreamWriter(encryptedOutputStream))
            {
                // In this block you do your writing, and it will automatically be encrypted
                writer.Write("This is the encrypted output data I want to write");
            }
        }

        private static string ZipXMLFile(string xmlFilename)
        {
            var doc = XDocument.Load(xmlFilename);
            string val = doc.ToString(SaveOptions.DisableFormatting);
            return Zip(val);
        }

        private static string UnzipXMLFile(string xmlFilename)
        {
            var doc = XDocument.Load(xmlFilename);
            string val = doc.ToString(SaveOptions.DisableFormatting);
            return UnZip(val);
        }

        // Given the full pathname of an output csv text file (probably want to use .CSV or .TXT extension)
        // Write a string array of lines to the file
        public static void WriteTextFile(string pathname, string[] lines)
        {
            // If there are any symbols for which we have subscribed to prices, save these to our symbols text file
            using (var writer = new StreamWriter(pathname))
            {
                for (int ri = 0; ri < lines.Length; ++ri)
                {
                    writer.WriteLine(lines[ri].Trim());
                }
            }
        }

        // Given a directory path and a string representing the "filename ending"
        // Return a string array containing the files in that directory that match the given filename ending
        public static string[] GetFilesEndingWith(string path, string filenameEnding)
        {
            var files = Directory.GetFiles(path);
            var filtered = new List<string>();
            foreach (string f in files)
            {
                if (f.EndsWith(filenameEnding))
                    filtered.Add(f);
            }
            return filtered.ToArray();
        }

        // Returns true if the given filename exists anywhere in the system path
        public static bool ExistsOnPath(string fileName)
        {
            return GetFullPath(fileName) != null;
        }

        // Iterates through the PATH environment variable and returns the first path in which the given filename exists
        // Returns null if the filename does not exist anywhere in the system path
        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(';'))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }

        static public ArrayList GetFilesWithExt(string path, string ext)
        {
            string myext = ext.ToLower().Trim();
            if (!myext.StartsWith("."))
                myext = "." + myext;

            ArrayList li = new ArrayList();

            if (Directory.Exists(path))
            {
                string[] fileEntries = Directory.GetFiles(path);
                foreach (string pathName in fileEntries)
                {
                    string fileName = Path.GetFileName(pathName);
                    if (fileName.EndsWith(myext))
                        li.Add(fileName);
                }
            }
            return li;
        }

        static public ArrayList ReadCopperTrades(string path)
        {
            ArrayList li = new ArrayList();

            if (Directory.Exists(path))
            {
                string[] fileEntries = Directory.GetFiles(path);
                foreach (string pathName in fileEntries)
                {
                    string fileName = Path.GetFileName(pathName);
                    if (fileName.StartsWith("backtest") && fileName.EndsWith(".csv"))
                        li.Add(fileName);
                }
            }
            return li;
        }

        // Change the current directory
        // Returns a DirectoryInfo object describing the newly selected directory
        public static DirectoryInfo ChangeDirectory(string directoryPath)
        {
            Environment.CurrentDirectory = directoryPath;
            DirectoryInfo info = new DirectoryInfo(".");
            dout("Directory Info:   " + info.FullName);
            return info;
        }

        // Given the full URL to a file online (i.e. "http://www.dtniq.com/product/mktsymbols_v2.zip")
        // Download that file to the given local pathname (i.e. @"D:\Downloads\symbols.zip")
        public static void DownloadFile(string url, string localPathname)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(url, localPathname);
            }
        }

        // Given a symbol (like "VIX.XO" or "M.CU3=LX") and an interval in seconds (for historical data)
        // Return the appropriate filename (including extension) for an associated CONTRACT DataFrame file
        public static string GetDfContractFilename(string symbol, int intervalSeconds)
        {
            return string.Format("{0}_contract.{1}", symbol, GetDfFileExtension(intervalSeconds));
        }

        // Given a root symbol (like "@VX" or "QHG") and an interval in seconds (for historical data)
        // Return the appropriate filename (including extension) for an associated FUTURES DataFrame file
        public static string GetDfFuturesFilename(string rootSymbol, int intervalSeconds)
        {
            return string.Format("{0}_futures.{1}", rootSymbol, GetDfFileExtension(intervalSeconds));
        }

        // Given a root symbol (like "@VX" or "QHG") and an interval in seconds (for historical data)
        // Return the appropriate filename (including extension) for an associated CONTINUOUS DataFrame file
        public static string GetDfContinuousFilename(string rootSymbol, int intervalSeconds)
        {
            return string.Format("{0}_continuous.{1}", rootSymbol, GetDfFileExtension(intervalSeconds));
        }

        // Given an interval in seconds (for historical data)
        // Return the appropriate file extension for an associated DataFrame file
        public static string GetDfFileExtension(int intervalSeconds)
        {
            if (intervalSeconds == 60)
                return "minute.DF.csv";
            else if (intervalSeconds == 3600)
                return "hour.DF.csv";
            else if (intervalSeconds == 86400)
                return "daily.DF.csv";
            else
                return string.Format("{0}.DF.csv", intervalSeconds);
        }

    } // end of class
} // end of namespace
