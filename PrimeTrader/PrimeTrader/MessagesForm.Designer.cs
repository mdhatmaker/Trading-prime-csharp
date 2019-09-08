namespace PrimeTrader
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rtbConsoleOutput = new System.Windows.Forms.RichTextBox();
            this.rtbDebugOutput = new System.Windows.Forms.RichTextBox();
            this.rtbErrorOutput = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(811, 566);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // rtbConsoleOutput
            // 
            this.rtbConsoleOutput.BackColor = System.Drawing.Color.Black;
            this.rtbConsoleOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbConsoleOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConsoleOutput.ForeColor = System.Drawing.Color.White;
            this.rtbConsoleOutput.HideSelection = false;
            this.rtbConsoleOutput.Location = new System.Drawing.Point(3, 3);
            this.rtbConsoleOutput.Name = "rtbConsoleOutput";
            this.rtbConsoleOutput.Size = new System.Drawing.Size(805, 248);
            this.rtbConsoleOutput.TabIndex = 7;
            this.rtbConsoleOutput.Text = "";
            // 
            // rtbDebugOutput
            // 
            this.rtbDebugOutput.BackColor = System.Drawing.Color.Black;
            this.rtbDebugOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDebugOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDebugOutput.ForeColor = System.Drawing.Color.LawnGreen;
            this.rtbDebugOutput.HideSelection = false;
            this.rtbDebugOutput.Location = new System.Drawing.Point(3, 257);
            this.rtbDebugOutput.Name = "rtbDebugOutput";
            this.rtbDebugOutput.Size = new System.Drawing.Size(805, 192);
            this.rtbDebugOutput.TabIndex = 6;
            this.rtbDebugOutput.Text = "";
            // 
            // rtbErrorOutput
            // 
            this.rtbErrorOutput.BackColor = System.Drawing.Color.Black;
            this.rtbErrorOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbErrorOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbErrorOutput.ForeColor = System.Drawing.Color.Red;
            this.rtbErrorOutput.HideSelection = false;
            this.rtbErrorOutput.Location = new System.Drawing.Point(3, 455);
            this.rtbErrorOutput.Name = "rtbErrorOutput";
            this.rtbErrorOutput.Size = new System.Drawing.Size(805, 108);
            this.rtbErrorOutput.TabIndex = 4;
            this.rtbErrorOutput.Text = "";
            // 
            // MessagesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 569);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MessagesForm";
            this.Text = "Messages";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MessagesForm_FormClosing);
            this.Load += new System.EventHandler(this.MessagesForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox rtbConsoleOutput;
        private System.Windows.Forms.RichTextBox rtbDebugOutput;
        private System.Windows.Forms.RichTextBox rtbErrorOutput;
    }
}