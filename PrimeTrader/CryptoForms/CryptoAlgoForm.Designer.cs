namespace CryptoForms
{
    partial class CryptoAlgoForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelGridArgs = new System.Windows.Forms.Panel();
            this.gridArgs = new System.Windows.Forms.DataGridView();
            this.listAlgos = new System.Windows.Forms.ListBox();
            this.rdoMain = new System.Windows.Forms.RadioButton();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.tableLayoutPanel3Rows = new System.Windows.Forms.TableLayoutPanel();
            this.panelCrypto2 = new System.Windows.Forms.Panel();
            this.panelCrypto3 = new System.Windows.Forms.Panel();
            this.panelCrypto1 = new System.Windows.Forms.Panel();
            this.ArgNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArgValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelMain.SuspendLayout();
            this.panelGridArgs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridArgs)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.tableLayoutPanel3Rows.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Controls.Add(this.tableLayoutPanel3Rows);
            this.panelMain.Controls.Add(this.panelGridArgs);
            this.panelMain.Controls.Add(this.listAlgos);
            this.panelMain.Location = new System.Drawing.Point(4, 65);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1110, 863);
            this.panelMain.TabIndex = 0;
            // 
            // panelGridArgs
            // 
            this.panelGridArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGridArgs.Controls.Add(this.gridArgs);
            this.panelGridArgs.Location = new System.Drawing.Point(430, 6);
            this.panelGridArgs.Name = "panelGridArgs";
            this.panelGridArgs.Size = new System.Drawing.Size(653, 256);
            this.panelGridArgs.TabIndex = 2;
            // 
            // gridArgs
            // 
            this.gridArgs.AllowUserToAddRows = false;
            this.gridArgs.AllowUserToDeleteRows = false;
            this.gridArgs.AllowUserToResizeRows = false;
            this.gridArgs.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridArgs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.gridArgs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridArgs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ArgNameColumn,
            this.ArgValueColumn});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridArgs.DefaultCellStyle = dataGridViewCellStyle8;
            this.gridArgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridArgs.Location = new System.Drawing.Point(0, 0);
            this.gridArgs.MultiSelect = false;
            this.gridArgs.Name = "gridArgs";
            this.gridArgs.RowHeadersVisible = false;
            this.gridArgs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridArgs.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.gridArgs.RowTemplate.Height = 24;
            this.gridArgs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridArgs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridArgs.ShowEditingIcon = false;
            this.gridArgs.Size = new System.Drawing.Size(653, 256);
            this.gridArgs.TabIndex = 1;
            // 
            // listAlgos
            // 
            this.listAlgos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listAlgos.FormattingEnabled = true;
            this.listAlgos.ItemHeight = 28;
            this.listAlgos.Items.AddRange(new object[] {
            "Kraken MACD Crossover",
            "Binance MACD Crossover",
            "Two-leg Arbitrage",
            "Three-leg Arbitrage",
            "Three-leg X-Exchange Arbitrage",
            "Modified Iceberg (standard)",
            "Modified Iceberg (VWAP)"});
            this.listAlgos.Location = new System.Drawing.Point(25, 6);
            this.listAlgos.Name = "listAlgos";
            this.listAlgos.Size = new System.Drawing.Size(350, 256);
            this.listAlgos.TabIndex = 0;
            this.listAlgos.SelectedIndexChanged += new System.EventHandler(this.listAlgos_SelectedIndexChanged);
            // 
            // rdoMain
            // 
            this.rdoMain.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoMain.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoMain.Location = new System.Drawing.Point(22, 7);
            this.rdoMain.Name = "rdoMain";
            this.rdoMain.Size = new System.Drawing.Size(125, 45);
            this.rdoMain.TabIndex = 1;
            this.rdoMain.TabStop = true;
            this.rdoMain.Text = "Main";
            this.rdoMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoMain.UseVisualStyleBackColor = true;
            this.rdoMain.Visible = false;
            this.rdoMain.CheckedChanged += new System.EventHandler(this.rdoMain_CheckedChanged);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.Controls.Add(this.btnLaunch);
            this.panelButtons.Controls.Add(this.rdoMain);
            this.panelButtons.Location = new System.Drawing.Point(4, 5);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1110, 56);
            this.panelButtons.TabIndex = 2;
            // 
            // btnLaunch
            // 
            this.btnLaunch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLaunch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLaunch.Location = new System.Drawing.Point(958, 7);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(125, 45);
            this.btnLaunch.TabIndex = 1;
            this.btnLaunch.Text = "Launch";
            this.btnLaunch.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3Rows
            // 
            this.tableLayoutPanel3Rows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3Rows.ColumnCount = 1;
            this.tableLayoutPanel3Rows.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3Rows.Controls.Add(this.panelCrypto1, 0, 0);
            this.tableLayoutPanel3Rows.Controls.Add(this.panelCrypto2, 0, 1);
            this.tableLayoutPanel3Rows.Controls.Add(this.panelCrypto3, 0, 2);
            this.tableLayoutPanel3Rows.Location = new System.Drawing.Point(0, 283);
            this.tableLayoutPanel3Rows.Name = "tableLayoutPanel3Rows";
            this.tableLayoutPanel3Rows.RowCount = 3;
            this.tableLayoutPanel3Rows.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3Rows.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3Rows.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3Rows.Size = new System.Drawing.Size(1110, 600);
            this.tableLayoutPanel3Rows.TabIndex = 3;
            // 
            // panelCrypto2
            // 
            this.panelCrypto2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrypto2.Location = new System.Drawing.Point(3, 202);
            this.panelCrypto2.Name = "panelCrypto2";
            this.panelCrypto2.Size = new System.Drawing.Size(1104, 193);
            this.panelCrypto2.TabIndex = 0;
            // 
            // panelCrypto3
            // 
            this.panelCrypto3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrypto3.Location = new System.Drawing.Point(3, 401);
            this.panelCrypto3.Name = "panelCrypto3";
            this.panelCrypto3.Size = new System.Drawing.Size(1104, 196);
            this.panelCrypto3.TabIndex = 1;
            // 
            // panelCrypto1
            // 
            this.panelCrypto1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrypto1.Location = new System.Drawing.Point(3, 3);
            this.panelCrypto1.Name = "panelCrypto1";
            this.panelCrypto1.Size = new System.Drawing.Size(1104, 193);
            this.panelCrypto1.TabIndex = 0;
            // 
            // ArgNameColumn
            // 
            this.ArgNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ArgNameColumn.HeaderText = "Setting";
            this.ArgNameColumn.Name = "ArgNameColumn";
            this.ArgNameColumn.ReadOnly = true;
            this.ArgNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // ArgValueColumn
            // 
            this.ArgValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ArgValueColumn.FillWeight = 60F;
            this.ArgValueColumn.HeaderText = "Value";
            this.ArgValueColumn.Name = "ArgValueColumn";
            this.ArgValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // CryptoAlgoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1120, 956);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CryptoAlgoForm";
            this.Text = "Automated Trading Algorithms";
            this.panelMain.ResumeLayout(false);
            this.panelGridArgs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridArgs)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.tableLayoutPanel3Rows.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.RadioButton rdoMain;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnLaunch;
        private System.Windows.Forms.ListBox listAlgos;
        private System.Windows.Forms.Panel panelGridArgs;
        private System.Windows.Forms.DataGridView gridArgs;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3Rows;
        private System.Windows.Forms.Panel panelCrypto1;
        private System.Windows.Forms.Panel panelCrypto2;
        private System.Windows.Forms.Panel panelCrypto3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArgNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArgValueColumn;
    }
}