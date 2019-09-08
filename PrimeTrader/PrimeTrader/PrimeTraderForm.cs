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
using System.Diagnostics;
using System.Collections;
using GuiTools;
using GuiTools.Grid;
using Tools;
using static Tools.GFile;
using static Tools.GSystem;
using static Tools.G;
using PrimeTrader.Properties;
using System.Configuration;

namespace PrimeTrader
{
    public partial class PrimeTraderForm : Form
    {
        static PrimeTraderForm()
        {
            Tools.Folders.ROOT_DATA_FOLDER = Tools.Settings.Instance["ROOT_DATA_FOLDER"];
        }

        Folders folders = Folders.DropboxFolders;

        FileListPanel m_strategyFilesPanel;
        FileListPanel m_dataFrameFilesPanel;

        DataFrameFile m_dfFile;
        
        DateTime m_lastStatusUpdate = DateTime.Now;
        bool m_ignoreResetStatus = false;
        bool m_ignoreTrackBarUpdates = false;
        int m_defaultRowCount = 300;
        int m_initialPanelRightWidth;

        Dictionary<string, string> m_scriptDescriptions;
        bool changingScriptSelection;

        Dictionary<string, Form> m_allForms;

        public Dictionary<string, Form> Forms { get { return m_allForms; } }

        public PrimeTraderForm()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;

            InitializeComponent();

            CreateAllFormsList();


            m_strategyFilesPanel = new FileListPanel(panelStrategyFiles, ".cs");
            m_strategyFilesPanel.Text = "Strategy Files";
            m_strategyFilesPanel.SelectedPath = Folders.DropboxFolders.strategy_folder;
            m_strategyFilesPanel.SelectedFileChanged += strategyFilesPanel_SelectedFileChanged;

            m_pricesForm = new IQFeed.GUI.PricesForm();
            m_dataFrameForm = new DataFrameForm();

            m_dataFrameFilesPanel = new FileListPanel(dataFrameFilePanel1, ".DF.csv");
            m_dataFrameFilesPanel.Text = "DataFrame Files";
            m_dataFrameFilesPanel.SelectedPath = folders.df_folder;
            m_dataFrameFilesPanel.SelectedFileChanged += dataFrameFilesPanel1_SelectedFileChanged;
            
            m_initialPanelRightWidth = panelDataRight.Width;

            historicalDataForm.UpdateDataFilesEvent += HistoricalDataForm_UpdateDataFilesEvent;
        }

        private void HistoricalDataForm_UpdateDataFilesEvent(object sender, EventArgs e)
        {
            m_dataFrameFilesPanel.UpdateFileList();
        }

        private void strategyFilesPanel_SelectedFileChanged(FileListPanel p, FileSelectedArgs e)
        {
            /*strategyForm.DisplayStrategyCode(e.Pathname);
            strategyForm.ShowInFront();*/
        }

        private void PythonUiForm_Load(object sender, EventArgs e)
        {
            UpdatePythonScriptListFilenames(chkHideFunctionModules.Checked);
            UpdatePythonScriptDescriptions();
            UpdateChartListFilenames();

            //CreateAllFormsList();

            messagesForm.ShowInFront();

            settingsForm.RestoreAllFormLocations();
        }

        private void DataFrameFileSelected(string filename)
        {
            status("Reading DataFrame: " + filename + " ...");
            StartStatusTimer();

            string dataFolder = m_dataFrameFilesPanel.SelectedPath;

            m_dfFile = new DataFrameFile(Path.Combine(dataFolder, filename));

            lblFirstDate.Text = m_dfFile.FirstIndexString;
            lblLastDate.Text = m_dfFile.LastIndexString;

            ResetTrackBars(true);

            dataFrameForm.ShowInFront();
            dataFrameForm.LoadDataFrameFile(m_dfFile, trackBarDate1.Value, trackBarDate2.Value);

            StopStatusTimer();
            status("Reading DataFrame: " + filename + " ... Done.");
        }

        private void dataFrameFilesPanel1_SelectedFileChanged(object sender, EventArgs e)
        {
            var selected = m_dataFrameFilesPanel.SelectedFilename;
            //if (selected != null) DataFrameFileSelected(selected);
        }

        private void UpdatePythonScriptListFilenames(bool hideFunctionModules, bool sort=true)
        {
            ArrayList list = GetFilesWithExt(folders.python_folder, ".py");
            if (sort) list.Sort();
            listScripts.Items.Clear();
            if (hideFunctionModules == false)
                listScripts.Items.AddRange(list.ToArray());
            else
            {
                foreach (string li in list)
                {
                    if (!li.StartsWith("f_"))
                        listScripts.Items.Add(li);
                }
            }
        }

        private void UpdatePythonScriptDescriptions()
        {
            m_scriptDescriptions = new Dictionary<string, string>();
            var df = DataFrame.ReadDataFrame(Folders.system_path("python_script_descriptions.DF.csv"));
            foreach (DataFrameRow row in df.Rows)
            {
                string symbol = row[0];
                string description = row[1];
                m_scriptDescriptions.Add(symbol, description);
                listScriptDescriptions.Items.Add(description);
            }
        }

        private void UpdateChartListFilenames()
        {
            ArrayList list = GetFilesWithExt(folders.charts_folder, ".html");
            listCharts.Items.Clear();
            listCharts.Items.AddRange(list.ToArray());
        }

        private void btnLaunchPythonScript_Click(object sender, EventArgs e)
        {
            if (listScripts.SelectedIndex < 0) return;

            enableLaunchPythonButton(false);

            string script = (string)listScripts.SelectedItem;

            m_ignoreResetStatus = true;
            status("Running Python script: " + script + " ...");
            statusColor(Color.Yellow);

            GSystem.ProcessExited += GSystem_ProcessExited;

            StartStatusTimer();

            try
            {
                /*foreach (string s in RunPython(script, "", this.Handle))
                {
                    cout(s);
                    Application.DoEvents();
                }*/
                RunPython(script, "", this.Handle);
            }
            catch (Exception ex)
            {
                string errorMsg = string.Format("An error occurred attempting to launch Python.\n\nCheck the app settings to ensure your Python is configured correctly.\n({0})", ex.Message);
                MessageBox.Show(this, errorMsg, "Python Error");
            }
        }

        private void GSystem_ProcessExited(object sender, EventArgs e)
        {
            enableLaunchPythonButton(true);
            StopStatusTimer();
            statusAppend(" Done.");
            statusColor();
            m_ignoreResetStatus = false;
            GSystem.ProcessExited -= GSystem_ProcessExited;
        }

        private void ReloadCopperTrades()
        {
            ArrayList li;

            string pathname = Path.Combine(folders.projects_folder, "copper");
            li = ReadCopperTrades(pathname);
            listTrades.Items.Clear();
            listTrades.Items.AddRange(li.ToArray());

            //li = G.GetFile
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadCopperTrades();
        }

        private void listTrades_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filename = (string)listTrades.SelectedItem;
            string pathname = Path.Combine(folders.projects_folder, "copper", filename);
            //LoadCsv(gridCopper1, pathname);
            gridCopper1.LoadCsv(pathname);
        }

        private void listScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changingScriptSelection) return;
            else changingScriptSelection = true;

            string script = listScripts.SelectedItem as string;

            if (m_scriptDescriptions.ContainsKey(script))
                listScriptDescriptions.SelectTextItem(m_scriptDescriptions[script]);
            else
                listScriptDescriptions.SelectedIndex = -1;

            changingScriptSelection = false;
        }

        private void listScriptDescriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changingScriptSelection) return;
            else changingScriptSelection = true;

            string desc = listScriptDescriptions.SelectedItem as string;

            string scriptToSelect = "";
            foreach (var script in m_scriptDescriptions.Keys)
            {
                if (m_scriptDescriptions[script] == desc)
                {
                    scriptToSelect = script;
                    break;
                }
            }
            listScripts.SelectTextItem(scriptToSelect);

            changingScriptSelection = false;
        }

        private void btnHistorical_Click(object sender, EventArgs e)
        {
            status("IQFeed: Historical");
            historicalDataForm.ShowInFront();
            //historicalDataForm.MessagesForm = messagesForm;
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            status("Display Chart");
            var selected = m_dataFrameFilesPanel.SelectedPathname;
            if (selected != null)
            {
                var chart = new CryptoForms.CryptoChartForm();
                var candlesticks = ZCandlestickMap.ReadFromFile(selected);
                chart.DisplayChart(candlesticks);
                chart.ShowInFront();
            }
        }

        private void btnDataGrid_Click(object sender, EventArgs e)
        {
            status("Data Grid");
            var selected = m_dataFrameFilesPanel.SelectedFilename;
            if (selected != null)
            {
                DataFrameFileSelected(selected);
                dataFrameForm.ShowInFront();
            }
        }

        private void btnCryptoInfo_Click(object sender, EventArgs e)
        {
            status("Crypto Market Information");
            cryptoInfoForm.ShowInFront();
        }

        private void btnCryptoPrices_Click(object sender, EventArgs e)
        {
            status("Crypto Prices");
            cryptoPricesForm.ShowInFront();
        }

        private void btnCryptoGator_Click(object sender, EventArgs e)
        {
            status("Crypto Aggregator");
            cryptoGatorForm.ShowInFront();
        }

        private void btnCryptoTrader_Click(object sender, EventArgs e)
        {
            status("Crypto Trader");
            cryptoTradeForm.ShowInFront();
        }

        private void btnLevel1Prices_Click(object sender, EventArgs e)
        {
            status("IQFeed: Level 1 Prices");
            pricesForm.ShowInFront();
        }

        private void btnMessages_Click(object sender, EventArgs e)
        {
            status("Output and Error Messages");
            messagesForm.ShowInFront();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            status("App Settings");
            settingsForm.ShowInFront();
        }

        private void chkHideFunctionModules_CheckStateChanged(object sender, EventArgs e)
        {
            UpdatePythonScriptListFilenames(chkHideFunctionModules.Checked);
        }

        private void enableLaunchPythonButton(bool b)
        {
            try
            {
                if (btnLaunchPythonScript.InvokeRequired) { btnLaunchPythonScript.Invoke(new Action<bool>(enableLaunchPythonButton), b); }     // check if we need to invoke the delegate
                else
                {
                    // <GUI operations here>
                    btnLaunchPythonScript.Enabled = b;
                }
            }
            catch (ObjectDisposedException) { }     // The GUI object went away, ignore it since we're probably exiting.
        }

        private void status(string text)
        {
            try
            {                
                if (statusStrip1.InvokeRequired) { statusStrip1.Invoke(new Action<string>(status), text); }     // check if we need to invoke the delegate
                else
                {
                    // <GUI operations here>
                    statusLabel1.Text = text;
                    m_lastStatusUpdate = DateTime.Now;
                    Application.DoEvents();
                }
            }
            catch (ObjectDisposedException) { }     // The GUI object went away, ignore it since we're probably exiting.
        }

        private void statusAppend(string text)
        {
            status(statusLabel1.Text + text);
        }

        private void statusColor(Color? color=null)
        {
            try
            {
                if (statusStrip1.InvokeRequired) { statusStrip1.Invoke(new Action<Color?>(statusColor), color); }     // check if we need to invoke the delegate
                else
                {
                    // <GUI operations here>
                    statusLabel1.BackColor = color ?? SystemColors.Control;
                    Application.DoEvents();
                }
            }
            catch (ObjectDisposedException) { }     // The GUI object went away, ignore it since we're probably exiting.
        }

        private void status2(string text)
        {
            statusLabel2.Text = text;
            Application.DoEvents();
        }

        private void listCharts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listCharts.SelectedIndex < 0) return;

            string chart = (string)listCharts.SelectedItem;

            string chart_filename = Path.Combine(folders.charts_folder, chart);

            string url = "file://" + chart_filename;
            browserForm.ShowInFront();
            Task.Run(() => browserForm.Navigate(url));
            //browserForm.Navigate(url);
            //Process.Start(url);
            //Process.Start("Chrome", Uri.EscapeDataString(chart_filename));
        }

        public void StartStatusTimer()
        {
            timerProgress.Enabled = true;
            timerProgress.Start();
            Application.DoEvents();
        }

        public void StopStatusTimer()
        {
            timerProgress.Stop();
            timerProgress.Enabled = false;
            Application.DoEvents();
        }

        private void timerProgress_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine("timer");
            if (statusProgress.Value >= statusProgress.Maximum)
                statusProgress.Value = 1;
            else
                statusProgress.Value += 1;
            Application.DoEvents();
        }

        private void listScripts_SelectedValueChanged(object sender, EventArgs e)
        {
            /*if (listScripts.SelectedItem != null)
            {
                string pathname = Path.Combine(folders.python_folder, listScripts.SelectedItem.ToString());
                //Directory.SetCurrentDirectory(folders.python_folder);
                List<string> lines = ReadPythonScriptText(pathname);
                DisplayOutput(lines);
            }*/
        }

        private void ResetTrackBars(bool ignoreTrackBarUpdates=false)
        {
            m_ignoreTrackBarUpdates = ignoreTrackBarUpdates;
            trackBarDate1.Minimum = 1;
            trackBarDate1.Maximum = m_dfFile.RowCount;
            trackBarDate2.Minimum = 1;
            trackBarDate2.Maximum = m_dfFile.RowCount;
            trackBarDate1.Value = trackBarDate1.Minimum;
            trackBarDate2.Value = trackBarDate2.Maximum;
            trackBarDate2.Value = trackBarDate2.Minimum;
            trackBarDate2.Value = trackBarDate2.Maximum;
            trackBarDate1.Value = trackBarDate1.Maximum;
            if (m_dfFile.RowCount > 0)
                trackBarDate1.Value = Math.Max(trackBarDate2.Maximum - m_defaultRowCount, 1);
            lblDate1.Text = m_dfFile.GetIndexString(trackBarDate1.Value);
            lblDate2.Text = m_dfFile.GetIndexString(trackBarDate2.Value);
            m_ignoreTrackBarUpdates = false;
        }

        private void DisplayDataFrameRowCount()
        {
            dataFrameForm.UpdateRowCount();
        }

        private void trackBarDate1_ValueChanged(object sender, EventArgs e)
        {
            if (m_ignoreTrackBarUpdates == true) return;

            if (trackBarDate1.Value > trackBarDate2.Value)
                trackBarDate1.Value = trackBarDate2.Value;
            lblDate1.Text = m_dfFile.GetIndexString(trackBarDate1.Value);
            DisplayDataFrameRowCount();
        }

        private void trackBarDate2_ValueChanged(object sender, EventArgs e)
        {
            if (m_ignoreTrackBarUpdates == true) return;

            if (trackBarDate2.Value < trackBarDate1.Value)
                trackBarDate2.Value = trackBarDate1.Value;
            lblDate2.Text = m_dfFile.GetIndexString(trackBarDate2.Value);
            DisplayDataFrameRowCount();
        }

        private void btnLoadDateRange_Click(object sender, EventArgs e)
        {
            status(string.Format("Loading date range: {0} to {1} ...", lblDate1.Text, lblDate2.Text));

            dataFrameForm.LoadDateRange(trackBarDate1.Value, trackBarDate2.Value);

            status(string.Format("Loading date range: {0} to {1} ... Done.", lblDate1.Text, lblDate2.Text));
        }

        private void timerResetStatus_Tick(object sender, EventArgs e)
        {
            if (m_ignoreResetStatus) return;
            if (DateTime.Now > (m_lastStatusUpdate.AddSeconds(3)))
                statusLabel1.Text = "";
        }

        private void btnPlotDataframe_Click(object sender, EventArgs e)
        {
            dataFrameForm.PlotDataFrame();
        }

        /*private void buttonHideShowDataFrameFilePicker_Click(object sender, EventArgs e)
        {
            if (panelDataLeft.Visible == true)
            {
                panelDataLeft.Visible = false;
                panelDataRight.Left = panelDataLeft.Location.X + 1;
                panelDataRight.Width = this.ClientRectangle.Width - 20;
                this.buttonHideShowDataFrameFilePicker.Image = global::PythonUI.Properties.Resources.button_right_gray_32;
            }
            else
            {
                panelDataLeft.Visible = true;
                panelDataRight.Left = panelDataLeft.Width + 10;
                panelDataRight.Width = m_initialPanelRightWidth;
                this.buttonHideShowDataFrameFilePicker.Image = global::PythonUI.Properties.Resources.button_left_gray_32;
            }
        }*/


        #region Form member variables --------------------------------------------------------------------------------------------------------------------------
        IQFeed.GUI.HistoricalDataForm m_historyForm = null;
        IQFeed.GUI.IQFeedSettingsForm m_launchFeedForm = null;
        //IQFeed.StreamingBarsForm m_streamingBarsForm = null;
        CryptoForms.CryptoChartForm m_chartForm = null;
        IQFeed.GUI.PricesForm m_pricesForm = null;
        CryptoForms.CryptoInfoForm m_cryptoInfoForm = null;
        CryptoForms.CryptoTradeForm m_cryptoTradeForm = null;
        CryptoForms.CryptoGatorForm m_cryptoGatorForm = null;
        CryptoForms.CryptoPricesForm m_cryptoPricesForm = null;
        DataFrameForm m_dataFrameForm = null;
        BrowserForm m_browserForm = null;
        PriceGridForm m_priceGridForm = null;
        SettingsForm m_settingsForm = null;
        MessagesForm m_messagesForm = null;
        //ZeroSumAPI.StrategyForm m_strategyForm = null;

        private void CreateAllFormsList()
        {
            m_allForms = new Dictionary<string, Form>() {
                { "PrimeTraderForm", this },
                { "HistoricalDataForm", m_historyForm },
                { "IQFeedSettingsForm", m_launchFeedForm },
                //{ "StreamingBarsForm", m_streamingBarsForm },
                { "CryptoChartForm", m_chartForm },
                { "PricesForm", m_pricesForm },
                { "CryptoInfoForm", m_cryptoInfoForm },
                { "CryptoTradeForm", m_cryptoTradeForm },
                { "CryptoGatorForm", m_cryptoGatorForm },
                { "CryptoPricesForm", m_cryptoPricesForm },
                { "DataFrameForm", m_dataFrameForm },
                { "BrowserForm", m_browserForm },
                { "PriceGridForm", m_priceGridForm },
                //{ "SettingsForm", m_settingsForm },       // ignore the settings form because it will never be saved/restored
                { "MessagesForm", m_messagesForm }
            };
            settingsForm.MainPrimeTraderForm = this;
        }

        private IQFeed.GUI.HistoricalDataForm historicalDataForm
        {
            get
            {
                if (Forms["HistoricalDataForm"] == null) Forms["HistoricalDataForm"] = new IQFeed.GUI.HistoricalDataForm(messagesForm);
                (Forms["HistoricalDataForm"] as IQFeed.GUI.HistoricalDataForm).MessagesForm = messagesForm;
                return Forms["HistoricalDataForm"] as IQFeed.GUI.HistoricalDataForm;
            }
        }

        private IQFeed.GUI.PricesForm pricesForm
        {
            get
            {
                if (Forms["PricesForm"] == null) Forms["PricesForm"] = new IQFeed.GUI.PricesForm();
                return Forms["PricesForm"] as IQFeed.GUI.PricesForm;
            }
        }

        /*private IQFeed.StreamingBarsForm streamingBarsForm
        {
            get
            {
                if (Forms["StreamingBarsForm"] == null) Forms["StreamingBarsForm"] = new IQFeed.StreamingBarsForm();
                return Forms["StreamingBarsForm"] as IQFeed.StreamingBarsForm;
            }
        }*/

        private CryptoForms.CryptoChartForm chartForm
        {
            get
            {
                if (Forms["CryptoChartForm"] == null) Forms["CryptoChartForm"] = new CryptoForms.CryptoChartForm();
                return Forms["CryptoChartForm"] as CryptoForms.CryptoChartForm;
            }
        }

        /*private IQFeed.IQFeedSettingsForm launchFeedForm
        {
            get
            {
                if (m_launchFeedForm == null) m_launchFeedForm = new IQFeed.IQFeedSettingsForm();
                return m_launchFeedForm;
            }
        }*/

        private CryptoForms.CryptoInfoForm cryptoInfoForm
        {
            get
            {
                if (Forms["CryptoInfoForm"] == null) Forms["CryptoInfoForm"] = new CryptoForms.CryptoInfoForm();
                return Forms["CryptoInfoForm"] as CryptoForms.CryptoInfoForm;
            }
        }

        private CryptoForms.CryptoTradeForm cryptoTradeForm
        {
            get
            {
                if (Forms["CryptoTradeForm"] == null) Forms["CryptoTradeForm"] = new CryptoForms.CryptoTradeForm();
                return Forms["CryptoTradeForm"] as CryptoForms.CryptoTradeForm;
            }
        }

        private CryptoForms.CryptoPricesForm cryptoPricesForm
        {
            get
            {
                //if (m_cryptoPricesForm == null) m_cryptoPricesForm = new CryptoForms.CryptoPricesForm();
                //return m_cryptoPricesForm;
                if (Forms["CryptoPricesForm"] == null) Forms["CryptoPricesForm"] = new CryptoForms.CryptoPricesForm();
                return Forms["CryptoPricesForm"] as CryptoForms.CryptoPricesForm;
            }
        }

        private CryptoForms.CryptoGatorForm cryptoGatorForm
        {
            get
            {
                if (Forms["CryptoGatorForm"] == null) Forms["CryptoGatorForm"] = new CryptoForms.CryptoGatorForm();
                return Forms["CryptoGatorForm"] as CryptoForms.CryptoGatorForm;
            }
        }

        private DataFrameForm dataFrameForm
        {
            get
            {
                if (Forms["DataFrameForm"] == null) Forms["DataFrameForm"] = new DataFrameForm();
                (Forms["DataFrameForm"] as PrimeTrader.DataFrameForm).MessagesForm = messagesForm;
                return Forms["DataFrameForm"] as PrimeTrader.DataFrameForm;
            }
        }

        private BrowserForm browserForm
        {
            get
            {
                if (Forms["BrowserForm"] == null) Forms["BrowserForm"] = new BrowserForm();
                return Forms["BrowserForm"] as PrimeTrader.BrowserForm;
            }
        }

        private PriceGridForm priceGridForm
        {
            get
            {
                if (Forms["PriceGridForm"] == null) Forms["PriceGridForm"] = (new PriceGridForm());
                return Forms["PriceGridForm"] as PrimeTrader.PriceGridForm;
            }
        }

        private SettingsForm settingsForm
        {
            get
            {
                if (m_settingsForm == null) m_settingsForm = new SettingsForm();
                return m_settingsForm;
            }
        }

        private MessagesForm messagesForm
        {
            get
            {
                if (Forms["MessagesForm"] == null) Forms["MessagesForm"] = new MessagesForm();
                return Forms["MessagesForm"] as GuiTools.MessagesForm;
            }
        }




        /*private StrategyForm strategyForm
        {
            get
            {
                if (m_strategyForm == null) m_strategyForm = new StrategyForm();
                return m_strategyForm;
            }
        }*/
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------


    } // end of class
} // end of namespace
