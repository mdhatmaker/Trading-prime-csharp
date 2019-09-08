namespace SymbolLookupSocket
{
    partial class SymbolLookupSocketForm
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
            this.btnLaunch = new System.Windows.Forms.Button();
            this.rbtnNAICS = new System.Windows.Forms.RadioButton();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.rbtnSIC = new System.Windows.Forms.RadioButton();
            this.rbtnSymbol = new System.Windows.Forms.RadioButton();
            this.rbtnDescription = new System.Windows.Forms.RadioButton();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtSearchString = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.lstData = new System.Windows.Forms.ListBox();
            this.txtFilterValue = new System.Windows.Forms.TextBox();
            this.rbtnSecurityType = new System.Windows.Forms.RadioButton();
            this.txtRequestID = new System.Windows.Forms.TextBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.rbtnListedMarket = new System.Windows.Forms.RadioButton();
            this.btnGetData = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.gbSearch.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLaunch
            // 
            this.btnLaunch.Location = new System.Drawing.Point(72, 35);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(129, 29);
            this.btnLaunch.TabIndex = 6;
            this.btnLaunch.Text = "Launch SystemInfo";
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // rbtnNAICS
            // 
            this.rbtnNAICS.AutoSize = true;
            this.rbtnNAICS.Location = new System.Drawing.Point(103, 33);
            this.rbtnNAICS.Name = "rbtnNAICS";
            this.rbtnNAICS.Size = new System.Drawing.Size(57, 17);
            this.rbtnNAICS.TabIndex = 9;
            this.rbtnNAICS.TabStop = true;
            this.rbtnNAICS.Text = "NAICS";
            this.rbtnNAICS.UseVisualStyleBackColor = true;
            this.rbtnNAICS.CheckedChanged += new System.EventHandler(this.rbtnNAICS_CheckedChanged);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.rbtnNAICS);
            this.gbSearch.Controls.Add(this.rbtnSIC);
            this.gbSearch.Controls.Add(this.rbtnSymbol);
            this.gbSearch.Controls.Add(this.rbtnDescription);
            this.gbSearch.Controls.Add(this.Label1);
            this.gbSearch.Controls.Add(this.txtSearchString);
            this.gbSearch.Location = new System.Drawing.Point(10, 6);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(187, 97);
            this.gbSearch.TabIndex = 18;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "FieldToSearch";
            // 
            // rbtnSIC
            // 
            this.rbtnSIC.AutoSize = true;
            this.rbtnSIC.Location = new System.Drawing.Point(9, 33);
            this.rbtnSIC.Name = "rbtnSIC";
            this.rbtnSIC.Size = new System.Drawing.Size(42, 17);
            this.rbtnSIC.TabIndex = 8;
            this.rbtnSIC.TabStop = true;
            this.rbtnSIC.Text = "SIC";
            this.rbtnSIC.UseVisualStyleBackColor = true;
            this.rbtnSIC.CheckedChanged += new System.EventHandler(this.rbtnSIC_CheckedChanged);
            // 
            // rbtnSymbol
            // 
            this.rbtnSymbol.AutoSize = true;
            this.rbtnSymbol.Location = new System.Drawing.Point(9, 12);
            this.rbtnSymbol.Name = "rbtnSymbol";
            this.rbtnSymbol.Size = new System.Drawing.Size(59, 17);
            this.rbtnSymbol.TabIndex = 6;
            this.rbtnSymbol.TabStop = true;
            this.rbtnSymbol.Text = "Symbol";
            this.rbtnSymbol.UseVisualStyleBackColor = true;
            this.rbtnSymbol.CheckedChanged += new System.EventHandler(this.rbtnSymbol_CheckedChanged);
            // 
            // rbtnDescription
            // 
            this.rbtnDescription.AutoSize = true;
            this.rbtnDescription.Location = new System.Drawing.Point(103, 12);
            this.rbtnDescription.Name = "rbtnDescription";
            this.rbtnDescription.Size = new System.Drawing.Size(78, 17);
            this.rbtnDescription.TabIndex = 7;
            this.rbtnDescription.TabStop = true;
            this.rbtnDescription.Text = "Description";
            this.rbtnDescription.UseVisualStyleBackColor = true;
            this.rbtnDescription.CheckedChanged += new System.EventHandler(this.rbtnDescription_CheckedChanged);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(6, 54);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(71, 13);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "Search String";
            // 
            // txtSearchString
            // 
            this.txtSearchString.Location = new System.Drawing.Point(9, 71);
            this.txtSearchString.Name = "txtSearchString";
            this.txtSearchString.Size = new System.Drawing.Size(172, 20);
            this.txtSearchString.TabIndex = 2;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(8, 51);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(59, 13);
            this.Label2.TabIndex = 5;
            this.Label2.Text = "Filter Value";
            // 
            // lstData
            // 
            this.lstData.FormattingEnabled = true;
            this.lstData.Location = new System.Drawing.Point(12, 123);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(506, 264);
            this.lstData.TabIndex = 16;
            // 
            // txtFilterValue
            // 
            this.txtFilterValue.Location = new System.Drawing.Point(11, 71);
            this.txtFilterValue.Name = "txtFilterValue";
            this.txtFilterValue.Size = new System.Drawing.Size(190, 20);
            this.txtFilterValue.TabIndex = 4;
            // 
            // rbtnSecurityType
            // 
            this.rbtnSecurityType.AutoSize = true;
            this.rbtnSecurityType.Location = new System.Drawing.Point(106, 12);
            this.rbtnSecurityType.Name = "rbtnSecurityType";
            this.rbtnSecurityType.Size = new System.Drawing.Size(90, 17);
            this.rbtnSecurityType.TabIndex = 9;
            this.rbtnSecurityType.TabStop = true;
            this.rbtnSecurityType.Text = "Security Type";
            this.rbtnSecurityType.UseVisualStyleBackColor = true;
            // 
            // txtRequestID
            // 
            this.txtRequestID.Location = new System.Drawing.Point(416, 24);
            this.txtRequestID.Name = "txtRequestID";
            this.txtRequestID.Size = new System.Drawing.Size(102, 20);
            this.txtRequestID.TabIndex = 20;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnLaunch);
            this.gbFilter.Controls.Add(this.Label2);
            this.gbFilter.Controls.Add(this.txtFilterValue);
            this.gbFilter.Controls.Add(this.rbtnSecurityType);
            this.gbFilter.Controls.Add(this.rbtnListedMarket);
            this.gbFilter.Location = new System.Drawing.Point(202, 6);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(208, 97);
            this.gbFilter.TabIndex = 19;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter By";
            // 
            // rbtnListedMarket
            // 
            this.rbtnListedMarket.AutoSize = true;
            this.rbtnListedMarket.Location = new System.Drawing.Point(11, 12);
            this.rbtnListedMarket.Name = "rbtnListedMarket";
            this.rbtnListedMarket.Size = new System.Drawing.Size(89, 17);
            this.rbtnListedMarket.TabIndex = 8;
            this.rbtnListedMarket.TabStop = true;
            this.rbtnListedMarket.Text = "Listed Market";
            this.rbtnListedMarket.UseVisualStyleBackColor = true;
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(416, 48);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(102, 55);
            this.btnGetData.TabIndex = 17;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(416, 6);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(61, 13);
            this.Label3.TabIndex = 21;
            this.Label3.Text = "Request ID";
            // 
            // SymbolLookupSocketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 397);
            this.Controls.Add(this.gbSearch);
            this.Controls.Add(this.lstData);
            this.Controls.Add(this.txtRequestID);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.Label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SymbolLookupSocketForm";
            this.Text = "C# Symbol Lookup Socket";
            this.Load += new System.EventHandler(this.SymbolLookupSocketForm_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnLaunch;
        internal System.Windows.Forms.RadioButton rbtnNAICS;
        internal System.Windows.Forms.GroupBox gbSearch;
        internal System.Windows.Forms.RadioButton rbtnSIC;
        internal System.Windows.Forms.RadioButton rbtnSymbol;
        internal System.Windows.Forms.RadioButton rbtnDescription;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtSearchString;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.ListBox lstData;
        internal System.Windows.Forms.TextBox txtFilterValue;
        internal System.Windows.Forms.RadioButton rbtnSecurityType;
        internal System.Windows.Forms.TextBox txtRequestID;
        internal System.Windows.Forms.GroupBox gbFilter;
        internal System.Windows.Forms.RadioButton rbtnListedMarket;
        internal System.Windows.Forms.Button btnGetData;
        internal System.Windows.Forms.Label Label3;
    }
}

