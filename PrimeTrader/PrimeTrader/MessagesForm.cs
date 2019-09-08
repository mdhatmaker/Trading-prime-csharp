using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace PrimeTrader
{
    public partial class MessagesForm : Form
    {
        public MessagesForm()
        {
            InitializeComponent();
        }

        private void MessagesForm_Load(object sender, EventArgs e)
        {
            G.COutput += G_COutput;
            G.DOutput += G_DOutput;
            G.ErrorOutput += G_ErrorOutput;

        }

        // TODO: Limit size of output lists so as to not overrun memory

        private void G_COutput(MessageArgs e)
        {
            AppendText(rtbConsoleOutput, e.Text);
        }

        private void G_DOutput(MessageArgs e)
        {
            AppendText(rtbDebugOutput, e.Text);
        }

        private void G_ErrorOutput(MessageArgs e)
        {
            AppendText(rtbErrorOutput, e.Text);
        }

        private void AppendText(RichTextBox rtb, string text)
        {
            if (rtb.InvokeRequired)
            {
                this.Invoke(new Action<RichTextBox, string>(AppendText), rtb, text);
            }
            else
            {
                const int MAX_TEXT_LENGTH = 65536;
                if (rtb.TextLength > MAX_TEXT_LENGTH)
                {
                    rtb.Clear();
                }
                rtb.AppendText(text + "\n");
            }
        }

        private void MessagesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        /// <summary>
        /// For efficiency, we limit the list items to the last 500 displayed.
        /// </summary>
        /*private void LimitListItems()
        {
            try
            {
                // check if we need a delegate for this call
                if (lstData.InvokeRequired)
                {
                    this.Invoke(new UpdateControlsHandler(LimitListItems));
                }
                else
                {
                    while (lstData.Items.Count > 500)
                    {
                        lstData.Items.RemoveAt(500);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // the listbox went away.  Ingore it since we are probably exiting
            }
        }*/


    } // end of class

} // end of namespace
