using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GuiTools;
using GuiTools.Grid;
using Tools;
using static Tools.G;
using static Tools.GSystem;

namespace PrimeTrader
{
    public partial class DataFrameForm : Form
    {
        Color DEFAULT_BACKGROUND_COLOR = Color.White;

        Folders folders = Folders.DropboxFolders;

        public MessagesForm MessagesForm { get; set; }

        DataFrameFile m_dfFile;
        DataFrameGrid m_dataframeGrid;

        //Dictionary<string, int> m_selectedColumns = new Dictionary<string, int>();  // <"filename::headertext", columnIndex>
        List<string> m_selectedColumns = new List<string>();  // <"filename::headertext">
        RunStudyForm m_studyForm;

        int m_dateRangeV1, m_dateRangeV2;

        public DataFrameForm(MessagesForm messagesForm = null)
        {
            InitializeComponent();

            MessagesForm = messagesForm;

            m_dataframeGrid = new DataFrameGrid(gridDataFrame);
            //m_dataframeGrid.CellValueChanged += 
            m_dataframeGrid.CellValueChanged += Grid_CellValueChanged;
        }

        private void Grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != 0) return;    // only handle clicking the checkboxes in row zero

            string headerText = m_dataframeGrid.HeaderText(e.ColumnIndex);
            bool value = (bool) m_dataframeGrid[e.RowIndex, e.ColumnIndex];
            string filename = m_dfFile.Filename;
            string key = string.Format("{0}::{1}", filename, headerText);

            // ADD this column to our selected columns list
            if (value == true && !m_selectedColumns.Contains(key))
            {
                m_selectedColumns.Add(key);
            }
            else if (value == false && m_selectedColumns.Contains(key))        // REMOVE this column from our selected columns list
            {
                m_selectedColumns.Remove(key);
            }
            UpdateSelectedColumnsUI();
        }

        // Update the displayed list of selected columns
        private void UpdateSelectedColumnsList()
        {
            int count = 0;
            foreach (string k in m_selectedColumns)
            {
                dout("{0}: {1}", ++count, k);
            }
        }

        private void DataFrameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void LoadDataFrameFile(DataFrameFile dfFile, int v1, int v2)
        {
            m_dfFile = dfFile;
            m_dataframeGrid.InitializeColumns(m_dfFile.Columns, DEFAULT_BACKGROUND_COLOR);
            LoadDateRange(v1, v2);
            UpdateRowCount();
            m_dataframeGrid.ScrollToTop();
            //m_grid.ScrollToBottom();
            this.Text = m_dfFile.Filename;
        }

        public void LoadDateRange(int v1, int v2)
        {
            if (this.InvokeRequired) this.Invoke(new Action<int, int>(LoadDateRange), v1, v2);
            else
            {
                m_dateRangeV1 = v1;
                m_dateRangeV2 = v2;
                this.Cursor = Cursors.WaitCursor;
                m_dataframeGrid.SetRows(m_dfFile.ReadRows(v1, v2));
                m_dataframeGrid.ScrollToBottom();
                this.Cursor = Cursors.Default;
            }
        }

        // Plot the selected columns in the DataFrame (only the rows we have filtered)
        public void PlotDataFrame()
        {
            var plotColumns = m_dataframeGrid.GetSelectedPlotColumns();
            if (plotColumns.Count > 0)
            {
                string plot_filename = "temp.DF.csv";
                m_dataframeGrid.WriteDataFrameFile(Path.Combine(folders.df_folder, plot_filename));
                string workingDirectory = folders.python_folder;
                //workingDirectory = @"X:\Users\Trader\Dropbox\dev\python";
                //workingDirectory = @"D:\Users\mhatmaker\Dropbox\dev\python";            
                StringBuilder sb = new StringBuilder();
                foreach (string col in plotColumns.Keys)
                {
                    sb.Append(string.Format(" {0}={1}", col, plotColumns[col]));
                }
                //G.ProcessStart(@"C:\Python27\python.exe", "_plot.py temp.DF.csv Close=_r", workingDirectory, this.Handle);
                string args = string.Format("_plot.py {0} {1}", plot_filename, sb.ToString());
                ProcessStart(GSystem.Python27ExePathname, args, workingDirectory, this.Handle);
            }
        }

        // Plot the selected columns in the DataFrame (ALL rows in the original DataFrame file)
        public void PlotAllDataFrame()
        {
            var plotColumns = m_dataframeGrid.GetSelectedPlotColumns();
            if (plotColumns.Count > 0)
            {
                string plot_filename = "temp.DF.csv";
                m_dfFile.CopyTo(Path.Combine(folders.df_folder, plot_filename));
                string workingDirectory = folders.python_folder;
                //workingDirectory = @"X:\Users\Trader\Dropbox\dev\python";
                //workingDirectory = @"D:\Users\mhatmaker\Dropbox\dev\python";            
                StringBuilder sb = new StringBuilder();
                foreach (string col in plotColumns.Keys)
                {
                    sb.Append(string.Format(" {0}={1}", col, plotColumns[col]));
                }
                //G.ProcessStart(@"C:\Python27\python.exe", "_plot.py temp.DF.csv Close=_r", workingDirectory, this.Handle);
                string args = string.Format("_plot.py {0} {1}", plot_filename, sb.ToString());
                ProcessStart(GSystem.Python27ExePathname, args, workingDirectory, this.Handle);
            }
        }

        public void UpdateRowCount()
        {
            int selectedRowCount = (m_dateRangeV2 - m_dateRangeV1 + 1);
            StatusRight(string.Format("rows: {0} / {1}", selectedRowCount, m_dfFile.RowCount));
        }

        #region StatusBar --------------------------------------------------------------------------------------------------------------------------------------
        public void StatusLeft(string text)
        {
            tslblLeft.Text = text;
        }

        public void StatusMiddle(string text)
        {
            tslblMiddle.Text = text;
        }

        public void StatusRight(string text)
        {
            tslblRight.Text = text;
        }
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------


        private void tsbtnToExcelRange_Click(object sender, EventArgs e)
        {
            string filename = m_dfFile.GetTemporaryExcelFilename();
            string pathname = Path.Combine(folders.excel_folder, filename);
            m_dataframeGrid.WriteDataFrameFile(pathname);
            ProcessStartSimple("Excel.exe", pathname);
        }

        private void tsbtnToExcelAll_Click(object sender, EventArgs e)
        {
            string filename = m_dfFile.GetTemporaryExcelFilename();
            string pathname = Path.Combine(folders.excel_folder, filename);
            m_dfFile.CopyTo(pathname);
            ProcessStartSimple("Excel.exe", pathname);
        }

        private void tsbtnRunStudy_ButtonClick(object sender, EventArgs e)
        {
            dout("RUN (SOME) STUDY!");
        }

        private void tsbtnPlot_ButtonClick(object sender, EventArgs e)
        {
            this.PlotDataFrame();
        }

        private void tsitemPlotARIMA_Click(object sender, EventArgs e)
        {
            dout("PLOT ARIMA!");
        }

        private void tsbtnPlotAll_Click(object sender, EventArgs e)
        {
            this.PlotAllDataFrame();
        }

        private void tslblClear_Click(object sender, EventArgs e)
        {
            ClearSelectedColumns();
        }

        private void tsitemPlotAllARIMA_Click(object sender, EventArgs e)
        {

        }

        private void UpdateSelectedColumnsUI()
        {
            UpdateSelectedColumnsList();
            StatusLeft(string.Format("{0} selected column(s)", m_selectedColumns.Count));
        }

        private void tsitemARIMAStudy_Click(object sender, EventArgs e)
        {
            //GNetwork.TestZMQ();
            if (m_studyForm == null) m_studyForm = new RunStudyForm();
            m_studyForm.Text = "Run Study: ARIMA";
            //m_studyForm.ParamsRequired = 3;
            m_studyForm.SetParamNames(new string[] { "length:", "width:", "depth:" });
            m_studyForm.ColumnsRequired = 2;
            m_studyForm.SetSelectedColumns(m_selectedColumns);
            if (m_studyForm.ShowDialog(this) == DialogResult.OK)    // anything other than user clicking "Go" ("OK"), we do nothing
            {
                dout("RUN ARIMA!");
                var p = m_studyForm.GetParams();
                var columns = m_studyForm.ColumnsForStudy;

                var dfFiles = new Dictionary<DataFrameFile, string>();
                foreach (var c in columns)
                {
                    string[] substrings = c.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                    string filename = substrings[0];
                    string columnName = substrings[1];
                    dout("'{0}'    '{1}'", filename, columnName);

                    dfFiles.Add(new DataFrameFile(Folders.df_path(filename)), columnName);
                }

                List<Dictionary<string, string>> maps = new List<Dictionary<string, string>>();     // list of all DateTime=>ColumnValue maps
                foreach (var dff in dfFiles.Keys)
                {
                    var mapDateToValue = new Dictionary<string, string>();
                    var rows = dff.ReadRows();
                    string columnName = dfFiles[dff];
                    int cix = dff.GetColumnIndex(columnName);
                    foreach (var row in rows)
                    {
                        string dt = row[0];             // TODO: Get rid of the assumption that 'DateTime' column has to be first column
                        string value = row[cix];
                        mapDateToValue[dt] = value;
                    }
                    maps.Add(mapDateToValue);
                }
                dout("AND MAPS!!!");
            }
        }

        
        // Add diff(1) column to dataframe
        private void tsitemDiff_Click(object sender, EventArgs e)
        {
            if (m_selectedColumns.Count != 1)
            {
                MessagesForm.Status(string.Format("diff study requires selection of 1 column ({0} selected)", m_selectedColumns.Count));
                return;
            }
            string filename = m_selectedColumns[0].DfFilename();    // TODO: Allow selection of multiple columns (and pass multiple columns to the Python script)
            string column_name = m_selectedColumns[0].DfColumn();   // TODO: (instead of using just the first item from the selected columns array)
            string args = string.Format("DIFF {0} {1}", filename, column_name);
            GSystem.ProcessExited += GSystem_ProcessExited;
            MessagesForm.ExecutePython("primetrader.py", args);
        }

        private void tsitemEMA_Click(object sender, EventArgs e)
        {
            if (m_selectedColumns.Count < 1)
            {
                MessagesForm.Status(string.Format("EMA study requires selection of at least 1 column ({0} selected)", m_selectedColumns.Count));
                return;
            }

            /*if (m_studyForm == null) m_studyForm = new RunStudyForm();

            m_studyForm.Text = "Add Column: EMA (Exponential Moving ";
            //m_studyForm.ParamsRequired = 3;
            m_studyForm.SetParamNames(new string[] { "length:", "width:", "depth:" });
            m_studyForm.ColumnsRequired = 2;
            m_studyForm.SetSelectedColumns(m_selectedColumns);
            if (m_studyForm.ShowDialog(this) == DialogResult.OK)    // anything other than user clicking "Go" ("OK"), we do nothing
            {
                dout("RUN ARIMA!");
            }*/

            string filename = m_selectedColumns[0].DfFilename();    // TODO: Allow selection of multiple columns (and pass multiple columns to the Python script)
            string column_name = m_selectedColumns[0].DfColumn();   // TODO: (instead of using just the first item from the selected columns array)
            int periods = 0;
            int result;
            if (int.TryParse(tstxtInput.Text.Trim(), out result)) periods = result;
            string args = string.Format("EMA {0} {1} {2}", filename, column_name, periods);
            GSystem.ProcessExited += GSystem_ProcessExited;
            MessagesForm.ExecutePython("primetrader.py", args);
        }

        private void tsitemSMA_Click(object sender, EventArgs e)
        {
            if (m_selectedColumns.Count < 1)
            {
                MessagesForm.Status(string.Format("SMA study requires selection of at least 1 column ({0} selected)", m_selectedColumns.Count));
                return;
            }
            string filename = m_selectedColumns[0].DfFilename();    // TODO: Allow selection of multiple columns (and pass multiple columns to the Python script)
            string column_name = m_selectedColumns[0].DfColumn();   // TODO: (instead of using just the first item from the selected columns array)
            int periods = 5;
            string args = string.Format("SMA {0} {1} {2}", filename, column_name, periods);
            GSystem.ProcessExited += GSystem_ProcessExited;
            MessagesForm.ExecutePython("primetrader.py", args);
        }

        private void Reload()
        {
            ClearSelectedColumns();                                 // Clear the selected columns when we reload....may want to do this or may not?
            var v1 = m_dateRangeV1;
            var v2 = m_dateRangeV2;
            m_dfFile = new DataFrameFile(m_dfFile.Pathname);
            LoadDataFrameFile(m_dfFile, v1, v2);
        }

        private void GSystem_ProcessExited(object sender, EventArgs e)
        {
            GSystem.ProcessExited -= GSystem_ProcessExited;
            Reload();
            MessagesForm.Status("Reloaded dataframe.");
        }

        private void ClearSelectedColumns()
        {
            m_selectedColumns.Clear();
            m_dataframeGrid.ClearAllCheckboxes();
            UpdateSelectedColumnsUI();
        }

    } // end of class
} // end of namespace
