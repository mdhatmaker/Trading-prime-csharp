namespace GuiTools
{
    partial class MessagesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessagesForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rtbConsoleOutput = new System.Windows.Forms.RichTextBox();
            this.rtbDebugOutput = new System.Windows.Forms.RichTextBox();
            this.rtbErrorOutput = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslblStatusLeft = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblStatusRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.rtbConsoleOutput, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rtbDebugOutput, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rtbErrorOutput, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(942, 700);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // rtbConsoleOutput
            // 
            this.rtbConsoleOutput.BackColor = System.Drawing.Color.Black;
            this.rtbConsoleOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbConsoleOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConsoleOutput.ForeColor = System.Drawing.Color.White;
            this.rtbConsoleOutput.HideSelection = false;
            this.rtbConsoleOutput.Location = new System.Drawing.Point(3, 3);
            this.rtbConsoleOutput.Name = "rtbConsoleOutput";
            this.rtbConsoleOutput.Size = new System.Drawing.Size(936, 274);
            this.rtbConsoleOutput.TabIndex = 7;
            this.rtbConsoleOutput.Text = "";
            // 
            // rtbDebugOutput
            // 
            this.rtbDebugOutput.BackColor = System.Drawing.Color.Black;
            this.rtbDebugOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDebugOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDebugOutput.ForeColor = System.Drawing.Color.LawnGreen;
            this.rtbDebugOutput.HideSelection = false;
            this.rtbDebugOutput.Location = new System.Drawing.Point(3, 283);
            this.rtbDebugOutput.Name = "rtbDebugOutput";
            this.rtbDebugOutput.Size = new System.Drawing.Size(936, 204);
            this.rtbDebugOutput.TabIndex = 6;
            this.rtbDebugOutput.Text = "";
            // 
            // rtbErrorOutput
            // 
            this.rtbErrorOutput.BackColor = System.Drawing.Color.Black;
            this.rtbErrorOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbErrorOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbErrorOutput.ForeColor = System.Drawing.Color.Red;
            this.rtbErrorOutput.HideSelection = false;
            this.rtbErrorOutput.Location = new System.Drawing.Point(3, 493);
            this.rtbErrorOutput.Name = "rtbErrorOutput";
            this.rtbErrorOutput.Size = new System.Drawing.Size(936, 204);
            this.rtbErrorOutput.TabIndex = 4;
            this.rtbErrorOutput.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblStatusLeft,
            this.tslblStatusRight});
            this.statusStrip1.Location = new System.Drawing.Point(0, 696);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(947, 30);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslblStatusLeft
            // 
            this.tslblStatusLeft.Name = "tslblStatusLeft";
            this.tslblStatusLeft.Size = new System.Drawing.Size(755, 25);
            this.tslblStatusLeft.Spring = true;
            this.tslblStatusLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tslblStatusRight
            // 
            this.tslblStatusRight.AutoSize = false;
            this.tslblStatusRight.BackColor = System.Drawing.Color.Silver;
            this.tslblStatusRight.Name = "tslblStatusRight";
            this.tslblStatusRight.Size = new System.Drawing.Size(179, 25);
            // 
            // MessagesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 726);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MessagesForm";
            this.Text = "Messages";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MessagesForm_FormClosing);
            this.Load += new System.EventHandler(this.MessagesForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox rtbConsoleOutput;
        private System.Windows.Forms.RichTextBox rtbDebugOutput;
        private System.Windows.Forms.RichTextBox rtbErrorOutput;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslblStatusLeft;
        private System.Windows.Forms.ToolStripStatusLabel tslblStatusRight;
    }
}