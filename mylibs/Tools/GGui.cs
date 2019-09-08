using System;
using System.Windows.Forms;

namespace Tools
{
    public static class GGui
    {
        public static Form GetParentForm(Control ctrl)
        {
            //Control ctrl = parentPanel;
            while (!(ctrl is Form))
            {
                ctrl = ctrl.Parent;
            }
            return ctrl as Form;
        }



    } // end of class
} // end of namespace
