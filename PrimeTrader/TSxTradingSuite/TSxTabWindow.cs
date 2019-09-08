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
    public partial class TSxTabWindow : Form
    {
        private Control m_control = null;
        private Form m_originalHost = null;

        public TSxTabWindow()
        {
            InitializeComponent();
            label1.Text = "ONECHAIN CAPITAL \\ Trade";

            button6.Left = this.Width - button6.Width - 18;
            button6.Top = 3;
            button6.FlatStyle = FlatStyle.Flat;
            button6.FlatAppearance.BorderSize = 0;
            button6.BackColor = TSx.m_buttonBackColorHighlight;
            button6.ForeColor = TSx.m_buttonForeColor;
            button6.FlatAppearance.BorderColor = TSx.m_buttonForeColor;
            button6.FlatAppearance.MouseOverBackColor = TSx.m_buttonBackColor;

            this.ControlBox = false;
            this.Text = String.Empty;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.Width = TSx.m_width_new;
            this.Height = TSx.m_height_new;
        }
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        ///
        /// Handling the window messages
        ///
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        public void setActiveControl(Form f, string name, Control c)
        {
            m_originalHost = f;
            m_control = c;
            m_control.Name = name;
            m_control.Top = 50;
            m_control.Left = (this.Width / 2) - (m_control.Width / 2);
            this.Controls.Add(m_control);
        }

        private void TSxTabWindow_Resize(object sender, EventArgs e)
        {

            button6.Left = this.Width - button6.Width - 18;
            if (m_control != null)
            {
                m_control.Left = (this.Width / 2) - (m_control.Width / 2);
                if (m_control.Left < 10)
                    m_control.Left = 10;
            }

        }
 

 
        private void button6_Click_1(object sender, EventArgs e)
        {
            (m_originalHost as TSxTradingSuiteMain).ReTabify(m_control);
            this.Hide();
        }


    }
}
