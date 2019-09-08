namespace HistorySocket
{
    partial class HistorySocketForm
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
            this.Label13 = new System.Windows.Forms.Label();
            this.rbTick = new System.Windows.Forms.RadioButton();
            this.rbVolume = new System.Windows.Forms.RadioButton();
            this.rbTime = new System.Windows.Forms.RadioButton();
            this.btnGetData = new System.Windows.Forms.Button();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtRequestID = new System.Windows.Forms.TextBox();
            this.txtDirection = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtDatapointsPerSend = new System.Windows.Forms.TextBox();
            this.txtEndFilterTime = new System.Windows.Forms.TextBox();
            this.txtBeginFilterTime = new System.Windows.Forms.TextBox();
            this.txtEndDateTime = new System.Windows.Forms.TextBox();
            this.txtBeginDateTime = new System.Windows.Forms.TextBox();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.txtDays = new System.Windows.Forms.TextBox();
            this.txtDatapoints = new System.Windows.Forms.TextBox();
            this.cboHistoryType = new System.Windows.Forms.ComboBox();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.lstData = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(380, 8);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(69, 13);
            this.Label13.TabIndex = 111;
            this.Label13.Text = "Interval Type";
            // 
            // rbTick
            // 
            this.rbTick.AutoSize = true;
            this.rbTick.Location = new System.Drawing.Point(501, 25);
            this.rbTick.Name = "rbTick";
            this.rbTick.Size = new System.Drawing.Size(46, 17);
            this.rbTick.TabIndex = 110;
            this.rbTick.Text = "Tick";
            this.rbTick.UseVisualStyleBackColor = true;
            // 
            // rbVolume
            // 
            this.rbVolume.AutoSize = true;
            this.rbVolume.Location = new System.Drawing.Point(435, 25);
            this.rbVolume.Name = "rbVolume";
            this.rbVolume.Size = new System.Drawing.Size(60, 17);
            this.rbVolume.TabIndex = 109;
            this.rbVolume.Text = "Volume";
            this.rbVolume.UseVisualStyleBackColor = true;
            // 
            // rbTime
            // 
            this.rbTime.AutoSize = true;
            this.rbTime.Checked = true;
            this.rbTime.Location = new System.Drawing.Point(383, 25);
            this.rbTime.Name = "rbTime";
            this.rbTime.Size = new System.Drawing.Size(48, 17);
            this.rbTime.TabIndex = 108;
            this.rbTime.TabStop = true;
            this.rbTime.Text = "Time";
            this.rbTime.UseVisualStyleBackColor = true;
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(613, 17);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(94, 69);
            this.btnGetData.TabIndex = 107;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // Label12
            // 
            this.Label12.BackColor = System.Drawing.SystemColors.Control;
            this.Label12.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label12.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label12.Location = new System.Drawing.Point(389, 50);
            this.Label12.Name = "Label12";
            this.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label12.Size = new System.Drawing.Size(107, 17);
            this.Label12.TabIndex = 106;
            this.Label12.Text = "Datapoints Per Send";
            // 
            // Label11
            // 
            this.Label11.BackColor = System.Drawing.SystemColors.Control;
            this.Label11.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label11.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label11.Location = new System.Drawing.Point(293, 50);
            this.Label11.Name = "Label11";
            this.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label11.Size = new System.Drawing.Size(92, 17);
            this.Label11.TabIndex = 105;
            this.Label11.Text = "End Filter Time";
            // 
            // Label10
            // 
            this.Label10.BackColor = System.Drawing.SystemColors.Control;
            this.Label10.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label10.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label10.Location = new System.Drawing.Point(199, 50);
            this.Label10.Name = "Label10";
            this.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label10.Size = new System.Drawing.Size(92, 17);
            this.Label10.TabIndex = 104;
            this.Label10.Text = "Begin Filter Time";
            // 
            // Label9
            // 
            this.Label9.BackColor = System.Drawing.SystemColors.Control;
            this.Label9.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label9.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label9.Location = new System.Drawing.Point(104, 50);
            this.Label9.Name = "Label9";
            this.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label9.Size = new System.Drawing.Size(92, 17);
            this.Label9.TabIndex = 103;
            this.Label9.Text = "End Date Time";
            // 
            // Label8
            // 
            this.Label8.BackColor = System.Drawing.SystemColors.Control;
            this.Label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label8.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label8.Location = new System.Drawing.Point(6, 50);
            this.Label8.Name = "Label8";
            this.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label8.Size = new System.Drawing.Size(92, 17);
            this.Label8.TabIndex = 102;
            this.Label8.Text = "Begin Date Time";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(551, 50);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(58, 13);
            this.Label7.TabIndex = 101;
            this.Label7.Text = "RequestID";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(496, 50);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(49, 13);
            this.Label6.TabIndex = 100;
            this.Label6.Text = "Direction";
            // 
            // txtRequestID
            // 
            this.txtRequestID.Location = new System.Drawing.Point(540, 67);
            this.txtRequestID.Name = "txtRequestID";
            this.txtRequestID.Size = new System.Drawing.Size(67, 20);
            this.txtRequestID.TabIndex = 99;
            // 
            // txtDirection
            // 
            this.txtDirection.Location = new System.Drawing.Point(504, 67);
            this.txtDirection.Name = "txtDirection";
            this.txtDirection.Size = new System.Drawing.Size(28, 20);
            this.txtDirection.TabIndex = 98;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(320, 8);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(42, 13);
            this.Label5.TabIndex = 97;
            this.Label5.Text = "Interval";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(282, 8);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(31, 13);
            this.Label4.TabIndex = 96;
            this.Label4.Text = "Days";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(218, 8);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(58, 13);
            this.Label3.TabIndex = 95;
            this.Label3.Text = "Datapoints";
            // 
            // txtDatapointsPerSend
            // 
            this.txtDatapointsPerSend.Location = new System.Drawing.Point(391, 67);
            this.txtDatapointsPerSend.Name = "txtDatapointsPerSend";
            this.txtDatapointsPerSend.Size = new System.Drawing.Size(104, 20);
            this.txtDatapointsPerSend.TabIndex = 94;
            // 
            // txtEndFilterTime
            // 
            this.txtEndFilterTime.Location = new System.Drawing.Point(295, 67);
            this.txtEndFilterTime.Name = "txtEndFilterTime";
            this.txtEndFilterTime.Size = new System.Drawing.Size(90, 20);
            this.txtEndFilterTime.TabIndex = 93;
            // 
            // txtBeginFilterTime
            // 
            this.txtBeginFilterTime.Location = new System.Drawing.Point(199, 67);
            this.txtBeginFilterTime.Name = "txtBeginFilterTime";
            this.txtBeginFilterTime.Size = new System.Drawing.Size(90, 20);
            this.txtBeginFilterTime.TabIndex = 92;
            // 
            // txtEndDateTime
            // 
            this.txtEndDateTime.Location = new System.Drawing.Point(103, 67);
            this.txtEndDateTime.Name = "txtEndDateTime";
            this.txtEndDateTime.Size = new System.Drawing.Size(90, 20);
            this.txtEndDateTime.TabIndex = 91;
            // 
            // txtBeginDateTime
            // 
            this.txtBeginDateTime.Location = new System.Drawing.Point(7, 67);
            this.txtBeginDateTime.Name = "txtBeginDateTime";
            this.txtBeginDateTime.Size = new System.Drawing.Size(90, 20);
            this.txtBeginDateTime.TabIndex = 90;
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(319, 25);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(43, 20);
            this.txtInterval.TabIndex = 89;
            // 
            // txtDays
            // 
            this.txtDays.Location = new System.Drawing.Point(285, 25);
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(28, 20);
            this.txtDays.TabIndex = 88;
            // 
            // txtDatapoints
            // 
            this.txtDatapoints.Location = new System.Drawing.Point(221, 25);
            this.txtDatapoints.Name = "txtDatapoints";
            this.txtDatapoints.Size = new System.Drawing.Size(58, 20);
            this.txtDatapoints.TabIndex = 87;
            // 
            // cboHistoryType
            // 
            this.cboHistoryType.BackColor = System.Drawing.SystemColors.Window;
            this.cboHistoryType.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboHistoryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHistoryType.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboHistoryType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboHistoryType.Location = new System.Drawing.Point(8, 25);
            this.cboHistoryType.Name = "cboHistoryType";
            this.cboHistoryType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cboHistoryType.Size = new System.Drawing.Size(113, 22);
            this.cboHistoryType.TabIndex = 84;
            this.cboHistoryType.SelectedIndexChanged += new System.EventHandler(this.cboHistoryType_SelectedIndexChanged);
            // 
            // txtSymbol
            // 
            this.txtSymbol.AcceptsReturn = true;
            this.txtSymbol.BackColor = System.Drawing.SystemColors.Window;
            this.txtSymbol.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSymbol.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSymbol.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSymbol.Location = new System.Drawing.Point(126, 25);
            this.txtSymbol.MaxLength = 0;
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtSymbol.Size = new System.Drawing.Size(89, 20);
            this.txtSymbol.TabIndex = 83;
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.SystemColors.Control;
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label2.Location = new System.Drawing.Point(5, 8);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(33, 17);
            this.Label2.TabIndex = 86;
            this.Label2.Text = "Type:";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.SystemColors.Control;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label1.Location = new System.Drawing.Point(123, 8);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(52, 17);
            this.Label1.TabIndex = 85;
            this.Label1.Text = "Symbol:";
            // 
            // lstData
            // 
            this.lstData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstData.Location = new System.Drawing.Point(9, 93);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(692, 333);
            this.lstData.TabIndex = 112;
            this.lstData.UseCompatibleStateImageBehavior = false;
            this.lstData.View = System.Windows.Forms.View.Details;
            // 
            // HistorySocketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 438);
            this.Controls.Add(this.lstData);
            this.Controls.Add(this.Label13);
            this.Controls.Add(this.rbTick);
            this.Controls.Add(this.rbVolume);
            this.Controls.Add(this.rbTime);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.Label12);
            this.Controls.Add(this.Label11);
            this.Controls.Add(this.Label10);
            this.Controls.Add(this.Label9);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.txtRequestID);
            this.Controls.Add(this.txtDirection);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.txtDatapointsPerSend);
            this.Controls.Add(this.txtEndFilterTime);
            this.Controls.Add(this.txtBeginFilterTime);
            this.Controls.Add(this.txtEndDateTime);
            this.Controls.Add(this.txtBeginDateTime);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.txtDays);
            this.Controls.Add(this.txtDatapoints);
            this.Controls.Add(this.cboHistoryType);
            this.Controls.Add(this.txtSymbol);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "HistorySocketForm";
            this.Text = "C# History Socket";
            this.Load += new System.EventHandler(this.HistorySocketForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.RadioButton rbTick;
        internal System.Windows.Forms.RadioButton rbVolume;
        internal System.Windows.Forms.RadioButton rbTime;
        internal System.Windows.Forms.Button btnGetData;
        public System.Windows.Forms.Label Label12;
        public System.Windows.Forms.Label Label11;
        public System.Windows.Forms.Label Label10;
        public System.Windows.Forms.Label Label9;
        public System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox txtRequestID;
        internal System.Windows.Forms.TextBox txtDirection;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox txtDatapointsPerSend;
        internal System.Windows.Forms.TextBox txtEndFilterTime;
        internal System.Windows.Forms.TextBox txtBeginFilterTime;
        internal System.Windows.Forms.TextBox txtEndDateTime;
        internal System.Windows.Forms.TextBox txtBeginDateTime;
        internal System.Windows.Forms.TextBox txtInterval;
        internal System.Windows.Forms.TextBox txtDays;
        internal System.Windows.Forms.TextBox txtDatapoints;
        public System.Windows.Forms.ComboBox cboHistoryType;
        public System.Windows.Forms.TextBox txtSymbol;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ListView lstData;

    }
}

