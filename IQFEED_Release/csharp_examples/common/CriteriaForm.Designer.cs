namespace OptionChainCriteria
{
    partial class CriteriaForm
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
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbFutureSpreads = new System.Windows.Forms.RadioButton();
            this.rdbFutureOptions = new System.Windows.Forms.RadioButton();
            this.rdbFutures = new System.Windows.Forms.RadioButton();
            this.rdbEquityOptions = new System.Windows.Forms.RadioButton();
            this.lblNears = new System.Windows.Forms.Label();
            this.nudOOTM = new System.Windows.Forms.NumericUpDown();
            this.nudNears = new System.Windows.Forms.NumericUpDown();
            this.nudITM = new System.Windows.Forms.NumericUpDown();
            this.chkBinary = new System.Windows.Forms.CheckBox();
            this.chkDec = new System.Windows.Forms.CheckBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.chkOct = new System.Windows.Forms.CheckBox();
            this.GroupBox5 = new System.Windows.Forms.GroupBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.rdbMoney = new System.Windows.Forms.RadioButton();
            this.rdbStrike = new System.Windows.Forms.RadioButton();
            this.rdbNoFilter = new System.Windows.Forms.RadioButton();
            this.chkNears = new System.Windows.Forms.CheckBox();
            this.chkAug = new System.Windows.Forms.CheckBox();
            this.chkNov = new System.Windows.Forms.CheckBox();
            this.chkSep = new System.Windows.Forms.CheckBox();
            this.btnLookup = new System.Windows.Forms.Button();
            this.chkJul = new System.Windows.Forms.CheckBox();
            this.chkMay = new System.Windows.Forms.CheckBox();
            this.chkJun = new System.Windows.Forms.CheckBox();
            this.nudEndYear = new System.Windows.Forms.NumericUpDown();
            this.chkApr = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.nudStartYear = new System.Windows.Forms.NumericUpDown();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.chkCalls = new System.Windows.Forms.CheckBox();
            this.chkPuts = new System.Windows.Forms.CheckBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.chkMar = new System.Windows.Forms.CheckBox();
            this.chkFeb = new System.Windows.Forms.CheckBox();
            this.chkJan = new System.Windows.Forms.CheckBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOOTM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNears)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudITM)).BeginInit();
            this.GroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndYear)).BeginInit();
            this.GroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartYear)).BeginInit();
            this.GroupBox3.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.rdbFutureSpreads);
            this.GroupBox1.Controls.Add(this.rdbFutureOptions);
            this.GroupBox1.Controls.Add(this.rdbFutures);
            this.GroupBox1.Controls.Add(this.rdbEquityOptions);
            this.GroupBox1.Location = new System.Drawing.Point(12, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(441, 47);
            this.GroupBox1.TabIndex = 7;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Generate";
            // 
            // rdbFutureSpreads
            // 
            this.rdbFutureSpreads.AutoSize = true;
            this.rdbFutureSpreads.Location = new System.Drawing.Point(319, 19);
            this.rdbFutureSpreads.Name = "rdbFutureSpreads";
            this.rdbFutureSpreads.Size = new System.Drawing.Size(97, 17);
            this.rdbFutureSpreads.TabIndex = 3;
            this.rdbFutureSpreads.TabStop = true;
            this.rdbFutureSpreads.Text = "Future Spreads";
            this.rdbFutureSpreads.UseVisualStyleBackColor = true;
            this.rdbFutureSpreads.CheckedChanged += new System.EventHandler(this.rdbFutureSpreads_CheckedChanged);
            // 
            // rdbFutureOptions
            // 
            this.rdbFutureOptions.AutoSize = true;
            this.rdbFutureOptions.Location = new System.Drawing.Point(209, 19);
            this.rdbFutureOptions.Name = "rdbFutureOptions";
            this.rdbFutureOptions.Size = new System.Drawing.Size(94, 17);
            this.rdbFutureOptions.TabIndex = 2;
            this.rdbFutureOptions.TabStop = true;
            this.rdbFutureOptions.Text = "Future Options";
            this.rdbFutureOptions.UseVisualStyleBackColor = true;
            this.rdbFutureOptions.CheckedChanged += new System.EventHandler(this.rdbFutureOptions_CheckedChanged);
            // 
            // rdbFutures
            // 
            this.rdbFutures.AutoSize = true;
            this.rdbFutures.Location = new System.Drawing.Point(133, 19);
            this.rdbFutures.Name = "rdbFutures";
            this.rdbFutures.Size = new System.Drawing.Size(60, 17);
            this.rdbFutures.TabIndex = 1;
            this.rdbFutures.TabStop = true;
            this.rdbFutures.Text = "Futures";
            this.rdbFutures.UseVisualStyleBackColor = true;
            this.rdbFutures.CheckedChanged += new System.EventHandler(this.rdbFutures_CheckedChanged);
            // 
            // rdbEquityOptions
            // 
            this.rdbEquityOptions.AutoSize = true;
            this.rdbEquityOptions.Location = new System.Drawing.Point(24, 19);
            this.rdbEquityOptions.Name = "rdbEquityOptions";
            this.rdbEquityOptions.Size = new System.Drawing.Size(93, 17);
            this.rdbEquityOptions.TabIndex = 0;
            this.rdbEquityOptions.TabStop = true;
            this.rdbEquityOptions.Text = "Equity Options";
            this.rdbEquityOptions.UseVisualStyleBackColor = true;
            this.rdbEquityOptions.CheckedChanged += new System.EventHandler(this.rdbEquityOptions_CheckedChanged);
            // 
            // lblNears
            // 
            this.lblNears.AutoSize = true;
            this.lblNears.Location = new System.Drawing.Point(103, 104);
            this.lblNears.Name = "lblNears";
            this.lblNears.Size = new System.Drawing.Size(90, 13);
            this.lblNears.TabIndex = 16;
            this.lblNears.Text = "# of Near Months";
            // 
            // nudOOTM
            // 
            this.nudOOTM.Location = new System.Drawing.Point(393, 66);
            this.nudOOTM.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudOOTM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOOTM.Name = "nudOOTM";
            this.nudOOTM.Size = new System.Drawing.Size(39, 20);
            this.nudOOTM.TabIndex = 12;
            this.nudOOTM.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudNears
            // 
            this.nudNears.Location = new System.Drawing.Point(194, 102);
            this.nudNears.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudNears.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNears.Name = "nudNears";
            this.nudNears.Size = new System.Drawing.Size(43, 20);
            this.nudNears.TabIndex = 15;
            this.nudNears.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudITM
            // 
            this.nudITM.Location = new System.Drawing.Point(249, 66);
            this.nudITM.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudITM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudITM.Name = "nudITM";
            this.nudITM.Size = new System.Drawing.Size(39, 20);
            this.nudITM.TabIndex = 11;
            this.nudITM.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkBinary
            // 
            this.chkBinary.AutoSize = true;
            this.chkBinary.Location = new System.Drawing.Point(10, 125);
            this.chkBinary.Name = "chkBinary";
            this.chkBinary.Size = new System.Drawing.Size(137, 17);
            this.chkBinary.TabIndex = 14;
            this.chkBinary.Text = "Remove Binary Options";
            this.chkBinary.UseVisualStyleBackColor = true;
            // 
            // chkDec
            // 
            this.chkDec.AutoSize = true;
            this.chkDec.Location = new System.Drawing.Point(195, 79);
            this.chkDec.Name = "chkDec";
            this.chkDec.Size = new System.Drawing.Size(46, 17);
            this.chkDec.TabIndex = 12;
            this.chkDec.Text = "Dec";
            this.chkDec.UseVisualStyleBackColor = true;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Label7.Location = new System.Drawing.Point(302, 62);
            this.Label7.MaximumSize = new System.Drawing.Size(95, 26);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(89, 26);
            this.Label7.TabIndex = 10;
            this.Label7.Text = "# of Out Of The Money contracts:";
            // 
            // chkOct
            // 
            this.chkOct.AutoSize = true;
            this.chkOct.Location = new System.Drawing.Point(94, 79);
            this.chkOct.Name = "chkOct";
            this.chkOct.Size = new System.Drawing.Size(43, 17);
            this.chkOct.TabIndex = 11;
            this.chkOct.Text = "Oct";
            this.chkOct.UseVisualStyleBackColor = true;
            // 
            // GroupBox5
            // 
            this.GroupBox5.Controls.Add(this.nudOOTM);
            this.GroupBox5.Controls.Add(this.nudITM);
            this.GroupBox5.Controls.Add(this.Label7);
            this.GroupBox5.Controls.Add(this.Label6);
            this.GroupBox5.Controls.Add(this.Label5);
            this.GroupBox5.Controls.Add(this.Label4);
            this.GroupBox5.Controls.Add(this.txtTo);
            this.GroupBox5.Controls.Add(this.txtFrom);
            this.GroupBox5.Controls.Add(this.rdbMoney);
            this.GroupBox5.Controls.Add(this.rdbStrike);
            this.GroupBox5.Controls.Add(this.rdbNoFilter);
            this.GroupBox5.Location = new System.Drawing.Point(12, 225);
            this.GroupBox5.Name = "GroupBox5";
            this.GroupBox5.Size = new System.Drawing.Size(441, 95);
            this.GroupBox5.TabIndex = 11;
            this.GroupBox5.TabStop = false;
            this.GroupBox5.Text = "Filters";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Label6.Location = new System.Drawing.Point(152, 62);
            this.Label6.MaximumSize = new System.Drawing.Size(95, 26);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(95, 26);
            this.Label6.TabIndex = 9;
            this.Label6.Text = "# of In The Money contracts:";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(327, 42);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(20, 13);
            this.Label5.TabIndex = 6;
            this.Label5.Text = "To";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(173, 42);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(30, 13);
            this.Label4.TabIndex = 5;
            this.Label4.Text = "From";
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(353, 39);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(79, 20);
            this.txtTo.TabIndex = 4;
            this.txtTo.Text = "0";
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(209, 39);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(79, 20);
            this.txtFrom.TabIndex = 3;
            this.txtFrom.Text = "0";
            // 
            // rdbMoney
            // 
            this.rdbMoney.AutoSize = true;
            this.rdbMoney.Location = new System.Drawing.Point(17, 66);
            this.rdbMoney.Name = "rdbMoney";
            this.rdbMoney.Size = new System.Drawing.Size(121, 17);
            this.rdbMoney.TabIndex = 2;
            this.rdbMoney.TabStop = true;
            this.rdbMoney.Text = "In/Out of the Money";
            this.rdbMoney.UseVisualStyleBackColor = true;
            this.rdbMoney.CheckedChanged += new System.EventHandler(this.rdbMoney_CheckedChanged);
            // 
            // rdbStrike
            // 
            this.rdbStrike.AutoSize = true;
            this.rdbStrike.Location = new System.Drawing.Point(17, 40);
            this.rdbStrike.Name = "rdbStrike";
            this.rdbStrike.Size = new System.Drawing.Size(87, 17);
            this.rdbStrike.TabIndex = 1;
            this.rdbStrike.TabStop = true;
            this.rdbStrike.Text = "Strike Range";
            this.rdbStrike.UseVisualStyleBackColor = true;
            this.rdbStrike.CheckedChanged += new System.EventHandler(this.rdbStrike_CheckedChanged);
            // 
            // rdbNoFilter
            // 
            this.rdbNoFilter.AutoSize = true;
            this.rdbNoFilter.Location = new System.Drawing.Point(17, 17);
            this.rdbNoFilter.Name = "rdbNoFilter";
            this.rdbNoFilter.Size = new System.Drawing.Size(64, 17);
            this.rdbNoFilter.TabIndex = 0;
            this.rdbNoFilter.TabStop = true;
            this.rdbNoFilter.Text = "No Filter";
            this.rdbNoFilter.UseVisualStyleBackColor = true;
            this.rdbNoFilter.CheckedChanged += new System.EventHandler(this.rdbNoFilter_CheckedChanged);
            // 
            // chkNears
            // 
            this.chkNears.AutoSize = true;
            this.chkNears.Location = new System.Drawing.Point(10, 102);
            this.chkNears.Name = "chkNears";
            this.chkNears.Size = new System.Drawing.Size(87, 17);
            this.chkNears.TabIndex = 13;
            this.chkNears.Text = "Near Months";
            this.chkNears.UseVisualStyleBackColor = true;
            this.chkNears.CheckedChanged += new System.EventHandler(this.chkNears_CheckedChanged);
            // 
            // chkAug
            // 
            this.chkAug.AutoSize = true;
            this.chkAug.Location = new System.Drawing.Point(195, 56);
            this.chkAug.Name = "chkAug";
            this.chkAug.Size = new System.Drawing.Size(45, 17);
            this.chkAug.TabIndex = 10;
            this.chkAug.Text = "Aug";
            this.chkAug.UseVisualStyleBackColor = true;
            // 
            // chkNov
            // 
            this.chkNov.AutoSize = true;
            this.chkNov.Location = new System.Drawing.Point(143, 79);
            this.chkNov.Name = "chkNov";
            this.chkNov.Size = new System.Drawing.Size(46, 17);
            this.chkNov.TabIndex = 9;
            this.chkNov.Text = "Nov";
            this.chkNov.UseVisualStyleBackColor = true;
            // 
            // chkSep
            // 
            this.chkSep.AutoSize = true;
            this.chkSep.Location = new System.Drawing.Point(45, 79);
            this.chkSep.Name = "chkSep";
            this.chkSep.Size = new System.Drawing.Size(45, 17);
            this.chkSep.TabIndex = 8;
            this.chkSep.Text = "Sep";
            this.chkSep.UseVisualStyleBackColor = true;
            // 
            // btnLookup
            // 
            this.btnLookup.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLookup.Location = new System.Drawing.Point(115, 326);
            this.btnLookup.Name = "btnLookup";
            this.btnLookup.Size = new System.Drawing.Size(97, 28);
            this.btnLookup.TabIndex = 12;
            this.btnLookup.Text = "Lookup";
            this.btnLookup.UseVisualStyleBackColor = true;
            // 
            // chkJul
            // 
            this.chkJul.AutoSize = true;
            this.chkJul.Location = new System.Drawing.Point(143, 56);
            this.chkJul.Name = "chkJul";
            this.chkJul.Size = new System.Drawing.Size(39, 17);
            this.chkJul.TabIndex = 6;
            this.chkJul.Text = "Jul";
            this.chkJul.UseVisualStyleBackColor = true;
            // 
            // chkMay
            // 
            this.chkMay.AutoSize = true;
            this.chkMay.Location = new System.Drawing.Point(45, 56);
            this.chkMay.Name = "chkMay";
            this.chkMay.Size = new System.Drawing.Size(46, 17);
            this.chkMay.TabIndex = 7;
            this.chkMay.Text = "May";
            this.chkMay.UseVisualStyleBackColor = true;
            // 
            // chkJun
            // 
            this.chkJun.AutoSize = true;
            this.chkJun.Location = new System.Drawing.Point(94, 56);
            this.chkJun.Name = "chkJun";
            this.chkJun.Size = new System.Drawing.Size(43, 17);
            this.chkJun.TabIndex = 5;
            this.chkJun.Text = "Jun";
            this.chkJun.UseVisualStyleBackColor = true;
            // 
            // nudEndYear
            // 
            this.nudEndYear.Location = new System.Drawing.Point(97, 48);
            this.nudEndYear.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudEndYear.Name = "nudEndYear";
            this.nudEndYear.Size = new System.Drawing.Size(80, 20);
            this.nudEndYear.TabIndex = 1;
            // 
            // chkApr
            // 
            this.chkApr.AutoSize = true;
            this.chkApr.Location = new System.Drawing.Point(195, 33);
            this.chkApr.Name = "chkApr";
            this.chkApr.Size = new System.Drawing.Size(42, 17);
            this.chkApr.TabIndex = 4;
            this.chkApr.Text = "Apr";
            this.chkApr.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(247, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(97, 28);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.nudStartYear);
            this.GroupBox4.Controls.Add(this.Label3);
            this.GroupBox4.Controls.Add(this.Label2);
            this.GroupBox4.Controls.Add(this.nudEndYear);
            this.GroupBox4.Location = new System.Drawing.Point(12, 141);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(187, 78);
            this.GroupBox4.TabIndex = 10;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Expiration Year";
            // 
            // nudStartYear
            // 
            this.nudStartYear.Location = new System.Drawing.Point(97, 19);
            this.nudStartYear.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudStartYear.Name = "nudStartYear";
            this.nudStartYear.Size = new System.Drawing.Size(80, 20);
            this.nudStartYear.TabIndex = 4;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(26, 50);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(65, 13);
            this.Label3.TabIndex = 3;
            this.Label3.Text = "Ending Year";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(23, 22);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(68, 13);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Starting Year";
            // 
            // chkCalls
            // 
            this.chkCalls.AutoSize = true;
            this.chkCalls.Location = new System.Drawing.Point(99, 33);
            this.chkCalls.Name = "chkCalls";
            this.chkCalls.Size = new System.Drawing.Size(48, 17);
            this.chkCalls.TabIndex = 1;
            this.chkCalls.Text = "Calls";
            this.chkCalls.UseVisualStyleBackColor = true;
            // 
            // chkPuts
            // 
            this.chkPuts.AutoSize = true;
            this.chkPuts.Location = new System.Drawing.Point(22, 33);
            this.chkPuts.Name = "chkPuts";
            this.chkPuts.Size = new System.Drawing.Size(47, 17);
            this.chkPuts.TabIndex = 0;
            this.chkPuts.Text = "Puts";
            this.chkPuts.UseVisualStyleBackColor = true;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.lblNears);
            this.GroupBox3.Controls.Add(this.nudNears);
            this.GroupBox3.Controls.Add(this.chkBinary);
            this.GroupBox3.Controls.Add(this.chkNears);
            this.GroupBox3.Controls.Add(this.chkDec);
            this.GroupBox3.Controls.Add(this.chkOct);
            this.GroupBox3.Controls.Add(this.chkAug);
            this.GroupBox3.Controls.Add(this.chkNov);
            this.GroupBox3.Controls.Add(this.chkSep);
            this.GroupBox3.Controls.Add(this.chkMay);
            this.GroupBox3.Controls.Add(this.chkJul);
            this.GroupBox3.Controls.Add(this.chkJun);
            this.GroupBox3.Controls.Add(this.chkApr);
            this.GroupBox3.Controls.Add(this.chkMar);
            this.GroupBox3.Controls.Add(this.chkFeb);
            this.GroupBox3.Controls.Add(this.chkJan);
            this.GroupBox3.Controls.Add(this.chkAll);
            this.GroupBox3.Location = new System.Drawing.Point(207, 65);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(246, 154);
            this.GroupBox3.TabIndex = 9;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Expiration Months";
            // 
            // chkMar
            // 
            this.chkMar.AutoSize = true;
            this.chkMar.Location = new System.Drawing.Point(143, 33);
            this.chkMar.Name = "chkMar";
            this.chkMar.Size = new System.Drawing.Size(44, 17);
            this.chkMar.TabIndex = 3;
            this.chkMar.Text = "Mar";
            this.chkMar.UseVisualStyleBackColor = true;
            // 
            // chkFeb
            // 
            this.chkFeb.AutoSize = true;
            this.chkFeb.Location = new System.Drawing.Point(94, 33);
            this.chkFeb.Name = "chkFeb";
            this.chkFeb.Size = new System.Drawing.Size(44, 17);
            this.chkFeb.TabIndex = 2;
            this.chkFeb.Text = "Feb";
            this.chkFeb.UseVisualStyleBackColor = true;
            // 
            // chkJan
            // 
            this.chkJan.AutoSize = true;
            this.chkJan.Location = new System.Drawing.Point(45, 33);
            this.chkJan.Name = "chkJan";
            this.chkJan.Size = new System.Drawing.Size(43, 17);
            this.chkJan.TabIndex = 1;
            this.chkJan.Text = "Jan";
            this.chkJan.UseVisualStyleBackColor = true;
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(6, 56);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(37, 17);
            this.chkAll.TabIndex = 0;
            this.chkAll.Text = "All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.chkCalls);
            this.GroupBox2.Controls.Add(this.chkPuts);
            this.GroupBox2.Location = new System.Drawing.Point(12, 65);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(187, 75);
            this.GroupBox2.TabIndex = 8;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Type";
            // 
            // CriteriaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 364);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.GroupBox5);
            this.Controls.Add(this.btnLookup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.GroupBox4);
            this.Controls.Add(this.GroupBox3);
            this.Controls.Add(this.GroupBox2);
            this.Name = "CriteriaForm";
            this.Text = "C# Option Chain Criteria";
            this.Load += new System.EventHandler(this.CriteriaForm_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOOTM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNears)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudITM)).EndInit();
            this.GroupBox5.ResumeLayout(false);
            this.GroupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndYear)).EndInit();
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartYear)).EndInit();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.RadioButton rdbFutureSpreads;
        internal System.Windows.Forms.RadioButton rdbFutureOptions;
        internal System.Windows.Forms.RadioButton rdbFutures;
        internal System.Windows.Forms.RadioButton rdbEquityOptions;
        internal System.Windows.Forms.Label lblNears;
        internal System.Windows.Forms.NumericUpDown nudOOTM;
        internal System.Windows.Forms.NumericUpDown nudNears;
        internal System.Windows.Forms.NumericUpDown nudITM;
        internal System.Windows.Forms.CheckBox chkBinary;
        internal System.Windows.Forms.CheckBox chkDec;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.CheckBox chkOct;
        internal System.Windows.Forms.GroupBox GroupBox5;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox txtTo;
        internal System.Windows.Forms.TextBox txtFrom;
        internal System.Windows.Forms.RadioButton rdbMoney;
        internal System.Windows.Forms.RadioButton rdbStrike;
        internal System.Windows.Forms.RadioButton rdbNoFilter;
        internal System.Windows.Forms.CheckBox chkNears;
        internal System.Windows.Forms.CheckBox chkAug;
        internal System.Windows.Forms.CheckBox chkNov;
        internal System.Windows.Forms.CheckBox chkSep;
        internal System.Windows.Forms.Button btnLookup;
        internal System.Windows.Forms.CheckBox chkJul;
        internal System.Windows.Forms.CheckBox chkMay;
        internal System.Windows.Forms.CheckBox chkJun;
        internal System.Windows.Forms.NumericUpDown nudEndYear;
        internal System.Windows.Forms.CheckBox chkApr;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.GroupBox GroupBox4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.CheckBox chkCalls;
        internal System.Windows.Forms.CheckBox chkPuts;
        internal System.Windows.Forms.GroupBox GroupBox3;
        internal System.Windows.Forms.CheckBox chkMar;
        internal System.Windows.Forms.CheckBox chkFeb;
        internal System.Windows.Forms.CheckBox chkJan;
        internal System.Windows.Forms.CheckBox chkAll;
        internal System.Windows.Forms.GroupBox GroupBox2;
        private System.Windows.Forms.NumericUpDown nudStartYear;
    }
}