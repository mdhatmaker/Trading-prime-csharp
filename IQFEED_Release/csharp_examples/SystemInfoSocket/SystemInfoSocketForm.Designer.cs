namespace SystemInfoSocket
{
    partial class SystemInfoSocketForm
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
            this.btnNAICSCodes = new System.Windows.Forms.Button();
            this.btnSICCodes = new System.Windows.Forms.Button();
            this.btnSecurityTypes = new System.Windows.Forms.Button();
            this.btnListedMarkets = new System.Windows.Forms.Button();
            this.lstData = new System.Windows.Forms.ListBox();
            this.btnTradeConditions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNAICSCodes
            // 
            this.btnNAICSCodes.Location = new System.Drawing.Point(169, 33);
            this.btnNAICSCodes.Name = "btnNAICSCodes";
            this.btnNAICSCodes.Size = new System.Drawing.Size(151, 23);
            this.btnNAICSCodes.TabIndex = 4;
            this.btnNAICSCodes.Text = "Get NAICS Code List";
            this.btnNAICSCodes.UseVisualStyleBackColor = true;
            this.btnNAICSCodes.Click += new System.EventHandler(this.btnNAICSCodes_Click);
            // 
            // btnSICCodes
            // 
            this.btnSICCodes.Location = new System.Drawing.Point(12, 33);
            this.btnSICCodes.Name = "btnSICCodes";
            this.btnSICCodes.Size = new System.Drawing.Size(151, 23);
            this.btnSICCodes.TabIndex = 3;
            this.btnSICCodes.Text = "Get SIC Code List";
            this.btnSICCodes.UseVisualStyleBackColor = true;
            this.btnSICCodes.Click += new System.EventHandler(this.btnSICCodes_Click);
            // 
            // btnSecurityTypes
            // 
            this.btnSecurityTypes.Location = new System.Drawing.Point(169, 7);
            this.btnSecurityTypes.Name = "btnSecurityTypes";
            this.btnSecurityTypes.Size = new System.Drawing.Size(151, 24);
            this.btnSecurityTypes.TabIndex = 1;
            this.btnSecurityTypes.Text = "Request Security Types";
            this.btnSecurityTypes.UseVisualStyleBackColor = true;
            this.btnSecurityTypes.Click += new System.EventHandler(this.btnSecurityTypes_Click);
            // 
            // btnListedMarkets
            // 
            this.btnListedMarkets.Location = new System.Drawing.Point(12, 7);
            this.btnListedMarkets.Name = "btnListedMarkets";
            this.btnListedMarkets.Size = new System.Drawing.Size(151, 24);
            this.btnListedMarkets.TabIndex = 0;
            this.btnListedMarkets.Text = "Request Listed Markets";
            this.btnListedMarkets.UseVisualStyleBackColor = true;
            this.btnListedMarkets.Click += new System.EventHandler(this.btnListedMarkets_Click);
            // 
            // lstData
            // 
            this.lstData.FormattingEnabled = true;
            this.lstData.Location = new System.Drawing.Point(12, 62);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(498, 381);
            this.lstData.TabIndex = 6;
            // 
            // btnTradeConditions
            // 
            this.btnTradeConditions.Location = new System.Drawing.Point(326, 8);
            this.btnTradeConditions.Name = "btnTradeConditions";
            this.btnTradeConditions.Size = new System.Drawing.Size(186, 23);
            this.btnTradeConditions.TabIndex = 5;
            this.btnTradeConditions.Text = "Get Trade Conditions";
            this.btnTradeConditions.UseVisualStyleBackColor = true;
            this.btnTradeConditions.Click += new System.EventHandler(this.btnTradeConditions_Click);
            // 
            // SystemInfoSocketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 455);
            this.Controls.Add(this.btnTradeConditions);
            this.Controls.Add(this.lstData);
            this.Controls.Add(this.btnNAICSCodes);
            this.Controls.Add(this.btnSICCodes);
            this.Controls.Add(this.btnSecurityTypes);
            this.Controls.Add(this.btnListedMarkets);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SystemInfoSocketForm";
            this.Text = "C# System Info Socket";
            this.Load += new System.EventHandler(this.SystemInfoSocketForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnNAICSCodes;
        internal System.Windows.Forms.Button btnSICCodes;
        internal System.Windows.Forms.Button btnSecurityTypes;
        internal System.Windows.Forms.Button btnListedMarkets;
        private System.Windows.Forms.ListBox lstData;
        private System.Windows.Forms.Button btnTradeConditions;
    }
}

