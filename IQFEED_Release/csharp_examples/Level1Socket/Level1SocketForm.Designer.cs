namespace Level1Socket
{
    partial class Level1SocketForm
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
            this.btnWatch = new System.Windows.Forms.Button();
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnWatchRegionals = new System.Windows.Forms.Button();
            this.btnTimestamp = new System.Windows.Forms.Button();
            this.btnNewsOn = new System.Windows.Forms.Button();
            this.btnSetFieldset = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnGetCurrentWatches = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveRegionals = new System.Windows.Forms.Button();
            this.btnForce = new System.Windows.Forms.Button();
            this.btnNewsOff = new System.Windows.Forms.Button();
            this.btnGetFieldset = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnRemoveAllWatches = new System.Windows.Forms.Button();
            this.lstData = new System.Windows.Forms.ListBox();
            this.btnGetFundamentalFields = new System.Windows.Forms.Button();
            this.btnGetUpdateSummaryFields = new System.Windows.Forms.Button();
            this.btnTradesOnly = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnWatch
            // 
            this.btnWatch.Location = new System.Drawing.Point(7, 37);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(54, 23);
            this.btnWatch.TabIndex = 0;
            this.btnWatch.Text = "Watch";
            this.btnWatch.UseVisualStyleBackColor = true;
            this.btnWatch.Click += new System.EventHandler(this.btnWatch_Click);
            // 
            // txtRequest
            // 
            this.txtRequest.Location = new System.Drawing.Point(131, 12);
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(584, 20);
            this.txtRequest.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Symbol / Request Data:";
            // 
            // btnWatchRegionals
            // 
            this.btnWatchRegionals.Location = new System.Drawing.Point(186, 37);
            this.btnWatchRegionals.Name = "btnWatchRegionals";
            this.btnWatchRegionals.Size = new System.Drawing.Size(112, 23);
            this.btnWatchRegionals.TabIndex = 3;
            this.btnWatchRegionals.Text = "Watch Regionals";
            this.btnWatchRegionals.UseVisualStyleBackColor = true;
            this.btnWatchRegionals.Click += new System.EventHandler(this.btnWatchRegionals_Click);
            // 
            // btnTimestamp
            // 
            this.btnTimestamp.Location = new System.Drawing.Point(304, 37);
            this.btnTimestamp.Name = "btnTimestamp";
            this.btnTimestamp.Size = new System.Drawing.Size(75, 23);
            this.btnTimestamp.TabIndex = 4;
            this.btnTimestamp.Text = "Timestamp";
            this.btnTimestamp.UseVisualStyleBackColor = true;
            this.btnTimestamp.Click += new System.EventHandler(this.btnTimestamp_Click);
            // 
            // btnNewsOn
            // 
            this.btnNewsOn.Location = new System.Drawing.Point(385, 37);
            this.btnNewsOn.Name = "btnNewsOn";
            this.btnNewsOn.Size = new System.Drawing.Size(61, 23);
            this.btnNewsOn.TabIndex = 5;
            this.btnNewsOn.Text = "News On";
            this.btnNewsOn.UseVisualStyleBackColor = true;
            this.btnNewsOn.Click += new System.EventHandler(this.btnNewsOn_Click);
            // 
            // btnSetFieldset
            // 
            this.btnSetFieldset.Location = new System.Drawing.Point(543, 95);
            this.btnSetFieldset.Name = "btnSetFieldset";
            this.btnSetFieldset.Size = new System.Drawing.Size(109, 23);
            this.btnSetFieldset.TabIndex = 6;
            this.btnSetFieldset.Text = "Set Fieldset";
            this.btnSetFieldset.UseVisualStyleBackColor = true;
            this.btnSetFieldset.Click += new System.EventHandler(this.btnSetFieldset_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(452, 37);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnGetCurrentWatches
            // 
            this.btnGetCurrentWatches.Location = new System.Drawing.Point(533, 37);
            this.btnGetCurrentWatches.Name = "btnGetCurrentWatches";
            this.btnGetCurrentWatches.Size = new System.Drawing.Size(119, 23);
            this.btnGetCurrentWatches.TabIndex = 8;
            this.btnGetCurrentWatches.Text = "Get Current Watches";
            this.btnGetCurrentWatches.UseVisualStyleBackColor = true;
            this.btnGetCurrentWatches.Click += new System.EventHandler(this.btnGetCurrentWatches_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(7, 66);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(173, 23);
            this.btnRemove.TabIndex = 9;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveRegionals
            // 
            this.btnRemoveRegionals.Location = new System.Drawing.Point(186, 66);
            this.btnRemoveRegionals.Name = "btnRemoveRegionals";
            this.btnRemoveRegionals.Size = new System.Drawing.Size(112, 23);
            this.btnRemoveRegionals.TabIndex = 10;
            this.btnRemoveRegionals.Text = "Remove Regionals";
            this.btnRemoveRegionals.UseVisualStyleBackColor = true;
            this.btnRemoveRegionals.Click += new System.EventHandler(this.btnRemoveRegionals_Click);
            // 
            // btnForce
            // 
            this.btnForce.Location = new System.Drawing.Point(304, 66);
            this.btnForce.Name = "btnForce";
            this.btnForce.Size = new System.Drawing.Size(75, 23);
            this.btnForce.TabIndex = 11;
            this.btnForce.Text = "Force";
            this.btnForce.UseVisualStyleBackColor = true;
            this.btnForce.Click += new System.EventHandler(this.btnForce_Click);
            // 
            // btnNewsOff
            // 
            this.btnNewsOff.Location = new System.Drawing.Point(385, 66);
            this.btnNewsOff.Name = "btnNewsOff";
            this.btnNewsOff.Size = new System.Drawing.Size(61, 23);
            this.btnNewsOff.TabIndex = 12;
            this.btnNewsOff.Text = "News Off";
            this.btnNewsOff.UseVisualStyleBackColor = true;
            this.btnNewsOff.Click += new System.EventHandler(this.btnNewsOff_Click);
            // 
            // btnGetFieldset
            // 
            this.btnGetFieldset.Location = new System.Drawing.Point(395, 95);
            this.btnGetFieldset.Name = "btnGetFieldset";
            this.btnGetFieldset.Size = new System.Drawing.Size(142, 23);
            this.btnGetFieldset.TabIndex = 13;
            this.btnGetFieldset.Text = "Get Current Fieldset";
            this.btnGetFieldset.UseVisualStyleBackColor = true;
            this.btnGetFieldset.Click += new System.EventHandler(this.btnGetFieldset_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(452, 66);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 14;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnRemoveAllWatches
            // 
            this.btnRemoveAllWatches.Location = new System.Drawing.Point(533, 66);
            this.btnRemoveAllWatches.Name = "btnRemoveAllWatches";
            this.btnRemoveAllWatches.Size = new System.Drawing.Size(119, 23);
            this.btnRemoveAllWatches.TabIndex = 15;
            this.btnRemoveAllWatches.Text = "Remove All Watches";
            this.btnRemoveAllWatches.UseVisualStyleBackColor = true;
            this.btnRemoveAllWatches.Click += new System.EventHandler(this.btnRemoveAllWatches_Click);
            // 
            // lstData
            // 
            this.lstData.FormattingEnabled = true;
            this.lstData.HorizontalScrollbar = true;
            this.lstData.Location = new System.Drawing.Point(7, 127);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(707, 264);
            this.lstData.TabIndex = 16;
            // 
            // btnGetFundamentalFields
            // 
            this.btnGetFundamentalFields.Location = new System.Drawing.Point(7, 95);
            this.btnGetFundamentalFields.Name = "btnGetFundamentalFields";
            this.btnGetFundamentalFields.Size = new System.Drawing.Size(183, 23);
            this.btnGetFundamentalFields.TabIndex = 17;
            this.btnGetFundamentalFields.Text = "Get All Fundamental Fields";
            this.btnGetFundamentalFields.UseVisualStyleBackColor = true;
            this.btnGetFundamentalFields.Click += new System.EventHandler(this.btnGetFundamentalFields_Click);
            // 
            // btnGetUpdateSummaryFields
            // 
            this.btnGetUpdateSummaryFields.Location = new System.Drawing.Point(196, 95);
            this.btnGetUpdateSummaryFields.Name = "btnGetUpdateSummaryFields";
            this.btnGetUpdateSummaryFields.Size = new System.Drawing.Size(193, 23);
            this.btnGetUpdateSummaryFields.TabIndex = 18;
            this.btnGetUpdateSummaryFields.Text = "Get All Update/Summary Fields";
            this.btnGetUpdateSummaryFields.UseVisualStyleBackColor = true;
            this.btnGetUpdateSummaryFields.Click += new System.EventHandler(this.btnGetUpdateSummaryFields_Click);
            // 
            // btnTradesOnly
            // 
            this.btnTradesOnly.Location = new System.Drawing.Point(67, 37);
            this.btnTradesOnly.Name = "btnTradesOnly";
            this.btnTradesOnly.Size = new System.Drawing.Size(113, 23);
            this.btnTradesOnly.TabIndex = 19;
            this.btnTradesOnly.Text = "Trades Only Watch";
            this.btnTradesOnly.UseVisualStyleBackColor = true;
            this.btnTradesOnly.Click += new System.EventHandler(this.btnTradesOnly_Click);
            // 
            // Level1SocketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 403);
            this.Controls.Add(this.btnTradesOnly);
            this.Controls.Add(this.btnGetUpdateSummaryFields);
            this.Controls.Add(this.btnGetFundamentalFields);
            this.Controls.Add(this.lstData);
            this.Controls.Add(this.btnRemoveAllWatches);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnGetFieldset);
            this.Controls.Add(this.btnNewsOff);
            this.Controls.Add(this.btnForce);
            this.Controls.Add(this.btnRemoveRegionals);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnGetCurrentWatches);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnSetFieldset);
            this.Controls.Add(this.btnNewsOn);
            this.Controls.Add(this.btnTimestamp);
            this.Controls.Add(this.btnWatchRegionals);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRequest);
            this.Controls.Add(this.btnWatch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Level1SocketForm";
            this.Text = "C# Level 1 Socket";
            this.Load += new System.EventHandler(this.Level1SocketForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnWatch;
        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnWatchRegionals;
        private System.Windows.Forms.Button btnTimestamp;
        private System.Windows.Forms.Button btnNewsOn;
        private System.Windows.Forms.Button btnSetFieldset;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnGetCurrentWatches;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnRemoveRegionals;
        private System.Windows.Forms.Button btnForce;
        private System.Windows.Forms.Button btnNewsOff;
        private System.Windows.Forms.Button btnGetFieldset;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnRemoveAllWatches;
        private System.Windows.Forms.ListBox lstData;
        private System.Windows.Forms.Button btnGetFundamentalFields;
        private System.Windows.Forms.Button btnGetUpdateSummaryFields;
        private System.Windows.Forms.Button btnTradesOnly;
    }
}

