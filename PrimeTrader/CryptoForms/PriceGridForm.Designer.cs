namespace CryptoForms
{
    partial class PriceGridForm
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
            this.panelGrid = new System.Windows.Forms.Panel();
            this.gridPrices = new System.Windows.Forms.DataGridView();
            this.tableBidAsk = new System.Windows.Forms.TableLayoutPanel();
            this.lblBidSize = new System.Windows.Forms.Label();
            this.lblAsk = new System.Windows.Forms.Label();
            this.lblBid = new System.Windows.Forms.Label();
            this.lblAskSize = new System.Windows.Forms.Label();
            this.lblSymbol = new System.Windows.Forms.Label();
            this.panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPrices)).BeginInit();
            this.tableBidAsk.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGrid
            // 
            this.panelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGrid.Controls.Add(this.gridPrices);
            this.panelGrid.Location = new System.Drawing.Point(4, 40);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(651, 478);
            this.panelGrid.TabIndex = 0;
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
            this.gridPrices.Size = new System.Drawing.Size(651, 478);
            this.gridPrices.TabIndex = 0;
            // 
            // tableBidAsk
            // 
            this.tableBidAsk.ColumnCount = 5;
            this.tableBidAsk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableBidAsk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableBidAsk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableBidAsk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableBidAsk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableBidAsk.Controls.Add(this.lblAskSize, 4, 0);
            this.tableBidAsk.Controls.Add(this.lblAsk, 3, 0);
            this.tableBidAsk.Controls.Add(this.lblBidSize, 1, 0);
            this.tableBidAsk.Controls.Add(this.lblSymbol, 0, 0);
            this.tableBidAsk.Controls.Add(this.lblBid, 2, 0);
            this.tableBidAsk.Location = new System.Drawing.Point(122, -2);
            this.tableBidAsk.Name = "tableBidAsk";
            this.tableBidAsk.RowCount = 1;
            this.tableBidAsk.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBidAsk.Size = new System.Drawing.Size(341, 36);
            this.tableBidAsk.TabIndex = 1;
            // 
            // lblBidSize
            // 
            this.lblBidSize.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lblBidSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBidSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBidSize.Location = new System.Drawing.Point(102, 0);
            this.lblBidSize.Margin = new System.Windows.Forms.Padding(0);
            this.lblBidSize.Name = "lblBidSize";
            this.lblBidSize.Size = new System.Drawing.Size(51, 36);
            this.lblBidSize.TabIndex = 3;
            this.lblBidSize.Text = "0";
            this.lblBidSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAsk
            // 
            this.lblAsk.BackColor = System.Drawing.Color.LightCoral;
            this.lblAsk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAsk.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsk.Location = new System.Drawing.Point(221, 0);
            this.lblAsk.Margin = new System.Windows.Forms.Padding(0);
            this.lblAsk.Name = "lblAsk";
            this.lblAsk.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblAsk.Size = new System.Drawing.Size(68, 36);
            this.lblAsk.TabIndex = 4;
            this.lblAsk.Text = "0";
            this.lblAsk.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBid
            // 
            this.lblBid.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lblBid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBid.Location = new System.Drawing.Point(153, 0);
            this.lblBid.Margin = new System.Windows.Forms.Padding(0);
            this.lblBid.Name = "lblBid";
            this.lblBid.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lblBid.Size = new System.Drawing.Size(68, 36);
            this.lblBid.TabIndex = 5;
            this.lblBid.Text = "0";
            this.lblBid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAskSize
            // 
            this.lblAskSize.BackColor = System.Drawing.Color.LightCoral;
            this.lblAskSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAskSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAskSize.Location = new System.Drawing.Point(289, 0);
            this.lblAskSize.Margin = new System.Windows.Forms.Padding(0);
            this.lblAskSize.Name = "lblAskSize";
            this.lblAskSize.Size = new System.Drawing.Size(52, 36);
            this.lblAskSize.TabIndex = 6;
            this.lblAskSize.Text = "0";
            this.lblAskSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSymbol
            // 
            this.lblSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbol.Location = new System.Drawing.Point(3, 0);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(96, 36);
            this.lblSymbol.TabIndex = 7;
            this.lblSymbol.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PriceGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 521);
            this.Controls.Add(this.tableBidAsk);
            this.Controls.Add(this.panelGrid);
            this.Name = "PriceGridForm";
            this.Text = "Price Compare Grid";
            this.panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPrices)).EndInit();
            this.tableBidAsk.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.DataGridView gridPrices;
        private System.Windows.Forms.TableLayoutPanel tableBidAsk;
        private System.Windows.Forms.Label lblAskSize;
        private System.Windows.Forms.Label lblAsk;
        private System.Windows.Forms.Label lblBidSize;
        private System.Windows.Forms.Label lblSymbol;
        private System.Windows.Forms.Label lblBid;
    }
}