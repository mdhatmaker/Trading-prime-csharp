namespace CryptoForms
{
    partial class CryptoPricesForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CryptoPricesForm));
            this.button1 = new System.Windows.Forms.Button();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelTickers = new System.Windows.Forms.Panel();
            this.listExchanges = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblJPYPctChg = new System.Windows.Forms.Label();
            this.lblRUBPctChg = new System.Windows.Forms.Label();
            this.lblEURPctChg = new System.Windows.Forms.Label();
            this.lblCHFPctChg = new System.Windows.Forms.Label();
            this.lblDXPctChg = new System.Windows.Forms.Label();
            this.lblNZDPctChg = new System.Windows.Forms.Label();
            this.lblKRWPctChg = new System.Windows.Forms.Label();
            this.lblCADPctChg = new System.Windows.Forms.Label();
            this.lblGBPPctChg = new System.Windows.Forms.Label();
            this.lblAUDPctChg = new System.Windows.Forms.Label();
            this.lblCNYPctChg = new System.Windows.Forms.Label();
            this.lblEUR = new System.Windows.Forms.Label();
            this.lblJPY = new System.Windows.Forms.Label();
            this.lblDX = new System.Windows.Forms.Label();
            this.lblAUD = new System.Windows.Forms.Label();
            this.lblGBP = new System.Windows.Forms.Label();
            this.lblCAD = new System.Windows.Forms.Label();
            this.lblCNY = new System.Windows.Forms.Label();
            this.lblNZD = new System.Windows.Forms.Label();
            this.lblCHF = new System.Windows.Forms.Label();
            this.lblRUB = new System.Windows.Forms.Label();
            this.lblKRW = new System.Windows.Forms.Label();
            this.listSymbolIds = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 964);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 30);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chart
            // 
            this.chart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(8, 799);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(163, 203);
            this.chart.TabIndex = 1;
            this.chart.Text = "chart";
            this.chart.Visible = false;
            // 
            // panelTickers
            // 
            this.panelTickers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTickers.Location = new System.Drawing.Point(140, 49);
            this.panelTickers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelTickers.Name = "panelTickers";
            this.panelTickers.Size = new System.Drawing.Size(1248, 949);
            this.panelTickers.TabIndex = 3;
            // 
            // listExchanges
            // 
            this.listExchanges.FormattingEnabled = true;
            this.listExchanges.ItemHeight = 16;
            this.listExchanges.Location = new System.Drawing.Point(3, 89);
            this.listExchanges.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listExchanges.Name = "listExchanges";
            this.listExchanges.Size = new System.Drawing.Size(132, 516);
            this.listExchanges.TabIndex = 4;
            this.listExchanges.SelectedIndexChanged += new System.EventHandler(this.listExchanges_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 11;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.Controls.Add(this.lblJPYPctChg, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRUBPctChg, 10, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblEURPctChg, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCHFPctChg, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDXPctChg, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblNZDPctChg, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblKRWPctChg, 9, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCADPctChg, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblGBPPctChg, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblAUDPctChg, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCNYPctChg, 8, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblEUR, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblJPY, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDX, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblAUD, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblGBP, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCAD, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCNY, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNZD, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCHF, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblRUB, 10, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblKRW, 9, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1376, 38);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // lblJPYPctChg
            // 
            this.lblJPYPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblJPYPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJPYPctChg.Location = new System.Drawing.Point(253, 22);
            this.lblJPYPctChg.Name = "lblJPYPctChg";
            this.lblJPYPctChg.Size = new System.Drawing.Size(119, 16);
            this.lblJPYPctChg.TabIndex = 22;
            this.lblJPYPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRUBPctChg
            // 
            this.lblRUBPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRUBPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRUBPctChg.Location = new System.Drawing.Point(1253, 22);
            this.lblRUBPctChg.Name = "lblRUBPctChg";
            this.lblRUBPctChg.Size = new System.Drawing.Size(120, 16);
            this.lblRUBPctChg.TabIndex = 21;
            this.lblRUBPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEURPctChg
            // 
            this.lblEURPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblEURPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEURPctChg.Location = new System.Drawing.Point(128, 22);
            this.lblEURPctChg.Name = "lblEURPctChg";
            this.lblEURPctChg.Size = new System.Drawing.Size(119, 16);
            this.lblEURPctChg.TabIndex = 20;
            this.lblEURPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCHFPctChg
            // 
            this.lblCHFPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCHFPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCHFPctChg.Location = new System.Drawing.Point(878, 22);
            this.lblCHFPctChg.Name = "lblCHFPctChg";
            this.lblCHFPctChg.Size = new System.Drawing.Size(119, 16);
            this.lblCHFPctChg.TabIndex = 19;
            this.lblCHFPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDXPctChg
            // 
            this.lblDXPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDXPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDXPctChg.Location = new System.Drawing.Point(3, 22);
            this.lblDXPctChg.Name = "lblDXPctChg";
            this.lblDXPctChg.Size = new System.Drawing.Size(119, 16);
            this.lblDXPctChg.TabIndex = 18;
            this.lblDXPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNZDPctChg
            // 
            this.lblNZDPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNZDPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNZDPctChg.Location = new System.Drawing.Point(753, 22);
            this.lblNZDPctChg.Name = "lblNZDPctChg";
            this.lblNZDPctChg.Size = new System.Drawing.Size(119, 16);
            this.lblNZDPctChg.TabIndex = 17;
            this.lblNZDPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblKRWPctChg
            // 
            this.lblKRWPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblKRWPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKRWPctChg.Location = new System.Drawing.Point(1128, 22);
            this.lblKRWPctChg.Name = "lblKRWPctChg";
            this.lblKRWPctChg.Size = new System.Drawing.Size(119, 16);
            this.lblKRWPctChg.TabIndex = 16;
            this.lblKRWPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCADPctChg
            // 
            this.lblCADPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCADPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCADPctChg.Location = new System.Drawing.Point(503, 22);
            this.lblCADPctChg.Name = "lblCADPctChg";
            this.lblCADPctChg.Size = new System.Drawing.Size(119, 16);
            this.lblCADPctChg.TabIndex = 15;
            this.lblCADPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGBPPctChg
            // 
            this.lblGBPPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblGBPPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGBPPctChg.Location = new System.Drawing.Point(378, 22);
            this.lblGBPPctChg.Name = "lblGBPPctChg";
            this.lblGBPPctChg.Size = new System.Drawing.Size(119, 16);
            this.lblGBPPctChg.TabIndex = 14;
            this.lblGBPPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAUDPctChg
            // 
            this.lblAUDPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblAUDPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAUDPctChg.Location = new System.Drawing.Point(628, 22);
            this.lblAUDPctChg.Name = "lblAUDPctChg";
            this.lblAUDPctChg.Size = new System.Drawing.Size(119, 16);
            this.lblAUDPctChg.TabIndex = 13;
            this.lblAUDPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCNYPctChg
            // 
            this.lblCNYPctChg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCNYPctChg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCNYPctChg.Location = new System.Drawing.Point(1003, 22);
            this.lblCNYPctChg.Name = "lblCNYPctChg";
            this.lblCNYPctChg.Size = new System.Drawing.Size(119, 16);
            this.lblCNYPctChg.TabIndex = 12;
            this.lblCNYPctChg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEUR
            // 
            this.lblEUR.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEUR.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEUR.Location = new System.Drawing.Point(128, 0);
            this.lblEUR.Name = "lblEUR";
            this.lblEUR.Size = new System.Drawing.Size(119, 19);
            this.lblEUR.TabIndex = 6;
            this.lblEUR.Text = "EUR:";
            this.lblEUR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblJPY
            // 
            this.lblJPY.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblJPY.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJPY.Location = new System.Drawing.Point(253, 0);
            this.lblJPY.Name = "lblJPY";
            this.lblJPY.Size = new System.Drawing.Size(119, 19);
            this.lblJPY.TabIndex = 1;
            this.lblJPY.Text = "JPY:";
            this.lblJPY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDX
            // 
            this.lblDX.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDX.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDX.Location = new System.Drawing.Point(3, 0);
            this.lblDX.Name = "lblDX";
            this.lblDX.Size = new System.Drawing.Size(119, 19);
            this.lblDX.TabIndex = 7;
            this.lblDX.Text = "DX:";
            this.lblDX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAUD
            // 
            this.lblAUD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAUD.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAUD.Location = new System.Drawing.Point(628, 0);
            this.lblAUD.Name = "lblAUD";
            this.lblAUD.Size = new System.Drawing.Size(119, 19);
            this.lblAUD.TabIndex = 3;
            this.lblAUD.Text = "AUD:";
            this.lblAUD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGBP
            // 
            this.lblGBP.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGBP.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGBP.Location = new System.Drawing.Point(378, 0);
            this.lblGBP.Name = "lblGBP";
            this.lblGBP.Size = new System.Drawing.Size(119, 19);
            this.lblGBP.TabIndex = 5;
            this.lblGBP.Text = "GBP:";
            this.lblGBP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCAD
            // 
            this.lblCAD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCAD.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCAD.Location = new System.Drawing.Point(503, 0);
            this.lblCAD.Name = "lblCAD";
            this.lblCAD.Size = new System.Drawing.Size(119, 19);
            this.lblCAD.TabIndex = 4;
            this.lblCAD.Text = "CAD:";
            this.lblCAD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCNY
            // 
            this.lblCNY.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCNY.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCNY.Location = new System.Drawing.Point(1003, 0);
            this.lblCNY.Name = "lblCNY";
            this.lblCNY.Size = new System.Drawing.Size(119, 19);
            this.lblCNY.TabIndex = 0;
            this.lblCNY.Text = "CNY: ";
            this.lblCNY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNZD
            // 
            this.lblNZD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNZD.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNZD.Location = new System.Drawing.Point(753, 0);
            this.lblNZD.Name = "lblNZD";
            this.lblNZD.Size = new System.Drawing.Size(119, 19);
            this.lblNZD.TabIndex = 8;
            this.lblNZD.Text = "NZD:";
            this.lblNZD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCHF
            // 
            this.lblCHF.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCHF.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCHF.Location = new System.Drawing.Point(878, 0);
            this.lblCHF.Name = "lblCHF";
            this.lblCHF.Size = new System.Drawing.Size(119, 19);
            this.lblCHF.TabIndex = 9;
            this.lblCHF.Text = "CHF:";
            this.lblCHF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRUB
            // 
            this.lblRUB.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRUB.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRUB.Location = new System.Drawing.Point(1253, 0);
            this.lblRUB.Name = "lblRUB";
            this.lblRUB.Size = new System.Drawing.Size(120, 19);
            this.lblRUB.TabIndex = 10;
            this.lblRUB.Text = "RUB:";
            this.lblRUB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblKRW
            // 
            this.lblKRW.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblKRW.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKRW.Location = new System.Drawing.Point(1128, 0);
            this.lblKRW.Name = "lblKRW";
            this.lblKRW.Size = new System.Drawing.Size(119, 19);
            this.lblKRW.TabIndex = 11;
            this.lblKRW.Text = "KRW:";
            this.lblKRW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listSymbolIds
            // 
            this.listSymbolIds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listSymbolIds.FormattingEnabled = true;
            this.listSymbolIds.ItemHeight = 16;
            this.listSymbolIds.Location = new System.Drawing.Point(14, 651);
            this.listSymbolIds.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listSymbolIds.Name = "listSymbolIds";
            this.listSymbolIds.Size = new System.Drawing.Size(112, 228);
            this.listSymbolIds.TabIndex = 6;
            this.listSymbolIds.SelectedIndexChanged += new System.EventHandler(this.listSymbolIds_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "Exchanges";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 623);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 26);
            this.label2.TabIndex = 8;
            this.label2.Text = "Symbol IDs";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CryptoPricesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1383, 1006);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listSymbolIds);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.listExchanges);
            this.Controls.Add(this.panelTickers);
            this.Controls.Add(this.chart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CryptoPricesForm";
            this.Text = "Crypto Ticker Prices";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CryptoPricesForm_Closing);
            this.Load += new System.EventHandler(this.CryptoPricesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.Panel panelTickers;
        private System.Windows.Forms.ListBox listExchanges;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblCNY;
        private System.Windows.Forms.Label lblCAD;
        private System.Windows.Forms.Label lblAUD;
        private System.Windows.Forms.Label lblJPY;
        private System.Windows.Forms.Label lblEUR;
        private System.Windows.Forms.Label lblDX;
        private System.Windows.Forms.Label lblGBP;
        private System.Windows.Forms.Label lblNZD;
        private System.Windows.Forms.Label lblCHF;
        private System.Windows.Forms.Label lblRUB;
        private System.Windows.Forms.Label lblKRW;
        private System.Windows.Forms.Label lblCNYPctChg;
        private System.Windows.Forms.Label lblJPYPctChg;
        private System.Windows.Forms.Label lblRUBPctChg;
        private System.Windows.Forms.Label lblEURPctChg;
        private System.Windows.Forms.Label lblCHFPctChg;
        private System.Windows.Forms.Label lblDXPctChg;
        private System.Windows.Forms.Label lblNZDPctChg;
        private System.Windows.Forms.Label lblKRWPctChg;
        private System.Windows.Forms.Label lblCADPctChg;
        private System.Windows.Forms.Label lblGBPPctChg;
        private System.Windows.Forms.Label lblAUDPctChg;
        private System.Windows.Forms.ListBox listSymbolIds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}