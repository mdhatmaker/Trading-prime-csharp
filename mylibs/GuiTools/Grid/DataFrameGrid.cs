using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;
using static GuiTools.GPlot;

namespace GuiTools.Grid
{
    public class DataFrameGrid
    {
        public DataGridView Grid { get { return m_grid; } }
        public int RowCount { get { return m_grid.Rows.Count; } }
        public int ColumnCount { get { return m_grid.Columns.Count; } }

        public DataGridViewCellEventHandler CellValueChanged;

        DataGridView m_grid;

        int m_interactiveRowCount;
        int m_colorRowIndex;
        int m_styleRowIndex;
        int m_checkboxRowIndex;

        public DataFrameGrid(DataGridView grid)
        {
            m_grid = grid;

            //Set Double buffering on the Grid using reflection and the bindingflags enum.
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, m_grid, new object[] { true });
        }

        public object this[int rix, int cix] { get { return m_grid[cix, rix].Value; } }      // NOTE: DataGridView indexes column-first (we use row-first)

        public string HeaderText(int ix)
        {
            return m_grid.Columns[ix].HeaderText;
        }

        public void InitializeColumns(string[] columns, Color cellBackColor)
        {
            if (m_grid.InvokeRequired) m_grid.Invoke(new Action<string[], Color>(InitializeColumns), columns, cellBackColor);
            else
            {
                m_grid.Columns.Clear();
                for (int i = 0; i < columns.Length; ++i)
                {
                    DataGridViewColumn col = new DataGridViewColumn();
                    col.HeaderText = columns[i];
                    DataGridViewCell cell = new DataGridViewTextBoxCell();
                    cell.Style.BackColor = cellBackColor;
                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.MinimumWidth = 70;
                    col.CellTemplate = cell;
                    m_grid.Columns.Add(col);
                }
                m_grid.RowHeadersVisible = false;
                m_grid.RowTemplate.Height = m_grid.Font.Height + 4;
                m_grid.AllowUserToAddRows = false;
                m_grid.AllowUserToDeleteRows = false;
                m_grid.AllowUserToResizeRows = false;

                m_grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                m_grid.AllowUserToResizeColumns = true;
                m_grid.AllowUserToOrderColumns = false;

                //SetInteractiveRows();
            }
        }

        public void ClearAllCheckboxes()
        {
            for (int i = 0; i < m_grid.Columns.Count; ++i)
            {
                var chk = m_grid[i, m_checkboxRowIndex] as DataGridViewCheckBoxCell;
                if (chk.Value == chk.TrueValue)
                {
                    chk.Value = chk.FalseValue;
                }
            }
            m_grid.EndEdit();   // this causes the grid to actually update its display (i.e. uncheck the checkbox)
        }

        private void SetInteractiveRows()
        {
            m_interactiveRowCount = 0;

            //DataGridViewButtonCell
            DataGridViewRow row;

            /*row = new DataGridViewRow();
            for (int i = 0; i < m_grid.Columns.Count; ++i)
            {
                DataGridViewButtonCell cell = new DataGridViewButtonCell();
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cell.Value = ".";
                row.Cells.Add(cell);
            }
            m_grid.Rows.Add(row);
            m_interactiveRowCount++;*/

            /*// LINE STYLE DROPDOWN cells
            row = new DataGridViewRow();
            for (int i = 0; i < m_grid.Columns.Count; ++i)
            {
                DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                foreach (string s in PlotLineStyles.Keys)
                {
                    cell.Items.Add(s);
                }
                cell.Value = cell.Items[0];
                row.Cells.Add(cell);
            }
            m_styleRowIndex = m_grid.Rows.Count;
            m_grid.Rows.Add(row);
            m_interactiveRowCount++;

            // COLOR DROPDOWN cells
            row = new DataGridViewRow();
            for (int i = 0; i < m_grid.Columns.Count; ++i)
            {
                DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                foreach (string c in PlotColors.Keys)
                {
                    cell.Items.Add(c);
                }
                cell.Value = cell.Items[0];
                row.Cells.Add(cell);
            }
            m_colorRowIndex = m_grid.Rows.Count;
            m_grid.Rows.Add(row);
            m_interactiveRowCount++;*/

            // CHECKBOX cells
            row = new DataGridViewRow();
            row.DefaultCellStyle.SelectionBackColor = m_grid.DefaultCellStyle.BackColor;        // these two lines stop the "BLUE SQUARE" from
            row.DefaultCellStyle.SelectionForeColor = m_grid.DefaultCellStyle.ForeColor;        // appearing when you click a checkbox cell
            for (int i = 0; i < m_grid.Columns.Count; ++i)
            {
                DataGridViewCheckBoxCell cell = new DataGridViewCheckBoxCell();
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cell.TrueValue = true;
                cell.FalseValue = false;
                cell.Value = false;
                row.Cells.Add(cell);
            }
            m_checkboxRowIndex = m_grid.Rows.Count;
            m_grid.Rows.Add(row);
            m_interactiveRowCount++;

            m_grid.CellClick += grid_CellClick;
            m_grid.CellContentClick += grid_CellContentClick;
            m_grid.CellValueChanged += grid_CellValueChanged;
            m_grid.EditingControlShowing += grid_EditingControlShowing;
            m_grid.CurrentCellDirtyStateChanged += grid_CurrentCellDirtyStateChanged;
        }

        private void grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (m_grid.IsCurrentCellDirty)
            {
                m_grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox comboBox = e.Control as ComboBox;
                comboBox.SelectedIndexChanged -= ComboSelectionChanged;
                comboBox.SelectedIndexChanged += ComboSelectionChanged;
            }
            //else
            //    Console.WriteLine(e.Control.GetType().ToString());
        }

        private void ComboSelectionChanged(object sender, EventArgs e)
        {
            var currentCell = m_grid.CurrentCellAddress;
            var sendingCB = sender as DataGridViewComboBoxEditingControl;
            if (currentCell.Y == m_colorRowIndex)
            {
                string colorStr = sendingCB.SelectedItem.ToString();
                m_grid[currentCell.X, m_checkboxRowIndex].Style.BackColor = PlotColors[colorStr];
            }
            else if (currentCell.Y == m_styleRowIndex)
            {
                string styleStr = sendingCB.SelectedItem.ToString();
                if (styleStr.Trim() == "")
                {
                    m_grid[currentCell.X, m_checkboxRowIndex].Value = false;
                    m_grid[currentCell.X, m_checkboxRowIndex].Style.BackColor = Color.White;
                }
                else
                {
                    m_grid[currentCell.X, m_checkboxRowIndex].Value = true;
                    string colorStr = m_grid[currentCell.X, m_colorRowIndex].Value.ToString();
                    m_grid[currentCell.X, m_checkboxRowIndex].Style.BackColor = PlotColors[colorStr];
                }
            }

            //m_grid.EndEdit();

            //var style = m_grid[currentCell.X, currentCell.Y].Style;
            //style.BackColor = G.PlotColors[colorStr];
            //sendingCB.ApplyCellStyleToEditingControl(style);
            //m_grid[currentCell.X, 0].Style.BackColor = G.PlotColors[colorStr];

            //sendingCB.BackColor = G.PlotColors[colorStr];
            //m_grid[currentCell.X, currentCell.Y].Style.BackColor = G.PlotColors[colorStr];
            //DataGridViewTextBoxCell cel = (DataGridViewTextBoxCell)m_grid.Rows[currentCell.Y].Cells[0];
            //cel.Value = sendingCB.EditingControlFormattedValue.ToString();
        }

        private void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("CELL VALUE CHANGED");
            if (e.RowIndex == m_checkboxRowIndex)
            {
                var cell = m_grid[e.ColumnIndex, e.RowIndex];
                var styleCell = m_grid[e.ColumnIndex, m_styleRowIndex];
                if ((bool)cell.Value == true && styleCell.Value.ToString().Trim() == "")
                    styleCell.Value = DefaultPlotLineStyle;

                CellValueChanged?.Invoke(this, new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
            }
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("CELL CONTENT CLICK");
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("CELL CLICK");
        }


        public void ScrollToTop()
        {
            if (m_grid.InvokeRequired) m_grid.Invoke(new Action(ScrollToTop));
            else
            {
                if (m_grid.RowCount > 0)
                    m_grid.FirstDisplayedScrollingRowIndex = 0;
            }
        }

        public void ScrollToBottom()
        {
            if (m_grid.InvokeRequired) m_grid.Invoke(new Action(ScrollToBottom));
            else
            {
                if (m_grid.RowCount > 0)
                    m_grid.FirstDisplayedScrollingRowIndex = m_grid.RowCount - 1;
            }
        }

        public void SetRows(List<string[]> rows)
        {
            m_grid.Rows.Clear();
            SetInteractiveRows();

            for (int ri = 0; ri < rows.Count; ++ri)
            {
                m_grid.Rows.Add(rows[ri]);
            }
        }

        public void WriteDataFrameFile(string pathname)
        {
            using (var writer = new StreamWriter(pathname))
            {
                // Column Headers (first row in file)
                var columnHeaders = m_grid.Columns
                    .Cast<DataGridViewColumn>()
                    .Select(c => c.HeaderText);
                writer.WriteLine(string.Join(",", columnHeaders));

                // Rows
                for (int i = m_interactiveRowCount; i < m_grid.Rows.Count; ++i)
                {
                    var cellValues = m_grid.Rows[i].Cells
                        .Cast<DataGridViewCell>()
                        .Select(cell => cell.Value);
                    writer.WriteLine(string.Join(",", cellValues));
                }
            }
        }

        public Dictionary<string, string> GetSelectedPlotColumns()
        {
            Dictionary<string, string> selected = new Dictionary<string, string>();
            for (int ci = 0; ci < m_grid.Columns.Count; ++ci)
            {
                string style = m_grid[ci, m_styleRowIndex].Value.ToString();
                char styleChar = style[0];
                char colorChar = m_grid[ci, m_colorRowIndex].Value.ToString()[0];
                if ((bool)m_grid[ci, m_checkboxRowIndex].Value == true && !char.IsWhiteSpace(styleChar))
                {
                    StringBuilder sb = new StringBuilder();
                    if (style.Contains("marker"))
                    {
                        sb.Append(colorChar);
                        sb.Append(styleChar);
                    }
                    else
                    {
                        sb.Append(styleChar);
                        sb.Append(colorChar);
                    }
                    selected.Add(m_grid.Columns[ci].HeaderText, sb.ToString());
                }
            }
            return selected;
        }



    } // end of CLASS
} // end of NAMESPACE
