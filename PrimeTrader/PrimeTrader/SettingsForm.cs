using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using GuiTools;
using Tools;
using static Tools.GSystem;
using static Tools.G;


namespace PrimeTrader
{
    public partial class SettingsForm : Form
    {
        private static readonly string python27x64_install = "https://www.python.org/ftp/python/2.7.13/python-2.7.13.amd64.msi";
        private static readonly string python27x32_install = "https://www.python.org/ftp/python/2.7.13/python-2.7.13.msi";

        public PrimeTraderForm MainPrimeTraderForm { get; set; }

        
        public SettingsForm()
        {
            InitializeComponent();

        }

        private void PopulateGeneralSettings()
        {
            gridSettings.Rows.Clear();
            foreach (var kv in Settings.Instance.SettingsMap)
            {
                gridSettings.Rows.Add(kv.Key, kv.Value);
            }
        }

        private void PopulateEnvironmentVariables()
        {
            gridEnvironmentVars.Rows.Clear();
            foreach (var kv in GSystem.GetEnvironmentVariables())
            {
                gridEnvironmentVars.Rows.Add(kv.Key, kv.Value);
            }
        }

        private void SaveGeneralSettings()
        {
            ErrorMessage("SettingsForm::SaveGeneralSettings=> NOT YET IMPLEMENTED!");
        }

        private void SaveEnvironmentVariables()
        {
            ErrorMessage("SettingsForm::SaveEnvironmentVariables=> NOT YET IMPLEMENTED!");
        }

        private void btnCheckEmbeddedBrowser_Click(object sender, EventArgs e)
        {
            int ieVersion = WebBrowserHelper.GetInstalledIEVersion(webBrowser);
            int version = WebBrowserHelper.GetBrowserVersion();
            int regKeyValue = WebBrowserHelper.GetEmbVersion();
            MessageBox.Show(this, string.Format("Installed IE Version: {0}\nBrowser Version: {1}\nRequired Registry Value: {2}", ieVersion, version, regKeyValue), "Check Browser", MessageBoxButtons.OK);
        }

        private void btnFixBrowserRegKey_Click(object sender, EventArgs e)
        {
            WebBrowserHelper.UpdateWebBrowserRegistryKey(webBrowser);
            MessageBox.Show(this, "Your Browser Registry Key value has been updated.\n\nRestart the PrimeTrader app and try displaying HTML charts on the 'Charts' tab.", "Browser Registry Key", MessageBoxButtons.OK);
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            RefreshSettings();
        }

        private void btnRefreshSettings_Click(object sender, EventArgs e)
        {
            RefreshSettings();
        }

        private void RefreshSettings()
        {
            PopulateGeneralSettings();
            PopulateEnvironmentVariables();
            CheckSelectedTab();

            string filename = "python.exe";
            if (GFile.ExistsOnPath(filename))
            {
                lblPythonIsInstalled.Text = "Python is installed";
                lblPythonIsInstalled.ForeColor = Color.Black;
                lblPythonInstallationPath.Text = GFile.GetFullPath(filename);
            }
            else
            {
                lblPythonIsInstalled.Text = "Python is NOT installed";
                lblPythonIsInstalled.ForeColor = Color.Red;
                lblPythonInstallationPath.Text = "";
            }
        }

        private void lnklblPython27x64_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = lnklblPython27x64.Text;
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                dout("Error occurred attempting to open link '{0}' with Process.Start", url);
            }
        }

        private void lnklblPython27x32_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = lnklblPython27x32.Text;
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                dout("Error occurred attempting to open link '{0}' with Process.Start", url);
            }
        }

        private void btnBenchmarkTool_Click(object sender, EventArgs e)
        {
            //status("IQFeed: Launching Feed");
            if (MainPrimeTraderForm.Forms["IQFeedSettingsForm"] == null) MainPrimeTraderForm.Forms["IQFeedSettingsForm"] = new IQFeed.GUI.IQFeedSettingsForm();
            MainPrimeTraderForm.Forms["IQFeedSettingsForm"].ShowInFront();
        }

        private void btnSaveWindowLocations_Click(object sender, EventArgs e)
        {
            SaveAllFormLocations();
            SaveGeneralSettings();
            SaveEnvironmentVariables();
        }



        #region Save and Restore Form locations/sizes ----------------------------------------------------------------------------------------------------------
        // Saves location info for ALL forms to registry
        public void SaveAllFormLocations()
        {
            foreach (string formName in MainPrimeTraderForm.Forms.Keys)
            {
                var frm = MainPrimeTraderForm.Forms[formName];
                SaveFormLocation(frm, "WindowLocation" + formName);
            }
        }

        // Pass in the Form whose location info you want to save and the Default.Settings property name (i.e. "WindowLocationPrimeTrader")
        public void SaveFormLocation(Form frm, string propertyName)
        {
            if (frm == null || frm.Visible == false)
            {
                //DeleteAppRegistryValue(propertyName)
                Settings.Instance.Delete(propertyName);
                return;
            }

            string state = frm.WindowState.ToString();
            string location = string.Format("{0}, {1}", frm.Location.X, frm.Location.Y);

            // Retrieve window size
            string size = string.Format("{0}, {1}", frm.Size.Width, frm.Size.Height);
            if (frm.WindowState != FormWindowState.Normal)
            {
                size = string.Format("{0}, {1}", frm.RestoreBounds.Size.Width, frm.RestoreBounds.Size.Height);
            }

            string sizeAndLocation = string.Format("{0}, {1}, {2}", state, location, size);
            dout("SAVING {0}=> {1}", propertyName, sizeAndLocation);
            //SetAppRegistryValue(propertyName, sizeAndLocation);
            Settings.Instance[propertyName] = sizeAndLocation;
        }

        // Restore location info for ALL forms from registry
        public void RestoreAllFormLocations()
        {
            foreach (string formName in MainPrimeTraderForm.Forms.Keys.ToList())
            {
                var frm = MainPrimeTraderForm.Forms[formName];
                RestoreFormLocation(frm, "WindowLocation" + formName);
            }
        }

        // Given the Form whose location/size you want to restore and a Default.Settings property name (i.e. "WindowLocationPrimeTraderForm")
        // Restore the form to its saved location and size
        // value in settings should be like "[Normal | Minimized | Maximized], x, y, width, height"
        public void RestoreFormLocation(Form frm, string propertyName)
        {
            /*var props = from SettingsProperty p in Settings.Default.Properties
                        where p.Name == propertyName
                        select p;
            if (props.Count() == 0) return;*/

            // if this property name does not exist in Default.Settings or if this property is null, do nothing
            //if (!SettingsPropertyExists(propertyName)) return;
            //string sizeAndLocation = GetAppRegistryValue<string>(propertyName);
            string sizeAndLocation = Settings.Instance[propertyName];
            if (sizeAndLocation == null) return;

            if (frm == null)
            {
                string formName = propertyName.Substring("WindowLocation".Length);
                frm = CreateForm(formName);                                             // "WindowLocationXXXXXXXXXXXX"
            }
            frm.ShowInFront();

            //string sizeAndLocation = Settings.Default[propertyName] as string;
            dout("{0}=> {1}", propertyName, sizeAndLocation);
            string[] substrings = sizeAndLocation.Split(',');
            string state = substrings[0];
            string locationX = substrings[1];
            string locationY = substrings[2];
            string width = substrings[3];
            string height = substrings[4];

            FormWindowState windowState;
            if (Enum.TryParse<FormWindowState>(state, true, out windowState))
            {
                // Location
                int x, y;
                if (int.TryParse(locationX, out x) && (int.TryParse(locationY, out y)))
                {
                    frm.Location = new Point(x, y);
                }

                // Size
                int w, h;
                if (int.TryParse(width, out w) && (int.TryParse(height, out h)))
                {
                    frm.Size = new Size(w, h);
                }

                // WindowState (Normal, Minimized, Maximized)
                frm.WindowState = windowState;
            }
        }
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------

        public Form CreateForm(string name)
        {
            if (name == "HistoricalDataForm")
                MainPrimeTraderForm.Forms[name] = new IQFeed.GUI.HistoricalDataForm();
            else if (name == "StreamingBarsForm")
                MainPrimeTraderForm.Forms[name] = new IQFeed.GUI.StreamingBarsForm();
            else if (name == "PricesForm")
                MainPrimeTraderForm.Forms[name] = new IQFeed.GUI.PricesForm();
            else if (name == "CryptoInfoForm")
                MainPrimeTraderForm.Forms[name] = new CryptoForms.CryptoInfoForm();
            else if (name == "CryptoTradeForm")
                MainPrimeTraderForm.Forms[name] = new CryptoForms.CryptoTradeForm();
            else if (name == "CryptoGatorForm")
                MainPrimeTraderForm.Forms[name] = new CryptoForms.CryptoGatorForm();
            else if (name == "CryptoPricesForm")
                MainPrimeTraderForm.Forms[name] = new CryptoForms.CryptoPricesForm();
            else if (name == "DataFrameForm")
                MainPrimeTraderForm.Forms[name] = new PrimeTrader.DataFrameForm();
            else if (name == "BrowserForm")
                MainPrimeTraderForm.Forms[name] = new PrimeTrader.BrowserForm();
            else if (name == "PriceGridForm")
                MainPrimeTraderForm.Forms[name] = new PrimeTrader.PriceGridForm();
            else if (name == "MessagesForm")
                MainPrimeTraderForm.Forms[name] = new GuiTools.MessagesForm();

            return MainPrimeTraderForm.Forms[name];
        }

        private void tabControlSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSelectedTab();
        }

        private void ShowAddDeleteButtons(bool b)
        {
            btnAdd.Visible = true;
            btnDelete.Visible = true;
        }

        // Show/hide "+" and "-" buttons AND (depending on selected tab) display text beneath the tab control
        private void CheckSelectedTab()
        {
            if (tabControlSettings.SelectedTab == tabPageGeneralSettings)
            {
                ShowAddDeleteButtons(true);
                lblDescriptionBelow.Text = string.Format("SETTINGS FILENAME: \"{0}\"", Settings.Instance.FilePathname);
            }
            else if (tabControlSettings.SelectedTab == tabPageEnvironmentVariables)
            {
                ShowAddDeleteButtons(true);
                lblDescriptionBelow.Text = "The \"DROPBOXPATH\" environment variable specifies root data folder";
            }
            else
            {
                ShowAddDeleteButtons(false);
                lblDescriptionBelow.Text = "";
            }
        }

    } // end of class
} // end of namespace
