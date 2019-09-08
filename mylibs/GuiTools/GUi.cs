using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuiTools
{
    public static class GUi
    {
        public static Form GetParentForm(Control child)
        {
            Control ctrl = child;
            while (!(ctrl is Form))
            {
                ctrl = ctrl.Parent;
            }
            return ctrl as Form;
        }

        public static void SetLabelText(Form frm, Label lbl, string text)
        {
            if (lbl.InvokeRequired) frm.Invoke(new Action<Form, Label, string>(SetLabelText), frm, lbl, text);
            else lbl.Text = text;
        }

        public static void SetLabelColor(Form frm, Label lbl, Color color)
        {
            if (lbl.InvokeRequired) frm.Invoke(new Action<Form, Label, Color>(SetLabelColor), frm, lbl, color);
            else lbl.ForeColor = color;
        }


    } // end of class
} // end of namespace
