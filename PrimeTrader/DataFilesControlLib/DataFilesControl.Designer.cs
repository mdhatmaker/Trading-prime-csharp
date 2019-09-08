namespace DataFilesControlLib
{
    partial class DataFilesControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelDataControl = new System.Windows.Forms.Panel();
            this.txtHistoricalFolder = new System.Windows.Forms.TextBox();
            this.lblDataFileCount = new System.Windows.Forms.Label();
            this.lvDataFiles = new System.Windows.Forms.ListView();
            this.columnFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.listDataTimeFrames = new System.Windows.Forms.ListBox();
            this.listDataSymbols = new System.Windows.Forms.ListBox();
            this.panelDataControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDataControl
            // 
            this.panelDataControl.Controls.Add(this.txtHistoricalFolder);
            this.panelDataControl.Controls.Add(this.lblDataFileCount);
            this.panelDataControl.Controls.Add(this.lvDataFiles);
            this.panelDataControl.Controls.Add(this.label20);
            this.panelDataControl.Controls.Add(this.label19);
            this.panelDataControl.Controls.Add(this.label18);
            this.panelDataControl.Controls.Add(this.listDataTimeFrames);
            this.panelDataControl.Controls.Add(this.listDataSymbols);
            this.panelDataControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDataControl.Location = new System.Drawing.Point(0, 0);
            this.panelDataControl.Name = "panelDataControl";
            this.panelDataControl.Size = new System.Drawing.Size(660, 537);
            this.panelDataControl.TabIndex = 148;
            // 
            // txtHistoricalFolder
            // 
            this.txtHistoricalFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtHistoricalFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHistoricalFolder.ForeColor = System.Drawing.Color.Black;
            this.txtHistoricalFolder.Location = new System.Drawing.Point(249, 467);
            this.txtHistoricalFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHistoricalFolder.Name = "txtHistoricalFolder";
            this.txtHistoricalFolder.ReadOnly = true;
            this.txtHistoricalFolder.Size = new System.Drawing.Size(401, 23);
            this.txtHistoricalFolder.TabIndex = 155;
            this.txtHistoricalFolder.Text = "D:\\Users\\mhatmaker\\Dropbox\\dev\\data\\DF_DATA";
            // 
            // lblDataFileCount
            // 
            this.lblDataFileCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataFileCount.Location = new System.Drawing.Point(557, -1);
            this.lblDataFileCount.Name = "lblDataFileCount";
            this.lblDataFileCount.Size = new System.Drawing.Size(91, 38);
            this.lblDataFileCount.TabIndex = 154;
            this.lblDataFileCount.Text = "0 files";
            this.lblDataFileCount.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lvDataFiles
            // 
            this.lvDataFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvDataFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFilename,
            this.columnDate});
            this.lvDataFiles.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDataFiles.Location = new System.Drawing.Point(249, 39);
            this.lvDataFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lvDataFiles.Name = "lvDataFiles";
            this.lvDataFiles.Size = new System.Drawing.Size(399, 422);
            this.lvDataFiles.TabIndex = 153;
            this.lvDataFiles.UseCompatibleStateImageBehavior = false;
            this.lvDataFiles.View = System.Windows.Forms.View.Details;
            this.lvDataFiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvDataFiles_ColumnClick);
            // 
            // columnFilename
            // 
            this.columnFilename.Text = "Filename";
            this.columnFilename.Width = 165;
            // 
            // columnDate
            // 
            this.columnDate.Text = "Last Update";
            this.columnDate.Width = 80;
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(42, 234);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(150, 30);
            this.label20.TabIndex = 152;
            this.label20.Text = "Symbols";
            this.label20.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(42, 27);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(150, 30);
            this.label19.TabIndex = 151;
            this.label19.Text = "Time Frames";
            this.label19.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(249, -1);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(385, 38);
            this.label18.TabIndex = 150;
            this.label18.Text = "Data Files";
            this.label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // listDataTimeFrames
            // 
            this.listDataTimeFrames.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listDataTimeFrames.FormattingEnabled = true;
            this.listDataTimeFrames.ItemHeight = 19;
            this.listDataTimeFrames.Location = new System.Drawing.Point(6, 60);
            this.listDataTimeFrames.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listDataTimeFrames.Name = "listDataTimeFrames";
            this.listDataTimeFrames.Size = new System.Drawing.Size(230, 156);
            this.listDataTimeFrames.Sorted = true;
            this.listDataTimeFrames.TabIndex = 149;
            this.listDataTimeFrames.SelectedIndexChanged += new System.EventHandler(this.listDataTimeFrames_SelectedIndexChanged);
            // 
            // listDataSymbols
            // 
            this.listDataSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listDataSymbols.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listDataSymbols.FormattingEnabled = true;
            this.listDataSymbols.ItemHeight = 19;
            this.listDataSymbols.Location = new System.Drawing.Point(6, 267);
            this.listDataSymbols.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listDataSymbols.Name = "listDataSymbols";
            this.listDataSymbols.Size = new System.Drawing.Size(230, 194);
            this.listDataSymbols.Sorted = true;
            this.listDataSymbols.TabIndex = 148;
            this.listDataSymbols.SelectedIndexChanged += new System.EventHandler(this.listDataSymbols_SelectedIndexChanged);
            // 
            // DataFilesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDataControl);
            this.Name = "DataFilesControl";
            this.Size = new System.Drawing.Size(660, 537);
            this.panelDataControl.ResumeLayout(false);
            this.panelDataControl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDataControl;
        private System.Windows.Forms.TextBox txtHistoricalFolder;
        private System.Windows.Forms.Label lblDataFileCount;
        private System.Windows.Forms.ListView lvDataFiles;
        private System.Windows.Forms.ColumnHeader columnFilename;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ListBox listDataTimeFrames;
        private System.Windows.Forms.ListBox listDataSymbols;
    }
}
