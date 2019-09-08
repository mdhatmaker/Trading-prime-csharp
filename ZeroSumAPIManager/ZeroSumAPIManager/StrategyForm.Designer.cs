namespace ZeroSumAPI
{
    partial class StrategyForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.cboTradingEngines = new System.Windows.Forms.ComboBox();
            this.btnRunStrategy = new System.Windows.Forms.Button();
            this.rtbStrategyOutput = new System.Windows.Forms.RichTextBox();
            this.listStrategyMethods = new System.Windows.Forms.ListBox();
            this.timerStrategyManager = new System.Windows.Forms.Timer(this.components);
            this.rtbStrategyCode = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(587, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 19);
            this.label4.TabIndex = 15;
            this.label4.Text = "Trading Engine:";
            // 
            // cboTradingEngines
            // 
            this.cboTradingEngines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTradingEngines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTradingEngines.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTradingEngines.FormattingEnabled = true;
            this.cboTradingEngines.Location = new System.Drawing.Point(726, 21);
            this.cboTradingEngines.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboTradingEngines.Name = "cboTradingEngines";
            this.cboTradingEngines.Size = new System.Drawing.Size(158, 33);
            this.cboTradingEngines.TabIndex = 14;
            // 
            // btnRunStrategy
            // 
            this.btnRunStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunStrategy.Location = new System.Drawing.Point(1002, 11);
            this.btnRunStrategy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRunStrategy.Name = "btnRunStrategy";
            this.btnRunStrategy.Size = new System.Drawing.Size(159, 51);
            this.btnRunStrategy.TabIndex = 13;
            this.btnRunStrategy.Text = "Run Strategy";
            this.btnRunStrategy.UseVisualStyleBackColor = true;
            this.btnRunStrategy.Click += new System.EventHandler(this.btnRunStrategy_Click);
            // 
            // rtbStrategyOutput
            // 
            this.rtbStrategyOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbStrategyOutput.BackColor = System.Drawing.Color.Black;
            this.rtbStrategyOutput.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbStrategyOutput.ForeColor = System.Drawing.Color.LawnGreen;
            this.rtbStrategyOutput.Location = new System.Drawing.Point(27, 594);
            this.rtbStrategyOutput.Margin = new System.Windows.Forms.Padding(2);
            this.rtbStrategyOutput.Name = "rtbStrategyOutput";
            this.rtbStrategyOutput.Size = new System.Drawing.Size(1211, 381);
            this.rtbStrategyOutput.TabIndex = 12;
            this.rtbStrategyOutput.Text = "";
            // 
            // listStrategyMethods
            // 
            this.listStrategyMethods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listStrategyMethods.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listStrategyMethods.FormattingEnabled = true;
            this.listStrategyMethods.ItemHeight = 19;
            this.listStrategyMethods.Location = new System.Drawing.Point(921, 71);
            this.listStrategyMethods.Name = "listStrategyMethods";
            this.listStrategyMethods.Size = new System.Drawing.Size(317, 498);
            this.listStrategyMethods.Sorted = true;
            this.listStrategyMethods.TabIndex = 16;
            this.listStrategyMethods.SelectedIndexChanged += new System.EventHandler(this.listStrategyMethods_SelectedIndexChanged);
            // 
            // timerStrategyManager
            // 
            this.timerStrategyManager.Enabled = true;
            this.timerStrategyManager.Interval = 500;
            this.timerStrategyManager.Tick += new System.EventHandler(this.timerStrategyManager_Tick);
            // 
            // rtbStrategyCode
            // 
            this.rtbStrategyCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbStrategyCode.BackColor = System.Drawing.Color.White;
            this.rtbStrategyCode.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbStrategyCode.ForeColor = System.Drawing.Color.DarkBlue;
            this.rtbStrategyCode.Location = new System.Drawing.Point(27, 71);
            this.rtbStrategyCode.Margin = new System.Windows.Forms.Padding(2);
            this.rtbStrategyCode.Name = "rtbStrategyCode";
            this.rtbStrategyCode.Size = new System.Drawing.Size(870, 498);
            this.rtbStrategyCode.TabIndex = 17;
            this.rtbStrategyCode.Text = "";
            // 
            // StrategyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1263, 1000);
            this.Controls.Add(this.rtbStrategyCode);
            this.Controls.Add(this.listStrategyMethods);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboTradingEngines);
            this.Controls.Add(this.btnRunStrategy);
            this.Controls.Add(this.rtbStrategyOutput);
            this.Name = "StrategyForm";
            this.Text = "Strategy Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboTradingEngines;
        private System.Windows.Forms.Button btnRunStrategy;
        private System.Windows.Forms.RichTextBox rtbStrategyOutput;
        private System.Windows.Forms.ListBox listStrategyMethods;
        private System.Windows.Forms.Timer timerStrategyManager;
        private System.Windows.Forms.RichTextBox rtbStrategyCode;
    }
}