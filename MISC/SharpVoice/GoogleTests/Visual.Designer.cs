namespace GoogleTests
{
    partial class Visual
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            this.loginButton = new System.Windows.Forms.Button();
            this.loginEmail = new System.Windows.Forms.TextBox();
            this.loginPass = new System.Windows.Forms.MaskedTextBox();
            this.callGroup = new System.Windows.Forms.GroupBox();
            this.callTo = new System.Windows.Forms.TextBox();
            this.callFrom = new System.Windows.Forms.TextBox();
            this.callStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.smsGroup = new System.Windows.Forms.GroupBox();
            this.smsTo = new System.Windows.Forms.TextBox();
            this.smsMsg = new System.Windows.Forms.TextBox();
            this.smsChars = new System.Windows.Forms.Label();
            this.smsSend = new System.Windows.Forms.Button();
            this.callGroup.SuspendLayout();
            this.smsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(205, 24);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 46);
            this.loginButton.TabIndex = 3;
            this.loginButton.Text = "Login!";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // loginEmail
            // 
            this.loginEmail.Location = new System.Drawing.Point(65, 24);
            this.loginEmail.Name = "loginEmail";
            this.loginEmail.Size = new System.Drawing.Size(123, 20);
            this.loginEmail.TabIndex = 0;
            // 
            // loginPass
            // 
            this.loginPass.Location = new System.Drawing.Point(65, 50);
            this.loginPass.Name = "loginPass";
            this.loginPass.PasswordChar = '*';
            this.loginPass.Size = new System.Drawing.Size(123, 20);
            this.loginPass.TabIndex = 1;
            // 
            // callGroup
            // 
            this.callGroup.Controls.Add(this.callStart);
            this.callGroup.Controls.Add(this.callFrom);
            this.callGroup.Controls.Add(this.callTo);
            this.callGroup.Enabled = false;
            this.callGroup.Location = new System.Drawing.Point(12, 76);
            this.callGroup.Name = "callGroup";
            this.callGroup.Size = new System.Drawing.Size(268, 75);
            this.callGroup.TabIndex = 3;
            this.callGroup.TabStop = false;
            this.callGroup.Text = "Call";
            // 
            // callTo
            // 
            this.callTo.Location = new System.Drawing.Point(7, 20);
            this.callTo.Name = "callTo";
            this.callTo.Size = new System.Drawing.Size(135, 20);
            this.callTo.TabIndex = 0;
            this.callTo.Text = "To";
            // 
            // callFrom
            // 
            this.callFrom.Location = new System.Drawing.Point(7, 47);
            this.callFrom.Name = "callFrom";
            this.callFrom.Size = new System.Drawing.Size(135, 20);
            this.callFrom.TabIndex = 1;
            this.callFrom.Text = "From";
            // 
            // callStart
            // 
            this.callStart.Location = new System.Drawing.Point(176, 29);
            this.callStart.Name = "callStart";
            this.callStart.Size = new System.Drawing.Size(75, 23);
            this.callStart.TabIndex = 2;
            this.callStart.Text = "Call";
            this.callStart.UseVisualStyleBackColor = true;
            this.callStart.Click += new System.EventHandler(this.callStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Email";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Pass";
            // 
            // smsGroup
            // 
            this.smsGroup.Controls.Add(this.smsSend);
            this.smsGroup.Controls.Add(this.smsChars);
            this.smsGroup.Controls.Add(this.smsMsg);
            this.smsGroup.Controls.Add(this.smsTo);
            this.smsGroup.Enabled = false;
            this.smsGroup.Location = new System.Drawing.Point(13, 158);
            this.smsGroup.Name = "smsGroup";
            this.smsGroup.Size = new System.Drawing.Size(267, 100);
            this.smsGroup.TabIndex = 6;
            this.smsGroup.TabStop = false;
            this.smsGroup.Text = "SMS";
            // 
            // smsTo
            // 
            this.smsTo.Location = new System.Drawing.Point(7, 20);
            this.smsTo.Name = "smsTo";
            this.smsTo.Size = new System.Drawing.Size(100, 20);
            this.smsTo.TabIndex = 0;
            this.smsTo.Text = "To";
            // 
            // smsMsg
            // 
            this.smsMsg.Location = new System.Drawing.Point(7, 47);
            this.smsMsg.Multiline = true;
            this.smsMsg.Name = "smsMsg";
            this.smsMsg.Size = new System.Drawing.Size(254, 47);
            this.smsMsg.TabIndex = 1;
            this.smsMsg.TextChanged += new System.EventHandler(this.countChars);
            // 
            // smsChars
            // 
            this.smsChars.AutoSize = true;
            this.smsChars.Location = new System.Drawing.Point(130, 23);
            this.smsChars.Name = "smsChars";
            this.smsChars.Size = new System.Drawing.Size(36, 13);
            this.smsChars.TabIndex = 2;
            this.smsChars.Text = "0/160";
            // 
            // smsSend
            // 
            this.smsSend.Location = new System.Drawing.Point(186, 17);
            this.smsSend.Name = "smsSend";
            this.smsSend.Size = new System.Drawing.Size(75, 23);
            this.smsSend.TabIndex = 3;
            this.smsSend.Text = "Send";
            this.smsSend.UseVisualStyleBackColor = true;
            this.smsSend.Click += new System.EventHandler(this.smsSend_Click);
            // 
            // Visual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 270);
            this.Controls.Add(this.smsGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.callGroup);
            this.Controls.Add(this.loginPass);
            this.Controls.Add(this.loginEmail);
            this.Controls.Add(this.loginButton);
            this.Name = "Visual";
            this.Text = "Visual";
            this.callGroup.ResumeLayout(false);
            this.callGroup.PerformLayout();
            this.smsGroup.ResumeLayout(false);
            this.smsGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox loginEmail;
        private System.Windows.Forms.MaskedTextBox loginPass;
        private System.Windows.Forms.GroupBox callGroup;
        private System.Windows.Forms.Button callStart;
        private System.Windows.Forms.TextBox callFrom;
        private System.Windows.Forms.TextBox callTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox smsGroup;
        private System.Windows.Forms.Button smsSend;
        private System.Windows.Forms.Label smsChars;
        private System.Windows.Forms.TextBox smsMsg;
        private System.Windows.Forms.TextBox smsTo;
    }
}