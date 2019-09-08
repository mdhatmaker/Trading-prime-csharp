using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using System.Windows.Forms;
using static Tools.G;

namespace Tools
{
    public class Folders
    {
        public static string ROOT_DATA_FOLDER { get; set; }

        public string root_folder { get; private set; }
        public string python_folder { get; private set; }
        public string csharp_folder { get; private set; }
        public string args_folder { get; private set; }
        public string data_folder { get; private set; }
        public string raw_folder { get; private set; }
        public string df_folder { get; private set; }
        public string charts_folder { get; private set; }
        public string projects_folder { get; private set; }
        public string misc_folder { get; private set; }
        public string system_folder { get; private set; }
        public string excel_folder { get; private set; }
        public string quandl_folder { get; private set; }
        public string strategy_folder { get; private set; }
        public string crypto_folder { get; private set; }

        // Singleton?
        public static Folders DropboxFolders
        {
            get
            {
                if (m_folders != null)
                {
                    return m_folders;
                }
                else
                {
                    string dropboxPath;
                    if (ROOT_DATA_FOLDER != null)                   // ROOT_DATA_FOLDER can be set as a public static variable of the class
                        dropboxPath = ROOT_DATA_FOLDER;
                    else
                        dropboxPath = GSystem.GetEnvironmentVariable("DROPBOXPATH");

                    if (dropboxPath != null)    // if %DROPBOXPATH% environment variable exists, use it
                    {
                        dout("Found %DROPBOXPATH% environment variable, so using it: '{0}'", dropboxPath);
                        m_folders = new Folders(dropboxPath);
                        return m_folders;
                    }
                    else        // otherwise, we need to attempt to find the Dropbox folder by traversing the path in reverse
                    {
                        dout("No %DROPBOXPATH% environment variable found, so attempting to find it...");
                        var rf = GetDropboxFolder();
                        if (rf == null)
                        {
                            ErrorMessage("FATAL ERROR: Path of root level Dropbox folder could not be found!\nPlease correct this issue and try again.");
                            m_folders = new Folders(GFile.GetExecutablePath());
                            return m_folders; 
                        }
                        else
                        {
                            dout("Path to root level Dropbox folder determined to be the following: '{0}'\nIf this seems incorrect, correct this issue and relaunch the app.", rf);
                            m_folders = new Folders(rf);
                            return m_folders;
                        }
                    }
                }
            }
        }

        private static string m_dropboxFolder = null;
        private static Folders m_folders = null;

        private Folders(string root_folder)
        {
            this.root_folder = root_folder;
            this.python_folder = Path.Combine(root_folder, "alvin", "python");
            this.csharp_folder = Path.Combine(root_folder, "alvin", "csharp");
            this.args_folder = Path.Combine(python_folder, "alvin", "args");
            this.data_folder = Path.Combine(root_folder, "alvin", "data");
            this.raw_folder = Path.Combine(data_folder, "RAW_DATA");
            this.df_folder = Path.Combine(data_folder, "DF_DATA");
            this.charts_folder = Path.Combine(data_folder, "charts");
            this.projects_folder = Path.Combine(data_folder, "projects");
            this.misc_folder = Path.Combine(data_folder, "MISC");
            this.system_folder = Path.Combine(data_folder, "SYSTEM");
            this.excel_folder = Path.Combine(data_folder, "EXCEL");
            this.quandl_folder = Path.Combine(data_folder, "DF_QUANDL");
            this.crypto_folder = Path.Combine(data_folder, "DF_CRYPTO");
            this.strategy_folder = Path.Combine(this.system_folder, "STRATEGY_FILES");
        }


        #region Convenience methods that quickly prepend the appropriate folder path onto the given filename (and return full pathname)
        // Use path of current Executable
        public static string exe_path(string filename)
        {
            string exe_dir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(exe_dir, filename);
        }

        public static string python_path(string filename)
        {
            return Path.Combine(DropboxFolders.python_folder, filename);
        }

        public static string df_path(string filename)
        {
            return Path.Combine(DropboxFolders.df_folder, filename);
        }

        public static string raw_path(string filename)
        {
            return Path.Combine(DropboxFolders.raw_folder, filename);
        }

        public static string charts_path(string filename)
        {
            return Path.Combine(DropboxFolders.charts_folder, filename);
        }

        public static string misc_path(string filename)
        {
            return Path.Combine(DropboxFolders.misc_folder, filename);
        }

        public static string system_path(string filename)
        {
            return Path.Combine(DropboxFolders.system_folder, filename);
        }

        public static string excel_path(string filename)
        {
            return Path.Combine(DropboxFolders.excel_folder, filename);
        }

        public static string quandl_path(string filename)
        {
            return Path.Combine(DropboxFolders.quandl_folder, filename);
        }

        public static string strategy_path(string filename)
        {
            return Path.Combine(DropboxFolders.strategy_folder, filename);
        }

        public static string crypto_path(string filename)
        {
            return Path.Combine(DropboxFolders.crypto_folder, filename);
        }
        #endregion


        // Return the "root-level" Dropbox folder
        public static string GetDropboxFolder(bool forceCheck = false)
        {
            if (m_dropboxFolder == null || forceCheck == true)
            {
                var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                var appPath = System.IO.Path.GetDirectoryName(location);
                //string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                var split = Path.GetDirectoryName(appPath).Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                //Console.WriteLine(split.Last());

                var index = Array.FindIndex(split, row => row.Contains("Dropbox"));

                if (index > 0)
                {
                    split = split.Take(index + 1).ToArray();

                    // If split[0] is the drive letter, to which we need to manually add the separator:
                    if (split[0].EndsWith(":"))
                        split[0] += Path.DirectorySeparatorChar;
                    else
                        split[0] = @"\\" + split[0];
                    m_dropboxFolder = Path.Combine(split);
                }
            }
            return m_dropboxFolder;
        }


        public static bool EnsureFolderExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return Directory.Exists(folderPath);
        }

    } // end of class
} // end of namespace
