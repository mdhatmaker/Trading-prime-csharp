namespace PrimeTrader
{
    partial class DataFrameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataFrameForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslblLeft = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblClear = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblMiddle = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelGrid = new System.Windows.Forms.Panel();
            this.gridDataFrame = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnToExcelRange = new System.Windows.Forms.ToolStripButton();
            this.tsbtnToExcelAll = new System.Windows.Forms.ToolStripButton();
            this.tsbtnPlot = new System.Windows.Forms.ToolStripSplitButton();
            this.tsitemPlotARIMA = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnPlotAll = new System.Windows.Forms.ToolStripSplitButton();
            this.tsitemPlotAllARIMA = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnRunBacktest = new System.Windows.Forms.ToolStripSplitButton();
            this.tsitemSomeBacktest = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnAddStudy = new System.Windows.Forms.ToolStripSplitButton();
            this.tsitemARIMAStudy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemEMA = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemSMA = new System.Windows.Forms.ToolStripMenuItem();
            this.tstxtInput = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataFrame)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblLeft,
            this.tslblClear,
            this.tslblMiddle,
            this.tslblRight});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1288);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 15, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1554, 40);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslblLeft
            // 
            this.tslblLeft.AutoSize = false;
            this.tslblLeft.BackColor = System.Drawing.Color.Black;
            this.tslblLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tslblLeft.ForeColor = System.Drawing.Color.White;
            this.tslblLeft.Name = "tslblLeft";
            this.tslblLeft.Size = new System.Drawing.Size(250, 35);
            // 
            // tslblClear
            // 
            this.tslblClear.BackColor = System.Drawing.Color.RoyalBlue;
            this.tslblClear.ForeColor = System.Drawing.Color.White;
            this.tslblClear.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.tslblClear.Name = "tslblClear";
            this.tslblClear.Size = new System.Drawing.Size(48, 35);
            this.tslblClear.Text = "clear";
            this.tslblClear.Click += new System.EventHandler(this.tslblClear_Click);
            // 
            // tslblMiddle
            // 
            this.tslblMiddle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tslblMiddle.Name = "tslblMiddle";
            this.tslblMiddle.Size = new System.Drawing.Size(1035, 35);
            this.tslblMiddle.Spring = true;
            // 
            // tslblRight
            // 
            this.tslblRight.AutoSize = false;
            this.tslblRight.BackColor = System.Drawing.Color.Gainsboro;
            this.tslblRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tslblRight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tslblRight.Name = "tslblRight";
            this.tslblRight.Size = new System.Drawing.Size(200, 35);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelGrid);
            this.panelMain.Controls.Add(this.toolStrip1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1554, 1288);
            this.panelMain.TabIndex = 1;
            // 
            // panelGrid
            // 
            this.panelGrid.Controls.Add(this.gridDataFrame);
            this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrid.Location = new System.Drawing.Point(0, 37);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(1554, 1251);
            this.panelGrid.TabIndex = 4;
            // 
            // gridDataFrame
            // 
            this.gridDataFrame.AllowUserToAddRows = false;
            this.gridDataFrame.AllowUserToDeleteRows = false;
            this.gridDataFrame.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDataFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDataFrame.Location = new System.Drawing.Point(0, 0);
            this.gridDataFrame.Name = "gridDataFrame";
            this.gridDataFrame.RowTemplate.Height = 28;
            this.gridDataFrame.Size = new System.Drawing.Size(1554, 1251);
            this.gridDataFrame.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnToExcelRange,
            this.tsbtnToExcelAll,
            this.tsbtnPlot,
            this.tsbtnPlotAll,
            this.tsbtnRunBacktest,
            this.toolStripSeparator1,
            this.tsbtnAddStudy,
            this.tstxtInput,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1554, 37);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnToExcelRange
            // 
            this.tsbtnToExcelRange.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnToExcelRange.Image")));
            this.tsbtnToExcelRange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnToExcelRange.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.tsbtnToExcelRange.Name = "tsbtnToExcelRange";
            this.tsbtnToExcelRange.Size = new System.Drawing.Size(97, 34);
            this.tsbtnToExcelRange.Text = "To Excel";
            this.tsbtnToExcelRange.Click += new System.EventHandler(this.tsbtnToExcelRange_Click);
            // 
            // tsbtnToExcelAll
            // 
            this.tsbtnToExcelAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnToExcelAll.Image")));
            this.tsbtnToExcelAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnToExcelAll.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.tsbtnToExcelAll.Name = "tsbtnToExcelAll";
            this.tsbtnToExcelAll.Size = new System.Drawing.Size(132, 34);
            this.tsbtnToExcelAll.Text = "To Excel (All)";
            this.tsbtnToExcelAll.Click += new System.EventHandler(this.tsbtnToExcelAll_Click);
            // 
            // tsbtnPlot
            // 
            this.tsbtnPlot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsitemPlotARIMA});
            this.tsbtnPlot.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnPlot.Image")));
            this.tsbtnPlot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnPlot.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.tsbtnPlot.Name = "tsbtnPlot";
            this.tsbtnPlot.Size = new System.Drawing.Size(84, 34);
            this.tsbtnPlot.Text = "Plot";
            this.tsbtnPlot.ButtonClick += new System.EventHandler(this.tsbtnPlot_ButtonClick);
            // 
            // tsitemPlotARIMA
            // 
            this.tsitemPlotARIMA.Name = "tsitemPlotARIMA";
            this.tsitemPlotARIMA.Size = new System.Drawing.Size(210, 30);
            this.tsitemPlotARIMA.Text = "ARIMA";
            this.tsitemPlotARIMA.Click += new System.EventHandler(this.tsitemPlotARIMA_Click);
            // 
            // tsbtnPlotAll
            // 
            this.tsbtnPlotAll.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsitemPlotAllARIMA});
            this.tsbtnPlotAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnPlotAll.Image")));
            this.tsbtnPlotAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnPlotAll.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.tsbtnPlotAll.Name = "tsbtnPlotAll";
            this.tsbtnPlotAll.Size = new System.Drawing.Size(119, 34);
            this.tsbtnPlotAll.Text = "Plot (All)";
            this.tsbtnPlotAll.Click += new System.EventHandler(this.tsbtnPlotAll_Click);
            // 
            // tsitemPlotAllARIMA
            // 
            this.tsitemPlotAllARIMA.Name = "tsitemPlotAllARIMA";
            this.tsitemPlotAllARIMA.Size = new System.Drawing.Size(210, 30);
            this.tsitemPlotAllARIMA.Text = "ARIMA";
            this.tsitemPlotAllARIMA.Click += new System.EventHandler(this.tsitemPlotAllARIMA_Click);
            // 
            // tsbtnRunBacktest
            // 
            this.tsbtnRunBacktest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsitemSomeBacktest});
            this.tsbtnRunBacktest.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnRunBacktest.Image")));
            this.tsbtnRunBacktest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnRunBacktest.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.tsbtnRunBacktest.Name = "tsbtnRunBacktest";
            this.tsbtnRunBacktest.Size = new System.Drawing.Size(118, 34);
            this.tsbtnRunBacktest.Text = "Backtest";
            // 
            // tsitemSomeBacktest
            // 
            this.tsitemSomeBacktest.Name = "tsitemSomeBacktest";
            this.tsitemSomeBacktest.Size = new System.Drawing.Size(211, 30);
            this.tsitemSomeBacktest.Text = "some backtest";
            // 
            // tsbtnAddStudy
            // 
            this.tsbtnAddStudy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsitemARIMAStudy,
            this.tsitemDiff,
            this.tsitemEMA,
            this.tsitemSMA});
            this.tsbtnAddStudy.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnAddStudy.Image")));
            this.tsbtnAddStudy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnAddStudy.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.tsbtnAddStudy.Name = "tsbtnAddStudy";
            this.tsbtnAddStudy.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.tsbtnAddStudy.Size = new System.Drawing.Size(118, 34);
            this.tsbtnAddStudy.Text = "Study";
            this.tsbtnAddStudy.ButtonClick += new System.EventHandler(this.tsbtnRunStudy_ButtonClick);
            // 
            // tsitemARIMAStudy
            // 
            this.tsitemARIMAStudy.Name = "tsitemARIMAStudy";
            this.tsitemARIMAStudy.Size = new System.Drawing.Size(210, 30);
            this.tsitemARIMAStudy.Text = "ARIMA";
            this.tsitemARIMAStudy.Click += new System.EventHandler(this.tsitemARIMAStudy_Click);
            // 
            // tsitemDiff
            // 
            this.tsitemDiff.Name = "tsitemDiff";
            this.tsitemDiff.Size = new System.Drawing.Size(210, 30);
            this.tsitemDiff.Text = "diff";
            this.tsitemDiff.Click += new System.EventHandler(this.tsitemDiff_Click);
            // 
            // tsitemEMA
            // 
            this.tsitemEMA.Name = "tsitemEMA";
            this.tsitemEMA.Size = new System.Drawing.Size(210, 30);
            this.tsitemEMA.Text = "EMA";
            this.tsitemEMA.Click += new System.EventHandler(this.tsitemEMA_Click);
            // 
            // tsitemSMA
            // 
            this.tsitemSMA.Name = "tsitemSMA";
            this.tsitemSMA.Size = new System.Drawing.Size(210, 30);
            this.tsitemSMA.Text = "SMA";
            this.tsitemSMA.Click += new System.EventHandler(this.tsitemSMA_Click);
            // 
            // tstxtInput
            // 
            this.tstxtInput.Margin = new System.Windows.Forms.Padding(30, 5, 10, 5);
            this.tstxtInput.Name = "tstxtInput";
            this.tstxtInput.Size = new System.Drawing.Size(80, 27);
            this.tstxtInput.Text = "5";
            this.tstxtInput.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 37);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 37);
            // 
            // DataFrameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1554, 1328);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.statusStrip1);
            this.Name = "DataFrameForm";
            this.Text = "DataFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataFrameForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDataFrame)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslblLeft;
        private System.Windows.Forms.ToolStripStatusLabel tslblMiddle;
        private System.Windows.Forms.ToolStripStatusLabel tslblRight;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnToExcelRange;
        private System.Windows.Forms.ToolStripButton tsbtnToExcelAll;
        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.DataGridView gridDataFrame;
        private System.Windows.Forms.ToolStripSplitButton tsbtnAddStudy;
        private System.Windows.Forms.ToolStripMenuItem tsitemARIMAStudy;
        private System.Windows.Forms.ToolStripSplitButton tsbtnPlot;
        private System.Windows.Forms.ToolStripMenuItem tsitemPlotARIMA;
        private System.Windows.Forms.ToolStripSplitButton tsbtnPlotAll;
        private System.Windows.Forms.ToolStripMenuItem tsitemPlotAllARIMA;
        private System.Windows.Forms.ToolStripSplitButton tsbtnRunBacktest;
        private System.Windows.Forms.ToolStripMenuItem tsitemSomeBacktest;
        private System.Windows.Forms.ToolStripStatusLabel tslblClear;
        private System.Windows.Forms.ToolStripMenuItem tsitemDiff;
        private System.Windows.Forms.ToolStripMenuItem tsitemEMA;
        private System.Windows.Forms.ToolStripMenuItem tsitemSMA;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox tstxtInput;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}