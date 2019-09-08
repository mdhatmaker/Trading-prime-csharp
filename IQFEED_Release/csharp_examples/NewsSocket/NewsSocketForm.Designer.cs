namespace NewsSocket
{
    partial class NewsSocketForm
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
            this.treeNewsTypes = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.cboRequestType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtStory = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.rbText = new System.Windows.Forms.RadioButton();
            this.rbXML = new System.Windows.Forms.RadioButton();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtRequestID = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.txtStoryID = new System.Windows.Forms.TextBox();
            this.txtNewsTypes = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.txtLimit = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.lstHeadlines = new System.Windows.Forms.ListBox();
            this.lblHeadlinesStory = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeNewsTypes
            // 
            this.treeNewsTypes.CheckBoxes = true;
            this.treeNewsTypes.Location = new System.Drawing.Point(12, 23);
            this.treeNewsTypes.Name = "treeNewsTypes";
            this.treeNewsTypes.Size = new System.Drawing.Size(138, 484);
            this.treeNewsTypes.TabIndex = 2;
            this.treeNewsTypes.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeNewsTypes_AfterCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "News Config (Sources)";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(452, 18);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(103, 29);
            this.btnSubmit.TabIndex = 8;
            this.btnSubmit.Text = "Submit Request";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // cboRequestType
            // 
            this.cboRequestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRequestType.FormattingEnabled = true;
            this.cboRequestType.Location = new System.Drawing.Point(156, 23);
            this.cboRequestType.Name = "cboRequestType";
            this.cboRequestType.Size = new System.Drawing.Size(262, 21);
            this.cboRequestType.TabIndex = 1;
            this.cboRequestType.SelectedIndexChanged += new System.EventHandler(this.cboRequestType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "Select Request";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(153, 305);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(186, 13);
            this.Label6.TabIndex = 114;
            this.Label6.Text = "Raw data from last request to IQFeed:";
            // 
            // txtStory
            // 
            this.txtStory.Location = new System.Drawing.Point(156, 173);
            this.txtStory.Multiline = true;
            this.txtStory.Name = "txtStory";
            this.txtStory.Size = new System.Drawing.Size(619, 121);
            this.txtStory.TabIndex = 113;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Label12);
            this.groupBox1.Controls.Add(this.rbText);
            this.groupBox1.Controls.Add(this.rbXML);
            this.groupBox1.Controls.Add(this.Label5);
            this.groupBox1.Controls.Add(this.txtEmail);
            this.groupBox1.Controls.Add(this.Label4);
            this.groupBox1.Controls.Add(this.txtRequestID);
            this.groupBox1.Controls.Add(this.Label3);
            this.groupBox1.Controls.Add(this.Label8);
            this.groupBox1.Controls.Add(this.txtSymbol);
            this.groupBox1.Controls.Add(this.txtStoryID);
            this.groupBox1.Controls.Add(this.txtNewsTypes);
            this.groupBox1.Controls.Add(this.Label9);
            this.groupBox1.Controls.Add(this.txtLimit);
            this.groupBox1.Controls.Add(this.Label10);
            this.groupBox1.Controls.Add(this.txtDate);
            this.groupBox1.Controls.Add(this.Label11);
            this.groupBox1.Location = new System.Drawing.Point(156, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(619, 101);
            this.groupBox1.TabIndex = 112;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Request Parameters";
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(334, 58);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(89, 13);
            this.Label12.TabIndex = 60;
            this.Label12.Text = "Return results as:";
            this.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbText
            // 
            this.rbText.AutoSize = true;
            this.rbText.Location = new System.Drawing.Point(389, 75);
            this.rbText.Name = "rbText";
            this.rbText.Size = new System.Drawing.Size(46, 17);
            this.rbText.TabIndex = 59;
            this.rbText.TabStop = true;
            this.rbText.Text = "Text";
            this.rbText.UseVisualStyleBackColor = true;
            // 
            // rbXML
            // 
            this.rbXML.AutoSize = true;
            this.rbXML.Location = new System.Drawing.Point(337, 75);
            this.rbXML.Name = "rbXML";
            this.rbXML.Size = new System.Drawing.Size(47, 17);
            this.rbXML.TabIndex = 58;
            this.rbXML.TabStop = true;
            this.rbXML.Text = "XML";
            this.rbXML.UseVisualStyleBackColor = true;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(156, 57);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(35, 13);
            this.Label5.TabIndex = 57;
            this.Label5.Text = "Email:";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(159, 75);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(143, 20);
            this.txtEmail.TabIndex = 56;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(7, 57);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(61, 13);
            this.Label4.TabIndex = 55;
            this.Label4.Text = "RequestID:";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRequestID
            // 
            this.txtRequestID.Location = new System.Drawing.Point(10, 75);
            this.txtRequestID.Name = "txtRequestID";
            this.txtRequestID.Size = new System.Drawing.Size(143, 20);
            this.txtRequestID.TabIndex = 54;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(6, 16);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(85, 13);
            this.Label3.TabIndex = 35;
            this.Label3.Text = "News Source(s):";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(155, 16);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(45, 13);
            this.Label8.TabIndex = 53;
            this.Label8.Text = "Story ID";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSymbol
            // 
            this.txtSymbol.Location = new System.Drawing.Point(366, 34);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(122, 20);
            this.txtSymbol.TabIndex = 7;
            // 
            // txtStoryID
            // 
            this.txtStoryID.Location = new System.Drawing.Point(158, 34);
            this.txtStoryID.Name = "txtStoryID";
            this.txtStoryID.Size = new System.Drawing.Size(98, 20);
            this.txtStoryID.TabIndex = 5;
            // 
            // txtNewsTypes
            // 
            this.txtNewsTypes.Location = new System.Drawing.Point(9, 34);
            this.txtNewsTypes.Name = "txtNewsTypes";
            this.txtNewsTypes.Size = new System.Drawing.Size(143, 20);
            this.txtNewsTypes.TabIndex = 4;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(363, 16);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(55, 13);
            this.Label9.TabIndex = 34;
            this.Label9.Text = "Symbol(s):";
            this.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLimit
            // 
            this.txtLimit.Location = new System.Drawing.Point(262, 34);
            this.txtLimit.Name = "txtLimit";
            this.txtLimit.Size = new System.Drawing.Size(98, 20);
            this.txtLimit.TabIndex = 6;
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(259, 16);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(92, 13);
            this.Label10.TabIndex = 44;
            this.Label10.Text = "Limit (per Source):";
            this.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(494, 34);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(98, 20);
            this.txtDate.TabIndex = 45;
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(491, 16);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(33, 13);
            this.Label11.TabIndex = 46;
            this.Label11.Text = "Date:";
            this.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtResults
            // 
            this.txtResults.AcceptsReturn = true;
            this.txtResults.BackColor = System.Drawing.SystemColors.Window;
            this.txtResults.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtResults.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResults.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtResults.Location = new System.Drawing.Point(156, 321);
            this.txtResults.MaxLength = 0;
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResults.Size = new System.Drawing.Size(619, 186);
            this.txtResults.TabIndex = 109;
            this.txtResults.WordWrap = false;
            // 
            // lstHeadlines
            // 
            this.lstHeadlines.FormattingEnabled = true;
            this.lstHeadlines.Location = new System.Drawing.Point(156, 173);
            this.lstHeadlines.Name = "lstHeadlines";
            this.lstHeadlines.Size = new System.Drawing.Size(619, 121);
            this.lstHeadlines.TabIndex = 110;
            this.lstHeadlines.SelectedIndexChanged += new System.EventHandler(this.lstHeadlines_SelectedIndexChanged);
            // 
            // lblHeadlinesStory
            // 
            this.lblHeadlinesStory.AutoSize = true;
            this.lblHeadlinesStory.Location = new System.Drawing.Point(153, 154);
            this.lblHeadlinesStory.Name = "lblHeadlinesStory";
            this.lblHeadlinesStory.Size = new System.Drawing.Size(57, 13);
            this.lblHeadlinesStory.TabIndex = 111;
            this.lblHeadlinesStory.Text = "Headlines:";
            // 
            // NewsSocketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 527);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.txtStory);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.lstHeadlines);
            this.Controls.Add(this.lblHeadlinesStory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboRequestType);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeNewsTypes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NewsSocketForm";
            this.Text = "C# News Text Socket";
            this.Load += new System.EventHandler(this.NewsSocketForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeNewsTypes;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.ComboBox cboRequestType;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox txtStory;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.RadioButton rbText;
        internal System.Windows.Forms.RadioButton rbXML;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox txtEmail;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox txtRequestID;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.TextBox txtSymbol;
        internal System.Windows.Forms.TextBox txtStoryID;
        internal System.Windows.Forms.TextBox txtNewsTypes;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.TextBox txtLimit;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.TextBox txtDate;
        internal System.Windows.Forms.Label Label11;
        public System.Windows.Forms.TextBox txtResults;
        internal System.Windows.Forms.ListBox lstHeadlines;
        internal System.Windows.Forms.Label lblHeadlinesStory;

    }
}

