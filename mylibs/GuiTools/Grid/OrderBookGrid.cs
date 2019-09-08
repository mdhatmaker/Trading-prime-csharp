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
    public class OrderBookGrid
    {
        //private delegate void AddGridRowHandler(IDataRow row);
        //private delegate void UpdateGridRowHandler(IDataRow row);

        public DataGridView Grid { get { return m_grid; } }
        public int RowCount { get { return m_grid.Rows.Count; } }
        public int ColumnCount { get { return m_grid.Columns.Count; } }
        public DataGridViewColumnCollection Columns { get { return m_grid.Columns; } }

        private DataGridView m_grid;
        private Form m_form;

        private Dictionary<string, int> m_rowIndexes;                       // given a Key, return an integer row number in the grid

        public OrderBookGrid(DataGridView grid, Form form=null)
        {
            m_grid = grid;
            m_form = form;

            m_rowIndexes = new Dictionary<string, int>();

            //Set Double buffering on the Grid using reflection and the bindingflags enum.
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, m_grid, new object[] { true });

            // TODO: Adjust this so the blue/red of bid/ask is maintained
            // Get rid of the "blue square" which (by default) highlights the selected cell
            m_grid.DefaultCellStyle.SelectionBackColor = m_grid.DefaultCellStyle.BackColor;
            m_grid.DefaultCellStyle.SelectionForeColor = m_grid.DefaultCellStyle.ForeColor;
        }

        public void UpdatePrices(SortedDictionary<double, IDataRow> updateDict)
        {
            //ClearRows();

            int ix = 0;
            foreach (var key in updateDict.Keys.Reverse())
            {
                if (m_grid.Rows.Count > ix)
                    UpdateRowAtIndex(ix, updateDict[key]);
                else
                    AddRow(updateDict[key]);
                ++ix;
                /*bool found = false;
                foreach (DataGridViewRow row in m_grid.Rows)
                {
                    if (row.Cells[0].Value.ToString() == key)
                    {
                        string[] cellValues = updateDict[key].GetCells();
                        cellValues[0] = key;
                        row.SetValues(cellValues);
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                string[] cellValues = updateDict[key].GetCells();
                    cellValues[0] = key;
                    AddRow(cellValues);
                }*/
            }
            // Clear any remaining rows
            for (int i = ix; i < m_grid.Rows.Count; ++i)
            {
                UpdateRowAtIndex(i, ZOrderBookRow.Empty);
            }
        }

        /*public void UpdatePrices(List<IDataRow> updateList)
        {
            foreach (var item in updateList)
            {
                bool found = false;
                foreach (DataGridViewRow row in m_grid.Rows)
                {
                    if (row.Cells[0].Value.ToString() == item.Key)
                    {
                        string[] cellValues = item.GetCells();
                        row.SetValues(cellValues);
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    string[] cellValues = item.GetCells();
                    m_grid.Rows.Add(cellValues);
                }
            }
        }*/

        public void AddRow(IDataRow row)
        {
            try
            {
                // check if we need to invoke the delegate
                if (m_grid.InvokeRequired)
                {
                    // call this function again using a delegate
                    //m_form.Invoke(new AddGridRowHandler(AddRow), row);
                    m_form.Invoke(new Action<IDataRow>(AddRow), row);
                }
                else
                {
                    // delegate not required, just add row to the grid
                    string[] cellValues = row.GetCells();
                    m_rowIndexes[row.Key] = m_grid.Rows.Count;
                    m_grid.Rows.Add(cellValues);
                }
            }
            catch (ObjectDisposedException)
            {
                // The list view object went away, ignore it since we're probably exiting.
            }
        }

        public void UpdateRowAtIndex(int ix, IDataRow row)
        {
            try
            {
                // check if we need to invoke the delegate
                if (m_grid.InvokeRequired)
                {
                    // call this function again using a delegate
                    m_form.Invoke(new Action<int, IDataRow>(UpdateRowAtIndex), ix, row);
                }
                else
                {
                    // delegate not required, just update the grid row
                    string[] cellValues = row.GetCells();
                    DataGridViewRow gr = m_grid.Rows[ix];
                    gr.SetValues(cellValues);
                }
            }
            catch (ObjectDisposedException)
            {
                // The list view object went away, ignore it since we're probably exiting.
            }
        }

        public void InitializeColumns(string[] columns, Color cellBackColor)
        {
            m_grid.Columns.Clear();
            for (int i = 0; i < columns.Length; ++i)
            {
                DataGridViewColumn col = new DataGridViewColumn();
                col.HeaderText = columns[i];
                col.Name = columns[i];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                if (columns[i] == "Price")
                    col.FillWeight = 26;
                else
                    col.FillWeight = 37;

                DataGridViewCell cell = new DataGridViewTextBoxCell();

                cell.Style.BackColor = Color.White;

                /*if (columns[i].StartsWith("Ask"))
                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                else
                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;*/
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                if (columns[i].StartsWith("Bid"))
                    cell.Style.ForeColor = Color.Blue;
                else if (columns[i].StartsWith("Ask"))
                    cell.Style.ForeColor = Color.Red;
                

                if (columns[i] == "Symbol")
                    col.MinimumWidth = 65;
                else if (columns[i] == "Last" || columns[i] == "Volume")
                    col.MinimumWidth = 25;

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


        private void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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
            if (m_grid.RowCount > 0)
                m_grid.FirstDisplayedScrollingRowIndex = 0;
        }

        public void ScrollToBottom()
        {
            if (m_grid.RowCount > 0)
                m_grid.FirstDisplayedScrollingRowIndex = m_grid.RowCount - 1;
        }

        public void ClearRows()
        {
            try
            {
                // check if we need to invoke the delegate
                if (m_grid.InvokeRequired)
                {
                    // call this function again using a delegate
                    m_form.Invoke(new Action(ClearRows));
                }
                else
                {
                    // delegate not required, just clear the rows in the grid
                    m_grid.Rows.Clear();
                }
            }
            catch (ObjectDisposedException)
            {
                // The list view object went away, ignore it since we're probably exiting.
            }
        }

        public void AddRow(string[] rowItems)
        {
            AddRow(rowItems.ToList());
        }

        public void AddRow(List<string> rowItems)
        {
            int ri = m_grid.Rows.Add();
            for (int ci = 0; ci < rowItems.Count; ++ci)
            {
                m_grid[ci, ri].Value = rowItems[ci];
            }
        }

        public void SetRows(List<string[]> rows)
        {
            ClearRows();
            for (int ri = 0; ri < rows.Count; ++ri)
            {
                m_grid.Rows.Add(rows[ri]);
            }
        }

        public void DeselectAllRows()
        {
            foreach (DataGridViewRow row in m_grid.SelectedRows)
            {
                row.Selected = false;
            }
        }

        public void WriteDataFrameFile(string pathname)
        {
            /*using (var writer = new StreamWriter(pathname))
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
            }*/
        }



    } // end of CLASS
} // end of NAMESPACE
