namespace StreamingBarsSocket
{
    partial class StreamingBarsSocketForm
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
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnWatch = new System.Windows.Forms.Button();
            this.btnUnwatch = new System.Windows.Forms.Button();
            this.btnUnwatchAll = new System.Windows.Forms.Button();
            this.btnRequestWatches = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRequestID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbIntervalType = new System.Windows.Forms.ComboBox();
            this.txtIntervalValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEndFilterTime = new System.Windows.Forms.TextBox();
            this.txtBeginFilterTime = new System.Windows.Forms.TextBox();
            this.txtBeginDateTime = new System.Windows.Forms.TextBox();
            this.txtDays = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUpdateIntervalInSeconds = new System.Windows.Forms.MaskedTextBox();
            this.lstData = new System.Windows.Forms.ListView();
            this.txtDatapoints = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // txtSymbol
            // 
            this.txtSymbol.Location = new System.Drawing.Point(15, 30);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(88, 20);
            this.txtSymbol.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Symbol";
            // 
            // btnWatch
            // 
            this.btnWatch.Location = new System.Drawing.Point(447, 3);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(108, 23);
            this.btnWatch.TabIndex = 20;
            this.btnWatch.Text = "Watch";
            this.btnWatch.UseVisualStyleBackColor = true;
            this.btnWatch.Click += new System.EventHandler(this.btnWatch_Click);
            // 
            // btnUnwatch
            // 
            this.btnUnwatch.Location = new System.Drawing.Point(447, 32);
            this.btnUnwatch.Name = "btnUnwatch";
            this.btnUnwatch.Size = new System.Drawing.Size(108, 23);
            this.btnUnwatch.TabIndex = 21;
            this.btnUnwatch.Text = "Unwatch";
            this.btnUnwatch.UseVisualStyleBackColor = true;
            this.btnUnwatch.Click += new System.EventHandler(this.btnUnwatch_Click);
            // 
            // btnUnwatchAll
            // 
            this.btnUnwatchAll.Location = new System.Drawing.Point(447, 90);
            this.btnUnwatchAll.Name = "btnUnwatchAll";
            this.btnUnwatchAll.Size = new System.Drawing.Size(108, 23);
            this.btnUnwatchAll.TabIndex = 23;
            this.btnUnwatchAll.Text = "Unwatch All";
            this.btnUnwatchAll.UseVisualStyleBackColor = true;
            this.btnUnwatchAll.Click += new System.EventHandler(this.btnUnwatchAll_Click);
            // 
            // btnRequestWatches
            // 
            this.btnRequestWatches.Location = new System.Drawing.Point(447, 61);
            this.btnRequestWatches.Name = "btnRequestWatches";
            this.btnRequestWatches.Size = new System.Drawing.Size(108, 23);
            this.btnRequestWatches.TabIndex = 22;
            this.btnRequestWatches.Text = "Request Watches";
            this.btnRequestWatches.UseVisualStyleBackColor = true;
            this.btnRequestWatches.Click += new System.EventHandler(this.btnRequestWatches_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Request ID";
            // 
            // txtRequestID
            // 
            this.txtRequestID.Location = new System.Drawing.Point(111, 30);
            this.txtRequestID.Name = "txtRequestID";
            this.txtRequestID.Size = new System.Drawing.Size(123, 20);
            this.txtRequestID.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AllowDrop = true;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(346, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Interval Type";
            // 
            // cbIntervalType
            // 
            this.cbIntervalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIntervalType.FormattingEnabled = true;
            this.cbIntervalType.Items.AddRange(new object[] {
            "second",
            "tick",
            "volume"});
            this.cbIntervalType.Location = new System.Drawing.Point(348, 29);
            this.cbIntervalType.Name = "cbIntervalType";
            this.cbIntervalType.Size = new System.Drawing.Size(93, 21);
            this.cbIntervalType.TabIndex = 7;
            // 
            // txtIntervalValue
            // 
            this.txtIntervalValue.Location = new System.Drawing.Point(238, 28);
            this.txtIntervalValue.Name = "txtIntervalValue";
            this.txtIntervalValue.Size = new System.Drawing.Size(100, 20);
            this.txtIntervalValue.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(235, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Interval";
            // 
            // Label11
            // 
            this.Label11.BackColor = System.Drawing.SystemColors.Control;
            this.Label11.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label11.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label11.Location = new System.Drawing.Point(308, 56);
            this.Label11.Name = "Label11";
            this.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label11.Size = new System.Drawing.Size(92, 17);
            this.Label11.TabIndex = 16;
            this.Label11.Text = "End Filter Time";
            // 
            // Label10
            // 
            this.Label10.BackColor = System.Drawing.SystemColors.Control;
            this.Label10.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label10.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label10.Location = new System.Drawing.Point(214, 56);
            this.Label10.Name = "Label10";
            this.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label10.Size = new System.Drawing.Size(92, 17);
            this.Label10.TabIndex = 14;
            this.Label10.Text = "Begin Filter Time";
            // 
            // Label8
            // 
            this.Label8.BackColor = System.Drawing.SystemColors.Control;
            this.Label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label8.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label8.Location = new System.Drawing.Point(116, 56);
            this.Label8.Name = "Label8";
            this.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label8.Size = new System.Drawing.Size(92, 17);
            this.Label8.TabIndex = 12;
            this.Label8.Text = "Begin Date Time";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(79, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Days";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Datapoints";
            // 
            // txtEndFilterTime
            // 
            this.txtEndFilterTime.Location = new System.Drawing.Point(310, 73);
            this.txtEndFilterTime.Name = "txtEndFilterTime";
            this.txtEndFilterTime.Size = new System.Drawing.Size(90, 20);
            this.txtEndFilterTime.TabIndex = 17;
            // 
            // txtBeginFilterTime
            // 
            this.txtBeginFilterTime.Location = new System.Drawing.Point(214, 73);
            this.txtBeginFilterTime.Name = "txtBeginFilterTime";
            this.txtBeginFilterTime.Size = new System.Drawing.Size(90, 20);
            this.txtBeginFilterTime.TabIndex = 15;
            // 
            // txtBeginDateTime
            // 
            this.txtBeginDateTime.Location = new System.Drawing.Point(117, 73);
            this.txtBeginDateTime.Name = "txtBeginDateTime";
            this.txtBeginDateTime.Size = new System.Drawing.Size(90, 20);
            this.txtBeginDateTime.TabIndex = 13;
            // 
            // txtDays
            // 
            this.txtDays.Location = new System.Drawing.Point(82, 73);
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(28, 20);
            this.txtDays.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Update Interval (sec)";
            // 
            // txtUpdateIntervalInSeconds
            // 
            this.txtUpdateIntervalInSeconds.Location = new System.Drawing.Point(18, 112);
            this.txtUpdateIntervalInSeconds.Mask = "990";
            this.txtUpdateIntervalInSeconds.Name = "txtUpdateIntervalInSeconds";
            this.txtUpdateIntervalInSeconds.PromptChar = ' ';
            this.txtUpdateIntervalInSeconds.Size = new System.Drawing.Size(100, 20);
            this.txtUpdateIntervalInSeconds.TabIndex = 19;
            this.txtUpdateIntervalInSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtUpdateIntervalInSeconds.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // lstData
            // 
            this.lstData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstData.Location = new System.Drawing.Point(18, 138);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(536, 276);
            this.lstData.TabIndex = 24;
            this.lstData.UseCompatibleStateImageBehavior = false;
            this.lstData.View = System.Windows.Forms.View.Details;
            // 
            // txtDatapoints
            // 
            this.txtDatapoints.Location = new System.Drawing.Point(18, 73);
            this.txtDatapoints.Mask = "9999990";
            this.txtDatapoints.Name = "txtDatapoints";
            this.txtDatapoints.PromptChar = ' ';
            this.txtDatapoints.Size = new System.Drawing.Size(55, 20);
            this.txtDatapoints.TabIndex = 9;
            // 
            // StreamingBarsSocketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 415);
            this.Controls.Add(this.txtDatapoints);
            this.Controls.Add(this.lstData);
            this.Controls.Add(this.txtUpdateIntervalInSeconds);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Label11);
            this.Controls.Add(this.Label10);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtEndFilterTime);
            this.Controls.Add(this.txtBeginFilterTime);
            this.Controls.Add(this.txtBeginDateTime);
            this.Controls.Add(this.txtDays);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtIntervalValue);
            this.Controls.Add(this.cbIntervalType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRequestID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRequestWatches);
            this.Controls.Add(this.btnUnwatchAll);
            this.Controls.Add(this.btnUnwatch);
            this.Controls.Add(this.btnWatch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSymbol);
            this.Name = "StreamingBarsSocketForm";
            this.Text = "Streaming Interval Bars Socket";
            this.Load += new System.EventHandler(this.StreamingBarsSocket_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSymbol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnWatch;
        private System.Windows.Forms.Button btnUnwatch;
        private System.Windows.Forms.Button btnUnwatchAll;
        private System.Windows.Forms.Button btnRequestWatches;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRequestID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbIntervalType;
        private System.Windows.Forms.TextBox txtIntervalValue;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label Label11;
        public System.Windows.Forms.Label Label10;
        public System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox txtEndFilterTime;
        internal System.Windows.Forms.TextBox txtBeginFilterTime;
        internal System.Windows.Forms.TextBox txtBeginDateTime;
        internal System.Windows.Forms.TextBox txtDays;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox txtUpdateIntervalInSeconds;
        private System.Windows.Forms.ListView lstData;
        private System.Windows.Forms.MaskedTextBox txtDatapoints;
    }
}

