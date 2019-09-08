namespace MarketDepthSocket
{
    partial class MarketDepthSocketForm
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
            this.lstData = new System.Windows.Forms.ListBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnMMID = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.btnWatch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstData
            // 
            this.lstData.FormattingEnabled = true;
            this.lstData.Location = new System.Drawing.Point(12, 59);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(565, 290);
            this.lstData.TabIndex = 33;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(349, 28);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 31;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(90, 28);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 26;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(268, 28);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 24;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnMMID
            // 
            this.btnMMID.Location = new System.Drawing.Point(171, 28);
            this.btnMMID.Name = "btnMMID";
            this.btnMMID.Size = new System.Drawing.Size(91, 23);
            this.btnMMID.TabIndex = 21;
            this.btnMMID.Text = "Request MMID";
            this.btnMMID.UseVisualStyleBackColor = true;
            this.btnMMID.Click += new System.EventHandler(this.btnMMID_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Symbol / Request Data:";
            // 
            // txtRequest
            // 
            this.txtRequest.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRequest.Location = new System.Drawing.Point(136, 3);
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(441, 20);
            this.txtRequest.TabIndex = 18;
            // 
            // btnWatch
            // 
            this.btnWatch.Location = new System.Drawing.Point(9, 28);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(75, 23);
            this.btnWatch.TabIndex = 17;
            this.btnWatch.Text = "Watch";
            this.btnWatch.UseVisualStyleBackColor = true;
            this.btnWatch.Click += new System.EventHandler(this.btnWatch_Click);
            // 
            // MarketDepthForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 361);
            this.Controls.Add(this.lstData);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnMMID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRequest);
            this.Controls.Add(this.btnWatch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MarketDepthForm";
            this.Text = "C# Market Depth Socket";
            this.Load += new System.EventHandler(this.MarketDepthSocketForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstData;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnMMID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.Button btnWatch;
    }
}

