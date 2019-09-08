namespace CryptoForms
{
    partial class CryptoInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CryptoInfoForm));
            this.cryptoTimer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusCryptoMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusCryptoRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelCrypto1 = new System.Windows.Forms.Panel();
            this.panelCrypto2 = new System.Windows.Forms.Panel();
            this.panelCrypto3 = new System.Windows.Forms.Panel();
            this.panelCrypto4 = new System.Windows.Forms.Panel();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 1241);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1489, 30);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusCryptoMain
            // 
            this.statusCryptoMain.Name = "statusCryptoMain";
            this.statusCryptoMain.Size = new System.Drawing.Size(1293, 25);
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
            this.tableLayoutPanel1.Controls.Add(this.panelCrypto4, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 1);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1489, 1238);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panelCrypto1
            // 
            this.panelCrypto1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrypto1.Location = new System.Drawing.Point(3, 2);
            this.panelCrypto1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelCrypto1.Name = "panelCrypto1";
            this.panelCrypto1.Size = new System.Drawing.Size(1483, 305);
            this.panelCrypto1.TabIndex = 1;
            // 
            // panelCrypto2
            // 
            this.panelCrypto2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrypto2.Location = new System.Drawing.Point(3, 311);
            this.panelCrypto2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelCrypto2.Name = "panelCrypto2";
            this.panelCrypto2.Size = new System.Drawing.Size(1483, 305);
            this.panelCrypto2.TabIndex = 5;
            // 
            // panelCrypto3
            // 
            this.panelCrypto3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrypto3.Location = new System.Drawing.Point(3, 622);
            this.panelCrypto3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelCrypto3.Name = "panelCrypto3";
            this.panelCrypto3.Size = new System.Drawing.Size(1483, 301);
            this.panelCrypto3.TabIndex = 6;
            // 
            // panelCrypto4
            // 
            this.panelCrypto4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrypto4.Location = new System.Drawing.Point(3, 929);
            this.panelCrypto4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelCrypto4.Name = "panelCrypto4";
            this.panelCrypto4.Size = new System.Drawing.Size(1483, 307);
            this.panelCrypto4.TabIndex = 7;
            // 
            // CryptoInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1489, 1271);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CryptoInfoForm";
            this.Text = "Crypto Market Info";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CryptoPricesForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer cryptoTimer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusCryptoMain;
        private System.Windows.Forms.ToolStripStatusLabel statusCryptoRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelCrypto1;
        private System.Windows.Forms.Panel panelCrypto2;
        private System.Windows.Forms.Panel panelCrypto3;
        private System.Windows.Forms.Panel panelCrypto4;
    }
}

