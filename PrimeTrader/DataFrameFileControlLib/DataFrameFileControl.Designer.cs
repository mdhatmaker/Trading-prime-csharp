namespace DataFrameFileControlLib
{
    partial class DataFrameFileControl
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
            this.panelDataFrameFile = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClearFilterDF = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.btnChooseFolderDF = new System.Windows.Forms.Button();
            this.txtHistoricalFolder = new System.Windows.Forms.TextBox();
            this.lblDataFileCount = new System.Windows.Forms.Label();
            this.lvDataFiles = new System.Windows.Forms.ListView();
            this.columnFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label18 = new System.Windows.Forms.Label();
            this.folderBrowserDialogDF = new System.Windows.Forms.FolderBrowserDialog();
            this.panelDataFrameFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDataFrameFile
            // 
            this.panelDataFrameFile.Controls.Add(this.label1);
            this.panelDataFrameFile.Controls.Add(this.btnClearFilterDF);
            this.panelDataFrameFile.Controls.Add(this.txtFilter);
            this.panelDataFrameFile.Controls.Add(this.btnChooseFolderDF);
            this.panelDataFrameFile.Controls.Add(this.txtHistoricalFolder);
            this.panelDataFrameFile.Controls.Add(this.lblDataFileCount);
            this.panelDataFrameFile.Controls.Add(this.lvDataFiles);
            this.panelDataFrameFile.Controls.Add(this.label18);
            this.panelDataFrameFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDataFrameFile.Location = new System.Drawing.Point(0, 0);
            this.panelDataFrameFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelDataFrameFile.Name = "panelDataFrameFile";
            this.panelDataFrameFile.Size = new System.Drawing.Size(373, 430);
            this.panelDataFrameFile.TabIndex = 148;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 159;
            this.label1.Text = "Filter:";
            // 
            // btnClearFilterDF
            // 
            this.btnClearFilterDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFilterDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearFilterDF.Location = new System.Drawing.Point(311, 6);
            this.btnClearFilterDF.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClearFilterDF.Name = "btnClearFilterDF";
            this.btnClearFilterDF.Size = new System.Drawing.Size(52, 30);
            this.btnClearFilterDF.TabIndex = 158;
            this.btnClearFilterDF.Text = "Clear";
            this.btnClearFilterDF.UseVisualStyleBackColor = true;
            this.btnClearFilterDF.Click += new System.EventHandler(this.btnClearFilterDF_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(52, 8);
            this.txtFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(249, 22);
            this.txtFilter.TabIndex = 157;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // btnChooseFolderDF
            // 
            this.btnChooseFolderDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseFolderDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseFolderDF.Location = new System.Drawing.Point(324, 405);
            this.btnChooseFolderDF.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChooseFolderDF.Name = "btnChooseFolderDF";
            this.btnChooseFolderDF.Size = new System.Drawing.Size(43, 21);
            this.btnChooseFolderDF.TabIndex = 156;
            this.btnChooseFolderDF.Text = "...";
            this.btnChooseFolderDF.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnChooseFolderDF.UseVisualStyleBackColor = true;
            this.btnChooseFolderDF.Click += new System.EventHandler(this.btnChooseFolderDF_Click);
            // 
            // txtHistoricalFolder
            // 
            this.txtHistoricalFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHistoricalFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHistoricalFolder.ForeColor = System.Drawing.Color.Black;
            this.txtHistoricalFolder.Location = new System.Drawing.Point(3, 406);
            this.txtHistoricalFolder.Name = "txtHistoricalFolder";
            this.txtHistoricalFolder.ReadOnly = true;
            this.txtHistoricalFolder.Size = new System.Drawing.Size(316, 21);
            this.txtHistoricalFolder.TabIndex = 155;
            this.txtHistoricalFolder.Text = "D:\\Users\\mhatmaker\\Dropbox\\dev\\data\\DF_DATA";
            // 
            // lblDataFileCount
            // 
            this.lblDataFileCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataFileCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataFileCount.Location = new System.Drawing.Point(287, 31);
            this.lblDataFileCount.Name = "lblDataFileCount";
            this.lblDataFileCount.Size = new System.Drawing.Size(81, 30);
            this.lblDataFileCount.TabIndex = 154;
            this.lblDataFileCount.Text = "0 files";
            this.lblDataFileCount.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lvDataFiles
            // 
            this.lvDataFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDataFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFilename,
            this.columnDate});
            this.lvDataFiles.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDataFiles.Location = new System.Drawing.Point(3, 63);
            this.lvDataFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lvDataFiles.MultiSelect = false;
            this.lvDataFiles.Name = "lvDataFiles";
            this.lvDataFiles.Size = new System.Drawing.Size(366, 338);
            this.lvDataFiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvDataFiles.TabIndex = 153;
            this.lvDataFiles.UseCompatibleStateImageBehavior = false;
            this.lvDataFiles.View = System.Windows.Forms.View.Details;
            this.lvDataFiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvDataFiles_ColumnClick);
            // 
            // columnFilename
            // 
            this.columnFilename.Text = "Filename";
            this.columnFilename.Width = 270;
            // 
            // columnDate
            // 
            this.columnDate.Text = "Last Update";
            this.columnDate.Width = 90;
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(3, 31);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(342, 30);
            this.label18.TabIndex = 150;
            this.label18.Text = "Data Files";
            this.label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // DataFrameFileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDataFrameFile);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DataFrameFileControl";
            this.Size = new System.Drawing.Size(373, 430);
            this.panelDataFrameFile.ResumeLayout(false);
            this.panelDataFrameFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDataFrameFile;
        private System.Windows.Forms.TextBox txtHistoricalFolder;
        private System.Windows.Forms.Label lblDataFileCount;
        private System.Windows.Forms.ListView lvDataFiles;
        private System.Windows.Forms.ColumnHeader columnFilename;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClearFilterDF;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button btnChooseFolderDF;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogDF;
    }
}
