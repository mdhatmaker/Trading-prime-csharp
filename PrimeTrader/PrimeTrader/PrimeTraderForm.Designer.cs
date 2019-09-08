namespace PrimeTrader
{
    partial class PrimeTraderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrimeTraderForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.panelDataRight = new System.Windows.Forms.Panel();
            this.panelDataRight1 = new System.Windows.Forms.Panel();
            this.panelDataRight2 = new System.Windows.Forms.Panel();
            this.panelDateSliders = new System.Windows.Forms.Panel();
            this.lblLastDate = new System.Windows.Forms.Label();
            this.lblFirstDate = new System.Windows.Forms.Label();
            this.btnLoadDateRange = new System.Windows.Forms.Button();
            this.lblDate2 = new System.Windows.Forms.Label();
            this.lblDate1 = new System.Windows.Forms.Label();
            this.trackBarDate2 = new System.Windows.Forms.TrackBar();
            this.trackBarDate1 = new System.Windows.Forms.TrackBar();
            this.panelDataLeft = new System.Windows.Forms.Panel();
            this.dataFrameFilePanel1 = new System.Windows.Forms.Panel();
            this.tabPagePython = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listScriptDescriptions = new System.Windows.Forms.ListBox();
            this.chkHideFunctionModules = new System.Windows.Forms.CheckBox();
            this.btnLaunchPythonScript = new System.Windows.Forms.Button();
            this.listScripts = new System.Windows.Forms.ListBox();
            this.tabPageCharts = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.listCharts = new System.Windows.Forms.ListBox();
            this.tabPageBackTest = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.listOther = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listTrades = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridCopper1 = new System.Windows.Forms.DataGridView();
            this.gridCopper2 = new System.Windows.Forms.DataGridView();
            this.tabPageStrategy = new System.Windows.Forms.TabPage();
            this.panelStrategyFiles = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCryptoTrader = new System.Windows.Forms.Button();
            this.btnCryptoInfo = new System.Windows.Forms.Button();
            this.btnMessages = new System.Windows.Forms.Button();
            this.btnDataGrid = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnCryptoAggregator = new System.Windows.Forms.Button();
            this.btnHistorical = new System.Windows.Forms.Button();
            this.btnLevel1Prices = new System.Windows.Forms.Button();
            this.btnCryptoPrices = new System.Windows.Forms.Button();
            this.btnChart = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.timerResetStatus = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelMainButtons = new System.Windows.Forms.Panel();
            this.tabControl.SuspendLayout();
            this.tabPageData.SuspendLayout();
            this.panelDataRight.SuspendLayout();
            this.panelDataRight2.SuspendLayout();
            this.panelDateSliders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDate2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDate1)).BeginInit();
            this.panelDataLeft.SuspendLayout();
            this.tabPagePython.SuspendLayout();
            this.tabPageCharts.SuspendLayout();
            this.tabPageBackTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCopper1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCopper2)).BeginInit();
            this.tabPageStrategy.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panelMainButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageData);
            this.tabControl.Controls.Add(this.tabPagePython);
            this.tabControl.Controls.Add(this.tabPageCharts);
            this.tabControl.Controls.Add(this.tabPageBackTest);
            this.tabControl.Controls.Add(this.tabPageStrategy);
            this.tabControl.ItemSize = new System.Drawing.Size(48, 31);
            this.tabControl.Location = new System.Drawing.Point(-3, 94);
            this.tabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(16, 3);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1219, 698);
            this.tabControl.TabIndex = 3;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this.panelDataRight);
            this.tabPageData.Controls.Add(this.panelDataLeft);
            this.tabPageData.Location = new System.Drawing.Point(4, 35);
            this.tabPageData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Size = new System.Drawing.Size(1211, 659);
            this.tabPageData.TabIndex = 4;
            this.tabPageData.Text = "Data";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // panelDataRight
            // 
            this.panelDataRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDataRight.Controls.Add(this.panelDataRight1);
            this.panelDataRight.Controls.Add(this.panelDataRight2);
            this.panelDataRight.Location = new System.Drawing.Point(532, 2);
            this.panelDataRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelDataRight.Name = "panelDataRight";
            this.panelDataRight.Size = new System.Drawing.Size(675, 530);
            this.panelDataRight.TabIndex = 1;
            // 
            // panelDataRight1
            // 
            this.panelDataRight1.BackColor = System.Drawing.Color.SkyBlue;
            this.panelDataRight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDataRight1.Location = new System.Drawing.Point(0, 160);
            this.panelDataRight1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelDataRight1.Name = "panelDataRight1";
            this.panelDataRight1.Size = new System.Drawing.Size(675, 370);
            this.panelDataRight1.TabIndex = 2;
            // 
            // panelDataRight2
            // 
            this.panelDataRight2.BackColor = System.Drawing.Color.Black;
            this.panelDataRight2.Controls.Add(this.panelDateSliders);
            this.panelDataRight2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDataRight2.Location = new System.Drawing.Point(0, 0);
            this.panelDataRight2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelDataRight2.Name = "panelDataRight2";
            this.panelDataRight2.Size = new System.Drawing.Size(675, 160);
            this.panelDataRight2.TabIndex = 1;
            // 
            // panelDateSliders
            // 
            this.panelDateSliders.Controls.Add(this.lblLastDate);
            this.panelDateSliders.Controls.Add(this.lblFirstDate);
            this.panelDateSliders.Controls.Add(this.btnLoadDateRange);
            this.panelDateSliders.Controls.Add(this.lblDate2);
            this.panelDateSliders.Controls.Add(this.lblDate1);
            this.panelDateSliders.Controls.Add(this.trackBarDate2);
            this.panelDateSliders.Controls.Add(this.trackBarDate1);
            this.panelDateSliders.Location = new System.Drawing.Point(123, 18);
            this.panelDateSliders.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelDateSliders.Name = "panelDateSliders";
            this.panelDateSliders.Size = new System.Drawing.Size(379, 123);
            this.panelDateSliders.TabIndex = 8;
            // 
            // lblLastDate
            // 
            this.lblLastDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastDate.ForeColor = System.Drawing.Color.White;
            this.lblLastDate.Location = new System.Drawing.Point(252, 0);
            this.lblLastDate.Name = "lblLastDate";
            this.lblLastDate.Size = new System.Drawing.Size(113, 14);
            this.lblLastDate.TabIndex = 13;
            this.lblLastDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFirstDate
            // 
            this.lblFirstDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstDate.ForeColor = System.Drawing.Color.White;
            this.lblFirstDate.Location = new System.Drawing.Point(15, 0);
            this.lblFirstDate.Name = "lblFirstDate";
            this.lblFirstDate.Size = new System.Drawing.Size(113, 14);
            this.lblFirstDate.TabIndex = 12;
            this.lblFirstDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLoadDateRange
            // 
            this.btnLoadDateRange.Location = new System.Drawing.Point(137, 78);
            this.btnLoadDateRange.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoadDateRange.Name = "btnLoadDateRange";
            this.btnLoadDateRange.Size = new System.Drawing.Size(105, 38);
            this.btnLoadDateRange.TabIndex = 4;
            this.btnLoadDateRange.Text = "Load Range";
            this.btnLoadDateRange.UseVisualStyleBackColor = true;
            this.btnLoadDateRange.Click += new System.EventHandler(this.btnLoadDateRange_Click);
            // 
            // lblDate2
            // 
            this.lblDate2.ForeColor = System.Drawing.Color.White;
            this.lblDate2.Location = new System.Drawing.Point(193, 33);
            this.lblDate2.Name = "lblDate2";
            this.lblDate2.Size = new System.Drawing.Size(181, 23);
            this.lblDate2.TabIndex = 11;
            this.lblDate2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDate1
            // 
            this.lblDate1.ForeColor = System.Drawing.Color.White;
            this.lblDate1.Location = new System.Drawing.Point(5, 33);
            this.lblDate1.Name = "lblDate1";
            this.lblDate1.Size = new System.Drawing.Size(181, 23);
            this.lblDate1.TabIndex = 10;
            this.lblDate1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarDate2
            // 
            this.trackBarDate2.Location = new System.Drawing.Point(193, 15);
            this.trackBarDate2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarDate2.Name = "trackBarDate2";
            this.trackBarDate2.Size = new System.Drawing.Size(181, 56);
            this.trackBarDate2.TabIndex = 9;
            this.trackBarDate2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarDate2.Value = 10;
            this.trackBarDate2.ValueChanged += new System.EventHandler(this.trackBarDate2_ValueChanged);
            // 
            // trackBarDate1
            // 
            this.trackBarDate1.Location = new System.Drawing.Point(5, 15);
            this.trackBarDate1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarDate1.Name = "trackBarDate1";
            this.trackBarDate1.Size = new System.Drawing.Size(181, 56);
            this.trackBarDate1.TabIndex = 8;
            this.trackBarDate1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarDate1.ValueChanged += new System.EventHandler(this.trackBarDate1_ValueChanged);
            // 
            // panelDataLeft
            // 
            this.panelDataLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelDataLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelDataLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDataLeft.Controls.Add(this.dataFrameFilePanel1);
            this.panelDataLeft.Location = new System.Drawing.Point(3, 6);
            this.panelDataLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelDataLeft.Name = "panelDataLeft";
            this.panelDataLeft.Size = new System.Drawing.Size(525, 526);
            this.panelDataLeft.TabIndex = 0;
            // 
            // dataFrameFilePanel1
            // 
            this.dataFrameFilePanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataFrameFilePanel1.Location = new System.Drawing.Point(0, 0);
            this.dataFrameFilePanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataFrameFilePanel1.Name = "dataFrameFilePanel1";
            this.dataFrameFilePanel1.Size = new System.Drawing.Size(523, 522);
            this.dataFrameFilePanel1.TabIndex = 0;
            // 
            // tabPagePython
            // 
            this.tabPagePython.BackColor = System.Drawing.Color.Transparent;
            this.tabPagePython.Controls.Add(this.label5);
            this.tabPagePython.Controls.Add(this.label4);
            this.tabPagePython.Controls.Add(this.listScriptDescriptions);
            this.tabPagePython.Controls.Add(this.chkHideFunctionModules);
            this.tabPagePython.Controls.Add(this.btnLaunchPythonScript);
            this.tabPagePython.Controls.Add(this.listScripts);
            this.tabPagePython.Location = new System.Drawing.Point(4, 35);
            this.tabPagePython.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPagePython.Name = "tabPagePython";
            this.tabPagePython.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPagePython.Size = new System.Drawing.Size(1211, 659);
            this.tabPagePython.TabIndex = 0;
            this.tabPagePython.Text = "Scripts";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Scripts";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(627, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Descriptions (when available)";
            // 
            // listScriptDescriptions
            // 
            this.listScriptDescriptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listScriptDescriptions.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listScriptDescriptions.FormattingEnabled = true;
            this.listScriptDescriptions.ItemHeight = 15;
            this.listScriptDescriptions.Location = new System.Drawing.Point(620, 36);
            this.listScriptDescriptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listScriptDescriptions.Name = "listScriptDescriptions";
            this.listScriptDescriptions.Size = new System.Drawing.Size(587, 544);
            this.listScriptDescriptions.TabIndex = 8;
            this.listScriptDescriptions.SelectedIndexChanged += new System.EventHandler(this.listScriptDescriptions_SelectedIndexChanged);
            // 
            // chkHideFunctionModules
            // 
            this.chkHideFunctionModules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHideFunctionModules.AutoSize = true;
            this.chkHideFunctionModules.Checked = true;
            this.chkHideFunctionModules.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHideFunctionModules.Location = new System.Drawing.Point(19, 629);
            this.chkHideFunctionModules.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkHideFunctionModules.Name = "chkHideFunctionModules";
            this.chkHideFunctionModules.Size = new System.Drawing.Size(168, 21);
            this.chkHideFunctionModules.TabIndex = 7;
            this.chkHideFunctionModules.Text = "hide function modules";
            this.chkHideFunctionModules.UseVisualStyleBackColor = true;
            this.chkHideFunctionModules.CheckStateChanged += new System.EventHandler(this.chkHideFunctionModules_CheckStateChanged);
            // 
            // btnLaunchPythonScript
            // 
            this.btnLaunchPythonScript.Location = new System.Drawing.Point(408, 34);
            this.btnLaunchPythonScript.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLaunchPythonScript.Name = "btnLaunchPythonScript";
            this.btnLaunchPythonScript.Size = new System.Drawing.Size(177, 62);
            this.btnLaunchPythonScript.TabIndex = 3;
            this.btnLaunchPythonScript.Text = "Launch Python Script";
            this.btnLaunchPythonScript.UseVisualStyleBackColor = true;
            this.btnLaunchPythonScript.Click += new System.EventHandler(this.btnLaunchPythonScript_Click);
            // 
            // listScripts
            // 
            this.listScripts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listScripts.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listScripts.FormattingEnabled = true;
            this.listScripts.ItemHeight = 15;
            this.listScripts.Location = new System.Drawing.Point(19, 36);
            this.listScripts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listScripts.Name = "listScripts";
            this.listScripts.Size = new System.Drawing.Size(360, 544);
            this.listScripts.TabIndex = 2;
            this.listScripts.SelectedIndexChanged += new System.EventHandler(this.listScripts_SelectedIndexChanged);
            this.listScripts.SelectedValueChanged += new System.EventHandler(this.listScripts_SelectedValueChanged);
            // 
            // tabPageCharts
            // 
            this.tabPageCharts.BackColor = System.Drawing.Color.SkyBlue;
            this.tabPageCharts.Controls.Add(this.label3);
            this.tabPageCharts.Controls.Add(this.listCharts);
            this.tabPageCharts.Location = new System.Drawing.Point(4, 35);
            this.tabPageCharts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPageCharts.Name = "tabPageCharts";
            this.tabPageCharts.Size = new System.Drawing.Size(1211, 659);
            this.tabPageCharts.TabIndex = 2;
            this.tabPageCharts.Text = "Charts";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(73, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(235, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "HTML Chart Files";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listCharts
            // 
            this.listCharts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listCharts.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listCharts.FormattingEnabled = true;
            this.listCharts.ItemHeight = 15;
            this.listCharts.Location = new System.Drawing.Point(19, 41);
            this.listCharts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listCharts.Name = "listCharts";
            this.listCharts.Size = new System.Drawing.Size(360, 514);
            this.listCharts.TabIndex = 3;
            this.listCharts.SelectedIndexChanged += new System.EventHandler(this.listCharts_SelectedIndexChanged);
            // 
            // tabPageBackTest
            // 
            this.tabPageBackTest.BackColor = System.Drawing.Color.SkyBlue;
            this.tabPageBackTest.Controls.Add(this.label2);
            this.tabPageBackTest.Controls.Add(this.listOther);
            this.tabPageBackTest.Controls.Add(this.label1);
            this.tabPageBackTest.Controls.Add(this.listTrades);
            this.tabPageBackTest.Controls.Add(this.splitContainer1);
            this.tabPageBackTest.Location = new System.Drawing.Point(4, 35);
            this.tabPageBackTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPageBackTest.Name = "tabPageBackTest";
            this.tabPageBackTest.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPageBackTest.Size = new System.Drawing.Size(1211, 659);
            this.tabPageBackTest.TabIndex = 1;
            this.tabPageBackTest.Text = "Back Test";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(85, 324);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(217, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "Other";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listOther
            // 
            this.listOther.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listOther.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listOther.FormattingEnabled = true;
            this.listOther.ItemHeight = 15;
            this.listOther.Location = new System.Drawing.Point(19, 350);
            this.listOther.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listOther.Name = "listOther";
            this.listOther.Size = new System.Drawing.Size(360, 199);
            this.listOther.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(85, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Trades";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listTrades
            // 
            this.listTrades.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listTrades.FormattingEnabled = true;
            this.listTrades.ItemHeight = 15;
            this.listTrades.Location = new System.Drawing.Point(19, 41);
            this.listTrades.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listTrades.Name = "listTrades";
            this.listTrades.Size = new System.Drawing.Size(360, 199);
            this.listTrades.TabIndex = 7;
            this.listTrades.SelectedIndexChanged += new System.EventHandler(this.listTrades_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(451, 15);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridCopper1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridCopper2);
            this.splitContainer1.Size = new System.Drawing.Size(727, 626);
            this.splitContainer1.SplitterDistance = 313;
            this.splitContainer1.TabIndex = 1;
            // 
            // gridCopper1
            // 
            this.gridCopper1.AllowUserToAddRows = false;
            this.gridCopper1.AllowUserToDeleteRows = false;
            this.gridCopper1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCopper1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCopper1.Location = new System.Drawing.Point(0, 0);
            this.gridCopper1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridCopper1.Name = "gridCopper1";
            this.gridCopper1.ReadOnly = true;
            this.gridCopper1.RowTemplate.Height = 24;
            this.gridCopper1.Size = new System.Drawing.Size(727, 313);
            this.gridCopper1.TabIndex = 1;
            // 
            // gridCopper2
            // 
            this.gridCopper2.AllowUserToAddRows = false;
            this.gridCopper2.AllowUserToDeleteRows = false;
            this.gridCopper2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCopper2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCopper2.Location = new System.Drawing.Point(0, 0);
            this.gridCopper2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridCopper2.Name = "gridCopper2";
            this.gridCopper2.ReadOnly = true;
            this.gridCopper2.RowTemplate.Height = 24;
            this.gridCopper2.Size = new System.Drawing.Size(727, 309);
            this.gridCopper2.TabIndex = 0;
            // 
            // tabPageStrategy
            // 
            this.tabPageStrategy.BackColor = System.Drawing.Color.SkyBlue;
            this.tabPageStrategy.Controls.Add(this.panelStrategyFiles);
            this.tabPageStrategy.Location = new System.Drawing.Point(4, 35);
            this.tabPageStrategy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPageStrategy.Name = "tabPageStrategy";
            this.tabPageStrategy.Size = new System.Drawing.Size(1211, 659);
            this.tabPageStrategy.TabIndex = 5;
            this.tabPageStrategy.Text = "Strategy";
            // 
            // panelStrategyFiles
            // 
            this.panelStrategyFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelStrategyFiles.Location = new System.Drawing.Point(11, 9);
            this.panelStrategyFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelStrategyFiles.Name = "panelStrategyFiles";
            this.panelStrategyFiles.Size = new System.Drawing.Size(353, 629);
            this.panelStrategyFiles.TabIndex = 9;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel1.ColumnCount = 11;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090214F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090209F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090209F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090209F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090209F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090209F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.092025F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.093196F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.093196F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090165F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090165F));
            this.tableLayoutPanel1.Controls.Add(this.btnCryptoTrader, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCryptoInfo, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnMessages, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDataGrid, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSettings, 10, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCryptoAggregator, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnHistorical, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLevel1Prices, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCryptoPrices, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnChart, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1185, 85);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // btnCryptoTrader
            // 
            this.btnCryptoTrader.BackColor = System.Drawing.Color.White;
            this.btnCryptoTrader.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCryptoTrader.Image = ((System.Drawing.Image)(resources.GetObject("btnCryptoTrader.Image")));
            this.btnCryptoTrader.Location = new System.Drawing.Point(538, 2);
            this.btnCryptoTrader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCryptoTrader.Name = "btnCryptoTrader";
            this.btnCryptoTrader.Size = new System.Drawing.Size(100, 80);
            this.btnCryptoTrader.TabIndex = 17;
            this.toolTip1.SetToolTip(this.btnCryptoTrader, "Crypto Trader");
            this.btnCryptoTrader.UseVisualStyleBackColor = false;
            this.btnCryptoTrader.Click += new System.EventHandler(this.btnCryptoTrader_Click);
            // 
            // btnCryptoInfo
            // 
            this.btnCryptoInfo.BackColor = System.Drawing.Color.White;
            this.btnCryptoInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCryptoInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnCryptoInfo.Image")));
            this.btnCryptoInfo.Location = new System.Drawing.Point(324, 2);
            this.btnCryptoInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCryptoInfo.Name = "btnCryptoInfo";
            this.btnCryptoInfo.Size = new System.Drawing.Size(100, 80);
            this.btnCryptoInfo.TabIndex = 16;
            this.toolTip1.SetToolTip(this.btnCryptoInfo, "Crypto Market Information");
            this.btnCryptoInfo.UseVisualStyleBackColor = false;
            this.btnCryptoInfo.Click += new System.EventHandler(this.btnCryptoInfo_Click);
            // 
            // btnMessages
            // 
            this.btnMessages.BackColor = System.Drawing.Color.White;
            this.btnMessages.Image = ((System.Drawing.Image)(resources.GetObject("btnMessages.Image")));
            this.btnMessages.Location = new System.Drawing.Point(859, 2);
            this.btnMessages.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMessages.Name = "btnMessages";
            this.btnMessages.Size = new System.Drawing.Size(100, 80);
            this.btnMessages.TabIndex = 15;
            this.toolTip1.SetToolTip(this.btnMessages, "Messages");
            this.btnMessages.UseVisualStyleBackColor = false;
            this.btnMessages.Click += new System.EventHandler(this.btnMessages_Click);
            // 
            // btnDataGrid
            // 
            this.btnDataGrid.BackColor = System.Drawing.Color.White;
            this.btnDataGrid.Image = ((System.Drawing.Image)(resources.GetObject("btnDataGrid.Image")));
            this.btnDataGrid.Location = new System.Drawing.Point(217, 2);
            this.btnDataGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDataGrid.Name = "btnDataGrid";
            this.btnDataGrid.Size = new System.Drawing.Size(100, 80);
            this.btnDataGrid.TabIndex = 14;
            this.toolTip1.SetToolTip(this.btnDataGrid, "Data Grid");
            this.btnDataGrid.UseVisualStyleBackColor = false;
            this.btnDataGrid.Click += new System.EventHandler(this.btnDataGrid_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.White;
            this.btnSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnSettings.Image")));
            this.btnSettings.Location = new System.Drawing.Point(1073, 2);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(105, 80);
            this.btnSettings.TabIndex = 13;
            this.toolTip1.SetToolTip(this.btnSettings, "Settings");
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnCryptoAggregator
            // 
            this.btnCryptoAggregator.BackColor = System.Drawing.Color.White;
            this.btnCryptoAggregator.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCryptoAggregator.Image = ((System.Drawing.Image)(resources.GetObject("btnCryptoAggregator.Image")));
            this.btnCryptoAggregator.Location = new System.Drawing.Point(645, 2);
            this.btnCryptoAggregator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCryptoAggregator.Name = "btnCryptoAggregator";
            this.btnCryptoAggregator.Size = new System.Drawing.Size(100, 80);
            this.btnCryptoAggregator.TabIndex = 12;
            this.toolTip1.SetToolTip(this.btnCryptoAggregator, "GATOR Crypto Orderbook Aggregator");
            this.btnCryptoAggregator.UseVisualStyleBackColor = false;
            this.btnCryptoAggregator.Click += new System.EventHandler(this.btnCryptoGator_Click);
            // 
            // btnHistorical
            // 
            this.btnHistorical.BackColor = System.Drawing.Color.White;
            this.btnHistorical.Image = ((System.Drawing.Image)(resources.GetObject("btnHistorical.Image")));
            this.btnHistorical.Location = new System.Drawing.Point(3, 2);
            this.btnHistorical.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnHistorical.Name = "btnHistorical";
            this.btnHistorical.Size = new System.Drawing.Size(100, 80);
            this.btnHistorical.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnHistorical, "Historical Data");
            this.btnHistorical.UseVisualStyleBackColor = false;
            this.btnHistorical.Click += new System.EventHandler(this.btnHistorical_Click);
            // 
            // btnLevel1Prices
            // 
            this.btnLevel1Prices.BackColor = System.Drawing.Color.White;
            this.btnLevel1Prices.Image = ((System.Drawing.Image)(resources.GetObject("btnLevel1Prices.Image")));
            this.btnLevel1Prices.Location = new System.Drawing.Point(752, 2);
            this.btnLevel1Prices.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLevel1Prices.Name = "btnLevel1Prices";
            this.btnLevel1Prices.Size = new System.Drawing.Size(100, 80);
            this.btnLevel1Prices.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnLevel1Prices, "Prices and Spreads");
            this.btnLevel1Prices.UseVisualStyleBackColor = false;
            this.btnLevel1Prices.Click += new System.EventHandler(this.btnLevel1Prices_Click);
            // 
            // btnCryptoPrices
            // 
            this.btnCryptoPrices.BackColor = System.Drawing.Color.White;
            this.btnCryptoPrices.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCryptoPrices.Image = ((System.Drawing.Image)(resources.GetObject("btnCryptoPrices.Image")));
            this.btnCryptoPrices.Location = new System.Drawing.Point(431, 2);
            this.btnCryptoPrices.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCryptoPrices.Name = "btnCryptoPrices";
            this.btnCryptoPrices.Size = new System.Drawing.Size(100, 80);
            this.btnCryptoPrices.TabIndex = 10;
            this.toolTip1.SetToolTip(this.btnCryptoPrices, "Crypto Prices");
            this.btnCryptoPrices.UseVisualStyleBackColor = false;
            this.btnCryptoPrices.Click += new System.EventHandler(this.btnCryptoPrices_Click);
            // 
            // btnChart
            // 
            this.btnChart.BackColor = System.Drawing.Color.White;
            this.btnChart.Image = ((System.Drawing.Image)(resources.GetObject("btnChart.Image")));
            this.btnChart.Location = new System.Drawing.Point(110, 2);
            this.btnChart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(100, 80);
            this.btnChart.TabIndex = 9;
            this.toolTip1.SetToolTip(this.btnChart, "Chart");
            this.btnChart.UseVisualStyleBackColor = false;
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel1,
            this.statusLabel2,
            this.statusProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 663);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 13, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1219, 39);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel1
            // 
            this.statusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(962, 34);
            this.statusLabel1.Spring = true;
            this.statusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusLabel2
            // 
            this.statusLabel2.AutoSize = false;
            this.statusLabel2.BackColor = System.Drawing.Color.DarkGray;
            this.statusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusLabel2.Name = "statusLabel2";
            this.statusLabel2.Size = new System.Drawing.Size(141, 34);
            // 
            // statusProgress
            // 
            this.statusProgress.AutoSize = false;
            this.statusProgress.Name = "statusProgress";
            this.statusProgress.Size = new System.Drawing.Size(100, 33);
            this.statusProgress.Step = 1;
            this.statusProgress.Value = 100;
            // 
            // timerProgress
            // 
            this.timerProgress.Interval = 1000;
            this.timerProgress.Tick += new System.EventHandler(this.timerProgress_Tick);
            // 
            // timerResetStatus
            // 
            this.timerResetStatus.Enabled = true;
            this.timerResetStatus.Interval = 1000;
            this.timerResetStatus.Tick += new System.EventHandler(this.timerResetStatus_Tick);
            // 
            // panelMainButtons
            // 
            this.panelMainButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMainButtons.BackColor = System.Drawing.Color.Black;
            this.panelMainButtons.Controls.Add(this.tableLayoutPanel1);
            this.panelMainButtons.Location = new System.Drawing.Point(-3, 1);
            this.panelMainButtons.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelMainButtons.Name = "panelMainButtons";
            this.panelMainButtons.Size = new System.Drawing.Size(1221, 89);
            this.panelMainButtons.TabIndex = 12;
            // 
            // PrimeTraderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1219, 702);
            this.Controls.Add(this.panelMainButtons);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PrimeTraderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ZTrader";
            this.Load += new System.EventHandler(this.PythonUiForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageData.ResumeLayout(false);
            this.panelDataRight.ResumeLayout(false);
            this.panelDataRight2.ResumeLayout(false);
            this.panelDateSliders.ResumeLayout(false);
            this.panelDateSliders.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDate2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDate1)).EndInit();
            this.panelDataLeft.ResumeLayout(false);
            this.tabPagePython.ResumeLayout(false);
            this.tabPagePython.PerformLayout();
            this.tabPageCharts.ResumeLayout(false);
            this.tabPageBackTest.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCopper1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCopper2)).EndInit();
            this.tabPageStrategy.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelMainButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPagePython;
        private System.Windows.Forms.Button btnLaunchPythonScript;
        private System.Windows.Forms.ListBox listScripts;
        private System.Windows.Forms.TabPage tabPageBackTest;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gridCopper1;
        private System.Windows.Forms.DataGridView gridCopper2;
        private System.Windows.Forms.TabPage tabPageCharts;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listOther;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listTrades;
        private System.Windows.Forms.Button btnHistorical;
        private System.Windows.Forms.Button btnLevel1Prices;
        private System.Windows.Forms.CheckBox chkHideFunctionModules;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel2;
        private System.Windows.Forms.ToolStripProgressBar statusProgress;
        private System.Windows.Forms.ListBox listCharts;
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.Button btnChart;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.Panel panelDataLeft;
        private System.Windows.Forms.Panel dataFrameFilePanel1;
        private System.Windows.Forms.Panel panelDataRight;
        private System.Windows.Forms.Panel panelDataRight1;
        private System.Windows.Forms.Panel panelDataRight2;
        private System.Windows.Forms.Button btnLoadDateRange;
        private System.Windows.Forms.Timer timerResetStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelDateSliders;
        private System.Windows.Forms.Label lblLastDate;
        private System.Windows.Forms.Label lblFirstDate;
        private System.Windows.Forms.Label lblDate2;
        private System.Windows.Forms.Label lblDate1;
        private System.Windows.Forms.TrackBar trackBarDate2;
        private System.Windows.Forms.TrackBar trackBarDate1;
        private System.Windows.Forms.Button btnCryptoPrices;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCryptoAggregator;
        private System.Windows.Forms.TabPage tabPageStrategy;
        private System.Windows.Forms.Panel panelStrategyFiles;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelMainButtons;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnDataGrid;
        private System.Windows.Forms.Button btnMessages;
        private System.Windows.Forms.Button btnCryptoInfo;
        private System.Windows.Forms.ListBox listScriptDescriptions;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCryptoTrader;
    }
}

