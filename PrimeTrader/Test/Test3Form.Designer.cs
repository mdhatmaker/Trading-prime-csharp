namespace ZeroSumAPI_Test
{
    partial class Test3Form
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.gboInstrumentInfo = new System.Windows.Forms.GroupBox();
            this.cboProductType = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblProductType = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblExchange = new System.Windows.Forms.Label();
            this.txtContract = new System.Windows.Forms.TextBox();
            this.lblContract = new System.Windows.Forms.Label();
            this.txtExchange = new System.Windows.Forms.TextBox();
            this.gboInstrumentInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(62, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(197, 64);
            this.button1.TabIndex = 0;
            this.button1.Text = "Create Instruments";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(62, 144);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(197, 64);
            this.button2.TabIndex = 1;
            this.button2.Text = "Submit Orders";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // gboInstrumentInfo
            // 
            this.gboInstrumentInfo.Controls.Add(this.cboProductType);
            this.gboInstrumentInfo.Controls.Add(this.btnConnect);
            this.gboInstrumentInfo.Controls.Add(this.lblProductType);
            this.gboInstrumentInfo.Controls.Add(this.txtProduct);
            this.gboInstrumentInfo.Controls.Add(this.lblProduct);
            this.gboInstrumentInfo.Controls.Add(this.lblExchange);
            this.gboInstrumentInfo.Controls.Add(this.txtContract);
            this.gboInstrumentInfo.Controls.Add(this.lblContract);
            this.gboInstrumentInfo.Controls.Add(this.txtExchange);
            this.gboInstrumentInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gboInstrumentInfo.Location = new System.Drawing.Point(344, 13);
            this.gboInstrumentInfo.Margin = new System.Windows.Forms.Padding(4);
            this.gboInstrumentInfo.Name = "gboInstrumentInfo";
            this.gboInstrumentInfo.Padding = new System.Windows.Forms.Padding(4);
            this.gboInstrumentInfo.Size = new System.Drawing.Size(288, 220);
            this.gboInstrumentInfo.TabIndex = 80;
            this.gboInstrumentInfo.TabStop = false;
            this.gboInstrumentInfo.Text = "Instrument Information";
            // 
            // cboProductType
            // 
            this.cboProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProductType.FormattingEnabled = true;
            this.cboProductType.Location = new System.Drawing.Point(128, 87);
            this.cboProductType.Margin = new System.Windows.Forms.Padding(4);
            this.cboProductType.Name = "cboProductType";
            this.cboProductType.Size = new System.Drawing.Size(151, 25);
            this.cboProductType.TabIndex = 42;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.SystemColors.Control;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnConnect.Location = new System.Drawing.Point(180, 169);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 28);
            this.btnConnect.TabIndex = 41;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblProductType
            // 
            this.lblProductType.Location = new System.Drawing.Point(11, 89);
            this.lblProductType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProductType.Name = "lblProductType";
            this.lblProductType.Size = new System.Drawing.Size(107, 20);
            this.lblProductType.TabIndex = 38;
            this.lblProductType.Text = "Product Type:";
            this.lblProductType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(128, 59);
            this.txtProduct.Margin = new System.Windows.Forms.Padding(4);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(151, 22);
            this.txtProduct.TabIndex = 35;
            this.txtProduct.Text = "ES";
            // 
            // lblProduct
            // 
            this.lblProduct.Location = new System.Drawing.Point(53, 59);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(64, 20);
            this.lblProduct.TabIndex = 36;
            this.lblProduct.Text = "Product:";
            this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExchange
            // 
            this.lblExchange.Location = new System.Drawing.Point(32, 30);
            this.lblExchange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExchange.Name = "lblExchange";
            this.lblExchange.Size = new System.Drawing.Size(85, 20);
            this.lblExchange.TabIndex = 34;
            this.lblExchange.Text = "Exchange:";
            this.lblExchange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtContract
            // 
            this.txtContract.Location = new System.Drawing.Point(128, 118);
            this.txtContract.Margin = new System.Windows.Forms.Padding(4);
            this.txtContract.Name = "txtContract";
            this.txtContract.Size = new System.Drawing.Size(151, 22);
            this.txtContract.TabIndex = 39;
            this.txtContract.Text = "Dec17";
            // 
            // lblContract
            // 
            this.lblContract.Location = new System.Drawing.Point(43, 118);
            this.lblContract.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContract.Name = "lblContract";
            this.lblContract.Size = new System.Drawing.Size(75, 20);
            this.lblContract.TabIndex = 40;
            this.lblContract.Text = "Contract:";
            this.lblContract.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtExchange
            // 
            this.txtExchange.Location = new System.Drawing.Point(128, 30);
            this.txtExchange.Margin = new System.Windows.Forms.Padding(4);
            this.txtExchange.Name = "txtExchange";
            this.txtExchange.Size = new System.Drawing.Size(151, 22);
            this.txtExchange.TabIndex = 33;
            this.txtExchange.Text = "CME";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 383);
            this.Controls.Add(this.gboInstrumentInfo);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestForm_FormClosing);
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.TestForm_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.TestForm_DragOver);
            this.gboInstrumentInfo.ResumeLayout(false);
            this.gboInstrumentInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox gboInstrumentInfo;
        private System.Windows.Forms.ComboBox cboProductType;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblProductType;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblExchange;
        private System.Windows.Forms.TextBox txtContract;
        private System.Windows.Forms.Label lblContract;
        private System.Windows.Forms.TextBox txtExchange;
    }
}