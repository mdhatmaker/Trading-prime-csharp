using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeTrader
{
    public partial class RunStudyForm : Form
    {
        public int ColumnsRequired {
            get { return m_columnsRequired; }
            set
            {
                m_columnsRequired = value;
                lblSelectXColumns.Text = string.Format("Choose {0} Columns:", m_columnsRequired);
                listColumnsForStudy.SelectionMode = SelectionMode.MultiExtended;
            }
        }

        public int ParamsRequired
        {
            get { return m_paramsRequired; }
            /*set
            {
                m_paramsRequired = value;
                ShowParams(m_paramsRequired);
            }*/
        }

        public string[] ColumnsForStudy
        {
            get { return listColumnsForStudy.SelectedItems.Cast<string>().ToArray(); }
        }

        private int m_columnsRequired;
        private int m_paramsRequired;

        public RunStudyForm()
        {
            InitializeComponent();

        }

        public string[] GetParams()
        {
            List<string> pAll = new List<string>();
            pAll.Add(txtParam1.Text);
            pAll.Add(txtParam2.Text);
            pAll.Add(txtParam3.Text);
            pAll.Add(txtParam4.Text);
            pAll.Add(txtParam5.Text);
            return pAll.Take(m_paramsRequired).ToArray();
        }

        public void SetSelectedColumns(List<string> selectedColumns)
        {
            listColumnsForStudy.Items.Clear();
            foreach (string key in selectedColumns)
            {
                listColumnsForStudy.Items.Add(key);
            }

            btnGoStudy.Enabled = false;                 // disable the Go button until user has selected the correct number of columns
        }

        public void SetParamNames(string[] paramNames)
        {
            m_paramsRequired = paramNames.Length;
            for (int i = 0; i < paramNames.Length; ++i)
            {
                if (i == 0)
                    lblParam1.Text = paramNames[i];
                else if (i == 1)
                    lblParam2.Text = paramNames[i];
                else if (i == 2)
                    lblParam3.Text = paramNames[i];
                else if (i == 3)
                    lblParam4.Text = paramNames[i];
                else if (i == 4)
                    lblParam5.Text = paramNames[i];
            }
            ShowParams(m_paramsRequired);
        }

        private void ShowParams(int count)
        {
            SetParamsVisible(false);        // hide all params
            SetParamsVisible(true, count);      // show first <count> params
        }

        private void SetParamsVisible(bool b, int count=int.MaxValue)
        {
            if (count >= 1)
            {
                lblParam1.Visible = b;
                txtParam1.Visible = b;
            }
            if (count >= 2)
            {
                lblParam2.Visible = b;
                txtParam2.Visible = b;
            }
            if (count >= 3)
            {
                lblParam3.Visible = b;
                txtParam3.Visible = b;
            }
            if (count >= 4)
            {
                lblParam4.Visible = b;
                txtParam4.Visible = b;
            }
            if (count >= 5)
            {
                lblParam5.Visible = b;
                txtParam5.Visible = b;
            }
        }

        private void listColumnsForStudy_SelectedValueChanged(object sender, EventArgs e)
        {
            List<string> selectedItems = listColumnsForStudy.SelectedItems.Cast<string>().ToList();
            if (selectedItems.Count == m_columnsRequired)
            {
                btnGoStudy.Enabled = true;
                btnGoStudy.BackColor = Color.LightGreen;
                //listColumnsForStudy.BackColor = Color.LightGreen;
            }
            else
            {
                btnGoStudy.Enabled = false;
                btnGoStudy.BackColor = SystemColors.Control;
                //listColumnsForStudy.BackColor = Color.White;
            }
        }

    } // end of class
} // end of namespace
