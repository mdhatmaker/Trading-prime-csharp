using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace GuiTools
{
    public class RichTextBoxConsole : TextWriter
    {
        RichTextBox output = null; // RichTextBox used to show Console's output.

        /// <summary>
        /// Custom RichTextBox-Class used to print the Console output.
        /// </summary>
        /// <param name="_output">RichTextBox used to show Console's output.</param>
        public RichTextBoxConsole(RichTextBox _output)
        {
            output = _output;
            output.ScrollBars = RichTextBoxScrollBars.Both;
            output.WordWrap = true;
        }

        // <summary>
        // Appends text to the richtextbox and to the logfile
        // </summary>
        // <param name="value">Input-string which is appended to the textbox.</param>
        public override void Write(char value)
        {
            base.Write(value);
            if (output.InvokeRequired)
            {
                output.Invoke(new Action<char>(Write), new object[] { value });
            }
            else
            {
                string txt = value.ToString().Replace('\r', ' ');
                output.AppendText(txt);    //Append char to the richtextbox (ignore '/r' to avoid unnecessary line feed)
                //if (txt.Contains("\n")) output.ScrollToBottom();
            }
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }

    } // end of class
} // end of namespace
