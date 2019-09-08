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

namespace DataFrameFileControlLib
{
    public partial class DataFrameFileControl: UserControl
    {
        public event EventHandler SelectedFileChanged;
        public event EventHandler FileDoubleClicked;

        Folders folders = Folders.DropboxFolders;

        string m_selectedFile = null;
        string m_dataFolder;

        public string DataFolder { get { return m_dataFolder; } }
        public string SelectedFile { get { return m_selectedFile; } }

        public DataFrameFileControl()
        {
            InitializeComponent();

            txtHistoricalFolder.Text = (folders == null) ? "" : folders.df_folder;

            UpdateFileList();

            lvDataFiles.SelectedIndexChanged += this.HandleSelectedFileChanged;
            lvDataFiles.DoubleClick += this.HandleDoubleClick;
        }

        private void HandleDoubleClick(object sender, EventArgs e)
        {                        
            FileDoubleClicked?.Invoke(this, EventArgs.Empty);
        }

        private void HandleSelectedFileChanged(object sender, EventArgs e)
        {
            if (lvDataFiles.SelectedItems.Count <= 0)
                m_selectedFile = null;
            else
                m_selectedFile = lvDataFiles.SelectedItems[0].Text;

            // We could raise SelectedFileChanged directly, but it is best practice to create an
            // OnEventName protected virtual method to raise the event for us. That way, inheriting
            // classes can "handle" the event by overriding the OnEventName method, which turns out
            // to have a little better performance than subscribing to the event. Even if you think
            // you will never override the OnEventName method, it is a good idea to get in the habit
            // of doing it anyway, as it simplifies the event raising process.
            this.OnSelectedFileChanged(EventArgs.Empty);
        }

        protected virtual void OnSelectedFileChanged(EventArgs e)
        {
            EventHandler handler = this.SelectedFileChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // From the data download folder retrieve a list of all filenames that have the given root symbol and timeframe
        private List<string> GetDataFiles(string folder, bool sort=true)
        {
            List<string> results = new List<string>();

            if (!string.IsNullOrEmpty(folder))
            {
                string[] fileEntries = Directory.GetFiles(folder);
                foreach (string pathName in fileEntries)
                {
                    string filename = Path.GetFileName(pathName);
                    //if (GetRootSymbol(filename) == rootSymbol && GetTimeFrame(filename) == timeframe)
                    if (filename.EndsWith(".DF.csv"))
                        results.Add(filename);
                }
                if (sort) results.Sort();
            }
            return results;
        }

        // Update the displayed list by examining files in the data folder
        private void UpdateFileList()
        {
            m_dataFolder = txtHistoricalFolder.Text.Trim();

            var files = GetDataFiles(m_dataFolder);
            var filter = txtFilter.Text.Trim();
            DisplayFileList(files, filter);
        }

        private void ClearFileList()
        {
            lvDataFiles.Items.Clear();
        }

        private void DisplayFileList(List<string> files, string filter, bool sortFutures = true)
        {
            ClearFileList();
            files.Sort(CompareFuturesDate);
            foreach (string f in files)
            {
                if (string.IsNullOrEmpty(filter) || f.Contains(filter))
                {
                    ListViewItem lvi = new ListViewItem(f);
                    DateTime modification = File.GetLastWriteTime(Path.Combine(folders.df_folder, f));
                    lvi.SubItems.Add(modification.ToShortDateString());
                    lvDataFiles.Items.Add(lvi);
                }
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

        private void btnChooseFolderDF_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogDF.ShowDialog(this) == DialogResult.OK)
            {
                txtHistoricalFolder.Text = folderBrowserDialogDF.SelectedPath;
                UpdateFileList();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            UpdateFileList();
        }

        private void btnClearFilterDF_Click(object sender, EventArgs e)
        {
            txtFilter.Text = "";
        }

    } // END CLASS
} // END NAMESPACE
