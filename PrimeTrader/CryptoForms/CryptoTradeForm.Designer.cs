namespace CryptoForms
{
    partial class CryptoTradeForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CryptoTradeForm));
            this.cryptoTimer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusCryptoMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusCryptoRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageTrade = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelCrypto1 = new System.Windows.Forms.Panel();
            this.btnChart = new System.Windows.Forms.Button();
            this.lblSymbol = new System.Windows.Forms.Label();
            this.gridBalances = new System.Windows.Forms.DataGridView();
            this.LeftColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RightColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExchangeButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.BidSizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BidColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AskColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AskSizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridSymbols = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numSellQty = new System.Windows.Forms.NumericUpDown();
            this.numSellPrice = new System.Windows.Forms.NumericUpDown();
            this.lblSell = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSell = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numBuyQty = new System.Windows.Forms.NumericUpDown();
            this.numBuyPrice = new System.Windows.Forms.NumericUpDown();
            this.lblBuy = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBuy = new System.Windows.Forms.Button();
            this.panelCrypto2 = new System.Windows.Forms.Panel();
            this.panelCrypto3 = new System.Windows.Forms.Panel();
            this.tabPageAlgo = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.listAlgo = new System.Windows.Forms.ListBox();
            this.tabPageStudy = new System.Windows.Forms.TabPage();
            this.btnLaunchStudy = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.listStudy = new System.Windows.Forms.ListBox();
            this.lblFutBTC = new System.Windows.Forms.Label();
            this.lblFutXBT = new System.Windows.Forms.Label();
            this.panelXbt = new System.Windows.Forms.Panel();
            this.panelBtc = new System.Windows.Forms.Panel();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageTrade.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelCrypto1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBalances)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSymbols)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSellQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSellPrice)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBuyQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBuyPrice)).BeginInit();
            this.tabPageAlgo.SuspendLayout();
            this.tabPageStudy.SuspendLayout();
            this.SuspendLayout();
            // 
            // cryptoTimer1
            // 
            this.cryptoTimer1.Interval = 1000;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusCryptoMain,
            this.statusCryptoRight});
            this.statusStrip1.Location = new System.Drawing.Point(0, 987);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1539, 30);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusCryptoMain
            // 
            this.statusCryptoMain.Name = "statusCryptoMain";
            this.statusCryptoMain.Size = new System.Drawing.Size(1344, 25);
            this.statusCryptoMain.Spring = true;
            // 
            // statusCryptoRight
            // 
            this.statusCryptoRight.AutoSize = false;
            this.statusCryptoRight.BackColor = System.Drawing.Color.Silver;
            this.statusCryptoRight.Name = "statusCryptoRight";
            this.statusCryptoRight.Size = new System.Drawing.Size(179, 25);
            this.statusCryptoRight.Text = "?";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageTrade);
            this.tabControl1.Controls.Add(this.tabPageAlgo);
            this.tabControl1.Controls.Add(this.tabPageStudy);
            this.tabControl1.Location = new System.Drawing.Point(0, 30);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(16, 6);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1539, 959);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageTrade
            // 
            this.tabPageTrade.Controls.Add(this.tableLayoutPanel1);
            this.tabPageTrade.Location = new System.Drawing.Point(4, 31);
            this.tabPageTrade.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageTrade.Name = "tabPageTrade";
            this.tabPageTrade.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageTrade.Size = new System.Drawing.Size(1531, 924);
            this.tabPageTrade.TabIndex = 0;
            this.tabPageTrade.Text = "Trade";
            this.tabPageTrade.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelCrypto1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelCrypto2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelCrypto3, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 5);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1510, 908);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panelCrypto1
            // 
            this.panelCrypto1.Controls.Add(this.btnChart);
            this.panelCrypto1.Controls.Add(this.lblSymbol);
            this.panelCrypto1.Controls.Add(this.gridBalances);
            this.panelCrypto1.Controls.Add(this.gridSymbols);
            this.panelCrypto1.Controls.Add(this.panel2);
            this.panelCrypto1.Controls.Add(this.panel1);
            this.panelCrypto1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrypto1.Location = new System.Drawing.Point(3, 1);
            this.panelCrypto1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.panelCrypto1.Name = "panelCrypto1";
            this.panelCrypto1.Size = new System.Drawing.Size(1504, 324);
            this.panelCrypto1.TabIndex = 1;
            // 
            // btnChart
            // 
            this.btnChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChart.Image = global::CryptoForms.Properties.Resources.base_charts;
            this.btnChart.Location = new System.Drawing.Point(1456, 281);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(45, 40);
            this.btnChart.TabIndex = 23;
            this.btnChart.UseVisualStyleBackColor = true;
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // lblSymbol
            // 
            this.lblSymbol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSymbol.BackColor = System.Drawing.Color.Gainsboro;
            this.lblSymbol.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbol.ForeColor = System.Drawing.Color.Black;
            this.lblSymbol.Location = new System.Drawing.Point(979, 110);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(205, 54);
            this.lblSymbol.TabIndex = 22;
            this.lblSymbol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gridBalances
            // 
            this.gridBalances.AllowUserToAddRows = false;
            this.gridBalances.AllowUserToDeleteRows = false;
            this.gridBalances.AllowUserToResizeColumns = false;
            this.gridBalances.AllowUserToResizeRows = false;
            this.gridBalances.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridBalances.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.gridBalances.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridBalances.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridBalances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBalances.ColumnHeadersVisible = false;
            this.gridBalances.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LeftColumn,
            this.RightColumn,
            this.ExchangeButtonColumn,
            this.BidSizeColumn,
            this.BidColumn,
            this.AskColumn,
            this.AskSizeColumn});
            this.gridBalances.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridBalances.Location = new System.Drawing.Point(4, 3);
            this.gridBalances.MultiSelect = false;
            this.gridBalances.Name = "gridBalances";
            this.gridBalances.ReadOnly = true;
            this.gridBalances.RowHeadersVisible = false;
            this.gridBalances.RowTemplate.Height = 28;
            this.gridBalances.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gridBalances.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridBalances.Size = new System.Drawing.Size(681, 313);
            this.gridBalances.TabIndex = 21;
            this.gridBalances.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridBalances_CellContentClick);
            // 
            // LeftColumn
            // 
            this.LeftColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.LeftColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.LeftColumn.FillWeight = 55F;
            this.LeftColumn.HeaderText = "LeftBalance";
            this.LeftColumn.Name = "LeftColumn";
            this.LeftColumn.ReadOnly = true;
            this.LeftColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // RightColumn
            // 
            this.RightColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.RightColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.RightColumn.FillWeight = 55F;
            this.RightColumn.HeaderText = "RightBalance";
            this.RightColumn.Name = "RightColumn";
            this.RightColumn.ReadOnly = true;
            this.RightColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ExchangeButtonColumn
            // 
            this.ExchangeButtonColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.ExchangeButtonColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.ExchangeButtonColumn.FillWeight = 50F;
            this.ExchangeButtonColumn.HeaderText = "ExchangeButton";
            this.ExchangeButtonColumn.Name = "ExchangeButtonColumn";
            this.ExchangeButtonColumn.ReadOnly = true;
            this.ExchangeButtonColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ExchangeButtonColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // BidSizeColumn
            // 
            this.BidSizeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.BidSizeColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.BidSizeColumn.FillWeight = 30F;
            this.BidSizeColumn.HeaderText = "BidSize";
            this.BidSizeColumn.Name = "BidSizeColumn";
            this.BidSizeColumn.ReadOnly = true;
            this.BidSizeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // BidColumn
            // 
            this.BidColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.BidColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.BidColumn.FillWeight = 80F;
            this.BidColumn.HeaderText = "Bid";
            this.BidColumn.Name = "BidColumn";
            this.BidColumn.ReadOnly = true;
            this.BidColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // AskColumn
            // 
            this.AskColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.LightCoral;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.LightCoral;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.AskColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.AskColumn.FillWeight = 80F;
            this.AskColumn.HeaderText = "Ask";
            this.AskColumn.Name = "AskColumn";
            this.AskColumn.ReadOnly = true;
            this.AskColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // AskSizeColumn
            // 
            this.AskSizeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightCoral;
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.LightCoral;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.AskSizeColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.AskSizeColumn.FillWeight = 30F;
            this.AskSizeColumn.HeaderText = "AskSize";
            this.AskSizeColumn.Name = "AskSizeColumn";
            this.AskSizeColumn.ReadOnly = true;
            this.AskSizeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // gridSymbols
            // 
            this.gridSymbols.AllowUserToAddRows = false;
            this.gridSymbols.AllowUserToDeleteRows = false;
            this.gridSymbols.AllowUserToResizeColumns = false;
            this.gridSymbols.AllowUserToResizeRows = false;
            this.gridSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSymbols.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridSymbols.BackgroundColor = System.Drawing.Color.White;
            this.gridSymbols.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridSymbols.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridSymbols.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridSymbols.ColumnHeadersVisible = false;
            this.gridSymbols.EnableHeadersVisualStyles = false;
            this.gridSymbols.Location = new System.Drawing.Point(704, 8);
            this.gridSymbols.MultiSelect = false;
            this.gridSymbols.Name = "gridSymbols";
            this.gridSymbols.ReadOnly = true;
            this.gridSymbols.RowHeadersVisible = false;
            this.gridSymbols.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridSymbols.RowTemplate.Height = 24;
            this.gridSymbols.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gridSymbols.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridSymbols.Size = new System.Drawing.Size(793, 90);
            this.gridSymbols.TabIndex = 20;
            this.gridSymbols.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridSymbols_CellContentClick);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.LightCoral;
            this.panel2.Controls.Add(this.numSellQty);
            this.panel2.Controls.Add(this.numSellPrice);
            this.panel2.Controls.Add(this.lblSell);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnSell);
            this.panel2.Location = new System.Drawing.Point(1103, 104);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(312, 209);
            this.panel2.TabIndex = 16;
            // 
            // numSellQty
            // 
            this.numSellQty.DecimalPlaces = 6;
            this.numSellQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSellQty.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numSellQty.Location = new System.Drawing.Point(105, 113);
            this.numSellQty.Name = "numSellQty";
            this.numSellQty.Size = new System.Drawing.Size(162, 30);
            this.numSellQty.TabIndex = 19;
            // 
            // numSellPrice
            // 
            this.numSellPrice.DecimalPlaces = 8;
            this.numSellPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSellPrice.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numSellPrice.Location = new System.Drawing.Point(105, 78);
            this.numSellPrice.Name = "numSellPrice";
            this.numSellPrice.Size = new System.Drawing.Size(162, 30);
            this.numSellPrice.TabIndex = 18;
            // 
            // lblSell
            // 
            this.lblSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSell.Location = new System.Drawing.Point(135, 10);
            this.lblSell.Name = "lblSell";
            this.lblSell.Size = new System.Drawing.Size(164, 31);
            this.lblSell.TabIndex = 17;
            this.lblSell.Text = "(click exchange)";
            this.lblSell.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(28, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 25);
            this.label3.TabIndex = 15;
            this.label3.Text = "Qty";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(28, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 25);
            this.label4.TabIndex = 13;
            this.label4.Text = "Price";
            // 
            // btnSell
            // 
            this.btnSell.BackColor = System.Drawing.Color.Red;
            this.btnSell.Enabled = false;
            this.btnSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSell.ForeColor = System.Drawing.Color.White;
            this.btnSell.Location = new System.Drawing.Point(105, 162);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(118, 39);
            this.btnSell.TabIndex = 3;
            this.btnSell.Text = "Sell";
            this.btnSell.UseVisualStyleBackColor = false;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Controls.Add(this.numBuyQty);
            this.panel1.Controls.Add(this.numBuyPrice);
            this.panel1.Controls.Add(this.lblBuy);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnBuy);
            this.panel1.Location = new System.Drawing.Point(753, 104);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(312, 209);
            this.panel1.TabIndex = 15;
            // 
            // numBuyQty
            // 
            this.numBuyQty.DecimalPlaces = 6;
            this.numBuyQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numBuyQty.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numBuyQty.Location = new System.Drawing.Point(102, 113);
            this.numBuyQty.Name = "numBuyQty";
            this.numBuyQty.Size = new System.Drawing.Size(162, 30);
            this.numBuyQty.TabIndex = 15;
            // 
            // numBuyPrice
            // 
            this.numBuyPrice.DecimalPlaces = 8;
            this.numBuyPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numBuyPrice.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numBuyPrice.Location = new System.Drawing.Point(102, 78);
            this.numBuyPrice.Name = "numBuyPrice";
            this.numBuyPrice.Size = new System.Drawing.Size(162, 30);
            this.numBuyPrice.TabIndex = 14;
            // 
            // lblBuy
            // 
            this.lblBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuy.Location = new System.Drawing.Point(12, 10);
            this.lblBuy.Name = "lblBuy";
            this.lblBuy.Size = new System.Drawing.Size(164, 31);
            this.lblBuy.TabIndex = 13;
            this.lblBuy.Text = "(click exchange)";
            this.lblBuy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 25);
            this.label2.TabIndex = 11;
            this.label2.Text = "Qty";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 25);
            this.label1.TabIndex = 9;
            this.label1.Text = "Price";
            // 
            // btnBuy
            // 
            this.btnBuy.BackColor = System.Drawing.Color.Blue;
            this.btnBuy.Enabled = false;
            this.btnBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuy.ForeColor = System.Drawing.Color.White;
            this.btnBuy.Location = new System.Drawing.Point(101, 162);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(118, 39);
            this.btnBuy.TabIndex = 8;
            this.btnBuy.Text = "Buy";
            this.btnBuy.UseVisualStyleBackColor = false;
            this.btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
            // 
            // panelCrypto2
            // 
            this.panelCrypto2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrypto2.Location = new System.Drawing.Point(3, 327);
            this.panelCrypto2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.panelCrypto2.Name = "panelCrypto2";
            this.panelCrypto2.Size = new System.Drawing.Size(1504, 288);
            this.panelCrypto2.TabIndex = 5;
            // 
            // panelCrypto3
            // 
            this.panelCrypto3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrypto3.Location = new System.Drawing.Point(3, 620);
            this.panelCrypto3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelCrypto3.Name = "panelCrypto3";
            this.panelCrypto3.Size = new System.Drawing.Size(1504, 284);
            this.panelCrypto3.TabIndex = 6;
            // 
            // tabPageAlgo
            // 
            this.tabPageAlgo.Controls.Add(this.label8);
            this.tabPageAlgo.Controls.Add(this.listAlgo);
            this.tabPageAlgo.Location = new System.Drawing.Point(4, 31);
            this.tabPageAlgo.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageAlgo.Name = "tabPageAlgo";
            this.tabPageAlgo.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageAlgo.Size = new System.Drawing.Size(1531, 924);
            this.tabPageAlgo.TabIndex = 1;
            this.tabPageAlgo.Text = "Algo";
            this.tabPageAlgo.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(42, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(295, 33);
            this.label8.TabIndex = 3;
            this.label8.Text = "Algo Trade";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listAlgo
            // 
            this.listAlgo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listAlgo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listAlgo.FormattingEnabled = true;
            this.listAlgo.ItemHeight = 20;
            this.listAlgo.Location = new System.Drawing.Point(19, 55);
            this.listAlgo.Name = "listAlgo";
            this.listAlgo.Size = new System.Drawing.Size(370, 644);
            this.listAlgo.TabIndex = 2;
            // 
            // tabPageStudy
            // 
            this.tabPageStudy.Controls.Add(this.btnLaunchStudy);
            this.tabPageStudy.Controls.Add(this.label6);
            this.tabPageStudy.Controls.Add(this.listStudy);
            this.tabPageStudy.Location = new System.Drawing.Point(4, 31);
            this.tabPageStudy.Name = "tabPageStudy";
            this.tabPageStudy.Size = new System.Drawing.Size(1531, 924);
            this.tabPageStudy.TabIndex = 2;
            this.tabPageStudy.Text = "Study";
            this.tabPageStudy.UseVisualStyleBackColor = true;
            // 
            // btnLaunchStudy
            // 
            this.btnLaunchStudy.Location = new System.Drawing.Point(484, 319);
            this.btnLaunchStudy.Name = "btnLaunchStudy";
            this.btnLaunchStudy.Size = new System.Drawing.Size(163, 59);
            this.btnLaunchStudy.TabIndex = 2;
            this.btnLaunchStudy.Text = "Launch Study";
            this.btnLaunchStudy.UseVisualStyleBackColor = true;
            this.btnLaunchStudy.Click += new System.EventHandler(this.btnLaunchStudy_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(50, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(295, 33);
            this.label6.TabIndex = 1;
            this.label6.Text = "Data Analytics Study";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listStudy
            // 
            this.listStudy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listStudy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listStudy.FormattingEnabled = true;
            this.listStudy.ItemHeight = 20;
            this.listStudy.Location = new System.Drawing.Point(19, 55);
            this.listStudy.Name = "listStudy";
            this.listStudy.Size = new System.Drawing.Size(370, 644);
            this.listStudy.TabIndex = 0;
            // 
            // lblFutBTC
            // 
            this.lblFutBTC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFutBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFutBTC.Location = new System.Drawing.Point(305, 10);
            this.lblFutBTC.Name = "lblFutBTC";
            this.lblFutBTC.Size = new System.Drawing.Size(72, 23);
            this.lblFutBTC.TabIndex = 5;
            this.lblFutBTC.Text = "0";
            this.lblFutBTC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFutXBT
            // 
            this.lblFutXBT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFutXBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFutXBT.Location = new System.Drawing.Point(969, 10);
            this.lblFutXBT.Name = "lblFutXBT";
            this.lblFutXBT.Size = new System.Drawing.Size(72, 23);
            this.lblFutXBT.TabIndex = 7;
            this.lblFutXBT.Text = "0";
            this.lblFutXBT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelXbt
            // 
            this.panelXbt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelXbt.Location = new System.Drawing.Point(1038, 5);
            this.panelXbt.Name = "panelXbt";
            this.panelXbt.Size = new System.Drawing.Size(489, 35);
            this.panelXbt.TabIndex = 8;
            // 
            // panelBtc
            // 
            this.panelBtc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBtc.Location = new System.Drawing.Point(374, 5);
            this.panelBtc.Name = "panelBtc";
            this.panelBtc.Size = new System.Drawing.Size(489, 35);
            this.panelBtc.TabIndex = 9;
            // 
            // CryptoTradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1539, 1017);
            this.Controls.Add(this.panelBtc);
            this.Controls.Add(this.panelXbt);
            this.Controls.Add(this.lblFutXBT);
            this.Controls.Add(this.lblFutBTC);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CryptoTradeForm";
            this.Text = "Crypto Trader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CryptoPricesForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageTrade.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelCrypto1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBalances)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSymbols)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSellQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSellPrice)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBuyQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBuyPrice)).EndInit();
            this.tabPageAlgo.ResumeLayout(false);
            this.tabPageStudy.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer cryptoTimer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusCryptoMain;
        private System.Windows.Forms.ToolStripStatusLabel statusCryptoRight;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageTrade;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelCrypto1;
        private System.Windows.Forms.Panel panelCrypto2;
        private System.Windows.Forms.Panel panelCrypto3;
        private System.Windows.Forms.TabPage tabPageAlgo;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBuy;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView gridBalances;
        private System.Windows.Forms.DataGridView gridSymbols;
        private System.Windows.Forms.Label lblSymbol;
        private System.Windows.Forms.Label lblSell;
        private System.Windows.Forms.Label lblBuy;
        private System.Windows.Forms.NumericUpDown numBuyPrice;
        private System.Windows.Forms.NumericUpDown numBuyQty;
        private System.Windows.Forms.NumericUpDown numSellQty;
        private System.Windows.Forms.NumericUpDown numSellPrice;
        private System.Windows.Forms.Label lblFutBTC;
        private System.Windows.Forms.Label lblFutXBT;
        private System.Windows.Forms.Button btnChart;
        private System.Windows.Forms.TabPage tabPageStudy;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox listAlgo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listStudy;
        private System.Windows.Forms.Button btnLaunchStudy;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeftColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RightColumn;
        private System.Windows.Forms.DataGridViewButtonColumn ExchangeButtonColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BidSizeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BidColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AskColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AskSizeColumn;
        private System.Windows.Forms.Panel panelXbt;
        private System.Windows.Forms.Panel panelBtc;
    }
}

