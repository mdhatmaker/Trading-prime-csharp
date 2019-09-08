namespace CryptoWindows
{
    partial class CryptoWindowsForm
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
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.panelBalances = new System.Windows.Forms.Panel();
            this.gridBalances = new System.Windows.Forms.DataGridView();
            this.lblBtcTotal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUsdTotal = new System.Windows.Forms.Label();
            this.timerBalances = new System.Windows.Forms.Timer(this.components);
            this.panelOrders = new System.Windows.Forms.Panel();
            this.gridOrders = new System.Windows.Forms.DataGridView();
            this.lblOrderCount = new System.Windows.Forms.Label();
            this.timerOrders = new System.Windows.Forms.Timer(this.components);
            this.panelBalances.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBalances)).BeginInit();
            this.panelOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbOutput
            // 
            this.rtbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbOutput.BackColor = System.Drawing.Color.Black;
            this.rtbOutput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbOutput.ForeColor = System.Drawing.Color.YellowGreen;
            this.rtbOutput.Location = new System.Drawing.Point(12, 872);
            this.rtbOutput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.Size = new System.Drawing.Size(1296, 118);
            this.rtbOutput.TabIndex = 0;
            this.rtbOutput.Text = "";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(12, 11);
            this.btnTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(66, 22);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // panelBalances
            // 
            this.panelBalances.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBalances.Controls.Add(this.gridBalances);
            this.panelBalances.Location = new System.Drawing.Point(977, 48);
            this.panelBalances.Name = "panelBalances";
            this.panelBalances.Size = new System.Drawing.Size(331, 819);
            this.panelBalances.TabIndex = 2;
            // 
            // gridBalances
            // 
            this.gridBalances.AllowUserToAddRows = false;
            this.gridBalances.AllowUserToDeleteRows = false;
            this.gridBalances.AllowUserToResizeRows = false;
            this.gridBalances.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridBalances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBalances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBalances.Location = new System.Drawing.Point(0, 0);
            this.gridBalances.Name = "gridBalances";
            this.gridBalances.RowHeadersVisible = false;
            this.gridBalances.RowTemplate.Height = 24;
            this.gridBalances.Size = new System.Drawing.Size(331, 819);
            this.gridBalances.TabIndex = 0;
            // 
            // lblBtcTotal
            // 
            this.lblBtcTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBtcTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBtcTotal.Location = new System.Drawing.Point(1031, 13);
            this.lblBtcTotal.Name = "lblBtcTotal";
            this.lblBtcTotal.Size = new System.Drawing.Size(128, 23);
            this.lblBtcTotal.TabIndex = 3;
            this.lblBtcTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(961, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "BTC:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblUsdTotal
            // 
            this.lblUsdTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsdTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsdTotal.Location = new System.Drawing.Point(1177, 13);
            this.lblUsdTotal.Name = "lblUsdTotal";
            this.lblUsdTotal.Size = new System.Drawing.Size(131, 23);
            this.lblUsdTotal.TabIndex = 5;
            this.lblUsdTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timerBalances
            // 
            this.timerBalances.Enabled = true;
            this.timerBalances.Interval = 61000;
            this.timerBalances.Tick += new System.EventHandler(this.timerBalances_Tick);
            // 
            // panelOrders
            // 
            this.panelOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelOrders.Controls.Add(this.gridOrders);
            this.panelOrders.Location = new System.Drawing.Point(12, 48);
            this.panelOrders.Name = "panelOrders";
            this.panelOrders.Size = new System.Drawing.Size(948, 819);
            this.panelOrders.TabIndex = 6;
            // 
            // gridOrders
            // 
            this.gridOrders.AllowUserToAddRows = false;
            this.gridOrders.AllowUserToDeleteRows = false;
            this.gridOrders.AllowUserToResizeRows = false;
            this.gridOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOrders.Location = new System.Drawing.Point(0, 0);
            this.gridOrders.Name = "gridOrders";
            this.gridOrders.RowHeadersVisible = false;
            this.gridOrders.RowTemplate.Height = 24;
            this.gridOrders.Size = new System.Drawing.Size(948, 819);
            this.gridOrders.TabIndex = 1;
            // 
            // lblOrderCount
            // 
            this.lblOrderCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOrderCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderCount.Location = new System.Drawing.Point(645, 17);
            this.lblOrderCount.Name = "lblOrderCount";
            this.lblOrderCount.Size = new System.Drawing.Size(252, 23);
            this.lblOrderCount.TabIndex = 7;
            this.lblOrderCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timerOrders
            // 
            this.timerOrders.Enabled = true;
            this.timerOrders.Interval = 51000;
            this.timerOrders.Tick += new System.EventHandler(this.timerOrders_Tick);
            // 
            // CryptoWindowsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 996);
            this.Controls.Add(this.lblOrderCount);
            this.Controls.Add(this.panelOrders);
            this.Controls.Add(this.lblUsdTotal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblBtcTotal);
            this.Controls.Add(this.panelBalances);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.rtbOutput);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CryptoWindowsForm";
            this.Text = "Crypto Windows";
            this.Load += new System.EventHandler(this.CryptoWindowsForm_Load);
            this.panelBalances.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBalances)).EndInit();
            this.panelOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridOrders)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Panel panelBalances;
        private System.Windows.Forms.DataGridView gridBalances;
        private System.Windows.Forms.Label lblBtcTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUsdTotal;
        private System.Windows.Forms.Timer timerBalances;
        private System.Windows.Forms.Panel panelOrders;
        private System.Windows.Forms.DataGridView gridOrders;
        private System.Windows.Forms.Label lblOrderCount;
        private System.Windows.Forms.Timer timerOrders;
    }
}

