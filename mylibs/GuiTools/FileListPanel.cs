using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using static Tools.G;

namespace GuiTools
{
    public class FileListPanel
    {
        public Color BackColor { get { return m_displayPanel.BackColor; } set { m_displayPanel.BackColor = value; } }
        public string Text { get { return lblFileListDescription.Text; } set { lblFileListDescription.Text = value; } }
        public int FileCount { get; private set; }
        public string SelectedFilename { get { return m_selectedFilename; } }
        public string SelectedPath { get { return txtFilesDirectory.Text; } set { ChangeDirectory(value); } }
        public string SelectedPathname { get { return Path.Combine(SelectedPath, SelectedFilename); } }

        //public EventHandler SelectedFileChanged;
        public delegate void SelectedFileChangedHandler(FileListPanel p, FileSelectedArgs e);
        public event SelectedFileChangedHandler SelectedFileChanged;

        private System.Windows.Forms.TextBox txtFilesDirectory;
        private System.Windows.Forms.Label lblFileListDescription;
        private System.Windows.Forms.ListBox listFiles;
        private System.Windows.Forms.FolderBrowserDialog dialogFolderBrowser;

        private Panel m_displayPanel;

        private string m_dataFolder = null;
        private string m_filter = "";
        private string m_filenameMatch;
        private string m_selectedFilename;

        public FileListPanel(Panel displayPanel, string filenameMatch)
        {
            m_displayPanel = displayPanel;
            m_filenameMatch = filenameMatch;
            Initialize();            
        }

        private void Initialize()
        {
            this.listFiles = new System.Windows.Forms.ListBox();
            this.lblFileListDescription = new System.Windows.Forms.Label();
            this.txtFilesDirectory = new System.Windows.Forms.TextBox();
            this.dialogFolderBrowser = new FolderBrowserDialog();

            this.m_displayPanel.SuspendLayout();

            // 
            // panel
            // 
            this.m_displayPanel.Controls.Add(this.txtFilesDirectory);
            this.m_displayPanel.Controls.Add(this.lblFileListDescription);
            this.m_displayPanel.Controls.Add(this.listFiles);
            // 
            // txtFilesDirectory
            // 
            this.txtFilesDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilesDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilesDirectory.ForeColor = System.Drawing.Color.Gray;
            this.txtFilesDirectory.Location = new System.Drawing.Point(0, m_displayPanel.Height - 20);
            this.txtFilesDirectory.Name = "txtFilesDirectory";
            this.txtFilesDirectory.Size = new System.Drawing.Size(m_displayPanel.Width, 21);
            this.txtFilesDirectory.TabIndex = 8;
            this.txtFilesDirectory.ReadOnly = true;
            this.txtFilesDirectory.Click += TxtFilesDirectory_Click;
            // 
            // lblFileListDescription
            // 
            this.lblFileListDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileListDescription.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileListDescription.Location = new System.Drawing.Point(2, 6);
            this.lblFileListDescription.Name = "lblFileListDescription";
            this.lblFileListDescription.Size = new System.Drawing.Size(m_displayPanel.Width - 4, 32);
            this.lblFileListDescription.TabIndex = 7;
            this.lblFileListDescription.Text = "Strategy Files";
            this.lblFileListDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            /*// 
            // btnRunStrategy
            // 
            this.btnRunStrategy.Location = new System.Drawing.Point(1211, 15);
            this.btnRunStrategy.Name = "btnRunStrategy";
            this.btnRunStrategy.Size = new System.Drawing.Size(101, 29);
            this.btnRunStrategy.TabIndex = 8;
            this.btnRunStrategy.Text = "Run Strategy";
            this.btnRunStrategy.UseVisualStyleBackColor = true;*/
            // 
            // listFiles
            // 
            this.listFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listFiles.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listFiles.FormattingEnabled = true;
            this.listFiles.ItemHeight = 15;
            this.listFiles.Location = new System.Drawing.Point(8, 40);
            this.listFiles.Margin = new System.Windows.Forms.Padding(2);
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size(m_displayPanel.Width - 16, m_displayPanel.Height - 70);
            this.listFiles.TabIndex = 6;
            this.listFiles.SelectedIndexChanged += ListFiles_SelectedIndexChanged;

            this.m_displayPanel.ResumeLayout(false);
        }

        private void ListFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listFiles.SelectedIndex >= 0)
            {
                m_selectedFilename = listFiles.SelectedItem as string;
                var args = new FileSelectedArgs(this.SelectedPath, SelectedFilename);
                SelectedFileChanged?.Invoke(this, args);
            }
        }

        private void TxtFilesDirectory_Click(object sender, EventArgs e)
        {
            dialogFolderBrowser.ShowNewFolderButton = false;
            dialogFolderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            dialogFolderBrowser.SelectedPath = txtFilesDirectory.Text;
            if (dialogFolderBrowser.ShowDialog(m_displayPanel) == DialogResult.OK)
            {
                ChangeDirectory(dialogFolderBrowser.SelectedPath);
            }
        }

        private void ChangeDirectory(string dir)
        {
            if (Directory.Exists(dir))
            {
                txtFilesDirectory.Text = dir;
                UpdateFileList();
            }
            else
            {
                dout("FileListPanel=> Unable to change directory to '{0}'", dir);
            }
        }

        public void ClearFileList()
        {
            listFiles.Items.Clear();
        }

        public void UpdateFileList()
        {
            m_dataFolder = txtFilesDirectory.Text.Trim();

            var files = GetFiles(m_dataFolder, m_filenameMatch);
            files.Sort();                                           // TODO: allow sort by name OR date
            DisplayFileList(files, m_filter);
        }

        // From the data download folder retrieve a list of all filenames that have the given root symbol and timeframe
        private List<string> GetFiles(string folder, string filenameMatch)
        {
            List<string> results = new List<string>();

            if (!string.IsNullOrEmpty(folder))
            {
                string[] fileEntries = Directory.GetFiles(folder);
                foreach (string pathName in fileEntries)
                {
                    string filename = Path.GetFileName(pathName);
                    //if (GetRootSymbol(filename) == rootSymbol && GetTimeFrame(filename) == timeframe)
                    if (filename.EndsWith(filenameMatch))
                        results.Add(filename);
                }
            }
            return results;
        }

        private void DisplayFileList(List<string> files, string filter, bool sortFutures = true)
        {
            ClearFileList();
            //files.Sort(CompareFuturesDate);
            foreach (string f in files)
            {
                if (string.IsNullOrEmpty(filter) || f.Contains(filter))
                {
                    //ListViewItem lvi = new ListViewItem(f);
                    //DateTime modification = File.GetLastWriteTime(Path.Combine(folders.df_folder, f));
                    //lvi.SubItems.Add(modification.ToShortDateString());
                    listFiles.Items.Add(f);
                }
            }

            this.FileCount = listFiles.Items.Count;
        }

    } // end of class


    public class FileSelectedArgs : EventArgs
    {
        public string Filename { get { return m_filename; } }
        public string Directory { get { return m_directory; } }
        public string Pathname { get { return Path.Combine(m_directory, m_filename); } }

        private string m_directory;
        private string m_filename;

        public FileSelectedArgs(string directory, string filename)
        {
            m_directory = directory;
            m_filename = filename;
        }
    } // end of class

} // end of namespace
