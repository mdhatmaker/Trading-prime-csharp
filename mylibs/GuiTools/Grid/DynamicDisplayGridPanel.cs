using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Tools;
using static GuiTools.GUi;

namespace GuiTools.Grid
{
    public class DynamicDisplayGridPanel
    {
        #region Underlying UI Components
        Panel parentPanel;
        Panel infoPanel;
        Panel gridPanel;
        DataGridView grid;
        Timer updateTimer;
        TableLayoutPanel tableLayoutPanel;
        Label lblInfo;
        Label lblDelay;
        CheckBox chkUpdatesEnabled;
        #endregion

        int m_infoPanelHeight = 30;
        DynamicDisplayGrid m_displayGrid;

        string m_name;
        Form m_parentForm;
        Color m_panelBackColor;
        TimeSpan m_gridUpdateDelay;
        DateTime m_lastGridUpdate;
        int m_millisecondsUpdateDelay;
        int m_timerPrecisionMilliseconds;

        public Func<Dictionary<string, IDataRow>> UpdateDictionaryFunction;
        public Func<List<IDataRow>> UpdateListFunction;

        public DynamicDisplayGridPanel(Panel parentPanel, string name, Color panelBackColor, int millisecondsUpdateDelay = 5000, int timerPrecisionMilliseconds = 1000)
        {
            this.parentPanel = parentPanel;

            // Find the Form that ultimately contains this panel
            m_parentForm = GetParentForm(parentPanel);

            m_name = name;
            m_panelBackColor = panelBackColor;
            m_gridUpdateDelay = new TimeSpan(0, 0, 0, 0, millisecondsUpdateDelay);
            m_lastGridUpdate = DateTime.Now;
            m_millisecondsUpdateDelay = millisecondsUpdateDelay;
            m_timerPrecisionMilliseconds = timerPrecisionMilliseconds;
        }

        public void InitializeColumns(string[] columns, Color cellBackColor)
        {
            m_displayGrid.InitializeColumns(columns, cellBackColor);
        }

        public void EnableUpdates(bool b)
        {
            Debug.WriteLine("SmallGridPanel::EnableUpdates: {0}", b);
            this.updateTimer.Enabled = b;
            if (b == true)
                PerformUpdate(true);
        }

        public void Initialize()
        {
            InitializeComponent();
            m_displayGrid = new DynamicDisplayGrid(this.grid, m_parentForm);
            this.updateTimer.Interval = m_timerPrecisionMilliseconds;
        }

        public void InitializeComponent()
        {
            this.infoPanel = new System.Windows.Forms.Panel();
            this.gridPanel = new System.Windows.Forms.Panel();
            this.grid = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblDelay = new System.Windows.Forms.Label();
            this.chkUpdatesEnabled = new System.Windows.Forms.CheckBox();
            this.updateTimer = new Timer();
            this.tableLayoutPanel.SuspendLayout();
            this.infoPanel.SuspendLayout();
            this.gridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.parentPanel.SuspendLayout();

            // 
            // parent Panel
            // 
            parentPanel.Controls.Add(this.infoPanel);
            parentPanel.Controls.Add(this.gridPanel);
            //parentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            //parentPanel.Location = new System.Drawing.Point(50, 50);
            //parentPanel.Name = m_name;
            //parentPanel.Size = new System.Drawing.Size(150, 50);
            //parentPanel.TabIndex = 5;

            int pwidth = this.parentPanel.Width;
            int pheight = this.parentPanel.Height;

            // 
            // infoPanel
            // 
            this.infoPanel.Controls.Add(this.tableLayoutPanel);
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoPanel.Location = new System.Drawing.Point(0, 0);
            this.infoPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.infoPanel.Name = "infoPanel" + m_name;
            this.infoPanel.Size = new System.Drawing.Size(150, m_infoPanelHeight);
            this.infoPanel.BackColor = m_panelBackColor;
            this.infoPanel.TabIndex = 21;

            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel.Controls.Add(this.lblInfo, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.lblDelay, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.chkUpdatesEnabled, 2, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel.Name = "tableLayoutPanel" + m_name;
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(pwidth, m_infoPanelHeight);
            this.tableLayoutPanel.TabIndex = 20;
            //
            // lblInfo
            //
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(2, 4);
            this.lblInfo.Name = "lblInfo" + m_name;
            this.lblInfo.Size = new System.Drawing.Size(46, 17);
            this.lblInfo.TabIndex = 34;
            this.lblInfo.Text = m_name;
            //
            // lblDelay
            //
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(2, 4);
            this.lblDelay.Name = "lblDelay" + m_name;
            this.lblDelay.Size = new System.Drawing.Size(46, 17);
            this.lblDelay.TabIndex = 34;
            this.lblDelay.Text = string.Format("delay: {0} seconds", m_gridUpdateDelay.Seconds);
            //
            // chkUpdatesEnabled
            //
            this.chkUpdatesEnabled.AutoSize = true;
            this.chkUpdatesEnabled.Checked = true;
            this.chkUpdatesEnabled.Location = new System.Drawing.Point(125, 4);
            this.chkUpdatesEnabled.Name = "chkUpdatesEnabled" + m_name;
            this.chkUpdatesEnabled.Size = new System.Drawing.Size(98, 41);
            this.chkUpdatesEnabled.TabIndex = 33;
            this.chkUpdatesEnabled.Text = "";
            this.chkUpdatesEnabled.UseVisualStyleBackColor = true;
            this.chkUpdatesEnabled.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            // 
            // gridPanel
            // 
            this.gridPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPanel.Controls.Add(this.grid);
            //this.gridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPanel.Location = new System.Drawing.Point(0, m_infoPanelHeight + 1);
            this.gridPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridPanel.Name = "gridPanel" + m_name;
            //this.gridPanel.Size = new System.Drawing.Size(150, 80);
            this.gridPanel.Size = new System.Drawing.Size(pwidth, pheight - m_infoPanelHeight);
            this.gridPanel.TabIndex = 22;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid" + m_name;
            this.grid.ReadOnly = true;
            this.grid.RowTemplate.Height = 28;
            this.grid.Size = new System.Drawing.Size(145, 45);
            this.grid.TabIndex = 23;
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = false;
            this.updateTimer.Interval = 1000;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);

            this.tableLayoutPanel.ResumeLayout(false);
            this.infoPanel.ResumeLayout(false);
            this.gridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.parentPanel.ResumeLayout(false);
            this.parentPanel.PerformLayout();
        }

        private void PerformUpdate(bool forceUpdate = false)
        {
            if (forceUpdate || DateTime.Now >= m_lastGridUpdate.Add(m_gridUpdateDelay))
            {
                //dout("FIRE {0}!!!   {1}", m_name, DateTime.Now.ToShortTimeString());
                if (UpdateDictionaryFunction != null)
                {
                    // TODO: For threading, ensure that we don't launch this task if the previously launched version of this task is still running!!!
                    Task.Factory.StartNew(() =>
                    {
                        Dictionary<string, IDataRow> d2 = UpdateDictionaryFunction();
                        m_displayGrid.UpdatePrices(d2);
                    });
                    //Dictionary<string, IDataRow> d2 = UpdateDictionaryFunction();
                    //m_displayGrid.UpdatePrices(d2);
                }
                else if (UpdateListFunction != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        List<IDataRow> li = UpdateListFunction();
                        m_displayGrid.UpdatePrices(li);
                    });
                    //List<IDataRow> li = UpdateListFunction();
                    //m_displayGrid.UpdatePrices(li);
                }

                m_lastGridUpdate = DateTime.Now;
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            PerformUpdate();
        }

    } // end of CLASS

} // end of NAMESPACE
