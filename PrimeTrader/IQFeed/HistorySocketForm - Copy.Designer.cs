namespace IQFeed
{
    partial class HistorySocketForm2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoricalDataForm));
            this.lstData = new System.Windows.Forms.ListView();
            this.columnList = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = new System.Windows.Forms.StatusStrip();
            this.tssMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbtnClearOutput = new System.Windows.Forms.ToolStripSplitButton();
            this.tssRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnGetHistory = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.numLastYear = new System.Windows.Forms.NumericUpDown();
            this.numFirstYear = new System.Windows.Forms.NumericUpDown();
            this.txtHistoricalFolder = new System.Windows.Forms.TextBox();
            this.panelContract = new System.Windows.Forms.Panel();
            this.chkDatesInFilename = new System.Windows.Forms.CheckBox();
            this.txtSymbol = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.numHourAdjustContract = new System.Windows.Forms.NumericUpDown();
            this.btnIntervalHour = new System.Windows.Forms.Button();
            this.btnIntervalMinute = new System.Windows.Forms.Button();
            this.Label13 = new System.Windows.Forms.Label();
            this.rbTick = new System.Windows.Forms.RadioButton();
            this.rbVolume = new System.Windows.Forms.RadioButton();
            this.rbTime = new System.Windows.Forms.RadioButton();
            this.btnGetData = new System.Windows.Forms.Button();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtRequestID = new System.Windows.Forms.TextBox();
            this.txtDirection = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtDatapointsPerSend = new System.Windows.Forms.TextBox();
            this.txtEndFilterTime = new System.Windows.Forms.TextBox();
            this.txtBeginFilterTime = new System.Windows.Forms.TextBox();
            this.txtEndDateTime = new System.Windows.Forms.TextBox();
            this.txtBeginDateTime = new System.Windows.Forms.TextBox();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.txtDays = new System.Windows.Forms.TextBox();
            this.txtDatapoints = new System.Windows.Forms.TextBox();
            this.cboHistoryType = new System.Windows.Forms.ComboBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.panelFutures = new System.Windows.Forms.Panel();
            this.cboSymbolRoot = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.numHourAdjustFutures = new System.Windows.Forms.NumericUpDown();
            this.cboFuturesInterval = new System.Windows.Forms.ComboBox();
            this.btnRefreshAllData = new System.Windows.Forms.Button();
            this.listDataSymbols = new System.Windows.Forms.ListBox();
            this.listDataTimeFrames = new System.Windows.Forms.ListBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lvDataFiles = new System.Windows.Forms.ListView();
            this.columnFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblDataFileCount = new System.Windows.Forms.Label();
            this.btnRefreshSelectedSymbol = new System.Windows.Forms.Button();
            this.panelSymbols = new System.Windows.Forms.Panel();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.cboSecurityTypes = new System.Windows.Forms.ComboBox();
            this.btnClearSymbolSearch = new System.Windows.Forms.Button();
            this.cboListedMarkets = new System.Windows.Forms.ComboBox();
            this.btnSymbolSearch = new System.Windows.Forms.Button();
            this.txtSymbolSearch = new System.Windows.Forms.TextBox();
            this.status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLastYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFirstYear)).BeginInit();
            this.panelContract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHourAdjustContract)).BeginInit();
            this.panelFutures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHourAdjustFutures)).BeginInit();
            this.panelSymbols.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstData
            // 
            this.lstData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstData.BackColor = System.Drawing.Color.Black;
            this.lstData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnList});
            this.lstData.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstData.ForeColor = System.Drawing.Color.Lime;
            this.lstData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstData.Location = new System.Drawing.Point(537, 237);
            this.lstData.Margin = new System.Windows.Forms.Padding(4);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(522, 537);
            this.lstData.TabIndex = 112;
            this.lstData.UseCompatibleStateImageBehavior = false;
            this.lstData.View = System.Windows.Forms.View.Details;
            this.lstData.Resize += new System.EventHandler(this.lstData_Resize);
            // 
            // columnList
            // 
            this.columnList.Text = "";
            // 
            // status
            // 
            this.status.AutoSize = false;
            this.status.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssMain,
            this.tsbtnClearOutput,
            this.tssRight});
            this.status.Location = new System.Drawing.Point(0, 781);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(1065, 40);
            this.status.TabIndex = 113;
            this.status.Text = "statusStrip1";
            // 
            // tssMain
            // 
            this.tssMain.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssMain.Font = new System.Drawing.Font("Courier New", 9F);
            this.tssMain.Name = "tssMain";
            this.tssMain.Size = new System.Drawing.Size(788, 35);
            this.tssMain.Spring = true;
            this.tssMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tssMain.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // tsbtnClearOutput
            // 
            this.tsbtnClearOutput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnClearOutput.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnClearOutput.Image")));
            this.tsbtnClearOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnClearOutput.Name = "tsbtnClearOutput";
            this.tsbtnClearOutput.Size = new System.Drawing.Size(112, 38);
            this.tsbtnClearOutput.Text = "Clear Output";
            this.tsbtnClearOutput.ToolTipText = "Clear output window";
            this.tsbtnClearOutput.Click += new System.EventHandler(this.tsbtnClearOutput_Click);
            // 
            // tssRight
            // 
            this.tssRight.AutoSize = false;
            this.tssRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssRight.Font = new System.Drawing.Font("Courier New", 9F);
            this.tssRight.Name = "tssRight";
            this.tssRight.Size = new System.Drawing.Size(150, 35);
            this.tssRight.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // btnGetHistory
            // 
            this.btnGetHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetHistory.Location = new System.Drawing.Point(401, 30);
            this.btnGetHistory.Name = "btnGetHistory";
            this.btnGetHistory.Size = new System.Drawing.Size(189, 38);
            this.btnGetHistory.TabIndex = 114;
            this.btnGetHistory.Text = "Get Futures Contracts";
            this.btnGetHistory.UseVisualStyleBackColor = true;
            this.btnGetHistory.Click += new System.EventHandler(this.btnGetFuturesContracts_Click);
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.SystemColors.Control;
            this.label14.Cursor = System.Windows.Forms.Cursors.Default;
            this.label14.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(268, 16);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label14.Size = new System.Drawing.Size(117, 25);
            this.label14.TabIndex = 117;
            this.label14.Text = "Symbol Root:";
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.Control;
            this.label15.Cursor = System.Windows.Forms.Cursors.Default;
            this.label15.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Location = new System.Drawing.Point(139, 16);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label15.Size = new System.Drawing.Size(117, 25);
            this.label15.TabIndex = 119;
            this.label15.Text = "Last Year:";
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.SystemColors.Control;
            this.label16.Cursor = System.Windows.Forms.Cursors.Default;
            this.label16.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label16.Location = new System.Drawing.Point(16, 16);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label16.Size = new System.Drawing.Size(117, 25);
            this.label16.TabIndex = 121;
            this.label16.Text = "First Year:";
            // 
            // numLastYear
            // 
            this.numLastYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numLastYear.Location = new System.Drawing.Point(138, 44);
            this.numLastYear.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numLastYear.Minimum = new decimal(new int[] {
            1990,
            0,
            0,
            0});
            this.numLastYear.Name = "numLastYear";
            this.numLastYear.Size = new System.Drawing.Size(102, 30);
            this.numLastYear.TabIndex = 122;
            this.numLastYear.Value = new decimal(new int[] {
            2017,
            0,
            0,
            0});
            // 
            // numFirstYear
            // 
            this.numFirstYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numFirstYear.Location = new System.Drawing.Point(13, 44);
            this.numFirstYear.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numFirstYear.Minimum = new decimal(new int[] {
            1990,
            0,
            0,
            0});
            this.numFirstYear.Name = "numFirstYear";
            this.numFirstYear.Size = new System.Drawing.Size(102, 30);
            this.numFirstYear.TabIndex = 123;
            this.numFirstYear.Value = new decimal(new int[] {
            2010,
            0,
            0,
            0});
            // 
            // txtHistoricalFolder
            // 
            this.txtHistoricalFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtHistoricalFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHistoricalFolder.Location = new System.Drawing.Point(164, 754);
            this.txtHistoricalFolder.Name = "txtHistoricalFolder";
            this.txtHistoricalFolder.ReadOnly = true;
            this.txtHistoricalFolder.Size = new System.Drawing.Size(357, 23);
            this.txtHistoricalFolder.TabIndex = 124;
            this.txtHistoricalFolder.Text = "C:\\Users\\Trader\\Dropbox\\alvin\\data\\DF_DATA";
            // 
            // panelContract
            // 
            this.panelContract.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContract.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelContract.Controls.Add(this.chkDatesInFilename);
            this.panelContract.Controls.Add(this.txtSymbol);
            this.panelContract.Controls.Add(this.label17);
            this.panelContract.Controls.Add(this.numHourAdjustContract);
            this.panelContract.Controls.Add(this.btnIntervalHour);
            this.panelContract.Controls.Add(this.btnIntervalMinute);
            this.panelContract.Controls.Add(this.Label13);
            this.panelContract.Controls.Add(this.rbTick);
            this.panelContract.Controls.Add(this.rbVolume);
            this.panelContract.Controls.Add(this.rbTime);
            this.panelContract.Controls.Add(this.btnGetData);
            this.panelContract.Controls.Add(this.Label12);
            this.panelContract.Controls.Add(this.Label11);
            this.panelContract.Controls.Add(this.Label10);
            this.panelContract.Controls.Add(this.Label9);
            this.panelContract.Controls.Add(this.Label8);
            this.panelContract.Controls.Add(this.Label7);
            this.panelContract.Controls.Add(this.Label6);
            this.panelContract.Controls.Add(this.txtRequestID);
            this.panelContract.Controls.Add(this.txtDirection);
            this.panelContract.Controls.Add(this.Label5);
            this.panelContract.Controls.Add(this.Label4);
            this.panelContract.Controls.Add(this.Label3);
            this.panelContract.Controls.Add(this.txtDatapointsPerSend);
            this.panelContract.Controls.Add(this.txtEndFilterTime);
            this.panelContract.Controls.Add(this.txtBeginFilterTime);
            this.panelContract.Controls.Add(this.txtEndDateTime);
            this.panelContract.Controls.Add(this.txtBeginDateTime);
            this.panelContract.Controls.Add(this.txtInterval);
            this.panelContract.Controls.Add(this.txtDays);
            this.panelContract.Controls.Add(this.txtDatapoints);
            this.panelContract.Controls.Add(this.cboHistoryType);
            this.panelContract.Controls.Add(this.Label2);
            this.panelContract.Controls.Add(this.Label1);
            this.panelContract.Location = new System.Drawing.Point(9, 12);
            this.panelContract.Name = "panelContract";
            this.panelContract.Size = new System.Drawing.Size(1050, 112);
            this.panelContract.TabIndex = 128;
            // 
            // chkDatesInFilename
            // 
            this.chkDatesInFilename.AutoSize = true;
            this.chkDatesInFilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDatesInFilename.Location = new System.Drawing.Point(813, 5);
            this.chkDatesInFilename.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDatesInFilename.Name = "chkDatesInFilename";
            this.chkDatesInFilename.Size = new System.Drawing.Size(143, 21);
            this.chkDatesInFilename.TabIndex = 164;
            this.chkDatesInFilename.Text = "Dates in Filename";
            this.chkDatesInFilename.UseVisualStyleBackColor = true;
            // 
            // txtSymbol
            // 
            this.txtSymbol.FormattingEnabled = true;
            this.txtSymbol.Location = new System.Drawing.Point(164, 21);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(121, 24);
            this.txtSymbol.TabIndex = 163;
            this.txtSymbol.Text = "VIX.XO";
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.SystemColors.Control;
            this.label17.Cursor = System.Windows.Forms.Cursors.Default;
            this.label17.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label17.Location = new System.Drawing.Point(814, 82);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label17.Size = new System.Drawing.Size(83, 20);
            this.label17.TabIndex = 159;
            this.label17.Text = "hour adjust:";
            // 
            // numHourAdjustContract
            // 
            this.numHourAdjustContract.Location = new System.Drawing.Point(904, 80);
            this.numHourAdjustContract.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numHourAdjustContract.Minimum = new decimal(new int[] {
            23,
            0,
            0,
            -2147483648});
            this.numHourAdjustContract.Name = "numHourAdjustContract";
            this.numHourAdjustContract.Size = new System.Drawing.Size(55, 22);
            this.numHourAdjustContract.TabIndex = 158;
            this.numHourAdjustContract.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnIntervalHour
            // 
            this.btnIntervalHour.Location = new System.Drawing.Point(448, 26);
            this.btnIntervalHour.Name = "btnIntervalHour";
            this.btnIntervalHour.Size = new System.Drawing.Size(47, 23);
            this.btnIntervalHour.TabIndex = 157;
            this.btnIntervalHour.Text = "1hr";
            this.btnIntervalHour.UseVisualStyleBackColor = true;
            this.btnIntervalHour.Click += new System.EventHandler(this.btnIntervalHour_Click);
            // 
            // btnIntervalMinute
            // 
            this.btnIntervalMinute.Location = new System.Drawing.Point(448, 3);
            this.btnIntervalMinute.Name = "btnIntervalMinute";
            this.btnIntervalMinute.Size = new System.Drawing.Size(47, 23);
            this.btnIntervalMinute.TabIndex = 156;
            this.btnIntervalMinute.Text = "1min";
            this.btnIntervalMinute.UseVisualStyleBackColor = true;
            this.btnIntervalMinute.Click += new System.EventHandler(this.btnIntervalMinute_Click);
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(580, 0);
            this.Label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(90, 17);
            this.Label13.TabIndex = 154;
            this.Label13.Text = "Interval Type";
            // 
            // rbTick
            // 
            this.rbTick.AutoSize = true;
            this.rbTick.Location = new System.Drawing.Point(717, 21);
            this.rbTick.Margin = new System.Windows.Forms.Padding(4);
            this.rbTick.Name = "rbTick";
            this.rbTick.Size = new System.Drawing.Size(55, 21);
            this.rbTick.TabIndex = 153;
            this.rbTick.Text = "Tick";
            this.rbTick.UseVisualStyleBackColor = true;
            // 
            // rbVolume
            // 
            this.rbVolume.AutoSize = true;
            this.rbVolume.Location = new System.Drawing.Point(639, 21);
            this.rbVolume.Margin = new System.Windows.Forms.Padding(4);
            this.rbVolume.Name = "rbVolume";
            this.rbVolume.Size = new System.Drawing.Size(76, 21);
            this.rbVolume.TabIndex = 152;
            this.rbVolume.Text = "Volume";
            this.rbVolume.UseVisualStyleBackColor = true;
            // 
            // rbTime
            // 
            this.rbTime.AutoSize = true;
            this.rbTime.Checked = true;
            this.rbTime.Location = new System.Drawing.Point(578, 21);
            this.rbTime.Margin = new System.Windows.Forms.Padding(4);
            this.rbTime.Name = "rbTime";
            this.rbTime.Size = new System.Drawing.Size(60, 21);
            this.rbTime.TabIndex = 151;
            this.rbTime.TabStop = true;
            this.rbTime.Text = "Time";
            this.rbTime.UseVisualStyleBackColor = true;
            // 
            // btnGetData
            // 
            this.btnGetData.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetData.Location = new System.Drawing.Point(791, 30);
            this.btnGetData.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(178, 38);
            this.btnGetData.TabIndex = 150;
            this.btnGetData.Text = "Get Contract Data";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetContractData_Click);
            // 
            // Label12
            // 
            this.Label12.BackColor = System.Drawing.SystemColors.Control;
            this.Label12.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label12.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label12.Location = new System.Drawing.Point(454, 57);
            this.Label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label12.Name = "Label12";
            this.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label12.Size = new System.Drawing.Size(74, 18);
            this.Label12.TabIndex = 149;
            this.Label12.Text = "dpts/send";
            // 
            // Label11
            // 
            this.Label11.BackColor = System.Drawing.SystemColors.Control;
            this.Label11.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label11.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label11.Location = new System.Drawing.Point(331, 58);
            this.Label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label11.Name = "Label11";
            this.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label11.Size = new System.Drawing.Size(123, 21);
            this.Label11.TabIndex = 148;
            this.Label11.Text = "End Filter Time";
            // 
            // Label10
            // 
            this.Label10.BackColor = System.Drawing.SystemColors.Control;
            this.Label10.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label10.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label10.Location = new System.Drawing.Point(218, 58);
            this.Label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label10.Name = "Label10";
            this.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label10.Size = new System.Drawing.Size(125, 21);
            this.Label10.TabIndex = 147;
            this.Label10.Text = "Begin Filter Time";
            // 
            // Label9
            // 
            this.Label9.BackColor = System.Drawing.SystemColors.Control;
            this.Label9.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label9.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label9.Location = new System.Drawing.Point(113, 58);
            this.Label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label9.Name = "Label9";
            this.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label9.Size = new System.Drawing.Size(123, 21);
            this.Label9.TabIndex = 146;
            this.Label9.Text = "End DateTime";
            // 
            // Label8
            // 
            this.Label8.BackColor = System.Drawing.SystemColors.Control;
            this.Label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label8.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label8.Location = new System.Drawing.Point(3, 58);
            this.Label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label8.Name = "Label8";
            this.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label8.Size = new System.Drawing.Size(123, 21);
            this.Label8.TabIndex = 145;
            this.Label8.Text = "Begin DateTime";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(597, 57);
            this.Label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(74, 17);
            this.Label7.TabIndex = 144;
            this.Label7.Text = "RequestID";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(526, 57);
            this.Label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(64, 17);
            this.Label6.TabIndex = 143;
            this.Label6.Text = "Direction";
            // 
            // txtRequestID
            // 
            this.txtRequestID.Location = new System.Drawing.Point(588, 78);
            this.txtRequestID.Margin = new System.Windows.Forms.Padding(4);
            this.txtRequestID.Name = "txtRequestID";
            this.txtRequestID.Size = new System.Drawing.Size(94, 22);
            this.txtRequestID.TabIndex = 142;
            this.txtRequestID.Text = "CSREQUEST";
            // 
            // txtDirection
            // 
            this.txtDirection.Location = new System.Drawing.Point(545, 78);
            this.txtDirection.Margin = new System.Windows.Forms.Padding(4);
            this.txtDirection.Name = "txtDirection";
            this.txtDirection.Size = new System.Drawing.Size(27, 22);
            this.txtDirection.TabIndex = 141;
            this.txtDirection.Text = "1";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(504, 0);
            this.Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(54, 17);
            this.Label5.TabIndex = 140;
            this.Label5.Text = "Interval";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(373, 0);
            this.Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(40, 17);
            this.Label4.TabIndex = 139;
            this.Label4.Text = "Days";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(288, 0);
            this.Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(76, 17);
            this.Label3.TabIndex = 138;
            this.Label3.Text = "Datapoints";
            // 
            // txtDatapointsPerSend
            // 
            this.txtDatapointsPerSend.Location = new System.Drawing.Point(457, 78);
            this.txtDatapointsPerSend.Margin = new System.Windows.Forms.Padding(4);
            this.txtDatapointsPerSend.Name = "txtDatapointsPerSend";
            this.txtDatapointsPerSend.Size = new System.Drawing.Size(66, 22);
            this.txtDatapointsPerSend.TabIndex = 137;
            this.txtDatapointsPerSend.Text = "5000";
            // 
            // txtEndFilterTime
            // 
            this.txtEndFilterTime.Location = new System.Drawing.Point(329, 79);
            this.txtEndFilterTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtEndFilterTime.Name = "txtEndFilterTime";
            this.txtEndFilterTime.Size = new System.Drawing.Size(102, 22);
            this.txtEndFilterTime.TabIndex = 136;
            // 
            // txtBeginFilterTime
            // 
            this.txtBeginFilterTime.Location = new System.Drawing.Point(223, 79);
            this.txtBeginFilterTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtBeginFilterTime.Name = "txtBeginFilterTime";
            this.txtBeginFilterTime.Size = new System.Drawing.Size(102, 22);
            this.txtBeginFilterTime.TabIndex = 135;
            // 
            // txtEndDateTime
            // 
            this.txtEndDateTime.Location = new System.Drawing.Point(111, 79);
            this.txtEndDateTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtEndDateTime.Name = "txtEndDateTime";
            this.txtEndDateTime.Size = new System.Drawing.Size(102, 22);
            this.txtEndDateTime.TabIndex = 134;
            this.txtEndDateTime.Text = "20170712";
            // 
            // txtBeginDateTime
            // 
            this.txtBeginDateTime.Location = new System.Drawing.Point(6, 79);
            this.txtBeginDateTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtBeginDateTime.Name = "txtBeginDateTime";
            this.txtBeginDateTime.Size = new System.Drawing.Size(102, 22);
            this.txtBeginDateTime.TabIndex = 133;
            this.txtBeginDateTime.Text = "20100101";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(502, 21);
            this.txtInterval.Margin = new System.Windows.Forms.Padding(4);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(56, 22);
            this.txtInterval.TabIndex = 132;
            // 
            // txtDays
            // 
            this.txtDays.Location = new System.Drawing.Point(377, 21);
            this.txtDays.Margin = new System.Windows.Forms.Padding(4);
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(36, 22);
            this.txtDays.TabIndex = 131;
            this.txtDays.Text = "1";
            // 
            // txtDatapoints
            // 
            this.txtDatapoints.Location = new System.Drawing.Point(292, 21);
            this.txtDatapoints.Margin = new System.Windows.Forms.Padding(4);
            this.txtDatapoints.Name = "txtDatapoints";
            this.txtDatapoints.Size = new System.Drawing.Size(76, 22);
            this.txtDatapoints.TabIndex = 130;
            // 
            // cboHistoryType
            // 
            this.cboHistoryType.BackColor = System.Drawing.SystemColors.Window;
            this.cboHistoryType.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboHistoryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHistoryType.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboHistoryType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboHistoryType.Location = new System.Drawing.Point(8, 21);
            this.cboHistoryType.Margin = new System.Windows.Forms.Padding(4);
            this.cboHistoryType.Name = "cboHistoryType";
            this.cboHistoryType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cboHistoryType.Size = new System.Drawing.Size(149, 24);
            this.cboHistoryType.TabIndex = 127;
            this.cboHistoryType.SelectedIndexChanged += new System.EventHandler(this.cboHistoryType_SelectedIndexChanged);
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.SystemColors.Control;
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label2.Location = new System.Drawing.Point(4, 0);
            this.Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(44, 21);
            this.Label2.TabIndex = 129;
            this.Label2.Text = "Type:";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.SystemColors.Control;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label1.Location = new System.Drawing.Point(161, 0);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(69, 21);
            this.Label1.TabIndex = 128;
            this.Label1.Text = "Symbol:";
            // 
            // panelFutures
            // 
            this.panelFutures.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFutures.Controls.Add(this.cboSymbolRoot);
            this.panelFutures.Controls.Add(this.label21);
            this.panelFutures.Controls.Add(this.numHourAdjustFutures);
            this.panelFutures.Controls.Add(this.cboFuturesInterval);
            this.panelFutures.Controls.Add(this.numFirstYear);
            this.panelFutures.Controls.Add(this.label16);
            this.panelFutures.Controls.Add(this.numLastYear);
            this.panelFutures.Controls.Add(this.label15);
            this.panelFutures.Controls.Add(this.btnGetHistory);
            this.panelFutures.Controls.Add(this.label14);
            this.panelFutures.Location = new System.Drawing.Point(9, 130);
            this.panelFutures.Name = "panelFutures";
            this.panelFutures.Size = new System.Drawing.Size(603, 100);
            this.panelFutures.TabIndex = 129;
            // 
            // cboSymbolRoot
            // 
            this.cboSymbolRoot.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSymbolRoot.FormattingEnabled = true;
            this.cboSymbolRoot.Location = new System.Drawing.Point(268, 44);
            this.cboSymbolRoot.Name = "cboSymbolRoot";
            this.cboSymbolRoot.Size = new System.Drawing.Size(113, 31);
            this.cboSymbolRoot.Sorted = true;
            this.cboSymbolRoot.TabIndex = 162;
            this.cboSymbolRoot.Text = "@ES";
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.SystemColors.Control;
            this.label21.Cursor = System.Windows.Forms.Cursors.Default;
            this.label21.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label21.Location = new System.Drawing.Point(417, 75);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label21.Size = new System.Drawing.Size(83, 20);
            this.label21.TabIndex = 161;
            this.label21.Text = "hour adjust:";
            // 
            // numHourAdjustFutures
            // 
            this.numHourAdjustFutures.Location = new System.Drawing.Point(504, 73);
            this.numHourAdjustFutures.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numHourAdjustFutures.Minimum = new decimal(new int[] {
            23,
            0,
            0,
            -2147483648});
            this.numHourAdjustFutures.Name = "numHourAdjustFutures";
            this.numHourAdjustFutures.Size = new System.Drawing.Size(55, 22);
            this.numHourAdjustFutures.TabIndex = 160;
            this.numHourAdjustFutures.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cboFuturesInterval
            // 
            this.cboFuturesInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFuturesInterval.FormattingEnabled = true;
            this.cboFuturesInterval.Items.AddRange(new object[] {
            "1 Hour",
            "1 Minute",
            "Daily"});
            this.cboFuturesInterval.Location = new System.Drawing.Point(415, 3);
            this.cboFuturesInterval.Name = "cboFuturesInterval";
            this.cboFuturesInterval.Size = new System.Drawing.Size(161, 24);
            this.cboFuturesInterval.TabIndex = 127;
            // 
            // btnRefreshAllData
            // 
            this.btnRefreshAllData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshAllData.Location = new System.Drawing.Point(9, 746);
            this.btnRefreshAllData.Name = "btnRefreshAllData";
            this.btnRefreshAllData.Size = new System.Drawing.Size(134, 28);
            this.btnRefreshAllData.TabIndex = 131;
            this.btnRefreshAllData.Text = "Refresh All Data";
            this.btnRefreshAllData.UseVisualStyleBackColor = true;
            this.btnRefreshAllData.Click += new System.EventHandler(this.btnRefreshAllData_Click);
            // 
            // listDataSymbols
            // 
            this.listDataSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listDataSymbols.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listDataSymbols.FormattingEnabled = true;
            this.listDataSymbols.ItemHeight = 20;
            this.listDataSymbols.Location = new System.Drawing.Point(9, 397);
            this.listDataSymbols.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listDataSymbols.Name = "listDataSymbols";
            this.listDataSymbols.Size = new System.Drawing.Size(134, 264);
            this.listDataSymbols.Sorted = true;
            this.listDataSymbols.TabIndex = 133;
            this.listDataSymbols.SelectedIndexChanged += new System.EventHandler(this.listDataSymbols_SelectedIndexChanged);
            // 
            // listDataTimeFrames
            // 
            this.listDataTimeFrames.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listDataTimeFrames.FormattingEnabled = true;
            this.listDataTimeFrames.ItemHeight = 20;
            this.listDataTimeFrames.Location = new System.Drawing.Point(11, 269);
            this.listDataTimeFrames.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listDataTimeFrames.Name = "listDataTimeFrames";
            this.listDataTimeFrames.Size = new System.Drawing.Size(132, 64);
            this.listDataTimeFrames.Sorted = true;
            this.listDataTimeFrames.TabIndex = 134;
            this.listDataTimeFrames.SelectedIndexChanged += new System.EventHandler(this.listDataTimeFrames_SelectedIndexChanged);
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(164, 237);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(342, 30);
            this.label18.TabIndex = 135;
            this.label18.Text = "Data Files";
            this.label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(8, 237);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(134, 30);
            this.label19.TabIndex = 136;
            this.label19.Text = "Time Frames";
            this.label19.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(8, 370);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(134, 24);
            this.label20.TabIndex = 137;
            this.label20.Text = "Symbols";
            this.label20.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lvDataFiles
            // 
            this.lvDataFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvDataFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFilename,
            this.columnDate});
            this.lvDataFiles.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDataFiles.Location = new System.Drawing.Point(164, 269);
            this.lvDataFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lvDataFiles.Name = "lvDataFiles";
            this.lvDataFiles.Size = new System.Drawing.Size(355, 477);
            this.lvDataFiles.TabIndex = 138;
            this.lvDataFiles.UseCompatibleStateImageBehavior = false;
            this.lvDataFiles.View = System.Windows.Forms.View.Details;
            this.lvDataFiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvDataFiles_ColumnClick);
            // 
            // columnFilename
            // 
            this.columnFilename.Text = "Filename";
            this.columnFilename.Width = 165;
            // 
            // columnDate
            // 
            this.columnDate.Text = "Last Update";
            this.columnDate.Width = 80;
            // 
            // lblDataFileCount
            // 
            this.lblDataFileCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataFileCount.Location = new System.Drawing.Point(437, 237);
            this.lblDataFileCount.Name = "lblDataFileCount";
            this.lblDataFileCount.Size = new System.Drawing.Size(81, 30);
            this.lblDataFileCount.TabIndex = 139;
            this.lblDataFileCount.Text = "0 files";
            this.lblDataFileCount.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnRefreshSelectedSymbol
            // 
            this.btnRefreshSelectedSymbol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshSelectedSymbol.Location = new System.Drawing.Point(9, 708);
            this.btnRefreshSelectedSymbol.Name = "btnRefreshSelectedSymbol";
            this.btnRefreshSelectedSymbol.Size = new System.Drawing.Size(134, 28);
            this.btnRefreshSelectedSymbol.TabIndex = 140;
            this.btnRefreshSelectedSymbol.Text = "Refresh Selected";
            this.btnRefreshSelectedSymbol.UseVisualStyleBackColor = true;
            this.btnRefreshSelectedSymbol.Click += new System.EventHandler(this.btnRefreshSelectedSymbol_Click);
            // 
            // panelSymbols
            // 
            this.panelSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSymbols.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSymbols.Controls.Add(this.label23);
            this.panelSymbols.Controls.Add(this.label22);
            this.panelSymbols.Controls.Add(this.cboSecurityTypes);
            this.panelSymbols.Controls.Add(this.btnClearSymbolSearch);
            this.panelSymbols.Controls.Add(this.cboListedMarkets);
            this.panelSymbols.Controls.Add(this.btnSymbolSearch);
            this.panelSymbols.Controls.Add(this.txtSymbolSearch);
            this.panelSymbols.Location = new System.Drawing.Point(618, 130);
            this.panelSymbols.Name = "panelSymbols";
            this.panelSymbols.Size = new System.Drawing.Size(441, 100);
            this.panelSymbols.TabIndex = 141;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.SystemColors.Control;
            this.label23.Cursor = System.Windows.Forms.Cursors.Default;
            this.label23.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label23.Location = new System.Drawing.Point(49, 70);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label23.Size = new System.Drawing.Size(55, 16);
            this.label23.TabIndex = 135;
            this.label23.Text = "Market:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.SystemColors.Control;
            this.label22.Cursor = System.Windows.Forms.Cursors.Default;
            this.label22.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label22.Location = new System.Drawing.Point(6, 40);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label22.Size = new System.Drawing.Size(98, 16);
            this.label22.TabIndex = 134;
            this.label22.Text = "Security Type:";
            // 
            // cboSecurityTypes
            // 
            this.cboSecurityTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSecurityTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSecurityTypes.FormattingEnabled = true;
            this.cboSecurityTypes.Items.AddRange(new object[] {
            "1 Hour",
            "1 Minute",
            "Daily"});
            this.cboSecurityTypes.Location = new System.Drawing.Point(108, 37);
            this.cboSecurityTypes.Name = "cboSecurityTypes";
            this.cboSecurityTypes.Size = new System.Drawing.Size(323, 24);
            this.cboSecurityTypes.TabIndex = 133;
            this.cboSecurityTypes.SelectedIndexChanged += new System.EventHandler(this.cboSecurityTypes_SelectedIndexChanged);
            // 
            // btnClearSymbolSearch
            // 
            this.btnClearSymbolSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSymbolSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearSymbolSearch.Location = new System.Drawing.Point(372, 3);
            this.btnClearSymbolSearch.Name = "btnClearSymbolSearch";
            this.btnClearSymbolSearch.Size = new System.Drawing.Size(60, 30);
            this.btnClearSymbolSearch.TabIndex = 132;
            this.btnClearSymbolSearch.Text = "Clear";
            this.btnClearSymbolSearch.UseVisualStyleBackColor = true;
            this.btnClearSymbolSearch.Click += new System.EventHandler(this.btnClearSymbolSearch_Click);
            // 
            // cboListedMarkets
            // 
            this.cboListedMarkets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboListedMarkets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboListedMarkets.FormattingEnabled = true;
            this.cboListedMarkets.Items.AddRange(new object[] {
            "1 Hour",
            "1 Minute",
            "Daily"});
            this.cboListedMarkets.Location = new System.Drawing.Point(108, 67);
            this.cboListedMarkets.Name = "cboListedMarkets";
            this.cboListedMarkets.Size = new System.Drawing.Size(323, 24);
            this.cboListedMarkets.TabIndex = 131;
            this.cboListedMarkets.SelectedIndexChanged += new System.EventHandler(this.cboListedMarkets_SelectedIndexChanged);
            // 
            // btnSymbolSearch
            // 
            this.btnSymbolSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSymbolSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSymbolSearch.Location = new System.Drawing.Point(297, 3);
            this.btnSymbolSearch.Name = "btnSymbolSearch";
            this.btnSymbolSearch.Size = new System.Drawing.Size(73, 30);
            this.btnSymbolSearch.TabIndex = 130;
            this.btnSymbolSearch.Text = "Search";
            this.btnSymbolSearch.UseVisualStyleBackColor = true;
            this.btnSymbolSearch.Click += new System.EventHandler(this.btnSymbolSearch_Click);
            // 
            // txtSymbolSearch
            // 
            this.txtSymbolSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSymbolSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSymbolSearch.Location = new System.Drawing.Point(8, 5);
            this.txtSymbolSearch.Name = "txtSymbolSearch";
            this.txtSymbolSearch.Size = new System.Drawing.Size(283, 26);
            this.txtSymbolSearch.TabIndex = 0;
            this.txtSymbolSearch.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtSymbolSearch_PreviewKeyDown);
            // 
            // HistorySocketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 821);
            this.Controls.Add(this.panelSymbols);
            this.Controls.Add(this.btnRefreshSelectedSymbol);
            this.Controls.Add(this.lblDataFileCount);
            this.Controls.Add(this.lvDataFiles);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.listDataTimeFrames);
            this.Controls.Add(this.listDataSymbols);
            this.Controls.Add(this.btnRefreshAllData);
            this.Controls.Add(this.panelFutures);
            this.Controls.Add(this.panelContract);
            this.Controls.Add(this.txtHistoricalFolder);
            this.Controls.Add(this.status);
            this.Controls.Add(this.lstData);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HistorySocketForm";
            this.Text = "IQFeed: Historical Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HistorySocketForm_FormClosing);
            this.Load += new System.EventHandler(this.HistorySocketForm_Load);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLastYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFirstYear)).EndInit();
            this.panelContract.ResumeLayout(false);
            this.panelContract.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHourAdjustContract)).EndInit();
            this.panelFutures.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numHourAdjustFutures)).EndInit();
            this.panelSymbols.ResumeLayout(false);
            this.panelSymbols.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView lstData;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel tssMain;
        private System.Windows.Forms.ToolStripStatusLabel tssRight;
        private System.Windows.Forms.Button btnGetHistory;
        public System.Windows.Forms.Label label14;
        public System.Windows.Forms.Label label15;
        public System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown numLastYear;
        private System.Windows.Forms.NumericUpDown numFirstYear;
        private System.Windows.Forms.TextBox txtHistoricalFolder;
        private System.Windows.Forms.Panel panelContract;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.RadioButton rbTick;
        internal System.Windows.Forms.RadioButton rbVolume;
        internal System.Windows.Forms.RadioButton rbTime;
        internal System.Windows.Forms.Button btnGetData;
        public System.Windows.Forms.Label Label12;
        public System.Windows.Forms.Label Label11;
        public System.Windows.Forms.Label Label10;
        public System.Windows.Forms.Label Label9;
        public System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox txtRequestID;
        internal System.Windows.Forms.TextBox txtDirection;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox txtDatapointsPerSend;
        internal System.Windows.Forms.TextBox txtEndFilterTime;
        internal System.Windows.Forms.TextBox txtBeginFilterTime;
        internal System.Windows.Forms.TextBox txtEndDateTime;
        internal System.Windows.Forms.TextBox txtBeginDateTime;
        internal System.Windows.Forms.TextBox txtInterval;
        internal System.Windows.Forms.TextBox txtDays;
        internal System.Windows.Forms.TextBox txtDatapoints;
        public System.Windows.Forms.ComboBox cboHistoryType;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Panel panelFutures;
        private System.Windows.Forms.Button btnRefreshAllData;
        private System.Windows.Forms.ListBox listDataSymbols;
        private System.Windows.Forms.ListBox listDataTimeFrames;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ListView lvDataFiles;
        private System.Windows.Forms.ColumnHeader columnFilename;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.Label lblDataFileCount;
        private System.Windows.Forms.Button btnRefreshSelectedSymbol;
        private System.Windows.Forms.ColumnHeader columnList;
        private System.Windows.Forms.Button btnIntervalHour;
        private System.Windows.Forms.Button btnIntervalMinute;
        private System.Windows.Forms.ComboBox cboFuturesInterval;
        public System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown numHourAdjustContract;
        public System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown numHourAdjustFutures;
        private System.Windows.Forms.ComboBox cboSymbolRoot;
        private System.Windows.Forms.ComboBox txtSymbol;
        private System.Windows.Forms.CheckBox chkDatesInFilename;
        private System.Windows.Forms.Panel panelSymbols;
        private System.Windows.Forms.Button btnSymbolSearch;
        private System.Windows.Forms.TextBox txtSymbolSearch;
        private System.Windows.Forms.ComboBox cboListedMarkets;
        private System.Windows.Forms.Button btnClearSymbolSearch;
        private System.Windows.Forms.ComboBox cboSecurityTypes;
        public System.Windows.Forms.Label label23;
        public System.Windows.Forms.Label label22;
        private System.Windows.Forms.ToolStripSplitButton tsbtnClearOutput;
    }
}

