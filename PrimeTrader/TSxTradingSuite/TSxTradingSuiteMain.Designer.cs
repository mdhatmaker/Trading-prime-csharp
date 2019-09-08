namespace PrimeTrader
{
    partial class TSxTradingSuiteMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TSxTradingSuiteMain));
            this.mainTabRegionBlank = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.plusButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mainTabRegionBlank
            // 
            this.mainTabRegionBlank.BackColor = System.Drawing.Color.Gray;
            this.mainTabRegionBlank.Location = new System.Drawing.Point(9, 161);
            this.mainTabRegionBlank.Margin = new System.Windows.Forms.Padding(2);
            this.mainTabRegionBlank.Name = "mainTabRegionBlank";
            this.mainTabRegionBlank.Size = new System.Drawing.Size(844, 93);
            this.mainTabRegionBlank.TabIndex = 1;
            // 
            // closeButton
            // 
            this.closeButton.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(1166, 3);
            this.closeButton.Margin = new System.Windows.Forms.Padding(0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(31, 24);
            this.closeButton.TabIndex = 8;
            this.closeButton.Text = "X";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(10, 5);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(52, 21);
            this.titleLabel.TabIndex = 9;
            this.titleLabel.Text = "label1";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.BackColor = System.Drawing.Color.Transparent;
            this.timeLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLabel.Location = new System.Drawing.Point(1134, 54);
            this.timeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(63, 21);
            this.timeLabel.TabIndex = 10;
            this.timeLabel.Text = "3:57pm";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.BackColor = System.Drawing.Color.Transparent;
            this.dateLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateLabel.Location = new System.Drawing.Point(9, 54);
            this.dateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(52, 21);
            this.dateLabel.TabIndex = 11;
            this.dateLabel.Text = "label3";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // plusButton
            // 
            this.plusButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.plusButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plusButton.Location = new System.Drawing.Point(24, 81);
            this.plusButton.Name = "plusButton";
            this.plusButton.Size = new System.Drawing.Size(35, 32);
            this.plusButton.TabIndex = 15;
            this.plusButton.Text = "+";
            this.plusButton.UseVisualStyleBackColor = false;
            this.plusButton.Click += new System.EventHandler(this.plusButton_Click);
            // 
            // TSxTradingSuiteMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1379, 766);
            this.Controls.Add(this.plusButton);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.mainTabRegionBlank);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TSxTradingSuiteMain";
            this.Text = "TraderSuite";
            this.Resize += new System.EventHandler(this.TSxTraderSuiteForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel mainTabRegionBlank;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button plusButton;
    }
}