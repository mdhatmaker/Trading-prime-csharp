namespace Level1ActiveX
{
    partial class Level1ActiveXForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Level1ActiveXForm));
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLoginID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSetAutoLogin = new System.Windows.Forms.Button();
            this.btnClearAutoLogin = new System.Windows.Forms.Button();
            this.btnEcho = new System.Windows.Forms.Button();
            this.btnWatch = new System.Windows.Forms.Button();
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnWatchRegionals = new System.Windows.Forms.Button();
            this.lstData = new System.Windows.Forms.ListBox();
            this.btnNewsOn = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveRegionals = new System.Windows.Forms.Button();
            this.btnNewsOff = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnTradesOnly = new System.Windows.Forms.Button();
            this.axIQFeedY1 = new AxIQFEEDYLib.AxIQFeedY();
            ((System.ComponentModel.ISupportInitialize)(this.axIQFeedY1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(686, 62);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(98, 20);
            this.txtPassword.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(627, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 42;
            this.label3.Text = "Password";
            // 
            // txtLoginID
            // 
            this.txtLoginID.Location = new System.Drawing.Point(686, 33);
            this.txtLoginID.Name = "txtLoginID";
            this.txtLoginID.Size = new System.Drawing.Size(98, 20);
            this.txtLoginID.TabIndex = 39;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(636, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "LoginID";
            // 
            // btnSetAutoLogin
            // 
            this.btnSetAutoLogin.Location = new System.Drawing.Point(438, 31);
            this.btnSetAutoLogin.Name = "btnSetAutoLogin";
            this.btnSetAutoLogin.Size = new System.Drawing.Size(93, 23);
            this.btnSetAutoLogin.TabIndex = 37;
            this.btnSetAutoLogin.Text = "Set AutoLogin";
            this.btnSetAutoLogin.UseVisualStyleBackColor = true;
            this.btnSetAutoLogin.Click += new System.EventHandler(this.btnSetAutoLogin_Click);
            // 
            // btnClearAutoLogin
            // 
            this.btnClearAutoLogin.Location = new System.Drawing.Point(438, 60);
            this.btnClearAutoLogin.Name = "btnClearAutoLogin";
            this.btnClearAutoLogin.Size = new System.Drawing.Size(93, 23);
            this.btnClearAutoLogin.TabIndex = 38;
            this.btnClearAutoLogin.Text = "Clear AutoLogin";
            this.btnClearAutoLogin.UseVisualStyleBackColor = true;
            this.btnClearAutoLogin.Click += new System.EventHandler(this.btnClearAutoLogin_Click);
            // 
            // btnEcho
            // 
            this.btnEcho.Location = new System.Drawing.Point(537, 31);
            this.btnEcho.Name = "btnEcho";
            this.btnEcho.Size = new System.Drawing.Size(84, 23);
            this.btnEcho.TabIndex = 35;
            this.btnEcho.Text = "Request Echo";
            this.btnEcho.UseVisualStyleBackColor = true;
            this.btnEcho.Click += new System.EventHandler(this.btnEcho_Click);
            // 
            // btnWatch
            // 
            this.btnWatch.Location = new System.Drawing.Point(12, 30);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(48, 23);
            this.btnWatch.TabIndex = 17;
            this.btnWatch.Text = "Watch";
            this.btnWatch.UseVisualStyleBackColor = true;
            this.btnWatch.Click += new System.EventHandler(this.btnWatch_Click);
            // 
            // txtRequest
            // 
            this.txtRequest.Location = new System.Drawing.Point(136, 5);
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(648, 20);
            this.txtRequest.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Symbol / Request Data:";
            // 
            // btnWatchRegionals
            // 
            this.btnWatchRegionals.Location = new System.Drawing.Point(180, 30);
            this.btnWatchRegionals.Name = "btnWatchRegionals";
            this.btnWatchRegionals.Size = new System.Drawing.Size(106, 23);
            this.btnWatchRegionals.TabIndex = 20;
            this.btnWatchRegionals.Text = "Watch Regionals";
            this.btnWatchRegionals.UseVisualStyleBackColor = true;
            this.btnWatchRegionals.Click += new System.EventHandler(this.btnWatchRegionals_Click);
            // 
            // lstData
            // 
            this.lstData.FormattingEnabled = true;
            this.lstData.HorizontalScrollbar = true;
            this.lstData.Location = new System.Drawing.Point(12, 87);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(772, 264);
            this.lstData.TabIndex = 33;
            // 
            // btnNewsOn
            // 
            this.btnNewsOn.Location = new System.Drawing.Point(292, 31);
            this.btnNewsOn.Name = "btnNewsOn";
            this.btnNewsOn.Size = new System.Drawing.Size(59, 23);
            this.btnNewsOn.TabIndex = 22;
            this.btnNewsOn.Text = "News On";
            this.btnNewsOn.UseVisualStyleBackColor = true;
            this.btnNewsOn.Click += new System.EventHandler(this.btnNewsOn_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(357, 31);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 24;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(12, 59);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(162, 23);
            this.btnRemove.TabIndex = 26;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveRegionals
            // 
            this.btnRemoveRegionals.Location = new System.Drawing.Point(180, 60);
            this.btnRemoveRegionals.Name = "btnRemoveRegionals";
            this.btnRemoveRegionals.Size = new System.Drawing.Size(106, 23);
            this.btnRemoveRegionals.TabIndex = 27;
            this.btnRemoveRegionals.Text = "Remove Regionals";
            this.btnRemoveRegionals.UseVisualStyleBackColor = true;
            this.btnRemoveRegionals.Click += new System.EventHandler(this.btnRemoveRegionals_Click);
            // 
            // btnNewsOff
            // 
            this.btnNewsOff.Location = new System.Drawing.Point(292, 60);
            this.btnNewsOff.Name = "btnNewsOff";
            this.btnNewsOff.Size = new System.Drawing.Size(59, 23);
            this.btnNewsOff.TabIndex = 29;
            this.btnNewsOff.Text = "News Off";
            this.btnNewsOff.UseVisualStyleBackColor = true;
            this.btnNewsOff.Click += new System.EventHandler(this.btnNewsOff_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(357, 60);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 31;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnTradesOnly
            // 
            this.btnTradesOnly.Location = new System.Drawing.Point(66, 30);
            this.btnTradesOnly.Name = "btnTradesOnly";
            this.btnTradesOnly.Size = new System.Drawing.Size(108, 23);
            this.btnTradesOnly.TabIndex = 44;
            this.btnTradesOnly.Text = "Trades Only Watch";
            this.btnTradesOnly.UseVisualStyleBackColor = true;
            this.btnTradesOnly.Click += new System.EventHandler(this.btnTradesOnly_Click);
            // 
            // axIQFeedY1
            // 
            this.axIQFeedY1.Enabled = true;
            this.axIQFeedY1.Location = new System.Drawing.Point(537, 60);
            this.axIQFeedY1.Name = "axIQFeedY1";
            this.axIQFeedY1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axIQFeedY1.OcxState")));
            this.axIQFeedY1.Size = new System.Drawing.Size(84, 23);
            this.axIQFeedY1.TabIndex = 43;
            this.axIQFeedY1.SystemMessage += new AxIQFEEDYLib._DIQFeedYEvents_SystemMessageEventHandler(this.axIQFeedY1_SystemMessage);
            this.axIQFeedY1.SummaryMessage += new AxIQFEEDYLib._DIQFeedYEvents_SummaryMessageEventHandler(this.axIQFeedY1_SummaryMessage);
            this.axIQFeedY1.NewsMessage += new AxIQFEEDYLib._DIQFeedYEvents_NewsMessageEventHandler(this.axIQFeedY1_NewsMessage);
            this.axIQFeedY1.TimeStampMessage += new AxIQFEEDYLib._DIQFeedYEvents_TimeStampMessageEventHandler(this.axIQFeedY1_TimeStampMessage);
            this.axIQFeedY1.RegionalMessage += new AxIQFEEDYLib._DIQFeedYEvents_RegionalMessageEventHandler(this.axIQFeedY1_RegionalMessage);
            this.axIQFeedY1.FundamentalMessage += new AxIQFEEDYLib._DIQFeedYEvents_FundamentalMessageEventHandler(this.axIQFeedY1_FundamentalMessage);
            this.axIQFeedY1.QuoteMessage += new AxIQFEEDYLib._DIQFeedYEvents_QuoteMessageEventHandler(this.axIQFeedY1_QuoteMessage);
            // 
            // Level1ActiveXForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 361);
            this.Controls.Add(this.btnTradesOnly);
            this.Controls.Add(this.axIQFeedY1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLoginID);
            this.Controls.Add(this.btnClearAutoLogin);
            this.Controls.Add(this.btnSetAutoLogin);
            this.Controls.Add(this.btnEcho);
            this.Controls.Add(this.lstData);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnNewsOff);
            this.Controls.Add(this.btnRemoveRegionals);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnNewsOn);
            this.Controls.Add(this.btnWatchRegionals);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRequest);
            this.Controls.Add(this.btnWatch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Level1ActiveXForm";
            this.Text = "C# Level 1 ActiveX";
            this.Load += new System.EventHandler(this.Level1ActiveXForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axIQFeedY1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLoginID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSetAutoLogin;
        private System.Windows.Forms.Button btnClearAutoLogin;
        private System.Windows.Forms.Button btnEcho;
        private System.Windows.Forms.Button btnWatch;
        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnWatchRegionals;
        private System.Windows.Forms.ListBox lstData;
        private System.Windows.Forms.Button btnNewsOn;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnRemoveRegionals;
        private System.Windows.Forms.Button btnNewsOff;
        private System.Windows.Forms.Button btnDisconnect;
        private AxIQFEEDYLib.AxIQFeedY axIQFeedY1;
        private System.Windows.Forms.Button btnTradesOnly;

    }
}

