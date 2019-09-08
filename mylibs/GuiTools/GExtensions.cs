using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using static Tools.G;

namespace GuiTools
{
    public static class GExtensions
    {
        public static void LoadCsv(this DataGridView grid, string pathname)
        {
            grid.Rows.Clear();
            grid.Columns.Clear();

            var f = File.OpenText(pathname);

            string line = f.ReadLine();
            string[] cols = line.Split(new char[] { ',' });
            foreach (string col in cols)
            {
                grid.Columns.Add(col, col);
            }

            int ri = 0;
            line = f.ReadLine();
            while (line != null)
            {
                grid.Rows.Add();
                cols = line.Split(new char[] { ',' });
                for (int ci = 0; ci < cols.Length; ++ci)
                {
                    grid[ci, ri].Value = cols[ci];
                }
                ++ri;
                line = f.ReadLine();
            }

            f.Close();
        }

        public static void ShowInFront(this Form form)
        {
            form.Show();
            form.WindowState = FormWindowState.Normal;
            form.BringToFront();
        }

        public static void ScrollToBottom(this RichTextBox rtb)
        {
            rtb.SelectionStart = rtb.Text.Length;
            rtb.ScrollToCaret();
        }

        public static void SelectTextItem(this ListBox lst, string text)
        {
            int ixFound = -1;
            for (int i = 0; i < lst.Items.Count; ++i)
            {
                if (lst.Items[i].ToString() == text)
                {
                    ixFound = i;
                    break;
                }
            }
            lst.SelectedIndex = ixFound;        // if text is not found, SelectedIndex will be set to -1 (no selection)
        }

        public static void PopulateFromFile(this ComboBox cbo, string pathname, int initialSelected=-1)
        {
            cbo.Items.Clear();
            using (var f = new StreamReader(pathname))
            {
                while (true)
                {
                    string line = f.ReadLine();
                    if (line == null) break;

                    cbo.Items.Add(line);
                }
            }
            if ((initialSelected >= 0) && (initialSelected < cbo.Items.Count))      // optional argument to select an index in the ComboBox
                cbo.SelectedIndex = initialSelected;
        }

        public static void PopulateFromList(this ComboBox cbo, IEnumerable list, int initialSelected = -1)
        {
            cbo.Items.Clear();
            foreach (var x in list)
            {
                cbo.Items.Add(x);
            }
            if ((initialSelected >= 0) && (initialSelected < cbo.Items.Count))      // optional argument to select an index in the ComboBox
                cbo.SelectedIndex = initialSelected;
        }

    } // end of class
} // end of namespace