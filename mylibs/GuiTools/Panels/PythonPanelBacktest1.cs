using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuiTools.Panels
{
    public class PythonPanelBacktest1 : System.Windows.Forms.Panel
    {
        public void Initialize()
        {
            TableLayoutPanel tblPanel = new TableLayoutPanel();
            tblPanel.Location = new System.Drawing.Point(5, 5);
            tblPanel.Name = "PythonArgs1";
            tblPanel.Size = new System.Drawing.Size(300, 200);
            tblPanel.TabIndex = 0;
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            Label lbl1 = new Label();
            lbl1.Text = "label1";
            lbl1.Anchor = AnchorStyles.Left & AnchorStyles.Top & AnchorStyles.Right;
            tblPanel.Location = new System.Drawing.Point(5, 5);
            tblPanel.Name = "PythonArgs1";
            tblPanel.Size = new System.Drawing.Size(100, 30);

            TextBox txt1 = new TextBox();
            txt1.Text = "";
            txt1.Anchor = AnchorStyles.Left & AnchorStyles.Top & AnchorStyles.Right;

            tblPanel.Controls.Add(lbl1, 0, 0);                                      // NOTE: specified as (control, COLUMN, row) instead of typical row-first
        }
    }
} // end of namespace
