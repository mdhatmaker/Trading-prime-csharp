using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;
using Tools;

namespace GuiTools.Grid
{
    public class BaseGrid
    {
        protected delegate void AddGridRowHandler(IDataRow row);
        protected delegate void UpdateGridRowHandler(IDataRow row);

        public DataGridView Grid { get { return m_grid; } }
        public int RowCount { get { return m_grid.Rows.Count; } }
        public int ColumnCount { get { return m_grid.Columns.Count; } }
        public DataGridViewColumnCollection Columns { get { return m_grid.Columns; } }

        protected DataGridView m_grid;
        protected Form m_form;

        protected int m_customizedRowCount = 0;                     // if there are customized rows at top of grid (like plot selection), indicate how many exist

        protected Dictionary<string, int> m_rowIndexes;             // given a Key, return an integer row number in the grid

        public BaseGrid(DataGridView grid, Form form)
        {
            m_grid = grid;
            m_form = form;

            m_rowIndexes = new Dictionary<string, int>();

            //Set Double buffering on the Grid using reflection and the bindingflags enum.
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, m_grid, new object[] { true });
        }

        public virtual void InitializeColumns(string[] columns, Color cellBackColor)
        {
            m_grid.Columns.Clear();
            for (int i = 0; i < columns.Length; ++i)
            {
                DataGridViewColumn col = new DataGridViewColumn();
                col.HeaderText = columns[i];
                col.Name = columns[i];
                DataGridViewCell cell = new DataGridViewTextBoxCell();

                cell.Style.BackColor = Color.White;

                if (columns[i].StartsWith("Ask"))
                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                else
                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                if (columns[i].StartsWith("Bid"))
                    cell.Style.ForeColor = Color.Blue;
                else if (columns[i].StartsWith("Ask"))
                    cell.Style.ForeColor = Color.Red;

                if (columns[i] == "Symbol")
                    col.MinimumWidth = 80;
                else
                    col.MinimumWidth = 35;

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

            m_grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            m_grid.MultiSelect = false;
        }

        public virtual void AddRow(IDataRow row)
        {
            try
            {                
                if (m_grid.InvokeRequired) m_form.Invoke(new AddGridRowHandler(AddRow), row);   // check if we need to invoke the delegate
                else
                {
                    // delegate not required, just add row to the grid
                    if (row.Key != null)
                    {
                        string[] cellValues = row.GetCells();
                        cellValues[0] = row.Key;
                        m_rowIndexes[row.Key] = m_grid.Rows.Count;
                        m_grid.Rows.Add(cellValues);
                    }
                }
            }
            catch (ObjectDisposedException) { }         // The object went away; ignore it since we're probably exiting
        }

        public virtual void UpdateRow(IDataRow row)
        {
            try
            {
                if (m_grid.InvokeRequired) m_form.Invoke(new UpdateGridRowHandler(UpdateRow), row); // check if we need to invoke the delegate
                else
                {
                    // delegate not required, just update the grid row
                    if (m_rowIndexes.ContainsKey(row.Key))
                    {
                        string[] cellValues = row.GetCells();
                        cellValues[0] = row.Key;
                        int ri = m_rowIndexes[row.Key];
                        DataGridViewRow gr = m_grid.Rows[ri];
                        gr.SetValues(cellValues);
                    }
                }
            }
            catch (ObjectDisposedException) { }         // The object went away; ignore it since we're probably exiting
        }

        public virtual void ClearRows()
        {
            m_grid.Rows.Clear();
        }

        public virtual void SetRows(List<string[]> rows)
        {
            ClearRows();
            for (int ri = 0; ri < rows.Count; ++ri)
            {
                m_grid.Rows.Add(rows[ri]);
            }
        }

        protected virtual void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("CELL VALUE CHANGED");
            /*if (e.RowIndex == m_checkboxRowIndex)
            {
                var cell = m_grid[e.ColumnIndex, e.RowIndex];
                var styleCell = m_grid[e.ColumnIndex, m_styleRowIndex];
                if ((bool)cell.Value == true && styleCell.Value.ToString().Trim() == "")
                    styleCell.Value = G.DefaultPlotLineStyle;
            }*/
        }

        protected virtual void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("CELL CONTENT CLICK");
        }

        protected virtual void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("CELL CLICK");
        }

        public virtual void ScrollToTop()
        {
            if (m_grid.RowCount > 0)
                m_grid.FirstDisplayedScrollingRowIndex = 0;
        }

        public virtual void ScrollToBottom()
        {
            if (m_grid.RowCount > 0)
                m_grid.FirstDisplayedScrollingRowIndex = m_grid.RowCount - 1;
        }

        public virtual void DeselectAllRows()
        {
            foreach (DataGridViewRow row in m_grid.SelectedRows)
            {
                row.Selected = false;
            }
        }

        public virtual void WriteDataFrameFile(string pathname)
        {
            using (var writer = new StreamWriter(pathname))
            {
                // Column Headers (first row in file)
                var columnHeaders = m_grid.Columns
                    .Cast<DataGridViewColumn>()
                    .Select(c => c.HeaderText);
                writer.WriteLine(string.Join(",", columnHeaders));

                // Rows
                for (int i = m_customizedRowCount; i < m_grid.Rows.Count; ++i)
                {
                    var cellValues = m_grid.Rows[i].Cells
                        .Cast<DataGridViewCell>()
                        .Select(cell => cell.Value);
                    writer.WriteLine(string.Join(",", cellValues));
                }
            }
        }

        // Given the full pathname of an output csv text file (probably want to use .CSV or .TXT extension)
        // Write the contents of this grid to the file
        // (optional) firstXcolumnsOnly can be used to write only the first (left-most) column, or first 2 columns or...
        //            firstXcolumnsOnly=0 (default) writes all columns
        public virtual void WriteCsvFile(string pathname, int firstXcolumnsOnly = 0)
        {
            // If there are any symbols for which we have subscribed to prices, save these to our symbols text file
            if (this.RowCount > 0)
            {
                using (var writer = new StreamWriter(pathname))
                {
                    int columnCount = this.ColumnCount;
                    if (firstXcolumnsOnly > 0)
                        columnCount = Math.Min(this.ColumnCount, firstXcolumnsOnly);
                    string[] cells = new string[columnCount];
                    for (int ri = 0; ri < this.RowCount; ++ri)
                    {
                        for (int ci = 0; ci < columnCount; ++ci)
                        {
                            cells[ci] = m_grid[ci, ri].Value.ToString();
                        }
                        //writer.WriteLine("{0},{1},{2},{3}", gridLevel1[0, ri].Value.ToString(), gridLevel1[1, ri].Value.ToString(), gridLevel1[2, ri].Value.ToString(), gridLevel1[3, ri].Value.ToString());
                        string outputLine = string.Join(",", cells);
                        writer.WriteLine(outputLine);
                    }
                }
            }
        }



    } // end of class
} // end of namespace
