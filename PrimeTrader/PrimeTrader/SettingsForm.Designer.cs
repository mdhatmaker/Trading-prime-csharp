namespace PrimeTrader
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.label5 = new System.Windows.Forms.Label();
            this.btnFixBrowserRegKey = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCheckEmbeddedBrowser = new System.Windows.Forms.Button();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageGeneralSettings = new System.Windows.Forms.TabPage();
            this.gridSettings = new System.Windows.Forms.DataGridView();
            this.SettingNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SettingValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPagePythonSettings = new System.Windows.Forms.TabPage();
            this.lblPythonInstallationPath = new System.Windows.Forms.Label();
            this.lblPythonIsInstalled = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lnklblPython27x32 = new System.Windows.Forms.LinkLabel();
            this.lnklblPython27x64 = new System.Windows.Forms.LinkLabel();
            this.tabPageChartBrowser = new System.Windows.Forms.TabPage();
            this.tabPageEnvironmentVariables = new System.Windows.Forms.TabPage();
            this.gridEnvironmentVars = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRefreshSettings = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnIQFeed = new System.Windows.Forms.Button();
            this.btnSaveWindowLocations = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblDescriptionBelow = new System.Windows.Forms.Label();
            this.tabControlSettings.SuspendLayout();
            this.tabPageGeneralSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSettings)).BeginInit();
            this.tabPagePythonSettings.SuspendLayout();
            this.tabPageChartBrowser.SuspendLayout();
            this.tabPageEnvironmentVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEnvironmentVars)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(54, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(227, 42);
            this.label5.TabIndex = 8;
            this.label5.Text = "If your \"Charts\" tab is displaying script errors, try this fix:";
            // 
            // btnFixBrowserRegKey
            // 
            this.btnFixBrowserRegKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFixBrowserRegKey.Location = new System.Drawing.Point(54, 207);
            this.btnFixBrowserRegKey.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFixBrowserRegKey.Name = "btnFixBrowserRegKey";
            this.btnFixBrowserRegKey.Size = new System.Drawing.Size(227, 38);
            this.btnFixBrowserRegKey.TabIndex = 7;
            this.btnFixBrowserRegKey.Text = "Fix Browser Registry Key";
            this.btnFixBrowserRegKey.UseVisualStyleBackColor = true;
            this.btnFixBrowserRegKey.Click += new System.EventHandler(this.btnFixBrowserRegKey_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(54, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(227, 53);
            this.label4.TabIndex = 6;
            this.label4.Text = "The embedded browser is used to displaying HTML charts on the \"Charts\" tab";
            // 
            // btnCheckEmbeddedBrowser
            // 
            this.btnCheckEmbeddedBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckEmbeddedBrowser.Location = new System.Drawing.Point(54, 86);
            this.btnCheckEmbeddedBrowser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCheckEmbeddedBrowser.Name = "btnCheckEmbeddedBrowser";
            this.btnCheckEmbeddedBrowser.Size = new System.Drawing.Size(227, 38);
            this.btnCheckEmbeddedBrowser.TabIndex = 5;
            this.btnCheckEmbeddedBrowser.Text = "Check Embedded Browser...";
            this.btnCheckEmbeddedBrowser.UseVisualStyleBackColor = true;
            this.btnCheckEmbeddedBrowser.Click += new System.EventHandler(this.btnCheckEmbeddedBrowser_Click);
            // 
            // webBrowser
            // 
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(864, 400);
            this.webBrowser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(110, 103);
            this.webBrowser.TabIndex = 10;
            this.webBrowser.Visible = false;
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlSettings.Controls.Add(this.tabPageGeneralSettings);
            this.tabControlSettings.Controls.Add(this.tabPagePythonSettings);
            this.tabControlSettings.Controls.Add(this.tabPageChartBrowser);
            this.tabControlSettings.Controls.Add(this.tabPageEnvironmentVariables);
            this.tabControlSettings.ItemSize = new System.Drawing.Size(49, 31);
            this.tabControlSettings.Location = new System.Drawing.Point(16, 14);
            this.tabControlSettings.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.Padding = new System.Drawing.Point(16, 3);
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(636, 469);
            this.tabControlSettings.TabIndex = 11;
            this.tabControlSettings.SelectedIndexChanged += new System.EventHandler(this.tabControlSettings_SelectedIndexChanged);
            // 
            // tabPageGeneralSettings
            // 
            this.tabPageGeneralSettings.Controls.Add(this.gridSettings);
            this.tabPageGeneralSettings.Location = new System.Drawing.Point(4, 35);
            this.tabPageGeneralSettings.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageGeneralSettings.Name = "tabPageGeneralSettings";
            this.tabPageGeneralSettings.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageGeneralSettings.Size = new System.Drawing.Size(628, 430);
            this.tabPageGeneralSettings.TabIndex = 0;
            this.tabPageGeneralSettings.Text = "General";
            this.tabPageGeneralSettings.UseVisualStyleBackColor = true;
            // 
            // gridSettings
            // 
            this.gridSettings.AllowUserToAddRows = false;
            this.gridSettings.AllowUserToDeleteRows = false;
            this.gridSettings.AllowUserToResizeRows = false;
            this.gridSettings.BackgroundColor = System.Drawing.Color.Silver;
            this.gridSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SettingNameColumn,
            this.SettingValueColumn});
            this.gridSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSettings.Location = new System.Drawing.Point(4, 4);
            this.gridSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridSettings.Name = "gridSettings";
            this.gridSettings.RowHeadersVisible = false;
            this.gridSettings.RowTemplate.Height = 24;
            this.gridSettings.Size = new System.Drawing.Size(620, 422);
            this.gridSettings.TabIndex = 10;
            // 
            // SettingNameColumn
            // 
            this.SettingNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SettingNameColumn.FillWeight = 75F;
            this.SettingNameColumn.HeaderText = "Setting";
            this.SettingNameColumn.Name = "SettingNameColumn";
            // 
            // SettingValueColumn
            // 
            this.SettingValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SettingValueColumn.HeaderText = "Value";
            this.SettingValueColumn.Name = "SettingValueColumn";
            // 
            // tabPagePythonSettings
            // 
            this.tabPagePythonSettings.BackColor = System.Drawing.Color.LightSkyBlue;
            this.tabPagePythonSettings.Controls.Add(this.lblPythonInstallationPath);
            this.tabPagePythonSettings.Controls.Add(this.lblPythonIsInstalled);
            this.tabPagePythonSettings.Controls.Add(this.label3);
            this.tabPagePythonSettings.Controls.Add(this.label2);
            this.tabPagePythonSettings.Controls.Add(this.label1);
            this.tabPagePythonSettings.Controls.Add(this.lnklblPython27x32);
            this.tabPagePythonSettings.Controls.Add(this.lnklblPython27x64);
            this.tabPagePythonSettings.Location = new System.Drawing.Point(4, 35);
            this.tabPagePythonSettings.Margin = new System.Windows.Forms.Padding(4);
            this.tabPagePythonSettings.Name = "tabPagePythonSettings";
            this.tabPagePythonSettings.Padding = new System.Windows.Forms.Padding(4);
            this.tabPagePythonSettings.Size = new System.Drawing.Size(628, 430);
            this.tabPagePythonSettings.TabIndex = 1;
            this.tabPagePythonSettings.Text = "Python";
            // 
            // lblPythonInstallationPath
            // 
            this.lblPythonInstallationPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPythonInstallationPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPythonInstallationPath.Location = new System.Drawing.Point(4, 107);
            this.lblPythonInstallationPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPythonInstallationPath.Name = "lblPythonInstallationPath";
            this.lblPythonInstallationPath.Size = new System.Drawing.Size(613, 28);
            this.lblPythonInstallationPath.TabIndex = 6;
            this.lblPythonInstallationPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPythonIsInstalled
            // 
            this.lblPythonIsInstalled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPythonIsInstalled.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPythonIsInstalled.Location = new System.Drawing.Point(4, 78);
            this.lblPythonIsInstalled.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPythonIsInstalled.Name = "lblPythonIsInstalled";
            this.lblPythonIsInstalled.Size = new System.Drawing.Size(613, 28);
            this.lblPythonIsInstalled.TabIndex = 5;
            this.lblPythonIsInstalled.Text = "Python is installed";
            this.lblPythonIsInstalled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 4);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(605, 28);
            this.label3.TabIndex = 4;
            this.label3.Text = "Settings to install and configure Python";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(67, 375);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(508, 28);
            this.label2.TabIndex = 3;
            this.label2.Text = "Python 2.7 install for Windows (32-bit)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(67, 302);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(508, 28);
            this.label1.TabIndex = 2;
            this.label1.Text = "Python 2.7 install for Windows (64-bit)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lnklblPython27x32
            // 
            this.lnklblPython27x32.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnklblPython27x32.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnklblPython27x32.Location = new System.Drawing.Point(8, 404);
            this.lnklblPython27x32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnklblPython27x32.Name = "lnklblPython27x32";
            this.lnklblPython27x32.Size = new System.Drawing.Size(610, 28);
            this.lnklblPython27x32.TabIndex = 1;
            this.lnklblPython27x32.TabStop = true;
            this.lnklblPython27x32.Text = "https://www.python.org/ftp/python/2.7.13/python-2.7.13.msi";
            this.lnklblPython27x32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnklblPython27x32.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblPython27x32_LinkClicked);
            // 
            // lnklblPython27x64
            // 
            this.lnklblPython27x64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnklblPython27x64.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnklblPython27x64.Location = new System.Drawing.Point(8, 331);
            this.lnklblPython27x64.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnklblPython27x64.Name = "lnklblPython27x64";
            this.lnklblPython27x64.Size = new System.Drawing.Size(610, 28);
            this.lnklblPython27x64.TabIndex = 0;
            this.lnklblPython27x64.TabStop = true;
            this.lnklblPython27x64.Text = "https://www.python.org/ftp/python/2.7.13/python-2.7.13.amd64.msi";
            this.lnklblPython27x64.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnklblPython27x64.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblPython27x64_LinkClicked);
            // 
            // tabPageChartBrowser
            // 
            this.tabPageChartBrowser.BackColor = System.Drawing.Color.DarkGray;
            this.tabPageChartBrowser.Controls.Add(this.btnCheckEmbeddedBrowser);
            this.tabPageChartBrowser.Controls.Add(this.label4);
            this.tabPageChartBrowser.Controls.Add(this.btnFixBrowserRegKey);
            this.tabPageChartBrowser.Controls.Add(this.label5);
            this.tabPageChartBrowser.Location = new System.Drawing.Point(4, 35);
            this.tabPageChartBrowser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPageChartBrowser.Name = "tabPageChartBrowser";
            this.tabPageChartBrowser.Size = new System.Drawing.Size(628, 430);
            this.tabPageChartBrowser.TabIndex = 3;
            this.tabPageChartBrowser.Text = "Chart Browser";
            // 
            // tabPageEnvironmentVariables
            // 
            this.tabPageEnvironmentVariables.BackColor = System.Drawing.Color.LightGreen;
            this.tabPageEnvironmentVariables.Controls.Add(this.gridEnvironmentVars);
            this.tabPageEnvironmentVariables.Location = new System.Drawing.Point(4, 35);
            this.tabPageEnvironmentVariables.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageEnvironmentVariables.Name = "tabPageEnvironmentVariables";
            this.tabPageEnvironmentVariables.Size = new System.Drawing.Size(628, 430);
            this.tabPageEnvironmentVariables.TabIndex = 2;
            this.tabPageEnvironmentVariables.Text = "Environment Vars";
            // 
            // gridEnvironmentVars
            // 
            this.gridEnvironmentVars.AllowUserToAddRows = false;
            this.gridEnvironmentVars.AllowUserToDeleteRows = false;
            this.gridEnvironmentVars.AllowUserToResizeRows = false;
            this.gridEnvironmentVars.BackgroundColor = System.Drawing.Color.LightGreen;
            this.gridEnvironmentVars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridEnvironmentVars.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.gridEnvironmentVars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEnvironmentVars.Location = new System.Drawing.Point(0, 0);
            this.gridEnvironmentVars.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridEnvironmentVars.Name = "gridEnvironmentVars";
            this.gridEnvironmentVars.RowHeadersVisible = false;
            this.gridEnvironmentVars.RowTemplate.Height = 24;
            this.gridEnvironmentVars.Size = new System.Drawing.Size(628, 430);
            this.gridEnvironmentVars.TabIndex = 11;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 75F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Environment Variable";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // btnRefreshSettings
            // 
            this.btnRefreshSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshSettings.Image = global::PrimeTrader.Properties.Resources.refresh_button_icon_blue_64;
            this.btnRefreshSettings.Location = new System.Drawing.Point(673, 389);
            this.btnRefreshSettings.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefreshSettings.Name = "btnRefreshSettings";
            this.btnRefreshSettings.Size = new System.Drawing.Size(105, 94);
            this.btnRefreshSettings.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnRefreshSettings, "Refresh Settings");
            this.btnRefreshSettings.UseVisualStyleBackColor = true;
            this.btnRefreshSettings.Click += new System.EventHandler(this.btnRefreshSettings_Click);
            // 
            // btnIQFeed
            // 
            this.btnIQFeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIQFeed.BackColor = System.Drawing.Color.OliveDrab;
            this.btnIQFeed.Image = ((System.Drawing.Image)(resources.GetObject("btnIQFeed.Image")));
            this.btnIQFeed.Location = new System.Drawing.Point(874, 108);
            this.btnIQFeed.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnIQFeed.Name = "btnIQFeed";
            this.btnIQFeed.Size = new System.Drawing.Size(100, 80);
            this.btnIQFeed.TabIndex = 12;
            this.toolTip1.SetToolTip(this.btnIQFeed, "IQFeed Settings and Performance");
            this.btnIQFeed.UseVisualStyleBackColor = false;
            this.btnIQFeed.Click += new System.EventHandler(this.btnBenchmarkTool_Click);
            // 
            // btnSaveWindowLocations
            // 
            this.btnSaveWindowLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveWindowLocations.BackColor = System.Drawing.Color.White;
            this.btnSaveWindowLocations.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveWindowLocations.Image")));
            this.btnSaveWindowLocations.Location = new System.Drawing.Point(874, 14);
            this.btnSaveWindowLocations.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveWindowLocations.Name = "btnSaveWindowLocations";
            this.btnSaveWindowLocations.Size = new System.Drawing.Size(100, 80);
            this.btnSaveWindowLocations.TabIndex = 13;
            this.toolTip1.SetToolTip(this.btnSaveWindowLocations, "Save settings");
            this.btnSaveWindowLocations.UseVisualStyleBackColor = false;
            this.btnSaveWindowLocations.Click += new System.EventHandler(this.btnSaveWindowLocations_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(673, 125);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(53, 46);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "+";
            this.toolTip1.SetToolTip(this.btnAdd, "Add new setting");
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(673, 177);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(53, 46);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "-";
            this.toolTip1.SetToolTip(this.btnDelete, "Delete selected setting");
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // lblDescriptionBelow
            // 
            this.lblDescriptionBelow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescriptionBelow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescriptionBelow.ForeColor = System.Drawing.Color.White;
            this.lblDescriptionBelow.Location = new System.Drawing.Point(24, 488);
            this.lblDescriptionBelow.Name = "lblDescriptionBelow";
            this.lblDescriptionBelow.Size = new System.Drawing.Size(620, 23);
            this.lblDescriptionBelow.TabIndex = 16;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(984, 517);
            this.Controls.Add(this.lblDescriptionBelow);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnSaveWindowLocations);
            this.Controls.Add(this.btnIQFeed);
            this.Controls.Add(this.btnRefreshSettings);
            this.Controls.Add(this.tabControlSettings);
            this.Controls.Add(this.webBrowser);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageGeneralSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSettings)).EndInit();
            this.tabPagePythonSettings.ResumeLayout(false);
            this.tabPageChartBrowser.ResumeLayout(false);
            this.tabPageEnvironmentVariables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEnvironmentVars)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnFixBrowserRegKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCheckEmbeddedBrowser;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageGeneralSettings;
        private System.Windows.Forms.DataGridView gridSettings;
        private System.Windows.Forms.TabPage tabPagePythonSettings;
        private System.Windows.Forms.TabPage tabPageEnvironmentVariables;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnklblPython27x32;
        private System.Windows.Forms.LinkLabel lnklblPython27x64;
        private System.Windows.Forms.Button btnRefreshSettings;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPythonInstallationPath;
        private System.Windows.Forms.Label lblPythonIsInstalled;
        private System.Windows.Forms.DataGridViewTextBoxColumn SettingNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SettingValueColumn;
        private System.Windows.Forms.TabPage tabPageChartBrowser;
        private System.Windows.Forms.Button btnIQFeed;
        private System.Windows.Forms.Button btnSaveWindowLocations;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView gridEnvironmentVars;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Label lblDescriptionBelow;
    }
}