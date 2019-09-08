namespace PrimeTrader
{
    partial class BrowserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslblLeft = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblMiddle = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelWeb = new System.Windows.Forms.Panel();
            this.web = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnToExcelRange = new System.Windows.Forms.ToolStripButton();
            this.tsbtnToExcelAll = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelWeb.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblLeft,
            this.tslblMiddle,
            this.tslblRight});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1030);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1381, 32);
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
            this.tslblLeft.Size = new System.Drawing.Size(250, 27);
            // 
            // tslblMiddle
            // 
            this.tslblMiddle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tslblMiddle.Name = "tslblMiddle";
            this.tslblMiddle.Size = new System.Drawing.Size(916, 27);
            this.tslblMiddle.Spring = true;
            // 
            // tslblRight
            // 
            this.tslblRight.AutoSize = false;
            this.tslblRight.BackColor = System.Drawing.Color.Gainsboro;
            this.tslblRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tslblRight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tslblRight.Name = "tslblRight";
            this.tslblRight.Size = new System.Drawing.Size(200, 27);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelWeb);
            this.panelMain.Controls.Add(this.toolStrip1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1381, 1030);
            this.panelMain.TabIndex = 1;
            // 
            // panelWeb
            // 
            this.panelWeb.Controls.Add(this.web);
            this.panelWeb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWeb.Location = new System.Drawing.Point(0, 61);
            this.panelWeb.Name = "panelWeb";
            this.panelWeb.Size = new System.Drawing.Size(1381, 969);
            this.panelWeb.TabIndex = 4;
            // 
            // web
            // 
            this.web.Dock = System.Windows.Forms.DockStyle.Fill;
            this.web.Location = new System.Drawing.Point(0, 0);
            this.web.MinimumSize = new System.Drawing.Size(20, 20);
            this.web.Name = "web";
            this.web.Size = new System.Drawing.Size(1381, 969);
            this.web.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnToExcelRange,
            this.tsbtnToExcelAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1381, 61);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnToExcelRange
            // 
            this.tsbtnToExcelRange.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnToExcelRange.Image")));
            this.tsbtnToExcelRange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnToExcelRange.Name = "tsbtnToExcelRange";
            this.tsbtnToExcelRange.Size = new System.Drawing.Size(87, 58);
            this.tsbtnToExcelRange.Text = "To Excel";
            // 
            // tsbtnToExcelAll
            // 
            this.tsbtnToExcelAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnToExcelAll.Image")));
            this.tsbtnToExcelAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnToExcelAll.Name = "tsbtnToExcelAll";
            this.tsbtnToExcelAll.Size = new System.Drawing.Size(119, 58);
            this.tsbtnToExcelAll.Text = "To Excel (All)";
            // 
            // BrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 1062);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.statusStrip1);
            this.Name = "BrowserForm";
            this.Text = "Chart Browser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrowserForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelWeb.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panelWeb;
        private System.Windows.Forms.WebBrowser web;
    }
}