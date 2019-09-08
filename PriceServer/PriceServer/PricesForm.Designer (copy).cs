namespace PriceServer
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
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridPrices = new System.Windows.Forms.DataGridView();
            this.gridSpreads = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPrices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSpreads)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRequest
            // 
            this.txtRequest.Location = new System.Drawing.Point(28, 8);
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(265, 22);
            this.txtRequest.TabIndex = 0;
            this.txtRequest.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRequest_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(5, 40);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridPrices);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridSpreads);
            this.splitContainer1.Size = new System.Drawing.Size(1146, 1046);
            this.splitContainer1.SplitterDistance = 804;
            this.splitContainer1.TabIndex = 1;
            // 
            // gridPrices
            // 
            this.gridPrices.AllowUserToAddRows = false;
            this.gridPrices.AllowUserToDeleteRows = false;
            this.gridPrices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPrices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPrices.Location = new System.Drawing.Point(0, 0);
            this.gridPrices.Name = "gridPrices";
            this.gridPrices.ReadOnly = true;
            this.gridPrices.RowTemplate.Height = 24;
            this.gridPrices.Size = new System.Drawing.Size(1146, 804);
            this.gridPrices.TabIndex = 0;
            // 
            // gridSpreads
            // 
            this.gridSpreads.AllowUserToAddRows = false;
            this.gridSpreads.AllowUserToDeleteRows = false;
            this.gridSpreads.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSpreads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSpreads.Location = new System.Drawing.Point(0, 0);
            this.gridSpreads.Name = "gridSpreads";
            this.gridSpreads.ReadOnly = true;
            this.gridSpreads.RowTemplate.Height = 24;
            this.gridSpreads.Size = new System.Drawing.Size(1146, 238);
            this.gridSpreads.TabIndex = 0;
            // 
            // PricesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1153, 1088);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.txtRequest);
            this.Name = "PricesForm";
            this.Text = "Prices";
            this.Load += new System.EventHandler(this.PricesForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPrices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSpreads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gridPrices;
        private System.Windows.Forms.DataGridView gridSpreads;
    }
}

