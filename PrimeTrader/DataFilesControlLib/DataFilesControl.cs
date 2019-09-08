using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Tools;

namespace DataFilesControlLib
{
    public partial class DataFilesControl: UserControl
    {
        Folders folders = Folders.DropboxFolders;

        public DataFilesControl()
        {
            InitializeComponent();

            UpdateAllDataLists();

            txtHistoricalFolder.Text = folders.df_folder;

            /*var tup = GetAllDataLists();
            var dataSymbols = tup.Item2;
            foreach (var txt in dataSymbols)
            {
                if (txt.Length <= 3)
                    cboSymbolRoot.Items.Add(txt);
                else
                    txtSymbol.Items.Add(txt);
            }*/
        }

        // From the data download folder retrieve a list of all filenames that have the given root symbol and timeframe
        private List<string> GetDataFiles(string rootSymbol, string timeframe)
        {
            List<string> results = new List<string>();

            if (timeframe != null && rootSymbol != null)
            {
                string[] fileEntries = Directory.GetFiles(folders.df_folder);
                foreach (string pathName in fileEntries)
                {
                    string filename = Path.GetFileName(pathName);
                    if (GetRootSymbol(filename) == rootSymbol && GetTimeFrame(filename) == timeframe)
                        results.Add(filename);
                }
            }

            return results;
        }

        // Given a data filename, return the text preceding the timeframe (ex: text before ".daily.")
        private string GetDataFrameName(string filename)
        {
            int i1 = filename.IndexOf('_');
            if (i1 < 0) return null;
            int i2 = filename.IndexOf('.', i1 + 1);
            if (i2 < 0) return null;
            return filename.Substring(0, i2);
        }

        // Given a data filename, retrieve the root symbol (ex: '@ES')
        private string GetRootSymbol(string filename)
        {
            string dfname = GetDataFrameName(filename);
            if (dfname == null) return null;
            int i1 = dfname.IndexOf('_');
            if (i1 < 0)
                return null;
            else
                return dfname.Substring(0, i1);
            /*if (G.IsFutureSymbol(symbol))
                return filename.Substring(0, 3);
            else
                return symbol;*/
        }

        // Given a data filename, retrieve the timeframe (within the filename parentheses, ex: 'Daily')
        private string GetTimeFrame(string filename)
        {
            int i2 = filename.IndexOf(".DF.csv");
            int i1 = -1;
            if (i2 > 0)
                i1 = filename.LastIndexOf('.', i2 - 1);

            string timeframe = null;
            if (i1 != -1 && i2 != -1)
                timeframe = filename.Substring(i1 + 1, i2 - i1 - 1);

            return timeframe;
        }

        private Tuple<List<string>, List<string>> GetAllDataLists()
        {
            var timeFrames = new List<string>();
            var dataSymbols = new List<string>();

            try
            {
                string[] fileEntries = Directory.GetFiles(folders.df_folder);
                foreach (string pathName in fileEntries)
                {
                    string filename = Path.GetFileName(pathName);
                    string timeframe = GetTimeFrame(filename);

                    if (timeframe != null)
                    {
                        if (!timeFrames.Contains(timeframe))
                            timeFrames.Add(timeframe);
                    }

                    //string symbol = GetRootSymbol(filename);
                    //if (symbol == null) symbol = GetSymbol(filename);
                    string symbol = GetDataFrameName(filename);
                    if (symbol != null && !dataSymbols.Contains(symbol))
                        dataSymbols.Add(symbol);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.Message);
            }

            return new Tuple<List<string>, List<string>>(timeFrames, dataSymbols);
        }

        // Update the lists for "root symbol" and "time frame" (by examining files in the data download folder)
        private void UpdateAllDataLists()
        {
            var tup = GetAllDataLists();
            var timeFrames = tup.Item1;
            var dataSymbols = tup.Item2;

            listDataTimeFrames.Items.Clear();
            listDataTimeFrames.Items.AddRange(timeFrames.ToArray());
            listDataSymbols.Items.Clear();
            listDataSymbols.Items.AddRange(dataSymbols.ToArray());
        }

        // Based on selected root symbol and selected timeframe, update the filenames in the Data Files list
        private void UpdateDataFilesList()
        {
            ClearFileList();

            string timeframe = (string)listDataTimeFrames.SelectedItem;
            string rootSymbol = (string)listDataSymbols.SelectedItem;

            var files = GetDataFiles(rootSymbol, timeframe);
            DisplayFileList(files);
            //listDataFiles.Items.AddRange(files.ToArray());
        }

        private void ClearFileList()
        {
            lvDataFiles.Items.Clear();
        }

        private void DisplayFileList(List<string> files, bool sortFutures = true)
        {
            files.Sort(CompareFuturesDate);
            foreach (string f in files)
            {
                //Console.WriteLine(f);
                ListViewItem lvi = new ListViewItem(f);
                //lvi.Text = f;
                DateTime modification = File.GetLastWriteTime(Path.Combine(folders.df_folder, f));
                lvi.SubItems.Add(modification.ToShortDateString());
                lvDataFiles.Items.Add(lvi);
            }

            lblDataFileCount.Text = string.Format("{0} Files", lvDataFiles.Items.Count);
        }

        private static string GetYYm(string rootSymbol)
        {
            string YYm = rootSymbol.Substring(4, 2) + rootSymbol.Substring(3, 1);
            return YYm;
        }

        private static int CompareFuturesDate(string x, string y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;
                else
                    return -1;
            }
            else
            {
                if (y == null)
                    return 1;
                else
                {
                    string cmpx = GetYYm(x);
                    string cmpy = GetYYm(y);
                    return cmpx.CompareTo(cmpy);
                }
            }
        }



        private void listDataTimeFrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataFilesList();
        }

        private void listDataSymbols_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataFilesList();
        }

        private void lvDataFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // For now just handle clicking on the filename column.
            if (e.Column == 0)
            {
                ListViewItem[] items = new ListViewItem[lvDataFiles.Items.Count];
                lvDataFiles.Items.CopyTo(items, 0);
                lvDataFiles.Items.Clear();
                lvDataFiles.Items.AddRange(items.Reverse().ToArray());
            }
        }




    } // END CLASS
} // END NAMESPACE
