namespace OptionChainSocket
{
    partial class OptionChainSocketForm
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
            this.txtRequestID = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.lstCalls = new System.Windows.Forms.ListBox();
            this.lstPuts = new System.Windows.Forms.ListBox();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnCriteria = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtRequestID
            // 
            this.txtRequestID.Location = new System.Drawing.Point(70, 31);
            this.txtRequestID.Name = "txtRequestID";
            this.txtRequestID.Size = new System.Drawing.Size(98, 20);
            this.txtRequestID.TabIndex = 26;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(3, 34);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(64, 13);
            this.Label4.TabIndex = 25;
            this.Label4.Text = "Request ID:";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(3, 55);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(32, 13);
            this.Label3.TabIndex = 24;
            this.Label3.Text = "Calls:";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(137, 55);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(31, 13);
            this.Label2.TabIndex = 23;
            this.Label2.Text = "Puts:";
            // 
            // lstCalls
            // 
            this.lstCalls.FormattingEnabled = true;
            this.lstCalls.Location = new System.Drawing.Point(6, 72);
            this.lstCalls.Name = "lstCalls";
            this.lstCalls.Size = new System.Drawing.Size(121, 342);
            this.lstCalls.TabIndex = 22;
            // 
            // lstPuts
            // 
            this.lstPuts.FormattingEnabled = true;
            this.lstPuts.Location = new System.Drawing.Point(136, 72);
            this.lstPuts.Name = "lstPuts";
            this.lstPuts.Size = new System.Drawing.Size(121, 342);
            this.lstPuts.TabIndex = 21;
            // 
            // txtSymbol
            // 
            this.txtSymbol.Location = new System.Drawing.Point(70, 5);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(98, 20);
            this.txtSymbol.TabIndex = 20;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(3, 8);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(44, 13);
            this.Label1.TabIndex = 19;
            this.Label1.Text = "Symbol:";
            // 
            // btnCriteria
            // 
            this.btnCriteria.Location = new System.Drawing.Point(174, 5);
            this.btnCriteria.Name = "btnCriteria";
            this.btnCriteria.Size = new System.Drawing.Size(86, 46);
            this.btnCriteria.TabIndex = 18;
            this.btnCriteria.Text = "Criteria";
            this.btnCriteria.UseVisualStyleBackColor = true;
            this.btnCriteria.Click += new System.EventHandler(this.btnCriteria_Click);
            // 
            // OptionChainSocketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 426);
            this.Controls.Add(this.txtRequestID);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.lstCalls);
            this.Controls.Add(this.lstPuts);
            this.Controls.Add(this.txtSymbol);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.btnCriteria);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "OptionChainSocketForm";
            this.Text = "C# Option Chain Socket";
            this.Load += new System.EventHandler(this.OptionChainSocketForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox txtRequestID;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.ListBox lstCalls;
        internal System.Windows.Forms.ListBox lstPuts;
        internal System.Windows.Forms.TextBox txtSymbol;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button btnCriteria;
    }
}

