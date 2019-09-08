using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Tools.G;

namespace GuiTools.Grid
{
    public class HeatMapGrid : BaseGrid
    {
        //public int RowCount { get { return m_grid.Rows.Count; } }
        public int ColCount { get { return m_grid.Columns.Count; } }

        private Dictionary<string, int> m_rowIndex = new Dictionary<string, int>();
        private Dictionary<string, int> m_colIndex = new Dictionary<string, int>();

        private string m_name;

        private string[] m_rowHeaders, m_colHeaders;
        private Color m_minColor, m_maxColor;

        public HeatMapGrid(DataGridView grid, Form form, string name = "HeatMapGrid") : base(grid, form)
        {
            m_name = name;

            Initialize();
        }

        public void UpdateCellValue(string rowHeader, string colHeader, decimal value)
        {
            int ri = m_rowIndex[rowHeader];
            int ci = m_colIndex[colHeader];
            UpdateCellValue(ri, ci, value);
        }

        public void UpdateCellValue(int ri, int ci, decimal value)
        {
            m_grid[ci, ri].Value = value;
            UpdateGridColors();
        }

        public void UpdateValue(string header, decimal value)
        {
            int ix = m_rowIndex[header];
            UpdateValue(ix, value);
        }

        public void UpdateValue(int ix, decimal value)
        {
            SetRowValue(ix, value);
            SetColValue(ix, value);

            for (int i = 0; i < RowCount; ++i)
            {
                m_grid[i, ix].Value = (decimal)m_grid.Rows[i].Tag - (decimal)m_grid.Columns[ix].Tag;
                m_grid[ix, i].Value = (decimal)m_grid.Rows[ix].Tag - (decimal)m_grid.Columns[i].Tag;
            }
            UpdateGridColors();
        }

        public void UpdateValueDelta(int ix, decimal value)
        {
            decimal existingValue = (decimal) m_grid.Rows[ix].Tag;
            SetRowValue(ix, existingValue + value);
            SetColValue(ix, existingValue + value);

            for (int i = 0; i < RowCount; ++i)
            {
                m_grid[i, ix].Value = (decimal)m_grid.Rows[i].Tag - (decimal)m_grid.Columns[ix].Tag;
                m_grid[ix, i].Value = (decimal)m_grid.Rows[ix].Tag - (decimal)m_grid.Columns[i].Tag;
            }
            UpdateGridColors();
        }

        public void SetRowValue(int ri, decimal value)
        {
            m_grid.Rows[ri].Tag = value;
        }

        public void SetColValue(int ci, decimal value)
        {
            m_grid.Columns[ci].Tag = value;
        }

        // Initialize the grid properties
        public void Initialize()
        {
            m_grid.AllowUserToAddRows = false;
            m_grid.AllowUserToDeleteRows = false;
            m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;            
            m_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            m_grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_grid.Location = new System.Drawing.Point(0, 0);
            m_grid.Name = "grid" + m_name;
            m_grid.ReadOnly = true;
            m_grid.RowTemplate.Height = 28;            
            m_grid.Size = new System.Drawing.Size(145, 45);
            m_grid.TabIndex = 23;
            m_grid.RowHeadersVisible = true;
        }

        // Given row header text (string  array) and column header text (string array) and the color representing each of minimum/maximum values 
        public void InitializeRowsAndColumns(string[] rowHeaders, string[] colHeaders, Color minColor, Color maxColor)
        {
            m_rowHeaders = rowHeaders;
            m_colHeaders = colHeaders;
            m_minColor = minColor;
            m_maxColor = maxColor;
            // Initialize the columns (using the InitializeColumns method of BaseGrid)
            for (int i = 0; i < colHeaders.Length; ++i)
            {
                colHeaders[i] = colHeaders[i].Substring(0, 4);      // cut off column header string at 4 chars
                rowHeaders[i] = colHeaders[i];
                m_colIndex[colHeaders[i]] = i;
            }
            this.InitializeColumns(colHeaders, Color.White);
            // Initialize the rows
            var rnd = new Random();            
            var rowValues = new object[colHeaders.Length];
            for (int i = 0; i < rowValues.Length; ++i)
            {
                rowValues[i] = (decimal) 10300;   // (decimal) rnd.Next();
            }
            for (int i = 0; i < rowHeaders.Length; ++i)
            {
                int ri = m_grid.Rows.Add(rowValues);
                //int ri = m_grid.Rows.Add("1.2", "3.4", "1.6", "2.0", "0.8", "1.3", "2.4", "8.9", "5.4");
                m_grid.Rows[ri].HeaderCell.Value = rowHeaders[i];
                m_rowIndex[rowHeaders[i]] = ri;
                m_grid.Rows[ri].Height = m_grid.Columns[0].Width;
            }

            UpdateGridColors();
        }

        public override void InitializeColumns(string[] columns, Color cellBackColor)
        {
            m_grid.Columns.Clear();
            for (int i = 0; i < columns.Length; ++i)
            {
                DataGridViewColumn col = new DataGridViewColumn();
                col.HeaderText = columns[i];
                col.Name = columns[i];
                DataGridViewCell cell = new DataGridViewTextBoxCell();

                cell.Style.BackColor = Color.White;
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.MinimumWidth = 35;

                col.CellTemplate = cell;
                m_grid.Columns.Add(col);
            }
            m_grid.RowHeadersVisible = true;
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

        public void UpdateGridColors()
        {
            decimal min, max;
            GetMinMaxValues(out min, out max);
            for (int ri = 0; ri < RowCount; ++ri)
            {
                for (int ci = 0; ci < ColCount; ++ci)
                {
                    m_grid[ci, ri].Style.BackColor = GetHeatMapColor((decimal) m_grid[ci, ri].Value, min, max);
                }
            }
        }

        // Get the minimum and maximum values in the grid
        private void GetMinMaxValues(out decimal min, out decimal max)
        {
            min = decimal.MaxValue;
            max = decimal.MinValue;
            for (int ri = 0; ri < RowCount; ++ri)
            {
                for (int ci = 0; ci < ColCount; ++ci)
                {
                    var value = (decimal) m_grid[ci, ri].Value;
                    if (value < min) min = value;
                    if (value > max) max = value;
                }
            }
        }

        private Color GetHeatMapColor(decimal value, decimal min, decimal max)
        {
            if (value < 0)
            {
                return HeatMapColor(Math.Abs(value), 0, Math.Abs(min), Color.DarkRed, Color.LightCoral);
            }
            else if (value > 0)
            {
                return HeatMapColor(value, 0, max, Color.DarkGreen, Color.LightGreen);
            }
            else
                return Color.Gray;
        }

        // Given a value, a minimum value, and a maximum value
        // Return the appropriate color
        private Color HeatMapColor(decimal value, decimal min, decimal max, Color cmin, Color cmax)
        {
            //Color m_minColor = Color.RoyalBlue;
            //Color m_maxColor = Color.LightSkyBlue;

            // Example: Take the RGB
            //135-206-250 // Light Sky Blue
            // 65-105-225 // Royal Blue
            // 70-101-25 // Delta

            int rOffset = Math.Max(cmin.R, cmax.R);
            int gOffset = Math.Max(cmin.G, cmax.G);
            int bOffset = Math.Max(cmin.B, cmax.B);

            int deltaR = Math.Abs(cmin.R - cmax.R);
            int deltaG = Math.Abs(cmin.G - cmax.G);
            int deltaB = Math.Abs(cmin.B - cmax.B);

            if (max == min)                 // avoid division by zero error
                return Color.White;
            else
            {

                decimal val = (value - min) / (max - min);
                int r = rOffset - Convert.ToByte(deltaR * (1 - val));
                int g = gOffset - Convert.ToByte(deltaG * (1 - val));
                int b = bOffset - Convert.ToByte(deltaB * (1 - val));

                return Color.FromArgb(255, r, g, b);
            }
        }

    } // end of class HeatMapGrid

} // end of namespace
