namespace IQFeed.GUI
{
    partial class PricesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PricesForm));
            this.splitContainerLevel1 = new System.Windows.Forms.SplitContainer();
            this.gridLevel1 = new System.Windows.Forms.DataGridView();
            this.gridSpreads = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.gridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteRowContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cancelContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlPrices = new System.Windows.Forms.TabControl();
            this.tabPageView = new System.Windows.Forms.TabPage();
            this.tabPageControls = new System.Windows.Forms.TabPage();
            this.rtbFileText = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnEditSpreadsFile = new System.Windows.Forms.Button();
            this.btnEditSymbolsFile = new System.Windows.Forms.Button();
            this.btnReloadSymbols = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnGetFundamentalFields = new System.Windows.Forms.Button();
            this.btnGetUpdateSummaryFields = new System.Windows.Forms.Button();
            this.btnSetFieldset = new System.Windows.Forms.Button();
            this.btnGetFieldset = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnForce = new System.Windows.Forms.Button();
            this.btnRemoveRegionals = new System.Windows.Forms.Button();
            this.btnWatchRegionals = new System.Windows.Forms.Button();
            this.btnTradesOnly = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnTimestamp = new System.Windows.Forms.Button();
            this.btnNewsOff = new System.Windows.Forms.Button();
            this.btnRemoveAllWatches = new System.Windows.Forms.Button();
            this.btnWatch = new System.Windows.Forms.Button();
            this.btnNewsOn = new System.Windows.Forms.Button();
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnGetCurrentWatches = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLevel1)).BeginInit();
            this.splitContainerLevel1.Panel1.SuspendLayout();
            this.splitContainerLevel1.Panel2.SuspendLayout();
            this.splitContainerLevel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLevel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSpreads)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.gridContextMenu.SuspendLayout();
            this.tabControlPrices.SuspendLayout();
            this.tabPageView.SuspendLayout();
            this.tabPageControls.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerLevel1
            // 
            this.splitContainerLevel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerLevel1.Location = new System.Drawing.Point(6, 6);
            this.splitContainerLevel1.Name = "splitContainerLevel1";
            this.splitContainerLevel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLevel1.Panel1
            // 
            this.splitContainerLevel1.Panel1.Controls.Add(this.gridLevel1);
            // 
            // splitContainerLevel1.Panel2
            // 
            this.splitContainerLevel1.Panel2.Controls.Add(this.gridSpreads);
            this.splitContainerLevel1.Size = new System.Drawing.Size(1122, 1087);
            this.splitContainerLevel1.SplitterDistance = 799;
            this.splitContainerLevel1.TabIndex = 30;
            // 
            // gridLevel1
            // 
            this.gridLevel1.AllowUserToAddRows = false;
            this.gridLevel1.AllowUserToDeleteRows = false;
            this.gridLevel1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridLevel1.DefaultCellStyle = dataGridViewCellStyle1;
            this.gridLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLevel1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridLevel1.Location = new System.Drawing.Point(0, 0);
            this.gridLevel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridLevel1.MultiSelect = false;
            this.gridLevel1.Name = "gridLevel1";
            this.gridLevel1.ReadOnly = true;
            this.gridLevel1.RowTemplate.Height = 28;
            this.gridLevel1.Size = new System.Drawing.Size(1122, 799);
            this.gridLevel1.TabIndex = 25;
            this.gridLevel1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridLevel1_CellContentDoubleClick);
            this.gridLevel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridLevel1_MouseClick);
            // 
            // gridSpreads
            // 
            this.gridSpreads.AllowUserToAddRows = false;
            this.gridSpreads.AllowUserToDeleteRows = false;
            this.gridSpreads.AllowUserToResizeColumns = false;
            this.gridSpreads.AllowUserToResizeRows = false;
            this.gridSpreads.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridSpreads.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridSpreads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSpreads.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridSpreads.Location = new System.Drawing.Point(0, 0);
            this.gridSpreads.Name = "gridSpreads";
            this.gridSpreads.ReadOnly = true;
            this.gridSpreads.RowHeadersVisible = false;
            this.gridSpreads.RowTemplate.Height = 24;
            this.gridSpreads.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSpreads.Size = new System.Drawing.Size(1122, 284);
            this.gridSpreads.TabIndex = 23;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel1,
            this.statusLabel2,
            this.statusProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1142);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1139, 46);
            this.statusStrip1.TabIndex = 31;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel1
            // 
            this.statusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(881, 41);
            this.statusLabel1.Spring = true;
            this.statusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusLabel2
            // 
            this.statusLabel2.AutoSize = false;
            this.statusLabel2.BackColor = System.Drawing.Color.DarkGray;
            this.statusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusLabel2.Name = "statusLabel2";
            this.statusLabel2.Size = new System.Drawing.Size(141, 41);
            // 
            // statusProgress
            // 
            this.statusProgress.AutoSize = false;
            this.statusProgress.Name = "statusProgress";
            this.statusProgress.Size = new System.Drawing.Size(100, 40);
            this.statusProgress.Step = 1;
            this.statusProgress.Value = 100;
            // 
            // gridContextMenu
            // 
            this.gridContextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.gridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteRowContextMenuItem,
            this.toolStripMenuItem1,
            this.cancelContextMenuItem});
            this.gridContextMenu.Name = "gridContextMenu";
            this.gridContextMenu.Size = new System.Drawing.Size(156, 58);
            // 
            // deleteRowContextMenuItem
            // 
            this.deleteRowContextMenuItem.Name = "deleteRowContextMenuItem";
            this.deleteRowContextMenuItem.Size = new System.Drawing.Size(155, 24);
            this.deleteRowContextMenuItem.Text = "Delete Row";
            this.deleteRowContextMenuItem.Click += new System.EventHandler(this.deleteRowContextMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 6);
            // 
            // cancelContextMenuItem
            // 
            this.cancelContextMenuItem.Name = "cancelContextMenuItem";
            this.cancelContextMenuItem.Size = new System.Drawing.Size(155, 24);
            this.cancelContextMenuItem.Text = "Cancel";
            this.cancelContextMenuItem.Click += new System.EventHandler(this.cancelContextMenuItem_Click);
            // 
            // tabControlPrices
            // 
            this.tabControlPrices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlPrices.Controls.Add(this.tabPageView);
            this.tabControlPrices.Controls.Add(this.tabPageControls);
            this.tabControlPrices.Location = new System.Drawing.Point(0, 5);
            this.tabControlPrices.Name = "tabControlPrices";
            this.tabControlPrices.Padding = new System.Drawing.Point(20, 6);
            this.tabControlPrices.SelectedIndex = 0;
            this.tabControlPrices.Size = new System.Drawing.Size(1142, 1134);
            this.tabControlPrices.TabIndex = 32;
            // 
            // tabPageView
            // 
            this.tabPageView.Controls.Add(this.splitContainerLevel1);
            this.tabPageView.Location = new System.Drawing.Point(4, 31);
            this.tabPageView.Name = "tabPageView";
            this.tabPageView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageView.Size = new System.Drawing.Size(1134, 1099);
            this.tabPageView.TabIndex = 0;
            this.tabPageView.Text = "View";
            this.tabPageView.UseVisualStyleBackColor = true;
            // 
            // tabPageControls
            // 
            this.tabPageControls.BackColor = System.Drawing.Color.LightBlue;
            this.tabPageControls.Controls.Add(this.rtbFileText);
            this.tabPageControls.Controls.Add(this.tableLayoutPanel3);
            this.tabPageControls.Controls.Add(this.panelButtons);
            this.tabPageControls.Location = new System.Drawing.Point(4, 31);
            this.tabPageControls.Name = "tabPageControls";
            this.tabPageControls.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageControls.Size = new System.Drawing.Size(1134, 1099);
            this.tabPageControls.TabIndex = 1;
            this.tabPageControls.Text = "Controls";
            // 
            // rtbFileText
            // 
            this.rtbFileText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbFileText.Location = new System.Drawing.Point(182, 340);
            this.rtbFileText.Name = "rtbFileText";
            this.rtbFileText.Size = new System.Drawing.Size(934, 739);
            this.rtbFileText.TabIndex = 36;
            this.rtbFileText.Text = "";
            this.rtbFileText.Visible = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.btnEditSpreadsFile, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnEditSymbolsFile, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnReloadSymbols, 0, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(17, 358);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(140, 106);
            this.tableLayoutPanel3.TabIndex = 35;
            // 
            // btnEditSpreadsFile
            // 
            this.btnEditSpreadsFile.Location = new System.Drawing.Point(3, 72);
            this.btnEditSpreadsFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEditSpreadsFile.Name = "btnEditSpreadsFile";
            this.btnEditSpreadsFile.Size = new System.Drawing.Size(134, 30);
            this.btnEditSpreadsFile.TabIndex = 27;
            this.btnEditSpreadsFile.Text = "Edit Spreads File";
            this.btnEditSpreadsFile.UseVisualStyleBackColor = true;
            this.btnEditSpreadsFile.Click += new System.EventHandler(this.btnEditSpreadsFile_Click);
            // 
            // btnEditSymbolsFile
            // 
            this.btnEditSymbolsFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditSymbolsFile.Location = new System.Drawing.Point(3, 2);
            this.btnEditSymbolsFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEditSymbolsFile.Name = "btnEditSymbolsFile";
            this.btnEditSymbolsFile.Size = new System.Drawing.Size(134, 31);
            this.btnEditSymbolsFile.TabIndex = 25;
            this.btnEditSymbolsFile.Text = "Edit Symbols FIle";
            this.btnEditSymbolsFile.UseVisualStyleBackColor = true;
            this.btnEditSymbolsFile.Click += new System.EventHandler(this.btnEditSymbolsFile_Click);
            // 
            // btnReloadSymbols
            // 
            this.btnReloadSymbols.Location = new System.Drawing.Point(3, 37);
            this.btnReloadSymbols.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReloadSymbols.Name = "btnReloadSymbols";
            this.btnReloadSymbols.Size = new System.Drawing.Size(134, 30);
            this.btnReloadSymbols.TabIndex = 26;
            this.btnReloadSymbols.Text = "Reload Symbols";
            this.btnReloadSymbols.UseVisualStyleBackColor = true;
            this.btnReloadSymbols.Click += new System.EventHandler(this.btnReloadSymbols_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.Controls.Add(this.tableLayoutPanel2);
            this.panelButtons.Controls.Add(this.tableLayoutPanel1);
            this.panelButtons.Location = new System.Drawing.Point(17, 29);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1099, 287);
            this.panelButtons.TabIndex = 33;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.btnGetFundamentalFields, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGetUpdateSummaryFields, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnSetFieldset, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnGetFieldset, 0, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 138);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(195, 138);
            this.tableLayoutPanel2.TabIndex = 34;
            // 
            // btnGetFundamentalFields
            // 
            this.btnGetFundamentalFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGetFundamentalFields.Location = new System.Drawing.Point(4, 4);
            this.btnGetFundamentalFields.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetFundamentalFields.Name = "btnGetFundamentalFields";
            this.btnGetFundamentalFields.Size = new System.Drawing.Size(187, 26);
            this.btnGetFundamentalFields.TabIndex = 19;
            this.btnGetFundamentalFields.Text = "Get All Fundamental Fields";
            this.btnGetFundamentalFields.UseVisualStyleBackColor = true;
            // 
            // btnGetUpdateSummaryFields
            // 
            this.btnGetUpdateSummaryFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGetUpdateSummaryFields.Location = new System.Drawing.Point(4, 38);
            this.btnGetUpdateSummaryFields.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetUpdateSummaryFields.Name = "btnGetUpdateSummaryFields";
            this.btnGetUpdateSummaryFields.Size = new System.Drawing.Size(187, 26);
            this.btnGetUpdateSummaryFields.TabIndex = 20;
            this.btnGetUpdateSummaryFields.Text = "Get All Update/Summary Fields";
            this.btnGetUpdateSummaryFields.UseVisualStyleBackColor = true;
            // 
            // btnSetFieldset
            // 
            this.btnSetFieldset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetFieldset.Location = new System.Drawing.Point(4, 106);
            this.btnSetFieldset.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetFieldset.Name = "btnSetFieldset";
            this.btnSetFieldset.Size = new System.Drawing.Size(187, 28);
            this.btnSetFieldset.TabIndex = 21;
            this.btnSetFieldset.Text = "Set Fieldset";
            this.btnSetFieldset.UseVisualStyleBackColor = true;
            // 
            // btnGetFieldset
            // 
            this.btnGetFieldset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGetFieldset.Location = new System.Drawing.Point(4, 72);
            this.btnGetFieldset.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetFieldset.Name = "btnGetFieldset";
            this.btnGetFieldset.Size = new System.Drawing.Size(187, 26);
            this.btnGetFieldset.TabIndex = 20;
            this.btnGetFieldset.Text = "Get Current Fieldset";
            this.btnGetFieldset.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnForce, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnRemoveRegionals, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnWatchRegionals, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTradesOnly, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRemove, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnTimestamp, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnNewsOff, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnRemoveAllWatches, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnWatch, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnNewsOn, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtRequest, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDisconnect, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnConnect, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnGetCurrentWatches, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(773, 110);
            this.tableLayoutPanel1.TabIndex = 33;
            // 
            // btnForce
            // 
            this.btnForce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnForce.Location = new System.Drawing.Point(158, 76);
            this.btnForce.Margin = new System.Windows.Forms.Padding(4);
            this.btnForce.Name = "btnForce";
            this.btnForce.Size = new System.Drawing.Size(146, 30);
            this.btnForce.TabIndex = 23;
            this.btnForce.Text = "Force";
            this.btnForce.UseVisualStyleBackColor = true;
            // 
            // btnRemoveRegionals
            // 
            this.btnRemoveRegionals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveRegionals.Location = new System.Drawing.Point(4, 76);
            this.btnRemoveRegionals.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemoveRegionals.Name = "btnRemoveRegionals";
            this.btnRemoveRegionals.Size = new System.Drawing.Size(146, 30);
            this.btnRemoveRegionals.TabIndex = 22;
            this.btnRemoveRegionals.Text = "Remove Regionals";
            this.btnRemoveRegionals.UseVisualStyleBackColor = true;
            // 
            // btnWatchRegionals
            // 
            this.btnWatchRegionals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWatchRegionals.Location = new System.Drawing.Point(4, 40);
            this.btnWatchRegionals.Margin = new System.Windows.Forms.Padding(4);
            this.btnWatchRegionals.Name = "btnWatchRegionals";
            this.btnWatchRegionals.Size = new System.Drawing.Size(146, 28);
            this.btnWatchRegionals.TabIndex = 24;
            this.btnWatchRegionals.Text = "Watch Regionals";
            this.btnWatchRegionals.UseVisualStyleBackColor = true;
            // 
            // btnTradesOnly
            // 
            this.btnTradesOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTradesOnly.Location = new System.Drawing.Point(466, 4);
            this.btnTradesOnly.Margin = new System.Windows.Forms.Padding(4);
            this.btnTradesOnly.Name = "btnTradesOnly";
            this.btnTradesOnly.Size = new System.Drawing.Size(146, 28);
            this.btnTradesOnly.TabIndex = 19;
            this.btnTradesOnly.Text = "Trades Only Watch";
            this.btnTradesOnly.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemove.Location = new System.Drawing.Point(620, 4);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(149, 28);
            this.btnRemove.TabIndex = 9;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnTimestamp
            // 
            this.btnTimestamp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTimestamp.Location = new System.Drawing.Point(158, 40);
            this.btnTimestamp.Margin = new System.Windows.Forms.Padding(4);
            this.btnTimestamp.Name = "btnTimestamp";
            this.btnTimestamp.Size = new System.Drawing.Size(146, 28);
            this.btnTimestamp.TabIndex = 25;
            this.btnTimestamp.Text = "Timestamp";
            this.btnTimestamp.UseVisualStyleBackColor = true;
            // 
            // btnNewsOff
            // 
            this.btnNewsOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewsOff.Location = new System.Drawing.Point(312, 76);
            this.btnNewsOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewsOff.Name = "btnNewsOff";
            this.btnNewsOff.Size = new System.Drawing.Size(146, 30);
            this.btnNewsOff.TabIndex = 26;
            this.btnNewsOff.Text = "News Off";
            this.btnNewsOff.UseVisualStyleBackColor = true;
            // 
            // btnRemoveAllWatches
            // 
            this.btnRemoveAllWatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveAllWatches.Location = new System.Drawing.Point(620, 76);
            this.btnRemoveAllWatches.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemoveAllWatches.Name = "btnRemoveAllWatches";
            this.btnRemoveAllWatches.Size = new System.Drawing.Size(149, 30);
            this.btnRemoveAllWatches.TabIndex = 15;
            this.btnRemoveAllWatches.Text = "Remove All Watches";
            this.btnRemoveAllWatches.UseVisualStyleBackColor = true;
            // 
            // btnWatch
            // 
            this.btnWatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWatch.Location = new System.Drawing.Point(312, 4);
            this.btnWatch.Margin = new System.Windows.Forms.Padding(4);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(146, 28);
            this.btnWatch.TabIndex = 0;
            this.btnWatch.Text = "Watch";
            this.btnWatch.UseVisualStyleBackColor = true;
            // 
            // btnNewsOn
            // 
            this.btnNewsOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewsOn.Location = new System.Drawing.Point(312, 40);
            this.btnNewsOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewsOn.Name = "btnNewsOn";
            this.btnNewsOn.Size = new System.Drawing.Size(146, 28);
            this.btnNewsOn.TabIndex = 27;
            this.btnNewsOn.Text = "News On";
            this.btnNewsOn.UseVisualStyleBackColor = true;
            // 
            // txtRequest
            // 
            this.txtRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRequest.Location = new System.Drawing.Point(158, 4);
            this.txtRequest.Margin = new System.Windows.Forms.Padding(4);
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(146, 22);
            this.txtRequest.TabIndex = 1;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDisconnect.Location = new System.Drawing.Point(466, 76);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(146, 30);
            this.btnDisconnect.TabIndex = 28;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConnect.Location = new System.Drawing.Point(466, 40);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(146, 28);
            this.btnConnect.TabIndex = 29;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnGetCurrentWatches
            // 
            this.btnGetCurrentWatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGetCurrentWatches.Location = new System.Drawing.Point(620, 40);
            this.btnGetCurrentWatches.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetCurrentWatches.Name = "btnGetCurrentWatches";
            this.btnGetCurrentWatches.Size = new System.Drawing.Size(149, 28);
            this.btnGetCurrentWatches.TabIndex = 30;
            this.btnGetCurrentWatches.Text = "Get Current Watches";
            this.btnGetCurrentWatches.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 36);
            this.label1.TabIndex = 2;
            this.label1.Text = "Symbol / Request Data:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PricesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 1188);
            this.Controls.Add(this.tabControlPrices);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PricesForm";
            this.Text = "Prices and Spreads";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PricesForm_FormClosing);
            this.Load += new System.EventHandler(this.PricesForm_Load);
            this.splitContainerLevel1.Panel1.ResumeLayout(false);
            this.splitContainerLevel1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLevel1)).EndInit();
            this.splitContainerLevel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLevel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSpreads)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.gridContextMenu.ResumeLayout(false);
            this.tabControlPrices.ResumeLayout(false);
            this.tabPageView.ResumeLayout(false);
            this.tabPageControls.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainerLevel1;
        private System.Windows.Forms.DataGridView gridLevel1;
        private System.Windows.Forms.DataGridView gridSpreads;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel2;
        private System.Windows.Forms.ToolStripProgressBar statusProgress;
        private System.Windows.Forms.ContextMenuStrip gridContextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteRowContextMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cancelContextMenuItem;
        private System.Windows.Forms.TabControl tabControlPrices;
        private System.Windows.Forms.TabPage tabPageView;
        private System.Windows.Forms.TabPage tabPageControls;
        private System.Windows.Forms.RichTextBox rtbFileText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnEditSpreadsFile;
        private System.Windows.Forms.Button btnEditSymbolsFile;
        private System.Windows.Forms.Button btnReloadSymbols;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnGetFundamentalFields;
        private System.Windows.Forms.Button btnGetUpdateSummaryFields;
        private System.Windows.Forms.Button btnSetFieldset;
        private System.Windows.Forms.Button btnGetFieldset;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnForce;
        private System.Windows.Forms.Button btnRemoveRegionals;
        private System.Windows.Forms.Button btnWatchRegionals;
        private System.Windows.Forms.Button btnTradesOnly;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnTimestamp;
        private System.Windows.Forms.Button btnNewsOff;
        private System.Windows.Forms.Button btnRemoveAllWatches;
        private System.Windows.Forms.Button btnWatch;
        private System.Windows.Forms.Button btnNewsOn;
        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnGetCurrentWatches;
        private System.Windows.Forms.Label label1;
    }
}

